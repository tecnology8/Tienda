using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TiendaOnline.Data;
using TiendaOnline.Domain.Models;
using TiendaOnline.Infrastructure;

namespace TiendaOnline.Shop.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Cliente model)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(model.Nombres) ? "" : model.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(model.Apellidos) ? "" : model.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(model.Correo) ? "" : model.Correo;

            if(model.Clave != model.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = new ClienteService().Registrar(model, out mensaje);
            if(resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente cliente = null;
            cliente = new ClienteService().Listar().Where(t => t.Correo == correo && t.Clave == Recursos.ConvertSha256(clave)).FirstOrDefault();
            
            if (cliente == null)
            {
                ViewBag.Error = "Correo o Contraseña no son correctas";
                return View();
            }
            else
            {
                if (cliente.Reestablecer)
                {
                    TempData["IdCliente"] = cliente.Id;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(cliente.Correo, false);
                    Session["Cliente"] = cliente;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            var cliente = new Cliente();
            string nuevaclave = Recursos.GenerarClave();

            cliente = new ClienteDb().Listar().Where(t => t.Correo == correo).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "No se encontro un cliente relacionado con ese correo";
                return View();
            }
            else
            {
                string mensaje = string.Empty;
                bool respuesta = new ClienteDb().ReestablerClave(cliente.Id, cliente.Clave, out mensaje);

                string asunto = "Contraseña Reestablecida";
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave", nuevaclave);

                //Enviar Correo al Usuario              
                bool enviar = Recursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Acceso");
                }
                else
                {
                    ViewBag.Error = mensaje;
                    return View();
                }
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idcliente, string claveactual, string nuevaclave, string confirmarclave)
        {
            var cliente = new Cliente();
            cliente = new ClienteDb().Listar().Where(t => t.Id == int.Parse(idcliente)).FirstOrDefault();

            if (cliente.Clave != Recursos.ConvertSha256(claveactual))
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La Contraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las Contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";

            nuevaclave = Recursos.ConvertSha256(nuevaclave);

            string mensaje = string.Empty;
            bool respuesta = new ClienteDb().CambiarClave(int.Parse(idcliente), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = idcliente;
                ViewBag.Error = mensaje;
                return View();
            }
        }
        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}