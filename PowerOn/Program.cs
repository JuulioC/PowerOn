// Usings necess�rios para o Program.cs
using Microsoft.EntityFrameworkCore;
using PowerOn.Data; // Onde seu ApplicationDbContext est�
using Microsoft.AspNetCore.Authentication.Cookies; // Necess�rio para a autentica��o por cookies
using Microsoft.AspNetCore.Authorization; // Necess�rio para UseAuthorization (embora n�o diretamente usado no Program.cs, � parte do stack)

var builder = WebApplication.CreateBuilder(args);

// --- IN�CIO DA CONFIGURA��O DO BANCO DE DADOS ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
// --- FIM DA CONFIGURA��O DO BANCO DE DADOS ---

// --- IN�CIO DA CONFIGURA��O DA AUTENTICA��O POR COOKIES ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });
// --- FIM DA CONFIGURA��O DA AUTENTICA��O POR COOKIES ---

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure o pipeline de requisi��es HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // ESSENCIAL PARA VER DETALHES DO ERRO
}

app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS
app.UseStaticFiles(); // Para servir CSS, JS, imagens
app.UseRouting();

// --- MIDDLEWARE DE AUTENTICA��O E AUTORIZA��O (ORDEM CRUCIAL) ---
app.UseAuthentication(); // Deve vir ANTES de UseAuthorization
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Error/{0}"); // Para p�ginas de erro gen�ricas

// Sua rota padr�o
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"); // Redireciona para o LoginController

app.Run();
