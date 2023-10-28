﻿using System;
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
    public partial class TrackingForm : Form
    {
        public TrackingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TrackingForm_Load(object sender, EventArgs e)
        {

        }

        private async void button9_Click(object sender, EventArgs e)
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
    }
}
