using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Controlador;
using System.Data;
using ApiGrupos.Models;

namespace ApiGrupos.Controllers
{
    public class GrupoController : ApiController
    {

        [Route("ApiGrupos/grupo")]
        public List<GrupoModel> GetGrupos()
        {
            DataTable grupos = ControlGrupo.ObtenerGrupos();

            List<GrupoModel> ListaDeGrupos = new List<GrupoModel>();

            foreach (DataRow grupo in grupos.Rows)
            {
                GrupoModel g = new GrupoModel();
                g.id_grupo = Int32.Parse(grupo["id_grupo"].ToString());
                g.nombre_grupo = grupo["nombre"].ToString();

                ListaDeGrupos.Add(g);
            }
            return ListaDeGrupos;
        }

        [Route("ApiGrupos/grupo/{id_grupo:int}/integrantes")]
        public List<GrupoModel> GetIntegrantes()
        {
            DataTable integrantes = ControlGrupo.ObtenerIntegrantesDeGrupo();

            List<GrupoModel> ListaDeIntegrantes = new List<GrupoModel>();

            foreach (DataRow integrante in integrantes.Rows)
            {
                GrupoModel g = new GrupoModel();
                g.nombre_grupo = integrante["nombre_grupo"].ToString();
                g.nombre_usuario = integrante["nombre_usuario"].ToString();
                g.rol = integrante["rol"].ToString();

                ListaDeIntegrantes.Add(g);
            }
            return ListaDeIntegrantes;
        }



        [Route("ApiGrupos/grupo/")]
        public IHttpActionResult Post(GrupoModel grupo)
        {
            
            ControlGrupo.CrearGrupo(grupo.nombre_grupo, grupo.descripcion, grupo.banner);
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            resultado.Add("mensaje", "grupo creado");
            return Ok(resultado);
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