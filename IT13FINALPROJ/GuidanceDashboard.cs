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
                    row["Status"] = "Pending"; // Set default status to pending
                    row["Action"] = "Accept";   // Set default action
                }

                // Bind data to DataGridView
                guna2DataGridView1.DataSource = dt;

                // Set the AutoSizeMode for columns
                foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Makes columns fill available space
                }

                // Adjust the header height
                guna2DataGridView1.ColumnHeadersHeight = 30; // Adjust height as needed
                guna2DataGridView1.RowTemplate.Height = 25; // Adjust row height as needed

                // Make sure headers are visible
                guna2DataGridView1.ColumnHeadersVisible = true;
            }
        }



        private void AcceptStudent(int studentId)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;password=;database=it13proj"))
            {
                con.Open();

                // Move student to accepted_students_enroll
                string moveQuery = "INSERT INTO accepted_students_enroll (student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade) " +
                                   "SELECT student_id, firstname, middlename, lastname, sex, birthdate, birthplace, region, province, city, address, grade " +
                                   "FROM students_enroll WHERE student_id = @studentId;";

                MySqlCommand cmd = new MySqlCommand(moveQuery, con);
                cmd.Parameters.AddWithValue("@studentId", studentId);
                cmd.ExecuteNonQuery();

                // Delete related parent records first
                string deleteParentsQuery = "DELETE FROM parents WHERE student_id = @studentId;";
                MySqlCommand deleteParentsCmd = new MySqlCommand(deleteParentsQuery, con);
                deleteParentsCmd.Parameters.AddWithValue("@studentId", studentId);
                deleteParentsCmd.ExecuteNonQuery();

                // Then remove the student from students_enroll
                string deleteQuery = "DELETE FROM students_enroll WHERE student_id = @studentId;";
                MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, con);
                deleteCmd.Parameters.AddWithValue("@studentId", studentId);
                deleteCmd.ExecuteNonQuery();

                MessageBox.Show("Student accepted successfully!");
            }
        }




        //END OF FUNCTION AREA

        private void GuidanceDashboard_Load(object sender, EventArgs e)
        {
            // Setup DataGridView properties (if needed)
            guna2DataGridView1.AutoGenerateColumns = true;
            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;
        }

        private void pendingBTN_Click(object sender, EventArgs e)
        {
            LoadPendingStudents();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check for valid row index and column index
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Ensure the clicked column is "Action"
                if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "Action")
                {
                    // Get the selected student's ID
                    int studentId = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["student_id"].Value);

                    // Move the student to accepted_students_enroll
                    AcceptStudent(studentId);

                    // Optionally refresh the DataGridView
                    LoadPendingStudents();
                }
            }
        }

    }
}
