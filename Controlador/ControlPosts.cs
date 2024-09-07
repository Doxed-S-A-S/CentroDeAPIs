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
        public static void CrearPost(string contenido,string url,string tipo_contenido, string idCuenta)
        {
            ModeloPost post = new ModeloPost();
            post.contenido = contenido;
            post.url_contenido = url;
            post.tipo_contenido = tipo_contenido;
            post.id_cuenta = Int32.Parse(idCuenta);

            post.GuardarPost();
        }

        public static void CrearEvento(string nombre_evento, string imagen, string descripcion_evento, string contenido, string url, string tipo_contenido, string idCuenta)
        {
            ModeloPost evento = new ModeloPost();

            evento.nombre_evento = nombre_evento;
            evento.imagen = imagen;
            evento.descripcion_evento = descripcion_evento;

            evento.contenido = contenido;
            evento.url_contenido = url;
            evento.tipo_contenido = tipo_contenido;
            evento.id_cuenta = Int32.Parse(idCuenta);

            evento.GuardarEvento();
        }

        public static void ElimiarPost(string id)
        {
            ModeloPost post = new ModeloPost();
            post.Id_Post = Int32.Parse(id);
            post.EliminarPost();
        }

        public static void ModificarPost(string id, string contenido,string url,string tipo_contenido)
        {
            ModeloPost post = new ModeloPost();
            post.Id_Post = Int32.Parse(id);
            post.contenido = contenido;
            post.url_contenido = url;
            post.tipo_contenido = tipo_contenido;
            post.GuardarPost();
        }

        public static DataTable Listar(string idCuenta)  
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("Id_Post", typeof(int));
            tabla.Columns.Add("Contenido", typeof(string));


            ModeloPost pizza = new ModeloPost();
            foreach (ModeloPost p in pizza.ObtenerPosts(Int32.Parse(idCuenta))) 
            {
                DataRow fila = tabla.NewRow();
                fila["Id_post"] = p.Id_Post;
                fila["Contenido"] = p.contenido;
                tabla.Rows.Add(fila);
            }

            return tabla;

        }

    }
}
