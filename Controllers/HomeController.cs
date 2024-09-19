using Microsoft.AspNetCore.Mvc;
using SistemaWebMedida.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using SistemaWebMedida.Data;
using OfficeOpenXml;
using System.Data;
using Aspose.Cells;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SistemaWebMedida.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

        [HttpGet]
        public JsonResult ReportePresion()
        {
            DtReporte ObjReporte = new DtReporte();

            List<ReporteDatos> Lis =  ObjReporte.RetornarPresion();
            return Json(Lis);
        }

        [HttpGet]
        public JsonResult ReportePresionDif()
        {
            DtReporte ObjReporte = new DtReporte();

            List<ReporteDatos> Lis = ObjReporte.RetornarPresionDif();
            return Json(Lis);
        }

        [HttpGet]
        public JsonResult ReporteCo2()
        {
            DtReporte ObjReporte = new DtReporte();

            List<ReporteDatos> Lis = ObjReporte.RetornarCo2();
            return Json(Lis);
        }

        [HttpGet]
        public JsonResult ReporteTemperatura()
        {
            DtReporte ObjReporte = new DtReporte();

            List<ReporteDatos> Lis = ObjReporte.RetornarTemperatura();
            return Json(Lis);
        }

        [HttpGet]
        public JsonResult ReporteOxigeno()
        {
            DtReporte ObjReporte = new DtReporte();

            List<ReporteDatos> Lis = ObjReporte.RetornarOxigeno();
            return Json(Lis);
        }

        [HttpGet]
        public ReporteDatos UltimoRegistro()
        {
            DtReporte ObjReporte = new DtReporte();

            ReporteDatos Entidad = ObjReporte.UltimoRegistro();
            return Entidad;
        }

        [HttpPost]
        public IActionResult ActualizarValores([FromBody] ValoresModel model)
        {
            DtReporte ObjReporte = new DtReporte();
            if (model == null)
            {
                return BadRequest("Datos no válidos.");
            }

            try
            {
                // Construir las cadenas de fecha y hora
                string fechaHoraInicioStr = $"{model.fecha:yyyy-MM-dd} {model.horaini}";
                string fechaHoraFinStr = $"{model.fecha:yyyy-MM-dd} {model.horafin}";

                // Convertir cadenas a DateTime
                DateTime fechaHoraInicio;
                DateTime fechaHoraFin;

                // Intentar convertir las cadenas a DateTime
                if (!DateTime.TryParse(fechaHoraInicioStr, out fechaHoraInicio))
                {
                    return BadRequest("Fecha y hora de inicio no válidos.");
                }

                if (!DateTime.TryParse(fechaHoraFinStr, out fechaHoraFin))
                {
                    return BadRequest("Fecha y hora de fin no válidos.");
                }

                // Convertir valores de string a decimal
                decimal peso;
                decimal volumen;

                if (!decimal.TryParse(model.peso, out peso))
                {
                    return BadRequest("Peso no válido.");
                }

                if (!decimal.TryParse(model.volumen, out volumen))
                {
                    return BadRequest("Volumen no válido.");
                }

                // Llamar al método ActualizarReporte
                bool Respuesta = ObjReporte.ActualizarDatos(peso, volumen, fechaHoraInicio, fechaHoraFin);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error al actualizar los datos: {ex.Message}");
            }

            return Ok(new { success = true });
        }

        [HttpGet]
        public ActionResult DescargarExcel()
        {
            DtReporte ObjReporte = new DtReporte();
            DataTable dt = ObjReporte.LlenarDataTable();

            // Crear un nuevo libro de trabajo de Excel
            var workbook = new Workbook();
            var worksheet = workbook.Worksheets[0];

            // Agregar los encabezados de columna
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                worksheet.Cells[0, i].PutValue(dt.Columns[i].ColumnName);
            }

            // Agregar los datos
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    worksheet.Cells[i + 1, j].PutValue(dt.Rows[i][j]);
                }
            }

            // Establecer la primera hoja como activa
            workbook.Worksheets.ActiveSheetIndex = 0;

            // Guardar el libro en un MemoryStream
            using (var stream = new MemoryStream())
            {
                workbook.Save(stream, SaveFormat.Xlsx);
                var fileContents = stream.ToArray();

                // Devolver el archivo Excel como una descarga
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte.xlsx");
            }
        }
    }
}
