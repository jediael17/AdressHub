using AddressManager.Data;
using AddressManager.Models;
using AddressManager.Services;
using AddressManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AddressManager.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ViaCepService _viaCep;

        public AddressController(AppDbContext db, ViaCepService viaCep)
        {
            _db = db;
            _viaCep = viaCep;
        }

        private int UsuarioId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // ─── LIST ──────────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var enderecos = await _db.Enderecos
                .AsNoTracking()
                .Where(e => e.UsuarioId == UsuarioId)
                .OrderByDescending(e => e.DataCriacao)
                .ToListAsync();

            return View(enderecos);
        }

        // ─── CREATE ────────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Create() => View(new EnderecoViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnderecoViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var endereco = MapToEntity(vm);
            endereco.UsuarioId = UsuarioId;

            _db.Enderecos.Add(endereco);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Endereço adicionado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // ─── EDIT ──────────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var endereco = await GetEnderecoDoUsuario(id);
            if (endereco == null) return NotFound();

            return View(MapToViewModel(endereco));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EnderecoViewModel vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            var endereco = await GetEnderecoDoUsuario(id);
            if (endereco == null) return NotFound();

            endereco.Cep = vm.Cep;
            endereco.Logradouro = vm.Logradouro;
            endereco.Complemento = vm.Complemento;
            endereco.Bairro = vm.Bairro;
            endereco.Cidade = vm.Cidade;
            endereco.Uf = vm.Uf;
            endereco.Numero = vm.Numero;
            endereco.DataAtualizacao = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Endereço atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // ─── DELETE ────────────────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var endereco = await GetEnderecoDoUsuario(id);
            if (endereco == null) return NotFound();

            _db.Enderecos.Remove(endereco);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Endereço removido com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // ─── VIA CEP API ───────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> BuscarCep(string cep)
        {
            var resultado = await _viaCep.BuscarCepAsync(cep);
            if (resultado == null)
                return NotFound(new { message = "CEP não encontrado." });

            return Ok(new
            {
                cep = resultado.Cep.Replace("-", ""),
                logradouro = resultado.Logradouro,
                complemento = resultado.Complemento,
                bairro = resultado.Bairro,
                cidade = resultado.Localidade,
                uf = resultado.Uf
            });
        }

        // ─── CSV EXPORT ────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> ExportarCsv()
        {
            var enderecos = await _db.Enderecos
                .AsNoTracking()
                .Where(e => e.UsuarioId == UsuarioId)
                .OrderBy(e => e.Cidade)
                .ThenBy(e => e.Logradouro)
                .ToListAsync();

            if (!enderecos.Any())
            {
                TempData["Info"] = "Nenhum endereço para exportar.";
                return RedirectToAction(nameof(Index));
            }

            var bytes = CsvExportService.ExportarEnderecos(enderecos);
            var fileName = $"enderecos_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            return File(bytes, "text/csv; charset=utf-8", fileName);
        }

        // ─── HELPERS ───────────────────────────────────────────────────────────

        private async Task<Endereco?> GetEnderecoDoUsuario(int id) =>
            await _db.Enderecos.FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == UsuarioId);

        private static Endereco MapToEntity(EnderecoViewModel vm) => new()
        {
            Id = vm.Id,
            Cep = vm.Cep,
            Logradouro = vm.Logradouro,
            Complemento = vm.Complemento,
            Bairro = vm.Bairro,
            Cidade = vm.Cidade,
            Uf = vm.Uf,
            Numero = vm.Numero
        };

        private static EnderecoViewModel MapToViewModel(Endereco e) => new()
        {
            Id = e.Id,
            Cep = e.Cep,
            Logradouro = e.Logradouro,
            Complemento = e.Complemento,
            Bairro = e.Bairro,
            Cidade = e.Cidade,
            Uf = e.Uf,
            Numero = e.Numero
        };
    }
}
