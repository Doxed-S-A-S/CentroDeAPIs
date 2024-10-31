using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Threading.Tasks;
using Controlador;
using APIAutenticar.Models;


namespace APIAutenticar.Controllers
{
    public class UsuarioController : ApiController
    {
        [Route("ApiAut/login")]
        [HttpPost]
        public IHttpActionResult Login(AutenticarModel usuario)
        {
            AutenticarModel auth = new AutenticarModel();
            Dictionary<string, string> resultado = ControlCuenta.Login(usuario.nombre_usuario, usuario.contraseña);

            if (resultado["resultado"] == "True")
            {
                auth.result = resultado["resultado"];
                auth.ID = resultado["ID"];
                return Ok(resultado);
            }
            return NotFound();
        }
    }
}