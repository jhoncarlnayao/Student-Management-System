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
    public partial class StudentDashboard : MaterialForm
    {
        private string studentFullName;
        // private string studentFirstname;
        //private string studentLastname;
        //private string studentMiddlename;
        private string fullname;
        private string email;
        private string phonenumber;
        public StudentDashboard(string fullname, string email, string phonenumber)
        {
            InitializeComponent();
            studentFullName = fullname;
            this.fullname = fullname;
            this.email = email;
            this.phonenumber = phonenumber;


            MajorPanel.Visible = false; //PANEL FOR PROGRAM
            MinorPanel.Visible = false; //PANEL FOR PROGRAM

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

        }



        private void StudentDashboard_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            string studentId = "5" + random.Next(1000000, 9999999).ToString();
            StudentIDtext.Text = studentId;

            Studentfullnametext.Text = fullname;
            Studentemailtext.Text = email;
            Studentphonenumbertext.Text = phonenumber;

            StudentIDtext.Enabled = false;
            Studentfullnametext.Enabled = false;
            Studentemailtext.Enabled = false;
            Studentphonenumbertext.Enabled = false;

           

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label56_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            // this is for the fullname
            //   Fullnamelabel.Text = studentFullName;
        }

        private void Home_Click(object sender, EventArgs e)
        {

        }

        private void label74_Click(object sender, EventArgs e)
        {

        }

        private void Professorprogram_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Enroll_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            int EnrollTab = 1;
            materialTabControl1.SelectedIndex = EnrollTab;
        }

        private void EnrollButton_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Programtab = 2;
            materialTabControl1.SelectedIndex = Programtab;
        }

        private void Program_Click(object sender, EventArgs e)
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void genderbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            genderbox.Text = "Gender";
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void nextbutton_Click(object sender, EventArgs e)
        {
            //nextinfo.Visible = true;
        }

        private void enroll2_Click(object sender, EventArgs e)
        {

        }

        private void createlabel_Click(object sender, EventArgs e)
        {

        }
    }
}

