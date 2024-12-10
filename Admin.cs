using System;
using System.Windows.Forms;

namespace GymSystem
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        // 2D array to hold username and password pairs
        private readonly string[,] adminCredentials = new string[3, 2]
        {
            { "relmie", "123456789" },  
            { "alvin", "villanueva" },    
            { "zab", "alinsunurin" }
        };

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the values from the TextBoxes
                string email = txtUserName.Text.Trim();
                string password = txtPassword.Text.Trim(); 

                // Ensure email and password are not empty
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("All fields are required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Flag to check if login is successful
                bool loginSuccessful = false;

                // Iterate over the admin credentials array
                for (int i = 0; i < adminCredentials.GetLength(0); i++)
                {
                    // Case-insensitive comparison for username and password
                    if (string.Equals(email, adminCredentials[i, 0], StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(password, adminCredentials[i, 1], StringComparison.Ordinal))
                    {
                        loginSuccessful = true;
                        break;
                    }
                }

                if (loginSuccessful)
                {
                    // Login successful, navigate to the History form
                   BuyTicket buyTicket = new BuyTicket();
                      buyTicket.Show(); 
                      this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid email or password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblClear_Click(object sender, EventArgs e)
        {
            txtUserName.Clear();
            txtPassword.Clear();
            txtUserName.Focus();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
