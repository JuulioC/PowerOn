using Microsoft.AspNetCore.Mvc;
using PowerOn.Models;

namespace PowerOn.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // AQUI ENTRA A SUA LÓGICA DE AUTENTICAÇÃO
                // Ex: Verificar se model.Email e model.Password existem no banco de dados.
                // Se o login for válido:
                // await SignInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                return RedirectToAction("Index", "Home");

                // Se o login for inválido:
                // ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
                // return View(model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // AQUI ENTRA A SUA LÓGICA DE LOGOUT
            // await SignInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}