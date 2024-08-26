using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Controlador;
using System.Data;
using APIPost.Models;

namespace APIPost.Controllers
{
    public class PostController : ApiController
    {
        [Route("LinugaLink/post/crear/")]
        public IHttpActionResult CrearPost(PostModel post)
        {
            ControlPosts.CrearPost(post.Contenido,post.url_contenido,post.Tags, post.Id_Cuenta.ToString());
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            resultado.Add("mensaje", "post creado");
            return Ok(resultado);
        }
    }
}