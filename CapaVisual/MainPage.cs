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

        private OpenFileDialog ofd;
        public MainPage()
        {
            InitializeComponent();
            mostrarPostsIniciales();
            ofd = new OpenFileDialog();
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

            
        }

        private void mostrarPostsIniciales()
        {
            List<PostDesdeAPI> posts = obtenerPostDesdeAPI();

            flowLayoutPanelPosts.Controls.Clear();

            foreach (PostDesdeAPI post in posts)
            {
                
                MaterialCard card = new MaterialCard();
                card.Width = 697;
                card.Height = 225;
                card.Padding = new Padding(10);

                
                PictureBox pictureBox = new PictureBox();
                pictureBox.Location = new Point(40, 24);
                pictureBox.Size = new Size(54, 54);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Image = CapaVisual.Properties.Resources.Profile_Picture_by_iconSvg_co;
                card.Controls.Add(pictureBox);

                
                Label lblUsuario = new Label();
                lblUsuario.Text = obtenerCreadorDePost(post.id_cuenta);
                lblUsuario.Location = new Point(18, 88);
                lblUsuario.Font = new Font("Arial", 10, FontStyle.Bold);
                lblUsuario.AutoSize = true;
                card.Controls.Add(lblUsuario);

                
                TextBox txtContenido = new TextBox();
                txtContenido.Location = new Point(128, 24);
                txtContenido.Text = post.contenido;
                txtContenido.Multiline = true;
                txtContenido.Width = 552;
                txtContenido.Height = 86;
                txtContenido.ReadOnly = true;
                card.Controls.Add(txtContenido);

                
                int iconYPosition = txtContenido.Bottom + 10; 
                int iconXStart = txtContenido.Left; 

                PictureBox pbLike = new PictureBox();
                pbLike.Location = new Point(iconXStart, iconYPosition); 
                pbLike.Size = new Size(32, 32);
                pbLike.SizeMode = PictureBoxSizeMode.CenterImage;
                pbLike.Image = CapaVisual.Properties.Resources.Heart_Attack_by_iconSvg_co_4_;
                card.Controls.Add(pbLike);

                PictureBox pbComment = new PictureBox();
                pbComment.Location = new Point(iconXStart + 40, iconYPosition); 
                pbComment.Size = new Size(32, 32);
                pbComment.SizeMode = PictureBoxSizeMode.CenterImage;
                pbComment.Image = CapaVisual.Properties.Resources.Chat_by_iconSvg_co_1_;
                card.Controls.Add(pbComment);

                PictureBox pbShare = new PictureBox();
                pbShare.Location = new Point(iconXStart + 80, iconYPosition); 
                pbShare.Size = new Size(32, 32);
                pbShare.SizeMode = PictureBoxSizeMode.Normal;
                pbShare.Image = CapaVisual.Properties.Resources.Post_by_iconSvg_co_1_;
                card.Controls.Add(pbShare);

                
                flowLayoutPanelPosts.Controls.Add(card);
            }
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

        private static string obtenerCreadorDePost(int id_cuenta)
        {
            RestClient client = new RestClient("http://localhost:44331/");
            RestRequest request = new RestRequest($"ApiPost/post/obtener-creador/{id_cuenta}", Method.Get);
            request.AddHeader("Accept", "application/json");
            RestResponse response = client.Execute(request);

            string content = response.Content.Trim('"');
            return content;
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
            
        }

        private void asdasdToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnAdjuntar_Click(object sender, EventArgs e)
        {
            ofd.ShowDialog();
        }
    }

}
