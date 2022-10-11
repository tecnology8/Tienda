using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TiendaOnline.Data;
using TiendaOnline.Domain.Models;

namespace TiendaOnline.Web.Controllers
{
    
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            var usuario = new Usuario();
            usuario = new UsuarioDb().Listar().Where(t => t.Correo == correo && t.Clave == Recursos.ConvertSha256(clave)).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña no es correcta";
                return View();
            }
            else
            {
                if (usuario.Reestablecer)
                {
                    TempData["IdUsuario"] = usuario.Id;
                    return RedirectToAction("CambiarClave");
                }
                FormsAuthentication.SetAuthCookie(usuario.Correo, false);

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {

            var usuario = new Usuario();
            usuario = new UsuarioDb().Listar().Where(t => t.Id == int.Parse(idusuario)).FirstOrDefault();

            if (usuario.Clave != Recursos.ConvertSha256(claveactual))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La Contraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las Contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";

            nuevaclave = Recursos.ConvertSha256(nuevaclave);

            string mensaje = string.Empty;
            bool respuesta = new UsuarioDb().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = idusuario;
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            var usuario = new Usuario();
            string nuevaclave = Recursos.GenerarClave();

            usuario = new UsuarioDb().Listar().Where(t => t.Correo == correo).FirstOrDefault();

            if(usuario == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado con ese correo";
                return View();
            }
            else
            {
                string mensaje = string.Empty;
                bool respuesta = new UsuarioDb().ReestableClave(usuario.Id, usuario.Clave, out mensaje);

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

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}