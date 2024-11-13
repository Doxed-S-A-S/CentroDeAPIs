using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using Controlador;

namespace PruebaAutomatica
{
    [TestClass]
    public class ControlPostsTest
    {
        private string GenerarStringRandom()
        {
            return DateTime.Now.Ticks.ToString();
        }

        string contendio = "hola";

        [TestMethod]
        public void TestCrearPost()
        {
            bool resultado;

            try
            {
                ControlPosts.CrearPost(
                    GenerarStringRandom(),
                    "https://asp.net.com",
                    "https://pepeargento.jpg",
                    "Pepe argento tomando mate",
                    "1"
                    );

                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestCrearPostSinDatos()
        {
            bool resultado;
            try
            {
                ControlPosts.CrearPost(
                "", "", "", "", ""
                );
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void TestEliminarPost()
        {
            bool resultado;

            try
            {
                ControlPosts.ElimiarPost("1");

                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestEliminarPostNoExistente()
        {
            bool resultado;

            try
            {
                ControlPosts.ElimiarPost("-1");

                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestListarPostsDeCuenta()
        {
            bool resultado;
            try
            {
                DataTable TablaPost = ControlPosts.ListarPostDeCuenta("1");
                resultado = TablaPost.Rows.Count > 0;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado);
        }


    }
}