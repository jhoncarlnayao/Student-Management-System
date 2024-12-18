﻿using MySql.Data.MySqlClient;
using System;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IT13FINALPROJ
{
    public partial class StudentForm : Form
    {
        public StudentForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.Text = "";

            this.StartPosition = FormStartPosition.CenterScreen;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Define the border radius
            int borderRadius = 30;  // Change this value for more/less rounding

            // Set the form's region (rounded corners)
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, borderRadius, borderRadius, 180, 90); // Top-left corner
                path.AddArc(this.Width - borderRadius - 1, 0, borderRadius, borderRadius, 270, 90); // Top-right corner
                path.AddArc(this.Width - borderRadius - 1, this.Height - borderRadius - 1, borderRadius, borderRadius, 0, 90); // Bottom-right corner
                path.AddArc(0, this.Height - borderRadius - 1, borderRadius, borderRadius, 90, 90); // Bottom-left corner
                path.CloseAllFigures();

                this.Region = new Region(path);
            }
        }


        public void EnrollStudentAndParent(string studentFirstName, string studentMiddleName, string studentLastName, string sex, DateTime birthdate, string birthplace, string region, string province, string city, string address, string grade,
                                   string parentFirstName, string parentMiddleName, string parentLastName, string phoneNumber, string email)
        {
            string mappedSex = sex == "Male" ? "M" : (sex == "Female" ? "F" : null);

            string connectionString = "server=localhost;user=root;password=;database=it13proj";

            string checkQuery = @"SELECT COUNT(*) FROM (
                            SELECT id FROM students_enroll 
                            WHERE firstname = @studentFirstName 
                              AND (middlename = @studentMiddleName OR (middlename IS NULL AND @studentMiddleName IS NULL)) 
                              AND lastname = @studentLastName
                            UNION
                            SELECT id FROM accepted_students_enroll
                            WHERE firstname = @studentFirstName 
                              AND (middlename = @studentMiddleName OR (middlename IS NULL AND @studentMiddleName IS NULL)) 
                              AND lastname = @studentLastName
                          ) AS combined";

            string studentQuery = "INSERT INTO students_enroll (firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade, parent_fullname) " +
                                  "VALUES (@studentFirstName, @studentMiddleName, @studentLastName, @sex, @birthdate, @birthplace, @region, @province, @city, @address, @grade, @parentFullName)";

            string parentQuery = "INSERT INTO parents (student_id, firstname, middlename, lastname, phonenumber, email) " +
                                 "VALUES (@studentId, @parentFirstName, @parentMiddleName, @parentLastName, @phoneNumber, @parentEmail)";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    // Check for duplicates
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@studentFirstName", studentFirstName);
                        checkCmd.Parameters.AddWithValue("@studentMiddleName", string.IsNullOrEmpty(studentMiddleName) ? (object)DBNull.Value : studentMiddleName);
                        checkCmd.Parameters.AddWithValue("@studentLastName", studentLastName);

                        int duplicateCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (duplicateCount > 0)
                        {
                            MessageBox.Show("A student with the same name already exists. Please check the records.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert student details
                    using (MySqlCommand studentCmd = new MySqlCommand(studentQuery, con))
                    {
                        string parentFullName = $"{parentFirstName} {parentMiddleName} {parentLastName}".Trim();

                        studentCmd.Parameters.AddWithValue("@studentFirstName", studentFirstName);
                        studentCmd.Parameters.AddWithValue("@studentMiddleName", string.IsNullOrEmpty(studentMiddleName) ? (object)DBNull.Value : studentMiddleName);
                        studentCmd.Parameters.AddWithValue("@studentLastName", studentLastName);
                        studentCmd.Parameters.AddWithValue("@sex", mappedSex);
                        studentCmd.Parameters.AddWithValue("@birthdate", birthdate);
                        studentCmd.Parameters.AddWithValue("@birthplace", birthplace);
                        studentCmd.Parameters.AddWithValue("@region", region);
                        studentCmd.Parameters.AddWithValue("@province", province);
                        studentCmd.Parameters.AddWithValue("@city", city);
                        studentCmd.Parameters.AddWithValue("@address", address);
                        studentCmd.Parameters.AddWithValue("@grade", grade);
                        studentCmd.Parameters.AddWithValue("@parentFullName", parentFullName);

                        studentCmd.ExecuteNonQuery();

                        long studentId = studentCmd.LastInsertedId;

                        if (studentId == 0)
                        {
                            MessageBox.Show("Failed to insert student.");
                            return;
                        }

                        // Insert parent details
                        using (MySqlCommand parentCmd = new MySqlCommand(parentQuery, con))
                        {
                            parentCmd.Parameters.AddWithValue("@studentId", studentId);
                            parentCmd.Parameters.AddWithValue("@parentFirstName", parentFirstName);
                            parentCmd.Parameters.AddWithValue("@parentMiddleName", string.IsNullOrEmpty(parentMiddleName) ? (object)DBNull.Value : parentMiddleName);
                            parentCmd.Parameters.AddWithValue("@parentLastName", parentLastName);
                            parentCmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                            parentCmd.Parameters.AddWithValue("@parentEmail", email);

                            parentCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Student and Parent enrolled successfully!");
                        Form1 loginpage = new Form1();
                        loginpage.Show();
                        this.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }




        private void guna2Button1_Click(object sender, EventArgs e)
        {

            string studentFirstName = STfirstname.Text;
            string studentMiddleName = STmiddlename.Text;
            string studentLastName = STlastname.Text;

            string sex = stsex.SelectedItem?.ToString() ?? string.Empty;
            DateTime birthdate = STbirthdate.Value;
            string birthplace = STbirthplace.Text;
            string region = STregion.Text;
            string province = STprovince.Text;
            string city = STcity.Text;
            string address = STaddress.Text;
            string grade = stgrade.SelectedItem?.ToString() ?? string.Empty;

            string parentFirstName = PRfirstname.Text;
            string parentMiddleName = PRmiddlename.Text;
            string parentLastName = PRlastname.Text;
            string phoneNumber = PRphonenumber.Text;
            string email = PRemail.Text;


            if (string.IsNullOrEmpty(sex) || string.IsNullOrEmpty(grade))
            {
                MessageBox.Show("Please select both sex and grade.");
                return;
            }
            else if (string.IsNullOrEmpty(studentFirstName) || string.IsNullOrEmpty(studentLastName))
            {
                MessageBox.Show("Please Fill this in");
                return;
            }


            EnrollStudentAndParent(studentFirstName, studentMiddleName, studentLastName, sex, birthdate, birthplace, region, province, city, address, grade,
                                   parentFirstName, parentMiddleName, parentLastName, phoneNumber, email);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void parentinfo_backbutton_Click(object sender, EventArgs e)
        {
            panel_parentinfo.Visible = false;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            panel_parentinfo.Visible = true;
            guna2HtmlLabel1.Text = "Parent Information";
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}
