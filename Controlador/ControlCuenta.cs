using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Modelos;

namespace Controlador

{
    public class ControlCuenta
    {
        public static void CrearCuenta(string nombreUsuario, string email, string contraseña, string nombre, string apellido, string apellido2, string pais, string idiomaHablado)
        {
            try
            {
                ModeloCuenta cuenta = new ModeloCuenta();
                cuenta.nombre_usuario = nombreUsuario;
                cuenta.email = email;
                cuenta.contraseña = contraseña;
                cuenta.nombre = nombre;
                cuenta.apellido1 = apellido;
                cuenta.apellido2 = apellido2;
                cuenta.pais = pais; 
                cuenta.idiomas_hablados = "eng";

                cuenta.Registro();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }
<<<<<<< Updated upstream
        
        public static bool Login (string nombre_usuario, string contraseña)
=======

        public static Dictionary<string, string> Login(string nombre_usuario, string contraseña)
>>>>>>> Stashed changes
        {
            try
            {
                ModeloCuenta c = new ModeloCuenta();
                c.nombre_usuario = nombre_usuario;
                c.contraseña = contraseña;

                return c.Autenticar();
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return false;
            }
        }

        public static bool ModificarContraseña(string id_Cuenta, string contraseña, string contraseñaAntigua)
        {
            try
            {
                ModeloCuenta c = new ModeloCuenta();
                if (c.ModificarContraseña(Int32.Parse(id_Cuenta)) && (c.ContraseñaExiste(Int32.Parse(id_Cuenta), contraseñaAntigua)))
                {
                    c.id_cuenta = Int32.Parse(id_Cuenta);
                    c.contraseña = contraseña;

                    c.ModificarContraseña(Int32.Parse(id_Cuenta));
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return false;
            }

        }

        public static bool ModificarCorreo(string id_cuenta, string email)
        {
            try
            {
                ModeloCuenta cuenta = new ModeloCuenta();
                if (cuenta.ModificarCorreo(Int32.Parse(id_cuenta)))
                {
                    cuenta.id_cuenta = Int32.Parse(id_cuenta);
                    cuenta.email = email;

                    cuenta.ModificarCorreo(Int32.Parse(id_cuenta));
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return false;
            }
        }

        public static bool ModificarPreferencias(string idCuenta, string idioma, Boolean recordarContraseña, string preferenciaContenido,
    Boolean notificacionPush, Boolean privacidad, string apariencia)
        {
            try
            {
                ModeloCuenta cuenta = new ModeloCuenta();
                if (cuenta.ObtenerDatosDeCuenta(Int32.Parse(idCuenta)))
                {
                    cuenta.idioma_app = idioma;
                    cuenta.recordar_contraseña = recordarContraseña;
                    cuenta.preferencias_contenido = preferenciaContenido;
                    cuenta.notificaciones_push = notificacionPush;
                    cuenta.muro_privado = privacidad;
                    cuenta.tema_de_apariencia = apariencia;
                    cuenta.ModificarPreferencias();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return false;
            }
        }


        public static Dictionary<string, string> BuscarUsuario(string id)
        {
            try
            {
                Dictionary<string, string> usuario = new Dictionary<string, string>();
                ModeloCuenta u = new ModeloCuenta();
                if (u.ObtenerDatosDeCuenta(Int32.Parse(id)))
                {
                    usuario.Add("resultado", "true");
                    usuario.Add("id_usuario", u.id_cuenta.ToString());
                    usuario.Add("nombre_usuario", u.nombre_usuario);
                    usuario.Add("nombre_grupo", u.nombre);
                    usuario.Add("apellido1", u.apellido1);
                    usuario.Add("apellido2", u.apellido2);
                    usuario.Add("email", u.email);
                    usuario.Add("biografia", u.biografia);
                    usuario.Add("reports", u.reports.ToString());
                    return usuario;
                }
                usuario.Add("resultado", "false");
                return usuario;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return null;
            }
        }

        public static DataTable ListarCuentas()
        {
            try
            {
                DataTable tabla = new DataTable();
                tabla.Columns.Add("ID", typeof(int));
                tabla.Columns.Add("Usuario", typeof(string));
                tabla.Columns.Add("Rol", typeof(string));
                tabla.Columns.Add("Miembro desde", typeof(DateTime));

                ModeloCuenta cuenta = new ModeloCuenta();
                foreach (ModeloCuenta c in cuenta.ObtenerCuentas())
                {
                    DataRow fila = tabla.NewRow();
                    fila["ID"] = c.id_cuenta;
                    fila["Usuario"] = c.nombre_usuario;
                    fila["Rol"] = c.rol_cuenta;
                    fila["Miembro desde"] = c.miembro_desde;
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

        public static Dictionary<string, string> BuscarPreferencia(string idCuenta)
        {
            try
            {
                Dictionary<string, string> preferencia = new Dictionary<string, string>();
                ModeloCuenta cuenta = new ModeloCuenta();
                if (cuenta.BuscarPreferencias(Int32.Parse(idCuenta)))
                {
                    preferencia.Add("resultado", "true");
                    preferencia.Add("tema de apariencia", cuenta.tema_de_apariencia);
                    preferencia.Add("idioma", cuenta.idioma_app);
                    preferencia.Add("preferencias", cuenta.preferencias_contenido);
                    preferencia.Add("recordar contraseña", cuenta.recordar_contraseña.ToString());
                    preferencia.Add("notificaciones push", cuenta.notificaciones_push.ToString());
                    preferencia.Add("muro privado", cuenta.muro_privado.ToString());
                    return preferencia;
                }
                preferencia.Add("resultado", "true");
                return preferencia;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return null;
            }
        }

<<<<<<< Updated upstream
=======
        public static Dictionary<string, string> CargarMuro(string idMuro)
        {
            try
            {
                Dictionary<string, string> muro = new Dictionary<string, string>();
                ModeloCuenta cuenta = new ModeloCuenta();
                cuenta.id_muro = Int32.Parse(idMuro);
                if (cuenta.BuscarMuro())
                {
                    muro.Add("resultado", "true");
                    muro.Add("Detalles", cuenta.detalles);
                    muro.Add("Biografia", cuenta.biografia);
                    muro.Add("Publicacion destacada", cuenta.pub_destacada.ToString());

                    return muro;
                }
                muro.Add("resultado", "true");
                return muro;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return null;
            }
        }

        public static DataTable UsuariosRelacionados(string idCuenta)
        {
            try
            {
                DataTable Relacion = new DataTable();
                Relacion.Columns.Add("Nombre", typeof(string));
                Relacion.Columns.Add("vinculo", typeof(string));
                Relacion.Columns.Add("ID vinculo", typeof(int));

                ModeloCuenta cuenta = new ModeloCuenta();
                cuenta.id_cuenta = Int32.Parse(idCuenta);
                foreach (ModeloCuenta c in cuenta.ObtenerRelacionados())
                {
                    DataRow fila = Relacion.NewRow();
                    fila["Nombre"] = c.nombre_usuario2;
                    fila["vinculo"] = c.vinculo;
                    fila["ID vinculo"] = c.id_cuenta2;
                    Relacion.Rows.Add(fila);
                }
                return Relacion;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return null;
            }
        }

        public static Dictionary<string, string> CargarCuenta(string idCuenta)
        {
            try
            {
                Dictionary<string, string> muro = new Dictionary<string, string>();
                ModeloCuenta cuenta = new ModeloCuenta();
                if (cuenta.BuscarCuenta(Int32.Parse(idCuenta)))
                {
                    muro.Add("resultado", "true");
                    muro.Add("ID cuenta", cuenta.id_cuenta.ToString());
                    muro.Add("ID muro", cuenta.id_muro.ToString());
                    muro.Add("ID preferencia", cuenta.id_preferencia.ToString());
                    muro.Add("ID usuario", cuenta.id_usuario.ToString());
                    muro.Add("Username", cuenta.nombre_usuario);
                    muro.Add("Publicacion destacada", cuenta.pub_destacada.ToString());

                    return muro;
                }
                muro.Add("resultado", "true");
                return muro;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
                return null;
            }
        }

        public static Dictionary<string, string> UsernameExiste(string username)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            ModeloCuenta cuenta = new ModeloCuenta();
            try
            {
                if (cuenta.UsernameExiste(username))
                {
                    resultado.Add("resultado", "true");
                    return resultado;
                }
                resultado.Add("resultado", "false");
                return resultado;
            }
            catch (Exception ex)
            {
                ErrorHandle(ex);
                resultado.Add("resultado", "false");
                return resultado;
            }

        }

>>>>>>> Stashed changes
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


