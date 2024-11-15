using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Controlador;
using System.Data;
using ApiGrupos.Models;
using ApiGrupos.DTO;
using System.Web;

namespace ApiGrupos.Controllers
{
    public class GrupoController : ApiController
    {

        [Route("ApiGrupos/grupos")]
        [HttpGet]
        public List<GetGruposDTO> GetGrupos()
        {
            try
            {
                DataTable grupos = ControlGrupo.ObtenerGrupos();

                List<GetGruposDTO> ListaDeGrupos = new List<GetGruposDTO>();

                foreach (DataRow grupo in grupos.Rows)
                {
                    GetGruposDTO g = new GetGruposDTO();
                    g.id_grupo = Int32.Parse(grupo["id_grupo"].ToString());
                    g.nombre_grupo = grupo["nombre_grupo"].ToString();

                    ListaDeGrupos.Add(g);
                }
                return ListaDeGrupos;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        [Route("ApiGrupos/conforma_grupos/{id_cuenta:int}")]
        [HttpGet]
        public List<GetGruposDTO> GetGruposConformadosPorUsuario(int id_cuenta)
        {
            try
            {
                DataTable grupos = ControlGrupo.ObtenerGruposQueConformaUsuario(id_cuenta.ToString());

                List<GetGruposDTO> ListaDeGrupos = new List<GetGruposDTO>();

                foreach (DataRow grupo in grupos.Rows)
                {
                    GetGruposDTO g = new GetGruposDTO();
                    g.id_grupo = Int32.Parse(grupo["id_grupo"].ToString());
                    g.nombre_grupo = grupo["nombre_grupo"].ToString();

                    ListaDeGrupos.Add(g);
                }
                return ListaDeGrupos;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        [Route("ApiGrupos/grupo/{id_grupo:int}")]
        [HttpGet]
        public GetGrupoDTO GetGrupo(int id_grupo)
        {
            try
            {
                DataTable grupo = ControlGrupo.ObtenerGrupo(id_grupo.ToString());
                HttpRequest request = HttpContext.Current.Request;
                string baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{request.ApplicationPath.TrimEnd('/')}/";

                if (grupo.Rows.Count > 0)
                {
                    DataRow row = grupo.Rows[0];

                    GetGrupoDTO g = new GetGrupoDTO
                    {
                        nombre_grupo = row["nombre_grupo"].ToString(),
                        descripcion = row["descripcion"].ToString(),
                        url_imagen = baseUrl+row["url_imagen"].ToString(),
                        imagen_banner = baseUrl+row["imagen_banner"].ToString()
                    };
                    return g;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [Route("ApiGrupos/grupo/{id_grupo:int}/integrantes")]
        [HttpGet]
        public List<GetIntegrantesDTO> GetIntegrantes(int id_grupo)
        {
            try
            {
                DataTable integrantes = ControlGrupo.ObtenerIntegrantesDeGrupo(id_grupo.ToString());

                List<GetIntegrantesDTO> ListaDeIntegrantes = new List<GetIntegrantesDTO>();

                foreach (DataRow integrante in integrantes.Rows)
                {
                    GetIntegrantesDTO g = new GetIntegrantesDTO();

                    g.nombre_grupo = integrante["nombre_grupo"].ToString();
                    g.nombre_usuario = integrante["nombre_usuario"].ToString();
                    g.rol = integrante["rol"].ToString();

                    ListaDeIntegrantes.Add(g);
                }
                return ListaDeIntegrantes;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        [Route("ApiGrupos/grupo/crear/{idCuenta:int}")]
        [HttpPost]
        public IHttpActionResult CrearGrupo(int idCuenta)
        {
            string filePath = "";
            string imagenPerfilURL = "";
            string imagenBannerlURL = "";
            HttpRequest request = HttpContext.Current.Request;
            string baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{request.ApplicationPath.TrimEnd('/')}/";
            try
            {
                HttpRequest httpRequest = HttpContext.Current.Request;

                string nombre_grupo = httpRequest.Form["nombre_grupo"];
                string descripcion = httpRequest.Form["descripcion"];
                string privacidad = httpRequest.Form["privacidad"];

                HttpPostedFile url_imagen_file = httpRequest.Files["url_imagen"];
                HttpPostedFile imagen_banner_file = httpRequest.Files["imagen_banner"];

                string url_imagen = url_imagen_file.FileName;
                filePath = HttpContext.Current.Server.MapPath($"~/Uploads/{url_imagen}");
                url_imagen_file.SaveAs(filePath);
                imagenPerfilURL = $"Uploads/{url_imagen}";


                string imagen_banner = imagen_banner_file.FileName;
                filePath = HttpContext.Current.Server.MapPath($"~/Uploads/{imagen_banner}");
                imagen_banner_file.SaveAs(filePath);
                imagenBannerlURL = $"Uploads/{imagen_banner}";

                ControlGrupo.CrearGrupo(idCuenta.ToString(), nombre_grupo, descripcion, privacidad, imagenBannerlURL, imagenPerfilURL);
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                resultado.Add("mensaje", "grupo creado");
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

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


        [Route("ApiGrupos/grupo/{idGrupo:int}/privacidad")]
        [HttpPut]
        public IHttpActionResult ModificarPrivacidad(GrupoModel grupo, int idGrupo)
        {
            try
            {
                ControlGrupo.ModificarPrivacidadGrupo(idGrupo.ToString(), grupo.privacidad);
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                resultado.Add("mensaje", "privacidad cambiada");
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



        [Route("ApiGrupos/grupo/{id_grupo:int}/agregarCuenta")]

        [HttpPost]
        public IHttpActionResult AgregarCuentaEnGrupo(int id_grupo, AgregarCuentaDto a)
        {
            try
            {
                var resultado = ControlGrupo.AgregarCuentaEnGrupo(a.rol, id_grupo.ToString(), a.id_cuenta.ToString());

                if (resultado["resultado"] == "true")
                {
                    string mensajeOK = "cuenta agregada al grupo con exito";
                    return Ok(mensajeOK);
                }
                string mensajeError = "la cuenta ya esta agregada a este grupo";
                return BadRequest(mensajeError);
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

        [Route("ApiGrupos/grupo/{id_grupo:int}/modificar-grupo")]
        [HttpPut]
        public IHttpActionResult ModificarGrupo(int id_grupo, GrupoModel grupo)
        {
            try
            {
                bool existe = ControlGrupo.ModificarGrupo(id_grupo.ToString(), grupo.nombre_grupo, grupo.descripcion, grupo.imagen_banner, grupo.url_imagen);

                if (existe)
                {
                    return Ok("Grupo modificado con éxito");
                }

                return NotFound();
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



        [Route("ApiGrupos/grupo/{id_grupo:int}/cambiar-rol")]
        [HttpPut]

        public IHttpActionResult CambiarRolDeCuentaEnGrupo(ModificarRolDeCuentaEnGrupoDTO grupo, int id_grupo)
        {
            try
            {
                var resultado = ControlGrupo.CambiarRolDeCuentaEnGrupo(grupo.id_cuenta.ToString(), id_grupo.ToString(), grupo.rol);

                if (resultado["resultado"] == "true")
                {
                    return Ok("Rol cambiado");
                }

                string mensajeError = "No existe la cuenta indicada en el grupo indicado";
                return BadRequest(mensajeError);
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



        [Route("ApiGrupos/grupo/{id_grupo:int}/eliminar")]
        [HttpDelete]
        public IHttpActionResult DeleteGrupo(int id_grupo)
        {
            try
            {
                var resultado = ControlGrupo.EliminarGrupo(id_grupo.ToString());

                if (resultado == true)
                {
                    return Ok("Grupo eliminado");
                }

                string mensajeError = "El grupo no existe";
                return BadRequest(mensajeError);
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


        [Route("ApiGrupos/grupo/{id_grupo:int}/cuenta/{id_cuenta:int}")]
        [HttpDelete]
        public IHttpActionResult DeleteCuentaDeGrupo(int id_grupo, int id_cuenta)
        {
            try
            {
                var resultado = ControlGrupo.EliminarCuentaDeGrupo(id_grupo.ToString(), id_cuenta.ToString());

                if (resultado["resultado"] == "true")
                {
                    return Ok("Cuenta eliminada del grupo con éxito");
                }

                string mensajeError = "La cuenta no existe en este grupo";
                return BadRequest(mensajeError);
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

        [Route("ApiGrupos/grupo/{idGrupo:int}/report")]
        [HttpPut]
        public IHttpActionResult AñadirReporte(int idGrupo)
        {
            try
            {
                ControlGrupo.AñadirReportGrupo(idGrupo.ToString());
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                resultado.Add("mensaje", "grupo reportado");
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

    }




}


/*
 * 
 *             catch (Exception ex)
            {
                if (ex.Message == "DUPLICATE_ENTRY")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "El grupo ya existe"));
                if (ex.Message == "ACCESS_DENIED")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized,"Acceso denegado"));
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

*/