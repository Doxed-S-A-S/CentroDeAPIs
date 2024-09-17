﻿using System;
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


        [Route("ApiPost/evento/crear")]
        [HttpPost]
        public IHttpActionResult CrearEvento(PostModel evento)
        {

            try
            {
                ControlPosts.CrearEvento(evento.nombre_evento, evento.imagen, evento.descripcion_evento, evento.contenido, evento.url_contenido, evento.tipo_contenido, evento.id_cuenta.ToString());
                string resultado = "evento creado";
                return Ok(resultado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("ApiPost/post/{idPost:int}/comentario")]
        [HttpPost]
        public IHttpActionResult CrearComentario(int idPost, PostModel post)
        {
            ControlComentarios.CrearComentario(post.id_cuenta.ToString(), idPost.ToString(), post.comentario);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created, "Comentario Creado"));
        }

        [Route("ApiPost/post/eliminar-comentario/{idComentario:int}")]
        [HttpDelete]
        public IHttpActionResult EliminarComentario(int idComentario, PostModel)
        {
            try
            {
                ControlComentarios.EliminarComentario(idComentario.ToString());
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created, "Comentario eliminado"));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [Route("ApiPost/post/modificar-comentario/{idComentario:int}")]
        [HttpPut]

        public IHttpActionResult ModificarComentario(int idComentario, PostModel comenatrio) 
        {
            try
            {
                ControlComentarios.ModificarComentario(idComentario.ToString(), comenatrio.id_cuenta.ToString());
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created, "Comentario modificado"));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

    

        [Route("ApiPost/post/compartir-en-muro/{id_post:int}/{id_muro:int}")]
        [HttpPost]
        public IHttpActionResult CompartirPostEnMuro(int id_post, int id_muro)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            ControlPosts.CompartirPostEnMuro(id_post.ToString(), id_muro.ToString());
            resultado.Add("Resultado", "Post compartido");
            return Ok(resultado);
        }

        [Route("ApiPost/post/compartir-en-muro/{id_post:int}/{id_muro:int}")]
        [HttpPost]
        public IHttpActionResult CompartirPostEnGrupo(int id_post, int id_grupo)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            ControlPosts.CompartirPostEnMuro(id_post.ToString(), id_grupo.ToString());
            resultado.Add("Resultado", "Post compartido");
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


        [Route("ApiPost/evento/modificar-evento/{id_evento:int}")]
        [HttpPut]

        public IHttpActionResult ModificarEvento(int id_evento, PostModel evento)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            ControlPosts.ModificarEvento(evento.Id_Post.ToString(), id_evento.ToString(), evento.url_contenido, evento.tipo_contenido, evento.contenido, evento.nombre_evento, evento.imagen, evento.descripcion_evento, evento.id_cuenta.ToString()); ;

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

        [Route("ApiPost/post/eliminar-evento/{id_evento:int}/{id_post:int}")]
        [HttpDelete]
        public IHttpActionResult EliminarEvento(int id_evento,int id_post)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            ControlPosts.ElimiarEvento(id_post.ToString(),id_evento.ToString());
            resultado.Add("Resultado", "Evento eliminado");
            return Ok(resultado);
        }



    }
}