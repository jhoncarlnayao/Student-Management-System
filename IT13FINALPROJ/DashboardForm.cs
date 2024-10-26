using MaterialSkin;
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
using Guna.UI2.WinForms;

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

       

            LoadStudentData();
        
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //   this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            createStudentAccountButton.Click += (s, e) => CreateStudentAccount();
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

        private void LoadStudentData()
        {
            string query = "SELECT student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade, parent_fullname FROM accepted_students_enroll";
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind data to the DataGridView
                    studenttable.DataSource = dataTable;
                    studenttable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    studenttable.MultiSelect = false; // Only one row can be selected
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void CreateStudentAccount()
        {
            if (studenttable.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = studenttable.SelectedRows[0];

                string firstname = selectedRow.Cells["firstname"].Value.ToString();
                string middlename = selectedRow.Cells["middlename"].Value?.ToString();
                string lastname = selectedRow.Cells["lastname"].Value.ToString();
                string sex = selectedRow.Cells["sex"].Value.ToString();

                string adminSchoolEmail = studentemail.Text;
                string adminPassword = studentpassword.Text;

                if (string.IsNullOrEmpty(adminSchoolEmail) || string.IsNullOrEmpty(adminPassword))
                {
                    MessageBox.Show("Please enter admin school email and password.");
                    return;
                }

                string connectionString = "server=localhost;database=it13proj;user=root;password=;";
                string insertQuery = "INSERT INTO student_accounts (firstname, middlename, lastname, sex, schoolemail, password) VALUES (@firstname, @middlename, @lastname, @sex, @schoolemail, @password)";


                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@firstname", firstname);
                            cmd.Parameters.AddWithValue("@middlename", middlename ?? (object)DBNull.Value); // Handle null value
                            cmd.Parameters.AddWithValue("@lastname", lastname);
                            cmd.Parameters.AddWithValue("@sex", sex);
                            cmd.Parameters.AddWithValue("@schoolemail", adminSchoolEmail);
                            cmd.Parameters.AddWithValue("@password", adminPassword); // Ensure hashing in production

                            int rows = cmd.ExecuteNonQuery();
                            if (rows > 0)
                            {
                                MessageBox.Show("Student account created successfully!");
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
            else
            {
                MessageBox.Show("Please select a student.");
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


        private void label74_Click(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

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
      
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            LoadAcceptedEnrollmentData();
        }

        private void totalenrolled_Click(object sender, EventArgs e)
        {

        }

        private void CreateaccountBTN_Click(object sender, EventArgs e)
        {
            //CREATE GUIDANCE STAFF'S ACCOUNT
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";
            string firstname = GTfirstname.Text;
            string middlename = GTmiddlename.Text;
            string lastname = GTlastname.Text;
            string email = GTemail.Text;
            string phonenumber = GTphonenumber.Text;
            string schoolemail = GTschoolemail.Text;
            string password = GTpassword.Text;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO `guidance_staff` (username, password_hash, firstname, middlename, lastname, email, phone_number) VALUES (@username, @password_hash, @firstname, @middlename, @lastname, @email, @phone_number)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", schoolemail);
                        cmd.Parameters.AddWithValue("@password_hash", password);
                        cmd.Parameters.AddWithValue("@firstname", firstname);
                        cmd.Parameters.AddWithValue("@middlename", middlename); // corrected parameter name
                        cmd.Parameters.AddWithValue("@lastname", lastname);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phone_number", phonenumber);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Guidance staff account created successfully!");
                            // CountTotalProfessor(); 
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

        private void guna2HtmlLabel19_Click(object sender, EventArgs e)
        {

        }

        private void TeacherAccountBTN_Click(object sender, EventArgs e)
        {
            //CREATE TEACHER ACCOUNT
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";

            string firstname = Tfirstname.Text;
            string middlename = Tmiddlaname.Text;
            string lastname = Tlastname.Text;
            string email = Temail.Text;
            string phonenumber = Tphonenumber.Text;
            string schoolemail = Tschoolemail.Text;
            string address = Taddress.Text;
            string password = Tpassword.Text; // Make sure to hash the password in a real implementation
            string sex = Tsex.SelectedItem?.ToString() ?? string.Empty;
            string gradelevel = Tgradelevel.SelectedItem?.ToString() ?? string.Empty;
            string subjectID = "1";  // Assuming a value, this should be from the input

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO `teacher_account` (Firstname, Lastname, Middlename, Phonenumber, Address, Email, Username, Password_Hash, Sex, PreferredGradeLevel, SubjectID) " +
                                   "VALUES (@firstname, @lastname, @middlename, @phonenumber, @address, @email, @username, @password_hash, @sex, @gradelevel, @subjectID)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstname", firstname);
                        cmd.Parameters.AddWithValue("@lastname", lastname);
                        cmd.Parameters.AddWithValue("@middlename", middlename); // Corrected
                        cmd.Parameters.AddWithValue("@phonenumber", phonenumber); // Corrected
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@email", email); // Added email field to match the table
                        cmd.Parameters.AddWithValue("@username", schoolemail);
                        cmd.Parameters.AddWithValue("@password_hash", password); // Ensure this is hashed
                        cmd.Parameters.AddWithValue("@sex", sex);
                        cmd.Parameters.AddWithValue("@gradelevel", gradelevel);
                        cmd.Parameters.AddWithValue("@subjectID", subjectID); // Added SubjectID

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Teacher account created successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to create teacher account.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

      

        private void guna2Button14_Click(object sender, EventArgs e)
        {

        }

        private void studenttable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

           
        }
    }
}
