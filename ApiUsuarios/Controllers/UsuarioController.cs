using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Controlador;
using System.Data;
using ApiUsuarios.Models;

namespace ApiUsuario.Controllers
{
    public class UsuarioController : ApiController
    {

        [Route("ApiUsuarios/usuarios")]
        public List<UsuarioModel> Get()
        {
            DataTable usuarios = ControlCuenta.ListarCuentas();

            List<UsuarioModel> ListaDeGrupos = new List<UsuarioModel>();

            foreach (DataRow usuario in usuarios.Rows)
            {
                UsuarioModel u = new UsuarioModel();
   
                u.nombre_usuario = usuario["Usuario"].ToString();
                u.email = usuario["Correo"].ToString();
                

                ListaDeGrupos.Add(u);
            }
            return ListaDeGrupos;
        }

        [Route("LinguaLink/usuarios/")]
        public IHttpActionResult Post(UsuarioModel usuario)
        {
            ControlCuenta.CrearCuenta(usuario.nombre_usuario, usuario.email);
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            resultado.Add("mensaje", "usuario creado");
            return Ok(resultado);
        }






    }
}