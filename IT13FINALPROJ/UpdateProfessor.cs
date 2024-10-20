using MaterialSkin.Controls;
using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace IT13FINALPROJ
{
    public partial class UpdateProfessor : MaterialForm
    {
        private string professorId; // Store the selected professor ID

        public UpdateProfessor(string id)
        {
            InitializeComponent();
            professorId = id; // Assign the passed professor ID
            LoadProfessorData(); // Load the professor's current data
        }

        private void LoadProfessorData()
        {
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";
            string query = $"SELECT * FROM professors_accounts WHERE id = {professorId}";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Pre-fill the form fields with the current professor's data
                    Updatefullname.Text = reader["Fullname"].ToString();
                    Updateemail.Text = reader["Email"].ToString();
                    Updatephonenumber.Text = reader["Phonenumber"].ToString();
                    Updateprogram.SelectedItem = reader["Program"].ToString(); // Assuming this is a ComboBox
                }
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            // Update professor data in the database
            string connectionString = "server=localhost;database=it13finalproj;user=root;password=;";
            string query = $"UPDATE professors_accounts SET Fullname=@Fullname, Email=@Email, Phonenumber=@Phone, Program=@Program WHERE id={professorId}";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Fullname", Updatefullname.Text);
                cmd.Parameters.AddWithValue("@Email", Updateemail.Text);
                cmd.Parameters.AddWithValue("@Phone", Updatephonenumber.Text);
                cmd.Parameters.AddWithValue("@Program", Updateprogram.SelectedItem?.ToString()); // Check if SelectedItem is null

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Professor updated successfully!");
            this.Close(); // Close the UpdateProfessor form after updating
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the UpdateProfessor form when cancel is clicked
        }
    }
}
