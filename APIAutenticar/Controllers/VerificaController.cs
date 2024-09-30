using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Controlador;
using System.Data;
using System.Threading.Tasks;
using APIAutenticar.Models;

namespace APIAutenticar.Controllers
{
    public class UsuarioController : ApiController
    {
        [Route("ApiAut/login")]
        public IHttpActionResult Login(AutenticarModel usuario)
        {
            Dictionary<string, bool> resultado = new Dictionary<string, bool>();

            bool autenticacion = UsuarioController.Login(usuario.nombre_usuario, usuario.contrasena);
            resultado.Add("Resultado", autenticacion);

            if (autenticacion)
                return Ok(resultado);

            return NotFound();
        }
    }
}