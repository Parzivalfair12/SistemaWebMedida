using Microsoft.AspNetCore.Mvc;
using SistemaWebMedida.Data;
using SistemaWebMedida.Models;
using Microsoft.EntityFrameworkCore;
using SistemaWebMedida.ViewsModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SistemaWebMedida.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _dbContext;
        public AccesoController(AppDBContext appDBContext)
        {
            _dbContext = appDBContext;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioViewModel Modelo)
        {

            if (Modelo.Clave != Modelo.ConfirmarClave)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden.";
                return View();
            }

            Usuario usuario = new Usuario()
            {
                NombreCompleto = Modelo.Nombres + " " + Modelo.Apellidos,
                Nombres = Modelo.Nombres,
                Apellidos = Modelo.Apellidos,
                FechaNacimiento = Modelo.FechaNacimiento,
                NumeroTelefono = Modelo.NumeroTelefono,
                CorreoUsuario = Modelo.CorreoUsuario,
                Clave = Modelo.Clave,
                Direccion = Modelo.Direccion,
            };

            await _dbContext.Usuarios.AddAsync(usuario);
            await _dbContext.SaveChangesAsync();

            if (usuario.IdUsuario != 0)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["Mensaje"] = "Ha ocurrido un error, contacte con administrador.";
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel Modelo)
        {
            Usuario? usuario_encontrado = await _dbContext.Usuarios
                .Where(u =>
                u.CorreoUsuario == Modelo.Correo &&
                u.Clave == Modelo.Clave
                ).FirstOrDefaultAsync();

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias.";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.NombreCompleto)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("index", "Home");

        }
    }
}
