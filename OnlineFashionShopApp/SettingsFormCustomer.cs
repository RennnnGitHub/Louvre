using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineFashionShopApp
{
    public partial class SettingsFormCustomer : Form
    {
        public SettingsFormCustomer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private async void button9_Click(object sender, EventArgs e)
        {
            string apiUrl = "https://localhost:7098/User/ChangeUserSettings"; // Replace with the actual API URL.

            // Define the JSON payload as a string
            string jsonPayload = "{\"userId\":2, \"newFirstname\": \"" + textBox1.Text + "\", \"newLastname\": \"" + textBox2.Text + "\", \"newEmail\": \"" + textBox4.Text + "\", \"newPassword\": \"" + textBox3.Text + "\"}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create a StringContent with the JSON payload and specify the content type
                    StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Make a POST request with the JSON payload
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        // You can process the API response (result) as needed.
                        MessageBox.Show("Settings Saved Sucessfully");
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e) //firstname
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e) //lastname
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e) //email address
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e) //password
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void SettingsFormCustomer_Load(object sender, EventArgs e)
        {

        }
    }
}
