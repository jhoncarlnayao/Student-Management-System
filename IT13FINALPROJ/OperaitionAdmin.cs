using MaterialSkin.Controls;
using System;
using System.Windows.Forms;

namespace IT13FINALPROJ
{
    public partial class OperaitionAdmin : MaterialForm
    {
        private string professorId; // Store the selected professor ID

        public OperaitionAdmin(string id)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            professorId = id; // Assign the passed professor ID
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            // Open the UpdateProfessor form and pass the professor ID
            UpdateProfessor updateForm = new UpdateProfessor(professorId);
            updateForm.ShowDialog(); // Show the update form as a dialog
            this.Hide(); // Hide the ChooseOperationForm
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            // Add your delete logic here
            // For example, move the record to the backup table
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the form when cancel is clicked
        }
    }
}
