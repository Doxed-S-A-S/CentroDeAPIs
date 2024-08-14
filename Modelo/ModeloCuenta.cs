using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class ModeloCuenta : Modelo
    {
        public int id_cuenta;
        public string nombre_usuario;
        public string email;
        public string contraseña = "123"; //placeholder
        public string imagen_perfil = "pic"; //placeholder
        public int id_muro = 1; //placeholder
        public int id_preferencia = 1; //placeholder


        public void CrearCuenta()
        {
            string sql = $"insert into cuenta (nombre_usuario,email,contraseña,imagen_perfil,id_muro,id_preferencia)" +
                $" values('{this.nombre_usuario}','{this.email}','{this.contraseña}','{this.imagen_perfil}',{this.id_muro},{this.id_preferencia})";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void ModificarContraseña()
        {
            string sql = $"update cuenta set contrasena ='{this.contraseña}'where id_cuenta ='{this.id_cuenta}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void ModificarCorreo()
        {
            string sql = $"update cuetna set email ='{this.email}'where id_cuenta ='{this.id_cuenta}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public void EliminarCuenta()
        {
            string sql = $"update cuenta set eliminado = true where id_cuenta ='{this.id_cuenta}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

        public List<ModeloCuenta> ObtenerCuentas()
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
                cuenta.email = this.Lector["email"].ToString();
                cuentas.Add(cuenta);
            }
            this.Lector.Close();
            return cuentas;
        }

        /************************************* Modificaciones del usuario ********************************/
    }
}
