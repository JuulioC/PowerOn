using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; // Usaremos para futuras chamadas de API

namespace PowerOn.Controllers
{
    // Garante que apenas usuários logados possam acessar
    // [Authorize] 
    public class AtendimentoController : Controller
    {
        // GET: /Atendimento/Novo
        // Vamos usar uma ação chamada 'Novo' para representar a página de novo atendimento.
        [HttpGet]
        public IActionResult Novo()
        {
            // Esta linha diz ao ASP.NET para procurar o arquivo:
            // /Views/Atendimento/Novo.cshtml
            return View();
        }

        // Futuramente, aqui entrarão as outras ações, como:
        // - BuscarCliente(string termo)
        // - GerarOrdemDeServico(...)
    }
}