using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Controlador;
using System.Data;
using APIPost.Models;
using System.Web;

namespace APIPost.Controllers
{
    public class PostController : ApiController


    {
        [Route("ApiPost/post/obtener-posts-usuario/{id_cuenta:int}")]
        [HttpGet]
        public List<PostModel> ObtenerPostsDeUsuario(int id_cuenta)
        {
            try
            {
                DataTable tablaPosts = ControlPosts.ListarPostDeCuenta(id_cuenta.ToString());

                List<PostModel> posts = new List<PostModel>();

                foreach (DataRow post in tablaPosts.Rows)
                {
                    PostModel p = new PostModel();
                    p.Id_Post = Int32.Parse(post["Id_Post"].ToString());
                    p.contenido = post["contenido"].ToString();
                    p.id_cuenta = Int32.Parse(post["id_cuenta"].ToString());
                    p.likes = Int32.Parse(post["Likes"].ToString());
                    string currentServer = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                    p.url_imagen = $"{currentServer}/{post["url_imagen"].ToString()}";
                    Console.Write(p.url_imagen);

                    posts.Add(p);
                }
                return posts;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        [Route("ApiPost/post/obtener-posts-grupo/{id_grupo:int}")]
        [HttpGet]
        public List<PostModel> ObtenerPostsDelGrupo(int id_grupo)
        {
            try
            {
                DataTable tablaPosts = ControlPosts.ListarPostDeGrupo(id_grupo.ToString());

                List<PostModel> posts = new List<PostModel>();

                foreach (DataRow post in tablaPosts.Rows)
                {
                    PostModel p = new PostModel();
                    p.Id_Post = Int32.Parse(post["Id_Post"].ToString());
                    p.contenido = post["contenido"].ToString();
                    p.id_cuenta = Int32.Parse(post["id_cuenta"].ToString());
                    p.likes = Int32.Parse(post["Likes"].ToString());
                    string currentServer = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                    p.url_imagen = $"{currentServer}/{post["url_imagen"].ToString()}";
                    Console.Write(p.url_imagen);

                    posts.Add(p);
                }
                return posts;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
                throw;
            }
        }

        [Route("ApiPost/post/obtener-posts")]
        [HttpGet]
        public List<PostModel> ObtenerPosts()
        {
            try
            {
                DataTable tablaPosts = ControlPosts.ListarTodosLosPost();

                List<PostModel> posts = new List<PostModel>();

                foreach (DataRow post in tablaPosts.Rows)
                {
                    PostModel p = new PostModel();
                    p.Id_Post = Int32.Parse(post["Id_Post"].ToString());
                    p.contenido = post["contenido"].ToString();
                    p.id_cuenta = Int32.Parse(post["id_cuenta"].ToString());
                    p.likes = Int32.Parse(post["Likes"].ToString());

                    string currentServer = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

                    p.url_imagen = $"{currentServer}/{post["url_imagen"].ToString()}";
                    Console.Write(p.url_imagen);
                    posts.Add(p);
                }
                return posts;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        [Route("ApiPost/post/obtener-creador/{id_cuenta:int}")]
        [HttpGet]
        public IHttpActionResult ObtenerCreadorDePost(int id_cuenta)
        {
            try
            {
                string username = ControlPosts.ObtenerCreadorDePost(id_cuenta.ToString());
                return Ok(username);
            }
            catch (Exception ex)
            {
                if (ex.Message == "DUPLICATE_ENTRY")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "El grupo ya existe"));
                if (ex.Message == "ACCESS_DENIED")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Acceso denegado"));
                if (ex.Message == "UNKNOWN_COLUMN")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Datos incorrectos"));
                if (ex.Message == "ERROR_CHILD_ROW")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al insertar id's"));
                if (ex.Message == "UNKNOWN_DB_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas con la base de datos"));
                if (ex.Message == "UNKNOWN_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas durante la ejecucion"));
                throw;
            }
        }

        [Route("ApiPost/post/crear/")]
        [HttpPost]
        public IHttpActionResult CrearPost()
        {
            string filePath = "";
            string fileUrl = "";
            try
            {
                HttpRequest httpRequest = HttpContext.Current.Request;

                string url_contenido = httpRequest.Form["url_contenido"];
                string tipo_contenido = httpRequest.Form["tipo_contenido"];
                string contenido = httpRequest.Form["contenido"];
                int id_cuenta = Convert.ToInt32(httpRequest.Form["id_cuenta"]);

                HttpPostedFile postedFile = httpRequest.Files["imagencita"];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    try
                    {
                        string fileName = postedFile.FileName;
                        filePath = HttpContext.Current.Server.MapPath($"~/Uploads/{fileName}");
                        postedFile.SaveAs(filePath);
                        HttpRequest request = HttpContext.Current.Request;
                        string baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{request.ApplicationPath.TrimEnd('/')}/";
                        fileUrl = $"Uploads/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }

                ControlPosts.CrearPost(contenido, url_contenido, fileUrl, tipo_contenido, id_cuenta.ToString());

                Dictionary<string, string> resultado = new Dictionary<string, string>();
                resultado.Add("mensaje", "post creado");
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                if (ex.Message == "DUPLICATE_ENTRY")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "El grupo ya existe"));
                if (ex.Message == "ACCESS_DENIED")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Acceso denegado"));
                if (ex.Message == "UNKNOWN_COLUMN")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Datos incorrectos"));
                if (ex.Message == "ERROR_CHILD_ROW")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al insertar id's"));
                if (ex.Message == "UNKNOWN_DB_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas con la base de datos"));
                if (ex.Message == "UNKNOWN_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas durante la ejecucion"));
                throw;
            }
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
            try
            {
                ControlComentarios.CrearComentario(post.id_cuenta.ToString(), idPost.ToString(), post.comentario);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created, "Comentario Creado"));
            }
            catch (Exception ex)
            {
                if (ex.Message == "DUPLICATE_ENTRY")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "El grupo ya existe"));
                if (ex.Message == "ACCESS_DENIED")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Acceso denegado"));
                if (ex.Message == "UNKNOWN_COLUMN")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Datos incorrectos"));
                if (ex.Message == "ERROR_CHILD_ROW")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al insertar id's"));
                if (ex.Message == "UNKNOWN_DB_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas con la base de datos"));
                if (ex.Message == "UNKNOWN_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas durante la ejecucion"));
                throw;
            }
        }


        [Route("ApiPost/post/{idPost:int}/comentario")]
        [HttpGet]
        public List<ComentarioDTO> ObtenerComentarios(int idPost)
        {
            try
            {
                DataTable comentarios = ControlComentarios.ListarComentarios(idPost.ToString());

                List<ComentarioDTO> coments = new List<ComentarioDTO>();

                foreach (DataRow coment in comentarios.Rows)
                {
                    ComentarioDTO c = new ComentarioDTO();
                    c.id_Post = Int32.Parse(coment["IdPost"].ToString());
                    c.id_comentario = Int32.Parse(coment["IdComentario"].ToString());
                    c.comentario = coment["Comentario"].ToString();
                    c.fecha = coment["Fecha de creacion"].ToString();
                    c.likes = Int32.Parse(coment["Likes"].ToString());

                    coments.Add(c);
                }
                return coments;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        [Route("ApiPost/post/comentario/{idComentario:int}")]
        [HttpDelete]
        public IHttpActionResult EliminarComentario(int idComentario)
        {
            try
            {
                ControlComentarios.EliminarComentario(idComentario.ToString());
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Gone, "Comentario eliminado"));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Route("ApiPost/post/comentario/{idComentario:int}")]
        [HttpPut]

        public IHttpActionResult ModificarComentario(int idComentario, PostModel comenatrio)
        {
            try
            {
                ControlComentarios.ModificarComentario(idComentario.ToString(), comenatrio.comentario);
                return Ok();
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
            try
            {
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                ControlPosts.CompartirPostEnMuro(id_post.ToString(), id_muro.ToString());
                resultado.Add("Resultado", "Post compartido");
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                if (ex.Message == "DUPLICATE_ENTRY")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "El grupo ya existe"));
                if (ex.Message == "ACCESS_DENIED")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Acceso denegado"));
                if (ex.Message == "UNKNOWN_COLUMN")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Datos incorrectos"));
                if (ex.Message == "ERROR_CHILD_ROW")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al insertar id's"));
                if (ex.Message == "UNKNOWN_DB_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas con la base de datos"));
                if (ex.Message == "UNKNOWN_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas durante la ejecucion"));
                throw;
            }
        }

        [Route("ApiPost/post/compartir-en-grupo/{id_post:int}/{id_grupo:int}")]
        [HttpPost]
        public IHttpActionResult CompartirPostEnGrupo(int id_post, int id_grupo)
        {
            try
            {
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                ControlPosts.CompartirPostEnGrupo(id_post.ToString(), id_grupo.ToString());
                resultado.Add("Resultado", "Post compartido");
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                if (ex.Message == "DUPLICATE_ENTRY")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "El grupo ya existe"));
                if (ex.Message == "ACCESS_DENIED")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Acceso denegado"));
                if (ex.Message == "UNKNOWN_COLUMN")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Datos incorrectos"));
                if (ex.Message == "ERROR_CHILD_ROW")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al insertar id's"));
                if (ex.Message == "UNKNOWN_DB_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas con la base de datos"));
                if (ex.Message == "UNKNOWN_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas durante la ejecucion"));
                throw;
            }
        }

        [Route("ApiPost/post/modificar-post/{idPost:int}")]
        [HttpPut]
        public IHttpActionResult ModificarPost(int idPost, PostModel post)
        {
            try
            {
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                ControlPosts.ModificarPost(idPost.ToString(), post.contenido, post.url_contenido, post.tipo_contenido);

                resultado.Add("Contenido", post.contenido);
                resultado.Add("url", post.url_contenido);
                resultado.Add("tipo_contenido", post.tipo_contenido);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                if (ex.Message == "DUPLICATE_ENTRY")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "El grupo ya existe"));
                if (ex.Message == "ACCESS_DENIED")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Acceso denegado"));
                if (ex.Message == "UNKNOWN_COLUMN")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Datos incorrectos"));
                if (ex.Message == "ERROR_CHILD_ROW")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al insertar id's"));
                if (ex.Message == "UNKNOWN_DB_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas con la base de datos"));
                if (ex.Message == "UNKNOWN_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas durante la ejecucion"));
                throw;
            }
        }


        [Route("ApiPost/evento/modificar-evento/{id_evento:int}")]
        [HttpPut]

        public IHttpActionResult ModificarEvento(int id_evento, PostModel evento)
        {
            try
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
            catch (Exception ex)
            {
                if (ex.Message == "DUPLICATE_ENTRY")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "El grupo ya existe"));
                if (ex.Message == "ACCESS_DENIED")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Acceso denegado"));
                if (ex.Message == "UNKNOWN_COLUMN")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Datos incorrectos"));
                if (ex.Message == "ERROR_CHILD_ROW")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al insertar id's"));
                if (ex.Message == "UNKNOWN_DB_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas con la base de datos"));
                if (ex.Message == "UNKNOWN_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas durante la ejecucion"));
                throw;
            }
        }


        [Route("ApiPost/post/eliminar-post/{idPost:int}")]
        [HttpDelete]
        public IHttpActionResult EliminarPost(int idPost)
        {
            try
            {
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                ControlPosts.ElimiarPost(idPost.ToString());
                resultado.Add("Resultado", "Post eliminado");
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                if (ex.Message == "DUPLICATE_ENTRY")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "El grupo ya existe"));
                if (ex.Message == "ACCESS_DENIED")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Acceso denegado"));
                if (ex.Message == "UNKNOWN_COLUMN")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Datos incorrectos"));
                if (ex.Message == "ERROR_CHILD_ROW")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al insertar id's"));
                if (ex.Message == "UNKNOWN_DB_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas con la base de datos"));
                if (ex.Message == "UNKNOWN_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas durante la ejecucion"));
                throw;
            }
        }

        [Route("ApiPost/post/eliminar-evento/{id_evento:int}/{id_post:int}")]
        [HttpDelete]
        public IHttpActionResult EliminarEvento(int id_evento, int id_post)
        {
            try
            {
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                ControlPosts.ElimiarEvento(id_post.ToString(), id_evento.ToString());
                resultado.Add("Resultado", "Evento eliminado");
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                if (ex.Message == "DUPLICATE_ENTRY")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "El grupo ya existe"));
                if (ex.Message == "ACCESS_DENIED")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Acceso denegado"));
                if (ex.Message == "UNKNOWN_COLUMN")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Datos incorrectos"));
                if (ex.Message == "ERROR_CHILD_ROW")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error al insertar id's"));
                if (ex.Message == "UNKNOWN_DB_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas con la base de datos"));
                if (ex.Message == "UNKNOWN_ERROR")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Problemas durante la ejecucion"));
                throw;
            }
        }




        [Route("ApiPost/MostrarAlgoritmo")]
        [HttpGet]
        public List<PostModel> TestingMuestraPost()
        {
            try
            {
                DataTable tablaPosts = ControlPosts.AlgoritmoPost();

                List<PostModel> posts = new List<PostModel>();

                foreach (DataRow post in tablaPosts.Rows)
                {
                    PostModel p = new PostModel();
                    p.Id_Post = Int32.Parse(post["Id_Post"].ToString());
                    p.contenido = post["contenido"].ToString();
                    p.id_cuenta = Int32.Parse(post["id_cuenta"].ToString());
                    p.url_contenido = post["url_contenido"].ToString();
                    p.url_imagen = post["url_imagen"].ToString();
                    p.fecha_creacion = post["fecha_creacion"].ToString();
                    p.likes = Int32.Parse(post["Likes"].ToString());

                    posts.Add(p);
                }
                return posts;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }



    }
}