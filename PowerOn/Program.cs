// 1. ADICIONE ESTES USINGS IMPORTANTES
using Microsoft.EntityFrameworkCore;
using PowerOn.Data; // Onde seu ApplicationDbContext está

// Não precisa mais deste using, pois o controller é encontrado automaticamente
// using PowerOn.Controllers;

var builder = WebApplication.CreateBuilder(args);

// --- INÍCIO DA CONFIGURAÇÃO DO BANCO DE DADOS ---

// 2. Pega a Connection String do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 3. Adiciona e configura o DbContext para usar MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// --- FIM DA CONFIGURAÇÃO DO BANCO DE DADOS ---

// Adiciona os serviços para Controllers e Views (isso você já tinha)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    // Esta é uma boa prática para ambientes de desenvolvimento
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Middleware de autorização (importante para áreas logadas)
app.UseAuthorization();

// Rota para páginas de erro genéricas (isso você já tinha)
app.UseStatusCodePagesWithReExecute("/Error/{0}");

// Sua rota padrão que direciona para a página de login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();