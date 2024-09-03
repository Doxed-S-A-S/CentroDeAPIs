﻿using System;
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
            DataTable integrantes = ControlGrupo.ObtenerIntegrantesDeGrupo(id_grupo.ToString());

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



        [Route("ApiGrupos/grupo")]
        public IHttpActionResult PostCrearGrupo(GrupoModel grupo)
        {
            ControlGrupo.CrearGrupo(grupo.nombre_grupo, grupo.descripcion, grupo.banner);
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            resultado.Add("mensaje", "grupo creado");
            return Ok(resultado);
        }



        [Route("ApiGrupos/grupo/{id_grupo:int}/agregarCuenta")]
        public IHttpActionResult PostAgregarCuentaEnGrupo(int id_grupo, AgregarCuentaDto a)
        {

            var resultado = ControlGrupo.AgregarCuentaEnGrupo(a.rol, id_grupo.ToString(), a.id_cuenta.ToString());

            if (resultado["resultado"] == "true")
            {
                string mensajeOK = "cuenta agregada al grupo con exito";
                return Ok(mensajeOK);
            }
            string mensajeError = "la cuenta ya esta agregada a este grupo";
            return BadRequest(mensajeError);
        }



        [Route("ApiGrupos/grupo/{id_grupo:int}")]
        public IHttpActionResult Put(int id_grupo, GrupoModel grupo)
        {
            bool existe = ControlGrupo.ModificarGrupo(id_grupo.ToString(), grupo.nombre_grupo, grupo.descripcion, grupo.banner);

            if (existe)
            {
                return Ok("Grupo modificado con éxito");
            }

            return NotFound();
        }


        [Route("ApiGrupos/grupo/{id_grupo:int}")]
        public IHttpActionResult DeleteGrupo(int id_grupo)
        {
            var resultado = ControlGrupo.EliminarGrupo(id_grupo.ToString());

            if (resultado == true)
            {
                return Ok("Grupo eliminado");
            }

            string mensajeError = "El grupo no existe";
            return BadRequest(mensajeError);
        }


        [Route("ApiGrupos/grupo/{id_grupo:int}/cuenta/{id_cuenta:int}")]

        public IHttpActionResult Delete(int id_grupo, int id_cuenta)
        {
            var resultado = ControlGrupo.EliminarCuentaDeGrupo(id_grupo.ToString(), id_cuenta.ToString());

            if(resultado["resultado"] == "true")
            {
                return Ok("Cuenta eliminada del grupo con éxito");
            }

            string mensajeError = "La cuenta no existe en este grupo";
            return BadRequest(mensajeError);
        }

    }




}
