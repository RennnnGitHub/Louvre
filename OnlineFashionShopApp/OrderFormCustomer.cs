using OnlineFashionShopApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using OnlineFashionShopApp.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OnlineFashionShopApp
{// This is the form foe Order Customer
    public partial class OrderFormCustomer : Form
    {
        private User _currentUser;
        public OrderFormCustomer(User currentUser)
        {

            InitializeComponent();
            _currentUser = currentUser;
            LoadOrderContentsAsync();
        }
        private async Task LoadOrderContentsAsync()
        {// Add column to table
            listView1.Items.Clear();
            listView1.Columns.Add("Shipping Address", 150);
            listView1.Columns.Add("Grand Total", 100);
            listView1.Columns.Add("Cart Number", 150);
            listView1.Columns.Add("Expiration Month", 80);
            listView1.Columns.Add("Expiration Year", 80);
            listView1.Columns.Add("CVV", 80);
            listView1.Columns.Add("Ordered Products", 500);

            try
            {
                //Fetch current user id
                int userID = _currentUser.Id;

                using (var client = new HttpClient())
                {


                    var response = await client.GetAsync($"https://localhost:7098/Order/GetOrderContentsByUserId/{userID}");
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        List<Orders> orderContents = JsonSerializer.Deserialize<List<Orders>>(content, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        foreach (var order in orderContents)
                        {
                            var item = new ListViewItem(order.ShippingAddress);
                            item.SubItems.Add(order.GrandTotal.ToString());
                            item.SubItems.Add(order.CartNumber);
                            item.SubItems.Add(order.ExpirationMonth.ToString());
                            item.SubItems.Add(order.ExpirationYear.ToString());
                            item.SubItems.Add(order.CVV); 

                            var orderedProductsSubItem = new ListViewItem.ListViewSubItem(item, "Products: ");

                            if (!string.IsNullOrWhiteSpace(order.OrderedProducts))
                            {
                                try
                                {
                                    // Parse the "orderedProducts" field as a nested JSON array
                                    List<Products> orderedProducts = JsonSerializer.Deserialize<List<Products>>(order.OrderedProducts);

                                    // Build a string representation of the ordered products
                                    string productsText = "Products: ";
                                    foreach (var product in orderedProducts)
                                    {
                                        productsText += $"{product.ProductName} (Price: {product.Price}, Quantity: {product.Quantity}) ";
                                    }

                                    orderedProductsSubItem.Text = productsText;
                                }
                                catch (JsonException)
                                {
                                    orderedProductsSubItem.Text = "Error parsing ordered products.";
                                }
                            }
                            else
                            {
                                orderedProductsSubItem.Text = "No ordered products.";
                            }

                            item.SubItems.Add(orderedProductsSubItem);
                            listView1.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve order contents. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HomeFormCustomer hm = new HomeFormCustomer(_currentUser);
            hm.Show();
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
            OrderFormCustomer omd = new OrderFormCustomer(_currentUser);
            omd.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CartForm cf = new CartForm(_currentUser);
            cf.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TrackingForm customer = new TrackingForm(_currentUser);
            customer.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
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
