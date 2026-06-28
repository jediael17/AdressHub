using System.ComponentModel.DataAnnotations;

namespace AddressManager.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Usuário é obrigatório")]
        [Display(Name = "Usuário")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;
    }

    public class EnderecoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "CEP é obrigatório")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "CEP deve conter 8 dígitos numéricos")]
        [Display(Name = "CEP")]
        public string Cep { get; set; } = string.Empty;

        [Required(ErrorMessage = "Logradouro é obrigatório")]
        [MaxLength(200, ErrorMessage = "Máximo 200 caracteres")]
        [Display(Name = "Logradouro")]
        public string Logradouro { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "Bairro é obrigatório")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Bairro")]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cidade é obrigatória")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "UF é obrigatória")]
        [MaxLength(2, ErrorMessage = "Máximo 2 caracteres")]
        [Display(Name = "UF")]
        public string Uf { get; set; } = string.Empty;

        [Required(ErrorMessage = "Número é obrigatório")]
        [MaxLength(20, ErrorMessage = "Máximo 20 caracteres")]
        [Display(Name = "Número")]
        public string Numero { get; set; } = string.Empty;
    }

    public class ViaCepResponse
    {
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Localidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public bool Erro { get; set; }
    }
}
