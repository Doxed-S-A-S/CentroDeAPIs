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

namespace ApiGrupos.Controllers
{
    public class GrupoController : ApiController
    {

        [Route("ApiGrupos/grupos")]
        public List<GetGruposDTO> GetGrupos()
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

        [Route("ApiGrupos/grupo/{id_grupo:int}/integrantes")]

        public List<GetIntegrantesDTO> GetIntegrantes(int id_grupo)
        {
            DataTable integrantes = ControlGrupo.ObtenerIntegrantesDeGrupo(id_grupo);

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



        [Route("ApiGrupos/grupo/")]
        public IHttpActionResult PostCrearGrupo(GrupoModel grupo)
        {
            ControlGrupo.CrearGrupo(grupo.nombre_grupo, grupo.descripcion, grupo.banner);
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            resultado.Add("mensaje", "grupo creado");
            return Ok(resultado);
        }
        
        [Route("ApiGrupos/grupo/agregarCuenta")]
        public IHttpActionResult PostAgregarCuentaEnGrupo(string rol, int id_grupo, int id_cuenta)
        {

            Dictionary<string, string> g = ControlGrupo.AgregarCuentaEnGrupo(rol, id_grupo.ToString(), id_cuenta.ToString());
            if (g["resultado"] == "true")
            {
                return Ok();
            }
            return Ok();
        }

        [Route("ApiGrupos/grupo/{id_grupo:int}")]
        public IHttpActionResult Put(int id_grupo, GrupoModel grupo)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            bool existe = ControlGrupo.ModificarGrupo(id_grupo.ToString(), grupo.nombre_grupo, grupo.descripcion, grupo.banner);

            if (existe)
            {
                resultado.Add("mensaje", "grupo creado");
                return Ok(resultado);
            }

            return NotFound();

        }




    }
}