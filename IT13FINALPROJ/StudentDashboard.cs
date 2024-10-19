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
    public partial class StudentDashboard : MaterialForm
    {
        private string studentFullName;
        private string fullname;
        private string email;
        private string phonenumber;
        public StudentDashboard(string fullname, string email, string phonenumber)
        {
            InitializeComponent();
            studentFullName = fullname;
            this.fullname = fullname;
            this.email = email;
            this.phonenumber = phonenumber;


            MajorPanel.Visible = false; //PANEL FOR PROGRAM
            MinorPanel.Visible = false; //PANEL FOR PROGRAM

        }



        private void StudentDashboard_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            string studentId = "5" + random.Next(1000000, 9999999).ToString();
            StudentIDtext.Text = studentId;

            Studentfullnametext.Text = fullname;
            Studentemailtext.Text = email;
            Studentphonenumbertext.Text = phonenumber;

            StudentIDtext.Enabled = false;
            Studentfullnametext.Enabled = false;
            Studentemailtext.Enabled = false;
            Studentphonenumbertext.Enabled = false;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label56_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            // this is for the fullname
            Fullnamelabel.Text = studentFullName;
        }

        private void Home_Click(object sender, EventArgs e)
        {

        }

        private void label74_Click(object sender, EventArgs e)
        {

        }

        private void Professorprogram_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Enroll_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            int EnrollTab = 1;
            materialTabControl1.SelectedIndex = EnrollTab;
        }

        private void EnrollButton_Click(object sender, EventArgs e)
        {
            // ENROLL BUTTON
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";
            string studentID = StudentIDtext.Text;
            string selectedProgram = Studentprogram.Text;
            string fullname = Studentfullnametext.Text;

            if (string.IsNullOrEmpty(selectedProgram))
            {
                MessageBox.Show("Please select a program to enroll in.", "Enrollment Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                // Check if the student is already enrolled in any program
                string checkEnrollmentQuery = "SELECT Program FROM enrollments WHERE StudentID=@studentID";
                MySqlCommand checkCmd = new MySqlCommand(checkEnrollmentQuery, con);
                checkCmd.Parameters.AddWithValue("@studentID", studentID);

                string enrolledProgram = checkCmd.ExecuteScalar() as string;

                if (enrolledProgram != null)
                {
                    // If the student is already enrolled in a different program, notify them
                    if (enrolledProgram.Equals(selectedProgram, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("You are already enrolled in this program.", "Enrollment Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                // Proceed with enrollment
                string enrollQuery = "INSERT INTO enrollments (StudentID, Fullname, Program) VALUES (@studentID, @fullname, @program)";
                MySqlCommand enrollCmd = new MySqlCommand(enrollQuery, con);
                enrollCmd.Parameters.AddWithValue("@studentID", studentID);
                enrollCmd.Parameters.AddWithValue("@fullname", fullname);
                enrollCmd.Parameters.AddWithValue("@program", selectedProgram);

                try
                {
                    enrollCmd.ExecuteNonQuery();
                    MessageBox.Show("Enrollment successful!", "Enrollment Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Enrollment failed: {ex.Message}", "Enrollment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Programtab = 2;
            materialTabControl1.SelectedIndex = Programtab;
        }

        private void Program_Click(object sender, EventArgs e)
        {

        }

        private void MajorButton_Click(object sender, EventArgs e)
        {
            MajorPanel.Visible = true;
            MinorPanel.Visible = false;
        }

        private void MinorButton_Click(object sender, EventArgs e)
        {
            MajorPanel.Visible = false;
            MinorPanel.Visible = true;
        }
    }
}

