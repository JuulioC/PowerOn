using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerOn.Data; // 1. Importar o namespace do seu DbContext
using PowerOn.Models;
using System.Linq; // 2. Importar o LINQ para fazer consultas
using System.Threading.Tasks; // Para operações assíncronas (boa prática)

namespace PowerOn.Controllers
{
    public class LoginController : Controller
    {
        // 3. Declarar uma variável para o DbContext
        private readonly ApplicationDbContext _context;

        // 4. Receber o DbContext no construtor (Injeção de Dependência)
        public LoginController(ApplicationDbContext context)
        {
            _context = context; // O ASP.NET Core vai "injetar" o DbContext aqui automaticamente
        }

        // GET: /Login
        // Esta action exibe a página de login
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Login
        // Esta action recebe os dados do formulário de login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // --- LÓGICA DE AUTENTICAÇÃO COM BANCO DE DADOS ---

                // 5. Busca no banco um usuário com o email fornecido que esteja ativo
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Ativo == 1);

                // 6. Verifica se o usuário foi encontrado
                if (usuario != null)
                {
                    // 7. ATENÇÃO: Verificação de senha (LEIA O AVISO ABAIXO)
                    // Esta verificação é simples e compara a senha digitada com a que está no banco.
                    // Em um projeto real, a senha NUNCA deve ser guardada como texto puro.
                    // Você deve usar um algoritmo de HASH para salvar e verificar a senha.
                    if (usuario.Senha == model.Password)
                    {
                        // LÓGICA PÓS-LOGIN (Ex: Criar o Cookie de Autenticação)
                        // ... aqui você vai configurar a sessão do usuário ...

                        // Redireciona para a página principal do sistema
                        return RedirectToAction("Index", "Home");
                    }
                }

                // Se o usuário não foi encontrado ou a senha está incorreta,
                // a mensagem de erro é a mesma por segurança.
                ModelState.AddModelError(string.Empty, "Email ou senha inválidos. Tente novamente.");
                return View(model);
            }

            // Se o ModelState não for válido, retorna para a mesma view
            return View(model);
        }
    }
}
