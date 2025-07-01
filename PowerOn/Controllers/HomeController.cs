using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization; // Necessário para o atributo [Authorize]
using PowerOn.Models; // Necessário para ErrorViewModel

namespace PowerOn.Controllers
{
    [Authorize] // Garante que apenas usuários autenticados possam acessar este controller
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Você pode acessar informações do usuário logado aqui, se necessário
            // Por exemplo: string userName = User.Identity.Name;

            ViewData["NomeConsultor"] = "Felippe Toledo";
            return View(); // Retorna a View padrão (Home/Index.cshtml)
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
