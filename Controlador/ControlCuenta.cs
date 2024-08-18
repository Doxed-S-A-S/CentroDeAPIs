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
        public static void CrearCuenta(string nombreUsuario, string email)
        {
            ModeloCuenta cuenta = new ModeloCuenta();
            cuenta.nombre_usuario = nombreUsuario;
            cuenta.email = email;

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
            if (u.ObtenerDatosUsuario(Int32.Parse(id)))
            {
                usuario.Add("resultado", "true");
                usuario.Add("id_usuario", u.id_cuenta.ToString());
                usuario.Add("nombre_usuario", u.nombre_usuario);
                usuario.Add("nombre", u.nombre);
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
    }
}