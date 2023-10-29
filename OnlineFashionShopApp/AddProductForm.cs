using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnlineFashionShopApp.Models;

namespace OnlineFashionShopApp
{
    // This is the Page foe Add Product
    public partial class AddProductForm : Form
    {
        private byte[] imageData;
        private User _currentUser;
        public AddProductForm(User currentUser)
        {
            InitializeComponent();
            comboBox1.Items.Add("Apparel");
            comboBox1.Items.Add("Footwear");
            comboBox1.Items.Add("Accessories");
            _currentUser = currentUser;
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

                // display the selected image in a PictureBox control
                imagePictureBox.Image = Image.FromFile(selectedImagePath);
            }
        }
        private async void AddProductButton_Click(object sender, EventArgs e)
        {
            string apiUrl = "https://localhost:7098/Product/AddProduct";
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
                    // Create a StringContent with the JSON payload
                    StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Make a POST requestt
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        //process the API response as needed.
                        MessageBox.Show("Product added successfully");
                        this.Hide();

                        // Open the ProductAdminForm to refresh the product listing
                        AddProductForm Addprod = new AddProductForm(_currentUser);
                        Addprod.Show();
                    }
                    else
                    {
                        // Handle the response if it's not successful
                        MessageBox.Show($"Failed to add the product. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void productsPage_Click(object sender, EventArgs e)
        {
            ProductAdminForm prodAdmin = new ProductAdminForm(_currentUser);
            prodAdmin.Show();
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
            OrderFormAdmin orfor = new OrderFormAdmin(_currentUser);
            orfor.ShowDialog();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TrackingFormAdmin track = new TrackingFormAdmin(_currentUser);
            track.ShowDialog();
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