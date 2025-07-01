using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Necessário para DbContext
using PowerOn.Data; // Importa o namespace do seu DbContext
using PowerOn.Models; // Importa o namespace dos seus modelos (Usuario, LoginViewModel)
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims; // Necessário para criar Claims para autenticação
using Microsoft.AspNetCore.Authentication; // Necessário para HttpContext.SignInAsync
using Microsoft.AspNetCore.Authentication.Cookies; // Necessário para CookieAuthenticationDefaults
using PowerOn.Utils; // Necessário para PasswordHasher
using System; // Necessário para Exception

namespace PowerOn.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (usuario != null && usuario.Senha != null && PasswordHasher.VerifyPassword(model.Password, usuario.Senha))
                {
                    // Claims completas.
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Name, usuario.Nome ?? usuario.Email),
                        new Claim(ClaimTypes.Role, usuario.Perfil?.ToString() ?? ""), // Garante que Perfil não é nulo
                        new Claim("EmpresaId", usuario.EmpresaId?.ToString() ?? ""), // Garante que EmpresaId não é nulo
                        new Claim("CodigoSistema", usuario.CodigoSistema ?? "")
                    };

                    // Lógica para a claim da imagem de perfil, garantindo que ela SEMPRE seja adicionada, mesmo que vazia.
                    string imgBase64 = "";
                    if (usuario.ImgPerfil != null && usuario.ImgPerfil.Length > 0)
                    {
                        imgBase64 = Convert.ToBase64String(usuario.ImgPerfil);
                    }
                    claims.Add(new Claim("ImgPerfilBase64", imgBase64));
                    claims.Add(new Claim("ImgPerfilMimeType", usuario.ImgPerfilMimeType ?? ""));


                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    };

                    try
                    {
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro durante SignIn ou Redirecionamento: {ex.Message}");
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado durante o login. Tente novamente.");
                        return View("~/Views/Account/Login.cshtml", model);
                    }
                }

                ModelState.AddModelError(string.Empty, "Email ou senha inválidos. Tente novamente.");
                return View("~/Views/Account/Login.cshtml", model);
            }

            return View("~/Views/Account/Login.cshtml", model);
        }
    }
}
