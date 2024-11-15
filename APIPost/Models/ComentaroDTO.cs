using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPost.Models
{
    public class ComentarioDTO
    {
        public int id_comentario;
        public int id_Post;
        public string contenido;
        public int likes;
        public string fecha;

    }
}