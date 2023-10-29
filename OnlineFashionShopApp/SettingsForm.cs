using OnlineFashionShopApp.Models;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OnlineFashionShopApp
{
    public partial class SettingsForm : Form
    {
        private User _currentUser;
        public SettingsForm()
        {
            InitializeComponent();
            ObjectCache cache = MemoryCache.Default;
            _currentUser = (User)cache.Get("currentUser");
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button9_Click_1(object sender, EventArgs e)
        {
            string apiUrl = "https://localhost:7098/User/ChangeUserSettings"; // Replace with the actual API URL.

            var payload = new
            {
                userId = _currentUser.Id,
                newEmail = textBox4.Text,
                newPassword = textBox3.Text,
                newFirstname = textBox1.Text,
                newLastname = textBox2.Text,
                newPhonenumber = textBox6.Text
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
                        if (obj["status"].ToString() == "true")
                        {
                            MessageBox.Show("Settings Updated Succesfully");
                            User u = new User()
                            {
                                Id = _currentUser.Id,
                                Firstname = payload.newFirstname,
                                Lastname = payload.newLastname,
                                Email = payload.newEmail,
                                Userrole = _currentUser.Userrole,
                                Phonenumber = payload.newPhonenumber
                            };
                            ObjectCache cache = MemoryCache.Default;
                            CacheItemPolicy policy = new CacheItemPolicy
                            {
                                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60)
                            };
                            cache.Set("currentUser", u, policy);
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

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

            textBox1.Text = _currentUser.Firstname;
            textBox2.Text = _currentUser.Lastname;
            textBox4.Text = _currentUser.Email;
            textBox6.Text = _currentUser.Phonenumber;

            if (_currentUser.Userrole == 1)
            {
                btnAddress.Visible = true;
            }
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            //address
            AddressListForm form = new AddressListForm();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HomeFormCustomer Hm = new HomeFormCustomer(_currentUser);
            Hm.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ProductCustomerForm pm = new ProductCustomerForm(_currentUser);
            pm.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OrderFormCustomer of = new OrderFormCustomer(_currentUser);
            of.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TrackingForm tf = new TrackingForm(_currentUser);
            tf.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AccessLogForm tf = new AccessLogForm();
            tf.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
