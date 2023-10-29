using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OnlineFashionShopApp.Models;
namespace OnlineFashionShopApp
{
    public partial class HomeFormCustomer : Form
        //This is the page for Customer Home Form
    {
        private User _currentUser;
        public HomeFormCustomer(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            ProductCustomerForm prodCust = new ProductCustomerForm(_currentUser);
            this.Hide();
            prodCust.Show(); // Use Show() to open the new form
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OrderFormCustomer orderform = new OrderFormCustomer(_currentUser);
            orderform.Show();
            this.Close();
        }

        private async void button5_Click(object sender, EventArgs e) //button cart
        {
            CartForm cartForm = new CartForm(_currentUser);
            cartForm.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TrackingForm cartForm = new TrackingForm(_currentUser);
            cartForm.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //settings
            SettingsForm form = new SettingsForm();
            this.Hide();
            form.ShowDialog();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ProductCustomerForm productCustomer = new ProductCustomerForm(_currentUser);
            productCustomer.ShowDialog();
            this.Hide();
        }

        private void HomeFormCustomer_Load(object sender, EventArgs e)
        {
            label1.Text = "Welcome " + _currentUser.Firstname + " " + _currentUser.Lastname;
        }
    }
}
