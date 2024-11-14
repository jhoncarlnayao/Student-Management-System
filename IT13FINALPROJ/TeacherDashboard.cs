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
        private string teacherUsername;

        public TeacherDashboard(string username)
        {
            InitializeComponent();
            teacherUsername = username;
        }


        private void TeacherDashboard_Load(object sender, EventArgs e)
        {
            LoadStudents();
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
    }
}
