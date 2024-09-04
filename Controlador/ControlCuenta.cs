using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Modelos;

namespace Controlador

{
    public class ControlCuenta
    {
        public static void CrearCuenta(string nombreUsuario, string email, string contraseña)
        {
            ModeloCuenta cuenta = new ModeloCuenta();
            cuenta.nombre_usuario = nombreUsuario;
            cuenta.email = email;
            cuenta.contraseña = contraseña;
            cuenta.CrearCuenta();

        }

        public static void ModificarContraseña(string id, string contraseña)
        {
            ModeloCuenta cuenta = new ModeloCuenta();
            cuenta.id_cuenta = Int32.Parse(id);
            cuenta.contraseña = contraseña;

            cuenta.ModificarContraseña();
        }

        public static void ModificarCorreo(string id, string email)
        {
            ModeloCuenta cuenta = new ModeloCuenta();
            cuenta.id_cuenta = Int32.Parse(id);
            cuenta.email = email;

            cuenta.ModificarCorreo();
        }

        public static Dictionary<string, string> BuscarUsuario(string id)
        {

            Dictionary<string, string> usuario = new Dictionary<string, string>();
            ModeloCuenta u = new ModeloCuenta();
            if (u.ObtenerDatosDeCuenta(Int32.Parse(id)))
            {
                usuario.Add("resultado", "true");
                usuario.Add("id_usuario", u.id_cuenta.ToString());
                usuario.Add("nombre_usuario", u.nombre_usuario);
                usuario.Add("nombre_grupo", u.nombre);
                usuario.Add("apellido1", u.apellido1);
                usuario.Add("apellido2", u.apellido2);
                usuario.Add("email", u.email);
                usuario.Add("biografia", u.biografia);
                usuario.Add("reports", u.reports.ToString());
                return usuario;
            }
            usuario.Add("resultado", "false");
            return usuario;
        }

            public static DataTable ListarCuentas()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("id_cuenta", typeof(int));
            tabla.Columns.Add("Usuario", typeof(string));
            tabla.Columns.Add("Correo", typeof(string));

            ModeloCuenta cuenta = new ModeloCuenta();
            foreach (ModeloCuenta c in cuenta.ObtenerCuentas())
            {
                DataRow fila = tabla.NewRow();
                fila["id_cuenta"] = c.id_cuenta;
                fila["Usuario"] = c.nombre_usuario;
                fila["Correo"] = c.email;
                tabla.Rows.Add(fila);
            }
            return tabla;
        }

        public static bool ModificarPreferencias(string idCuenta,string idioma, Boolean recordarContraseña, string preferenciaContenido,
            Boolean notificacionPush, Boolean privacidad, string apariencia)
        {
            ModeloCuenta cuenta = new ModeloCuenta();
            if (cuenta.ObtenerDatosDeCuenta(Int32.Parse(idCuenta)))
            {
                cuenta.idioma_app = idioma;
                cuenta.recordar_contraseña = recordarContraseña;
                cuenta.preferencias_contenido = preferenciaContenido;
                cuenta.notificaciones_push = notificacionPush;
                cuenta.muro_privado = privacidad;
                cuenta.tema_de_apariencia = apariencia;
                cuenta.ModificarPreferencias();
                return true;
            }
            return false;
        }

        public static Dictionary<string,string> BuscarPreferencia(string idCuenta)
        {
            Dictionary<string, string> preferencia = new Dictionary<string, string>();
            ModeloCuenta cuenta = new ModeloCuenta();
            if (cuenta.BuscarPreferencias(Int32.Parse(idCuenta)))
            {
                preferencia.Add("resultado", "true");
                preferencia.Add("tema de apariencia", cuenta.tema_de_apariencia);
                preferencia.Add("idioma", cuenta.idioma_app);
                preferencia.Add("preferencias", cuenta.preferencias_contenido);
                preferencia.Add("recordar contraseña", cuenta.recordar_contraseña.ToString());
                preferencia.Add("notificaciones push", cuenta.notificaciones_push.ToString());
                preferencia.Add("muro privado", cuenta.muro_privado.ToString());
                return preferencia;
            }
            preferencia.Add("resultado", "true");
            return preferencia;
        }
    }
}


