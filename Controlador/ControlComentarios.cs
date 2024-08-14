using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Modelos;

namespace Controlador
{
    public class ControlComentarios
    {
        public static void CrearComentario(string idPost,string comentario,string reacciones)
        {
            ModeloComentario coment = new ModeloComentario();
            coment.IdPost = Int32.Parse(idPost);
            coment.Contenido = comentario;

            coment.GuardarComentario();
        }

        public static void EliminarComentario(string idcoment)
        {
            ModeloComentario coment = new ModeloComentario();
            coment.IdComentario = Int32.Parse(idcoment);
            coment.EliminarComentario();
        }

        public static void ModificarComentario(string idcoment,string comentario)
        {
            ModeloComentario coment = new ModeloComentario();
            coment.IdComentario = Int32.Parse(idcoment);
            coment.Contenido = comentario;
            coment.GuardarComentario();
        }


        public static DataTable ListarComentarios(string idPost)
        { 
            DataTable tabla = new DataTable();
            tabla.Columns.Add("IdComentario", typeof(int));
            tabla.Columns.Add("IdPost", typeof(int));
            tabla.Columns.Add("Comentario", typeof(string));


            ModeloComentario coment = new ModeloComentario();
            foreach (ModeloComentario p in coment.ObtenerComentarios(idPost))
            {
                DataRow fila = tabla.NewRow();
                fila["IdComentario"] = p.IdComentario;
                fila["IdPost"] = p.IdPost;
                fila["Comentario"] = p.Contenido;
                tabla.Rows.Add(fila);
            }

            return tabla;

        }
    }
}