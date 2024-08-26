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

        [Route("LinguaLink/post/{idPost:int}")]
        [HttpPut]
        public IHttpActionResult ModificarPost(int idPost,PostModel post)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            ControlPosts.ModificarPost(idPost.ToString(), post.Contenido, post.url_contenido, post.Tags);

            resultado.Add("Contenido", post.Contenido);
            resultado.Add("url", post.url_contenido);
            resultado.Add("Tags", post.Tags);
            return Ok(resultado);
        }

        [Route("LinguaLink/post/{idPost:int}")]
        [HttpDelete]
        public IHttpActionResult EliminarPost(int idPost)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            ControlPosts.ElimiarPost(idPost.ToString());
            resultado.Add("Resultado", "Post eliminado");
            return Ok(resultado);
        }
}