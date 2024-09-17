using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaVisual.DTO;
using MaterialSkin.Controls;
using Newtonsoft.Json;
using RestSharp;


namespace CapaVisual
{
    public partial class MainPage : MaterialForm
    {
        public MainPage()
        {
            InitializeComponent();
            var skinManager = MaterialSkin.MaterialSkinManager.Instance;

            // Asignar el formulario en el que estás trabajando
            skinManager.AddFormToManage(this);

            // Cambiar el esquema de colores (Claro u Oscuro)
            skinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;

            // Cambiar los colores primarios y de acento
            skinManager.ColorScheme = new MaterialSkin.ColorScheme(
                MaterialSkin.Primary.Red600,   // Color primario
                MaterialSkin.Primary.Red700,   // Color oscuro primario
                MaterialSkin.Primary.Red200,   // Color claro primario
                MaterialSkin.Accent.Green400,     // Color de acento
                MaterialSkin.TextShade.BLACK    // Sombra del texto
            );
        }

        private static List<PostDesdeAPI> obtenerPostDesdeAPI()
        {
            RestClient client = new RestClient("http://localhost:44331/");
            RestRequest request = new RestRequest("ApiPost/post/obtener-posts", Method.Get);
            request.AddHeader("Accept", "application/json");
            RestResponse response = client.Execute(request);

            List<PostDesdeAPI> posts;
            posts = JsonConvert.DeserializeObject<List<PostDesdeAPI>>(response.Content);
            return posts;
        }


        private void btnPostear_Click(object sender, EventArgs e)
        {

            List<PostDesdeAPI> post = obtenerPostDesdeAPI();
            //txtContenido.Text = post[1].contenido;
        }
    }
}
