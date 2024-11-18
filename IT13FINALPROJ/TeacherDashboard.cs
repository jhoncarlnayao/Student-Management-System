using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IT13FINALPROJ
{
    public partial class TeacherDashboard : MaterialForm
    {
        private string teacherFullName;
        private string teacherAddress;
        private string teacherEmail;
        private string teacherPhoneNumber;
        private string teacherSex;
        private string teacherUsername;
        private string teacherPassword;
        private string preferredGradeLevel;
        private string subjectID;
        private string id;

        public TeacherDashboard(
            string username,
            string firstname,
            string middlename,
            string lastname,
            string address,
            string email,
            string phonenumber,
            string sex,
            string id,  // id is passed from the database
            string password,
            string preferredGradeLevel,
            string subjectID)
        { 
            InitializeComponent();
            teacherFullName = $"{firstname} {middlename} {lastname}";
            teacherAddress = address;
            teacherEmail = email;
            teacherPhoneNumber = phonenumber;
            teacherSex = sex;
            teacherUsername = username;
            teacherPassword = password;
            this.preferredGradeLevel = $"Grade{preferredGradeLevel}";
            this.subjectID = subjectID;
            this.id = id;  
                           
        }

        private void TeacherDashboard_Load(object sender, EventArgs e)
        {
            LoadStudents();

            //! LOAD THE TEACHER ACCOUNT INFORMATION TO BE VIEWED IN PROFILE
            teacher_fullname.Text = teacherFullName;
            teacher_address.Text = teacherAddress;
            teacher_email.Text = teacherEmail;
            teacher_phonenumber.Text = teacherPhoneNumber;
            teacher_sex.Text = teacherSex == "M" ? "Male" : "Female";
            teacher_username.Text = teacherUsername;
            teacher_password.Text = teacherPassword;
            teacher_gradelevel.Text = preferredGradeLevel;
            // lblSubjectID.Text = subjectID;
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

        private void LoadStudents()
        {
            string connectionString = "server=localhost;user=root;password=;database=it13proj";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string cleanedTeacherUsername = teacherUsername.Trim();
                Console.WriteLine($"Cleaned Teacher Username: '{cleanedTeacherUsername}'");

                string sectionQuery = "SELECT grade_level, section_name FROM teacher_assignments " +
                                      "WHERE TRIM(LOWER(teacher_username)) = TRIM(LOWER(@teacherName))";

                using (MySqlCommand sectionCmd = new MySqlCommand(sectionQuery, con))
                {
                    sectionCmd.Parameters.AddWithValue("@teacherName", cleanedTeacherUsername);

                    // Log the actual query being executed for debugging
                    Console.WriteLine($"Executing query: {sectionQuery} with parameter: {cleanedTeacherUsername}");

                    using (var reader = sectionCmd.ExecuteReader())
                    {
                        if (reader.Read()) // If data is returned for the teacher
                        {
                            string gradeLevel = reader["grade_level"].ToString();
                            string sectionName = reader["section_name"].ToString();

                            // Log the grade level and section name
                            Console.WriteLine($"Grade Level: {gradeLevel}, Section: {sectionName}");

                            // Use the section name directly as the table name
                            string tableName = $"{sectionName}";  // Just use the section name as the table name

                            // Query to load students and subjects for this dynamically constructed section (using the section name)
                            string loadStudentsQuery = $"SELECT Fullname, Mathematics, Science, English, Filipino, Araling_Panlipunan, Edukasyon_sa_Pagpapakatao AS ESP, MAPEH " +
                                                       $"FROM `{tableName}`"; // Dynamically load students based on the section name

                            // Log the query being executed to load students
                            Console.WriteLine($"Executing student query for section: {tableName}");

                            // Use a new connection to prevent conflict with the DataReader
                            using (MySqlConnection studentConnection = new MySqlConnection(connectionString))
                            {
                                studentConnection.Open();
                                using (MySqlDataAdapter adapter = new MySqlDataAdapter(loadStudentsQuery, studentConnection))
                                {
                                    DataTable dataTable = new DataTable();
                                    adapter.Fill(dataTable); // Fill the DataTable with student data

                                    if (dataTable.Rows.Count > 0)
                                    {
                                        // Bind DataTable to DataGridView
                                        guna2DataGridView1.DataSource = dataTable;
                                        Console.WriteLine($"Loaded {dataTable.Rows.Count} students.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("No students found in the section.");
                                        MessageBox.Show("No students found in this section.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Show a message if no section is assigned to the teacher
                            Console.WriteLine("No section assigned to this teacher.");
                            MessageBox.Show("No section assigned to this teacher.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Handle any additional label click event here if needed
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell content click events in the DataGridView if needed
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void guna2HtmlLabel43_Click(object sender, EventArgs e)
        {
        }

        private void teacher_updatebutton_Click(object sender, EventArgs e) //! UPDATE NEW USERNAME AND PASSWORD FOR TEACHER PROFILE
        {
            string newUsername = teacher_username.Text;
            string newPassword = teacher_password.Text;

            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Username and Password cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "server=localhost;database=it13proj;user=root;password=;";
            string updateQuery = "UPDATE teacher_account SET Username=@username, Password_Hash=@password WHERE id=@id";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", newUsername);
                        cmd.Parameters.AddWithValue("@password", newPassword);
                        cmd.Parameters.AddWithValue("@id", id);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Username and Password updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to update. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
