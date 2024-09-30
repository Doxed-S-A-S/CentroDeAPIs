using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using APIAutenticar.Models;

namespace APIAutenticar.Controllers
{
    public class UsuarioController : VerificaController
    {
        [Route("ApiAut/login")]
        public IHttpActionResult Login(UserModel usuario)
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