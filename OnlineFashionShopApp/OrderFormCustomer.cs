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

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OnlineFashionShopApp
{
    public partial class OrderFormCustomer : Form
    {
        public OrderFormCustomer()
        {
            InitializeComponent();
            LoadOrderContentsAsync();
        }
        private async Task LoadOrderContentsAsync()
        {
            listView1.Items.Clear();
            listView1.Columns.Add("Shipping Address",150);
            listView1.Columns.Add("Grand Total",100);
            listView1.Columns.Add("Cart Number",150);
            listView1.Columns.Add("Expiration Month",80);
            listView1.Columns.Add("Expiration Year",80);
            listView1.Columns.Add("CVV", 80);
            listView1.Columns.Add("Ordered Products",500);

            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("https://localhost:7098/Order/GetOrderContentsByUserId");
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
                            item.SubItems.Add(order.CVV); // Display the CVV

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
    }
}
