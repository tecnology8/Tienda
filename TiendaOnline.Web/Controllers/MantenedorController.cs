using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Data;
using TiendaOnline.Domain.Models;
using TiendaOnline.Infrastructure;

namespace TiendaOnline.Web.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        #region CRUD Categoria

        public JsonResult ListarCategorias()
        {
            var lista = new List<Categoria>();

            lista = new CategoriaService().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarCategoria(Categoria model)
        {
            object resultado;
            string mensaje = string.Empty;

            if (model.Id == 0)
            {
                resultado = new CategoriaService().Registrar(model, out mensaje);
            }
            else
            {
                resultado = new CategoriaService().Editar(model, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CategoriaService().Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CRUD Marca
        public JsonResult ListarMarcas()
        {
            var lista = new List<Marca>();

            lista = new MarcaService().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca model)
        {
            object resultado;
            string mensaje = string.Empty;

            if (model.Id == 0)
            {
                resultado = new MarcaService().Registrar(model, out mensaje);
            }
            else
            {
                resultado = new MarcaService().Editar(model, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new MarcaService().Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region CRUD Productos
        public JsonResult ListarProductos()
        {
            var lista = new List<Producto>();

            lista = new ProductoService().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase imagen)
        {
            //object resultado;
            string mensaje = string.Empty;
            bool operacionExitosa = true;
            bool guardarImagenExitosa = true;

            var model = new Producto();
            model = JsonConvert.DeserializeObject<Producto>(objeto);

            //Validar Precio
            decimal precio;
            if (decimal.TryParse(model.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-DO"), out precio))
            {
                model.Precio = precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El Formato del Precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }
            if (model.Id == 0)
            {
                int idProductoGenerado = new ProductoService().Registrar(model, out mensaje);
                if (idProductoGenerado != 0)
                {
                    model.Id = idProductoGenerado;
                }
                else
                {
                    operacionExitosa = false;
                }
            }
            else
            {
                operacionExitosa = new ProductoService().Editar(model, out mensaje);
            }

            //Guardar Imagen

            if (operacionExitosa)
            {
                if(imagen != null)
                {
                    string rutaGuardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(imagen.FileName);
                    string nombreImage = string.Concat(model.Id.ToString(), extension);

                    try
                    {
                        imagen.SaveAs(Path.Combine(rutaGuardar, nombreImage));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardarImagenExitosa = false;
                    }

                    if (guardarImagenExitosa)
                    {
                        model.RutaImagen = rutaGuardar;
                        model.NombreImagen = nombreImage;
                        bool respuesta = new ProductoService().GuardarDatosImagen(model, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto pero hubo problemas con al Imagen";
                    }
                }
            }
            
            return Json(new { operacionExitosa = operacionExitosa, idGenerado = model.Id, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            var model = new ProductoService().Listar().Where(t => t.Id == id).FirstOrDefault();

            string textoBase64 = Recursos.ConvertirBase64(Path.Combine(model.RutaImagen, model.NombreImagen), out conversion);

            return Json(new { conversion = conversion, 
                textoBase64 = textoBase64, 
                extension = Path.GetExtension(model.NombreImagen)},
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new ProductoService().Eliminar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}