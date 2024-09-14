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
        [Route("ApiPost/post/crear/")]
        public IHttpActionResult CrearPost(PostModel post)
        {
            ControlPosts.CrearPost(post.contenido, post.url_contenido, post.tipo_contenido, post.id_cuenta.ToString());
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            resultado.Add("mensaje", "post creado");
            return Ok(resultado);
        }

        [Route("ApiPost/evento/crear/")]
        [HttpPost]
        public IHttpActionResult CrearEvento(PostModel evento)
        {
            ControlPosts.CrearEvento(evento.nombre_evento, evento.imagen, evento.descripcion_evento, evento.contenido, evento.url_contenido, evento.tipo_contenido, evento.id_cuenta.ToString());
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            resultado.Add("mensaje", "evento creado");
            return Ok(resultado);
        }
        

        [Route("ApiPost/post/modificar-post/{idPost:int}")]
        [HttpPut]
        public IHttpActionResult ModificarPost(int idPost, PostModel post)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            ControlPosts.ModificarPost(idPost.ToString(), post.contenido, post.url_contenido, post.tipo_contenido);

            resultado.Add("Contenido", post.contenido);
            resultado.Add("url", post.url_contenido);
            resultado.Add("tipo_contenido", post.tipo_contenido);
            return Ok(resultado);
        }

        [Route("ApiPost/evento/modificar-evento{idPost:int}")]
        [HttpPut]

        public IHttpActionResult ModificarEvento(int idPost, PostModel evento)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            ControlPosts.ModificarEvento(idPost.ToString(), evento.url_contenido, evento.tipo_contenido, evento.contenido, evento.nombre_evento, evento.imagen, evento.descripcion_evento);

            resultado.Add("url", evento.url_contenido);
            resultado.Add("tipo_contenido", evento.tipo_contenido);
            resultado.Add("contenido", evento.contenido);
            resultado.Add("nombre_evento", evento.nombre_evento);
            resultado.Add("imagen", evento.imagen);
            resultado.Add("descripcion_evento", evento.descripcion_evento);

            return Ok(resultado);
        }


        [Route("ApiPost/post/eliminar-post/{idPost:int}")]
        [HttpDelete]
        public IHttpActionResult EliminarPost(int idPost)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            ControlPosts.ElimiarPost(idPost.ToString());
            resultado.Add("Resultado", "Post eliminado");
            return Ok(resultado);
        }
    }
}