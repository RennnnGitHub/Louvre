using OnlineFashionShopApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineFashionShopApp
{
    public partial class AddressListForm : Form
    {

        private User _currentUser;
        public AddressListForm()
        {
            InitializeComponent();
            ObjectCache cache = MemoryCache.Default;
            _currentUser = (User)cache.Get("currentUser");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
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

        private void button5_Click(object sender, EventArgs e)
        {
            CartForm cat = new CartForm(_currentUser);
            cat.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TrackingForm tf = new TrackingForm(_currentUser);
            tf.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private async void button10_Click(object sender, EventArgs e) //button for selecting shipment
        {
            //change selected shipment
            string apiUrl = "https://localhost:7098/User/UpdateSelectedShipment"; // Replace with the actual API URL.


            string jsonPayload = JsonSerializer.Serialize(new
            {
                id = _currentUser.Id,
                shipmentId = lvwAddress.SelectedItems[0].Tag
            });

            // Define the JSON payload as a string
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create a StringContent with the JSON payload and specify the content type
                    StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Make a POST request with the JSON payload
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        foreach (ListViewItem i in lvwAddress.Items)
                        {
                            i.BackColor = Color.White;
                            if (i.Tag == lvwAddress.SelectedItems[0].Tag)
                            {
                                i.BackColor = Color.DarkGreen;
                            }
                        }
                        MessageBox.Show("Shipment Selected");

                    }
                    else
                    {
                        // Handle the response if it's not successful (e.g., display an error message).
                        MessageBox.Show($"Failed to post data. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., network issues).
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnAddAddress_Click(object sender, EventArgs e)
        {
            string apiUrl = "https://localhost:7098/Shipping/InsertAddress"; // Replace with the actual API URL.


            string jsonPayload = JsonSerializer.Serialize(new
            {
                userId = _currentUser.Id,
                shippingAddress1 = txtAddress.Text,
                shippingAddress2 = "",
                shippingCountry = "",
                shippingCity = "",
                shippingState = "",
                shippingPostalCode = ""
            });

            // Define the JSON payload as a string
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create a StringContent with the JSON payload and specify the content type
                    StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Make a POST request with the JSON payload
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        // You can process the API response (result) as needed.

                        JsonNode obj = JsonObject.Parse(result);
                        int i = lvwAddress.Items.Count + 1;
                        ListViewItem item = new ListViewItem(i.ToString());
                        item.SubItems.Add(
                           txtAddress.Text
                        );
                        item.Tag = (int)obj["shipmentId"];
                        lvwAddress.Items.Add(item);
                        MessageBox.Show("Address Inserted");
                    }
                    else
                    {
                        // Handle the response if it's not successful (e.g., display an error message).
                        MessageBox.Show($"Failed to post data. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., network issues).
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private async void AddressListForm_Load(object sender, EventArgs e)
        {
            string apiUrl = "https://localhost:7098/Shipping/GetShipmentsByUserId"; // Replace with the actual API URL.


            string jsonPayload = JsonSerializer.Serialize(new
            {
                id = _currentUser.Id
            });

            // Define the JSON payload as a string
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create a StringContent with the JSON payload and specify the content type
                    StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Make a POST request with the JSON payload
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        // You can process the API response (result) as needed.

                        JsonNode obj = JsonObject.Parse(result);
                        int i = 1;
                        foreach (var log in obj.AsArray())
                        {
                            ListViewItem item = new ListViewItem(i.ToString());
                            item.SubItems.Add(
                                log["shippingAddress1"].ToString() + " " + log["shippingAddress2"].ToString() + "," + log["shippingCountry"].ToString()
                            );
                            item.Tag = (int)log["id"];
                            if (_currentUser.ShipmentId == (int)log["id"])
                            {
                                item.BackColor = Color.DarkGreen;
                            }
                            lvwAddress.Items.Add(item);
                            i++;
                        }


                    }
                    else
                    {
                        // Handle the response if it's not successful (e.g., display an error message).
                        MessageBox.Show($"Failed to post data. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., network issues).
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private async void button9_Click_1(object sender, EventArgs e)
        {
            //delete
            string apiUrl = "https://localhost:7098/Shipping/DeleteAddress/" + _currentUser.Id + "/" + lvwAddress.SelectedItems[0].Tag;

            // Define the JSON payload as a string
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Create a StringContent with the JSON payload and specify the content type
                    //StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Make a POST request with the JSON payload
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        // You can process the API response (result) as needed.
                        MessageBox.Show("Address Deleted");
                        lvwAddress.SelectedItems[0].Remove();
                        int i = 1;
                        foreach (ListViewItem lvi in lvwAddress.Items)
                        {
                            lvi.Text = i.ToString();
                            i++;
                        }
                    }
                    else
                    {
                        // Handle the response if it's not successful (e.g., display an error message).
                        MessageBox.Show($"Failed to post data. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., network issues).
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}
