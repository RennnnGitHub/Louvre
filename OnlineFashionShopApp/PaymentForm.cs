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
using Microsoft.VisualBasic.ApplicationServices;
using OnlineFashionShopApp.Models;
namespace OnlineFashionShopApp
{
    public partial class PaymentForm : Form
    {
        private List<PaymentProduct> paymentProducts = new List<PaymentProduct>();
        private OnlineFashionShopApp.Models.User _currentUser;
        public PaymentForm(OnlineFashionShopApp.Models.User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            LoadPaymentProducts();
        }
        private async Task LoadPaymentProducts()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    int userID = _currentUser.Id;
                    var response = await client.GetAsync($"https://localhost:7098/Cart/GetCartContents/{userID}");

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            Dictionary<int, int> cartData = JsonSerializer.Deserialize<Dictionary<int, int>>(json);

                            // Create a list of PaymentProduct objects based on the dictionary
                            List<PaymentProduct> paymentProducts = new List<PaymentProduct>();

                            foreach (var item in cartData)
                            {
                                decimal price = await GetProductPriceFromServer(item.Key);
                                string name = await GetProductName(item.Key);

                                // Create a PaymentProduct
                                var paymentProduct = new PaymentProduct
                                {
                                    ProductName = name,
                                    Quantity = item.Value,
                                    Price = price,
                                };

                                paymentProducts.Add(paymentProduct);
                            }

                            // Clear existing items in listView1
                            listView1.Items.Clear();

                            foreach (var paymentProduct in paymentProducts)
                            {
                                // Create a new ListViewItem with the product name
                                var item = new ListViewItem(paymentProduct.ProductName);
                                item.SubItems.Add(paymentProduct.Quantity.ToString());
                                // Add sub-items for price, quantity, and subtotal
                                item.SubItems.Add(paymentProduct.Price.ToString("C"));

                                item.SubItems.Add((paymentProduct.Price * paymentProduct.Quantity).ToString("C"));

                                // Add the item to listView1
                                listView1.Items.Add(item);
                            }
                            decimal totalPrice = 0;
                            foreach (var paymentProduct in paymentProducts)
                            {
                                totalPrice += paymentProduct.Price * paymentProduct.Quantity;
                            }
                            // Display the total price in the textBox1
                            textBox2.Text = totalPrice.ToString("C"); // Format as currency
                        }
                        catch (JsonException ex)
                        {
                            MessageBox.Show("An error occurred during deserialization: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve cart.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        private async Task<decimal> GetProductPriceFromServer(int productKey)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7098/Product/GetProductPrice/{productKey}");

                if (response.IsSuccessStatusCode)
                {

                    var priceJson = await response.Content.ReadAsStringAsync();

                    if (decimal.TryParse(priceJson, out decimal productPrice))
                    {
                        return productPrice;
                    }
                    else
                    {
                        Console.WriteLine("Failed to parse product price as a decimal.");
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve product price. Status code: {response.StatusCode}");
                }

                return 0; // Return a default value or handle errors.
            }
        }

        private async Task<string> GetProductName(int productId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7098/Product/GetProductName/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve product name. Status code: {response.StatusCode}");
                    return string.Empty; // Return an empty string or handle errors.
                }
            }
        }


        private void label9_Click(object sender, EventArgs e)
        {

        }



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private async void button9_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                int userID = _currentUser.Id;
                var response = await client.GetAsync($"https://localhost:7098/Cart/GetCartContents/{userID}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Dictionary<int, int> cartData = JsonSerializer.Deserialize<Dictionary<int, int>>(json);

                        // Create a list of PaymentProduct objects based on the dictionary
                        paymentProducts.Clear(); // Clear any existing items in the list
                        foreach (var item in cartData)
                        {
                            decimal price = await GetProductPriceFromServer(item.Key);
                            string name = await GetProductName(item.Key);

                            // Create a PaymentProduct
                            var paymentProduct = new PaymentProduct
                            {
                                ProductName = name,
                                Quantity = item.Value,
                                Price = price,
                            };

                            paymentProducts.Add(paymentProduct);
                        }
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show("An error occurred during deserialization: " + ex.Message);
                    }
                }
            }
            // Ensure that required fields are filled
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
    string.IsNullOrWhiteSpace(textBox2.Text) ||
    string.IsNullOrWhiteSpace(textBox3.Text) ||
    string.IsNullOrWhiteSpace(textBox4.Text) ||
    string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please fill in all required information.");
                return;
            }

            // Remove the currency symbol ($) from the grand total value
            string grandTotalText = textBox2.Text.Replace("$", "");
            if (decimal.TryParse(grandTotalText, out decimal grandTotal))
            {
                // Now, grandTotal is the correct numeric value without the currency symbol

                // Create an object to represent the order on the client-side
                var orderData = new Order
                {
                    ShippingAddress = textBox1.Text,
                    GrandTotal = grandTotal, // Use the corrected numeric value
                    CartNumber = textBox3.Text,
                    ExpirationMonth = int.Parse(textBox4.Text),
                    ExpirationYear = int.Parse(textBox5.Text),
                    CVV = textBox6.Text,
                    OrderedProducts = paymentProducts // Assuming paymentProducts is a list of ordered products
                };
                MessageBox.Show($"ShippingAddress: {orderData.ShippingAddress}");
                MessageBox.Show($"GrandTotal: {orderData.GrandTotal}");
                MessageBox.Show($"CartNumber: {orderData.CartNumber}");
                MessageBox.Show($"ExpirationMonth: {orderData.ExpirationMonth}");
                MessageBox.Show($"ExpirationYear: {orderData.ExpirationYear}");
                MessageBox.Show($"CVV: {orderData.CVV}");

                // Serialize the order object to JSON
                string jsonPayload = JsonSerializer.Serialize(orderData);
                MessageBox.Show(jsonPayload);
                // Define the API URL
                int userID = _currentUser.Id;
                string apiUrl = $"https://localhost:7098/Order/AddOrder/{userID}"; // Replace with your actual API URL.

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Create a StringContent with the JSON payload and specify the content type
                        StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                        // Make a POST request with the JSON payload to add the order
                        HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            // You can process the API response (result) as needed.

                            // Clear the cart contents by calling an API endpoint


                            if (await ClearCartContentsOnServer(userID))
                            {
                                MessageBox.Show("Order placed successfully, and the cart has been cleared.");
                            }
                            else
                            {
                                MessageBox.Show("Order placed successfully, but failed to clear the cart.");
                            }

                            this.Close(); // Close the payment form
                        }
                        else
                        {
                            // Handle the response if it's not successful (e.g., display an error message).
                            MessageBox.Show($"Failed to place the order. Status code: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions (e.g., network issues).
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid grand total value. Please enter a valid numeric value.");
            }

            PaymentSuccessForm formCust = new PaymentSuccessForm(_currentUser);
            formCust.ShowDialog();
            this.Close();
        }
        private async Task<bool> ClearCartContentsOnServer(int userId)
        {
            using (var client = new HttpClient())
            {
                string url = $"https://localhost:7098/Cart/ClearCartContents/{userId}";
                var response = await client.PostAsync(url, null); // Use null for the request content if not needed.

                return response.IsSuccessStatusCode;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HomeFormCustomer Hm = new HomeFormCustomer();
            Hm.Show();
        }
    }
}
public class PaymentProduct
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

