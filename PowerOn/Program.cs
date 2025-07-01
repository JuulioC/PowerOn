// Usings necessários para o Program.cs
using Microsoft.EntityFrameworkCore;
using PowerOn.Data; // Onde seu ApplicationDbContext está
using Microsoft.AspNetCore.Authentication.Cookies; // Necessário para a autenticação por cookies
using Microsoft.AspNetCore.Authorization; // Necessário para UseAuthorization (embora não diretamente usado no Program.cs, é parte do stack)

var builder = WebApplication.CreateBuilder(args);

// --- INÍCIO DA CONFIGURAÇÃO DO BANCO DE DADOS ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
// --- FIM DA CONFIGURAÇÃO DO BANCO DE DADOS ---

// --- INÍCIO DA CONFIGURAÇÃO DA AUTENTICAÇÃO POR COOKIES ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });
// --- FIM DA CONFIGURAÇÃO DA AUTENTICAÇÃO POR COOKIES ---

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure o pipeline de requisições HTTP.
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

// --- MIDDLEWARE DE AUTENTICAÇÃO E AUTORIZAÇÃO (ORDEM CRUCIAL) ---
app.UseAuthentication(); // Deve vir ANTES de UseAuthorization
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Error/{0}"); // Para páginas de erro genéricas

// Sua rota padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"); // Redireciona para o LoginController

app.Run();
