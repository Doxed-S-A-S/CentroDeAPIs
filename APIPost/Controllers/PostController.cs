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
        [Route("ApiPost/post/obtener-posts/{id_cuenta:int}")]
        [HttpGet]
        public List<PostModel> ObtenerPostsDeUsuario(int id_cuenta)
        {
            DataTable tablaPosts = ControlPosts.Listar(id_cuenta.ToString());

            List<PostModel> posts = new List<PostModel>();

            foreach (DataRow post in tablaPosts.Rows)
            {
                PostModel p = new PostModel();
                p.Id_Post = Int32.Parse(post["Id_Post"].ToString());
                p.contenido = post["contenido"].ToString();
                p.id_cuenta = Int32.Parse(post["id_cuenta"].ToString());

                posts.Add(p);
            }
            return posts;
        }


        [Route("ApiPost/post/obtener-creador/{id_cuenta:int}")]
        [HttpGet]
        public IHttpActionResult ObtenerCreadorDePost(int id_cuenta)
        {
            string username = ControlPosts.ObtenerCreadorDePost(id_cuenta.ToString());
            return Ok(username);
        }


        [Route("ApiPost/post/crear/")]
        [HttpPost]
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
                string resultado =  "evento creado";
                return Ok(resultado);
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

        [Route("ApiPost/MostrarAlgoritmo")]
        [HttpGet]
        public Dictionary<string,string> TestingMuestraPost()
        {
            ControlPosts p = new ControlPosts();
            PostModel post = new PostModel();
            Dictionary<string, string> PostMuestra = p.AlgoritmoPost();
            
            post.contenido = PostMuestra["contenido"];
            post.tipo_contenido = PostMuestra["tipo_contenido"];
            post.Id_Post = Int32.Parse(PostMuestra["id_post"]);
            return PostMuestra;


        }



    }
}