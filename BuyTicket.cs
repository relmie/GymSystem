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
    public partial class BuyTicket : Form
    {
        public BuyTicket()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnStudent_Click(object sender, EventArgs e)
        {
            Student student = new Student();
            student.Show();
            this.Hide();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            employee.Show();
            this.Hide();
        }

        private void btnAlumni_Click(object sender, EventArgs e)
        {
            Alumni alumni = new Alumni();
            alumni.Show();
            this.Hide();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainForm main = new MainForm();
            main.Show();
            this.Hide();
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            Receipt receipt = new Receipt();
            receipt.Show();
            this.Hide();
        }
    }
}
