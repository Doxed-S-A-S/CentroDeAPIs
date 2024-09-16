using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiGrupos.Models
{
    public class GrupoModel
    {

        public int id_grupo;
        public string nombre_grupo;
        public string descripcion;
        public string privacidad;
        public string banner;
        public string rol;

        public int id_cuenta;
        public string nombre_usuario;

    }
}