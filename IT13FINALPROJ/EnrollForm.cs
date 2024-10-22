using Guna.UI2.WinForms;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT13FINALPROJ
{
    public partial class EnrollForm : MaterialForm
    {
        public EnrollForm()
        {
            InitializeComponent();
        }

        private void EnrollForm_Load(object sender, EventArgs e)
        {
            // nextinfo.Visible = false;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //  this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void nextbutton_Click(object sender, EventArgs e)
        {
            //  nextinfo.Visible = true;
        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UsernameText_TextChanged(object sender, EventArgs e)
        {
            //Firsname
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            //Middlename
        }

        private void Lastname_TextChanged(object sender, EventArgs e)
        {
            //Lastname
        }

        private void Phonenumber_TextChanged(object sender, EventArgs e)
        {
            //Phonenumber
        }

        private void Address_TextChanged(object sender, EventArgs e)
        {
            //Address
        }

        private void Email_TextChanged(object sender, EventArgs e)
        {
            //Email
        }

        private void Sex_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Sex
        }

        private void Program_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Program
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //Button enrollnow

            string firstname = Firstname.Text;
            string middlename = Middlename.Text; // This can be null or empty
            string lastname = Lastname.Text;
            string phonenumber = Phonenumber.Text;
            string address = Address.Text;
            string email = Email.Text;
            string sex = Sex.SelectedItem.ToString();
            string program = Program.SelectedItem.ToString();


            InsertStudentEnrollee(firstname, middlename, lastname, phonenumber, address, email, sex, program);
        }

        private void enroll_Click(object sender, EventArgs e)
        {
            //Button enrollnow

            string firstname = Firstname.Text;
            string middlename = Middlename.Text; // This can be null or empty
            string lastname = Lastname.Text;
            string phonenumber = Phonenumber.Text;
            string address = Address.Text;
            string email = Email.Text;
            string sex = Sex.SelectedItem.ToString();
            string program = Program.SelectedItem.ToString();


            InsertStudentEnrollee(firstname, middlename, lastname, phonenumber, address, email, sex, program);
        }

        private void InsertStudentEnrollee(string firstname, string middlename, string lastname, string phonenumber, string address, string email, string sex, string program)
        {
            string connectionString = "server=localhost;user=root;database=it13proj;password=";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO student_enrollees (Firstname, Middlename, Lastname, Phonenumber, Address, Email, Sex, Program) 
                                     VALUES (@firstname, @middlename, @lastname, @phonenumber, @address, @email, @sex, @program)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstname", firstname);
                        cmd.Parameters.AddWithValue("@middlename", string.IsNullOrEmpty(middlename) ? (object)DBNull.Value : middlename); // Handle null/empty middle name
                        cmd.Parameters.AddWithValue("@lastname", lastname);
                        cmd.Parameters.AddWithValue("@phonenumber", phonenumber);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@sex", sex);
                        cmd.Parameters.AddWithValue("@program", program);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Student enrollment successful.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

      
    }
}
    
