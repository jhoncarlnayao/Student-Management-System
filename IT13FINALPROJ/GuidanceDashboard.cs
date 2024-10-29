using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT13FINALPROJ
{
    public partial class GuidanceDashboard : MaterialForm
    {
        public GuidanceDashboard()
        {
            InitializeComponent();
            gradelevel.Items.Add("1");
            gradelevel.Items.Add("2");
            gradelevel.Items.Add("3");
            gradelevel.Items.Add("4");
            gradelevel.Items.Add("5");
            gradelevel.Items.Add("6");
            this.gradelevel.SelectedIndexChanged += new System.EventHandler(this.gradeLevelComboBox_SelectedIndexChanged);


        }

        //FUNCTION AREA:
        private void LoadPendingStudents()
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
            {
                con.Open();
                string query = "SELECT student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade FROM students_enroll";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Add Status and Action columns
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("Action", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    row["Status"] = "Pending";
                    row["Action"] = "Accept";
                }

                guna2DataGridView1.DataSource = dt;


                foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                // Adjust the header height
                guna2DataGridView1.ColumnHeadersHeight = 30;
                guna2DataGridView1.RowTemplate.Height = 25;

                // Make sure headers are visible
                guna2DataGridView1.ColumnHeadersVisible = true;
            }
        }

       


        private void AcceptStudent(int studentId)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
            {
                con.Open();

                string moveQuery = "INSERT INTO accepted_students_enroll (student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade) " +
                                   "SELECT student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade " +
                                   "FROM students_enroll WHERE student_id = @studentId;";

                MySqlCommand cmd = new MySqlCommand(moveQuery, con);
                cmd.Parameters.AddWithValue("@studentId", studentId);
                cmd.ExecuteNonQuery();


                string deleteParentsQuery = "DELETE FROM parents WHERE student_id = @studentId;";
                MySqlCommand deleteParentsCmd = new MySqlCommand(deleteParentsQuery, con);
                deleteParentsCmd.Parameters.AddWithValue("@studentId", studentId);
                deleteParentsCmd.ExecuteNonQuery();


                string deleteQuery = "DELETE FROM students_enroll WHERE student_id = @studentId;";
                MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, con);
                deleteCmd.Parameters.AddWithValue("@studentId", studentId);
                deleteCmd.ExecuteNonQuery();

                MessageBox.Show("Student accepted successfully!");
            }
        }

        private void LoadTeachersByGrade(int gradeLevel)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
                {
                    con.Open();
                    string query = "SELECT DISTINCT teacher_name FROM grade_levels WHERE grade_level = @gradeLevel";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@gradeLevel", gradeLevel);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    assignteacher.Items.Clear(); // Clear previous items

                    if (!reader.HasRows)
                    {
                        MessageBox.Show("No teachers found for the selected grade level."); // Log message
                    }
                    else
                    {
                        // Populate the assignteacher ComboBox with teacher names
                        while (reader.Read())
                        {
                            assignteacher.Items.Add(reader["teacher_name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }




        private void gradeLevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gradelevel.SelectedItem != null)
            {
                // Ensure the selected item is not null and can be parsed to an integer
                if (int.TryParse(gradelevel.SelectedItem.ToString(), out int selectedGrade))
                {
                    LoadTeachersByGrade(selectedGrade);
                }
                else
                {
                    MessageBox.Show("Please select a valid grade level.");
                }
            }
        }




        private void CreateNewTable(string tableName, string gradeLevel, string sectionName, string assignedTeacher, string academicYear, string roomNumber, int studentCapacity, string description)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
                {
                    con.Open();
                    string createTableQuery = $"CREATE TABLE IF NOT EXISTS `{tableName}` (" +
                                               "id INT AUTO_INCREMENT PRIMARY KEY, " +
                                               $"grade_level VARCHAR(255) NOT NULL, " +
                                               $"section_name VARCHAR(255) NOT NULL, " +
                                               $"assigned_teacher VARCHAR(255), " +
                                               $"academic_year VARCHAR(255), " +
                                               $"room_number VARCHAR(255), " +
                                               $"student_capacity INT, " +
                                               $"description TEXT)";

                    MySqlCommand cmd = new MySqlCommand(createTableQuery, con);
                    cmd.ExecuteNonQuery();

                    // Optionally insert the data into the new table
                    string insertDataQuery = $"INSERT INTO `{tableName}` (grade_level, section_name, assigned_teacher, academic_year, room_number, student_capacity, description) " +
                                              $"VALUES (@gradeLevel, @sectionName, @assignedTeacher, @academicYear, @roomNumber, @studentCapacity, @description)";

                    MySqlCommand insertCmd = new MySqlCommand(insertDataQuery, con);
                    insertCmd.Parameters.AddWithValue("@gradeLevel", gradeLevel);
                    insertCmd.Parameters.AddWithValue("@sectionName", sectionName);
                    insertCmd.Parameters.AddWithValue("@assignedTeacher", assignedTeacher);
                    insertCmd.Parameters.AddWithValue("@academicYear", academicYear);
                    insertCmd.Parameters.AddWithValue("@roomNumber", roomNumber);
                    insertCmd.Parameters.AddWithValue("@studentCapacity", studentCapacity);
                    insertCmd.Parameters.AddWithValue("@description", description); // Fixed parameter name
                    insertCmd.ExecuteNonQuery();

                    MessageBox.Show("New table created and data inserted successfully!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }





        //END OF FUNCTION AREA

        private void GuidanceDashboard_Load(object sender, EventArgs e)
        {

            guna2DataGridView1.AutoGenerateColumns = true;
            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;
        }

        private void pendingBTN_Click(object sender, EventArgs e)
        {
            LoadPendingStudents();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Action")
                {

                    int studentId = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["student_id"].Value);


                    AcceptStudent(studentId);


                    LoadPendingStudents();
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string tableName = nameoftable.Text; // Assuming you have a textbox for the table name
            string gradeLevel = gradelevel.SelectedItem.ToString();
            string sectionName = sectionname.Text; // Assuming you have a textbox for the section name
            string assignedTeacher = assignteacher.SelectedItem?.ToString(); // Use the null conditional 
            string academicYear = academicyear.Text; // Assuming you have a textbox for academic year
            string roomNumber = roomnumber.Text; // Assuming you have a textbox for room number
            int studentCapacity = Convert.ToInt32(studentcapacity.Text); // Assuming you have a textbox for student capacity
            string descriptionn = description.Text; // Assuming you have a textbox for description

            CreateNewTable(tableName, gradeLevel, sectionName, assignedTeacher, academicYear, roomNumber, studentCapacity, descriptionn);
        
    }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
    }
}
