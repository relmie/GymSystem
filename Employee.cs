using GymSystem;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GymSystem
{
    public partial class Employee : Form
    {
        // Connection string to the gymdb database
        private string connectionString = (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Relmie\Documents\gymdb.mdf;Integrated Security=True;Connect Timeout=30");

        public Employee()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            string query = "SELECT * FROM EmployeeTbl"; // Retrieve all records from StudentTbl

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind the data to the DataGridView
                    EmployeeGV.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }




        // Event handler for the Back button
        private void btnBack_Click(object sender, EventArgs e)
        {
            BuyTicket ticket = new BuyTicket();
            ticket.Show();
            this.Hide();
        }

        private void Employee_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbEmployee.Text) ||
                  string.IsNullOrWhiteSpace(dtpDate.Text) ||
                  string.IsNullOrWhiteSpace(dtpTime.Text) ||
                  string.IsNullOrWhiteSpace(txtName.Text) ||
                  string.IsNullOrWhiteSpace(txtAge.Text) ||
                  string.IsNullOrWhiteSpace(txtPhoneNum.Text) ||
                  string.IsNullOrWhiteSpace(txtEmail.Text) ||
                  string.IsNullOrWhiteSpace(txtAddress.Text) ||
                  string.IsNullOrWhiteSpace(txtDepartment.Text) ||
                  string.IsNullOrWhiteSpace(cmbPaymentMethod.Text) ||
                  string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MessageBox.Show("Please fill out all required fields.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string queryInsertEmployee = "INSERT INTO EmployeeTbl (Employee, Date, Time, Name, Age, PhoneNum, Email, Address, Department, PaymentMethod, AmountPaid) " +
                                         "VALUES (@Employee, @Date, @Time, @Name, @Age, @PhoneNum, @Email, @Address, @Department, @PaymentMethod, @AmountPaid)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Insert into EmployeeTbl
                    using (SqlCommand cmd = new SqlCommand(queryInsertEmployee, con))
                    {
                        // Add parameters to the query
                        cmd.Parameters.AddWithValue("@Employee", cmbEmployee.SelectedItem?.ToString());
                        cmd.Parameters.AddWithValue("@Date", DateTime.Parse(dtpDate.Text));
                        cmd.Parameters.AddWithValue("@Time", DateTime.Parse(dtpTime.Text));
                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Age", int.TryParse(txtAge.Text, out int age) ? age : 0); // Convert Age to integer
                        cmd.Parameters.AddWithValue("@PhoneNum", txtPhoneNum.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@Department", txtDepartment.Text.Trim());
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
                cmbEmployee.SelectedIndex = -1;
                dtpDate.Value = DateTime.Now;
                dtpTime.Value = DateTime.Now;
                txtName.Clear();
                txtAge.Clear();
                txtPhoneNum.Clear();
                txtEmail.Clear();
                txtAddress.Clear();
                txtDepartment.Clear();
                cmbPaymentMethod.SelectedIndex = -1;
                txtAmount.Clear();
            }
        }

        private void EmployeeGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index is valid
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow row = EmployeeGV.Rows[e.RowIndex];

                // Populate the fields with the selected row's data
                cmbEmployee.Text = row.Cells["Employee"].Value?.ToString();
                dtpDate.Value = DateTime.TryParse(row.Cells["Date"].Value?.ToString(), out DateTime date) ? date : DateTime.Now;
                dtpTime.Value = DateTime.TryParse(row.Cells["Time"].Value?.ToString(), out DateTime time) ? time : DateTime.Now;
                txtName.Text = row.Cells["Name"].Value?.ToString();
                txtAge.Text = row.Cells["Age"].Value?.ToString();
                txtPhoneNum.Text = row.Cells["PhoneNum"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtAddress.Text = row.Cells["Address"].Value?.ToString();
                txtDepartment.Text = row.Cells["Department"].Value?.ToString();
                cmbPaymentMethod.Text = row.Cells["PaymentMethod"].Value?.ToString();
                txtAmount.Text = row.Cells["AmountPaid"].Value?.ToString();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM EmployeeTbl";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                EmployeeGV.DataSource = table;

                btnDelete.Enabled = EmployeeGV.SelectedRows.Count > 0;

                LoadData();
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (EmployeeGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            // Get the EmployeeId from the selected row
            string selectedEmployeeId = EmployeeGV.SelectedRows[0].Cells["EmployeeId"].Value.ToString();

            // Confirm deletion
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete the record with Employee ID {selectedEmployeeId}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM EmployeeTbl WHERE EmployeeId = @EmployeeId";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            // Add parameter for EmployeeId
                            cmd.Parameters.AddWithValue("@EmployeeId", selectedEmployeeId);

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
            string query = "SELECT * FROM EmployeeTbl";

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
                                EmployeeGV.DataSource = dt; // Populate DataGridView
                            }
                            else
                            {
                                MessageBox.Show("No records found.");
                                EmployeeGV.DataSource = null;
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
            if (EmployeeGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected Alumni ID
            string selectedEmployeeId = EmployeeGV.SelectedRows[0].Cells["EmployeeId"].Value.ToString();

            string queryUpdate = "UPDATE EmployeeTbl SET " +
                                "Employee = @NewEmployee, Date = @NewDate, Time = @NewTime, Name = @NewName, " +
                                 "Age = @NewAge, PhoneNum = @NewPhoneNum, Email = @NewEmail, " +
                                 "Address = @NewAddress, Department = @NewDepartment, PaymentMethod = @NewPaymentMethod, " +
                                 "AmountPaid = @NewAmountPaid " +
                                 "WHERE EmployeeId = @EmployeeId"; // Add a WHERE clause to target the correct record

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(queryUpdate, con))
                    {
                        // New values
                        cmd.Parameters.AddWithValue("@NewEmployee", cmbEmployee.Text);
                        cmd.Parameters.AddWithValue("@NewDate", dtpDate.Value.Date);
                        cmd.Parameters.AddWithValue("@NewTime", dtpTime.Value);
                        cmd.Parameters.AddWithValue("@NewName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewAge", int.Parse(txtAge.Text));
                        cmd.Parameters.AddWithValue("@NewPhoneNum", txtPhoneNum.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewEmail", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewAddress", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewDepartment", txtDepartment.Text);
                        cmd.Parameters.AddWithValue("@NewPaymentMethod", cmbPaymentMethod.Text);
                        cmd.Parameters.AddWithValue("@NewAmountPaid", decimal.Parse(txtAmount.Text));

                        // Add the parameter for the AlumniId (condition)
                        cmd.Parameters.AddWithValue("@EmployeeId", selectedEmployeeId);

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
                cmbEmployee.SelectedIndex = -1;
                dtpDate.Value = DateTime.Now;
                dtpTime.Value = DateTime.Now;
                txtName.Clear();
                txtAge.Clear();
                txtPhoneNum.Clear();
                txtEmail.Clear();
                txtAddress.Clear();
                txtDepartment.Clear();
                cmbPaymentMethod.SelectedIndex = -1;
                txtAmount.Clear();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Ensure the user has entered an Employee ID to search
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Please enter an Employee ID to search.");
                return;
            }

            string employeeId = txtID.Text; // Get the input Employee ID
            string query = "SELECT * FROM EmployeeTbl WHERE EmployeeId = @EmployeeId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add the parameter for EmployeeId
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Bind the result to the DataGridView
                        if (dt.Rows.Count > 0)
                        {
                            EmployeeGV.DataSource = dt; // Display matching data
                        }
                        else
                        {
                            MessageBox.Show("No record found for the given Employee ID.");
                            EmployeeGV.DataSource = null; // Clear the grid if no record is found
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
                if (value == 20)
                {
                    // If the input is 20
                    MessageBox.Show("The input is 20 and accepted.");
                }
                else
                {
                    // If the input is not 20
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



































