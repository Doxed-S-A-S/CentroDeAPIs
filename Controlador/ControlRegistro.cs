using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Modelos;

namespace Controlador
{
    class ControlRegistro
    {
        public static void CrearUsuario(string id_usuario, string nombre, string apellido1, string apellido2, string pais)
        {
            ModeloRegistro registro = new ModeloRegistro();

            registro.id_usuario = Int32.Parse(id_usuario);
            registro.nombre = nombre;
            registro.apellido1 = apellido1;
            registro.apellido2 = apellido2;
            registro.pais = pais;

            registro.InsertarUsuario();
        }

        public static void EliminarUsuario(string id_usuario)
        {
            ModeloRegistro registro = new ModeloRegistro();
            registro.id_usuario= Int32.Parse(id_usuario);
            registro.EliminarUsuario();
        }

    }
}
