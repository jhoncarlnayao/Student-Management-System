namespace IT13FINALPROJ
{
    partial class OperaitionAdmin
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
            label1 = new Label();
            panel1 = new Panel();
            materialButton1 = new MaterialSkin.Controls.MaterialButton();
            label3 = new Label();
            label2 = new Label();
            panel2 = new Panel();
            materialButton2 = new MaterialSkin.Controls.MaterialButton();
            label4 = new Label();
            label5 = new Label();
            panel3 = new Panel();
            materialButton3 = new MaterialSkin.Controls.MaterialButton();
            label6 = new Label();
            label7 = new Label();
            button1 = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(18, 72);
            label1.Name = "label1";
            label1.Size = new Size(466, 37);
            label1.TabIndex = 1;
            label1.Text = "Choose from the available actions";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLight;
            panel1.Controls.Add(materialButton1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(18, 130);
            panel1.Name = "panel1";
            panel1.Size = new Size(396, 141);
            panel1.TabIndex = 2;
            // 
            // materialButton1
            // 
            materialButton1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton1.Depth = 0;
            materialButton1.HighEmphasis = true;
            materialButton1.Icon = null;
            materialButton1.Location = new Point(216, 84);
            materialButton1.Margin = new Padding(4, 6, 4, 6);
            materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton1.Name = "materialButton1";
            materialButton1.NoAccentTextColor = Color.Empty;
            materialButton1.Size = new Size(169, 36);
            materialButton1.TabIndex = 5;
            materialButton1.Text = "Click me to update";
            materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton1.UseAccentColor = false;
            materialButton1.UseVisualStyleBackColor = true;
            materialButton1.Click += materialButton1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(17, 46);
            label3.Name = "label3";
            label3.Size = new Size(317, 17);
            label3.TabIndex = 4;
            label3.Text = "Modify or edit existing data in a database or system.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Black", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(17, 16);
            label2.Name = "label2";
            label2.Size = new Size(88, 30);
            label2.TabIndex = 3;
            label2.Text = "Update";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ControlLight;
            panel2.Controls.Add(materialButton2);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label5);
            panel2.Location = new Point(436, 130);
            panel2.Name = "panel2";
            panel2.Size = new Size(396, 141);
            panel2.TabIndex = 6;
            // 
            // materialButton2
            // 
            materialButton2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            materialButton2.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton2.Depth = 0;
            materialButton2.HighEmphasis = true;
            materialButton2.Icon = null;
            materialButton2.Location = new Point(216, 84);
            materialButton2.Margin = new Padding(4, 6, 4, 6);
            materialButton2.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton2.Name = "materialButton2";
            materialButton2.NoAccentTextColor = Color.Empty;
            materialButton2.Size = new Size(169, 36);
            materialButton2.TabIndex = 5;
            materialButton2.Text = "Click me to update";
            materialButton2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton2.UseAccentColor = false;
            materialButton2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(17, 46);
            label4.Name = "label4";
            label4.Size = new Size(340, 34);
            label4.TabIndex = 4;
            label4.Text = "Moves data to a backup table for safe storage, allowing \r\nrecovery if needed instead of permanent removal.";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Black", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(17, 16);
            label5.Name = "label5";
            label5.Size = new Size(64, 30);
            label5.TabIndex = 3;
            label5.Text = "Drop";
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlLight;
            panel3.Controls.Add(materialButton3);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(label7);
            panel3.Location = new Point(18, 292);
            panel3.Name = "panel3";
            panel3.Size = new Size(396, 141);
            panel3.TabIndex = 6;
            // 
            // materialButton3
            // 
            materialButton3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            materialButton3.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton3.Depth = 0;
            materialButton3.HighEmphasis = true;
            materialButton3.Icon = null;
            materialButton3.Location = new Point(216, 84);
            materialButton3.Margin = new Padding(4, 6, 4, 6);
            materialButton3.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton3.Name = "materialButton3";
            materialButton3.NoAccentTextColor = Color.Empty;
            materialButton3.Size = new Size(169, 36);
            materialButton3.TabIndex = 5;
            materialButton3.Text = "Click me to update";
            materialButton3.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton3.UseAccentColor = false;
            materialButton3.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(17, 46);
            label6.Name = "label6";
            label6.Size = new Size(292, 34);
            label6.TabIndex = 4;
            label6.Text = "Display data or records for users to see without \r\nmaking any changes.";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Black", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(17, 16);
            label7.Name = "label7";
            label7.Size = new Size(64, 30);
            label7.TabIndex = 3;
            label7.Text = "View";
            // 
            // button1
            // 
            button1.BackColor = Color.SpringGreen;
            button1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(648, 390);
            button1.Name = "button1";
            button1.Size = new Size(184, 43);
            button1.TabIndex = 7;
            button1.Text = "Confirm";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_1;
            // 
            // OperaitionAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(865, 481);
            Controls.Add(button1);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(label1);
            Name = "OperaitionAdmin";
            Text = "OperaitionAdmin";
            Load += OperaitionAdmin_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private Label label2;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private Label label3;
        private Panel panel2;
        private MaterialSkin.Controls.MaterialButton materialButton2;
        private Label label4;
        private Label label5;
        private Panel panel3;
        private MaterialSkin.Controls.MaterialButton materialButton3;
        private Label label6;
        private Label label7;
        private Button button1;
    }
}