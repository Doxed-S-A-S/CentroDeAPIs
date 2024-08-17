using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class ModeloRegistro : Modelo
    {
        public int id_usuario;
        public string nombre;
        public string apellido1;
        public string apellido2;
        public string pais;
        public string nombre_usuario;
        public string email;
        public string contrasena;

        public void InsertarUsuario()
        {
            string sql = $"insert into usuario (id_usuario,nombre,apellido1,apelido2,pais) values('{this.id_usuario}',{this.nombre}),'{this.apellido1}','{this.apellido2}','{this.pais}',";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }


        public void EliminarUsuario()
        {
            string sql = $"update cuenta set eliminado = true where id_usuario ='{this.id_usuario}'";
            this.Comando.CommandText = sql;
            this.Comando.ExecuteNonQuery();
        }

    }

    
}
