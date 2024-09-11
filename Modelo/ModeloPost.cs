using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class ModeloPost : Modelo
    {
        public long Id_Post;
        public string url_contenido = "url";
        public string tipo_contenido = "tagsito";
        public string contenido;
        public int id_cuenta ;

        public int id_evento;
        public string nombre_evento;
        public string imagen = "url imagen";
        public string descripcion_evento;
        public string fecha_evento = "2022-04-22 10:34:53";

        const int MYSQL_DUPLICATE_ENTRY = 1062;

        public void GuardarPost()
        {
            if (this.Id_Post == 0) InsertarPost();
            if (this.Id_Post > 0) ActualizarPost();
        }

        public void GuardarEvento()
        {
            if (this.id_evento == 0) InsertarEvento();
            if (this.id_evento > 0) ActualizarEvento();
        }

        private void InsertarPost()
        {
            string sql = $"insert into posts (contenido,url_contenido,tipo_contenido,id_cuenta) values('{this.contenido}','{this.url_contenido}','{this.tipo_contenido}',{this.id_cuenta})";
            PrintDesktop(sql);
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
            PrintDesktop(sql);
        }


        private void VerificarEventoEnBD()
        {
            string verificarEventoSql = "SELECT COUNT(*) FROM evento WHERE nombre_evento = @nombre_evento";
            this.Comando.CommandText = verificarEventoSql;
            this.Comando.Parameters.Clear(); // Limpia los parámetros antes de agregar los nuevos
            this.Comando.Parameters.AddWithValue("@nombre_evento", this.nombre_evento);

            long count = (long)this.Comando.ExecuteScalar();

            if (count > 0)
            {
                throw new Exception("DUPLICATE_ENTRY");
            }
        }
        public void InsertarEvento()
        {
            InsertarPost();
            this.Id_Post = this.Comando.LastInsertedId;
            try {
                VerificarEventoEnBD();
                this.Comando.Parameters.Clear();
                string sql = $"INSERT INTO evento (id_post, nombre_evento,imagen,fecha_evento, descripcion_evento) " +
                    $"VALUES('{this.Id_Post}',@nombre_evento,'{this.imagen}','{this.fecha_evento}',@descripcion_evento)";
                this.Comando.CommandText = sql;
                PrintDesktop(sql);
                this.Comando.Parameters.AddWithValue("@nombre_evento", this.nombre_evento);
                //this.Comando.Parameters.AddWithValue("@imagen", this.imagen);
                this.Comando.Parameters.AddWithValue("@descripcion_evento", this.descripcion_evento);
                this.Comando.Prepare();
                this.Comando.ExecuteNonQuery();
            }
            catch(MySqlException e)
            {
                if (e.Number == MYSQL_DUPLICATE_ENTRY)
                    throw new Exception("DUPLICATE_ENTRY");
            }



        }
        public void ActualizarPost()
        {
            string sql = $"update posts set contenido ='{this.contenido}',tipo_contenido = '{this.tipo_contenido}'," +
                $"url_contenido = '{this.url_contenido}' where id_post ='{this.Id_Post}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
            PrintDesktop(sql);
        }

        public void ActualizarEvento()
        {
            ActualizarPost();
            string sql = $"update evento set id_post ='{this.Id_Post}',nombre_evento='{this.nombre_evento}',imagen='{this.imagen}',descripcion_evento='{this.descripcion_evento}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
            PrintDesktop(sql);
        }

        public void EliminarPost()
        {
            string sql = $"update posts set eliminado = true where id_post ='{this.Id_Post}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public List<ModeloPost> ObtenerPosts(int id_cuenta)
        {
            List<ModeloPost> posts = new List<ModeloPost>();

            string sql = $"select * from posts where eliminado = false and id_cuenta = {id_cuenta}";
            this.Comando.CommandText = sql;
            this.Lector = this.Comando.ExecuteReader();

            while (this.Lector.Read())
            {
                ModeloPost post = new ModeloPost();
                post.Id_Post = Int32.Parse(this.Lector["Id_post"].ToString());
                post.contenido = this.Lector["Contenido"].ToString();
                posts.Add(post);
            }
            this.Lector.Close();
            return posts;
        }
    }

}