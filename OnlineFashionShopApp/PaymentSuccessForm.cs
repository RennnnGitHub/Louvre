using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnlineFashionShopApp.Models;
namespace OnlineFashionShopApp
{// This is the page for Payment Success form
    public partial class PaymentSuccessForm : Form
    {
        private User _currentUser;
        public PaymentSuccessForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            loadPage();
        }
        private async void loadPage()
        {
            //fetch userID
            int userID = _currentUser.Id;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7098/Order/GetLastOrderID/{userID}");// String api url

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        int lastOrderID = int.Parse(json);

                        textBox1.Text = lastOrderID.ToString();
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show("An error occurred during deserialization: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to retrieve the latest order ID.");
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            HomeFormCustomer homeForm = new HomeFormCustomer(_currentUser);
            homeForm.Show();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ProductCustomerForm productCustomerForm = new ProductCustomerForm(_currentUser);
            productCustomerForm.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HomeFormCustomer homeForm = new HomeFormCustomer(_currentUser);
            homeForm.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ProductCustomerForm productCustomerForm = new ProductCustomerForm(_currentUser);    
            productCustomerForm.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OrderFormCustomer orderFormCustomer = new OrderFormCustomer(_currentUser);
            orderFormCustomer.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CartForm cartForm = new CartForm(_currentUser);
            cartForm.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TrackingForm trackingForm = new TrackingForm(_currentUser);
            trackingForm.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm();
            settings.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}
