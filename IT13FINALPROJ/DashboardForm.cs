﻿using MaterialSkin;
using MaterialSkin.Controls;
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
using MySql.Data.MySqlClient;
using System.Buffers;

namespace IT13FINALPROJ
{
    public partial class DashboardForm : MaterialForm
    {
        public DashboardForm()
        {
            InitializeComponent();


         

            CountTotalStudents(); //COUNT TOTAL STUDENTS ADMIN PANEL
            CountTotalEnrolledStudents();//COUNT TOTAL ENROLLED STUDENTS
            CountTotalProfessor();//COUNT TOTAL PROFESSORS

            LoadEnrollmentData();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //   this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void CountTotalStudents()
        {
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";
            string query = "SELECT COUNT(*) FROM students_accounts";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Execute the query and get the result (the total count of students)
                        int totalStudents = Convert.ToInt32(cmd.ExecuteScalar());

                        // Display the total number of students in the label
                        Totalstudentlabel.Text = totalStudents.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void CountTotalProfessor()
        {
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";
            string query = "SELECT COUNT(*) FROM student_enrollees";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Execute the query and get the result (the total count of students)
                        int totalprofessor = Convert.ToInt32(cmd.ExecuteScalar());

                        // Display the total number of students in the label
                        pendingenrollment.Text = totalprofessor.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void CountTotalEnrolledStudents()
        {
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";
            string query = "SELECT COUNT(*) FROM accepted_students";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Execute the query and get the result (the total count of students)
                        int totalenrolledstudents = Convert.ToInt32(cmd.ExecuteScalar());

                        // Display the total number of students in the label
                        totalenrolled.Text = totalenrolledstudents.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void LoadEnrollmentData()
        {
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";
            string query = "SELECT * FROM student_enrollees";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable); // Fill the DataTable with the data from the query

                    // Add a Status column with default value "Pending"
                    dataTable.Columns.Add("Status", typeof(string));
                    foreach (DataRow row in dataTable.Rows)
                    {
                        row["Status"] = "Pending";
                    }

                    guna2DataGridView1.DataSource = dataTable;

                    // Customize DataGridView appearance
                    guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    guna2DataGridView1.Columns["Status"].DisplayIndex = 0; // Put Status as the first column

                    // Add Accept button column
                    DataGridViewButtonColumn acceptButton = new DataGridViewButtonColumn();
                    acceptButton.Name = "Accept";
                    acceptButton.HeaderText = "Action";
                    acceptButton.Text = "Accept";
                    acceptButton.UseColumnTextForButtonValue = true;
                    guna2DataGridView1.Columns.Add(acceptButton);

                    guna2DataGridView1.CellClick += guna2DataGridView1_CellClick;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading enrollments: " + ex.Message);
                }
            }
        }

        // Handle the Accept button click event
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the click is from the button column
            if (e.ColumnIndex == guna2DataGridView1.Columns["Accept"].Index && e.RowIndex >= 0)
            {
                int studentId = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                AcceptEnrollment(studentId);

                // Change status after acceptance
                guna2DataGridView1.Rows[e.RowIndex].Cells["Status"].Value = "Accepted";
                guna2DataGridView1.Rows[e.RowIndex].Cells["Status"].Style.BackColor = Color.Green;
            }
        }

        // Move the student to accepted_students and delete from student_enrollees
        private void AcceptEnrollment(int studentId)
        {
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";
            string insertQuery = @"INSERT INTO accepted_students (Firstname, Middlename, Lastname, Phonenumber, Address, Email, Sex, Program, Enrollment_date)
                           SELECT Firstname, Middlename, Lastname, Phonenumber, Address, Email, Sex, Program, Enrollment_date
                           FROM student_enrollees WHERE id = @id;
                           DELETE FROM student_enrollees WHERE id = @id;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@id", studentId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Student ID " + studentId + " enrollment accepted and moved to accepted_students.");
        }

        private void LoadAcceptedEnrollmentData()
        {
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";
            string query = "SELECT * FROM accepted_students";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);


                    if (dataTable.Rows.Count > 0)
                    {

                        guna2DataGridView1.DataSource = dataTable;


                        guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                    else
                    {
                        MessageBox.Show("No professors found in the database.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading professors: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void materialCard1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(173, 216, 230));
        }

        private void materialCard2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(0, 255, 0));
        }

        private void materialCard3_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.FromArgb(255, 182, 193));
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void MajorButton_Click(object sender, EventArgs e)
        {
         
        }

        private void MinorButton_Click(object sender, EventArgs e)
        {
          
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void MinorPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MajorPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";

            string firstname = Studentfirstname.Text;
            string middlename = Studentmiddlename.Text;
            string lastname = Studentlastname.Text;
            string email = Studentemail.Text;
            string phonenumber = Studentphonenumber.Text;
            string address = Studentaddress.Text;
            string password = Studentpassword.Text;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO `student_accounts` (Firstname, Middlename, Lastname, Phonenumber, Email, Address, Password) VALUES (@Firstname, @Middlename, @Lastname, @Phonenumber, @Email, @Address, @Password)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Firstname", firstname);
                        cmd.Parameters.AddWithValue("@Middlename", middlename);
                        cmd.Parameters.AddWithValue("@Lastname", lastname);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phonenumber);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Password", password);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Student account created successfully!");

                            CountTotalStudents(); // TO AUTOMATIC RELOAD ARON DINA MAG RESTART SA SYSTEM PAG UPDATE SA TOTAL STUDENTS
                        }
                        else
                        {
                            MessageBox.Show("Failed to create student account.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void label74_Click(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";

            string fullname = Professorfullname.Text;
            string email = Professoremail.Text;
            string phonenumber = Professorphonenumber.Text;
            string password = Professorpassword.Text;
            string program = Professorprogram.Text;


            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO `professors_accounts` (Fullname, Email, PhoneNumber, Password, Program) VALUES (@Fullname, @Email, @PhoneNumber, @Password, @Program)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Fullname", fullname);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phonenumber);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Program", program);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Professor account created successfully!");

                            CountTotalProfessor(); // TO AUTOMATIC RELOAD ARON DINA MAG RESTART SA SYSTEM PAG UPDATE SA TOTAL STUDENTS
                        }
                        else
                        {
                            MessageBox.Show("Failed to create Professor account.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void Professorfullname_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            int CourseTabIndex = 2;
            materialTabControl1.SelectedIndex = CourseTabIndex;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            CountTotalEnrolledStudents();
        }


        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
         
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadEnrollmentData();

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void MajorButton_Click_1(object sender, EventArgs e)
        {
         
         
        }

        private void MinorButton_Click_1(object sender, EventArgs e)
        {
         
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            LoadEnrollmentData();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            LoadAcceptedEnrollmentData();
        }

        private void totalenrolled_Click(object sender, EventArgs e)
        {

        }
    }
}
