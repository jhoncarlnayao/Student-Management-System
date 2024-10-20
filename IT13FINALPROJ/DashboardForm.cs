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

namespace IT13FINALPROJ
{
    public partial class DashboardForm : MaterialForm
    {
        public DashboardForm()
        {
            InitializeComponent();


            MajorPanel.Visible = false; //PANEL FOR COURSES MAJOR
            MinorPanel.Visible = false; //PANEL FOR COURSES MINOR

            CountTotalStudents(); //COUNT TOTAL STUDENTS ADMIN PANEL
            CountTotalEnrolledStudents();//COUNT TOTAL ENROLLED STUDENTS
            CountTotalProfessor();//COUNT TOTAL PROFESSORS

            LoadEnrollmentData();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void CountTotalStudents()
        {
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";
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
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";
            string query = "SELECT COUNT(*) FROM professors_accounts";

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
                        Totalprofessorlabel.Text = totalprofessor.ToString();
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
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";
            string query = "SELECT COUNT(*) FROM enrollments";

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
                        totalenrolledstudentslabel.Text = totalenrolledstudents.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void LoadEnrollmentData()
        {
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";
            string query = "SELECT * FROM enrollments"; // Adjust the query based on the columns you want to retrieve

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable); // Fill the DataTable with the data from the query

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = dataTable;

                    // Optional: Set auto-size for the columns based on the content
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading enrollments: " + ex.Message);
                }
            }
        }

        private void LoadTotalStudentsData()
        {
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";
            string query = "SELECT * FROM students_accounts"; // Adjust the query based on the columns you want to retrieve

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable); // Fill the DataTable with the data from the query

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = dataTable;

                    // Optional: Set auto-size for the columns based on the content
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading enrollments: " + ex.Message);
                }
            }
        }

        private void LoadProfessorsData()
        {
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";
            string query = "SELECT * FROM professors_accounts";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    DataTable dataTable = new DataTable();
                    new MySqlDataAdapter(query, conn).Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dataTable;
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        // THIS IS BUTTON INSIDE OF TABLE
                        var btnColumn = new DataGridViewButtonColumn
                        {
                            HeaderText = "Action",
                            Name = "Action",
                            Text = "Click Me",
                            UseColumnTextForButtonValue = true
                        };
                        dataGridView1.Columns.Add(btnColumn);
                    }
                    else
                    {
                        MessageBox.Show("No professors found in the database.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading professors: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Action"].Index && e.RowIndex >= 0)
            {
                // OPEN NEW FILE THIS IS FOR UPDATE , EDIT , DELETE FUNCTION
               OperaitionAdmin operationadmin = new OperaitionAdmin();  
                operationadmin.Show(); 
                operationadmin.StartPosition = FormStartPosition.CenterParent;
            }
        }


        private void LoadAllProgramsData()
        {
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";
            string query = "SELECT * FROM programs"; // Adjust the query based on the columns you want to retrieve

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable); // Fill the DataTable with the data from the query

                    // Check if data is retrieved
                    if (dataTable.Rows.Count > 0)
                    {
                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Optional: Set auto-size for the columns based on the content
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        private void SearchInDataGridView(string SearchValue)
        {
            DataTable dataTable = new DataTable();

            if (dataTable != null)
            {
                dataTable.DefaultView.RowFilter = string.Format("Fullname LIKE '%{0}%' OR Email LIKE '%{0}%' OR PhoneNumber LIKE '%{0}%'", SearchValue);

                // Optional: Display a message if no results are found
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No matching records found.");
                }
            }
        }


        private void HideEnrollmentData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
        }

        private void HideTotalStudentsData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
        }

        private void HideProfessorsData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
        }

        private void HideAvailableProgramsData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
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
            MajorPanel.Visible = true;
            MinorPanel.Visible = false;

        }

        private void MinorButton_Click(object sender, EventArgs e)
        {
            MajorPanel.Visible = false;
            MinorPanel.Visible = true;
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
            MajorPanel.Visible = true;
            MinorPanel.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";

            string fullname = Studentfullname.Text;
            string email = Studentemail.Text;
            string phonenumber = Studentphonenumber.Text;
            string password = Studentpassword.Text;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO `students_accounts` (Fullname, Email, PhoneNumber, Password) VALUES (@Fullname, @Email, @PhoneNumber, @Password)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Fullname", fullname);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phonenumber);
                        cmd.Parameters.AddWithValue("@Password", password);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Student account created successfully!");

                            CountTotalStudents(); // TO AUTOMATIC RELOAD ARON DINA MAG RESTART SA SYSTEM PAG UPDATE SA TOTAL STUDENTS
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

        private void label74_Click(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";

            string fullname = Professorfullname.Text;
            string email = Professoremail.Text;
            string phonenumber = Professorphonenumber.Text;
            string password = Professorpassword.Text;
            string program = Professorprogram.Text;


            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO `professors_accounts` (Fullname, Email, PhoneNumber, Password, Program) VALUES (@Fullname, @Email, @PhoneNumber, @Password, @Program)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Fullname", fullname);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phonenumber);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Program", program);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Professor account created successfully!");

                            CountTotalProfessor(); // TO AUTOMATIC RELOAD ARON DINA MAG RESTART SA SYSTEM PAG UPDATE SA TOTAL STUDENTS
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

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{

//        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //DESIGN
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;

            LoadTotalStudentsData();
            //HideEnrollmentData();
            //HideAvailableProgramsData();


            Listlabel.Text = "List of Total Students";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadEnrollmentData();
            //  HideTotalStudentsData();
            //HideProfessorsData();
            //HideAvailableProgramsData();
            Listlabel.Text = "List of Student Enrolled";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadProfessorsData();
            // HideEnrollmentData();
            //HideTotalStudentsData();
            //HideAvailableProgramsData();
            Listlabel.Text = "List of Professors";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoadAllProgramsData();
            Listlabel.Text = "List of Available Programs";
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        
            string searchText = textBox2.Text.Trim();

          
            if (dataGridView1.DataSource is DataTable dataTable)
            {
           
                DataView dataView = new DataView(dataTable);

                // Apply filter based on the Fullname column
                // Modify the column name as necessary
                dataView.RowFilter = $"Fullname LIKE '%{searchText}%'";

                // Bind the filtered view back to the DataGridView
                dataGridView1.DataSource = dataView;
            }
            else
            {
                MessageBox.Show("No data available to search.");
            }
        }

        private void MajorButton_Click_1(object sender, EventArgs e)
        {
            MajorPanel.Visible = true;
            MinorPanel.Visible = false;
        }

        private void MinorButton_Click_1(object sender, EventArgs e)
        {
            MajorPanel.Visible = false;
            MinorPanel.Visible = true;
        }
    }
}
