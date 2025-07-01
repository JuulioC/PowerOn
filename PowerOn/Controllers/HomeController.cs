using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization; // Necess�rio para o atributo [Authorize]
using PowerOn.Models; // Necess�rio para ErrorViewModel

namespace PowerOn.Controllers
{
    [Authorize] // Garante que apenas usu�rios autenticados possam acessar este controller
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Voc� pode acessar informa��es do usu�rio logado aqui, se necess�rio
            // Por exemplo: string userName = User.Identity.Name;

            ViewData["NomeConsultor"] = "Felippe Toledo";
            return View(); // Retorna a View padr�o (Home/Index.cshtml)
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
