﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class ModeloGrupo : Modelo
    {
        public int id_grupo;
        public string nombre;
        public string descripcion;
        public string banner; //placeholder
        public string rol;
        public int id_cuenta;
        public string nombre_usuario;
        public void Guardar()
        {
            if (this.id_grupo == 0) CrearGrupo();
            if (this.id_grupo > 0) ModificarGrupo();
        }
        public void CrearGrupo()
        {
            string sql = $"insert into grupos (nombre,descripcion,banner) values('{this.nombre}','{this.descripcion}','{this.banner}')";
            PrintDesktop(sql);
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }
        private void ModificarGrupo()
        {
            string sql = $"UPDATE grupos set nombre ='{this.nombre}', descripcion = '{this.descripcion}', banner = '{this.banner}' WHERE id_grupo = {this.id_grupo}";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }
        public void ModificarNombreGrupo()
        {
            string sql = $"update grupos set nombre ='{this.nombre}' where id_grupo ='{this.id_grupo}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }
        public void ModificarDescripcionGrupo()
        {
            string sql = $"update grupos set descripcion = '{this.descripcion}' where id_grupo = '{this.id_grupo}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }
        public void ModificarBannerGrupo()
        {
            string sql = $"update grupos set banner = '{this.banner}'where id_grupo = '{this.id_grupo}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }
        public void EliminarGrupo()
        {
            string sql = $"update grupos set eliminado = true where id_grupo ='{this.id_grupo}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
 
        }
        public List<ModeloGrupo> ObtenerGrupos()
        {
            List<ModeloGrupo> grupos = new List<ModeloGrupo>();

            string sql = $"select * from grupos where eliminado = false";
            this.Comando.CommandText = sql;
            this.Lector = this.Comando.ExecuteReader();

            while (this.Lector.Read())
            {
                ModeloGrupo grupo = new ModeloGrupo();
                grupo.id_grupo = Int32.Parse(this.Lector["id_grupo"].ToString());
                grupo.nombre = this.Lector["nombre"].ToString();
                grupos.Add(grupo);
            }
            this.Lector.Close();

            return grupos;
        }

        public List<ModeloGrupo> ObtenerIntegrantesDeGrupo()
        {
            List<ModeloGrupo> grupos = new List<ModeloGrupo>();

            string sql = $"SELECT cuenta.nombre_usuario, grupos.nombre AS nombre_grupo " +
                         $"FROM cuenta JOIN conforma ON cuenta.id_cuenta = conforma.id_cuenta " +
                         $"JOIN grupos ON conforma.id_grupo = grupos.id_grupo " +
                         $"WHERE grupos.id_grupo ='{this.id_grupo}'";
            this.Comando.CommandText = sql;
            this.Lector = this.Comando.ExecuteReader();

            while (this.Lector.Read())
            {
                ModeloGrupo grupo = new ModeloGrupo();
                grupo.id_grupo = Int32.Parse(this.Lector["id_grupo"].ToString());
                grupo.id_cuenta = Int32.Parse(this.Lector["id_grupo"].ToString());
                grupo.rol = this.Lector["nombre"].ToString();
                grupos.Add(grupo);
            }
            this.Lector.Close();

            return grupos;
        }
        public bool BuscarGrupo(int id)
        {
            string sql = $"select * from grupos where eliminado = false and id_grupo = {id}";
            this.Comando.CommandText = sql;
            this.Lector = this.Comando.ExecuteReader();

            if (this.Lector.HasRows)
            {
                this.Lector.Read();
                this.id_grupo = Int32.Parse(this.Lector["id_grupo"].ToString());
                this.nombre = this.Lector["nombre"].ToString();
                this.descripcion = this.Lector["descripcion"].ToString();
                this.banner = this.Lector["banner"].ToString();
                this.Lector.Close();
                return true;

            }
            this.Lector.Close();
            return false;

        }

        public void AgregarCuentaEnGrupo()
        {
            string sql = $"insert into conforma (id_cuenta,id_grupo,rol) values('{this.id_cuenta}','{this.id_grupo}','{this.rol}')";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void EliminarCuentaDeGrupo()
        {
            string sql = $"delete from conforma where id_cuenta = {this.id_cuenta} and id_grupo ={this.id_grupo};";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void CambiarRolDeCuentaEnGrupo()
        {
            string sql = $"update conforma set rol = '{this.rol}' where id_grupo = '{this.id_grupo}' and id_cuenta = '{this.id_cuenta}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }


    }
}
