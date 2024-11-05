using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Controlador;
using System.Data;
using ApiUsuarios.Models;
using ApiUsuarios.DTO;
using System.Web;

namespace ApiUsuario.Controllers
{
    public class UsuarioController : ApiController
    {

        [Route("ApiUsuarios/ListarUsuarios")]
        [HttpGet]
        public List<ListarCuentasDto> ListarUsuarios()
        {
            try
            {
                DataTable usuarios = ControlCuenta.ListarCuentas();

                List<ListarCuentasDto> ListaDeGrupos = new List<ListarCuentasDto>();

                foreach (DataRow usuario in usuarios.Rows)
                {
                    ListarCuentasDto u = new ListarCuentasDto();

                    u.id_cuenta = usuario["ID"].ToString();
                    u.nombre_usuario = usuario["Usuario"].ToString();
                    u.rol_cuenta = usuario["Rol"].ToString();
                    u.miembro_desde = usuario["Miembro desde"].ToString();


                    ListaDeGrupos.Add(u);
                }
                return ListaDeGrupos;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Route("ApiUsuarios/CrearCuenta/")]
        [HttpPost]
        public IHttpActionResult CrearCuenta()
        {
            string filePath = "";
            string fileUrl = "";
            try
            {
                HttpRequest httpRequest = HttpContext.Current.Request;

                string nombre_usuario = httpRequest.Form["nombre_usuario"];
                string email = httpRequest.Form["email"];
                string nombre = httpRequest.Form["nombre"];
                string apellido = httpRequest.Form["apellido"];
                string apellido2 = httpRequest.Form["apellido2"];
                string pais = httpRequest.Form["pais"];
                string idiomaHablado = httpRequest.Form["idiomaHablado"];
                string contraseña = httpRequest.Form["contrasena"];

                HttpPostedFile postedFile = httpRequest.Files["imagen_perfil"];

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

                ControlCuenta.CrearCuenta( nombre_usuario, email, contraseña, nombre, apellido, apellido2, pais, idiomaHablado, fileUrl);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created, "Cuenta Creada"));
            }
            catch (Exception ex)
            {
                if (ex.Message == "DUPLICATE_ENTRY")
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Conflict, "La cuenta ya existe"));
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

        /*[Route("ApiUsuarios/Usuarios/verificar/")]
        [HttpPut]
        public IHttpActionResult VerificarUser(UsuarioModel usuario)
        {
            try
            {
                bool existe = ControlCuenta.Login(usuario.nombre_usuario, usuario.contraseña);
                Dictionary<string, string> resultado = new Dictionary<string, string>();

                if (existe)
                {
                    resultado.Add("Mensaje", "Login aprobado");
                    return Ok(resultado);
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
        }*/
        [Route("ApiUsuarios/usuarios/usernameCheck/{username}")]
        [HttpGet]
        public IHttpActionResult PreferenciasGet(string username)
        {
            try
            {
                UsuarioModel usuario = new UsuarioModel();
                Dictionary<string, string> resultado = ControlCuenta.UsernameExiste(username);

                if (resultado["resultado"] == "true")
                {
                    return Ok("El username ya esta en uso");
                }
                return NotFound();

            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }
        [Route("ApiUsuarios/usuarios/preferencias/{idCuenta:int}")]
        [HttpGet]
        public IHttpActionResult PreferenciasGet(int idCuenta)
        {
            try
            {
                PreferenciasModel preferencias = new PreferenciasModel();
                Dictionary<string, string> resultado = ControlCuenta.BuscarPreferencia(idCuenta.ToString());

                if (resultado["resultado"] == "true")
                {
                    preferencias.tema_de_apariencia = resultado["tema de apariencia"];
                    preferencias.idioma_app = resultado["idioma"];
                    preferencias.preferencias_contenido = resultado["preferencias"];
                    preferencias.recordar_contraseña = bool.Parse(resultado["recordar contraseña"]);
                    preferencias.notificaciones_push = bool.Parse(resultado["notificaciones push"]);
                    preferencias.muro_privado = bool.Parse(resultado["muro privado"]);
                    return Ok(preferencias);
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

        [Route("ApiUsuarios/usuarios/preferencias/{idCuenta:int}")]
        [HttpPut]
        public IHttpActionResult PreferenciasPut(int idCuenta, PreferenciasModel Pref)
        {
            try
            {
                Dictionary<string, string> preferencias = new Dictionary<string, string>();
                bool existe = ControlCuenta.ModificarPreferencias(idCuenta.ToString(), Pref.idioma_app, Pref.recordar_contraseña, Pref.preferencias_contenido,
                    Pref.notificaciones_push, Pref.muro_privado, Pref.tema_de_apariencia);
                if (existe)
                {
                    preferencias.Add("resultado", "true");
                    preferencias.Add("tema de apariencia", Pref.tema_de_apariencia);
                    preferencias.Add("idioma", Pref.idioma_app);
                    preferencias.Add("preferencias", Pref.preferencias_contenido);
                    preferencias.Add("recordar contraseña", Pref.recordar_contraseña.ToString());
                    preferencias.Add("notificaciones push", Pref.notificaciones_push.ToString());
                    preferencias.Add("muro privado", Pref.muro_privado.ToString());
                    return Ok(preferencias);
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

        [Route("ApiUsuarios/usuarios/Pass/{id:int}")]
        [HttpPost]
        public IHttpActionResult ModificarContraseña(int id, UsuarioModel usuario)
        {
            try
            {
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                bool existe = ControlCuenta.ModificarContraseña(id.ToString(), usuario.contraseña, usuario.contraseñaAntigua);

                if (existe)
                {
                    resultado.Add("mensaje", "Contraseña modificada");
                    return Ok(resultado);
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



        [Route("ApiUsuarios/usuarios/Correo/{id:int}")]
        [HttpPost]
        public IHttpActionResult ModificarCorreo(int id, UsuarioModel usuario)
        {
            try
            {
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                bool existe = ControlCuenta.ModificarCorreo(id.ToString(), usuario.email);

                if (existe)
                {
                    resultado.Add("mensaje", "Correo modificado");
                    return Ok(resultado);
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
    }
}
