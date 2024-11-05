using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Modelos
{
    public class ModeloCuenta : Modelo
    {
        public long id_registro;
        public long id_cuenta;
        public long id_usuario;
        public long id_muro;
        public long id_preferencia;

        public long id_cuenta2;
        public string vinculo;
        public string nombre_usuario2;

        public string nombre_usuario;
        public string email;
        public string contraseña;
        public string imagen_perfil; //placeholder
        public string rol_cuenta;
        public string miembro_desde;
       
        public int reports;


        const int MYSQL_DUPLICATE_ENTRY = 1062;
        const int MYSQL_ACCESS_DENIED = 1045;
        const int MYSQL_UNKNOWN_COLUMN = 1054;
        const int MYSQL_ERROR_CHILD_ROW = 1452;

        public void Registro()
        {
            try
            {
                CrearCuenta();
                string sql = $"insert into registro (nombre_usuario,email,contrasena,id_cuenta) values(@username,@email,@contrasena,@id_cuenta)";
                this.Comando.CommandText = sql;
                this.Comando.Parameters.AddWithValue("@username", this.nombre_usuario);
                this.Comando.Parameters.AddWithValue("@email", this.email);
                this.Comando.Parameters.AddWithValue("@contrasena", this.contraseña);
                this.Comando.Parameters.AddWithValue("@id_cuenta", this.id_cuenta);
                this.Comando.Prepare();
                this.Comando.ExecuteNonQuery();
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
            }
            /*catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }*/
        }
        public void CrearCuenta()
        {
            CrearUsuario();
            CrearMuro();
            CrearPreferencias();
            try
            {
                string sql = $"insert into cuenta (nombre_usuario,imagen_perfil,id_usuario,id_muro,id_preferencia)" +
    $" values(@nombre_usuario,'{this.imagen_perfil}',{this.id_usuario},{this.id_muro},{this.id_preferencia})";
                this.Comando.CommandText = sql;
                this.Comando.Parameters.AddWithValue("@nombre_usuario", this.nombre_usuario);
                this.Comando.Prepare();
                this.Comando.ExecuteNonQuery();
                this.id_cuenta = this.Comando.LastInsertedId;
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

        public bool ModificarContraseña(int id)
        {
            try
            {
                if (VerificarRegistro(id))
                {
                    string sql = $"update registro set contrasena ='{this.contraseña}'where id_cuenta ='{this.id_cuenta}'";
                    this.Comando.CommandText = sql;
                    this.Comando.ExecuteNonQuery();
                    return true;
                }
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }

        }

        public bool ModificarCorreo(int id)
        {
            try
            {
                if (VerificarRegistro(id))
                {
                    string sql = $"UPDATE registro set email = '{this.email}' where id_cuenta = {this.id_cuenta}";
                    this.Comando.CommandText = sql;
                    this.Comando.ExecuteNonQuery();
                    return true;
                }
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public void EliminarCuenta()
        {
            try
            {
                string sql = $"update cuenta set eliminado = true where id_cuenta ='{this.id_cuenta}'";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
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

        public bool ObtenerDatosDeCuenta(int idCuenta)
        {
            try
            {
                if (BuscarCuenta(idCuenta))
                {
                    BuscarMuro();
                    BuscarPreferencias(idCuenta);
                    BuscarUsuario();
                    return true;
                }
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public bool BuscarCuenta(int id)
        {
            try
            {
                string sql = $"select * from cuenta where id_cuenta = {id} and eliminado = false";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                if (this.Lector.HasRows)
                {
                    this.Lector.Read();
                    this.id_cuenta = Int32.Parse(this.Lector["id_cuenta"].ToString());
                    this.nombre_usuario = this.Lector["nombre_usuario"].ToString();
                    //this.imagen_perfil = this.Lector["imagen_perfil"].ToString();
                    this.reports = Int32.Parse(this.Lector["reports"].ToString());
                    this.id_usuario = Int32.Parse(this.Lector["id_usuario"].ToString());
                    this.id_muro = Int32.Parse(this.Lector["id_muro"].ToString());
                    this.id_preferencia = Int32.Parse(this.Lector["id_preferencia"].ToString());
                    this.Lector.Close();
                    return true;
                }
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public bool VerificarRegistro(int id)
        {
            try
            {
                string sql = $"select count(*) from registro join cuenta on registro.id_cuenta = cuenta.id_cuenta " +
    $"where registro.id_cuenta = {id} and eliminado = false";
                this.Comando.CommandText = sql;
                string resultado = this.Comando.ExecuteScalar().ToString();

                if (resultado == "1")
                    return true;
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }

        }

        public bool Autenticar()
        {
            try
            {
                string sql = $"select count(*) from registro where nombre_usuario = @nombre_usuario and contrasena = @contrasena";
                this.Comando.CommandText = sql;
                this.Comando.Parameters.AddWithValue("@nombre_usuario", this.nombre_usuario);
                this.Comando.Parameters.AddWithValue("@contrasena", this.contraseña);
                this.Comando.Prepare();
                string resultado = this.Comando.ExecuteScalar().ToString();
                this.Comando.Parameters.Clear();

                if (resultado == "1")
                {
                    sql = $"select id_cuenta from registro where nombre_usuario = @nombre_usuario and contrasena = @contrasena";
                    this.Comando.CommandText = sql;
                    this.Comando.Parameters.AddWithValue("@nombre_usuario", this.nombre_usuario);
                    this.Comando.Parameters.AddWithValue("@contrasena", this.contraseña);
                    this.Comando.Prepare();
                    this.id_cuenta = Int32.Parse(this.Comando.ExecuteScalar().ToString());
                    return true;
                }

                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public bool ContraseñaExiste(int id,string ContraseñaAntigua)
        {
            try
            {
                string sql = $"select count(*) from registro where id_cuenta = '{id}' and contrasena = @contrasena";
                this.Comando.CommandText = sql;
                this.Comando.Parameters.AddWithValue("@contrasena", ContraseñaAntigua);
                this.Comando.Prepare();
                string resultado = this.Comando.ExecuteScalar().ToString();

                if (resultado == "1")
                    return true;
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public bool UsernameExiste(string username)
        {
            try
            {
                string sql = $"select count(*) from registro where nombre_usuario = '{username}'";
                this.Comando.CommandText = sql;
                this.Comando.Parameters.AddWithValue("@username", username);
                this.Comando.Prepare();
                string resultado = this.Comando.ExecuteScalar().ToString();

                if (resultado == "1")
                    return true;
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public List<ModeloCuenta> ObtenerCuentas()
        {
            try
            {
                List<ModeloCuenta> cuentas = new List<ModeloCuenta>();

                string sql = $"select * from cuenta where eliminado = false";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                while (this.Lector.Read())
                {
                    ModeloCuenta cuenta = new ModeloCuenta();
                    cuenta.id_cuenta = Int32.Parse(this.Lector["id_cuenta"].ToString());
                    cuenta.nombre_usuario = this.Lector["nombre_usuario"].ToString();
                    cuenta.rol_cuenta = this.Lector["rol_cuenta"].ToString();
                    cuenta.miembro_desde = this.Lector["fecha_registro"].ToString();
                    cuentas.Add(cuenta);
                }
                this.Lector.Close();
                return cuentas;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return null;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }


        public List<ModeloCuenta> ObtenerRelacionados()
        {
            try
            {
                List<ModeloCuenta> cuentas = new List<ModeloCuenta>();

                string sql = $"SELECT cuenta2.nombre_usuario AS nombre_usuario2, vinculados.tipo_vinculo, vinculados.id_cuenta2FROM cuenta " +
                    $"JOIN vinculados ON cuenta.id_cuenta = vinculados.id_cuenta1 " +
                    $"JOIN cuenta AS cuenta2 ON vinculados.id_cuenta2 = cuenta2.id_cuenta " +
                    $"WHERE cuenta.id_cuenta = {this.id_cuenta}";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                while (this.Lector.Read())
                {
                    ModeloCuenta cuenta = new ModeloCuenta();
                    cuenta.nombre_usuario2 = this.Lector["nombre_usuario2"].ToString();
                    cuenta.vinculo = this.Lector["tipo_vinculo"].ToString();
                    cuenta.id_cuenta2 = Int32.Parse(this.Lector["id_cuenta2"].ToString());

                    cuentas.Add(cuenta);
                }
                this.Lector.Close();
                return cuentas;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return null;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        /************************************* Usuario ********************************/

        public string nombre;
        public string apellido1;
        public string apellido2;
        public string pais;
        public string idiomas_hablados;

        public void CrearUsuario()
        {
            try
            {
                string sql = $"insert into usuario (nombre,apellido1,apellido2,pais,idiomas_hablados) " +
    $"values ('{this.nombre}','{this.apellido1}','{this.apellido2}','{this.pais}','{this.idiomas_hablados}')";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
                PrintDesktop(sql);
                id_usuario = this.Comando.LastInsertedId;
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

        public bool BuscarUsuario()
        {
            try
            {
                string sql = $"select * from usuario where id_usuario = '{this.id_usuario}'";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                if (Lector.HasRows)
                {
                    this.Lector.Read();
                    this.nombre = this.Lector["nombre_grupo"].ToString();
                    this.apellido1 = this.Lector["apellido1"].ToString();
                    this.apellido2 = this.Lector["apellido2"].ToString();
                    this.Lector.Close();
                    return true;
                }
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        /************************************* Muro ********************************/

        public string detalles = "";
        public int pub_destacada = 0;
        public string biografia = "";

        public void CrearMuro()
        {
            try
            {
                string sql = $"insert into muro (detalles,pub_destacada,biografia) values ('{this.detalles}',{this.pub_destacada},'{this.biografia}')";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
                PrintDesktop(sql);
                id_muro = this.Comando.LastInsertedId;
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

        public bool BuscarMuro()
        {
            try
            {
                string sql = $"select * from muro where id_muro = {this.id_muro}";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                if (Lector.HasRows)
                {
                    this.Lector.Read();
                    this.biografia = this.Lector["biografia"].ToString();
                    this.detalles = this.Lector["detalles"].ToString();
                    this.pub_destacada = Int32.Parse(this.Lector["pub_destacada"].ToString());
                    this.Lector.Close();
                    return true;
                }
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public void ModificarMuro()
        {
            ModificarDetalles();
            ModificarBiografia();
        }
        public void ModificarDetalles()
        {
            try
            {
                string sql = $"update muro set detalles ='{this.detalles}' where id_muro ='{this.id_muro}'";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
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

        public void ModificarBiografia()
        {
            try
            {
                string sql = $"update muro set biografia ='{this.biografia}' where id_muro ='{this.id_muro}'";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
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

        public void ModificarPublicacionDestacada()
        {
            try
            {
                string sql = $"update muro set pub_destacada ='{this.pub_destacada}' where id_muro ='{this.id_muro}'";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
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


        /************************************* Preferencias ********************************/

        public string tema_de_apariencia = "claro"; // dejar elejir en el futuro
        public string idioma_app;
        public bool recordar_contraseña;
        public string preferencias_contenido;
        public bool notificaciones_push;
        public bool muro_privado;


        public void CrearPreferencias()
        {
            try
            {
                string sql = $"insert into set_preferencias (tema_de_apariencia) value('{this.tema_de_apariencia}')";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
                PrintDesktop(sql);
                id_preferencia = this.Comando.LastInsertedId;
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

        public bool BuscarPreferencias(int idCuenta)
        {
            try
            {
                string sql = $"select * from set_preferencias where id_preferencia = {idCuenta}";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                if (Lector.HasRows)
                {
                    Lector.Read();
                    this.id_preferencia = Int32.Parse(this.Lector["id_preferencia"].ToString());
                    this.idioma_app = this.Lector["idioma_app"].ToString();
                    this.recordar_contraseña = Boolean.Parse(this.Lector["recordar_contrasena"].ToString());
                    this.preferencias_contenido = this.Lector["preferencias_contenido"].ToString();
                    this.notificaciones_push = Boolean.Parse(this.Lector["notificaciones_push"].ToString());
                    this.muro_privado = Boolean.Parse(this.Lector["muro_privado"].ToString());
                    this.tema_de_apariencia = this.Lector["tema_de_apariencia"].ToString();
                    this.Lector.Close();
                    return true;
                }
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }

        public void ModificarPreferencias()
        {
            try
            {
                string sql = $"update set_preferencias set idioma_app ='{this.idioma_app}', recordar_contrasena = {this.recordar_contraseña}," +
    $"preferencias_contenido = '{this.preferencias_contenido}',notificaciones_push ={this.notificaciones_push}," +
    $"muro_privado = {this.muro_privado},tema_de_apariencia = '{this.tema_de_apariencia}' where id_preferencia = {this.id_preferencia}";
                this.Comando.CommandText = sql;
                this.Comando.ExecuteNonQuery();
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

        public bool BuscarRegistro(int id_cuenta)
        {
            try
            {
                string sql = $"SELECT * FROM registro WHERE eliminado = false and id_cuenta = {id_cuenta}";
                this.Comando.CommandText = sql;
                this.Lector = this.Comando.ExecuteReader();

                if (this.Lector.HasRows)
                {
                    this.Lector.Read();
                    this.id_cuenta = Int32.Parse(this.Lector["Id"].ToString());
                    this.email = this.Lector["Nombre"].ToString();
                    this.Lector.Close();
                    return true;

                }
                this.Lector.Close();
                return false;
            }
            catch (MySqlException sqlx)
            {
                MySqlErrorCatch(sqlx);
                return false;
            }
            catch (Exception)
            {
                throw new Exception("UNKNOWN_ERROR");
            }
        }
        private void MySqlErrorCatch(MySqlException sqlx)
        {
            if (sqlx.Number == MYSQL_DUPLICATE_ENTRY)
                throw new Exception("DUPLICATE_ENTRY");
            if (sqlx.Number == MYSQL_ACCESS_DENIED)
                throw new Exception("ACCESS_DENIED");
            if (sqlx.Number == MYSQL_UNKNOWN_COLUMN)
                throw new Exception("UNKNOWN_COLUMN");
            if (sqlx.Number == MYSQL_ERROR_CHILD_ROW)
                throw new Exception("ERROR_CHILD_ROW");

            throw new Exception("UNKNOWN_DB_ERROR");
        }
    }
}
