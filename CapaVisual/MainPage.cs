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
using MaterialSkin;
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

            flowLayoutPanel1.Scroll += new ScrollEventHandler(flowLayoutPanel1_Scroll);
            flowLayoutPanel1.MouseWheel += flowLayoutPanel1_MouseWheel;
            var skinManager = MaterialSkin.MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
            skinManager.ColorScheme = new MaterialSkin.ColorScheme(
                MaterialSkin.Primary.Red600,
                MaterialSkin.Primary.Red700,
                MaterialSkin.Primary.Red200,
                MaterialSkin.Accent.Green400,
                MaterialSkin.TextShade.BLACK
                );

            button1.BackColor = Color.FromArgb(255, 128, 64); // Color personalizado
            richTextBox1.BackColor = Color.White;  // Ajustar color de texto
            txtContenido.BackColor = Color.White;
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
            txtContenido.Text = post[1].contenido;
        }

        private void CrearMaterialCard(string contenido)
        {
            
            MaterialSkin.Controls.MaterialCard materialCard = new MaterialSkin.Controls.MaterialCard
            {
                Width = 697,
                Height = 225,
                BackColor = Color.White 
            };

            
            Label lblContenido = new Label
            {
                Text = contenido,
                Location = new Point(10, 10), 
                AutoSize = true
            };

            
            materialCard.Controls.Add(lblContenido);

            
            flowLayoutPanel1.Controls.Add(materialCard);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            CrearMaterialCard("Contenido de la tarjeta 1");
            CrearMaterialCard("Contenido de la tarjeta 2");
            CrearMaterialCard("Contenido de la tarjeta 3");

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Scroll(object sender, ScrollEventArgs e)
        {
            
            if (flowLayoutPanel1.VerticalScroll.Value + flowLayoutPanel1.ClientSize.Height >= flowLayoutPanel1.VerticalScroll.Maximum)
            {
                
                CrearMaterialCard("Nuevo post al llegar al fondo");
            }
        }

        
        private void flowLayoutPanel1_MouseWheel(object sender, MouseEventArgs e)
        {
            
            if (flowLayoutPanel1.VerticalScroll.Value + flowLayoutPanel1.ClientSize.Height >= flowLayoutPanel1.VerticalScroll.Maximum)
            {
                
                CrearMaterialCard("Nuevo post al llegar al fondo usando MouseWheel");
            }
        }

        
        
    }

}
