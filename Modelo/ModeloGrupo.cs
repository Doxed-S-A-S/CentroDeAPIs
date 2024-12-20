﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Modelos
{
    public class ModeloGrupo : Modelo
    {
        public long id_grupo;
        public string nombre_grupo;
        public string descripcion;
        public string imagen_banner;
        public string url_imagen;
        public Boolean privacidad;

        public string rol;
        public int id_cuenta;
        public string nombre_usuario;


        const int MYSQL_DUPLICATE_ENTRY = 1062;
        const int MYSQL_ACCESS_DENIED = 1045;
        const int MYSQL_UNKNOWN_COLUMN = 1054;
        const int MYSQL_ERROR_CHILD_ROW = 1452;

        public void Guardar()
        {
            if (this.id_grupo == 0) CrearGrupo();
            if (this.id_grupo > 0) ModificarGrupo();
        }
        public void CrearGrupo()
        {
            try
            { 
                string sql = $"insert into grupos (nombre_grupo,descripcion,privacidad,imagen_banner,url_imagen) values(@nombre_grupo,@descripcion,@privacidad,@imagen_banner,@url_imagen)";
                Console.Write(this.id_cuenta);
                
                this.Comando.CommandText = sql;
                this.Comando.Parameters.AddWithValue("@nombre_grupo",this.nombre_grupo);
                this.Comando.Parameters.AddWithValue("@descripcion",this.descripcion);
                this.Comando.Parameters.AddWithValue("@privacidad", this.privacidad);
                this.Comando.Parameters.AddWithValue("@imagen_banner", this.imagen_banner);
                this.Comando.Parameters.AddWithValue("@url_imagen", this.url_imagen);
                this.Comando.Prepare();
                this.Comando.ExecuteNonQuery();
                this.id_grupo = this.Comando.LastInsertedId;
                AsignarGrupoOwner();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }catch (Exception e)
            {
                Console.Write(e.Message);
                throw new Exception("UNKNOWN_ERROR");
            }
        }
        private void ModificarGrupo()
        {
            try
            {
                string sql = $"UPDATE grupos set nombre_grupo =@nombre_grupo, descripcion = @descripcion, banner = @banner WHERE id_grupo = {this.id_grupo}";
                this.Comando.CommandText = sql;
                this.Comando.Parameters.AddWithValue("@nombre_grupo", this.nombre_grupo);
                this.Comando.Parameters.AddWithValue("@descripcion", this.descripcion);
                this.Comando.Parameters.AddWithValue("@banner", this.imagen_banner);
                this.Comando.Prepare();
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }
        public void ModificarNombreGrupo()
        {
            try
            {
                string sql = $"update grupos set nombre_grupo = @nombre_grupo where id_grupo ='{this.id_grupo}'";
                this.Comando.CommandText = sql;
                this.Comando.Parameters.AddWithValue("@nombre_grupo", this.nombre_grupo);
                this.Comando.Prepare();
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }
        public void ModificarDescripcionGrupo()
        {
            try
            {
                string sql = $"update grupos set descripcion = '{this.descripcion}' where id_grupo = '{this.id_grupo}'";
                this.Comando.CommandText = sql;
                this.Comando.Parameters.AddWithValue("@descripcion", this.descripcion);
                this.Comando.Prepare();
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public void ModificarPrivacidadGrupo()
        {
            try
            {
                string sql = $"update grupos set privacidad = {this.privacidad} where id_grupo = {this.id_grupo}";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }
        public void ModificarBannerGrupo()
        {
            try
            {
                string sql = $"update grupos set banner = @banner where id_grupo = '{this.id_grupo}'";
                this.Comando.CommandText = sql;
                this.Comando.Parameters.AddWithValue("@banner", this.imagen_banner);
                this.Comando.Prepare();
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }
        public void EliminarGrupo()
        {
            try
            {
                string sql = $"update grupos set eliminado = true where id_grupo ='{this.id_grupo}'";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }
        public List<ModeloGrupo> ObtenerGrupos()
        {
            try
            {
                List<ModeloGrupo> grupos = new List<ModeloGrupo>();

                string sql = $"select * from grupos where eliminado = false";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                while (this.Lector.Read())
                {
                    ModeloGrupo grupo = new ModeloGrupo();
                    grupo.id_grupo = Int32.Parse(this.Lector["id_grupo"].ToString());
                    grupo.nombre_grupo = this.Lector["nombre_grupo"].ToString();
                    grupos.Add(grupo);
                }
                this.Lector.Close();

                return grupos;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return null;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public List<ModeloGrupo> ObtenerGruposQueConformaUsuario(int id_usuario)
        {
            List<ModeloGrupo> grupos = new List<ModeloGrupo>();
            try
            {
                string sql = $"SELECT g.id_grupo, g.nombre_grupo " +
               $"FROM grupos g " +
               $"JOIN conforma c ON g.id_grupo = c.id_grupo " +
               $"JOIN cuenta cu ON c.id_cuenta = cu.id_cuenta " +
               $"WHERE cu.id_cuenta = '{id_usuario}' AND cu.eliminado = false;";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();
                while (this.Lector.Read())
                {
                    ModeloGrupo grupo = new ModeloGrupo();
                    grupo.id_grupo = Int32.Parse(this.Lector["id_grupo"].ToString());
                    grupo.nombre_grupo = this.Lector["nombre_grupo"].ToString();
                    grupos.Add(grupo);
                }
                this.Lector.Close();

                return grupos;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("UNKNOWN_ERROR");
            }

        }

        public DataTable ObtenerGrupo(int id_grupo)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string sql = $"SELECT * FROM grupos WHERE eliminado = false AND id_grupo = {id_grupo}";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                dataTable.Load(this.Lector);

                return dataTable;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }
        public List<ModeloGrupo> ObtenerIntegrantesDeGrupo(int id)
        {
            try
            {
                List<ModeloGrupo> grupos = new List<ModeloGrupo>();

                string sql = $"SELECT cuenta.nombre_usuario, grupos.nombre_grupo AS nombre_grupo, conforma.rol " +
                             $"FROM cuenta JOIN conforma ON cuenta.id_cuenta = conforma.id_cuenta " +
                             $"JOIN grupos ON conforma.id_grupo = grupos.id_grupo " +
                             $"WHERE grupos.id_grupo ='{id}'";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                while (this.Lector.Read())
                {
                    ModeloGrupo grupo = new ModeloGrupo();
                    grupo.nombre_usuario = this.Lector["nombre_usuario"].ToString();
                    grupo.nombre_grupo = this.Lector["nombre_grupo"].ToString();
                    grupo.rol = this.Lector["rol"].ToString();
                    grupos.Add(grupo);
                }
                this.Lector.Close();

                return grupos;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return null;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public bool BuscarGrupo(int id)
        {
            try
            {
                string sql = $"select * from grupos where eliminado = false and id_grupo = {id}";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                if (this.Lector.HasRows)
                {
                    this.Lector.Read();
                    this.id_grupo = Int32.Parse(this.Lector["id_grupo"].ToString());
                    this.nombre_grupo = this.Lector["nombre_grupo"].ToString();
                    this.descripcion = this.Lector["descripcion"].ToString();
                    this.imagen_banner = this.Lector["banner"].ToString();
                    this.Lector.Close();
                    return true;

                }
                this.Lector.Close();
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public bool FormaParteDelGrupo()
        {
            try
            {
                string sql = $"SELECT COUNT(*) FROM conforma WHERE id_grupo = @id_grupo AND id_cuenta = @id_cuenta";
                this.Comando.CommandText = sql;
                this.Comando.Parameters.Clear();
                this.Comando.Parameters.AddWithValue("@id_grupo", this.id_grupo);
                this.Comando.Parameters.AddWithValue("@id_cuenta", this.id_cuenta);

                string count = this.Comando.ExecuteScalar().ToString();

                return count == "1";
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }
        public void AsignarGrupoOwner()
        {
            try
            {
                string sql = $"insert into conforma (id_grupo,id_cuenta,rol) values({this.id_grupo},{this.id_cuenta},'owner')";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }
        public void AgregarCuentaEnGrupo()
        {
            try
            {
                string sql = $"insert into conforma (id_cuenta,id_grupo,rol) values('{this.id_cuenta}','{this.id_grupo}','participante')";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public void EliminarCuentaDeGrupo()
        {
            try
            {
                string sql = $"delete from conforma where id_cuenta = {this.id_cuenta} and id_grupo ={this.id_grupo};";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public void CambiarRolDeCuentaEnGrupo()
        {
            try
            {
                string sql = $"update conforma set rol = '{this.rol}' where id_grupo = '{this.id_grupo}' and id_cuenta = '{this.id_cuenta}'";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public void AñadirReporteGrupo()
        {
            try
            {
                string sql = $"update grupos set reports = reports +1 where id_grupo = {this.id_grupo}";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }


        private void MySqlErrorCatch(MySqlException sqlx)
        {
            if (sqlx.Number == MYSQL_DUPLICATE_ENTRY)
                throw new Exception("DUPLICATE_ENTRY");
            if (sqlx.Number == MYSQL_ACCESS_DENIED)
                throw new Exception("ACCESS_DENIED");
            if (sqlx.Number == MYSQL_UNKNOWN_COLUMN)
                throw new Exception("UNKNOWN_COLUMN");
            if (sqlx.Number == MYSQL_ERROR_CHILD_ROW)
                throw new Exception("ERROR_CHILD_ROW");

            throw new Exception("UNKNOWN_DB_ERROR");
        }

    }
}
