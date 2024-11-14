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

namespace IT13FINALPROJ
{
    public partial class StudentDashboard : MaterialForm
    {
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }

        public StudentDashboard()
        {
            InitializeComponent();
        }

        private void StudentDashboard_Load(object sender, EventArgs e)
        {
            studentname.Text = StudentName;
            studentemail.Text = StudentEmail;
        }

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
