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
            LoadStudentData();
            LoadGenderData();
            CountSex();
            CountTotalPendingStudents();
            CountTotalStudents();
            CountTotalTeachers();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.StartPosition = FormStartPosition.CenterScreen;
            //createStudentAccountButton.Click += (s, e) => CreateStudentAccount();

            //FOR AUTOMATIC TIMER
            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            timer1.Start();

            createdby.Enabled = false;


        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            automatictime.Text = DateTime.Now.ToString("hh:mm:ss tt").ToUpper();
            automaticdate.Text = DateTime.Now.ToString("MMMM dd, yyyy");

        }

        private void LoadGenderData()
        {
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Count male and female students
                    string query = "SELECT sex, COUNT(*) as count FROM accepted_students_enroll GROUP BY sex";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    int maleCount = 0;
                    int femaleCount = 0;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string sex = row["sex"].ToString();
                        int count = Convert.ToInt32(row["count"]);

                        if (sex == "M")
                        {
                            maleCount = count;
                        }
                        else if (sex == "F")
                        {
                            femaleCount = count;
                        }
                    }

                    // Calculate total and percentages
                    int total = maleCount + femaleCount;
                    if (total > 0)
                    {
                        float malePercentage = (maleCount / (float)total) * 100;
                        float femalePercentage = (femaleCount / (float)total) * 100;

                        // Set male progress bar
                        guna2CircleProgressBar1.Value = (int)malePercentage; // Male percentage
                        guna2CircleProgressBar1.ProgressColor = System.Drawing.Color.Blue; // Solid color for males
                        guna2CircleProgressBar1.ProgressColor2 = System.Drawing.Color.Blue; // Same color to avoid gradient

                        // Set female progress bar
                        guna2CircleProgressBar2.Value = (int)femalePercentage; // Female percentage
                        guna2CircleProgressBar2.ProgressColor = System.Drawing.Color.Pink; // Solid color for females
                        guna2CircleProgressBar2.ProgressColor2 = System.Drawing.Color.Pink; // Same color to avoid gradient
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        public void CountSex()
        {
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            int maleCount = 0;
            int femaleCount = 0;

            string query = @"
        SELECT 
            SUM(CASE WHEN sex = 'M' THEN 1 ELSE 0 END) AS MaleCount,
            SUM(CASE WHEN sex = 'F' THEN 1 ELSE 0 END) AS FemaleCount
        FROM accepted_students_enroll";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Retrieve the counts
                                maleCount = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                femaleCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            // Update labels with counts, make sure labels are initialized
            boysnumber.Text = maleCount.ToString();
            girlsnumber.Text = femaleCount.ToString();
        }

        //TOTAL STUDENTS
        public void CountTotalStudents()
        {
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            int totalCount = 0;

            string query = "SELECT COUNT(*) FROM accepted_students_enroll";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    totalCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            totalstudents.Text = totalCount.ToString();
        }


        //TOTAL TEACHERS
        public void CountTotalTeachers()
        {
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            int totalCount = 0;

            string query = "SELECT COUNT(*) FROM teacher_account";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    totalCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            totalteachers.Text = totalCount.ToString();
        }


        //TOTAL PENDING STUDENTS
        public void CountTotalPendingStudents()
        {
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            int totalCount = 0;

            string query = "SELECT COUNT(*) FROM students_enroll";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    totalCount = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            totalpendingstudents.Text = totalCount.ToString();
        }

        public void InsertAnnouncement(string title, string description)
        {
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            string query = "INSERT INTO Announcements (Title, Description, CreatedBy) VALUES (@Title, @Description, @CreatedBy)";
            string createdBy = "Admin";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@CreatedBy", createdBy);


                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Announcement sent successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to send announcement.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }






        private void LoadPendingStudents()
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
            {
                con.Open();
                string query = "SELECT student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade FROM students_enroll";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Add Status and Action columns
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("Action", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    row["Status"] = "Pending";
                    row["Action"] = "Accept";
                }

                guna2DataGridView1.DataSource = dt;


                foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }


                guna2DataGridView1.ColumnHeadersHeight = 30;
                guna2DataGridView1.RowTemplate.Height = 25;


                guna2DataGridView1.ColumnHeadersVisible = true;
            }
        }

        private void AcceptStudent(int studentId)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
            {
                con.Open();

                string moveQuery = "INSERT INTO accepted_students_enroll (student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade) " +
                                   "SELECT student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade " +
                                   "FROM students_enroll WHERE student_id = @studentId;";

                MySqlCommand cmd = new MySqlCommand(moveQuery, con);
                cmd.Parameters.AddWithValue("@studentId", studentId);
                cmd.ExecuteNonQuery();


                string deleteParentsQuery = "DELETE FROM parents WHERE student_id = @studentId;";
                MySqlCommand deleteParentsCmd = new MySqlCommand(deleteParentsQuery, con);
                deleteParentsCmd.Parameters.AddWithValue("@studentId", studentId);
                deleteParentsCmd.ExecuteNonQuery();


                string deleteQuery = "DELETE FROM students_enroll WHERE student_id = @studentId;";
                MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, con);
                deleteCmd.Parameters.AddWithValue("@studentId", studentId);
                deleteCmd.ExecuteNonQuery();

                MessageBox.Show("Student accepted successfully!");
            }
        }
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Action")
                {

                    int studentId = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["student_id"].Value);


                    AcceptStudent(studentId);


                    LoadPendingStudents();
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








        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.AutoGenerateColumns = true;
            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;
            CountSex();
            CountTotalPendingStudents();
            CountTotalStudents();
            CountTotalTeachers();
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


        private void guna2Button8_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {

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

        private void guna2Button14_Click_1(object sender, EventArgs e)
        {

        }

        private void guna2Button16_Click(object sender, EventArgs e)
        {
            selecttabletoview.Visible = !selecttabletoview.Visible;

        }

        private void guna2Button17_Click(object sender, EventArgs e)
        {
            viewactions.Visible = !viewactions.Visible;
        }

        private void guna2Button18_Click(object sender, EventArgs e)
        {
            //STUDENT ACCOUNTS LIST
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";


            string query = "SELECT id, firstname, middlename, lastname, sex, schoolemail FROM student_accounts";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the data to the recordedlistingstable DataGridView
                    recordlistingstable.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void guna2Button19_Click(object sender, EventArgs e)
        {
            //TEACHER ACCOUNT

            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";

            // Query to retrieve guidance staff records
            string query = "SELECT TeacherID, SubjectID, Firstname, Lastname, Middlename, Phonenumber, Address, Email, Username, Sex, PreferredGradeLevel FROM teacher_account";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    recordlistingstable.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void guna2Button20_Click(object sender, EventArgs e)
        {
            //GUIDANCE ACCOUNT
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";

            // Query to retrieve guidance staff records
            string query = "SELECT staff_id, username, firstname, middlename, lastname, email, phone_number FROM guidance_staff";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);


                    recordlistingstable.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

        }

        private void guna2Button21_Click(object sender, EventArgs e)
        {
            //PARENTS
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            string query = "SELECT parent_id, student_id, firstname, middlename, lastname, phonenumber, email FROM parents";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);


                    recordlistingstable.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void guna2Button22_Click(object sender, EventArgs e)
        {
            //PENDING STUDENTS
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            string query = "SELECT student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade FROM students_enroll";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);


                    recordlistingstable.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

        }

        private void guna2Button23_Click(object sender, EventArgs e)
        {
            //ACCEPTED STUDENTS LIST
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            string query = "SELECT student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade, parent_fullname FROM accepted_students_enroll";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);


                    recordlistingstable.DataSource = dataTable;

                    recordlistingstable.CellFormatting += recordlistingstable_CellFormatting;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void guna2Button15_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (result == DialogResult.Yes)
            {

                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.ShowDialog();
                this.Close();
            }
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (result == DialogResult.Yes)
            {

                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.ShowDialog();
                this.Close();
            }
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (result == DialogResult.Yes)
            {

                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.ShowDialog();
                this.Close();
            }
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (result == DialogResult.Yes)
            {

                this.Hide();
                Form1 loginForm = new Form1();
                loginForm.ShowDialog();
                this.Close();
            }
        }

        private void guna2Button39_Click(object sender, EventArgs e)
        {
            LoadPendingStudents();
            guna2DataGridView2.Visible = false;
            guna2DataGridView1.Visible = true;
        }

        private void guna2Button38_Click(object sender, EventArgs e)
        {
            guna2DataGridView2.Visible = true;
            guna2DataGridView1.Visible = false;

            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM accepted_students_enroll";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    guna2DataGridView2.DataSource = dataTable;
                    guna2DataGridView2.CellFormatting += guna2DataGridView2_CellFormatting;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void guna2DataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (guna2DataGridView2.Columns[e.ColumnIndex].Name == "grade" && e.Value != null)
            {
                string grade = e.Value.ToString();

                switch (grade)
                {
                    case "Grade 1":
                        e.CellStyle.BackColor = Color.LightGreen;
                        break;
                    case "Grade 2":
                        e.CellStyle.BackColor = Color.Yellow;
                        break;
                    case "Grade 3":
                        e.CellStyle.BackColor = Color.Pink;
                        break;
                    case "Grade 4":
                        e.CellStyle.BackColor = Color.LightBlue;
                        break;
                    case "Grade 5":
                        e.CellStyle.BackColor = Color.LightYellow;
                        break;
                    case "Grade 6":
                        e.CellStyle.BackColor = Color.Orange;
                        break;
                    default:
                        e.CellStyle.BackColor = Color.White;
                        break;
                }
            }
        }
        private void recordlistingstable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (recordlistingstable.Columns[e.ColumnIndex].Name == "grade" && e.Value != null)
            {
                string grade = e.Value.ToString();

                switch (grade)
                {
                    case "Grade 1":
                        e.CellStyle.BackColor = Color.LightGreen;
                        break;
                    case "Grade 2":
                        e.CellStyle.BackColor = Color.Yellow;
                        break;
                    case "Grade 3":
                        e.CellStyle.BackColor = Color.Pink;
                        break;
                    case "Grade 4":
                        e.CellStyle.BackColor = Color.LightBlue;
                        break;
                    case "Grade 5":
                        e.CellStyle.BackColor = Color.LightYellow;
                        break;
                    case "Grade 6":
                        e.CellStyle.BackColor = Color.Orange;
                        break;
                    default:
                        e.CellStyle.BackColor = Color.White;
                        break;
                }
            }
        }

        private void recordlistingstable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2CircleProgressBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2CircleProgressBar2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void automatictime_Click(object sender, EventArgs e)
        {

        }

        private void totalteachers_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            string titlee = title.Text;
            string descriptionn = description.Text;


            InsertAnnouncement(titlee, descriptionn);
        }
    }
}
