// 1. ADICIONE ESTES USINGS IMPORTANTES
using Microsoft.EntityFrameworkCore;
using PowerOn.Data; // Onde seu ApplicationDbContext est�

// N�o precisa mais deste using, pois o controller � encontrado automaticamente
// using PowerOn.Controllers; 

var builder = WebApplication.CreateBuilder(args);

// --- IN�CIO DA CONFIGURA��O DO BANCO DE DADOS ---

// 2. Pega a Connection String do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 3. Adiciona e configura o DbContext para usar MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// --- FIM DA CONFIGURA��O DO BANCO DE DADOS ---

// Adiciona os servi�os para Controllers e Views (isso voc� j� tinha)
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
    // Esta � uma boa pr�tica para ambientes de desenvolvimento
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Middleware de autoriza��o (importante para �reas logadas)
app.UseAuthorization();

// Rota para p�ginas de erro gen�ricas (isso voc� j� tinha)
app.UseStatusCodePagesWithReExecute("/Error/{0}");

// Sua rota padr�o que direciona para a p�gina de login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();