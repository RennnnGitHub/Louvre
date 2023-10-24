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

namespace OnlineFashionShopApp
{
    public partial class HomeFormCustomer : Form
    {
        public HomeFormCustomer()
        {
            InitializeComponent();
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
            ProductCustomerForm prodCust = new ProductCustomerForm();
            prodCust.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OrderFormCustomer orderform = new OrderFormCustomer();
            orderform.ShowDialog();
            this.Close();
        }

        private async void button5_Click(object sender, EventArgs e) //button cart
        {
            CartForm cartForm = new CartForm();
            cartForm.ShowDialog();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ProductCustomerForm productCustomer = new ProductCustomerForm();
            productCustomer.ShowDialog();
            this.Close();
        }
    }
}
