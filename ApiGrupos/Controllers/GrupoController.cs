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
        public List<GrupoModel> Get()
        {
            DataTable grupos = ControlGrupo.ObtenerGrupos();

            List<GrupoModel> ListaDeGrupos = new List<GrupoModel>();

            foreach (DataRow grupo in grupos.Rows)
            {
                GrupoModel g = new GrupoModel();
                g.id_grupo = Int32.Parse(grupo["id_grupo"].ToString());
                g.nombre = grupo["nombre"].ToString();

                ListaDeGrupos.Add(g);
            }
            return ListaDeGrupos;
        }






    }
}