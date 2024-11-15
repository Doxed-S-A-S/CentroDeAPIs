using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Controlador;

namespace PruebaAutomatica
{
    [TestClass]
    public class ControlGrupoTest
    {
        [TestMethod]
        public void TestCrearGrupo()
        {
            bool resultado;

            try
            {
                ControlGrupo.CrearGrupo(
                    "1",
                    "Tussi warror army",
                    "aguante los tussi wariors",
                    "true",
                    "banner.jpg",
                    "imagen.jpg"
                );
                var grupo = ControlGrupo.BuscarGrupo("1");
                resultado = grupo["resultado"] == "true";
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void TestGrupoDuplicado()
        {
            bool resultado;
            bool errorEsperado = false;

            try
            {
                ControlGrupo.CrearGrupo(
                    "1",
                    "aa",
                    "lo que sea",
                    "true",
                    "banner.jpg",
                    "imagen.jpg"
                );
                ControlGrupo.CrearGrupo(
                    "1",
                    "aa",
                    "lo que sea",
                    "true",
                    "banner.jpg",
                    "imagen.jpg"
                );
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
                errorEsperado = ex.Message == "DUPLICATE_ENTRY";
            }

            Assert.IsFalse(resultado);
            Assert.IsTrue(errorEsperado);
        }

        [TestMethod]
        public void TestGrupoSinDatos()
        {
            bool resultado;

            try
            {
                ControlGrupo.CrearGrupo("", "", "", "", "", "");
                resultado = true;
            }
            catch (Exception)
            {
                resultado = false;
            }

            Assert.IsFalse(resultado);
        }
    }
}
