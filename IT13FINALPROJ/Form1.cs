using MaterialSkin;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;

namespace IT13FINALPROJ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.Text = "";

            PasswordText.PasswordChar = '●'; // Set to a bullet or other masking character by default


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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void PasswordText_TextChanged(object sender, EventArgs e)
        {
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void materialMaskedTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void materialLabel1_Click(object sender, EventArgs e)
        {

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {

        }



        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void label73_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void UsernameText_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string username = UsernameText.Text;
            string password = PasswordText.Text;
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";
            bool isLoggedIn = false; // Track login success

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                // Check for Admin login
                string adminQuery = "SELECT COUNT(1) FROM Admins WHERE Username=@username AND Password=@password";
                MySqlCommand adminCmd = new MySqlCommand(adminQuery, con);
                adminCmd.Parameters.AddWithValue("@username", username);
                adminCmd.Parameters.AddWithValue("@password", password);

                int adminResult = Convert.ToInt32(adminCmd.ExecuteScalar());

                if (adminResult == 1)
                {
                    MessageBox.Show("Admin logged in successfully!", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DashboardForm dashboard = new DashboardForm();
                    dashboard.Show();
                    this.Hide();
                    isLoggedIn = true;
                    return;
                }

                // Check for GuidanceStaff login
                string guidanceQuery = "SELECT COUNT(1) FROM guidance_staff WHERE email=@username AND password_hash=@password";
                MySqlCommand guidanceCmd = new MySqlCommand(guidanceQuery, con);
                guidanceCmd.Parameters.AddWithValue("@username", username);
                guidanceCmd.Parameters.AddWithValue("@password", password);

                int guidanceResult = Convert.ToInt32(guidanceCmd.ExecuteScalar());

                if (guidanceResult == 1)
                {
                    MessageBox.Show("Guidance Staff logged in successfully!", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GuidanceDashboard guidanceDashboard = new GuidanceDashboard();
                    guidanceDashboard.Show();
                    this.Hide();
                    isLoggedIn = true;
                    return;
                }

                // Check for Student login
                string studentQuery = "SELECT is_disabled FROM student_accounts WHERE schoolemail=@username AND password=@password";
                MySqlCommand studentCmd = new MySqlCommand(studentQuery, con);
                studentCmd.Parameters.AddWithValue("@username", username);
                studentCmd.Parameters.AddWithValue("@password", password);

                object studentResult = studentCmd.ExecuteScalar();

                if (studentResult != null)
                {
                    bool isDisabled = Convert.ToBoolean(studentResult);
                    if (isDisabled)
                    {
                        MessageBox.Show("Your account is disabled. Please contact the administrator.", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // If not disabled, proceed with fetching student details
                    MessageBox.Show("Student logged in successfully!", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Fetch additional details for the dashboard
                    string detailsQuery = @"
                SELECT 
                    sa.firstname AS account_firstname, 
                    sa.middlename AS account_middlename, 
                    sa.lastname AS account_lastname, 
                    sa.schoolemail,
                    se.id AS student_id, 
                    se.grade, 
                    se.sex, 
                    se.birthdate, 
                    se.birthplace, 
                    se.region, 
                    se.province, 
                    se.city, 
                    se.address 
                FROM 
                    student_accounts sa
                LEFT JOIN 
                    accepted_students_enroll se 
                ON 
                    sa.firstname = se.firstname AND 
                    (sa.middlename = se.middlename OR (sa.middlename IS NULL AND se.middlename IS NULL)) AND 
                    sa.lastname = se.lastname
                WHERE 
                    sa.schoolemail=@username";

                    MySqlCommand detailsCmd = new MySqlCommand(detailsQuery, con);
                    detailsCmd.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = detailsCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Extract data from the reader
                            string fullName = $"{reader["account_firstname"]} {reader["account_middlename"] ?? ""} {reader["account_lastname"]}".Trim();
                            string schoolEmail = reader["schoolemail"].ToString();
                            int studentId = reader["student_id"] != DBNull.Value ? Convert.ToInt32(reader["student_id"]) : 0;

                            string grade = reader["grade"]?.ToString() ?? "N/A";
                            string sex = reader["sex"]?.ToString() ?? "N/A";

                            // Format birthdate as "Month day, year" (e.g., "November 28, 2024")
                            string birthdate = reader["birthdate"] != DBNull.Value
                                ? Convert.ToDateTime(reader["birthdate"]).ToString("MMMM dd, yyyy")
                                : "N/A";

                            string birthplace = reader["birthplace"]?.ToString() ?? "N/A";
                            string region = reader["region"]?.ToString() ?? "N/A";
                            string province = reader["province"]?.ToString() ?? "N/A";
                            string city = reader["city"]?.ToString() ?? "N/A";
                            string address = reader["address"]?.ToString() ?? "N/A";

                            // Pass the details to the dashboard
                            StudentDashboard studentDashboard = new StudentDashboard
                            {
                                StudentName = fullName,
                                StudentEmail = schoolEmail,
                                StudentId = studentId,
                                StudentGrade = grade,
                                StudentSex = sex,
                                StudentBirthdate = birthdate, // Display the formatted birthdate
                                StudentBirthplace = birthplace,
                                StudentRegion = region,
                                StudentProvince = province,
                                StudentCity = city,
                                StudentAddress = address
                            };

                            studentDashboard.Show();
                            this.Hide();
                            isLoggedIn = true;
                            return; // Exit after successful student login
                        }
                    }
                }

                // Check for Teacher login
                string teacherQuery = "SELECT is_disabled FROM teacher_account WHERE Username=@username AND Password_Hash=@password";
                MySqlCommand teacherCmd = new MySqlCommand(teacherQuery, con);
                teacherCmd.Parameters.AddWithValue("@username", username);
                teacherCmd.Parameters.AddWithValue("@password", password);

                object teacherResult = teacherCmd.ExecuteScalar();

                if (teacherResult != null)
                {
                    bool isDisabled = Convert.ToBoolean(teacherResult);
                    if (isDisabled)
                    {
                        MessageBox.Show("Your account is disabled. Please contact the administrator.", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Fetch additional details and load TeacherDashboard (unchanged code)
                }

                // If no valid login, show error
                if (!isLoggedIn)
                {
                    MessageBox.Show("Invalid Username or Password.", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }






        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            StudentForm enrollform = new StudentForm();
            enrollform.Show();
            this.Hide();
            return;
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            {
                PasswordText.PasswordChar = '\0'; 
            }
            else
            {
                PasswordText.PasswordChar = '●'; 
            }
        }


    }
}

