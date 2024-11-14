using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Modelos
{
    public class ModeloPost : Modelo
    {
        public long id_post;
        public string url_contenido;
        public string tipo_contenido;
        public string contenido;
        public string fecha_post;
        public string url_imagen;
        public int id_cuenta;

        public long id_upvote;
        public int likes;

        public int id_evento;
        public string nombre_evento;
        public string imagen = "url imagen";
        public string descripcion_evento;
        public string fecha_evento = "2022-04-22 10:34:53";

        public int id_muro;
        public int id_grupo;

        const int MYSQL_DUPLICATE_ENTRY = 1062;
        const int MYSQL_ACCESS_DENIED = 1045;
        const int MYSQL_UNKNOWN_COLUMN = 1054;
        const int MYSQL_ERROR_CHILD_ROW = 1452;

        public void GuardarPost()
        {
            if (this.id_post == 0) InsertarPost();
            if (this.id_post > 0) ActualizarPost();
        }

        public void GuardarEvento()
        {
            if (this.id_evento == 0) InsertarEvento();
            if (this.id_evento > 0) ActualizarEvento();
        }

        private void InsertarPost()
        {
            try
            {
                string sql = $"insert into posts (contenido,url_contenido, url_imagen,tipo_contenido,id_cuenta) values('{this.contenido}','{this.url_contenido}','{this.url_imagen}','{this.tipo_contenido}',{this.id_cuenta})";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
                this.id_post = this.Comando.LastInsertedId;
            }
            catch (MySqlException sqlx)
            {
                Console.Write(sqlx.Message);
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
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
            this.id_post = this.Comando.LastInsertedId;
            try
            {
                VerificarEventoEnBD();
                this.Comando.Parameters.Clear();
                string sql = $"INSERT INTO evento (id_post, nombre_evento,imagen,fecha_evento, descripcion_evento) " +
                    $"VALUES('{this.id_post}',@nombre_evento,'{this.imagen}','{this.fecha_evento}',@descripcion_evento)";
                this.Comando.CommandText = sql;
                PrintDesktop(sql);
                this.Comando.Parameters.AddWithValue("@nombre_evento", this.nombre_evento);
                //this.Comando.Parameters.AddWithValue("@imagen", this.imagen);
                this.Comando.Parameters.AddWithValue("@descripcion_evento", this.descripcion_evento);
                this.Comando.Prepare();
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number == MYSQL_DUPLICATE_ENTRY)
                    throw new Exception("DUPLICATE_ENTRY");
            }
        }


        public void ActualizarPost()
        {
            try
            {
                string sql = $"update posts set contenido ='{this.contenido}',tipo_contenido = '{this.tipo_contenido}'," +
    $"url_contenido = '{this.url_contenido}' where id_post ={this.id_post}";
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

        public void ActualizarEvento()
        {
            ActualizarPost();
            try
            {
                string sql = $"update evento set nombre_evento='{this.nombre_evento}',imagen='{this.imagen}',descripcion_evento='{this.descripcion_evento}' where id_evento ={this.id_evento}";
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

        public void EliminarPost()
        {
            try
            {
                string sql = $"update posts set eliminado = true where id_post ='{this.id_post}'";
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

        public void EliminarEvento()
        {
            try
            {
                string sql = $"update evento set eliminado = true where id_evento ='{this.id_evento}'";
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

        public void CompartirPostEnMuro()
        {
            try
            {
                string sql = $"insert into postea_muro (id_muro,id_post) values({this.id_muro},{this.id_post})";
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

        public void CompartirPostEnGrupo()
        {
            try
            {
                string sql = $"insert into postea_grupos (id_grupo,id_post) values({this.id_grupo},{this.id_post})";
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


        public List<ModeloPost> ObtenerPostsDeCuenta(int id_cuenta)
        {
            try
            {
                List<ModeloPost> posts = new List<ModeloPost>();

                string sql = $"select * from posts where eliminado = false and id_cuenta = {id_cuenta}";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                while (this.Lector.Read())
                {
                    ModeloPost post = new ModeloPost();
                    post.id_post = Int32.Parse(this.Lector["id_post"].ToString());
                    post.contenido = this.Lector["Contenido"].ToString();
                    post.id_cuenta = Int32.Parse(this.Lector["id_cuenta"].ToString());
                    post.url_imagen = this.Lector["Url_imagen"].ToString();
                    posts.Add(post);
                }
                this.Lector.Close();
                return posts;
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

        public List<ModeloPost> ObtenerPostsDeGrupo(int id_grupo)
        {
            try
            {
                List<ModeloPost> posts = new List<ModeloPost>();

                string sql = $"SELECT p.id_post, p.id_cuenta, p.url_contenido, p.tipo_contenido, p.fecha_creacion, p.contenido, p.reports, p.eliminado, g.url_imagen " +
             $"FROM posts p " +
             $"JOIN postea_grupos pg ON p.id_post = pg.id_post " +
             $"JOIN grupos g ON pg.id_grupo = g.id_grupo " +
             $"WHERE pg.id_grupo = {id_grupo};";
                ;

                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                while (this.Lector.Read())
                {
                    ModeloPost post = new ModeloPost();
                    post.id_post = Int32.Parse(this.Lector["id_post"].ToString());
                    post.contenido = this.Lector["Contenido"].ToString();
                    post.id_cuenta = Int32.Parse(this.Lector["id_cuenta"].ToString());
                    post.url_imagen = this.Lector["url_imagen"].ToString();
                    posts.Add(post);
                }
                this.Lector.Close();
                return posts;
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

        public List<ModeloPost> ObtenerPostsDeMuro(int id_muro)
        {
            try
            {
                List<ModeloPost> posts = new List<ModeloPost>();

                string sql = $"SELECT p.id_post, p.id_cuenta, p.url_contenido, p.tipo_contenido, p.fecha_creacion, p.contenido, p.reports, p.eliminado, p.url_imagen " +
             $"FROM posts p " +
             $"JOIN postea_muro pm ON p.id_post = pm.id_post " +
             $"WHERE pm.id_muro = {id_muro};";


                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                while (this.Lector.Read())
                {
                    ModeloPost post = new ModeloPost();
                    post.id_post = Int32.Parse(this.Lector["id_post"].ToString());
                    post.contenido = this.Lector["Contenido"].ToString();
                    post.id_cuenta = Int32.Parse(this.Lector["id_cuenta"].ToString());
                    post.url_imagen = this.Lector["Url_imagen"].ToString();
                    posts.Add(post);
                }
                this.Lector.Close();
                return posts;
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
        public List<ModeloPost> ObtenerPosts()
        {
            try
            {
                List<ModeloPost> posts = new List<ModeloPost>();

                string sql = $"select * from posts where eliminado = false and id_cuenta";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                while (this.Lector.Read())
                {
                    ModeloPost post = new ModeloPost();
                    post.id_post = Int32.Parse(this.Lector["id_post"].ToString());
                    post.contenido = this.Lector["Contenido"].ToString();
                    post.id_cuenta = Int32.Parse(this.Lector["id_cuenta"].ToString());
                    post.url_imagen = this.Lector["Url_imagen"].ToString();
                    posts.Add(post);
                }
                this.Lector.Close();
                return posts;
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


        public List<ModeloPost> BuscarPostRandom()
        {
            // agregar alguna logica de fecha, buscar por tags y amigos
            try
            {
                List<ModeloPost> posts = new List<ModeloPost>();

                string sql = $"SELECT * FROM posts " +
                    $"where eliminado = false and reports < 5 ORDER BY RAND() LIMIT 5";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                while (this.Lector.Read())
                {
                    ModeloPost post = new ModeloPost();
                    post.id_post = Int32.Parse(this.Lector["id_post"].ToString());
                    post.contenido = this.Lector["Contenido"].ToString();
                    post.tipo_contenido = this.Lector["tipo_contenido"].ToString();
                    post.id_cuenta = Int32.Parse(this.Lector["id_cuenta"].ToString());
                    post.url_contenido = this.Lector["url_contenido"].ToString();
                    post.url_imagen = this.Lector["url_imagen"].ToString();
                    post.fecha_post = this.Lector["fecha_creacion"].ToString();
                    post.likes = NumeroDeLikes(this.id_post);
                    posts.Add(post);
                }
                this.Lector.Close();
                return posts;
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


        public string ObtenerCreadorDePost()
        {
            string username = null;
            string sql = $"select nombre_usuario from cuenta where id_cuenta = ({this.id_cuenta})";
            this.Comando.CommandText = sql;


            this.Lector = this.Comando.ExecuteReader();
            if (this.Lector.Read())
            {
                username = this.Lector["nombre_usuario"].ToString();
            }
            this.Lector.Close();


            return username;
        }

        public bool usuario_ya_dio_like()
        {
            try
            {
                string query = "SELECT COUNT(*) AS usuario_ya_dio_like " +
                               "FROM da_upvote AS du " +
                               "JOIN upvote AS u ON du.id_upvote = u.id_upvote " +
                               "WHERE du.id_cuenta = @IdCuenta AND u.id_post = @IdPost;";

                // Limpia los parámetros para evitar duplicación.
                this.Comando.Parameters.Clear();

                // Agrega los parámetros con el tipo correcto.
                this.Comando.Parameters.Add("@IdCuenta", MySqlDbType.Int32).Value = this.id_cuenta;
                this.Comando.Parameters.Add("@IdPost", MySqlDbType.Int32).Value = this.id_post;

                // Asigna el comando y ejecuta la consulta.
                this.Comando.CommandText = query;
                int count = Convert.ToInt32(this.Comando.ExecuteScalar());

                return count > 0;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("UNKNOWN_ERROR", ex);
            }
        }

        public void AñadirLike()
        {
            if (usuario_ya_dio_like())
            {
                throw new Exception("Ya le dio like");
                EliminarLike();
            }
            try
            {
                string sql = $"insert into upvote (id_post) values ({this.id_post})";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
                this.id_upvote = this.Comando.LastInsertedId;
                LikeDeCuenta();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LikeDeCuenta()
        {
            try
            {
                string sql = $"insert into da_upvote (id_cuenta,id_upvote) values ({this.id_cuenta},{this.id_upvote})";
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

        public void EliminarLike()
        {
            try
            {
                string sql = $"delete from da_upvote where id_cuenta = {this.id_cuenta} and id_upvote = {this.id_upvote}";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();

                sql = $"delete from upvote where id_upvote = {this.id_upvote} and id_post = {this.id_post}";
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

        public int NumeroDeLikes(long idPost)
        {
            try
            {
                string sql = $"select count(*) from upvote where id_post = {idPost}";
                this.Comando.CommandText = sql;
                string likes = this.Comando.ExecuteScalar().ToString();
                return Int32.Parse(likes);
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return 0;
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


