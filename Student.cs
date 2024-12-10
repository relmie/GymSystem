using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymSystem
{

    public partial class Student : Form
    {
        string connectionString = ("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Relmie\\Documents\\gymdb.mdf;Integrated Security=True;Connect Timeout=30");

        public Student()
        {
            InitializeComponent();
        }


        private void LoadData()
        {
            string query = "SELECT * FROM StudentTbl"; // Retrieve all records from StudentTbl

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind the data to the DataGridView
                    StudentGV.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            BuyTicket buyTicket = new BuyTicket();
            buyTicket.Show();
            this.Hide();
        }


        private void Student_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            // Check if the input is a valid number
            if (int.TryParse(txtAmount.Text, out int value))
            {
                if (value == 30)
                {
                    // If the input is 30
                    MessageBox.Show("The input is 30 and accepted.");
                }
                else
                {
                    // If the input is not 30
                    MessageBox.Show("Your amount is not correct.");
                }
            }
            else
            {
                // If the input is not a number
                // MessageBox.Show("Please enter a valid number.");
            }
        }
    

        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM StudentTbl";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                StudentGV.DataSource = table;

                btnDelete.Enabled = StudentGV.SelectedRows.Count > 0;

                LoadData();
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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
             string.IsNullOrWhiteSpace(txtCourse.Text) ||
             cmbDepartment.SelectedItem == null ||
             cmbPaymentMethod.SelectedItem == null ||
             string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MessageBox.Show("Please fill out all required fields.");
                return;
            }

            // Validate Age and AmountPaid as numeric values
            if (!int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("Invalid age. Please enter a numeric value.");
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amountPaid))
            {
                MessageBox.Show("Invalid amount paid. Please enter a numeric value.");
                return;
            }

            // SQL query to insert a new record
            string queryInsertStudent = "INSERT INTO StudentTbl (Date, Time, StudentName, Age, PhoneNum, Email, Address, Course, Department, PaymentMethod, AmountPaid) " +
                                         "VALUES (@Date, @Time, @StudentName, @Age, @PhoneNum, @Email, @Address, @Course, @Department, @PaymentMethod, @AmountPaid)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Insert the new record
                    using (SqlCommand cmdStudent = new SqlCommand(queryInsertStudent, con))
                    {
                        cmdStudent.Parameters.AddWithValue("@Date", dtpDate.Value);
                        cmdStudent.Parameters.AddWithValue("@Time", dtpTime.Value);
                        cmdStudent.Parameters.AddWithValue("@StudentName", txtName.Text);
                        cmdStudent.Parameters.AddWithValue("@Age", age); // Ensure age is passed as an integer
                        cmdStudent.Parameters.AddWithValue("@PhoneNum", txtPhoneNum.Text);
                        cmdStudent.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmdStudent.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmdStudent.Parameters.AddWithValue("@Course", txtCourse.Text);
                        cmdStudent.Parameters.AddWithValue("@Department", cmbDepartment.SelectedItem?.ToString());
                        cmdStudent.Parameters.AddWithValue("@PaymentMethod", cmbPaymentMethod.SelectedItem?.ToString());
                        cmdStudent.Parameters.AddWithValue("@AmountPaid", amountPaid); // Ensure AmountPaid is passed as decimal

                        cmdStudent.ExecuteNonQuery(); // Execute the query
                    }

                    // Reload data to show the new record
                    LoadData();

                    MessageBox.Show("Record Added Successfully!");

                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
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
            txtCourse.Clear();
            cmbDepartment.SelectedIndex = -1;
            cmbPaymentMethod.SelectedIndex = -1;
            txtAmount.Clear();
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM StudentTbl";

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
                                StudentGV.DataSource = dt; // Populate DataGridView
                            }
                            else
                            {
                                MessageBox.Show("No records found.");
                                StudentGV.DataSource = null;
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (StudentGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            // Get the StudentNum from the selected row
            string selectedStudentId = StudentGV.SelectedRows[0].Cells["StudentId"].Value.ToString();

            // Confirm deletion
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete the record with Student Number {selectedStudentId}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM StudentTbl WHERE StudentId = @StudentId";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            // Add parameter for StudentNum
                            cmd.Parameters.AddWithValue("@StudentId", selectedStudentId);

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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (StudentGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected Alumni ID
            string selectedStudentId = StudentGV.SelectedRows[0].Cells["StudentId"].Value.ToString();

            string queryUpdate = "UPDATE StudentTbl SET " +
                                 "Date = @NewDate, Time = @NewTime, StudentName = @NewStudentName, " +
                                 "Age = @NewAge, PhoneNum = @NewPhoneNum, Email = @NewEmail, " +
                                 "Address = @NewAddress, Course = @NewCourse, Department = @NewDepartment, PaymentMethod = @NewPaymentMethod, " +
                                 "AmountPaid = @NewAmountPaid " +
                                 "WHERE StudentId = @StudentId"; // Add a WHERE clause to target the correct record

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
                        cmd.Parameters.AddWithValue("@NewStudentName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewAge", int.Parse(txtAge.Text));
                        cmd.Parameters.AddWithValue("@NewPhoneNum", txtPhoneNum.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewEmail", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewAddress", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewCourse", txtCourse.Text.Trim());
                        cmd.Parameters.AddWithValue("@NewDepartment", cmbDepartment.Text);
                        cmd.Parameters.AddWithValue("@NewPaymentMethod", cmbPaymentMethod.Text);
                        cmd.Parameters.AddWithValue("@NewAmountPaid", decimal.Parse(txtAmount.Text));

                        // Add the parameter for the AlumniId (condition)
                        cmd.Parameters.AddWithValue("@StudentId", selectedStudentId);

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
                txtCourse.Clear();
                cmbDepartment.SelectedIndex = -1;
                cmbPaymentMethod.SelectedIndex = -1;
                txtAmount.Clear();
            }
        }
    
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Ensure the user has entered a StudentId to search
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Please enter a Student ID to search.");
                return;
            }

            string studentId = txtID.Text; // Get the input StudentId
            string query = "SELECT * FROM StudentTbl WHERE StudentId = @StudentId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add the parameter for StudentId
                        cmd.Parameters.AddWithValue("@StudentId", studentId);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Bind the result to the DataGridView
                        if (dt.Rows.Count > 0)
                        {
                            StudentGV.DataSource = dt; // Display matching data
                        }
                        else
                        {
                            MessageBox.Show("No record found for the given Student ID.");
                            StudentGV.DataSource = null; // Clear the grid if no record is found
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
}










        