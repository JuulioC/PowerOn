using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Necessário para DbContext
using PowerOn.Data; // Importa o namespace do seu DbContext
using PowerOn.Models; // Importa o namespace dos seus modelos (Usuario, RegisterViewModel, EditProfileViewModel)
using PowerOn.Utils; // Importa o namespace da classe PasswordHasher
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims; // Necessário para autenticação
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication; // Necessário para autenticação
using Microsoft.AspNetCore.Authentication.Cookies; // Necessário para autenticação
using Microsoft.AspNetCore.Authorization; // Necessário para o atributo [Authorize]
using PowerOn.Utils; // Adicione este using para acessar TiposEnum (assumindo que está em PowerOn.Utils)
using System.IO; // Necessário para MemoryStream

namespace PowerOn.Controllers
{
    // Aplica o atributo [Authorize] a todo o controller para garantir que apenas usuários logados acessem suas ações.
    // Você pode remover isso e aplicar [Authorize] individualmente se algumas ações forem públicas.
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        // Esta action exibe o formulário de cadastro de usuário (não requer autenticação para acessar)
        [AllowAnonymous] // Permite acesso a usuários não autenticados
        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Retorna a View padrão (Register.cshtml)
        }

        // POST: /Account/Register
        // Esta action recebe os dados do formulário de cadastro
        [AllowAnonymous] // Permite acesso a usuários não autenticados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) // Verifica se os dados do formulário são válidos
            {
                // 1. Verifica se já existe um usuário com o mesmo email
                var existingUser = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Este email já está cadastrado.");
                    return View(model); // Retorna à View com o erro
                }

                // 2. Cria uma nova instância de Usuario a partir do ViewModel
                var newUser = new Usuario
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    // Hashear a senha antes de salvar
                    Senha = PasswordHasher.HashPassword(model.Password),
                    Perfil = model.Perfil ?? 1, // Usa o perfil do modelo ou 1 (usuário padrão) se for nulo
                    EmpresaId = model.EmpresaId,
                    CodigoSistema = model.CodigoSistema,
                    Ativo = 1, // Define o usuário como ativo por padrão
                    UltimoAcesso = DateTime.Now // Define a data do último acesso como agora
                    // ImgPerfil e ImgPerfilMimeType não são definidos aqui, pois é um upload separado ou padrão.
                };

                // 3. Adiciona o novo usuário ao DbContext e salva no banco de dados
                _context.Usuarios.Add(newUser);
                await _context.SaveChangesAsync();

                // 4. Opcional: Autenticar o usuário automaticamente após o registro
                // Isso evita que o usuário precise fazer login imediatamente após se cadastrar.
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, newUser.Nome ?? newUser.Email),
                    new Claim(ClaimTypes.Role, newUser.Perfil.ToString()),
                    new Claim("EmpresaId", newUser.EmpresaId.ToString()),
                    new Claim("CodigoSistema", newUser.CodigoSistema ?? "")
                };

                // As claims de imagem não são adicionadas aqui para evitar problemas de tamanho de cookie.
                // A imagem será carregada sob demanda via o endpoint GetProfileImage.

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redireciona para uma página de sucesso ou para a página inicial
                return RedirectToAction("RegistrationSuccess", "Account"); // Você pode criar esta View
            }

            // Se o ModelState não for válido, retorna para a View de cadastro com os erros
            return View(model);
        }

        // GET: /Account/RegistrationSuccess
        [AllowAnonymous] // Permite acesso a usuários não autenticados
        [HttpGet]
        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        // GET: /Account/Profile
        // Exibe o formulário para o usuário editar seu próprio perfil
        // Requer que o usuário esteja autenticado (pelo [Authorize] no controller)
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Pega o ID do usuário logado através das Claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // Se o ID do usuário não for encontrado (usuário não logado ou claim ausente),
                // redireciona para o login.
                return RedirectToAction("Index", "Login");
            }

            // Busca o usuário no banco de dados
            var usuario = await _context.Usuarios.FindAsync(int.Parse(userId));

            if (usuario == null)
            {
                // Se o usuário não for encontrado no banco (apesar de estar logado),
                // pode ser um erro, então desloga e redireciona.
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Login");
            }

            // Mapeia os dados do Usuario para o EditProfileViewModel
            var model = new EditProfileViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil,
                EmpresaId = usuario.EmpresaId,
                CodigoSistema = usuario.CodigoSistema
                // Senha e Confirmar Senha não são carregados aqui por segurança.
            };

            // ImgPerfilBase64 e ImgPerfilMimeType não são passados diretamente para o ViewModel para exibição direta,
            // porque a imagem será carregada via endpoint.
            // No entanto, para a pré-visualização no formulário de edição, precisamos do Base64.
            if (usuario.ImgPerfil != null && usuario.ImgPerfil.Length > 0)
            {
                model.ImgPerfilBase64 = Convert.ToBase64String(usuario.ImgPerfil);
                model.ImgPerfilMimeType = usuario.ImgPerfilMimeType; // Passa o tipo MIME para o ViewModel para uso na View
            }


            var userProfileClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            int currentUserProfileId = -1;
            if (int.TryParse(userProfileClaim, out int profileId))
            {
                currentUserProfileId = profileId;
            }
            ViewData["CurrentUserProfileId"] = currentUserProfileId;


            return View(model);
        }

        // POST: /Account/Profile
        // Processa o envio do formulário de edição de perfil
        // Requer que o usuário esteja autenticado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(EditProfileViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId) || model.Id != int.Parse(userId))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Login");
            }

            var userProfileClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            int currentUserProfileId = -1;
            if (int.TryParse(userProfileClaim, out int profileId))
            {
                currentUserProfileId = profileId;
            }
            bool canEditRestrictedFields = currentUserProfileId == 0 || currentUserProfileId == 4;


            if (string.IsNullOrEmpty(model.NewPassword))
            {
                ModelState.Remove(nameof(model.NewPassword));
                ModelState.Remove(nameof(model.ConfirmNewPassword));
            }

            var usuarioToUpdate = await _context.Usuarios.FindAsync(model.Id);

            if (usuarioToUpdate == null)
            {
                TempData["ErrorMessage"] = "Usuário não encontrado.";
                // Recarrega ImgPerfilBase64 e ImgPerfilMimeType para que a imagem atual seja exibida corretamente
                if (usuarioToUpdate != null && usuarioToUpdate.ImgPerfil != null && usuarioToUpdate.ImgPerfil.Length > 0)
                {
                    model.ImgPerfilBase64 = Convert.ToBase64String(usuarioToUpdate.ImgPerfil);
                }
                model.ImgPerfilMimeType = usuarioToUpdate.ImgPerfilMimeType;
                return View(model);
            }

            if (ModelState.IsValid)
            {
                if (usuarioToUpdate.Email != model.Email)
                {
                    var existingUserWithNewEmail = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.Email == model.Email && u.Id != model.Id);
                    if (existingUserWithNewEmail != null)
                    {
                        ModelState.AddModelError("Email", "Este email já está sendo usado por outro usuário.");
                        // Recarrega ImgPerfilBase64 e ImgPerfilMimeType para que a imagem atual seja exibida corretamente
                        if (usuarioToUpdate.ImgPerfil != null && usuarioToUpdate.ImgPerfil.Length > 0)
                        {
                            model.ImgPerfilBase64 = Convert.ToBase64String(usuarioToUpdate.ImgPerfil);
                        }
                        model.ImgPerfilMimeType = usuarioToUpdate.ImgPerfilMimeType;
                        return View(model);
                    }
                }

                usuarioToUpdate.Nome = model.Nome;
                usuarioToUpdate.Email = model.Email;

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    usuarioToUpdate.Senha = PasswordHasher.HashPassword(model.NewPassword);
                }

                // Lógica para Upload de Imagem de Perfil e SALVAR TIPO MIME
                if (model.NewProfileImageFile != null)
                {
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        await model.NewProfileImageFile.CopyToAsync(memoryStream);
                        usuarioToUpdate.ImgPerfil = memoryStream.ToArray();
                        usuarioToUpdate.ImgPerfilMimeType = model.NewProfileImageFile.ContentType; // Salva o tipo MIME
                    }
                }
                else if (model.ImgPerfilBase64 == null && usuarioToUpdate.ImgPerfil != null)
                {
                    // Lógica para remover a imagem se for o caso (não implementado na View ainda)
                    // usuarioToUpdate.ImgPerfil = null;
                    // usuarioToUpdate.ImgPerfilMimeType = null;
                }


                if (canEditRestrictedFields)
                {
                    usuarioToUpdate.Perfil = model.Perfil;
                    usuarioToUpdate.EmpresaId = model.EmpresaId;
                    usuarioToUpdate.CodigoSistema = model.CodigoSistema;
                }
                else
                {
                    // Não atualiza Perfil, EmpresaId, CodigoSistema se o usuário não tem permissão.
                }


                _context.Usuarios.Update(usuarioToUpdate);
                await _context.SaveChangesAsync();

                // Após a atualização, é uma boa prática re-autenticar o usuário
                // para atualizar as Claims no cookie de autenticação,
                // especialmente se o Nome, Email ou Imagem mudaram.
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarioToUpdate.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuarioToUpdate.Nome ?? usuarioToUpdate.Email),
                    new Claim(ClaimTypes.Role, usuarioToUpdate.Perfil.ToString()),
                    new Claim("EmpresaId", usuarioToUpdate.EmpresaId.ToString()),
                    new Claim("CodigoSistema", usuarioToUpdate.CodigoSistema ?? "")
                };
                // As claims de imagem não são adicionadas ao cookie de autenticação.
                // Elas serão carregadas sob demanda pelo endpoint GetProfileImage.


                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = false, ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


                TempData["SuccessMessage"] = "Seu perfil foi atualizado com sucesso!";
                return RedirectToAction("Profile");
            }

            // Se o ModelState não for válido, retorna para a View com os erros
            // Recarrega a ImgPerfilBase64 e ImgPerfilMimeType para que a imagem atual seja exibida corretamente
            if (usuarioToUpdate != null && usuarioToUpdate.ImgPerfil != null && usuarioToUpdate.ImgPerfil.Length > 0)
            {
                model.ImgPerfilBase64 = Convert.ToBase64String(usuarioToUpdate.ImgPerfil);
            }
            model.ImgPerfilMimeType = usuarioToUpdate.ImgPerfilMimeType;
            return View(model);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return Redirect("/Login");
        }

        // NOVO ENDPOINT: Para servir a imagem de perfil diretamente
        [HttpGet]
        [AllowAnonymous] // Pode ser acessado por qualquer um que tenha a URL
        public async Task<IActionResult> GetProfileImage(int userId)
        {
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null || usuario.ImgPerfil == null || usuario.ImgPerfil.Length == 0)
            {
                // Retorna uma imagem padrão se o usuário não for encontrado ou não tiver imagem
                // Você pode servir um arquivo estático aqui
                return File("~/assets/images/UserImage.png", "image/png");
            }

            // Retorna a imagem como um FileContentResult com o tipo MIME correto
            return File(usuario.ImgPerfil, usuario.ImgPerfilMimeType ?? "image/jpeg"); // Fallback para image/jpeg se MIME Type for nulo
        }

        [HttpGet]
        public async Task<IActionResult> ProfileImage()
        {
            // Pega o ID do usuário logado a partir das claims do cookie
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                                        .AsNoTracking() // Melhora a performance para consultas de apenas leitura
                                        .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

            if (usuario?.ImgPerfil == null || usuario.ImgPerfil.Length == 0)
            {
                // Opcional: retorne uma imagem padrão caso o usuário não tenha uma.
                // var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", "default-avatar.png");
                // return PhysicalFile(defaultImagePath, "image/png");
                return NotFound(); // Ou retorne um status 404
            }

            // Retorna os bytes da imagem diretamente com o tipo MIME correto
            return File(usuario.ImgPerfil, usuario.ImgPerfilMimeType ?? "image/jpeg");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Dados inválidos.", errors });
            }

            // Pega o ID do usuário logado a partir das Claims
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            {
                return Json(new { success = false, message = "Usuário não autenticado." });
            }

            var user = await _context.Usuarios.FindAsync(userId);
            if (user == null)
            {
                return Json(new { success = false, message = "Usuário não encontrado." });
            }

            // Atualiza a senha
            user.Senha = PasswordHasher.HashPassword(model.NewPassword); // Use seu método de hash
            _context.Usuarios.Update(user);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Senha alterada com sucesso!" });
        }
    }
}
