using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelos;
using System.Data;
using Newtonsoft.Json;
using RestSharp;

namespace Controlador
{
    public class ControlGrupo
    {
        public static void CrearGrupo(string id_cuenta, string nombreGrupo, string descripcion,string privacidad, string banner)
        {
            ModeloGrupo grupo = new ModeloGrupo();
            grupo.nombre_grupo = nombreGrupo;
            grupo.descripcion = descripcion;
            grupo.privacidad = bool.Parse(privacidad) ;
            grupo.banner = banner;
            grupo.id_cuenta = Int32.Parse(id_cuenta);

            grupo.CrearGrupo();
        }

        public static bool ModificarGrupo(string id, string nombre, string descripcion, string banner)
        {
            ModeloGrupo grupo = new Modelos.ModeloGrupo();
            if (grupo.BuscarGrupo(Int32.Parse(id)))
            {
                grupo.nombre_grupo = nombre;
                grupo.descripcion = descripcion;
                grupo.banner = banner;

                grupo.Guardar();
                return true;
            }

            return false;
        }

        public static void ModificarNombreGrupo(string id, string nombre)
        {
            ModeloGrupo grupo = new Modelos.ModeloGrupo();
            grupo.id_grupo = Int32.Parse(id);
            grupo.nombre_grupo = nombre;

            grupo.ModificarNombreGrupo();

        }

        public static void ModificarDescripcionGrupo(string id, string descripcion)
        {
            ModeloGrupo grupo = new Modelos.ModeloGrupo();
            grupo.id_grupo = Int32.Parse(id);
            grupo.nombre_grupo = descripcion;

            grupo.ModificarDescripcionGrupo();

        }

        public static void ModificarPrivacidadGrupo(string id_grupo,string privacidad)
        {
            ModeloGrupo grupo = new Modelos.ModeloGrupo();
            grupo.id_grupo = Int32.Parse(id_grupo);
            grupo.privacidad = bool.Parse(privacidad);

            grupo.ModificarPrivacidadGrupo();
        }

        public static void ModificarBannerGrupo(string id, string banner)
        {
            ModeloGrupo grupo = new Modelos.ModeloGrupo();
            grupo.id_grupo = Int32.Parse(id);
            grupo.nombre_grupo = banner;

            grupo.ModificarBannerGrupo();

        }

        public static bool EliminarGrupo(string id)
        {
            ModeloGrupo grupo = new Modelos.ModeloGrupo();
            if (grupo.BuscarGrupo(Int32.Parse(id)))
            {
                grupo.EliminarGrupo();
                return true;
            }

            return false;
        }

        public static DataTable ObtenerGrupos()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("id_grupo", typeof(int));
            tabla.Columns.Add("nombre_grupo", typeof(string));


            ModeloGrupo grupo = new ModeloGrupo();
            foreach (ModeloGrupo p in grupo.ObtenerGrupos())
            {
                DataRow fila = tabla.NewRow();
                fila["id_grupo"] = p.id_grupo;
                fila["nombre_grupo"] = p.nombre_grupo;
                tabla.Rows.Add(fila);
            }

            return tabla;

        }

        public static DataTable ObtenerIntegrantesDeGrupo(string id_grupo)
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("nombre_grupo", typeof(string));
            tabla.Columns.Add("nombre_usuario", typeof(string));
            tabla.Columns.Add("rol", typeof(string));


            ModeloGrupo grupo = new ModeloGrupo();
            foreach (ModeloGrupo p in grupo.ObtenerIntegrantesDeGrupo(Int32.Parse(id_grupo)))
            {
                DataRow fila = tabla.NewRow();
                fila["nombre_grupo"] = p.nombre_grupo;
                fila["nombre_usuario"] = p.nombre_usuario;
                fila["rol"] = p.rol;
                tabla.Rows.Add(fila);
            }

            return tabla;

        }

        public static Dictionary<string, string> BuscarGrupo(string id_grupo)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            ModeloGrupo grupo = new ModeloGrupo();
            if (grupo.BuscarGrupo(Int32.Parse(id_grupo)))
            {
                resultado.Add("resultado", "true");
                resultado.Add("id", grupo.id_grupo.ToString());
                resultado.Add("nombre_grupo", grupo.nombre_grupo);
                resultado.Add("descripcion", grupo.descripcion);
                resultado.Add("banner", grupo.banner);
                return resultado;
            }
            resultado.Add("resultado", "false");
            return resultado;
        }

        public static Dictionary<string, string> AgregarCuentaEnGrupo(string rol, string id_grupo, string id_cuenta)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();

            ModeloGrupo grupo = new ModeloGrupo();
            grupo.id_cuenta = Int32.Parse(id_cuenta);
            grupo.id_grupo = Int32.Parse(id_grupo);
            grupo.rol = rol;

            if (!grupo.FormaParteDelGrupo())
            {
                grupo.AgregarCuentaEnGrupo();
                resultado.Add("resultado", "true");
                return resultado;
            }
            resultado.Add("resultado", "false");
            return resultado;

        }
        public static Dictionary<string, string> EliminarCuentaDeGrupo(string id_grupo, string id_cuenta)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();

            ModeloGrupo grupo = new ModeloGrupo();
            grupo.id_cuenta = Int32.Parse(id_cuenta);
            grupo.id_grupo = Int32.Parse(id_grupo);

            if (!grupo.FormaParteDelGrupo())
            {

                resultado.Add("resultado", "false");
                return resultado;
            }
            grupo.EliminarCuentaDeGrupo();
            resultado.Add("resultado", "true");
            return resultado;


        }
        public static Dictionary<string, string> CambiarRolDeCuentaEnGrupo(string id_cuenta, string id_grupo, string rol)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();

            ModeloGrupo grupo = new ModeloGrupo();
            grupo.id_cuenta = Int32.Parse(id_cuenta);
            grupo.id_grupo = Int32.Parse(id_grupo);
            grupo.rol = rol;

            if (!grupo.FormaParteDelGrupo())
            {

                resultado.Add("resultado", "false");
                return resultado;
            }
            grupo.CambiarRolDeCuentaEnGrupo();
            resultado.Add("resultado", "true");
            return resultado;
        }


    }
}
