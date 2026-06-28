using System.Security.Cryptography;
using System.Text;
using AddressManager.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace AddressManager.Services
{
    /// <summary>
    /// Serviço para operações de segurança (hash de senha).
    /// </summary>
    public static class SecurityService
    {
        /// <summary>
        /// Gera hash SHA-256 em Base64 da senha fornecida.
        /// </summary>
        public static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Verifica se a senha fornecida corresponde ao hash armazenado.
        /// </summary>
        public static bool VerifyPassword(string password, string hash)
            => HashPassword(password) == hash;
    }

    /// <summary>
    /// Serviço para integração com a API ViaCEP.
    /// </summary>
    public class ViaCepService
    {
        private readonly HttpClient _http;
        private readonly ILogger<ViaCepService> _logger;

        public ViaCepService(HttpClient http, ILogger<ViaCepService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<ViewModels.ViaCepResponse?> BuscarCepAsync(string cep)
        {
            try
            {
                var cleanCep = new string(cep.Where(char.IsDigit).ToArray());
                if (cleanCep.Length != 8) return null;

                var response = await _http.GetFromJsonAsync<ViewModels.ViaCepResponse>(
                    $"https://viacep.com.br/ws/{cleanCep}/json/"
                );

                return response?.Erro == true ? null : response;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Erro ao consultar ViaCEP para o CEP {Cep}", cep);
                return null;
            }
        }
    }

    /// <summary>
    /// Serviço para exportação de endereços em formato CSV.
    /// </summary>
    public static class CsvExportService
    {
        public static byte[] ExportarEnderecos(IEnumerable<Endereco> enderecos)
        {
            using var ms = new MemoryStream();
            using var writer = new StreamWriter(ms, Encoding.UTF8);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            });

            csv.WriteHeader<EnderecoExportDto>();
            csv.NextRecord();

            foreach (var e in enderecos)
            {
                csv.WriteRecord(new EnderecoExportDto
                {
                    CEP = e.Cep,
                    Logradouro = e.Logradouro,
                    Numero = e.Numero,
                    Complemento = e.Complemento ?? "",
                    Bairro = e.Bairro,
                    Cidade = e.Cidade,
                    UF = e.Uf
                });
                csv.NextRecord();
            }

            writer.Flush();
            return ms.ToArray();
        }

        private sealed class EnderecoExportDto
        {
            public string CEP { get; set; } = "";
            public string Logradouro { get; set; } = "";
            public string Numero { get; set; } = "";
            public string Complemento { get; set; } = "";
            public string Bairro { get; set; } = "";
            public string Cidade { get; set; } = "";
            public string UF { get; set; } = "";
        }
    }
}
