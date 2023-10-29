using System;
using System.Drawing.Drawing2D;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using OnlineFashionShopApp.Models;
using System.Runtime.Caching;
using System.Net;

namespace OnlineFashionShopApp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;

            // Attach the Paint event handler to the form.
            this.Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int borderRadius = 20; // Adjust this value to control the roundness of the bottom corners.
            Rectangle bounds = new Rectangle(0, 0, this.Width, this.Height);
            GraphicsPath path = new GraphicsPath();

            // Add squared rectangle to the GraphicsPath.
            path.AddLine(bounds.X, bounds.Y, bounds.Right, bounds.Y);
            path.AddLine(bounds.Right, bounds.Y, bounds.Right, bounds.Bottom - borderRadius);

            // Add rounded corners to the GraphicsPath (bottom left and bottom right).
            path.AddArc(bounds.Right - borderRadius * 2, bounds.Bottom - borderRadius * 2, borderRadius * 2, borderRadius * 2, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - borderRadius * 2, borderRadius * 2, borderRadius * 2, 90, 90);

            // Close the path.
            path.CloseFigure();

            // Set the region of the form to the custom shape.
            this.Region = new Region(path);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox1.BorderStyle = BorderStyle.None;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // register 
            this.Hide();
            RegisterForm form = new RegisterForm();
            form.ShowDialog();
            this.Show();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e) //email
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e) //password
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string apiUrl = "https://localhost:7098/User/Login"; // Replace with the actual API URL.

            var payload = new
            {
                email = textBox1.Text,
                password = textBox3.Text
            };

            string jsonPayload = JsonSerializer.Serialize(payload);


            using (HttpClient client = new HttpClient())
            {
                try
                {
                    StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
        
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        // You can process the API response (result) as needed.
                        JsonNode obj = JsonObject.Parse(result);
                        if (obj["success"].ToString() == "true")
                        {
                            //MessageBox.Show(obj["success"].ToString());
                            MessageBox.Show("Welcome " + obj["data"]["firstname"].ToString());
                            this.Hide();
                            User u = new User()
                            {
                                Id = (int)obj["data"]["id"],
                                Firstname = obj["data"]["firstname"].ToString(),
                                Lastname = obj["data"]["lastname"].ToString(),
                                Email = obj["data"]["email"].ToString(),
                                Userrole = (int)obj["data"]["userrole"],
                                Phonenumber = obj["data"]["phonenumber"].ToString(),
                                ShipmentId = (int)obj["data"]["shipmentId"]
                            };

                            ObjectCache cache = MemoryCache.Default;
                            CacheItemPolicy policy = new CacheItemPolicy
                            {
                                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60)
                            };
                            cache.Add("currentUser", u, policy);
                            if(u.Userrole == 1)
                            {
                                HomeFormCustomer formHomeCustomer = new HomeFormCustomer(u);
                                formHomeCustomer.ShowDialog();
                                
                            }
                            else 
                            {
                                HomeFormAdmin formHomeAdmin = new HomeFormAdmin(u);
                                formHomeAdmin.ShowDialog();
                                
                            }
                            this.Show();
                        }
                        else {

                            MessageBox.Show(obj["message"].ToString());
                        }
                    }
                    else
                    {
                        // Handle the response if it's not successful (e.g., display an error message).
                        MessageBox.Show($"Failed to post data. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., network issues).
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }


    }
}