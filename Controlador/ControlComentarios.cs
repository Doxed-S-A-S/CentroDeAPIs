﻿using System;
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
                tabla.Columns.Add("Fecha de creacion", typeof(string));
                tabla.Columns.Add("Likes", typeof(int));

                ModeloComentario coment = new ModeloComentario();
                foreach (ModeloComentario c in coment.ObtenerComentarios(idPost))
                {
                    DataRow fila = tabla.NewRow();
                    fila["IdComentario"] = c.IdComentario;
                    fila["IdPost"] = c.IdPost;
                    fila["Comentario"] = c.Contenido;
                    fila["Fecha de creacion"] = c.fechaCreacion;
                    fila["Likes"] = c.NumeroLikesComentario(c.IdComentario);
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

        public static DataTable obtenerCreadorComentarioYSuFoto(string id_comentario)
        {
            try
            {
                DataTable tabla = new DataTable();
                tabla.Columns.Add("id_cuenta", typeof(int));
                tabla.Columns.Add("nombre_usuario", typeof(string));
                tabla.Columns.Add("imagen_perfil", typeof(string));
                

                ModeloComentario cuenta = new ModeloComentario();
                foreach (ModeloCuenta c in cuenta.obtenerCreadorComentarioYSuFoto(Int32.Parse(id_comentario)))
                {
                    DataRow fila = tabla.NewRow();
                    fila["id_cuenta"] = c.id_cuenta;
                    fila["nombre_usuario"] = c.nombre_usuario;
                    fila["imagen_perfil"] = c.imagen_perfil;
                    
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

        public static void AñadirLikeComentario(string id_comentario,string id_post)
        {
            try
            {
                ModeloComentario coment = new ModeloComentario();
                coment.IdComentario = Int32.Parse(id_comentario);
                coment.IdPost = Int32.Parse(id_post);

                coment.AñadirLikeComent();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }

        public static void EliminarLikeComent(string id_comentario, string id_post,string id_upvote)
        {
            ModeloComentario coment = new ModeloComentario();
            coment.IdComentario = Int32.Parse(id_comentario);
            coment.IdPost = Int32.Parse(id_post);
            coment.idUpvote = Int32.Parse(id_upvote);

            coment.EliminarLikeComent();
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