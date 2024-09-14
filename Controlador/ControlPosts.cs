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

        public static bool CrearEvento(string nombre_evento, string imagen, string descripcion_evento, string contenido, string url, string tipo_contenido, string idCuenta)
        {
            ModeloPost evento = new ModeloPost();
            try
            {
                evento.nombre_evento = nombre_evento;
                evento.imagen = imagen;
                evento.descripcion_evento = descripcion_evento;

                evento.contenido = contenido;
                evento.url_contenido = url;
                evento.tipo_contenido = tipo_contenido;
                evento.id_cuenta = Int32.Parse(idCuenta);
                evento.GuardarEvento();
                return true;
            }
            catch(Exception e)
            {
                if (e.Message == "DUPLICATE_ENTRY")
                    throw new Exception("El evento ya existe");
                return false;
            }
            
        }

        public static void ElimiarPost(string id)
        {
            ModeloPost post = new ModeloPost();
            post.id_post = Int32.Parse(id);
            post.EliminarPost();
        }

        public static void ElimiarEvento(string id_post, string id_evento)
        {
            ElimiarPost(id_post);
            ModeloPost evento = new ModeloPost();
            evento.id_evento = Int32.Parse(id_evento);
            evento.EliminarEvento();
        }

        public static void ModificarPost(string id, string contenido,string url,string tipo_contenido)
        {
            ModeloPost post = new ModeloPost();
            post.id_post = Int32.Parse(id);
            post.contenido = contenido;
            post.url_contenido = url;
            post.tipo_contenido = tipo_contenido;
            post.GuardarPost();
        }

        public static void ModificarEvento(string Id_Post, string id_evento, string url_contenido, string tipo_contenido, string contenido, string nombre_evento, string imagen, string descripcion_evento, string id_cuenta)
        {
            ModeloPost evento = new ModeloPost();
            evento.id_post = Int32.Parse(Id_Post);
            evento.id_evento = Int32.Parse(id_evento);
            evento.url_contenido = url_contenido;
            evento.tipo_contenido = tipo_contenido;
            evento.contenido = contenido;
            evento.nombre_evento = nombre_evento;
            evento.imagen = imagen;
            evento.descripcion_evento = descripcion_evento;
            evento.id_cuenta = Int32.Parse(id_cuenta);

            evento.ActualizarEvento();
        }

        public static void CompartirPostEnMuro(string id_post, string id_muro)
        {
            ModeloPost post = new ModeloPost();

            post.id_post = Int32.Parse(id_post);
            post.id_muro = Int32.Parse(id_muro);

            post.CompartirPostEnMuro();
        }

        public static void CompartirPostEnGrupo(string id_post, string id_grupo)
        {
            ModeloPost post = new ModeloPost();

            post.id_post = Int32.Parse(id_post);
            post.id_muro = Int32.Parse(id_grupo);

            post.CompartirPostEnGrupo();
        }


        public static DataTable Listar(string idCuenta)  
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("id_post", typeof(int));
            tabla.Columns.Add("Contenido", typeof(string));


            ModeloPost pizza = new ModeloPost();
            foreach (ModeloPost p in pizza.ObtenerPosts(Int32.Parse(idCuenta))) 
            {
                DataRow fila = tabla.NewRow();
                fila["Id_post"] = p.id_post;
                fila["Contenido"] = p.contenido;
                tabla.Rows.Add(fila);
            }

            return tabla;

        }

    }
}
