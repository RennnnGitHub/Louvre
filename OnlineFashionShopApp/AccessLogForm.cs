using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OnlineFashionShopApp.Models;

// This is the page for Access Log
namespace OnlineFashionShopApp
{
    public partial class AccessLogForm : Form
    {
        private User _currentUser;
        ObjectCache _cache = MemoryCache.Default;
        public AccessLogForm()
        {
            InitializeComponent();
            _currentUser = (User)_cache.Get("currentUser");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private async void AccessLogForm_Load(object sender, EventArgs e)
        {
            string apiUrl = "https://localhost:7098/User/GetAccessLogsByUserId"; // Replace with the actual API URL.


            string jsonPayload = JsonSerializer.Serialize(new
            {
                id = _currentUser.Id
            });

            // Define the JSON payload as a string
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

                        JsonNode obj = JsonObject.Parse(result);
                        int i = 0;
                        foreach (var log in obj.AsArray())
                        {
                            ListViewItem item = new ListViewItem(log["user"]["firstname"].ToString() + " " + log["user"]["lastname"].ToString());
                            item.SubItems.Add(
                                log["action"].ToString()
                            );
                            item.SubItems.Add(
                                log["actionDate"].ToString()
                            );
                            lvwAccessLog.Items.Add(item);

                            i++;
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            HomeFormAdmin homeFormAdmin = new HomeFormAdmin(_currentUser);
            homeFormAdmin.Show();
            this.Close();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            ProductAdminForm pm = new ProductAdminForm(_currentUser);
            pm.Show();
            this.Close();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            OrderFormAdmin of = new OrderFormAdmin(_currentUser);
            of.Show();
            this.Close();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            TrackingFormAdmin tf = new TrackingFormAdmin(_currentUser);
            tf.Show();
            this.Close();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //settings
            SettingsForm form = new SettingsForm();

            form.ShowDialog();
            this.Close();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}
