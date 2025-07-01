using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Necessário para IFormFile

namespace PowerOn.Models
{
    public class EditProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(255, ErrorMessage = "O Nome não pode exceder 255 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Por favor, insira um endereço de email válido.")]
        [StringLength(255, ErrorMessage = "O Email não pode exceder 255 caracteres.")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "A Senha deve ter entre 6 e 100 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "A Nova Senha e a Confirmação de Senha não coincidem.")]
        [Display(Name = "Confirmar Nova Senha")]
        public string? ConfirmNewPassword { get; set; }

        [Display(Name = "Perfil")]
        public int? Perfil { get; set; }

        [Display(Name = "Empresa")]
        public int? EmpresaId { get; set; }

        [StringLength(255, ErrorMessage = "O Código do Sistema não pode exceder 255 caracteres.")]
        [Display(Name = "Código do Sistema")]
        public string? CodigoSistema { get; set; }

        public string? ImgPerfilBase64 { get; set; }

        // *** MUDANÇA AQUI: Adicionada a propriedade ImgPerfilMimeType ***
        public string? ImgPerfilMimeType { get; set; }

        [Display(Name = "Nova Imagem de Perfil")]
        public IFormFile? NewProfileImageFile { get; set; }
    }
}
