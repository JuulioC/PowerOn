using Microsoft.AspNetCore.Mvc;
using PowerOn.Models; // Namespace do seu ViewModel

namespace PowerOn.Controllers
{
    public class LoginController : Controller
    {
        // GET: /Login
        // Esta action exibe a página de login
        [HttpGet]
        public IActionResult Index()
        {
            // Retorna a View "Index.cshtml" que deve estar dentro da pasta "Views/Login"
            return View();
        }

        // POST: /Login
        // Esta action recebe os dados do formulário de login
        [HttpPost]
        [ValidateAntiForgeryToken] // Proteção contra ataques CSRF
        public IActionResult Index(LoginViewModel model)
        {
            // Verifica se o modelo de dados (usuário, senha) é válido
            if (ModelState.IsValid)
            {
                // --- LÓGICA DE AUTENTICAÇÃO ---
                // Aqui você deve verificar se o model.Email e model.Senha são válidos.
                // Exemplo: consultar um banco de dados, chamar um serviço de identidade, etc.

                // Exemplo de verificação simples (substitua pela sua lógica real):
                if (model.Email == "usuario@poweron.com" && model.Senha == "senha123")
                {
                    // LÓGICA PÓS-LOGIN (Ex: Criar o Cookie de Autenticação)
                    // ...

                    // Redireciona para a página principal do sistema após o login bem-sucedido
                    return RedirectToAction("Index", "Home"); // Redireciona para a Action "Index" do "HomeController"
                }
                else
                {
                    // Se a autenticação falhar, adiciona uma mensagem de erro ao ModelState.
                    // Isso será exibido na sua View.
                    ModelState.AddModelError(string.Empty, "Credenciais inválidas. Tente novamente.");
                    return View(model);
                }
            }

            // Se o ModelState não for válido (ex: campos em branco),
            // retorna para a mesma view, exibindo as mensagens de erro de validação.
            return View(model);
        }
    }
}