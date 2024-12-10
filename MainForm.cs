using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Admin admin = new Admin();
            admin.Show();
            this.Hide();
        }

        private void btnBuyTicket_Click(object sender, EventArgs e)
        {
            BuyTicket buyTicket = new BuyTicket();
            buyTicket.Show();   
            this.Hide();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            Aboutus aboutus = new Aboutus();
            aboutus.Show();
            this.Hide();
        }

        private void btnEquipments_Click(object sender, EventArgs e)
        {
            Equipments equipments = new Equipments();
            equipments.Show(); 
            this.Hide();
        }
    }
}
