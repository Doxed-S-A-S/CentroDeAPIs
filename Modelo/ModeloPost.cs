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
        public string url_contenido = "url"; //placeholder
        public string tipo_contenido = "tag"; //placeholder
        public string Contenido;
        public int Id_Cuenta ;

        public void GuardarPost()
        {
            if (this.Id_Post == 0) InsertarPost();
            if (this.Id_Post > 0) AcutalizarPost();
        }

        private void InsertarPost()
        {
            string sql = $"insert into posts (contenido,url_contenido,tipo_contenido,id_cuenta) values('{this.Contenido}','{this.url_contenido}','{this.tipo_contenido}',{this.Id_Cuenta})";
            PrintDesktop(sql);
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
            
        }

        public void AcutalizarPost()
        {
            string sql = $"update posts set contenido ='{this.Contenido}'where id_post ='{this.Id_Post}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
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
                post.Contenido = this.Lector["Contenido"].ToString();
                posts.Add(post);
            }
            this.Lector.Close();
            return posts;
        }
    }

}