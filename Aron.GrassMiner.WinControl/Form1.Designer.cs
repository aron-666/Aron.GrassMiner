namespace Aron.GrassMiner.WinControl
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
            components = new System.ComponentModel.Container();
            ckServiceInstalled = new CheckBox();
            groupBox1 = new GroupBox();
            btnLogs = new Button();
            btnConfig = new Button();
            btnAdmin = new Button();
            btnControl = new Button();
            lbPorts = new Label();
            lbPid = new Label();
            lbModule = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // ckServiceInstalled
            // 
            ckServiceInstalled.Appearance = Appearance.Button;
            ckServiceInstalled.BackColor = Color.White;
            ckServiceInstalled.Font = new Font("Microsoft Sans Serif", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ckServiceInstalled.ImageAlign = ContentAlignment.TopLeft;
            ckServiceInstalled.Location = new Point(44, 65);
            ckServiceInstalled.Margin = new Padding(0);
            ckServiceInstalled.Name = "ckServiceInstalled";
            ckServiceInstalled.Size = new Size(35, 35);
            ckServiceInstalled.TabIndex = 0;
            ckServiceInstalled.TextAlign = ContentAlignment.TopLeft;
            ckServiceInstalled.UseVisualStyleBackColor = false;
            ckServiceInstalled.CheckedChanged += ckServiceInstalled_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnLogs);
            groupBox1.Controls.Add(btnConfig);
            groupBox1.Controls.Add(btnAdmin);
            groupBox1.Controls.Add(btnControl);
            groupBox1.Controls.Add(lbPorts);
            groupBox1.Controls.Add(lbPid);
            groupBox1.Controls.Add(lbModule);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(ckServiceInstalled);
            groupBox1.Font = new Font("微軟正黑體", 12F, FontStyle.Bold, GraphicsUnit.Point, 136);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(818, 131);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Modules";
            // 
            // btnLogs
            // 
            btnLogs.Font = new Font("微軟正黑體", 9F);
            btnLogs.Location = new Point(646, 64);
            btnLogs.Name = "btnLogs";
            btnLogs.Size = new Size(77, 36);
            btnLogs.TabIndex = 12;
            btnLogs.Text = "Logs";
            btnLogs.UseVisualStyleBackColor = true;
            btnLogs.Click += btnLogs_Click;
            // 
            // btnConfig
            // 
            btnConfig.Font = new Font("微軟正黑體", 9F);
            btnConfig.Location = new Point(563, 64);
            btnConfig.Name = "btnConfig";
            btnConfig.Size = new Size(77, 36);
            btnConfig.TabIndex = 11;
            btnConfig.Text = "Config";
            btnConfig.UseVisualStyleBackColor = true;
            // 
            // btnAdmin
            // 
            btnAdmin.Font = new Font("微軟正黑體", 9F);
            btnAdmin.Location = new Point(480, 65);
            btnAdmin.Name = "btnAdmin";
            btnAdmin.Size = new Size(77, 36);
            btnAdmin.TabIndex = 10;
            btnAdmin.Text = "Admin";
            btnAdmin.UseVisualStyleBackColor = true;
            // 
            // btnControl
            // 
            btnControl.Font = new Font("微軟正黑體", 9F);
            btnControl.Location = new Point(397, 64);
            btnControl.Name = "btnControl";
            btnControl.Size = new Size(77, 36);
            btnControl.TabIndex = 9;
            btnControl.Text = "Start";
            btnControl.UseVisualStyleBackColor = true;
            btnControl.Click += btnControl_Click;
            // 
            // lbPorts
            // 
            lbPorts.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 136);
            lbPorts.Location = new Point(302, 69);
            lbPorts.Name = "lbPorts";
            lbPorts.Size = new Size(69, 25);
            lbPorts.TabIndex = 8;
            lbPorts.Text = "0";
            lbPorts.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbPid
            // 
            lbPid.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 136);
            lbPid.Location = new Point(205, 68);
            lbPid.Name = "lbPid";
            lbPid.Size = new Size(69, 25);
            lbPid.TabIndex = 7;
            lbPid.Text = "0";
            lbPid.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbModule
            // 
            lbModule.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 136);
            lbModule.Location = new Point(99, 70);
            lbModule.Name = "lbModule";
            lbModule.Size = new Size(90, 24);
            lbModule.TabIndex = 6;
            lbModule.Text = "Grass";
            lbModule.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("微軟正黑體", 10.2F, FontStyle.Bold);
            label5.Location = new Point(397, 32);
            label5.Name = "label5";
            label5.Size = new Size(73, 22);
            label5.TabIndex = 5;
            label5.Text = "Actions";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("微軟正黑體", 10.2F, FontStyle.Bold);
            label4.Location = new Point(313, 32);
            label4.Name = "label4";
            label4.Size = new Size(46, 22);
            label4.TabIndex = 4;
            label4.Text = "Port";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("微軟正黑體", 10.2F, FontStyle.Bold);
            label3.Location = new Point(218, 32);
            label3.Name = "label3";
            label3.Size = new Size(39, 22);
            label3.TabIndex = 3;
            label3.Text = "PID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("微軟正黑體", 10.2F, FontStyle.Bold);
            label2.Location = new Point(107, 32);
            label2.Name = "label2";
            label2.Size = new Size(75, 22);
            label2.TabIndex = 2;
            label2.Text = "Module";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("微軟正黑體", 10.2F, FontStyle.Bold);
            label1.Location = new Point(29, 32);
            label1.Name = "label1";
            label1.Size = new Size(70, 22);
            label1.TabIndex = 1;
            label1.Text = "Service";
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 500;
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(842, 475);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private CheckBox ckServiceInstalled;
        private GroupBox groupBox1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label lbPid;
        private Label lbModule;
        private Button btnLogs;
        private Button btnConfig;
        private Button btnAdmin;
        private Button btnControl;
        private Label lbPorts;
        private System.Windows.Forms.Timer timer1;
    }
}
