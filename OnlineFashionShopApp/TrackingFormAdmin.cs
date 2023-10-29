using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OnlineFashionShopApp.Models;

namespace OnlineFashionShopApp
{
    //This is the admin tracking form
    public partial class TrackingFormAdmin : Form
    {
        private User _currentUser;
        public TrackingFormAdmin(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            // Check if textBox1 is empty
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please input your order ID (tracking number).");
                return;
            }

            // Get the order ID from textBox1
            if (!int.TryParse(textBox1.Text, out int orderId))
            {
                MessageBox.Show("Invalid order ID (tracking number).");
                return;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    // Replace with the actual API endpoint to change the order status to "Shipped"
                    string apiUrl = $"https://localhost:7098/Order/ChangeOrderStatus/{orderId}/Shipped";

                    // Send a POST request to change the order status
                    var response = await client.PostAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Order status changed to 'Shipped'.");
                        textBox2.Text = "Shipped";
                    }
                    else
                    {
                        // Handle the case where the POST request is not successful
                        MessageBox.Show("Failed to change the order status.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., network issues or unexpected errors
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private async void button10_Click(object sender, EventArgs e)
        {
            // Check if textBox1 is empty
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please input your order ID (tracking number).");
                return;
            }

            // Get the order ID from textBox1
            int orderId;
            if (!int.TryParse(textBox1.Text, out orderId))
            {
                MessageBox.Show("Invalid order ID (tracking number).");
                return;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    // Replace with the actual API endpoint to get the order status
                    string apiUrl = $"https://localhost:7098/Order/GetOrderStatus/{orderId}";

                    // Send a GET request
                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string status = await response.Content.ReadAsStringAsync();

                        // Update textBox2 with the retrieved status
                        textBox2.Text = status;
                    }
                    else
                    {
                        // Handle the case where the GET request is not successful
                        textBox2.Text = "Failed to retrieve order status";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., network issues or unexpected errors
                textBox2.Text = "Error: " + ex.Message;
            }
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            // Check if textBox1 is empty
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please input your order ID (tracking number).");
                return;
            }

            // Get the order ID from textBox1
            if (!int.TryParse(textBox1.Text, out int orderId))
            {
                MessageBox.Show("Invalid order ID (tracking number).");
                return;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    // Replace with the actual API endpoint to change the order status to "Delivered"
                    string apiUrl = $"https://localhost:7098/Order/ChangeOrderStatus/{orderId}/Delivered";

                    // Send a POST request to change the order status
                    var response = await client.PostAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Order status changed to 'Delivered'.");
                        textBox2.Text = "Delivered";
                    }
                    else
                    {
                        // Handle the case where the POST request is not successful
                        MessageBox.Show("Failed to change the order status.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., network issues or unexpected errors
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void button23_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please input your order ID (tracking number).");
                return;
            }

            // Get the order ID from textBox1
            if (!int.TryParse(textBox1.Text, out int orderId))
            {
                MessageBox.Show("Invalid order ID (tracking number).");
                return;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    // Replace with the actual API endpoint to change the order status to "In Progress"
                    string apiUrl = $"https://localhost:7098/Order/ChangeOrderStatus/{orderId}/In%20Progress";

                    // Send a POST request to change the order status
                    var response = await client.PostAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Order status changed to 'In Progress'.");
                        textBox2.Text = "In Progress";
                    }
                    else
                    {
                        // Handle the case where the POST request is not successful
                        MessageBox.Show("Failed to change the order status.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., network issues or unexpected errors
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HomeFormAdmin homeFormAdmin = new HomeFormAdmin(_currentUser);
            homeFormAdmin.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ProductAdminForm productAdminForm = new ProductAdminForm(_currentUser);
            productAdminForm.Show();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OrderFormAdmin orderFormAdmin = new OrderFormAdmin(_currentUser);
            orderFormAdmin.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TrackingFormAdmin trackingFormAdmin = new TrackingFormAdmin(_currentUser);
            trackingFormAdmin.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AccessLogForm accessLogForm = new AccessLogForm();
            accessLogForm.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Show();
            this.Close();
        }
    }
}
