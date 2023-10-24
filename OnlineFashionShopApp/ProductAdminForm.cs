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
namespace OnlineFashionShopApp
{
    public partial class ProductAdminForm : Form
    {
        string apiUrl = "https://localhost:7098/Product/GetProducts";
        private List<ProductDTO> products = new List<ProductDTO>();
        public ProductAdminForm()
        {
            InitializeComponent();
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
                            PropertyNameCaseInsensitive = true // Use this option for case-insensitive property matching
                        });


                        // No products available, clear or hide the UI elements.


                        for (int i = 0; i < products.Count; i++)
                        {
                            if (i >= 10) // Assuming you have PictureBox, TextBox, and other controls named systematically
                                break;
                            
                            PictureBox pictureBox = (PictureBox)this.Controls.Find("pictureBox" + (i + 1), true).FirstOrDefault();
                            
                            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; // To display the image in its original size.
                            pictureBox.Dock = DockStyle.Fill;
                            TextBox nameBox = (TextBox)this.Controls.Find("nameBox" + (i + 1), true).FirstOrDefault();
                            TextBox priceBox = (TextBox)this.Controls.Find("priceBox" + (i + 1), true).FirstOrDefault();
                            TextBox stockBox = (TextBox)this.Controls.Find("stockBox" + (i + 1), true).FirstOrDefault();

                            if (pictureBox != null && nameBox != null && priceBox != null && stockBox != null)
                            {
                                Image image = Base64StringToImage(products[i].ImageBase64);
                                pictureBox.Image = image;

                                nameBox.Text = products[i].ProductName;
                                priceBox.Text = products[i].ProductPrice.ToString();
                                stockBox.Text = products[i].ProductStock.ToString();
                            }
                        }

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

        private void ClearProductUI(int productCount)
        {
            if (productCount < 7)
            {
                pictureBox7.Image = null;
                nameBox7.Text = null;
                priceBox7.Text = null;
                stockBox7.Text = null;
            }
            if (productCount < 6)
            {
                pictureBox6.Image = null;
                nameBox6.Text = null;
                priceBox6.Text = null;
                stockBox6.Text = null;
            }
            if (productCount < 5)
            {
                pictureBox5.Image = null;
                nameBox5.Text = null;
                priceBox5.Text = null;
                stockbox5.Text = null;

            }
            if (productCount < 4)
            {
                pictureBox4.Image = null;
                nameBox4.Text = null;
                priceBox4.Text = null;
                stockBox4.Text = null;
            }
            if (productCount < 3)
            {
                pictureBox3.Image = null;
                nameBox3.Text = null;
                priceBox3.Text = null;
                stockBox3.Text = null;

            }
            if (productCount < 2)
            {
                pictureBox2.Image = null;
                nameBox2.Text = null;
                priceBox2.Text = null;
                stockBox2.Text = null;

            }
            if (productCount < 1)
            {
                pictureBox1.Image = null;
                nameBox1.Text = null;
                priceBox1.Text = null;
                stockBox1.Text = null;
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
                    ClearProductUI(productIndex);

                    // Refresh the UI to reflect the changes
                    LoadProducts();
                }
                else
                {
                    // Handle the case where removal failed
                    MessageBox.Show($"Failed to remove the product. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., network issues)
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
        

        private void button8_Click(object sender, EventArgs e)
        {
            ProductAdminForm podmin = new ProductAdminForm();
            podmin.Show();
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
            try
            {
                using (var client = new HttpClient())
                {
                    // Construct the API URL for filtering by category
                    string apiUrl = $"https://localhost:7098/Product/GetProductsFilter?category={category}";

                    var response = await client.GetAsync(apiUrl);
                    ClearProductUI(0);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var products = System.Text.Json.JsonSerializer.Deserialize<List<ProductDTO>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        // Clear or hide the UI elements if needed.

                        for (int i = 0; i < products.Count; i++)
                        {
                            if (i >= 10) // Assuming you have PictureBox, TextBox, and other controls named systematically
                                break;

                            PictureBox pictureBox = (PictureBox)this.Controls.Find("pictureBox" + (i + 1), true).FirstOrDefault();
                            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; // To display the image in its original size.
                            pictureBox.Dock = DockStyle.Fill;
                            TextBox nameBox = (TextBox)this.Controls.Find("nameBox" + (i + 1), true).FirstOrDefault();
                            TextBox priceBox = (TextBox)this.Controls.Find("priceBox" + (i + 1), true).FirstOrDefault();
                            TextBox stockBox = (TextBox)this.Controls.Find("stockBox" + (i + 1), true).FirstOrDefault();

                            if (pictureBox != null && nameBox != null && priceBox != null && stockBox != null)
                            {
                                Image image = Base64StringToImage(products[i].ImageBase64);
                                pictureBox.Image = image;

                                nameBox.Text = products[i].ProductName;
                                priceBox.Text = products[i].ProductPrice.ToString();
                                stockBox.Text = products[i].ProductStock.ToString();
                            }
                        }
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
}
    }