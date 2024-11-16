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
                    return;
                }

                string studentQuery = "SELECT COUNT(1) FROM student_accounts WHERE schoolemail=@username AND password=@password";
                MySqlCommand studentCmd = new MySqlCommand(studentQuery, con);
                studentCmd.Parameters.AddWithValue("@username", username);
                studentCmd.Parameters.AddWithValue("@password", password);

                int studentResult = Convert.ToInt32(studentCmd.ExecuteScalar());

                if (studentResult == 1)
                {
                    MessageBox.Show("Student logged in successfully!", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Retrieve the student's details
                    string detailsQuery = "SELECT firstname, middlename, lastname, schoolemail FROM student_accounts WHERE schoolemail=@username";
                    MySqlCommand detailsCmd = new MySqlCommand(detailsQuery, con);
                    detailsCmd.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = detailsCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve name and email
                            string fullName = $"{reader["firstname"]} {reader["middlename"]} {reader["lastname"]}";
                            string schoolEmail = reader["schoolemail"].ToString();

                            // Open the StudentDashboard and pass the retrieved data
                            StudentDashboard studentDashboard = new StudentDashboard();
                            studentDashboard.StudentName = fullName;
                            studentDashboard.StudentEmail = schoolEmail;
                            studentDashboard.Show();
                            this.Hide();
                        }
                    }
                    return;
                }

                // Check for Teacher login
                // Check for Teacher login
                string teacherQuery = "SELECT * FROM teacher_account WHERE Username=@username AND Password_Hash=@password";
                MySqlCommand teacherCmd = new MySqlCommand(teacherQuery, con);
                teacherCmd.Parameters.AddWithValue("@username", username);
                teacherCmd.Parameters.AddWithValue("@password", password);

                using (MySqlDataReader reader = teacherCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        TeacherDashboard teacherDashboard = new TeacherDashboard(
                            reader["firstname"].ToString(),
                            reader["middlename"].ToString(),
                            reader["lastname"].ToString(),
                            reader["address"].ToString(),
                            reader["email"].ToString(),
                            reader["phonenumber"].ToString(),
                            reader["sex"].ToString(),
                            username,
                            password,
                            reader["PreferredGradeLevel"].ToString(),
                            reader["SubjectID"].ToString()
                        );
                        teacherDashboard.Show();
                        this.Hide();
                        return;
                    }
                }

                // If no role matched, show invalid login message
                MessageBox.Show("Invalid Username or Password.", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
    }
}

