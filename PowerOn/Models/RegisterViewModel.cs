using System.ComponentModel.DataAnnotations;

namespace PowerOn.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(255, ErrorMessage = "O Nome não pode exceder 255 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Por favor, insira um endereço de email válido.")]
        [StringLength(255, ErrorMessage = "O Email não pode exceder 255 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A Senha deve ter entre 6 e 100 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo Confirmação de Senha é obrigatório.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "A Senha e a Confirmação de Senha não coincidem.")]
        [Display(Name = "Confirmar Senha")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Perfil")]
        public int? Perfil { get; set; }

        [Display(Name = "Empresa")]
        public int? EmpresaId { get; set; }

        [StringLength(255, ErrorMessage = "O Código do Sistema não pode exceder 255 caracteres.")]
        [Display(Name = "Código do Sistema")]
        public string? CodigoSistema { get; set; }
    }
}
