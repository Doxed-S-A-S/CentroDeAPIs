using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Modelos
{
    public class ModeloComentario : Modelo
    {
        public int IdPost;
        public int idCuenta;
        public long IdComentario;
        public string Contenido;
        public long idUpvote;

        const int MYSQL_DUPLICATE_ENTRY = 1062;
        const int MYSQL_ACCESS_DENIED = 1045;
        const int MYSQL_UNKNOWN_COLUMN = 1054;
        const int MYSQL_ERROR_CHILD_ROW = 1452;

        public void GuardarComentario()
        {
            try
            {
                if (this.IdComentario == 0) InsertarComentario();
                if (this.IdComentario > 0) AcualizarComentario();
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

        private void InsertarComentario()
        {
            try
            {
                string sql = $"insert into comentarios (id_post,contenido) values(@id_post,@contenido)";
                this.Comando.CommandText = sql;
                this.Comando.Parameters.AddWithValue("@contenido", this.Contenido);
                this.Comando.Parameters.AddWithValue("@id_post", this.IdPost);
                this.Comando.Prepare();
                this.Comando.ExecuteNonQuery();
                IdComentario = this.Comando.LastInsertedId;

                sql = $"insert into hace (id_comentario,id_cuenta) values('{this.IdComentario}','{this.idCuenta}')";
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


        public void EliminarComentario()
        {
            try
            {
                string sql = $"update comentarios set eliminado = true where id_comentario ='{this.IdComentario}'";
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

        public void AcualizarComentario()
        {
            try
            {
                string sql = $"update comentarios set contenido ='{this.Contenido}'where id_comentario ='{this.IdComentario}'";
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

        public List<ModeloComentario> ObtenerComentarios(string idPost)
        {
            try
            {
                List<ModeloComentario> comentarios = new List<ModeloComentario>();

                string sql = $"select * from comentarios where id_post = '{Int32.Parse(idPost)}' and eliminado = false";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                while (this.Lector.Read())
                {
                    ModeloComentario coment = new ModeloComentario();
                    coment.IdComentario = Int32.Parse(this.Lector["id_comentario"].ToString());
                    coment.IdPost = Int32.Parse(this.Lector["id_post"].ToString());
                    coment.Contenido = this.Lector["contenido"].ToString();
                    comentarios.Add(coment);
                }
                this.Lector.Close();
                return comentarios;
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

        public void ComentarioLikeDeCuenta()
        {
            try
            {
                string sql = $"insert into LikeComent (id_comentario,id_upvote) values ({this.IdComentario},{this.idUpvote})";
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

        public void AñadirLikeComent()
        {
            try
            {
                string sql = $"insert into upvote (id_post,id_upvote) values ({this.IdPost}";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
                this.idUpvote = this.Comando.LastInsertedId;
                ComentarioLikeDeCuenta();
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


        public void EliminarLikeComent()
        {
            try
            {
                string sql = $"delete from LikeComent where id_comentario = {this.IdComentario} and id_upvote = {this.idUpvote}";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();

                sql = $"delete from upvote where id_upvote = {this.idUpvote} and id_post = {this.IdPost}";
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
