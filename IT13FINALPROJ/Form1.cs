using MaterialSkin;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;

namespace IT13FINALPROJ
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
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
            string username = UsernameText.Text;
            string password = PasswordText.Text;
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";

            // Check if it's a login for Admin
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

                // Check for Student login
                string studentQuery = "SELECT Fullname, Email, PhoneNumber FROM students_accounts WHERE Email=@username AND Password=@password";
                MySqlCommand studentCmd = new MySqlCommand(studentQuery, con);
                studentCmd.Parameters.AddWithValue("@username", username);
                studentCmd.Parameters.AddWithValue("@password", password);

                using (MySqlDataReader reader = studentCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string fullname = reader["Fullname"]?.ToString();
                        string email = reader["Email"]?.ToString();
                        string phonenumber = reader["PhoneNumber"]?.ToString();

                        if (string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phonenumber))
                        {
                            MessageBox.Show("Some student details are missing.", "Log in Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        MessageBox.Show("Student logged in successfully!", "Log in Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        StudentDashboard studentDashboard = new StudentDashboard(fullname, email, phonenumber);
                        studentDashboard.Show();
                        this.Hide();
                        return;
                    }
                }


                // Check for Professor login
                string professorQuery = "SELECT COUNT(1) FROM professors_accounts WHERE Email=@username AND Password=@password";
                MySqlCommand professorCmd = new MySqlCommand(professorQuery, con);
                professorCmd.Parameters.AddWithValue("@username", username);
                professorCmd.Parameters.AddWithValue("@password", password);

                int professorResult = Convert.ToInt32(professorCmd.ExecuteScalar());

                if (professorResult == 1)
                {
                    MessageBox.Show("Professor logged in successfully!", "Log in Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    // ProfessorDashboardForm professorDashboard = new ProfessorDashboardForm();
                    // professorDashboard.Show();
                    this.Hide();
                    return;
                }


                MessageBox.Show("Invalid Username or Password.", "Log in Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label73_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_2(object sender, PaintEventArgs e)
        {

        }


    }
}
