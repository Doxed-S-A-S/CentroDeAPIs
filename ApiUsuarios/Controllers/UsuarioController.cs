using APIPost.Models;
using ApiUsuarios.Controllers;
using ApiUsuarios.DTO;
using ApiUsuarios.Models;
using Controlador;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ApiUsuario.Controllers
{
    public class UsuarioController : ApiController
    {

        [Route("ApiUsuarios/cuenta/ObtenerInformacion/{id_cuenta:int}")]
        [HttpGet]
        public UsuarioModel GetInfoCuenta(int id_cuenta)
        {
            try
            {
                DataTable cuenta = ControlCuenta.ObtenerInfoDeCuenta(id_cuenta.ToString());
                HttpRequest request = HttpContext.Current.Request;
                string baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{request.ApplicationPath.TrimEnd('/')}/";

                if (cuenta.Rows.Count > 0)
                {
                    DataRow row = cuenta.Rows[0];

                    UsuarioModel g = new UsuarioModel
                    {
                        nombre_usuario = row["nombre_usuario"].ToString(),
                        imagen_perfil = baseUrl + row["imagen_perfil"].ToString(),
                        rol_cuenta = row["rol_cuenta"].ToString(),
                        id_muro = Convert.ToInt32(row["id_muro"]),
                        id_preferencia = Convert.ToInt32(row["id_preferencia"].ToString()),
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

        [Route("ApiUsuarios/cuenta/ObtenerInformacion/Muro/{id_muro:int}")]
        [HttpGet]
        public UsuarioModel GetInfoMuro(int id_muro)
        {
            try
            {
                DataTable muro = ControlCuenta.obtenerDatosDelMuro(id_muro.ToString());
                HttpRequest request = HttpContext.Current.Request;
                string baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{request.ApplicationPath.TrimEnd('/')}/";

                if (muro.Rows.Count > 0)
                {
                    DataRow row = muro.Rows[0];

                    UsuarioModel g = new UsuarioModel
                    {
                        detalles = row["detalles"].ToString(),
                        pub_destacada = Convert.ToInt32(row["pub_destacada"]),
                        biografia = row["biografia"].ToString(),
                        imagen_banner = baseUrl + row["imagen_banner"],
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

        [Route("ApiUsuarios/obtenerDatosComentadores/{id_comentario:int}")]
        [HttpGet]
        public CuentaForCommentModel obtenerCreadorComentarioYSuFoto(int id_comentario)
        {
            try
            {
                DataTable cuentas = ControlComentarios.obtenerCreadorComentarioYSuFoto(id_comentario.ToString());

                if (cuentas.Rows.Count == 0)
                {
                    return null;
                }

                DataRow cuenta = cuentas.Rows[0];

                HttpRequest request = HttpContext.Current.Request;
                string baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{request.ApplicationPath.TrimEnd('/')}/";

                CuentaForCommentModel u = new CuentaForCommentModel
                {
                    id_cuenta = cuenta["id_cuenta"].ToString(),
                    nombre_usuario = cuenta["nombre_usuario"].ToString(),
                    imagen_perfil = baseUrl + cuenta["imagen_perfil"].ToString()
                };

                return u;
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
            string fileUrl_imagenperfil = "";
            string fileUrl_imagenbanner = "";
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
                //string detalles = httpRequest.Form["detalles"];
                string biografia = httpRequest.Form["biografia"];

                HttpPostedFile imagen_perfil = httpRequest.Files["imagen_perfil"];
                HttpPostedFile imagen_banner = httpRequest.Files["imagen_banner"];

                if (imagen_perfil != null && imagen_perfil.ContentLength > 0)
                {
                    try
                    {
                        string file_imagenperfil = imagen_perfil.FileName;
                        filePath = HttpContext.Current.Server.MapPath($"~/Uploads/{file_imagenperfil}");
                        imagen_perfil.SaveAs(filePath);
                        HttpRequest request = HttpContext.Current.Request;
                        string baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{request.ApplicationPath.TrimEnd('/')}/";
                        fileUrl_imagenperfil = $"Uploads/{file_imagenperfil}";

                        string file_imagenbanner = imagen_banner.FileName;
                        filePath = HttpContext.Current.Server.MapPath($"~/Uploads/{file_imagenbanner}");
                        imagen_perfil.SaveAs(filePath);
                        fileUrl_imagenbanner = $"Uploads/{file_imagenbanner}";
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }

                ControlCuenta.CrearCuenta(nombre_usuario, email, contraseña, nombre, apellido, apellido2, pais, idiomaHablado, fileUrl_imagenperfil, fileUrl_imagenbanner, biografia);
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
        [Route("ApiUsuarios/AnadirAmigoo/{id_cuenta:int}/{id_cuenta2:int}/{vinculo}")]
        [HttpPost]
        public IHttpActionResult AñadirAmigo(int id_cuenta, int id_cuenta2, string vinculo)
        {
            try
            {
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                ControlCuenta.AñadirAmigo(id_cuenta.ToString(),id_cuenta2.ToString(),vinculo);
                resultado.Add("mensaje", "Amigo añadido");
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

        [Route("ApiUsuarios/cuenta/GetRelacionados/{id_cuenta:int}")]
        [HttpGet]
        public List<VinculadoDTO> GetRelacionados(int id_cuenta)
        {
            List<VinculadoDTO> usuariosRelacionados = new List<VinculadoDTO>();

            try
            {
                DataTable cuenta = ControlCuenta.UsuariosRelacionados(id_cuenta.ToString());

                foreach (DataRow row in cuenta.Rows)
                {
                    VinculadoDTO v = new VinculadoDTO
                    {
                        nombre_usuario2 = row["Nombre"].ToString(),
                        vinculo = row["vinculo"].ToString(),
                        id_cuenta2 = Convert.ToInt32(row["ID vinculo"].ToString())
                    };
                    usuariosRelacionados.Add(v);
                }

                return usuariosRelacionados;
            }
            catch (Exception e)
            {
                // En caso de error, devolver una lista vacía en lugar de null
                return new List<VinculadoDTO>();
            }
        }



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
            catch (Exception ex)
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
        public IHttpActionResult ModificarContraseña(int id, PassModel usuario)
        {
            try
            {
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                    
                bool existe = ControlCuenta.ModificarContraseña(id.ToString(), usuario.contraseña , usuario.contraseñaAntigua);

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
