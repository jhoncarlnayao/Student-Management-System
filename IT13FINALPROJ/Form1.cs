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

          
            string adminQuery = "SELECT COUNT(1) FROM Admins WHERE Username=@username AND Password=@password";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                // Check for Admin login
                MySqlCommand adminCmd = new MySqlCommand(adminQuery, con);
                adminCmd.Parameters.AddWithValue("@username", username);
                adminCmd.Parameters.AddWithValue("@password", password);

                int adminResult = Convert.ToInt32(adminCmd.ExecuteScalar());

                if (adminResult == 1)
                {
                    MessageBox.Show("Admin logged in successfully!", "Log in Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DashboardForm dashboard = new DashboardForm();
                    dashboard.Show();
                    this.Hide();
                    return;
                }

                // Check for GuidanceStaff login
                string guidancequery = "SELECT COUNT(1) FROM guidance_staff WHERE email=@username AND password_hash=@password";
                MySqlCommand guidanceCmd = new MySqlCommand(guidancequery, con);
                guidanceCmd.Parameters.AddWithValue("@username", username);
                guidanceCmd.Parameters.AddWithValue("@password", password);

                int guidanceResult = Convert.ToInt32(guidanceCmd.ExecuteScalar());

                if (guidanceResult == 1)
                {
                    MessageBox.Show("Guidance Staff logged in successfully!", "Log in Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    GuidanceDashboard guidancedashboard = new GuidanceDashboard();
                    guidancedashboard.Show();
                    this.Hide();
                    return;
                }

                // Check for Student  login
                string studentquery = "SELECT COUNT(1) FROM student_accounts WHERE schoolemail=@username AND password=@password";
                MySqlCommand studentcmd = new MySqlCommand(studentquery, con);
                studentcmd.Parameters.AddWithValue("@username", username);
                studentcmd.Parameters.AddWithValue("@password", password);

                int studentresult = Convert.ToInt32(studentcmd.ExecuteScalar());

                if (studentresult == 1)
                {
                    MessageBox.Show("Student Account logged in successfully!", "Log in Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);


                   // GuidanceDashboard guidancedashboard = new GuidanceDashboard();
                  // guidancedashboard.Show();
                   // this.Hide();
                   // return;
                }


                // Check for Teacher login
                string teacherquery = "SELECT COUNT(1) FROM teacher_account WHERE Username=@username AND Password_Hash=@password";
                MySqlCommand teachercmd = new MySqlCommand(teacherquery, con);
                teachercmd.Parameters.AddWithValue("@username", username);
                teachercmd.Parameters.AddWithValue("@password", password);

                int teacherresult = Convert.ToInt32(teachercmd.ExecuteScalar());

                if (teacherresult == 1)
                {
                    MessageBox.Show("Teacher Account logged in successfully!", "Log in Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    // GuidanceDashboard guidancedashboard = new GuidanceDashboard();
                    // guidancedashboard.Show();
                    // this.Hide();
                    // return;
                }


                MessageBox.Show("Invalid Username or Password.", "Log in Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

