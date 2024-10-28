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
        public IHttpActionResult Login(AutenticarModel usuario)
        {
            Dictionary<string, bool> resultado = new Dictionary<string, bool>();

            bool autenticacion = ControlCuenta.Login(usuario.nombre_usuario, usuario.contraseña);
            resultado.Add("Resultado", autenticacion);
            resultado.Add("id_cuenta", id_cuenta);

            if (autenticacion)
                return Ok(resultado);

            return NotFound();
        }
    }
}