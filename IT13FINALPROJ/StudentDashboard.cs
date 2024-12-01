using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace IT13FINALPROJ
{
    public partial class StudentDashboard : MaterialForm
    {
        // Properties to store student data
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public int StudentId { get; set; }
        public string StudentGrade { get; set; }
        public string StudentSex { get; set; }
        public string StudentBirthdate { get; set; }
        public string StudentBirthplace { get; set; }
        public string StudentRegion { get; set; }
        public string StudentProvince { get; set; }
        public string StudentCity { get; set; }
        public string StudentAddress { get; set; }

        public StudentDashboard()
        {
            InitializeComponent();
        }

        private void StudentDashboard_Load(object sender, EventArgs e)
        {
            // Debug message to check if properties are passed correctly
        //    MessageBox.Show($"StudentName: {StudentName}, StudentId: {StudentId}, StudentGrade: {StudentGrade}");

            // Display data in the dashboard, handling null values
            student_fullname.Text = string.IsNullOrEmpty(StudentName) ? "N/A" : StudentName;
            student_id.Text = StudentId > 0 ? StudentId.ToString() : "N/A";
            student_grade.Text = string.IsNullOrEmpty(StudentGrade) ? "N/A" : StudentGrade;
            student_sex.Text = string.IsNullOrEmpty(StudentSex) ? "N/A" : StudentSex;
            student_birthdate.Text = string.IsNullOrEmpty(StudentBirthdate) ? "N/A" : StudentBirthdate;
            student_birthplace.Text = string.IsNullOrEmpty(StudentBirthplace) ? "N/A" : StudentBirthplace;
            student_region.Text = string.IsNullOrEmpty(StudentRegion) ? "N/A" : StudentRegion;
            student_province.Text = string.IsNullOrEmpty(StudentProvince) ? "N/A" : StudentProvince;
            student_city.Text = string.IsNullOrEmpty(StudentCity) ? "N/A" : StudentCity;
            student_address.Text = string.IsNullOrEmpty(StudentAddress) ? "N/A" : StudentAddress;

            LoadStudentGrades();
        }

        private void LoadStudentGrades()
        {
            string connectionString = "server=localhost;database=it13proj;user=root;password=;";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Step 1: Get the section name for the student
                    string sectionQuery = "SELECT section_name FROM student_assignments WHERE student_name = @studentName";
                    MySqlCommand sectionCmd = new MySqlCommand(sectionQuery, con);
                    sectionCmd.Parameters.AddWithValue("@studentName", StudentName);

                    string sectionName = sectionCmd.ExecuteScalar()?.ToString();

                    if (string.IsNullOrEmpty(sectionName))
                    {
                        MessageBox.Show("Student is not assigned to any section.", "No Section Assigned", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Step 2: Get the grades for the student in the identified section
                    string gradesQuery = @"
                SELECT 
                    Mathematics, Science, English, Filipino, 
                    Araling_Panlipunan, Edukasyon_sa_Pagpapakatao, MAPEH 
                FROM grade_6_testing 
                WHERE Fullname = @studentName AND section_name = @sectionName";

                    MySqlCommand gradesCmd = new MySqlCommand(gradesQuery, con);
                    gradesCmd.Parameters.AddWithValue("@studentName", StudentName);
                    gradesCmd.Parameters.AddWithValue("@sectionName", sectionName);

                    using (MySqlDataReader reader = gradesCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Parse grades
                            int math = reader["Mathematics"] != DBNull.Value ? Convert.ToInt32(reader["Mathematics"]) : 0;
                            int science = reader["Science"] != DBNull.Value ? Convert.ToInt32(reader["Science"]) : 0;
                            int english = reader["English"] != DBNull.Value ? Convert.ToInt32(reader["English"]) : 0;
                            int filipino = reader["Filipino"] != DBNull.Value ? Convert.ToInt32(reader["Filipino"]) : 0;
                            int aralingPanlipunan = reader["Araling_Panlipunan"] != DBNull.Value ? Convert.ToInt32(reader["Araling_Panlipunan"]) : 0;
                            int esp = reader["Edukasyon_sa_Pagpapakatao"] != DBNull.Value ? Convert.ToInt32(reader["Edukasyon_sa_Pagpapakatao"]) : 0;
                            int mapeh = reader["MAPEH"] != DBNull.Value ? Convert.ToInt32(reader["MAPEH"]) : 0;

                            // Display grades
                            mathematics_grade.Text = math > 0 ? math.ToString() : "N/A";
                            science_grade.Text = science > 0 ? science.ToString() : "N/A";
                            english_grade.Text = english > 0 ? english.ToString() : "N/A";
                            filipino_grade.Text = filipino > 0 ? filipino.ToString() : "N/A";
                            aralingpanlipunan_grade.Text = aralingPanlipunan > 0 ? aralingPanlipunan.ToString() : "N/A";
                            ESP_grade.Text = esp > 0 ? esp.ToString() : "N/A";
                            mapeh_grade.Text = mapeh > 0 ? mapeh.ToString() : "N/A";

                            // Calculate average grade
                            double average = CalculateAverage(math, science, english, filipino, aralingPanlipunan, esp, mapeh);
                            average_grade.Text = average > 0 ? average.ToString("0.00") : "N/A";
                        }
                        else
                        {
                            MessageBox.Show("No grades found for the student in this section.", "No Grades Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while loading grades: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private double CalculateAverage(params int[] grades)
        {
            int total = 0;
            int count = 0;

            foreach (int grade in grades)
            {
                if (grade > 0) 
                {
                    total += grade;
                    count++;
                }
            }

            return count > 0 ? (double)total / count : 0; 
        }


        private void guna2HtmlLabel4_Click(object sender, EventArgs e) { }
        private void tabPage1_Click(object sender, EventArgs e) { }
        private void studentname_Click(object sender, EventArgs e) { }
        private void tabPage2_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel23_Click(object sender, EventArgs e) { }
    }
}
