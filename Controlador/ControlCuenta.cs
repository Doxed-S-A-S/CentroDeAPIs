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


