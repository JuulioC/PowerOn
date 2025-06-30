using System.ComponentModel.DataAnnotations;

// O namespace deve corresponder à pasta onde o arquivo está.
namespace PowerOn.Models
{
    // Esta classe representa os dados do formulário de login.
    public class LoginViewModel
    {
        // Propriedade para o campo de Email.
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Por favor, insira um endereço de email válido.")]
        public string Email { get; set; }

        // Propriedade para o campo de Senha (que estava faltando).
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
