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

        List<int> IdPostMostrados = new List<int>();

        public static void CrearPost(string contenido,string url,string tipo_contenido, string idCuenta)
        {
            try
            {
                ModeloPost post = new ModeloPost();
                post.contenido = contenido;
                post.url_contenido = url;
                post.tipo_contenido = tipo_contenido;
                post.id_cuenta = Int32.Parse(idCuenta);

                post.GuardarPost();
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
            try
            {
                ModeloPost post = new ModeloPost();
                post.id_post = Int32.Parse(id);
                post.EliminarPost();
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

        public static void ElimiarEvento(string id_post, string id_evento)
        {
            try
            {
                ElimiarPost(id_post);
                ModeloPost evento = new ModeloPost();
                evento.id_evento = Int32.Parse(id_evento);
                evento.EliminarEvento();
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

        public static void ModificarPost(string id, string contenido,string url,string tipo_contenido)
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
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
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
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
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
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public static void CompartirPostEnGrupo(string id_post, string id_grupo)
        {
            try
            {
                ModeloPost post = new ModeloPost();

                post.id_post = Int32.Parse(id_post);
                post.id_muro = Int32.Parse(id_grupo);

                post.CompartirPostEnGrupo();
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


        public static DataTable Listar(string idCuenta)  
        {
            try
            {
                DataTable tabla = new DataTable();
                tabla.Columns.Add("id_post", typeof(int));
                tabla.Columns.Add("Contenido", typeof(string));
                tabla.Columns.Add("id_cuenta", typeof(string));

                ModeloPost pizza = new ModeloPost();
                foreach (ModeloPost p in pizza.ObtenerPostsDeCuenta(Int32.Parse(idCuenta)))
                {
                    DataRow fila = tabla.NewRow();
                    fila["Id_post"] = p.id_post;
                    fila["Contenido"] = p.contenido;
                    fila["id_cuenta"] = p.id_cuenta;
                    tabla.Rows.Add(fila);
                }

                return tabla;
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

        public static DataTable ListarPosts()
        {
            try
            {
                DataTable tabla = new DataTable();
                tabla.Columns.Add("id_post", typeof(int));
                tabla.Columns.Add("Contenido", typeof(string));
                tabla.Columns.Add("id_cuenta", typeof(string));

                ModeloPost pizza = new ModeloPost();
                foreach (ModeloPost p in pizza.ObtenerPosts())
                {
                    DataRow fila = tabla.NewRow();
                    fila["Id_post"] = p.id_post;
                    fila["Contenido"] = p.contenido;
                    fila["id_cuenta"] = p.id_cuenta;
                    tabla.Rows.Add(fila);
                }

                return tabla;
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


        public Dictionary<string,string> AlgoritmoPost() // a ver----------
        {
            Dictionary<string, string> post = new Dictionary<string, string>();
            ModeloPost p = new ModeloPost();
            int IdMostrada = 0;
            //IdPostMostrados.Add(IdMostrada);
            bool FueMostrado = IdPostMostrados.Contains(IdMostrada);
            Console.WriteLine(IdPostMostrados.ToString());

            while (true)
            {
                if(!IdPostMostrados.Contains(IdMostrada) && p.BuscarPostRandom())
                {
                    post.Add("contenido", p.contenido);
                    post.Add("fecha", p.fecha_post);
                    post.Add("tipo_contenido", p.tipo_contenido);
                    post.Add("id_cuenta", p.id_cuenta.ToString());
                    post.Add("id_post", p.id_post.ToString());
                    IdMostrada = Int32.Parse(p.id_post.ToString());
                    Console.WriteLine(IdPostMostrados.ToString());
                    IdPostMostrados.Add(IdMostrada);
                    Console.WriteLine(IdPostMostrados.ToString());
                    return post;
                    break;
                }
                return null;
            }

        }

        
        public static string ObtenerCreadorDePost(string id_cuenta)
        {
            try
            {
                ModeloPost post = new ModeloPost();
                post.id_cuenta = Int32.Parse(id_cuenta);
                return post.ObtenerCreadorDePost();
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

        public static void AñadirLike(string id_cuenta,string id_post)
        {
            try
            {
                ModeloPost post = new ModeloPost();
                post.id_cuenta = Int32.Parse(id_cuenta);
                post.id_post = Int32.Parse(id_post);

                post.AñadirLike();
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
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
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
