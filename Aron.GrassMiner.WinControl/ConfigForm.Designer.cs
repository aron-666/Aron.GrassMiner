namespace Aron.GrassMiner.WinControl
{
    partial class ConfigForm
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
            groupBox1 = new GroupBox();
            txGrassPass = new TextBox();
            txGrassUser = new TextBox();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            txAdminPass = new TextBox();
            txAdminUser = new TextBox();
            label3 = new Label();
            label4 = new Label();
            groupBox3 = new GroupBox();
            panelProxy = new Panel();
            panelAuth = new Panel();
            txProxyUser = new TextBox();
            label7 = new Label();
            txProxyPass = new TextBox();
            label5 = new Label();
            txProxyHost = new TextBox();
            ckProxyAuth = new CheckBox();
            label6 = new Label();
            ckProxy = new CheckBox();
            btnSave = new Button();
            txUrl = new TextBox();
            label8 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            panelProxy.SuspendLayout();
            panelAuth.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txGrassPass);
            groupBox1.Controls.Add(txGrassUser);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point, 136);
            groupBox1.Location = new Point(24, 21);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(414, 125);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Grass Account";
            // 
            // txGrassPass
            // 
            txGrassPass.Location = new Point(154, 71);
            txGrassPass.Name = "txGrassPass";
            txGrassPass.PasswordChar = '*';
            txGrassPass.Size = new Size(221, 34);
            txGrassPass.TabIndex = 3;
            // 
            // txGrassUser
            // 
            txGrassUser.Location = new Point(154, 27);
            txGrassUser.Name = "txGrassUser";
            txGrassUser.Size = new Size(221, 34);
            txGrassUser.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(32, 74);
            label2.Name = "label2";
            label2.Size = new Size(102, 25);
            label2.TabIndex = 1;
            label2.Text = "Password";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 30);
            label1.Name = "label1";
            label1.Size = new Size(107, 25);
            label1.TabIndex = 0;
            label1.Text = "Username";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txAdminPass);
            groupBox2.Controls.Add(txAdminUser);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label4);
            groupBox2.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point, 136);
            groupBox2.Location = new Point(24, 152);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(414, 125);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Admin Account";
            // 
            // txAdminPass
            // 
            txAdminPass.Location = new Point(154, 71);
            txAdminPass.Name = "txAdminPass";
            txAdminPass.PasswordChar = '*';
            txAdminPass.Size = new Size(221, 34);
            txAdminPass.TabIndex = 3;
            // 
            // txAdminUser
            // 
            txAdminUser.Location = new Point(154, 27);
            txAdminUser.Name = "txAdminUser";
            txAdminUser.Size = new Size(221, 34);
            txAdminUser.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(32, 74);
            label3.Name = "label3";
            label3.Size = new Size(102, 25);
            label3.TabIndex = 1;
            label3.Text = "Password";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(32, 30);
            label4.Name = "label4";
            label4.Size = new Size(107, 25);
            label4.TabIndex = 0;
            label4.Text = "Username";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(panelProxy);
            groupBox3.Controls.Add(ckProxy);
            groupBox3.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point, 136);
            groupBox3.Location = new Point(24, 283);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(414, 294);
            groupBox3.TabIndex = 5;
            groupBox3.TabStop = false;
            groupBox3.Text = "Proxy";
            // 
            // panelProxy
            // 
            panelProxy.Controls.Add(panelAuth);
            panelProxy.Controls.Add(txProxyHost);
            panelProxy.Controls.Add(ckProxyAuth);
            panelProxy.Controls.Add(label6);
            panelProxy.Location = new Point(27, 68);
            panelProxy.Name = "panelProxy";
            panelProxy.Size = new Size(379, 209);
            panelProxy.TabIndex = 6;
            // 
            // panelAuth
            // 
            panelAuth.Controls.Add(txProxyUser);
            panelAuth.Controls.Add(label7);
            panelAuth.Controls.Add(txProxyPass);
            panelAuth.Controls.Add(label5);
            panelAuth.Location = new Point(21, 98);
            panelAuth.Name = "panelAuth";
            panelAuth.Size = new Size(354, 105);
            panelAuth.TabIndex = 8;
            // 
            // txProxyUser
            // 
            txProxyUser.Location = new Point(130, 6);
            txProxyUser.Name = "txProxyUser";
            txProxyUser.Size = new Size(221, 34);
            txProxyUser.TabIndex = 5;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(8, 9);
            label7.Name = "label7";
            label7.Size = new Size(107, 25);
            label7.TabIndex = 4;
            label7.Text = "Username";
            // 
            // txProxyPass
            // 
            txProxyPass.Location = new Point(130, 55);
            txProxyPass.Name = "txProxyPass";
            txProxyPass.PasswordChar = '*';
            txProxyPass.Size = new Size(221, 34);
            txProxyPass.TabIndex = 3;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 58);
            label5.Name = "label5";
            label5.Size = new Size(102, 25);
            label5.TabIndex = 1;
            label5.Text = "Password";
            // 
            // txProxyHost
            // 
            txProxyHost.Location = new Point(127, 23);
            txProxyHost.Name = "txProxyHost";
            txProxyHost.Size = new Size(221, 34);
            txProxyHost.TabIndex = 2;
            // 
            // ckProxyAuth
            // 
            ckProxyAuth.AutoSize = true;
            ckProxyAuth.Location = new Point(21, 63);
            ckProxyAuth.Name = "ckProxyAuth";
            ckProxyAuth.Size = new Size(147, 29);
            ckProxyAuth.TabIndex = 7;
            ckProxyAuth.Text = "Auth Enable";
            ckProxyAuth.UseVisualStyleBackColor = true;
            ckProxyAuth.CheckedChanged += ckProxyAuth_CheckedChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(19, 26);
            label6.Name = "label6";
            label6.Size = new Size(56, 25);
            label6.TabIndex = 0;
            label6.Text = "Host";
            // 
            // ckProxy
            // 
            ckProxy.AutoSize = true;
            ckProxy.Location = new Point(27, 33);
            ckProxy.Name = "ckProxy";
            ckProxy.Size = new Size(154, 29);
            ckProxy.TabIndex = 4;
            ckProxy.Text = "Proxy Enable";
            ckProxy.UseVisualStyleBackColor = true;
            ckProxy.CheckedChanged += ckProxy_CheckedChanged;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("微軟正黑體", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(287, 727);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(151, 54);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // txUrl
            // 
            txUrl.Font = new Font("Microsoft JhengHei UI", 12F);
            txUrl.Location = new Point(24, 648);
            txUrl.Name = "txUrl";
            txUrl.Size = new Size(414, 33);
            txUrl.TabIndex = 7;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft JhengHei UI", 12F);
            label8.Location = new Point(24, 611);
            label8.Name = "label8";
            label8.Size = new Size(84, 25);
            label8.TabIndex = 6;
            label8.Text = "App Url";
            // 
            // ConfigForm
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(469, 802);
            Controls.Add(txUrl);
            Controls.Add(label8);
            Controls.Add(btnSave);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "ConfigForm";
            Text = "ConfigForm";
            Load += ConfigForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            panelProxy.ResumeLayout(false);
            panelProxy.PerformLayout();
            panelAuth.ResumeLayout(false);
            panelAuth.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private TextBox txGrassPass;
        private TextBox txGrassUser;
        private Label label2;
        private GroupBox groupBox2;
        private TextBox txAdminPass;
        private TextBox txAdminUser;
        private Label label3;
        private Label label4;
        private GroupBox groupBox3;
        private TextBox txProxyPass;
        private TextBox txProxyHost;
        private Label label5;
        private Label label6;
        private Panel panelProxy;
        private CheckBox ckProxy;
        private Panel panelAuth;
        private TextBox txProxyUser;
        private Label label7;
        private CheckBox ckProxyAuth;
        private Button btnSave;
        private TextBox txUrl;
        private Label label8;
    }
}