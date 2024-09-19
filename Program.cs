using SistemaWebMedida.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Definimos donde se encuentra nuestra cadena de conexion
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"));
});

//Aqui configuarmos el tiempo de sesion del usuario y que no pueda acceder a otros formularios desde el
//buscador del navegador
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//Aca decimos que use la  autenticacion
app.UseAuthorization();

//Aqui configuramos lo que se abre por defecto al levantar la aplicacion
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

app.Run();
