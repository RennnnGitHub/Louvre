using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OnlineFashionShopApp.Models;


namespace OnlineFashionShopApp
{
    public partial class HomeForm : Form
    {
        private User _currentUser;
        public HomeForm()
        {
            InitializeComponent();
            ObjectCache cache = MemoryCache.Default;
            _currentUser = (User)cache.Get("currentUser");
            if (_currentUser.Userrole == 0)
            {
                pbxCart.Visible = false;
                btnCart.Visible = false;
                pbxCart.Enabled = false;
                btnCart.Enabled = false;
                pbxLogAccess.Visible = true;
                btnAccessLog.Visible = true;
                pbxLogAccess.Enabled = true;
                btnAccessLog.Enabled = true;
            }
            else
            {
                pbxLogAccess.Visible = false;
                btnAccessLog.Visible = false;
                pbxLogAccess.Enabled = false;
                btnAccessLog.Enabled = false;
                pbxCart.Visible = true;
                btnCart.Visible = true;
                pbxCart.Enabled = true;
                btnCart.Enabled = true;
            }
            pnlMain.Visible = false;
            pnlMain.BringToFront();

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
            //access log
            AccessLogForm form = new AccessLogForm();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //settings
            SettingsForm form = new SettingsForm();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e) //logout
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            label1.Text = "Welcome " + _currentUser.Firstname + " " + _currentUser.Lastname;
        }

        private void btnCart_Click(object sender, EventArgs e)
        {
           
        }
    }
}
