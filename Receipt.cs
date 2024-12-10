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
    public partial class Receipt : Form
    {
        public Receipt()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainForm main = new MainForm();
            main.Show();
            this.Hide();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.Document = printDocument1; // Link to PrintDocument
            previewDialog.ShowDialog();
        
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Define fonts
            Font titleFont = new Font("Georgia", 20, FontStyle.Bold);
            Font labelFont = new Font("Georgia", 14, FontStyle.Regular);
            Font contentFont = new Font("Georgia", 14, FontStyle.Regular);
            Font footerFont = new Font("Georgia", 18, FontStyle.Bold);

            float yPosition = 50; // Starting vertical position

            // Calculate the center of the page (horizontal)
            float pageCenter = e.PageBounds.Width / 2;

            // Center-align format
            StringFormat centerAlign = new StringFormat()
            {
                Alignment = StringAlignment.Center, // Horizontal alignment
                LineAlignment = StringAlignment.Near // Top-aligned vertically
            };

            yPosition = 80; // Adjust position below logos
            e.Graphics.DrawString("ACKNOWLEDGMENT RECEIPT", titleFont, Brushes.Black, pageCenter, yPosition, centerAlign);
            yPosition += 100;

            // Draw Date (on the Right Side)
            yPosition += 20;
            e.Graphics.DrawString(
                $"Date: {dtpDate.Value.ToString("dd MMMM , yyyy")}",
                labelFont,
                Brushes.Black,
                500, // Right-aligned X-coordinate
                yPosition
            );

            // Starting position
            yPosition += 80;

            // Draw the introductory text
            e.Graphics.DrawString("This is to certify that Mr./Ms.", labelFont, Brushes.Black, 50, yPosition);

            // Draw Name and Amount on the same line
            e.Graphics.DrawString(txtName.Text, contentFont, Brushes.Black, 308, yPosition - 1); // Name input
            e.Graphics.DrawLine(Pens.Black, 300, yPosition + 20, 540, yPosition + 20); // Line for Name

            // Place "The Amount of Ten Pesos..." on the same line
            e.Graphics.DrawString("The Amount of Ten Pesos.", labelFont, Brushes.Black, 550, yPosition - 1);

            yPosition += 40; // Move down to the next section

            // Add "as payment for GYM Maintenance Fee."
            e.Graphics.DrawString("(php 10.00) as payment for GYM Maintenance Fee.", labelFont, Brushes.Black, 50, yPosition);

            // Adjust position for the note
            yPosition += 150; // Smaller gap between text and the note

            // Bold the word "Note:" and keep the rest regular
            e.Graphics.DrawString(
                "Note:",                                     // Bold text
                new Font("Georgia", 18, FontStyle.Bold),     // Font: Bold, Size 18
                Brushes.Black,                               // Color
                150,                                          // X-coordinate for alignment
                yPosition
            );

            e.Graphics.DrawString(
                " Please show this to the gym receptionist.", // Regular text
                new Font("Georgia", 18, FontStyle.Regular),   // Font: Regular, Size 18
                Brushes.Black,                                // Color
                220,                                         // X-coordinate for alignment (right after "Note:")
                yPosition
            );


            yPosition += 400; // Add space below the name
            e.Graphics.DrawString(
                "Thank You!",                 // Position
                new Font("Georgia", 18, FontStyle.Bold), // Font: Regular, Size 11
                Brushes.Black,                         // Color
                510,                                   // X-coordinate for alignment
                yPosition
            );
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            BuyTicket buyTicket= new BuyTicket();
            buyTicket.Show();
            this.Hide();
        }
    }
 }

