using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class ModeloGrupo : Modelo
    {
        public long id_grupo;
        public string nombre_grupo;
        public string descripcion;
        public string banner;
        public Boolean privacidad;

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
            string sql = $"insert into grupos (nombre_grupo,descripcion,privacidad,banner) values('{this.nombre_grupo}','{this.descripcion}',{this.privacidad},'{this.banner}')";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
            this.id_grupo = this.Comando.LastInsertedId;
            AsignarGrupoOwner();
        }
        private void ModificarGrupo()
        {
            string sql = $"UPDATE grupos set nombre_grupo ='{this.nombre_grupo}', descripcion = '{this.descripcion}', banner = '{this.banner}' WHERE id_grupo = {this.id_grupo}";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }
        public void ModificarNombreGrupo()
        {
            string sql = $"update grupos set nombre_grupo ='{this.nombre_grupo}' where id_grupo ='{this.id_grupo}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }
        public void ModificarDescripcionGrupo()
        {
            string sql = $"update grupos set descripcion = '{this.descripcion}' where id_grupo = '{this.id_grupo}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void ModificarPrivacidadGrupo()
        {
            string sql = $"update grupos set privacidad = {this.privacidad} where id_grupo = {this.id_grupo}";
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
                grupo.nombre_grupo = this.Lector["nombre_grupo"].ToString();
                grupos.Add(grupo);
            }
            this.Lector.Close();

            return grupos;
        }

        public List<ModeloGrupo> ObtenerIntegrantesDeGrupo(int id)
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

        public bool BuscarGrupo(int id)
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
                this.banner = this.Lector["banner"].ToString();
                this.Lector.Close();
                return true;

            }
            this.Lector.Close();
            return false;

        }

        public bool FormaParteDelGrupo()
        {
            string sql = $"SELECT COUNT(*) FROM conforma WHERE id_grupo = @id_grupo AND id_cuenta = @id_cuenta";
            this.Comando.CommandText = sql;
            this.Comando.Parameters.Clear();
            this.Comando.Parameters.AddWithValue("@id_grupo", this.id_grupo);
            this.Comando.Parameters.AddWithValue("@id_cuenta", this.id_cuenta);

            string count = this.Comando.ExecuteScalar().ToString();

            return count == "1";
        }
        public void AsignarGrupoOwner()
        {
            string sql = $"insert into conforma (id_grupo,id_cuenta,rol) values({this.id_grupo},{this.id_cuenta},'owner')";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }
        public void AgregarCuentaEnGrupo()
        {
            string sql = $"insert into conforma (id_cuenta,id_grupo,rol) values('{this.id_cuenta}','{this.id_grupo}','participante')";
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
