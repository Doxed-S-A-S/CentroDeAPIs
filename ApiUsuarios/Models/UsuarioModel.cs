using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUsuarios.Models
{
    public class UsuarioModel
    {
        public string nombre_usuario;
        public string email;
        public string contraseña;
        public string contraseñaAntigua;
        public string rol_cuenta;

        public string nombre;
        public string apellido;
        public string apellido2;
        public string idiomaHablado;
        public string pais;
        public string imagen_perfil;

        public string detalles ;
        public int pub_destacada;
        public string biografia;
        public string imagen_banner;

        public int id_muro;
        public int id_preferencia;
    }
}