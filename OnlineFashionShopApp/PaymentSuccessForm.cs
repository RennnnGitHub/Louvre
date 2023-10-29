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
{
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
            int userID = _currentUser.Id;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7098/Order/GetLastOrderID/{userID}");

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

        }

        private void button9_Click(object sender, EventArgs e)
        {

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

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
