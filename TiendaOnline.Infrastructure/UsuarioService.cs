using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaOnline.Data;
using TiendaOnline.Domain.Models;

namespace TiendaOnline.Infrastructure
{
    public class UsuarioService
    {
        private UsuarioDb usuarioDb = new UsuarioDb();
        public List<Usuario> Listar()
        {
            return usuarioDb.Listar();
        }
        public int Registrar(Usuario model, out string mensaje)
        {
            mensaje = string.Empty;
            if(string.IsNullOrEmpty(model.Nombres) || string.IsNullOrWhiteSpace(model.Nombres))
            {
                mensaje = "El Nombre del usuario no puede ser vacio";
            }
            else if(string.IsNullOrEmpty(model.Apellidos) || string.IsNullOrWhiteSpace(model.Apellidos))
            {
                mensaje = "El Apellido del usuario no puede ser vacio";
            }
            else if(string.IsNullOrEmpty(model.Correo) || string.IsNullOrWhiteSpace(model.Correo))
            {

                mensaje = "El Correo del usuario no puede ser vacio";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                //Enviar Correo al Usuario

                string clave = Recursos.GenerarClave();

                string asunto = "Creacion Cuenta";
                string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave", clave);

                bool respuesta = Recursos.EnviarCorreo(model.Correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    model.Clave = Recursos.ConvertSha256(clave);
                    return usuarioDb.Registrar(model, out mensaje);
                }
                else
                {
                    mensaje = "No se puede enviar el correo";
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public bool Editar(Usuario model, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(model.Nombres) || string.IsNullOrWhiteSpace(model.Nombres))
            {
                mensaje = "El Nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(model.Apellidos) || string.IsNullOrWhiteSpace(model.Apellidos))
            {
                mensaje = "El Apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(model.Correo) || string.IsNullOrWhiteSpace(model.Correo))
            {

                mensaje = "El Correo del usuario no puede ser vacio";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                return usuarioDb.Editar(model, out mensaje);
            }
            else
            {
                return false;
            }
        }
        public bool Eliminar(int id, out string mensaje)
        {
            return usuarioDb.Eliminar(id, out mensaje);
        }

        public bool CambiarClave(int id, string nuevaclave, out string mensaje)
        {
            return usuarioDb.CambiarClave(id, nuevaclave, out mensaje);
        }
        public bool ReestablecerClave(int id, string correo, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaclave = Recursos.GenerarClave();
            bool resultado = usuarioDb.ReestableClave(id, Recursos.ConvertSha256(nuevaclave), out mensaje);
            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave", nuevaclave);

                //Enviar Correo al Usuario              
                bool respuesta = Recursos.EnviarCorreo(correo, asunto, mensaje_correo);
                if (respuesta)
                {
                    return true;
                }
                else
                {
                    mensaje = "No se puede enviar el correo";
                    return false;
                }
            }
            else
            {
                mensaje = "No se puede reestablecer la contraseña";
                return false;
            }
        }
    }
}