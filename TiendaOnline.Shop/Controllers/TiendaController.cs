using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TiendaOnline.Data;
using TiendaOnline.Domain.Models;
using TiendaOnline.Infrastructure;

namespace TiendaOnline.Shop.Controllers
{
    public class TiendaController : Controller
    {
        [HttpPost]
        public JsonResult AgregarCarrito(int productoid)
        {
            int clienteid = ((Cliente)Session["Cliente"]).Id;
            bool existe = new CarritoService().ExisteCarrito(clienteid, productoid);
            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El Producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CarritoService().OperacionCarrito(clienteid, productoid, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CantidadEnCarrito()
        {
            int clienteid = ((Cliente)Session["Cliente"]).Id;
            int cantidad = new CarritoService().CantidadEnCarrito(clienteid);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DetalleProducto(int idproducto = 0)
        {
            var producto = new Producto();

            bool conversion;

            producto = new ProductoService().Listar().Where(p => p.Id == idproducto).FirstOrDefault();
            if (producto != null)
            {
                producto.Base64 = Recursos.ConvertirBase64(Path.Combine(producto.RutaImagen, producto.NombreImagen), out conversion);
                producto.Extension = Path.GetExtension(producto.NombreImagen);
            }
            return View(producto);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int productoId)
        {
            int clienteid = ((Cliente)Session["Cliente"]).Id;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CarritoService().EliminarCarrito(clienteid, productoId);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ListaCategorias()
        {
            var lista = new List<Categoria>();

            lista = new CategoriaService().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListaMarcas()
        {
            var lista = new List<Marca>();

            lista = new MarcaService().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListaProductoCarrito()
        {
            int clienteid = ((Cliente)Session["Cliente"]).Id;
            var lista = new List<Carrito>();

            bool conversion;

            lista = new CarritoService().ListarProducto(clienteid).Select(oc => new Carrito()
            {
                ProductoId = new Producto
                {
                    Id = oc.Id,
                    Nombre = oc.ProductoId.Nombre,
                    MarcaId = oc.ProductoId.MarcaId,
                    Precio = oc.ProductoId.Precio,
                    RutaImagen = oc.ProductoId.RutaImagen,
                    Base64 = Recursos.ConvertirBase64(Path.Combine(oc.ProductoId.RutaImagen, oc.ProductoId.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.ProductoId.NombreImagen)
                },
                Cantidad = oc.Cantidad

            }).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarMarcaPorCategoria(int idcategoria)
        {
            var lista = new List<Marca>();

            lista = new MarcaService().ListarMarcaPorCategoria(idcategoria);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            var lista = new List<Producto>();

            bool conversion;

            lista = new ProductoService().Listar().Select(p => new Producto()
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                MarcaId = p.MarcaId,
                CategoriaId = p.CategoriaId,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p => p.CategoriaId.Id == (idcategoria == 0 ? p.CategoriaId.Id : idcategoria) && p.MarcaId.Id == (idmarca == 0 ? p.MarcaId.Id : idmarca) &&
            p.Stock > 0 && p.Activo == true)
            .ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int productoid, bool sumar)
        {
            int clienteid = ((Cliente)Session["Cliente"]).Id;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CarritoService().OperacionCarrito(clienteid, productoid, true, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerProvincia()
        {
            var lista = new UbicacionService().ObtenerProvincia();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerMunicipio(string provinciaId)
        {
            var lista = new List<Municipio>();

            lista = new UbicacionService().ObtenerMunicipio(provinciaId);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDistrito(string provinciaId, string municipioId)
        {
            var lista = new List<Distrito>();

            lista = new UbicacionService().ObtenerDistrito(provinciaId, municipioId);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Carrito()
        {
            return View();
        }
    }
}