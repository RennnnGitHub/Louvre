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
    public partial class HomeFormAdmin : Form
    {
        private User _currentUser;
        public HomeFormAdmin(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            HomeFormAdmin homeFormAdmin = new HomeFormAdmin(_currentUser);
            homeFormAdmin.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ProductAdminForm productAdminForm = new ProductAdminForm(_currentUser);
            productAdminForm.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OrderFormAdmin orderFormAdmin = new OrderFormAdmin(_currentUser);
            orderFormAdmin.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //access log
            AccessLogForm form = new AccessLogForm();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TrackingFormAdmin trackingForm = new TrackingFormAdmin(_currentUser);
            trackingForm.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //settings
            SettingsForm form = new SettingsForm();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HomeFormAdmin_Load(object sender, EventArgs e)
        {
            label1.Text = "Welcome " + _currentUser.Firstname + " " + _currentUser.Lastname;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }
    }
}
