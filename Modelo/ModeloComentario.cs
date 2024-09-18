using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class ModeloComentario : Modelo
    {
        public int IdPost;
        public int idCuenta;
        public long IdComentario;
        public string Contenido;

        public void GuardarComentario()
        {
            if (this.IdComentario == 0) InsertarComentario();
            if (this.IdComentario > 0) AcualizarComentario();
        }

        private void InsertarComentario()
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


        public void EliminarComentario()
        {
            string sql = $"update comentarios set eliminado = true where id_comentario ='{this.IdComentario}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void AcualizarComentario()
        {
            string sql = $"update comentarios set contenido ='{this.Contenido}'where id_comentario ='{this.IdComentario}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public List<ModeloComentario> ObtenerComentarios(string idPost)
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
    }
}
