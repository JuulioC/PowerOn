using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PowerOn.Controllers
{
    public class ErrorController : Controller
    {
        // Adicionamos um campo privado para guardar a informação do ambiente
        private readonly IWebHostEnvironment _environment;

        // O serviço IWebHostEnvironment é injetado automaticamente pelo ASP.NET Core aqui no construtor
        public ErrorController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("Error")]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error500()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ErrorPath = exceptionHandlerPathFeature?.Path;
            ViewBag.ErrorMessage = exceptionHandlerPathFeature?.Error.Message;

            // AQUI ESTÁ A MUDANÇA: Verificamos o ambiente aqui no Controller...
            // ...e passamos o resultado (true ou false) para a View através do ViewBag.
            ViewBag.ShowErrorDetails = _environment.IsDevelopment();

            return View();
        }
    }
}