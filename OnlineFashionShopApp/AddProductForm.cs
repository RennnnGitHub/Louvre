using System;
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
    public partial class AddProductForm : Form
    {
        private byte[] imageData;
        public AddProductForm()
        {
            InitializeComponent();
            comboBox1.Items.Add("Apparel");
            comboBox1.Items.Add("Footwear");
            comboBox1.Items.Add("Accessories");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void uploadImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select Product Image";
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file's path
                string selectedImagePath = openFileDialog.FileName;

                // Read the image file and convert it to a byte array
                imageData = File.ReadAllBytes(selectedImagePath);

                // Optionally, you can display the selected image in a PictureBox control if needed.
                imagePictureBox.Image = Image.FromFile(selectedImagePath);
            }
        }
        private async void AddProductButton_Click(object sender, EventArgs e)
        {
            string apiUrl = "https://localhost:7098/Product/AddProduct"; // Replace with the actual API URL for adding a product.
            if (imageData == null)
            {
                MessageBox.Show("Please upload an image.");
                return;
            }
            if (textBox1.Text == null || textBox2.Text == null || textBox3.Text == null || imageData == null || comboBox1.SelectedItem?.ToString() == null)
            {
                MessageBox.Show("Please input all details.");
                return;
            }
            string selectedCategory = comboBox1.SelectedItem?.ToString() ?? "";

            // Define the JSON payload as a string
            string jsonPayload = "{\"productName\": \"" + textBox1.Text + "\", " +
                                 "\"productPrice\": " + textBox2.Text + ", " +
                                 "\"productStock\": " + textBox3.Text + ", " +
                                 "\"productImage\": \"" + Convert.ToBase64String(imageData) + "\", " +
                                 "\"category\": \"" + selectedCategory + "\"}";
            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create a StringContent with the JSON payload and specify the content type
                    StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Make a POST request with the JSON payload to add the product
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        // You can process the API response (result) as needed.
                        MessageBox.Show("Product added successfully");
                        this.Hide();

                        // Open the ProductAdminForm to refresh the product listing
                        AddProductForm Addprod = new AddProductForm();
                        Addprod.Show();
                    }
                    else
                    {
                        // Handle the response if it's not successful (e.g., display an error message).
                        MessageBox.Show($"Failed to add the product. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., network issues).
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void productsPage_Click(object sender, EventArgs e)
        {
            ProductAdminForm prodAdmin = new ProductAdminForm();
            prodAdmin.Show();
        }
    }
}
