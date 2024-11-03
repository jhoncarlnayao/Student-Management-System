using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT13FINALPROJ
{
    public partial class GuidanceDashboard : MaterialForm
    {
        public GuidanceDashboard()
        {
            InitializeComponent();
            CountSex();
            gradelevel.Items.Add("1");
            gradelevel.Items.Add("2");
            gradelevel.Items.Add("3");
            gradelevel.Items.Add("4");
            gradelevel.Items.Add("5");
            gradelevel.Items.Add("6");
            this.gradelevel.SelectedIndexChanged += new System.EventHandler(this.gradeLevelComboBox_SelectedIndexChanged);
            createdby.Enabled = false;


        }

        //FUNCTION AREA:
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
            string createdBy = "Guidance Staff";
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

        private void LoadTeachersByGrade(int gradeLevel)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
                {
                    con.Open();
                    // Updated query to match teacher's PreferredGradeLevel
                    string query = "SELECT DISTINCT CONCAT(Firstname, ' ', IFNULL(Middlename, ''), ' ', Lastname) AS teacher_name " +
                                   "FROM teacher_account " +
                                   "WHERE PreferredGradeLevel = @gradeLevel";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@gradeLevel", gradeLevel.ToString());

                    MySqlDataReader reader = cmd.ExecuteReader();
                    assignteacher.Items.Clear();

                    if (!reader.HasRows)
                    {
                        MessageBox.Show("No teachers found for the selected grade level.");
                    }
                    else
                    {
                        // Populate the assignteacher ComboBox with teacher names
                        while (reader.Read())
                        {
                            assignteacher.Items.Add(reader["teacher_name"].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void LoadStudentsByGrade(string gradeLevel)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
                {
                    con.Open();
                    string query = "SELECT firstname, middlename, lastname FROM accepted_students_enroll WHERE grade = @gradeLevel";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@gradeLevel", gradeLevel);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    studentname2.Items.Clear();

                    while (reader.Read())
                    {
                        string fullName = $"{reader["firstname"]} {reader["middlename"]} {reader["lastname"]}";
                        studentname2.Items.Add(fullName);
                    }

                    if (studentname2.Items.Count == 0)
                    {
                        MessageBox.Show("No students found for the selected grade level.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void LoadSectionsByGrade(string gradeLevel)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
                {
                    con.Open();
                    string query = "SELECT DISTINCT section_name FROM sections WHERE grade_level = @gradeLevel";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@gradeLevel", gradeLevel);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    availablesection2.Items.Clear();

                    while (reader.Read())
                    {
                        availablesection2.Items.Add(reader["section_name"].ToString());
                    }

                    if (availablesection2.Items.Count == 0)
                    {
                        MessageBox.Show("No sections found for the selected grade level.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }





        private void gradeLevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gradelevel.SelectedItem != null)
            {
                // Ensure the selected item is not null and can be parsed to an integer
                if (int.TryParse(gradelevel.SelectedItem.ToString(), out int selectedGrade))
                {
                    LoadTeachersByGrade(selectedGrade);
                }
                else
                {
                    MessageBox.Show("Please select a valid grade level.");
                }
            }
        }




        private void CreateNewTable(string tableName, string gradeLevel, string sectionName, string assignedTeacher, string academicYear, string roomNumber, int studentCapacity, string description)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
                {
                    con.Open();


                    string createTableQuery = $"CREATE TABLE IF NOT EXISTS `{tableName}` (" +
                                               "id INT AUTO_INCREMENT PRIMARY KEY, " +
                                               "grade_level VARCHAR(255) NOT NULL, " +
                                               "section_name VARCHAR(255) NOT NULL, " +
                                               "Fullname VARCHAR(255), " +
                                               "academic_year VARCHAR(255), " +
                                               "room_number VARCHAR(255), " +
                                               "student_capacity INT, " +
                                               "description TEXT, " +
                                               "role VARCHAR(255) DEFAULT 'Teacher')";

                    MySqlCommand cmd = new MySqlCommand(createTableQuery, con);
                    cmd.ExecuteNonQuery();


                    string insertDataQuery = $"INSERT INTO `{tableName}` (grade_level, section_name, Fullname, academic_year, room_number, student_capacity, description, role) " +
                                              $"VALUES (@gradeLevel, @sectionName, @assignedTeacher, @academicYear, @roomNumber, @studentCapacity, @description, 'Teacher')";

                    MySqlCommand insertCmd = new MySqlCommand(insertDataQuery, con);
                    insertCmd.Parameters.AddWithValue("@gradeLevel", gradeLevel);
                    insertCmd.Parameters.AddWithValue("@sectionName", sectionName);
                    insertCmd.Parameters.AddWithValue("@assignedTeacher", assignedTeacher);
                    insertCmd.Parameters.AddWithValue("@academicYear", academicYear);
                    insertCmd.Parameters.AddWithValue("@roomNumber", roomNumber);
                    insertCmd.Parameters.AddWithValue("@studentCapacity", studentCapacity);
                    insertCmd.Parameters.AddWithValue("@description", description);
                    insertCmd.ExecuteNonQuery();

                    MessageBox.Show("New table created and data with role 'Teacher' inserted successfully!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
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


        //END OF FUNCTION AREA

        private void GuidanceDashboard_Load(object sender, EventArgs e)
        {

            guna2DataGridView1.AutoGenerateColumns = true;
            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;
            CountTotalPendingStudents();
            CountTotalStudents();
            CountTotalTeachers();
            CountSex();
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string tableName = nameoftable.Text;
            string gradeLevel = gradelevel.SelectedItem.ToString();
            string sectionName = sectionname.Text;
            string assignedTeacher = assignteacher.SelectedItem?.ToString();
            string academicYear = academicyear.Text;
            string roomNumber = roomnumber.Text;
            int studentCapacity = Convert.ToInt32(studentcapacity.Text);
            string descriptionn = description.Text;

            CreateNewTable(tableName, gradeLevel, sectionName, assignedTeacher, academicYear, roomNumber, studentCapacity, descriptionn);
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button16_Click(object sender, EventArgs e)
        {
            // selecttabletoview.Visible = !selecttabletoview.Visible;
            gradeandsection.Visible = true;
            studentgradesection.Visible = false;
        }

        private void guna2Button17_Click(object sender, EventArgs e)
        {
            studentgradesection.Visible = !studentgradesection.Visible;
            //    gradeandsection.Visible = !gradeandsection.Visible;
            studentgradesection.Visible = true;
            gradeandsection.Visible = false;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Make sure grade and section are selected
            if (gradelevel2.SelectedItem == null || availablesection2.SelectedItem == null || studentname2.SelectedItem == null)
            {
                MessageBox.Show("Please select a grade level, section, and student.");
                return;
            }

            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            string selectedSection = availablesection2.SelectedItem.ToString();
            string selectedStudent = studentname2.SelectedItem.ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Insert the selected student into the chosen section table with role as "Student"
                string insertQuery = "INSERT INTO " + selectedSection + " (Fullname, grade_level, section_name, role) " +
                                     "VALUES (@Fullname, @GradeLevel, @SectionName, 'Student')";

                MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                insertCmd.Parameters.AddWithValue("@Fullname", selectedStudent);
                insertCmd.Parameters.AddWithValue("@GradeLevel", gradelevel2.SelectedItem.ToString());
                insertCmd.Parameters.AddWithValue("@SectionName", selectedSection);

                try
                {
                    insertCmd.ExecuteNonQuery();
                    MessageBox.Show("Student assigned successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error assigning student: " + ex.Message);
                }
            }
        }


        private void gradelevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            string selectedGrade = gradelevel2.SelectedItem.ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Step 1: Retrieve tables starting with "Grade"
                string tableQuery = "SHOW TABLES LIKE 'Grade%'";
                MySqlCommand tableCmd = new MySqlCommand(tableQuery, connection);
                MySqlDataReader tableReader = tableCmd.ExecuteReader();

                availablesection2.Items.Clear();

                while (tableReader.Read())
                {
                    availablesection2.Items.Add(tableReader[0].ToString());
                }

                tableReader.Close();

                // Step 2: Automatically input teacher name for the grade level
                string teacherQuery = "SELECT CONCAT(Firstname, ' ', IFNULL(Middlename, ''), ' ', Lastname) AS teacher_name " +
                                      "FROM teacher_account " +
                                      "WHERE PreferredGradeLevel = @gradeLevel";

                MySqlCommand teacherCmd = new MySqlCommand(teacherQuery, connection);
                teacherCmd.Parameters.AddWithValue("@gradeLevel", selectedGrade);

                object teacherName = teacherCmd.ExecuteScalar();
                assignedteacher2.Text = teacherName != null ? teacherName.ToString() : "No teacher assigned";

                // Step 3: Display all students in the selected grade level in studentname2
                string studentQuery = "SELECT CONCAT(firstname, ' ', IFNULL(middlename, ''), ' ', lastname) AS student_name " +
                                      "FROM accepted_students_enroll " +
                                      "WHERE grade = @grade";

                MySqlCommand studentCmd = new MySqlCommand(studentQuery, connection);
                studentCmd.Parameters.AddWithValue("@grade", selectedGrade);

                MySqlDataReader studentReader = studentCmd.ExecuteReader();
                studentname2.Items.Clear();

                while (studentReader.Read())
                {
                    studentname2.Items.Add(studentReader["student_name"].ToString());
                }

                studentReader.Close();
            }
        }


        private void studentname2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void availablesection2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=it13proj;User=root;Password=;";
            string selectedSection = availablesection2.SelectedItem.ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Query to get the assigned teacher for the selected section
                string teacherQuery = "SELECT Fullname " +
                                      "FROM " + selectedSection + " " +
                                      "WHERE role = 'Teacher' " +
                                      "LIMIT 1";  // Assuming only one teacher is assigned per section

                MySqlCommand teacherCmd = new MySqlCommand(teacherQuery, connection);
                object teacherName = teacherCmd.ExecuteScalar();

                assignedteacher2.Text = teacherName != null ? teacherName.ToString() : "No teacher assigned";
            }
        }

        private void pendingenrollment_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button39_Click(object sender, EventArgs e)
        {
            LoadPendingStudents();
        }

        private void guna2Panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            string titlee = title.Text;
            string description = descriptionannounce.Text;


            InsertAnnouncement(titlee, description);
        }

        private void createdby_TextChanged(object sender, EventArgs e)
        {
            createdby.Enabled = false;
        }

        private void guna2NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
