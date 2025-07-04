using System.ComponentModel.DataAnnotations;

namespace PowerOn.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [StringLength(100, ErrorMessage = "A {0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
        public string ConfirmNewPassword { get; set; }
    }
}