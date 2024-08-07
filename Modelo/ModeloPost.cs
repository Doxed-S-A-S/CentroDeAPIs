using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class ModeloPost : Modelo
    {
        public int Id_Post;
        public string Contenido;
        public int Reacciones;

        public void GuardarPost()
        {
            if (this.Id_Post == 0) InsertarPost();
            if (this.Id_Post > 0) AcutalizarPost();
        }

        private void InsertarPost()
        {
            string sql = $"insert into Posts (Contenido,Reacciones) values('{this.Contenido}',{this.Reacciones})";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void AcutalizarPost()
        {
            string sql = $"update Posts set Contenido ='{this.Contenido}'where ID_post ='{this.Id_Post}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void EliminarPost()
        {
            string sql = $"update Posts set Eliminado = true where ID_post ='{this.Id_Post}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public List<ModeloPost> ObtenerPosts() // Modificar en el futuro cuando tegamos creada la bd y la clase de usr, para que busque post por usr
        {
            List<ModeloPost> posts = new List<ModeloPost>();

            string sql = $"select * from Posts where Eliminado = false";
            this.Comando.CommandText = sql;
            this.Lector = this.Comando.ExecuteReader();

            while (this.Lector.Read())
            {
                ModeloPost post = new ModeloPost();
                post.Id_Post = Int32.Parse(this.Lector["Id_post"].ToString());
                post.Contenido = this.Lector["Contenido"].ToString();
                post.Reacciones = Int32.Parse(this.Lector["Reacciones"].ToString());
                posts.Add(post);
            }
            this.Lector.Close();
            return posts;
        }
    }

}