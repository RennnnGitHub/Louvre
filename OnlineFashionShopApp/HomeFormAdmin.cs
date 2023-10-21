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

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

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

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HomeFormAdmin_Load(object sender, EventArgs e)
        {
            label1.Text = "Welcome " + _currentUser.Firstname + " " + _currentUser.Lastname;
        }
    }
}
