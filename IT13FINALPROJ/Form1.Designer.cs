namespace IT13FINALPROJ
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            checkBox1 = new CheckBox();
            button1 = new Button();
            label4 = new Label();
            PasswordText = new TextBox();
            label73 = new Label();
            UsernameText = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.BackColor = Color.White;
            checkBox1.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkBox1.ForeColor = Color.Gray;
            checkBox1.Location = new Point(994, 470);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(296, 24);
            checkBox1.TabIndex = 31;
            checkBox1.Text = "i agree to our Terms of Service and Term";
            checkBox1.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.Black;
            button1.Location = new Point(974, 396);
            button1.Name = "button1";
            button1.Size = new Size(332, 47);
            button1.TabIndex = 30;
            button1.Text = "Sign in";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.White;
            label4.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(974, 311);
            label4.Name = "label4";
            label4.Size = new Size(66, 17);
            label4.TabIndex = 26;
            label4.Text = "Password";
            // 
            // PasswordText
            // 
            PasswordText.BorderStyle = BorderStyle.FixedSingle;
            PasswordText.Location = new Point(957, 320);
            PasswordText.Multiline = true;
            PasswordText.Name = "PasswordText";
            PasswordText.Size = new Size(364, 44);
            PasswordText.TabIndex = 25;
            PasswordText.TextChanged += this.PasswordText_TextChanged;
            // 
            // label73
            // 
            label73.AutoSize = true;
            label73.BackColor = Color.White;
            label73.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label73.ForeColor = Color.Black;
            label73.Location = new Point(974, 225);
            label73.Name = "label73";
            label73.Size = new Size(40, 17);
            label73.TabIndex = 24;
            label73.Text = "Email";
            label73.Click += label73_Click;
            // 
            // UsernameText
            // 
            UsernameText.BorderStyle = BorderStyle.FixedSingle;
            UsernameText.Location = new Point(957, 234);
            UsernameText.Multiline = true;
            UsernameText.Name = "UsernameText";
            UsernameText.Size = new Size(364, 44);
            UsernameText.TabIndex = 23;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.Window;
            label3.Font = new Font("Segoe UI", 11.25F);
            label3.ForeColor = SystemColors.GrayText;
            label3.Location = new Point(1081, 509);
            label3.Name = "label3";
            label3.Size = new Size(122, 20);
            label3.TabIndex = 18;
            label3.Text = "Forgot Account ?";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.White;
            label2.Font = new Font("Segoe UI Semilight", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(1011, 159);
            label2.Name = "label2";
            label2.Size = new Size(261, 25);
            label2.TabIndex = 6;
            label2.Text = "Log in to yout existing account";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.White;
            label1.Font = new Font("Segoe UI Black", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(994, 109);
            label1.Name = "label1";
            label1.Size = new Size(301, 50);
            label1.TabIndex = 5;
            label1.Text = "Welcome Back!";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(84, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(733, 641);
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox2.TabIndex = 32;
            pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(label3);
            panel1.Controls.Add(checkBox1);
            panel1.Controls.Add(label73);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(PasswordText);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(UsernameText);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 64);
            panel1.Name = "panel1";
            panel1.Size = new Size(1558, 709);
            panel1.TabIndex = 33;
            panel1.Paint += panel1_Paint_2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            ClientSize = new Size(1564, 776);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Login Account";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label label2;
        private Label label1;
        private Label label3;
        private Label label4;
        private TextBox PasswordText;
        private Label label73;
        private TextBox UsernameText;
        private Button button1;
        private CheckBox checkBox1;
        private PictureBox pictureBox2;
        private Panel panel1;
    }
}
