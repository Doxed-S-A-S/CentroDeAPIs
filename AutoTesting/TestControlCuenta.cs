using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Controlador;

namespace PruebaAutomatica
{
    [TestClass]
    public class ControlCuentaTest
    {
        private string GenerarStringRandom()
        {
            return DateTime.Now.Ticks.ToString();
        }

        private string GenerarEmailRandom()
        {
            return $"test_{DateTime.Now.Ticks}@gmail.com";
        }

        [TestMethod]
        public void TestCrearCuenta()
        {
            bool resultado;
            Dictionary<string, string> cuenta;

            try
            {
                ControlCuenta.CrearCuenta(
                    GenerarStringRandom(),
                    GenerarEmailRandom(),
                    "qQ123456789",
                    "Juan",
                    "Algo",
                    "Algo2",
                    "china",
                    "Español",
                    "ImagenPerfilPrueba"
                );

                cuenta = ControlCuenta.BuscarUsuario("1");
                resultado = cuenta["resultado"] == "true";
            }
            catch (Exception)
            {
                resultado = false;
                cuenta = null;
            }

            Assert.IsTrue(resultado);
            Assert.IsNotNull(cuenta);
        }

        [TestMethod]
        public void TestCrearCuentaDuplicada()
        {
            bool resultado;
            bool errorEsperado = false;
            string nombreUsuario = "usuarioDuplicado";

            try
            {
                ControlCuenta.CrearCuenta(
                    nombreUsuario,
                    "emailDuplicado@gmail.com",
                    "Qq123456789",
                    "juan",
                    "Algo",
                    "Algo2",
                    "China",
                    "Español",
                    "ImagenPerfilDuplicado"
                );

                ControlCuenta.CrearCuenta(
                    nombreUsuario,
                    "otroEmail@gmail.com",
                    "Qq123456789",
                    "Pepe",
                    "Argento",
                    "YoQueSe",
                    "Argentina",
                    "Español",
                    "OtraImagenPerfil"
                );

                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
                errorEsperado = ex.Message == "UsarioExistente";
            }

            Assert.IsFalse(resultado);
            Assert.IsTrue(errorEsperado);
        }

        [TestMethod]
        public void TestCrearCuentaSinDatos()
        {
            bool resultado;
            try
            {
                ControlCuenta.CrearCuenta(
                    "", "", "", "", "", "", "", "", ""
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
        public void TestModificarContraseña()
        {
            bool resultado;

            try
            {
                resultado = ControlCuenta.ModificarContraseña("2", "1234", "123sd4");
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestModificarContraseñaIncorrecta()
        {
            bool resultado;

            try
            {
                resultado = ControlCuenta.ModificarContraseña("2", "12sad34", "a12345");
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void TestModificarContraseñaSinUnDato()
        {
            bool resultado;

            try
            {
                resultado = ControlCuenta.ModificarContraseña("1", "123456", "");
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void TestModificarCorreo()
        {
            bool resultado;

            try
            {
                resultado = ControlCuenta.ModificarCorreo("1", GenerarEmailRandom());
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestModificarCorreoInvalido()
        {
            bool resultado;

            try
            {
                resultado = ControlCuenta.ModificarCorreo("1", "aaaaaaaaaaa@12345.12345");
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void TestModificarCorreoSinID()
        {
            bool resultado;

            try
            {
                resultado = ControlCuenta.ModificarCorreo("", GenerarEmailRandom());
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsFalse(resultado);
        }
    }
}
