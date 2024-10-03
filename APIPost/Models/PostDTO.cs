using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPost.Models
{
    public class PostDTO
    {
        public int Id_Post;
        public string url_contenido;
        public string tipo_contenido;
        public string contenido;
        public int id_cuenta;
        public string comentario;
        public int likes;
    }
}