using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace IT13FINALPROJ
{
    public partial class StudentForm : Form
    {
        public StudentForm()
        {
            InitializeComponent();
        }

        // Function to enroll student and parent
        public void EnrollStudentAndParent(string studentFirstName, string studentMiddleName, string studentLastName, string sex, DateTime birthdate, string birthplace, string region, string province, string city, string address, string grade,
                                           string parentFirstName, string parentMiddleName, string parentLastName, string phoneNumber, string email)
        {
            // Connection string to the database
            string connectionString = "server=localhost;user=root;password=;database=it13proj";

            // Student insert query
            string studentQuery = "INSERT INTO students_enroll (firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade) " +
                                  "VALUES (@studentFirstName, @studentMiddleName, @studentLastName, @sex, @birthdate, @birthplace, @region, @province, @city, @address, @grade)";

            // Parent insert query
            string parentQuery = "INSERT INTO parents (student_id, firstname, middlename, lastname, phonenumber, email) " +
                                 "VALUES (@studentId, @parentFirstName, @parentMiddleName, @parentLastName, @phoneNumber, @parentEmail)";

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    // Insert Student Data
                    using (MySqlCommand studentCmd = new MySqlCommand(studentQuery, con))
                    {
                        // Add parameters for student data
                        studentCmd.Parameters.AddWithValue("@studentFirstName", studentFirstName);
                        studentCmd.Parameters.AddWithValue("@studentMiddleName", string.IsNullOrEmpty(studentMiddleName) ? (object)DBNull.Value : studentMiddleName);
                        studentCmd.Parameters.AddWithValue("@studentLastName", studentLastName);
                        studentCmd.Parameters.AddWithValue("@sex", sex);
                        studentCmd.Parameters.AddWithValue("@birthdate", birthdate);
                        studentCmd.Parameters.AddWithValue("@birthplace", birthplace);
                        studentCmd.Parameters.AddWithValue("@region", region);
                        studentCmd.Parameters.AddWithValue("@province", province);
                        studentCmd.Parameters.AddWithValue("@city", city);
                        studentCmd.Parameters.AddWithValue("@address", address);
                        studentCmd.Parameters.AddWithValue("@grade", grade);

                        studentCmd.ExecuteNonQuery(); // Execute the student insert query

                        // Retrieve the student_id of the inserted student
                        long studentId = studentCmd.LastInsertedId;

                        // Insert Parent Data
                        using (MySqlCommand parentCmd = new MySqlCommand(parentQuery, con))
                        {
                            // Add parameters for parent data
                            parentCmd.Parameters.AddWithValue("@studentId", studentId);
                            parentCmd.Parameters.AddWithValue("@parentFirstName", parentFirstName);
                            parentCmd.Parameters.AddWithValue("@parentMiddleName", string.IsNullOrEmpty(parentMiddleName) ? (object)DBNull.Value : parentMiddleName);
                            parentCmd.Parameters.AddWithValue("@parentLastName", parentLastName);
                            parentCmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                            parentCmd.Parameters.AddWithValue("@parentEmail", email);

                            parentCmd.ExecuteNonQuery(); // Execute the parent insert query
                        }

                        MessageBox.Show("Student and Parent enrolled successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Event handler for the 'Enroll' button click
        // Event handler for the 'Enroll' button click
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Collect Student Data
            string studentFirstName = STfirstname.Text;
            string studentMiddleName = STmiddlename.Text;
            string studentLastName = STlastname.Text;

            string sex = STsex.SelectedItem?.ToString() ?? string.Empty; // Safely get selected item
            DateTime birthdate = STbirthdate.Value; // Assuming DateTimePicker for 'Birthdate'
            string birthplace = STbirthplace.Text;
            string region = STregion.Text;
            string province = STprovince.Text;
            string city = STcity.Text;
            string address = STaddress.Text;
            string grade = stgrade.SelectedItem?.ToString() ?? string.Empty; // Safely get selected item

            // Collect Parent Data
            string parentFirstName = PRfirstname.Text;
            string parentMiddleName = PRmiddlename.Text;
            string parentLastName = PRlastname.Text;
            string phoneNumber = PRphonenumber.Text;
            string email = PRemail.Text;

            // Check for null values
            if (string.IsNullOrEmpty(sex) || string.IsNullOrEmpty(grade))
            {
                MessageBox.Show("Please select both sex and grade.");
                return;
            }

            // Call the function to enroll student and parent
            EnrollStudentAndParent(studentFirstName, studentMiddleName, studentLastName, sex, birthdate, birthplace, region, province, city, address, grade,
                                   parentFirstName, parentMiddleName, parentLastName, phoneNumber, email);
        }

    }
}
