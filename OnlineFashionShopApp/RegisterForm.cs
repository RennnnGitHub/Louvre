using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineFashionShopApp
{
    public partial class RegisterForm : Form
    {
        ComboBox cbxRole = new ComboBox();
        public RegisterForm()
        {
            InitializeComponent();
            cbxRole.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cbxRole.Location = new Point(lblRole.Location.X, textBox5.Location.Y);
            cbxRole.Name = "cbxRole";
            cbxRole.Size = new Size(177, 23);
            cbxRole.TabIndex = 8;
            cbxRole.Items.Add("Admin");
            cbxRole.Items.Add("Customer");
            panel2.Controls.Add(cbxRole);

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) //firstname
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e) //lastname
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e) //register button
        {
            string apiUrl = "https://localhost:7098/User/Register"; // Replace with the actual API URL.

            var payload = new
            {
                role = cbxRole.SelectedText,
                email = textBox4.Text,
                password = textBox3.Text,
                firstname = textBox1.Text,
                lastname = textBox2.Text,
                phonenumber = textBox5.Text
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
                            MessageBox.Show("You have registered Succesfully");
                            this.Close();
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

        private void textBox4_TextChanged(object sender, EventArgs e) //email address
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e) //password
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
        }

        private void label8_Click_1(object sender, EventArgs e)
        {

        }
    }
}
