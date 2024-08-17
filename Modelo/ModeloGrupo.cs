﻿using System;
using System.Collections.Generic;
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


        public void CrearGrupo()
        {
            string sql = $"insert into grupos (nombre,descripcion,banner) values('{this.nombre}','{this.descripcion}','{this.banner}')";
            PrintDesktop(sql);
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void ModificarNombreGrupo()
        {
            string sql = $"update grupos SET nombre ='{this.nombre}' where id ='{this.id_grupo}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }
        public void ModificarDescripcionGrupo()
        {
            string sql = $"update grupos SET descripcion = '{this.descripcion}' where id = '{this.id_grupo}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void ModificarBannerGrupo()
        {
            string sql = $"update grupos SET banner = '{this.banner}'where id = '{this.id_grupo}'";
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
                grupo.descripcion = this.Lector["descripcion"].ToString();
                grupo.banner = this.Lector["banner"].ToString();
             }
            this.Lector.Close();   
            return grupos;
        }

        public bool BuscarGrupo(int id)
        {
            string sql = $"SELECT * FROM grupos WHERE eliminado = false and id = {id}";
            this.Comando.CommandText = sql;
            this.Lector = this.Comando.ExecuteReader();

            if (this.Lector.HasRows)
            {
                this.Lector.Read();
                this.id_grupo = Int32.Parse(this.Lector["Id"].ToString());
                this.nombre = this.Lector["nombre"].ToString();
                this.descripcion = this.Lector["descripcion"].ToString();
                this.banner = this.Lector["banner"].ToString();
                this.Lector.Close();
                return true;

            }
            this.Lector.Close();
            return false;

        }



    }
}
