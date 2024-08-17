using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft;

namespace Modelo
{
    class ModeloREST
    {
        public string Url = "http://localhost:57063/";
        public RestClient client;
        public RestRequest request;
        public RestResponse response;

    }
}
