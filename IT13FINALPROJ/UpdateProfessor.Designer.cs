namespace IT13FINALPROJ
{
    partial class UpdateProfessor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Updateprogram = new ComboBox();
            Update = new Button();
            label70 = new Label();
            Updatepassword = new TextBox();
            label71 = new Label();
            Updatephonenumber = new TextBox();
            label72 = new Label();
            Updateemail = new TextBox();
            label73 = new Label();
            Updatefullname = new TextBox();
            label1 = new Label();
            Cancel = new Button();
            SuspendLayout();
            // 
            // Updateprogram
            // 
            Updateprogram.FormattingEnabled = true;
            Updateprogram.Items.AddRange(new object[] { "BS in Information Technology", "BS in Computer Science", "BS in Cybersecurity", "BS in Software Engineer", "Bacherlor of Architecture", "BS in Interior Design", "BS in Graphic Design", "BS in Urban Planning" });
            Updateprogram.Location = new Point(46, 457);
            Updateprogram.Name = "Updateprogram";
            Updateprogram.Size = new Size(365, 23);
            Updateprogram.TabIndex = 50;
            Updateprogram.Text = "Select what program you want to handle";
            Updateprogram.SelectedIndexChanged += Updateprogram_SelectedIndexChanged;
            // 
            // Update
            // 
            Update.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Update.ForeColor = Color.Black;
            Update.Location = new Point(47, 503);
            Update.Name = "Update";
            Update.Size = new Size(364, 47);
            Update.TabIndex = 49;
            Update.Text = "Update";
            Update.UseVisualStyleBackColor = true;
            Update.Click += Update_Click;
            // 
            // label70
            // 
            label70.AutoSize = true;
            label70.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label70.ForeColor = Color.Black;
            label70.Location = new Point(64, 380);
            label70.Name = "label70";
            label70.Size = new Size(66, 17);
            label70.TabIndex = 48;
            label70.Text = "Password";
            // 
            // Updatepassword
            // 
            Updatepassword.BorderStyle = BorderStyle.FixedSingle;
            Updatepassword.Location = new Point(47, 389);
            Updatepassword.Multiline = true;
            Updatepassword.Name = "Updatepassword";
            Updatepassword.Size = new Size(364, 44);
            Updatepassword.TabIndex = 47;
            Updatepassword.TextChanged += Updatepassword_TextChanged;
            // 
            // label71
            // 
            label71.AutoSize = true;
            label71.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label71.ForeColor = Color.Black;
            label71.Location = new Point(64, 307);
            label71.Name = "label71";
            label71.Size = new Size(101, 17);
            label71.TabIndex = 46;
            label71.Text = "Phone Number";
            // 
            // Updatephonenumber
            // 
            Updatephonenumber.BorderStyle = BorderStyle.FixedSingle;
            Updatephonenumber.Location = new Point(47, 316);
            Updatephonenumber.Multiline = true;
            Updatephonenumber.Name = "Updatephonenumber";
            Updatephonenumber.Size = new Size(364, 44);
            Updatephonenumber.TabIndex = 45;
            Updatephonenumber.TextChanged += Updatephonenumber_TextChanged;
            // 
            // label72
            // 
            label72.AutoSize = true;
            label72.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label72.ForeColor = Color.Black;
            label72.Location = new Point(64, 227);
            label72.Name = "label72";
            label72.Size = new Size(40, 17);
            label72.TabIndex = 44;
            label72.Text = "Email";
            // 
            // Updateemail
            // 
            Updateemail.BorderStyle = BorderStyle.FixedSingle;
            Updateemail.Location = new Point(47, 236);
            Updateemail.Multiline = true;
            Updateemail.Name = "Updateemail";
            Updateemail.Size = new Size(364, 44);
            Updateemail.TabIndex = 43;
            Updateemail.TextChanged += Updateemail_TextChanged;
            // 
            // label73
            // 
            label73.AutoSize = true;
            label73.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label73.ForeColor = Color.Black;
            label73.Location = new Point(64, 148);
            label73.Name = "label73";
            label73.Size = new Size(63, 17);
            label73.TabIndex = 42;
            label73.Text = "Fullname";
            // 
            // Updatefullname
            // 
            Updatefullname.BorderStyle = BorderStyle.FixedSingle;
            Updatefullname.Location = new Point(47, 157);
            Updatefullname.Multiline = true;
            Updatefullname.Name = "Updatefullname";
            Updatefullname.Size = new Size(364, 44);
            Updatefullname.TabIndex = 41;
            Updatefullname.TextChanged += Updatefullname_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(47, 96);
            label1.Name = "label1";
            label1.Size = new Size(366, 32);
            label1.TabIndex = 51;
            label1.Text = "Update Professor Information";
            // 
            // Cancel
            // 
            Cancel.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Cancel.ForeColor = Color.Black;
            Cancel.Location = new Point(47, 565);
            Cancel.Name = "Cancel";
            Cancel.Size = new Size(364, 47);
            Cancel.TabIndex = 52;
            Cancel.Text = "Cancel";
            Cancel.UseVisualStyleBackColor = true;
            Cancel.Click += Cancel_Click;
            // 
            // UpdateProfessor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(478, 669);
            Controls.Add(Cancel);
            Controls.Add(label1);
            Controls.Add(Updateprogram);
            Controls.Add(Update);
            Controls.Add(label70);
            Controls.Add(Updatepassword);
            Controls.Add(label71);
            Controls.Add(Updatephonenumber);
            Controls.Add(label72);
            Controls.Add(Updateemail);
            Controls.Add(label73);
            Controls.Add(Updatefullname);
            Name = "UpdateProfessor";
            Text = "UpdateProfessor";
            Load += UpdateProfessor_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox Updateprogram;
        private Button Update;
        private Label label70;
        private TextBox Updatepassword;
        private Label label71;
        private TextBox Updatephonenumber;
        private Label label72;
        private TextBox Updateemail;
        private Label label73;
        private TextBox Updatefullname;
        private Label label1;
        private Button Cancel;
    }
}