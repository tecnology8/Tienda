using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Domain.Models;
using TiendaOnline.Infrastructure;

namespace TiendaOnline.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Usuarios()
        {
            return View();
        }
        public JsonResult ListarUsuarios()
        {
            var lista = new List<Usuario>();

            lista = new UsuarioService().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario model)
        {
            object resultado;
            string mensaje = string.Empty;

            if(model.Id == 0)
            {
                resultado = new UsuarioService().Registrar(model, out mensaje);
            }
            else
            {
                resultado = new UsuarioService().Editar(model, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new UsuarioService().Eliminar(id, out mensaje);
           
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult VistaDashboard()
        {
            var dashboard = new ReporteService().VerDashboard();
            return Json(new { resultado = dashboard }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListaReporte(string fechainicio, string fechafin, string transaccionid)
        {
            var lista = new List<Reporte>();
            lista = new ReporteService().Ventas(fechainicio, fechafin, transaccionid);
            return Json(new { resultado = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin, string transaccionid)
        {
            var lista = new List<Reporte>();

            lista = new ReporteService().Ventas(fechainicio, fechafin, transaccionid);

            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-DO");
            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("TransaccionId", typeof(string));

            foreach (Reporte item in lista)
            {
                dt.Rows.Add(new object[]
                {
                    item.FechaVenta,
                    item.Cliente,
                    item.Producto,
                    item.Precio,
                    item.Cantidad,
                    item.Total,
                    item.TransaccionId
                });
            }

            dt.TableName = "Datos";

            //Exportar a Excel

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString()+ ".xlsx");
                }
            }
        }
    }
}  