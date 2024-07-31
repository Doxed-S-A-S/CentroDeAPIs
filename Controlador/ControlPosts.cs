using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Modelos;

namespace Controlador
{
    public class ControlPosts
    {
        public static void CrearPost(string contenido, string reacciones)
        {
            ModeloPost post = new ModeloPost();
            post.Contenido = contenido;
            post.Reacciones = Int32.Parse(reacciones);

            post.GuardarPost();
        }

        public static void ElimiarPost(string id)
        {
            ModeloPost post = new ModeloPost();
            post.Id_Post = Int32.Parse(id);
            post.EliminarPost();
        }

        public static void ModificarPost(string id, string contenido)
        {
            ModeloPost post = new ModeloPost();
            post.Id_Post = Int32.Parse(id);
            post.Contenido = contenido;
            post.AcutalizarPost();
        }

        public static DataTable Listar()  // iterar con foreach y trear contenido e i, luego agarrar el id y cargar los datos alado
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("Id_Post", typeof(int));
            tabla.Columns.Add("Contenido", typeof(string));
            tabla.Columns.Add("Reacciones", typeof(int));


            ModeloPost pizza = new ModeloPost();
            foreach (ModeloPost p in pizza.ObtenerPosts())
            {
                DataRow fila = tabla.NewRow();
                fila["Id_post"] = p.Id_Post;
                fila["Contenido"] = p.Contenido;
                fila["Reacciones"] = p.Reacciones;
                tabla.Rows.Add(fila);
            }

            return tabla;

        }

    }
}
