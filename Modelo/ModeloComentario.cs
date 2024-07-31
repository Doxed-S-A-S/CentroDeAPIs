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
        public int IdComentario;
        public string Comentario;
        public int ReaccionesCom;

        public void GuardarComentario()
        {
            string sql = $"insert into Comentarios (ID_post,Comentario,C_Reacciones) values({this.IdPost},'{this.Comentario}',{this.ReaccionesCom})";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void EliminarComentario()
        {
            string sql = $"update Comentarios set Eliminado = true where ID_comentario ='{this.IdComentario}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void AcualizarComentario()
        {
            string sql = $"update Comentarios set Comentario ='{this.Comentario}'where ID_comentario ='{this.IdComentario}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public List<ModeloComentario> ObtenerComentarios(string idPost)
        {
            string idP = idPost;
            List<ModeloComentario> comentarios = new List<ModeloComentario>();

            string sql = $"select * from Comentarios where ID_post = '{idP}' and Eliminado = false";
            this.Comando.CommandText = sql;
            this.Lector = this.Comando.ExecuteReader();

            while (this.Lector.Read())
            {
                ModeloComentario coment = new ModeloComentario();
                coment.IdComentario = Int32.Parse(this.Lector["ID_comentario"].ToString());
                coment.IdPost = Int32.Parse(this.Lector["ID_post"].ToString());
                coment.Comentario = this.Lector["Comentario"].ToString();
                coment.ReaccionesCom = Int32.Parse(this.Lector["C_reacciones"].ToString());
                comentarios.Add(coment);
            }
            return comentarios;
        }
    }
}
