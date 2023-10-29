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
using static System.Net.Mime.MediaTypeNames;
using OnlineFashionShopApp.Models;

namespace OnlineFashionShopApp
{
    // This is the page for product Admin form
    public partial class ProductAdminForm : Form
    {
        private TableLayoutPanel tableLayoutPanel;
        private User _currentUser;
        string apiUrl = "https://localhost:7098/Product/GetProducts";
        private List<ProductDTO> products = new List<ProductDTO>();
        public ProductAdminForm(User currentUser)
        {
            InitializeComponent();
            LoadProducts();
            comboBox1.Items.Add("Apparel");
            comboBox1.Items.Add("Footwear");
            comboBox1.Items.Add("Accessories");
            _currentUser = currentUser;
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
                        // Assuming a container (e.g., a panel) to hold the dynamically created elements.
                        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
                        tableLayoutPanel.Dock = DockStyle.Fill;
                        tableLayoutPanel.AutoScroll = true;
                        tableLayoutPanel.HorizontalScroll.Enabled = false; // Enable vertical scrolling


                        // Set the column count for the TableLayoutPanel (2 columns for two products per row)
                        tableLayoutPanel.ColumnCount = 2;

                        // Set the spacing between rows and columns
                        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                        tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                        // Set a maximum width for the TableLayoutPanel to prevent horizontal scrolling



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
                            stockLabel.Text = "Price";
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

                            Button removeButton = new Button();
                            removeButton.BackColor = Color.Red;
                            removeButton.FlatStyle = FlatStyle.Flat;
                            removeButton.Location = new Point(78, 283);
                            removeButton.Margin = new Padding(3, 4, 3, 4);
                            removeButton.Name = "removeButton" + i; // Use a unique name for each remove button.
                            removeButton.Size = new Size(136, 60);
                            removeButton.TabIndex = 7;
                            removeButton.Text = "Remove Product";
                            removeButton.UseVisualStyleBackColor = false;
                            removeButton.Click += RemoveButton_Click;
                            removeButton.Click += async (sender, e) =>
                            {

                                panel4.Controls.Remove(tableLayoutPanel);
                            };
                            // Add controls to the product panel.

                            productPanel.Controls.Add(nameBox);
                            productPanel.Controls.Add(priceBox);
                            productPanel.Controls.Add(stockBox);
                            productPanel.Controls.Add(removeButton);

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

        private System.Drawing.Image Base64StringToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new System.IO.MemoryStream(imageBytes))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }
        private async void RemoveButton_Click(object sender, EventArgs e)
        {
            // Extract the product index from the button's name
            if (sender is Button removeButton && int.TryParse(removeButton.Name.Replace("removeButton", ""), out int productIndex))
            {
                string apiUrl;
                if (comboBox1.SelectedItem?.ToString() != null) // Category is selected
                {
                    // Get the selected category from the ComboBox
                    string selectedCategory = comboBox1.SelectedItem.ToString();
                    apiUrl = $"https://localhost:7098/Product/RemoveProductByIndex/{productIndex}/{selectedCategory}";
                }
                else
                {
                    // Remove the product by index only
                    apiUrl = $"https://localhost:7098/Product/RemoveProductByIndex/{productIndex}";
                }

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Send an HTTP DELETE request to the API
                        HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            // Handle the successful removal here
                            MessageBox.Show("Product removed successfully");

                            string selectedCategory = comboBox1.SelectedItem?.ToString();

                            // Call the filtering method with the selected category
                            await FilterProductsByCategoryAsync(selectedCategory);
                            // Refresh the UI to reflect the changes

                        }
                        else
                        {
                            // Handle the case where removal failed
                            MessageBox.Show($"Failed to remove the product. Status code: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
        }


        private void button8_Click(object sender, EventArgs e)
        {
            ProductAdminForm podmin = new ProductAdminForm(_currentUser);
            podmin.Show();
            this.Close();
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
                        // Assuming a container (e.g., a panel) to hold the dynamically created elements.
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

                            // Create and customize other control


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
                            stockLabel.Text = "Price";
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

                            Button removeButton = new Button();
                            removeButton.BackColor = Color.Red;
                            removeButton.FlatStyle = FlatStyle.Flat;
                            removeButton.Location = new Point(78, 283);
                            removeButton.Margin = new Padding(3, 4, 3, 4);
                            removeButton.Name = "removeButton" + i; // Use a unique name for each remove button.
                            removeButton.Size = new Size(136, 60);
                            removeButton.TabIndex = 7;
                            removeButton.Text = "Remove Product";
                            removeButton.UseVisualStyleBackColor = false;
                            removeButton.Click += RemoveButton_Click;
                            removeButton.Click += async (sender, e) =>
                            {

                                panel4.Controls.Remove(tableLayoutPanel);
                            };
                            // Add controls to the product panel.

                            productPanel.Controls.Add(nameBox);
                            productPanel.Controls.Add(priceBox);
                            productPanel.Controls.Add(stockBox);
                            productPanel.Controls.Add(removeButton);

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
        public class ProductDTO
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal ProductPrice { get; set; }
            public int ProductStock { get; set; }
            public string ImageBase64 { get; set; }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OrderFormAdmin orfor = new OrderFormAdmin(_currentUser);
            orfor.Show();
            this.Close();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            AddProductForm addProd = new AddProductForm(_currentUser);
            addProd.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HomeFormAdmin homeFormAdmin = new HomeFormAdmin(_currentUser);
            homeFormAdmin.Show();
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