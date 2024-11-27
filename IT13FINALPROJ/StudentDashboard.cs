using MaterialSkin.Controls;
using System;
using System.Windows.Forms;

namespace IT13FINALPROJ
{
    public partial class StudentDashboard : MaterialForm
    {
        // Public properties to hold student data
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
            // Set control values using properties
            student_fullname.Text = StudentName ?? "N/A"; // Use "N/A" as default if null
            student_id.Text = StudentId.ToString();
            student_grade.Text = StudentGrade ?? "N/A";
            student_sex.Text = StudentSex ?? "N/A";
            student_birthdate.Text = StudentBirthdate ?? "N/A";
            student_birthplace.Text = StudentBirthplace ?? "N/A";
            student_region.Text = StudentRegion ?? "N/A";
            student_province.Text = StudentProvince ?? "N/A";
            student_city.Text = StudentCity ?? "N/A";
            student_address.Text = StudentAddress ?? "N/A";
        }

        // Other event handlers remain unchanged
        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }

        private void studentname_Click(object sender, EventArgs e)
        {
        }
    }
}
