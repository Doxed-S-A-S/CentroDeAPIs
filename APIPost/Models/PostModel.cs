using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPost.Models
{
    public class PostModel
    {
        public int Id_Post;
        public string url_contenido;
        public string tipo_contenido;
        public string contenido;
        public int id_cuenta;
        public string url_imagen;
        public string fecha_creacion;
        public string comentario;
        public byte[] contenidoImagen;
        public int likes;

        public string nombre_evento;
        public string imagen ;
        public string descripcion_evento;
    }
}