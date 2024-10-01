using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Modelos
{
    public abstract class Modelo
    {
        public string IP;
        public string NombreBase;
        public string NombreDeUsuario;
        public string Password;
        public string Puerto;

        public MySqlConnection Conexion;
        public MySqlCommand Comando;
        public MySqlDataReader Lector;

        public Modelo()
        {
            this.IP = "127.0.0.1";
            this.NombreBase = "LinguaLinkDB";
            this.Password = "1234";
            this.NombreDeUsuario = "root";
            this.Puerto = "3306";

            this.Conexion = new MySqlConnection(
                $"server={this.IP};" +
                $"user={this.NombreDeUsuario};" +
                $"password={this.Password};" +
                $"database={this.NombreBase};" +
                $"port={this.Puerto};"
            );

            try
            {

                this.Conexion.Open();

                this.Comando = new MySqlCommand();
                this.Comando.Connection = this.Conexion;
            }
            catch (Exception sqlex)
            {
                throw new Exception("CANNOT_CONNECT_TO_DB");
            }


        }

        // Funcion que agarra la salida de sql desde el programa y la guarda en un .txt en el escritorio,
        // crea el archivo de forma automatica en el escritorio, y se puede activar o desactivar cambiando el bool de abajo

        public void PrintDesktop(string sql) 
        {
            if (true) // cambiar a false para desactivar
            {
                string Ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                Ruta += "/SalidaSQL.txt";
                sql += ";";
                System.IO.File.AppendAllText(Ruta, sql + Environment.NewLine);
            }
        }

    }
}