using OnlineFashionShopApp.Models;
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
{// This is the form for Admin Order
    public partial class OrderFormAdmin : Form
    {
        private User _currentUser;

        public OrderFormAdmin(User currentUser)
        {
            InitializeComponent();
            LoadOrderContentsAsync();
            _currentUser = currentUser;
        }
        private async Task LoadOrderContentsAsync()
        {// Add column table
            listView1.Items.Clear();
            listView1.Columns.Add("Tracking ID", 80);
            listView1.Columns.Add("Shipping Address", 150);
            listView1.Columns.Add("Grand Total", 100);
            listView1.Columns.Add("Cart Number", 150);
            listView1.Columns.Add("Expiration Month", 80);
            listView1.Columns.Add("Expiration Year", 80);
            listView1.Columns.Add("CVV", 80);
            listView1.Columns.Add("Ordered Products", 600);

            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("https://localhost:7098/Order/GetAllOrderContent");
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        List<Orders> orderContents = JsonSerializer.Deserialize<List<Orders>>(content, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        int orderid = 1;
                        foreach (var order in orderContents)
                        {
                            // Initialize order ID
                            var item = new ListViewItem(orderid.ToString());
                            item.SubItems.Add(order.ShippingAddress);
                            item.SubItems.Add(order.GrandTotal.ToString());
                            item.SubItems.Add(order.CartNumber);
                            item.SubItems.Add(order.ExpirationMonth.ToString());
                            item.SubItems.Add(order.ExpirationYear.ToString());
                            item.SubItems.Add(order.CVV); 

                            var orderedProductsSubItem = new ListViewItem.ListViewSubItem(item, "Products: ");
                            orderid++;
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
        private void OrderFormAdmin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ProductAdminForm form = new ProductAdminForm(_currentUser);
            form.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HomeFormAdmin homeFormAdmin = new HomeFormAdmin(_currentUser);
            homeFormAdmin.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OrderFormAdmin orderFormAdmin = new OrderFormAdmin(_currentUser);
            orderFormAdmin.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TrackingFormAdmin track = new TrackingFormAdmin(_currentUser);
            track.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
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
