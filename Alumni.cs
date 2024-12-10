using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace GymSystem
{
    public partial class Alumni : Form
    {
        private string connectionString = (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Relmie\Documents\gymdb.mdf;Integrated Security=True;Connect Timeout=30");

        public Alumni()
        {
            InitializeComponent();
        }
       
        private void LoadData()
        {
            string query = "SELECT * FROM AlumniTbl"; // Retrieve all records from StudentTbl

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    AlumniGV.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
           BuyTicket ticket = new BuyTicket();
            ticket.Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dtpDate.Text) ||
              string.IsNullOrWhiteSpace(dtpTime.Text) ||
              string.IsNullOrWhiteSpace(txtName.Text) ||
              string.IsNullOrWhiteSpace(txtAge.Text) ||
              string.IsNullOrWhiteSpace(txtPhoneNum.Text) ||
              string.IsNullOrWhiteSpace(txtEmail.Text) ||
              string.IsNullOrWhiteSpace(txtAddress.Text) ||
              string.IsNullOrWhiteSpace(cmbPaymentMethod.Text) ||
              string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MessageBox.Show("Please fill out all required fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string queryInsertAlumni = "INSERT INTO AlumniTbl (Date, Time, AlumniName, Age, PhoneNum, Email, Address, PaymentMethod, AmountPaid) " +
                                         "VALUES (@Date, @Time, @AlumniName, @Age, @PhoneNum, @Email, @Address, @PaymentMethod, @AmountPaid)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Insert into EmployeeTbl
                    using (SqlCommand cmd = new SqlCommand(queryInsertAlumni, con))
                    {
                        // Add parameters to the query

                        cmd.Parameters.AddWithValue("@Date", DateTime.Parse(dtpDate.Text));
                        cmd.Parameters.AddWithValue("@Time", DateTime.Parse(dtpTime.Text));
                        cmd.Parameters.AddWithValue("@AlumniName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Age", int.TryParse(txtAge.Text, out int age) ? age : 0); // Convert Age to integer
                        cmd.Parameters.AddWithValue("@PhoneNum", txtPhoneNum.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@PaymentMethod", cmbPaymentMethod.SelectedItem?.ToString());
                        cmd.Parameters.AddWithValue("@AmountPaid", decimal.TryParse(txtAmount.Text, out decimal amount) ? amount : 0); // Convert AmountPaid to decimal

                        cmd.ExecuteNonQuery();
                    }

                    // Refresh the data grid
                    LoadData();

                    MessageBox.Show("Record Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally, clear fields after successful insertion
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Method to clear the input fields
            void ClearFields()
            {
                dtpDate.Value = DateTime.Now;
                dtpTime.Value = DateTime.Now;
                txtName.Clear();
                txtAge.Clear();
                txtPhoneNum.Clear();
                txtEmail.Clear();
                txtAddress.Clear();
                cmbPaymentMethod.SelectedIndex = -1;
                txtAmount.Clear();
            }
        }

        private void AlumniGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM AlumniTbl";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                AlumniGV.DataSource = table;

                btnDelete.Enabled =AlumniGV.SelectedRows.Count > 0;

               
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (AlumniGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            // Get the AlumniId from the selected row
            string selectedAlumniId = AlumniGV.SelectedRows[0].Cells["AlumniId"].Value.ToString();

            // Confirm deletion
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete the record with Alumni ID? {selectedAlumniId}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM AlumniTbl WHERE AlumniId = @AlumniId";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            // Add parameter for AlumniId
                            cmd.Parameters.AddWithValue("@AlumniId", selectedAlumniId);

                            // Execute delete query
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record deleted successfully!");

                                // Refresh DataGridView
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete the record. Please try again.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM AlumniTbl";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                AlumniGV.DataSource = dt; // Populate DataGridView
                            }
                            else
                            {
                                MessageBox.Show("No records found.");
                                AlumniGV.DataSource = null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (AlumniGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected Alumni ID
            string selectedAlumniId = AlumniGV.SelectedRows[0].Cells["AlumniId"].Value.ToString();

            string queryUpdate = "UPDATE AlumniTbl SET " +
                                 "Date = @NewDate, Time = @NewTime, AlumniName = @NewAlumniName, " +
                                 "Age = @NewAge, PhoneNum = @NewPhoneNum, Email = @NewEmail, " +
                                 "Address = @NewAddress, PaymentMethod = @NewPaymentMethod, " +
                                 "AmountPaid = @NewAmountPaid " +
                                 "WHERE AlumniId = @AlumniId"; // Add a WHERE clause to target the correct record

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(queryUpdate, con))
                    {
                        // New values
                        cmd.Parameters.AddWithValue("@NewDate", dtpDate.Value.Date);
                        cmd.Parameters.AddWithValue("@NewTime", dtpTime.Value);
                        cmd.Parameters.AddWithValue("@NewAlumniName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewAge", int.Parse(txtAge.Text));
                        cmd.Parameters.AddWithValue("@NewPhoneNum", txtPhoneNum.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewEmail", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewAddress", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewPaymentMethod", cmbPaymentMethod.Text);
                        cmd.Parameters.AddWithValue("@NewAmountPaid", decimal.Parse(txtAmount.Text));

                        // Add the parameter for the AlumniId (condition)
                        cmd.Parameters.AddWithValue("@AlumniId", selectedAlumniId);

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No matching record found to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // Refresh the DataGridView
                        LoadData();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            void ClearFields()
            {
                dtpDate.Value = DateTime.Now;
                dtpTime.Value = DateTime.Now;
                txtName.Clear();
                txtAge.Clear();
                txtPhoneNum.Clear();
                txtEmail.Clear();
                txtAddress.Clear();
                cmbPaymentMethod.SelectedIndex = -1;
                txtAmount.Clear();
            }
        }

            private void btnSearch_Click(object sender, EventArgs e)
        {
            // Ensure the user has entered an Alumni ID to search
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Please enter an Alumni ID to search.");
                return;
            }

            string alumniId = txtID.Text; // Get the input Alumni ID
            string query = "SELECT * FROM AlumniTbl WHERE AlumniId = @AlumniId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add the parameter for AlumniId
                        cmd.Parameters.AddWithValue("@AlumniId", alumniId);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Bind the result to the DataGridView
                        if (dt.Rows.Count > 0)
                        {
                            AlumniGV.DataSource = dt; // Display matching data
                        }
                        else
                        {
                            MessageBox.Show("No record found for the given Alumni ID.");
                            AlumniGV.DataSource = null; // Clear the grid if no record is found
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            // Check if the input is a valid number
            if (int.TryParse(txtAmount.Text, out int value))
            {
                if (value == 50)
                {
                    // If the input is 50
                    MessageBox.Show("The input is 50 and accepted.");
                }
                else
                {
                    // If the input is not 50
                    MessageBox.Show("Your amount is not correct.");
                }
            }
            else
            {
                // If the input is not a number
                // MessageBox.Show("Please enter a valid number.");
            }
        }


    }
}









