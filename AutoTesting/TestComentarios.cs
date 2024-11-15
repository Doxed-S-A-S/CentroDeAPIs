using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using Controlador;

namespace PruebaAutomatica
{
    [TestClass]
    public class ControlComentariosTest
    {
        private string GenerarStringRandom()
        {
            return DateTime.Now.Ticks.ToString();
        }

        [TestMethod]
        public void TestCrearComentario()
        {
            bool resultado;

            try
            {
                ControlComentarios.CrearComentario("1", "1", GenerarStringRandom());
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestCrearComentarioSinDatos()
        {
            bool resultado;

            try
            {
                ControlComentarios.CrearComentario("", "", "");
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void TestEliminarComentario()
        {
            bool resultado;

            try
            {
                ControlComentarios.EliminarComentario("1");
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestEliminarComentarioNoExistente()
        {
            bool resultado;

            try
            {
                ControlComentarios.EliminarComentario("-1");
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestListarComentarios()
        {
            bool resultado;

            try
            {
                DataTable tablaComentarios = ControlComentarios.ListarComentarios("1");
                resultado = tablaComentarios.Rows.Count > 0;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestListarComentariosDePostInexistente()
        {
            bool resultado;

            try
            {
                DataTable tablaComentarios = ControlComentarios.ListarComentarios("-1");
                resultado = tablaComentarios.Rows.Count == 0;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestAñadirLikeComentario()
        {
            bool resultado;

            try
            {
                ControlComentarios.AñadirLikeComentario("1", "1");
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestAñadirLikeComentarioInexistente()
        {
            bool resultado;

            try
            {
                ControlComentarios.AñadirLikeComentario("-1", "1");
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestEliminarLikeComentario()
        {
            bool resultado;

            try
            {
                ControlComentarios.EliminarLikeComent("1", "1", "1");
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestEliminarLikeComentarioNoExistente()
        {
            bool resultado;

            try
            {
                ControlComentarios.EliminarLikeComent("-1", "1", "-1");
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }
    }
}
