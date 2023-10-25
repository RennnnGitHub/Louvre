using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Json;
using System.Net.Http.Json;
using System.Net.Http.Json;

using System.Net.Http.Json;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection.Metadata;

namespace OnlineFashionShopApp
{
    public partial class CartForm : Form
    {


        public CartForm()
        {
            InitializeComponent();

            // Create and configure a FlowLayoutPanel for displaying cart items

            Controls.Add(cartItemsPanel);

            LoadCart();

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
        private async Task<string> GetProductIDFromServer(int productKey)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Construct the URL to your server's API endpoint
                    var apiUrl = $"https://localhost:7098/Product/GetProductID/{productKey}";

                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the product ID from the response content
                        string productId = await response.Content.ReadAsStringAsync();
                        return productId;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve product ID. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Return null or an appropriate default value when there's an error
            return null;
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
        private async Task<byte[]> GetProductImageFromServer(int productKey)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7098/Product/GetProductImage/{productKey}");

                if (response.IsSuccessStatusCode)
                {
                    // Read the image data as a byte array
                    return await response.Content.ReadAsByteArrayAsync();
                }

                // Handle errors or return an empty byte array in case of failure
                return new byte[0];
            }
        }
        private async Task LoadCart()
        {
            try
            {
                using (var client = new HttpClient())
                {

                    var response = await client.GetAsync("https://localhost:7098/Cart/GetCartContents");

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            Dictionary<int, int> cartData = JsonSerializer.Deserialize<Dictionary<int, int>>(json);

                            // Create a list of CartItem objects based on the dictionary
                            List<CartItem> cartItems = new List<CartItem>();

                            foreach (var item in cartData)
                            {
                                byte[] imageBytes = await GetProductImageFromServer(item.Key);
                                decimal price = await GetProductPriceFromServer(item.Key);
                                string name = await GetProductName(item.Key);
                                string prodID = await GetProductIDFromServer(item.Key);
                                // Convert the image bytes to a base64-encoded string
                                string base64Image = Convert.ToBase64String(imageBytes);

                                // Create a CartItem
                                var cartItem = new CartItem
                                {
                                    ProductName = name,
                                    Quantity = item.Value,
                                    Price = price,
                                    ProductImageBase64 = base64Image, // Assign the base64-encoded image string
                                    productId = prodID,
                                };

                                cartItems.Add(cartItem);
                            }

                            cartItemsPanel.Controls.Clear(); // Clear existing items

                            // Create a scrollable panel for displaying cart items
                            var scrollablePanel = new Panel
                            {
                                Dock = DockStyle.Fill,
                                AutoScroll = true,
                            };

                            // Add the scrollable panel to the cartItemsPanel
                            cartItemsPanel.Controls.Add(scrollablePanel);

                            int currentTop = 0; // To keep track of the top position
                            decimal totalPrice = 0;
                            // Create and configure controls for displaying cart items
                            foreach (var cartItem in cartItems)
                            {

                                // Create a container panel for each cart item
                                var itemPanel = new Panel
                                {
                                    Width = scrollablePanel.Width - SystemInformation.VerticalScrollBarWidth,
                                    Height = 120,
                                    BorderStyle = BorderStyle.FixedSingle,
                                    Location = new Point(0, currentTop), // Position each item below the previous one
                                };

                                // Increase the top position for the next item
                                currentTop += itemPanel.Height + 10;

                                var pictureBox = new PictureBox
                                {
                                    Location = new Point(10, 10),
                                    Size = new Size(100, 100),

                                    Image = Base64StringToImage(cartItem.ProductImageBase64),
                                };
                                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; // To display the image in its original size.

                                var nameLabel = new Label
                                {
                                    Location = new Point(120, 10),
                                    Width = itemPanel.Width - 120,
                                    Text = "Product Name: " + cartItem.ProductName,
                                    AutoEllipsis = true,
                                };

                                var quantityLabel = new Label
                                {
                                    Location = new Point(120, 40),
                                    Text = "Quantity: " + cartItem.Quantity,
                                };

                                var priceLabel = new Label
                                {
                                    Location = new Point(120, 70),
                                    Text = "Price: " + cartItem.Price.ToString("C"),
                                };
                                var incrementButton = new Button
                                {
                                    Location = new Point(300, 40),
                                    Text = "+",
                                    Width = 40,
                                };
                                incrementButton.Click += async (sender, e) =>
                                {
                                    // Handle increment button click here

                                    // Capture the 'productId' variable
                                    string productId = cartItem.productId;

                                    // Send an update request to the server to increment the quantity for the product with the specified product ID
                                    var apiUrl = $"https://localhost:7098/Cart/IncrementProductQuantity/{productId}";

                                    // Send the update request to the server here using the apiUrl
                                    using (var client = new HttpClient())
                                    {
                                        var response = await client.PostAsync(apiUrl, null);

                                        if (response.IsSuccessStatusCode)
                                        {
                                            // Update the quantity on the client side
                                            cartItem.Quantity++;
                                            quantityLabel.Text = "Quantity: " + cartItem.Quantity;
                                            MessageBox.Show("Item added successfully.");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Failed to add item to the cart.");
                                        }

                                        totalPrice += cartItem.Price;
                                        textBox1.Text = totalPrice.ToString("C");
                                    }
                                };
                                var quantitysLabel = new Label
                                {
                                    Location = new Point(340, 40),
                                    Text = "Quantity",
                                    Width = 80,

                                };
                                var decrementButton = new Button
                                {
                                    Location = new Point(420, 40),
                                    Text = "-",
                                    Width = 40,
                                };
                                decrementButton.Click += async (sender, e) =>
                                {
                                    // Handle decrement button click here

                                    // Capture the 'productId' variable
                                    string productId = cartItem.productId;

                                    // Send an update request to the server to decrement the quantity for the product with the specified product ID
                                    var apiUrl = $"https://localhost:7098/Cart/DecrementProductQuantity/{productId}";

                                    // Send the update request to the server here using the apiUrl
                                    using (var client = new HttpClient())
                                    {
                                        var response = await client.PostAsync(apiUrl, null);

                                        if (response.IsSuccessStatusCode)
                                        {
                                            // Update the quantity on the client side
                                            cartItem.Quantity--;
                                            quantityLabel.Text = "Quantity: " + cartItem.Quantity;

                                            // Check if the quantity is zero and remove the item from the UI
                                            if (cartItem.Quantity == 0)
                                            {
                                                // Assuming itemPanel is the parent of your controls, you can remove it from the parent
                                                itemPanel.Parent.Controls.Remove(itemPanel);
                                            }

                                            MessageBox.Show("Item removed successfully.");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Failed to remove item from the cart.");
                                        }
                                        totalPrice -= cartItem.Price;
                                        textBox1.Text = totalPrice.ToString("C");
                                    }
                                };


                                // Add controls to the item panel
                                itemPanel.Controls.Add(pictureBox);
                                itemPanel.Controls.Add(nameLabel);
                                itemPanel.Controls.Add(quantityLabel);
                                itemPanel.Controls.Add(priceLabel);
                                itemPanel.Controls.Add(incrementButton);
                                itemPanel.Controls.Add(quantitysLabel);
                                itemPanel.Controls.Add(decrementButton);
                                // Add the item panel to the scrollable panel
                                scrollablePanel.Controls.Add(itemPanel);
                            }


                            // Loop through your cart items and calculate the total price
                            foreach (var cartItem in cartItems)
                            {
                                totalPrice += cartItem.Price * cartItem.Quantity;
                            }
                            // Display the total price in the textBox1
                            textBox1.Text = totalPrice.ToString("C"); // Format as currency
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


        // ... (Rest of your event handlers)
        private Image Base64StringToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new System.IO.MemoryStream(imageBytes))
            {
                return Image.FromStream(ms);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Handle text changed event
        }

        private void button11_Click(object sender, EventArgs e)
        {
            PaymentForm form = new PaymentForm();
            form.ShowDialog();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ProductCustomerForm form = new ProductCustomerForm();
            form.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HomeFormCustomer om = new HomeFormCustomer();
            om.ShowDialog();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OrderFormCustomer oom = new OrderFormCustomer();
            oom.ShowDialog();
            this.Close();
        }
    }
}
public class CartItem
{
    public string productId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ProductImageBase64 { get; set; }
}
public class Product
{
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }

    public string ProductImageBase64 { get; set; }
    // Add other properties as needed
}