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
        public static void CrearComentario(string idCuenta, string idPost,string comentario)
        {
            try
            {
                ModeloComentario coment = new ModeloComentario();
                coment.idCuenta = Int32.Parse(idCuenta);
                coment.IdPost = Int32.Parse(idPost);
                coment.Contenido = comentario;

                coment.GuardarComentario();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }

        }

        public static void EliminarComentario(string idcoment)
        {
            try
            {
                ModeloComentario coment = new ModeloComentario();
                coment.IdComentario = Int32.Parse(idcoment);
                coment.EliminarComentario();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }

        public static void ModificarComentario(string idcoment,string comentario)
        {
            try
            {
                ModeloComentario coment = new ModeloComentario();
                coment.IdComentario = Int32.Parse(idcoment);
                coment.Contenido = comentario;
                coment.GuardarComentario();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }


        public static DataTable ListarComentarios(string idPost)
        {
            try
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
            catch (Exception e)
            {
                ErrorHandle(e);
                return null;
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