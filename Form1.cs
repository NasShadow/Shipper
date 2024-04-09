using Giao_Diện_Shipper.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Giao_Diện_Shipper
{
    public partial class Form1 : Form
    {
        public Form1(string email)
        {
            InitializeComponent();
            UC_Home uc = new UC_Home(email);
            addUserControl(uc);

            label2.Text = email;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
         
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(userControl);
            userControl.BringToFront();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            UC_Home uc = new UC_Home(label2.Text);
            addUserControl(uc);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            UC_Inbox uc = new UC_Inbox(label2.Text);
            addUserControl(uc);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            UC_Sent uc = new UC_Sent(label2.Text);
            addUserControl(uc);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            UC_Settings uc = new UC_Settings(label2.Text);
            addUserControl(uc);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            UC_Settings uc = new UC_Settings(label2.Text);
            addUserControl(uc);
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            UC_Sent uc = new UC_Sent(label2.Text);
            addUserControl(uc);
        }

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
