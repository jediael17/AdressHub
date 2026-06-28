using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressManager.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Usuario1 { get; set; } = string.Empty;

        [Required, MaxLength(256)]
        public string Senha { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
    }

    [Table("Enderecos")]
    public class Endereco
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(8)]
        public string Cep { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Logradouro { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Complemento { get; set; }

        [Required, MaxLength(100)]
        public string Bairro { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Cidade { get; set; } = string.Empty;

        [Required, MaxLength(2)]
        public string Uf { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Numero { get; set; } = string.Empty;

        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
    }
}
