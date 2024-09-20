using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUsuarios.Models
{
    public class PreferenciasModel
    {
        public string tema_de_apariencia;
        public string idioma_app;
        public bool recordar_contraseña;
        public string preferencias_contenido;
        public bool notificaciones_push;
        public bool muro_privado;
    }
}