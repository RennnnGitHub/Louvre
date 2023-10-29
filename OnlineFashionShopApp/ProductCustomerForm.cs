using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Net.Http;
using OnlineFashionShopApp.Models;

namespace OnlineFashionShopApp
{
    public partial class ProductCustomerForm : Form
    {
        string apiUrl = "https://localhost:7098/Product/GetProducts";
        private List<ProductDTO> products = new List<ProductDTO>();
        private User _currentUser;

        public ProductCustomerForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            LoadProducts();
            comboBox1.Items.Add("Apparel");
            comboBox1.Items.Add("Footwear");
            comboBox1.Items.Add("Accessories");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private async void LoadProducts()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var products = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        panel4.Controls.Clear();
                        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
                        tableLayoutPanel.Dock = DockStyle.Fill;
                        tableLayoutPanel.AutoScroll = true;
                        // Set the column count for the TableLayoutPanel (2 columns for two products per row)
                        tableLayoutPanel.ColumnCount = 2;

                        // Set the spacing between rows and columns
                        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                        tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                        // Create UI elements for each product.
                        for (int i = 0; i < products.Count; i++)
                        {

                            if (i >= 10) // Limit the number of products.
                                break;
                            System.Drawing.Image image = Base64StringToImage(products[i].ImageBase64);
                            // Create a new panel for each product.
                            Panel productPanel = new Panel();
                            productPanel.BackColor = SystemColors.ActiveCaption;
                            productPanel.Margin = new Padding(3, 4, 45, 50);
                            // Calculate the position based on the product index
                            productPanel.BackgroundImage = image;
                            productPanel.BackgroundImageLayout = ImageLayout.Stretch;


                            productPanel.Size = new Size(290, 357);
                            productPanel.TabIndex = 9;

                            // Create and customize other controls (e.g., PictureBox, TextBox).


                            Label nameLabel = new Label();
                            nameLabel.Text = "Name";
                            nameLabel.Location = new Point(15, 170); // Adjust the location as needed.
                            nameLabel.BackColor = Color.Transparent;
                            nameLabel.Size = new Size(65, 27);
                            productPanel.Controls.Add(nameLabel);

                            TextBox nameBox = new TextBox();
                            nameBox.Location = new Point(78, 168); // Adjust the location as needed.
                            nameBox.Size = new Size(171, 27);
                            nameBox.Name = "nameBox" + i;
                            nameBox.Text = products[i].ProductName; // Set the product name.

                            Label priceLabel = new Label();
                            priceLabel.Text = "Price";
                            priceLabel.Location = new Point(15, 210); // Adjust the location as needed.
                            priceLabel.BackColor = Color.Transparent;
                            priceLabel.Size = new Size(65, 27);
                            productPanel.Controls.Add(priceLabel);

                            TextBox priceBox = new TextBox();
                            priceBox.Location = new Point(78, 207); // Adjust the location as needed.
                            priceBox.Size = new Size(109, 27);
                            priceBox.Name = "priceBox" + i;
                            // Set the product price as needed.
                            priceBox.Text = products[i].ProductPrice.ToString();

                            Label stockLabel = new Label();
                            stockLabel.Text = "Stock";
                            stockLabel.Location = new Point(15, 248); // Adjust the location as needed.
                            stockLabel.BackColor = Color.Transparent;
                            stockLabel.Size = new Size(65, 27);
                            productPanel.Controls.Add(stockLabel);
                            TextBox stockBox = new TextBox();
                            stockBox.Location = new Point(78, 244); // Adjust the location as needed.
                            stockBox.Size = new Size(171, 27);
                            stockBox.Name = "stockBox" + i;
                            // Set the product stock as needed.
                            stockBox.Text = products[i].ProductStock.ToString();

                            Button addToCart = new Button();
                            addToCart.BackColor = Color.YellowGreen;
                            addToCart.FlatStyle = FlatStyle.Flat;
                            addToCart.Location = new Point(78, 283);
                            addToCart.Margin = new Padding(3, 4, 3, 4);
                            addToCart.Name = "addToCart" + i; // Use a unique name for each remove button.
                            addToCart.Size = new Size(136, 60);
                            addToCart.TabIndex = 7;
                            addToCart.Text = "Add To Cart";
                            addToCart.UseVisualStyleBackColor = false;
                            addToCart.Click += addToCart_Click;
                            addToCart.Click += async (sender, e) =>

                            // Add controls to the product panel.

                            productPanel.Controls.Add(nameBox);
                            productPanel.Controls.Add(nameBox);
                            productPanel.Controls.Add(priceBox);
                            productPanel.Controls.Add(stockBox);
                            productPanel.Controls.Add(addToCart);

                            // Add the product panel to the parent panel.
                            tableLayoutPanel.Controls.Add(productPanel);
                        }

                        // Add the parent panel to the form or container of your choice.
                        panel4.Controls.Add(tableLayoutPanel);
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve products.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private Image Base64StringToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new System.IO.MemoryStream(imageBytes))
            {
                return Image.FromStream(ms);
            }
        }
        private void searchProductByName_Click(object sender, EventArgs e)
        {
            // Extract the name to search for from the TextBox (assuming you have a TextBox named searchTextBox)
            string productNameToSearch = textBox40.Text;

            // Call the searching method with the provided name
            SearchProductsByName(productNameToSearch);
        }

        private async void SearchProductsByName(string productName)
        {
            string apiUrl;  // Define apiUrl before the if block

            if (textBox40.Text == "")
            {

                apiUrl = "https://localhost:7098/Product/GetProducts";
            }
            else
            {
                apiUrl = $"https://localhost:7098/Product/GetProductsByName?name={productName}";
            }
            try
            {
                using (var client = new HttpClient())
                {
                    // Construct the API URL for filtering by category


                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var products = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        panel4.Controls.Clear();

                        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
                        tableLayoutPanel.Dock = DockStyle.Fill;
                        tableLayoutPanel.AutoScroll = true;
                        // Set the column count for the TableLayoutPanel (2 columns for two products per row)
                        tableLayoutPanel.ColumnCount = 2;

                        // Set the spacing between rows and columns
                        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                        tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                        // Create UI elements for each product.
                        for (int i = 0; i < products.Count; i++)
                        {

                            if (i >= 10) // Limit the number of products.
                                break;
                            System.Drawing.Image image = Base64StringToImage(products[i].ImageBase64);
                            // Create a new panel for each product.
                            Panel productPanel = new Panel();
                            productPanel.BackColor = SystemColors.ActiveCaption;
                            productPanel.Margin = new Padding(3, 4, 45, 50);
                            // Calculate the position based on the product index
                            productPanel.BackgroundImage = image;
                            productPanel.BackgroundImageLayout = ImageLayout.Stretch;


                            productPanel.Size = new Size(290, 357);
                            productPanel.TabIndex = 9;

                            // Create and customize other controls (e.g., PictureBox, TextBox).


                            Label nameLabel = new Label();
                            nameLabel.Text = "Name";
                            nameLabel.Location = new Point(15, 170); // Adjust the location as needed.
                            nameLabel.BackColor = Color.Transparent;
                            nameLabel.Size = new Size(65, 27);
                            productPanel.Controls.Add(nameLabel);

                            TextBox nameBox = new TextBox();
                            nameBox.Location = new Point(78, 168); // Adjust the location as needed.
                            nameBox.Size = new Size(171, 27);
                            nameBox.Name = "nameBox" + i;
                            nameBox.Text = products[i].ProductName; // Set the product name.

                            Label priceLabel = new Label();
                            priceLabel.Text = "Price";
                            priceLabel.Location = new Point(15, 210); // Adjust the location as needed.
                            priceLabel.BackColor = Color.Transparent;
                            priceLabel.Size = new Size(65, 27);
                            productPanel.Controls.Add(priceLabel);

                            TextBox priceBox = new TextBox();
                            priceBox.Location = new Point(78, 207); // Adjust the location as needed.
                            priceBox.Size = new Size(109, 27);
                            priceBox.Name = "priceBox" + i;
                            // Set the product price as needed.
                            priceBox.Text = products[i].ProductPrice.ToString();

                            Label stockLabel = new Label();
                            stockLabel.Text = "Stock";
                            stockLabel.Location = new Point(15, 248); // Adjust the location as needed.
                            stockLabel.BackColor = Color.Transparent;
                            stockLabel.Size = new Size(65, 27);
                            productPanel.Controls.Add(stockLabel);
                            TextBox stockBox = new TextBox();
                            stockBox.Location = new Point(78, 244); // Adjust the location as needed.
                            stockBox.Size = new Size(171, 27);
                            stockBox.Name = "stockBox" + i;
                            // Set the product stock as needed.
                            stockBox.Text = products[i].ProductStock.ToString();

                            Button addToCart = new Button();
                            addToCart.BackColor = Color.YellowGreen;
                            addToCart.FlatStyle = FlatStyle.Flat;
                            addToCart.Location = new Point(78, 283);
                            addToCart.Margin = new Padding(3, 4, 3, 4);
                            addToCart.Name = "addToCart" + i; // Use a unique name for each remove button.
                            addToCart.Size = new Size(136, 60);
                            addToCart.TabIndex = 7;
                            addToCart.Text = "Add To Cart";
                            addToCart.UseVisualStyleBackColor = false;
                            addToCart.Click += addToCart_Click;
                            addToCart.Click += async (sender, e) =>

                            // Add controls to the product panel.

                            productPanel.Controls.Add(nameBox);
                            productPanel.Controls.Add(nameBox);
                            productPanel.Controls.Add(priceBox);
                            productPanel.Controls.Add(stockBox);
                            productPanel.Controls.Add(addToCart);

                            // Add the product panel to the parent panel.
                            tableLayoutPanel.Controls.Add(productPanel);
                        }

                        // Add the parent panel to the form or container of your choice.
                        panel4.Controls.Add(tableLayoutPanel);
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve products.");
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void filterButton_CLick(object sender, EventArgs e)
        {
            // Extract the selected category from the ComboBox
            string selectedCategory = comboBox1.SelectedItem?.ToString();

            // Call the filtering method with the selected category
            await FilterProductsByCategoryAsync(selectedCategory);
        }
        private async Task FilterProductsByCategoryAsync(string category)
        {
            string apiUrl;  // Define apiUrl before the if block

            if (category == null)
            {

                apiUrl = "https://localhost:7098/Product/GetProducts";
            }
            else
            {
                apiUrl = $"https://localhost:7098/Product/GetProductsFilter?category={category}";
            }
            try
            {
                using (var client = new HttpClient())
                {
                    // Construct the API URL for filtering by category


                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var products = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        panel4.Controls.Clear();

                        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
                        tableLayoutPanel.Dock = DockStyle.Fill;
                        tableLayoutPanel.AutoScroll = true;
                        // Set the column count for the TableLayoutPanel (2 columns for two products per row)
                        tableLayoutPanel.ColumnCount = 2;

                        // Set the spacing between rows and columns
                        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                        tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                        // Create UI elements for each product.
                        for (int i = 0; i < products.Count; i++)
                        {

                            if (i >= 10) // Limit the number of products.
                                break;
                            System.Drawing.Image image = Base64StringToImage(products[i].ImageBase64);
                            // Create a new panel for each product.
                            Panel productPanel = new Panel();
                            productPanel.BackColor = SystemColors.ActiveCaption;
                            productPanel.Margin = new Padding(3, 4, 45, 50);
                            // Calculate the position based on the product index
                            productPanel.BackgroundImage = image;
                            productPanel.BackgroundImageLayout = ImageLayout.Stretch;


                            productPanel.Size = new Size(290, 357);
                            productPanel.TabIndex = 9;

                            // Create and customize other controls (e.g., PictureBox, TextBox).


                            Label nameLabel = new Label();
                            nameLabel.Text = "Name";
                            nameLabel.Location = new Point(15, 170); // Adjust the location as needed.
                            nameLabel.BackColor = Color.Transparent;
                            nameLabel.Size = new Size(65, 27);
                            productPanel.Controls.Add(nameLabel);

                            TextBox nameBox = new TextBox();
                            nameBox.Location = new Point(78, 168); // Adjust the location as needed.
                            nameBox.Size = new Size(171, 27);
                            nameBox.Name = "nameBox" + i;
                            nameBox.Text = products[i].ProductName; // Set the product name.

                            Label priceLabel = new Label();
                            priceLabel.Text = "Price";
                            priceLabel.Location = new Point(15, 210); // Adjust the location as needed.
                            priceLabel.BackColor = Color.Transparent;
                            priceLabel.Size = new Size(65, 27);
                            productPanel.Controls.Add(priceLabel);

                            TextBox priceBox = new TextBox();
                            priceBox.Location = new Point(78, 207); // Adjust the location as needed.
                            priceBox.Size = new Size(109, 27);
                            priceBox.Name = "priceBox" + i;
                            // Set the product price as needed.
                            priceBox.Text = products[i].ProductPrice.ToString();

                            Label stockLabel = new Label();
                            stockLabel.Text = "Stock";
                            stockLabel.Location = new Point(15, 248); // Adjust the location as needed.
                            stockLabel.BackColor = Color.Transparent;
                            stockLabel.Size = new Size(65, 27);
                            productPanel.Controls.Add(stockLabel);
                            TextBox stockBox = new TextBox();
                            stockBox.Location = new Point(78, 244); // Adjust the location as needed.
                            stockBox.Size = new Size(171, 27);
                            stockBox.Name = "stockBox" + i;
                            // Set the product stock as needed.
                            stockBox.Text = products[i].ProductStock.ToString();

                            Button addToCart = new Button();
                            addToCart.BackColor = Color.YellowGreen;
                            addToCart.FlatStyle = FlatStyle.Flat;
                            addToCart.Location = new Point(78, 283);
                            addToCart.Margin = new Padding(3, 4, 3, 4);
                            addToCart.Name = "addToCart" + i; // Use a unique name for each remove button.
                            addToCart.Size = new Size(136, 60);
                            addToCart.TabIndex = 7;
                            addToCart.Text = "Add To Cart";
                            addToCart.UseVisualStyleBackColor = false;
                            addToCart.Click += addToCart_Click;
                            addToCart.Click += async (sender, e) =>

                            // Add controls to the product panel.

                            productPanel.Controls.Add(nameBox);
                            productPanel.Controls.Add(nameBox);
                            productPanel.Controls.Add(priceBox);
                            productPanel.Controls.Add(stockBox);
                            productPanel.Controls.Add(addToCart);

                            // Add the product panel to the parent panel.
                            tableLayoutPanel.Controls.Add(productPanel);
                        }

                        // Add the parent panel to the form or container of your choice.
                        panel4.Controls.Add(tableLayoutPanel);
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve products.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private async void addToCart_Click(object sender, EventArgs e)
        {

            if (sender is Button addToCart && int.TryParse(addToCart.Name.Replace("addToCart", ""), out int productIndex))
            {
                string apiUrl;
                int userID = _currentUser.Id;
                if (comboBox1.SelectedItem?.ToString() != null) // Category is selected
                {
                    // Get the selected category from the ComboBox
                    string selectedCategory = comboBox1.SelectedItem.ToString();
                    apiUrl = $"https://localhost:7098/Cart/AddProductToCart/{userID}/{productIndex}/{selectedCategory}";
                }
                else
                {


                    apiUrl = $"https://localhost:7098/Cart/AddProductToCart/{userID}/{productIndex}";
                    Console.WriteLine($"apiUrl: {apiUrl}");
                    Console.WriteLine($"productIndex: {productIndex}");

                }

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.PostAsync(apiUrl, null);

                        // Log request URL
                        Console.WriteLine($"Request URL: {apiUrl}");

                        if (response.IsSuccessStatusCode)
                        {
                            // Handle the successful addition to the cart here
                            MessageBox.Show("Product added to the cart successfully");

                            // Log successful response
                            Console.WriteLine("Request succeeded");

                            // You can update the UI to reflect the change in the cart, e.g., update the cart total or quantity.
                            // Refresh the cart UI or update cart-related controls.
                            // UpdateCartUI();

                            // Optionally, you can load or refresh the cart products from the server.
                            LoadProducts();
                        }
                        else
                        {
                            // Handle the case where adding to the cart failed
                            MessageBox.Show($"Failed to add the product to the cart. Status code: {response.StatusCode}");

                            // Log failed response
                            Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions (e.g., network issues)
                        MessageBox.Show($"Error: {ex.Message}");

                        // Log exception details
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CartForm cat = new CartForm(_currentUser);
            cat.Show();
            this.Close();
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
    }
}
public class ProductDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductStock { get; set; }
    public string ImageBase64 { get; set; }
}
