﻿using System;
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

        [Route("LinguaLink/usuarios/preferencias/{idCuenta:int}")]
        [HttpGet]
        public IHttpActionResult PreferenciasGet(int idCuenta)
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

        [Route("LinguaLink/usuarios/preferencias/{idCuenta:int}")]
        [HttpPut]
        public IHttpActionResult PreferenciasPut(int idCuenta,PreferenciasModel Pref)
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
    }
}