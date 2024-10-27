using MaterialSkin;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;

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

                // Check for Student login
                string studentQuery = "SELECT COUNT(1) FROM student_accounts WHERE schoolemail=@username AND password=@password";
                MySqlCommand studentCmd = new MySqlCommand(studentQuery, con);
                studentCmd.Parameters.AddWithValue("@username", username);
                studentCmd.Parameters.AddWithValue("@password", password);

                int studentResult = Convert.ToInt32(studentCmd.ExecuteScalar());

                if (studentResult == 1)
                {
                    MessageBox.Show("Student logged in successfully!", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    StudentDashboard studentDashboard = new StudentDashboard();
                    studentDashboard.Show();
                    this.Hide();
                    return;
                }

                // Check for Teacher login
                string teacherQuery = "SELECT COUNT(1) FROM teacher_account WHERE Username=@username AND Password_Hash=@password";
                MySqlCommand teacherCmd = new MySqlCommand(teacherQuery, con);
                teacherCmd.Parameters.AddWithValue("@username", username);
                teacherCmd.Parameters.AddWithValue("@password", password);

                int teacherResult = Convert.ToInt32(teacherCmd.ExecuteScalar());

                if (teacherResult == 1)
                {
                    MessageBox.Show("Teacher logged in successfully!", "Login Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 //   TeacherDashboard teacherDashboard = new TeacherDashboard();
                  //  teacherDashboard.Show();
                    this.Hide();
                    return;
                }

                // If none of the account checks passed, show error message
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

