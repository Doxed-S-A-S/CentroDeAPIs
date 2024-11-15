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


        public static void CrearPost(string contenido, string url_contenido, string url_imagen, string tipo_contenido, string idCuenta)
        {
            try
            {
                ModeloPost post = new ModeloPost();
                post.contenido = contenido;
                post.url_contenido = url_contenido;
                post.url_imagen = url_imagen;
                post.tipo_contenido = tipo_contenido;
                post.id_cuenta = Int32.Parse(idCuenta);

                post.GuardarPost();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
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
            catch (Exception e)
            {
                if (e.Message == "DUPLICATE_ENTRY")
                    throw new Exception("El evento ya existe");
                return false;
            }

        }

        public static void ElimiarPost(string id)
        {
            try
            {
                ModeloPost post = new ModeloPost();
                post.id_post = Int32.Parse(id);
                post.EliminarPost();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }

        public static void ElimiarEvento(string id_post, string id_evento)
        {
            try
            {
                ElimiarPost(id_post);
                ModeloPost evento = new ModeloPost();
                evento.id_evento = Int32.Parse(id_evento);
                evento.EliminarEvento();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }

        public static void ModificarPost(string id, string contenido, string url, string tipo_contenido)
        {
            try
            {
                ModeloPost post = new ModeloPost();
                post.id_post = Int32.Parse(id);
                post.contenido = contenido;
                post.url_contenido = url;
                post.tipo_contenido = tipo_contenido;
                post.GuardarPost();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }

        public static void ModificarEvento(string Id_Post, string id_evento, string url_contenido, string tipo_contenido, string contenido, string nombre_evento, string imagen, string descripcion_evento, string id_cuenta)
        {
            try
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
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }

        public static void CompartirPostEnMuro(string id_post, string id_muro)
        {
            try
            {
                ModeloPost post = new ModeloPost();

                post.id_post = Int32.Parse(id_post);
                post.id_muro = Int32.Parse(id_muro);

                post.CompartirPostEnMuro();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }

        public static void CompartirPostEnGrupo(string id_post, string id_grupo)
        {
            try
            {
                ModeloPost post = new ModeloPost();

                post.id_post = Int32.Parse(id_post);
                post.id_grupo = Int32.Parse(id_grupo);

                post.CompartirPostEnGrupo();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }


        public static DataTable ListarPostDeCuenta(string idCuenta)
        {
            try
            {
                DataTable tabla = new DataTable();
                tabla.Columns.Add("id_post", typeof(int));
                tabla.Columns.Add("Contenido", typeof(string));
                tabla.Columns.Add("id_cuenta", typeof(string));
                tabla.Columns.Add("Likes", typeof(int));
                tabla.Columns.Add("Url_imagen", typeof(string));

                ModeloPost post = new ModeloPost();
                foreach (ModeloPost p in post.ObtenerPostsDeCuenta(Int32.Parse(idCuenta)))
                {
                    DataRow fila = tabla.NewRow();
                    fila["Id_post"] = p.id_post;
                    fila["Contenido"] = p.contenido;
                    fila["id_cuenta"] = p.id_cuenta;
                    fila["Likes"] = p.NumeroDeLikes(p.id_post);
                    fila["Url_imagen"] = p.url_imagen;

                    tabla.Rows.Add(fila);
                }

                return tabla;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return null;
            }

        }

        public static DataTable ListarPostDeGrupo(string id_grupo)
        {
            try
            {
                DataTable tabla = new DataTable();
                tabla.Columns.Add("id_post", typeof(int));
                tabla.Columns.Add("Contenido", typeof(string));
                tabla.Columns.Add("id_cuenta", typeof(string));
                tabla.Columns.Add("Likes", typeof(int));
                tabla.Columns.Add("Url_imagen", typeof(string));

                ModeloPost post = new ModeloPost();
                foreach (ModeloPost p in post.ObtenerPostsDeGrupo(Int32.Parse(id_grupo)))
                {
                    DataRow fila = tabla.NewRow();
                    fila["Id_post"] = p.id_post;
                    fila["Contenido"] = p.contenido;
                    fila["id_cuenta"] = p.id_cuenta;
                    fila["Likes"] = p.NumeroDeLikes(p.id_post);
                    fila["Url_imagen"] = p.url_imagen;
                    tabla.Rows.Add(fila);
                }

                return tabla;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return null;
            }

        }
        public static DataTable ListarPostDeMuro(string id_muro)
        {
            try
            {
                DataTable tabla = new DataTable();
                tabla.Columns.Add("id_post", typeof(int));
                tabla.Columns.Add("Contenido", typeof(string));
                tabla.Columns.Add("id_cuenta", typeof(string));
                tabla.Columns.Add("Likes", typeof(int));
                tabla.Columns.Add("Url_imagen", typeof(string));

                ModeloPost post = new ModeloPost();
                foreach (ModeloPost p in post.ObtenerPostsDeMuro(Int32.Parse(id_muro)))
                {
                    DataRow fila = tabla.NewRow();
                    fila["Id_post"] = p.id_post;
                    fila["Contenido"] = p.contenido;
                    fila["id_cuenta"] = p.id_cuenta;
                    fila["Likes"] = p.NumeroDeLikes(p.id_post);
                    fila["Url_imagen"] = p.url_imagen;
                    tabla.Rows.Add(fila);
                }

                return tabla;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return null;
            }

        }
        public static DataTable ListarTodosLosPost()
        {
            try
            {
                DataTable tabla = new DataTable();
                tabla.Columns.Add("id_post", typeof(int));
                tabla.Columns.Add("Contenido", typeof(string));
                tabla.Columns.Add("id_cuenta", typeof(string));
                tabla.Columns.Add("Likes", typeof(int));
                tabla.Columns.Add("Url_imagen", typeof(string));

                ModeloPost post = new ModeloPost();
                foreach (ModeloPost p in post.ObtenerPosts())
                {
                    DataRow fila = tabla.NewRow();
                    fila["Id_post"] = p.id_post;
                    fila["Contenido"] = p.contenido;
                    fila["id_cuenta"] = p.id_cuenta;
                    fila["Likes"] = p.NumeroDeLikes(p.id_post);
                    fila["Url_imagen"] = p.url_imagen;
                    tabla.Rows.Add(fila);
                }

                return tabla;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return null;
            }

        }



        public static DataTable AlgoritmoPost() // a ver----------
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("id_post", typeof(int));
            tabla.Columns.Add("Contenido", typeof(string));
            tabla.Columns.Add("Tipo_Contenido", typeof(string));
            tabla.Columns.Add("id_cuenta", typeof(string));
            tabla.Columns.Add("url_contenido", typeof(string));
            tabla.Columns.Add("url_imagen", typeof(string));
            tabla.Columns.Add("fecha_creacion", typeof(string));
            tabla.Columns.Add("Likes", typeof(int));


            ModeloPost post = new ModeloPost();
            foreach (ModeloPost p in post.ObtenerPosts())
            {
                DataRow fila = tabla.NewRow();
                fila["Id_post"] = p.id_post;
                fila["Contenido"] = p.contenido;
                fila["Tipo_Contenido"] = p.tipo_contenido;
                fila["id_cuenta"] = p.id_cuenta;
                fila["Likes"] = p.NumeroDeLikes(p.id_post);
                tabla.Rows.Add(fila);
            }
            return tabla;
        }

        public static string ObtenerCreadorDePost(string id_cuenta)
        {
            try
            {
                ModeloPost post = new ModeloPost();
                post.id_cuenta = Int32.Parse(id_cuenta);
                return post.ObtenerCreadorDePost();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return null;
            }
        }

        public static void AñadirLike(string id_cuenta, string id_post)
        {
            try
            {
                ModeloPost post = new ModeloPost();
                post.id_cuenta = Int32.Parse(id_cuenta);
                post.id_post = Int32.Parse(id_post);

                post.AñadirLike();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void EliminarLike(string id_cuenta, string id_post, string id_upvote)
        {
            try
            {
                ModeloPost post = new ModeloPost();
                post.id_cuenta = Int32.Parse(id_cuenta);
                post.id_post = Int32.Parse(id_post);
                post.id_upvote = Int32.Parse(id_upvote);

                post.EliminarLike();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }


        private static void ErrorHandle(Exception ex)
        {
            if (ex.Message == "DUPLICATE_ENTRY")
                throw new Exception("DUPLICATE_ENTRY");
            if (ex.Message == "ACCESS_DENIED")
                throw new Exception("ACCESS_DENIED");
            if (ex.Message == "UNKNOWN_COLUMN")
                throw new Exception("UNKNOWN_COLUMN");
            if (ex.Message == "UNKNOWN_DB_ERROR")
                throw new Exception("UNKNOWN_DB_ERROR");
            if (ex.Message == "ERROR_CHILD_ROW")
                throw new Exception("ERROR_CHILD_ROW");

            throw new Exception("UNKNOWN_ERROR");
        }

    }
}
