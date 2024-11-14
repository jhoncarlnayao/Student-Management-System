using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        //FUNCTION AREA:
        private void LoadPendingStudents()
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
            {
                con.Open();

                // Ensure 'id' is the alias for the primary identifier in the DataGridView
                string query = "SELECT id AS student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade " +
                               "FROM students_enroll " +
                               "WHERE id NOT IN (SELECT student_id FROM accepted_students_enroll);";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Bind the data to the DataGridView
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




        private bool AcceptStudent(int id)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
            {
                con.Open();
                MySqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Disable foreign key checks
                    MySqlCommand disableFKCheck = new MySqlCommand("SET FOREIGN_KEY_CHECKS=0;", con, transaction);
                    disableFKCheck.ExecuteNonQuery();

                    // Move student data to accepted_students_enroll
                    string moveQuery = "INSERT INTO accepted_students_enroll (student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade, parent_fullname) " +
                                       "SELECT id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade, parent_fullname " +
                                       "FROM students_enroll WHERE id = @id;";

                    MySqlCommand cmdMove = new MySqlCommand(moveQuery, con, transaction);
                    cmdMove.Parameters.AddWithValue("@id", id);
                    int rowsInserted = cmdMove.ExecuteNonQuery();

                    // Delete from parents table
                    string deleteParentsQuery = "DELETE FROM parents WHERE student_id = @id;";
                    MySqlCommand cmdDeleteParents = new MySqlCommand(deleteParentsQuery, con, transaction);
                    cmdDeleteParents.Parameters.AddWithValue("@id", id);
                    cmdDeleteParents.ExecuteNonQuery();

                    // Delete student from students_enroll
                    string deleteQuery = "DELETE FROM students_enroll WHERE id = @id;";
                    MySqlCommand cmdDelete = new MySqlCommand(deleteQuery, con, transaction);
                    cmdDelete.Parameters.AddWithValue("@id", id);
                    int rowsDeleted = cmdDelete.ExecuteNonQuery();

                    // Re-enable foreign key checks
                    MySqlCommand enableFKCheck = new MySqlCommand("SET FOREIGN_KEY_CHECKS=1;", con, transaction);
                    enableFKCheck.ExecuteNonQuery();

                    // Check if insert and delete affected the expected rows
                    if (rowsInserted == 1 && rowsDeleted == 1)
                    {
                        transaction.Commit();
                        MessageBox.Show("Student accepted successfully!");
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        MessageBox.Show("Failed to accept student. Transaction rolled back.");
                        return false;
                    }
                }
                catch (MySqlException ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Database error: " + ex.Message);
                    return false;
                }
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

        private void LoadStudentsByGrade(int gradeLevel)
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


        private void LoadSectionsByGrade(int gradeLevel)
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





        private void CreateNewTable(string tableName, string gradeLevel, string sectionName, string assignedTeacher, string teacherUsername, string academicYear, string roomNumber, int studentCapacity, string description)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
                {
                    con.Open();

                    // Create the grade/section table
                    string createTableQuery = $"CREATE TABLE IF NOT EXISTS `{tableName}` (" +
                                      "id INT AUTO_INCREMENT PRIMARY KEY, " +
                                      "grade_level VARCHAR(255) NOT NULL, " +
                                      "section_name VARCHAR(255) NOT NULL, " +
                                      "Fullname VARCHAR(255), " +
                                      "academic_year VARCHAR(255), " +
                                      "room_number VARCHAR(255), " +
                                      "student_capacity INT, " +
                                      "description TEXT, " +
                                      "role VARCHAR(255) DEFAULT 'Teacher', " +
                                      "Mathematics FLOAT NULL, " +
                                      "Science FLOAT NULL, " +
                                      "English FLOAT NULL, " +
                                      "Filipino FLOAT NULL, " +
                                      "Araling_Panlipunan FLOAT NULL, " +
                                      "Edukasyon_sa_Pagpapakatao FLOAT NULL, " +
                                      "MAPEH FLOAT NULL)";
                    MySqlCommand cmd = new MySqlCommand(createTableQuery, con);
                    cmd.ExecuteNonQuery();

                    // Insert the teacher into the grade/section table
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

                    // Insert the teacher assignment into teacher_assignments table with username
                    string insertAssignmentQuery = "INSERT INTO teacher_assignments (teacher_username, grade_level, section_name) " +
                                                  "VALUES (@teacherUsername, @gradeLevel, @sectionName)";
                    MySqlCommand insertAssignmentCmd = new MySqlCommand(insertAssignmentQuery, con);
                    insertAssignmentCmd.Parameters.AddWithValue("@teacherUsername", teacherUsername); // Use the username
                    insertAssignmentCmd.Parameters.AddWithValue("@gradeLevel", gradeLevel);
                    insertAssignmentCmd.Parameters.AddWithValue("@sectionName", tableName);  // Table name as section name
                    insertAssignmentCmd.ExecuteNonQuery();

                    MessageBox.Show("New table created, data with role 'Teacher' inserted successfully, and teacher assigned!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private string GetTeacherUsername(string fullName)
        {
            string teacherUsername = string.Empty;

            try
            {
                MessageBox.Show($"Fetching username for: {fullName}");


                string[] nameParts = fullName.Split(' ');

                if (nameParts.Length >= 2) // Ensure there's at least a Firstname and Lastname
                {
                    string firstName = nameParts[0];
                    string lastName = nameParts[nameParts.Length - 1];

                    // Check for middle name/initial if present
                    string middleName = nameParts.Length == 3 ? nameParts[1] : string.Empty;

                    using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
                    {
                        con.Open();
                        // Query to fetch the username based on the Firstname and Lastname
                        string query = "SELECT Username FROM teacher_account WHERE Firstname = @firstName AND Lastname = @lastName LIMIT 1";

                        // Pass the split name parts for better accuracy
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@firstName", firstName);
                        cmd.Parameters.AddWithValue("@lastName", lastName);

                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            teacherUsername = reader["Username"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No matching username found!"); // If no matching username is found
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Full Name format is invalid!"); // If name doesn't have enough parts
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error retrieving teacher's username: {ex.Message}");
            }

            return teacherUsername;
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
            // Ensure that the click happened in a valid row and column
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Action")
                {
                    // Check if the value in the student_id cell is DBNull
                    if (guna2DataGridView1.Rows[e.RowIndex].Cells["student_id"].Value != DBNull.Value)
                    {
                        int studentId = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["id"].Value);

                        AcceptStudent(studentId);
                        LoadPendingStudents();
                    }
                    else
                    {
                        MessageBox.Show("Student ID is missing or invalid.");
                    }
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
            string assignedTeacherFullName = assignteacher.SelectedItem?.ToString(); // Assuming this is the full name
            string academicYear = academicyear.Text;
            string roomNumber = roomnumber.Text;
            int studentCapacity = Convert.ToInt32(studentcapacity.Text);
            string descriptionn = description.Text;  // Make sure description is correctly captured

            // Retrieve the username of the assigned teacher
            string teacherUsername = GetTeacherUsername(assignedTeacherFullName);

            // Pass the username to CreateNewTable
            CreateNewTable(tableName, gradeLevel, sectionName, assignedTeacherFullName, teacherUsername, academicYear, roomNumber, studentCapacity, descriptionn);
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

            // Extract the numeric part of the grade level (e.g., "Grade 1" -> "1")
            string gradeText = selectedGrade.Replace("Grade", "").Trim();

            // Attempt to parse the grade number
            int gradeLevel;
            if (!int.TryParse(gradeText, out gradeLevel))
            {
                MessageBox.Show("Invalid grade level selected.");
                return;
            }

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
                teacherCmd.Parameters.AddWithValue("@gradeLevel", gradeLevel);  // Pass as an integer

                object teacherName = teacherCmd.ExecuteScalar();
                assignedteacher2.Text = teacherName != null ? teacherName.ToString() : "No teacher assigned";

                // Step 3: Display all students in the selected grade level in studentname2
                string studentQuery = "SELECT CONCAT(firstname, ' ', IFNULL(middlename, ''), ' ', lastname) AS student_name " +
                                      "FROM accepted_students_enroll " +
                                      "WHERE grade = @grade";  // Assuming 'grade' is an integer column

                MySqlCommand studentCmd = new MySqlCommand(studentQuery, connection);
                studentCmd.Parameters.AddWithValue("@grade", gradeLevel);  // Pass as an integer

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

        private void guna2Button38_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                // Use the correct column name as per LoadPendingStudents() (e.g., student_id)
                int studentId = Convert.ToInt32(guna2DataGridView1.SelectedRows[0].Cells["student_id"].Value);

                // Attempt to accept the student
                if (AcceptStudent(studentId))
                {
                    LoadPendingStudents();
                }
                else
                {
                    MessageBox.Show("Failed to accept student. Please try again.");
                }
            }
            else
            {
                MessageBox.Show("Please select a student to accept.");
            }
        }

        private void guna2Panel7_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
