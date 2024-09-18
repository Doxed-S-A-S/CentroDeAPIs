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

            flowLayoutPanelPosts.Scroll += new ScrollEventHandler(flowLayoutPanel1_Scroll);
            flowLayoutPanelPosts.MouseWheel += flowLayoutPanel1_MouseWheel;
            var skinManager = MaterialSkin.MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
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

        private void mostrarPostsIniciales()
        {
            List<PostDesdeAPI> posts = obtenerPostDesdeAPI();

            // Paso 2: Limpiar el FlowLayoutPanel si ya tiene controles
            //flowLayoutPanelPosts.Controls.Clear();

            // Paso 3: Crear dinámicamente un MaterialCard para cada post
            foreach (PostDesdeAPI post in posts)
            {
                // Crear un MaterialCard
                MaterialCard card = new MaterialCard();
                card.Width = 300;
                card.Height = 200;
                card.Padding = new Padding(10);

                // Crear un Label para el nombre de usuario
                Label lblUsuario = new Label();
                //lblUsuario.Text = post.UsuarioNombre; // Asumiendo que tienes un atributo UsuarioNombre
                lblUsuario.Font = new Font("Arial", 10, FontStyle.Bold);
                lblUsuario.AutoSize = true;
                card.Controls.Add(lblUsuario);

                // Crear un TextBox para el contenido del post
                TextBox txtContenido = new TextBox();
                //txtContenido.Text = post.Contenido; // Asumiendo que tienes un atributo Contenido
                txtContenido.Multiline = true;
                txtContenido.Width = 250;
                txtContenido.Height = 100;
                txtContenido.ReadOnly = true;
                card.Controls.Add(txtContenido);

                // Agregar el MaterialCard al FlowLayoutPanel
                flowLayoutPanelPosts.Controls.Add(card);
            }
        }
        private static List<PostDesdeAPI> obtenerPostDesdeAPI()
        {
            RestClient client = new RestClient("http://localhost:44331/");
            RestRequest request = new RestRequest("ApiPost/post/obtener-posts/1", Method.Get);
            request.AddHeader("Accept", "application/json");
            RestResponse response = client.Execute(request);

            List<PostDesdeAPI> posts;
            posts = JsonConvert.DeserializeObject<List<PostDesdeAPI>>(response.Content);
            return posts;
        }

        private static void obtenerCreadorDePost()
        {

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

            
            flowLayoutPanelPosts.Controls.Add(materialCard);
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
            
            if (flowLayoutPanelPosts.VerticalScroll.Value + flowLayoutPanelPosts.ClientSize.Height >= flowLayoutPanelPosts.VerticalScroll.Maximum)
            {
                
                CrearMaterialCard("Nuevo post al llegar al fondo");
            }
        }

        
        private void flowLayoutPanel1_MouseWheel(object sender, MouseEventArgs e)
        {
            
            if (flowLayoutPanelPosts.VerticalScroll.Value + flowLayoutPanelPosts.ClientSize.Height >= flowLayoutPanelPosts.VerticalScroll.Maximum)
            {
                
                CrearMaterialCard("Nuevo post al llegar al fondo usando MouseWheel");
            }
        }

        private void btnPostear_Click_1(object sender, EventArgs e)
        {
            List<PostDesdeAPI> post = obtenerPostDesdeAPI();
            txtContenido.Text = post[1].contenido;
        }
    }

}
