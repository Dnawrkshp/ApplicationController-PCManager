namespace PS3ApplicationControllerProject
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonConnect = new System.Windows.Forms.Button();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.cbConInput = new System.Windows.Forms.CheckBox();
            this.cbScrShare = new System.Windows.Forms.CheckBox();
            this.cbFPSCount = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.configSave = new System.Windows.Forms.Button();
            this.configLoad = new System.Windows.Forms.Button();
            this.toXMBButt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(12, 12);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(93, 15);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(100, 20);
            this.ipBox.TabIndex = 3;
            // 
            // cbConInput
            // 
            this.cbConInput.AutoSize = true;
            this.cbConInput.Checked = true;
            this.cbConInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConInput.Location = new System.Drawing.Point(200, 17);
            this.cbConInput.Name = "cbConInput";
            this.cbConInput.Size = new System.Drawing.Size(97, 17);
            this.cbConInput.TabIndex = 4;
            this.cbConInput.Text = "Controller Input";
            this.cbConInput.UseVisualStyleBackColor = true;
            this.cbConInput.CheckedChanged += new System.EventHandler(this.cbConInput_CheckedChanged);
            // 
            // cbScrShare
            // 
            this.cbScrShare.AutoSize = true;
            this.cbScrShare.Checked = true;
            this.cbScrShare.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbScrShare.Location = new System.Drawing.Point(303, 17);
            this.cbScrShare.Name = "cbScrShare";
            this.cbScrShare.Size = new System.Drawing.Size(99, 17);
            this.cbScrShare.TabIndex = 5;
            this.cbScrShare.Text = "Screen Sharing";
            this.cbScrShare.UseVisualStyleBackColor = true;
            this.cbScrShare.CheckedChanged += new System.EventHandler(this.cbScrShare_CheckedChanged);
            // 
            // cbFPSCount
            // 
            this.cbFPSCount.AutoSize = true;
            this.cbFPSCount.Location = new System.Drawing.Point(408, 18);
            this.cbFPSCount.Name = "cbFPSCount";
            this.cbFPSCount.Size = new System.Drawing.Size(46, 17);
            this.cbFPSCount.TabIndex = 6;
            this.cbFPSCount.Text = "FPS";
            this.cbFPSCount.UseVisualStyleBackColor = true;
            this.cbFPSCount.CheckedChanged += new System.EventHandler(this.cbFPSCount_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1%",
            "5%",
            "10%",
            "15%",
            "20%",
            "25%",
            "30%",
            "35%",
            "40%",
            "45%",
            "50%",
            "55%",
            "60%",
            "65%",
            "70%",
            "75%",
            "80%",
            "85%",
            "90%",
            "95%",
            "100%"});
            this.comboBox1.Location = new System.Drawing.Point(60, 41);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(133, 21);
            this.comboBox1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Quality:";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(525, 460);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Controls";
            // 
            // configSave
            // 
            this.configSave.Location = new System.Drawing.Point(381, 41);
            this.configSave.Name = "configSave";
            this.configSave.Size = new System.Drawing.Size(75, 23);
            this.configSave.TabIndex = 11;
            this.configSave.Text = "Save Config";
            this.configSave.UseVisualStyleBackColor = true;
            this.configSave.Click += new System.EventHandler(this.configSave_Click);
            // 
            // configLoad
            // 
            this.configLoad.Location = new System.Drawing.Point(462, 41);
            this.configLoad.Name = "configLoad";
            this.configLoad.Size = new System.Drawing.Size(75, 23);
            this.configLoad.TabIndex = 12;
            this.configLoad.Text = "Load Config";
            this.configLoad.UseVisualStyleBackColor = true;
            this.configLoad.Click += new System.EventHandler(this.configLoad_Click);
            // 
            // toXMBButt
            // 
            this.toXMBButt.Location = new System.Drawing.Point(462, 13);
            this.toXMBButt.Name = "toXMBButt";
            this.toXMBButt.Size = new System.Drawing.Size(75, 23);
            this.toXMBButt.TabIndex = 13;
            this.toXMBButt.Text = "XMB";
            this.toXMBButt.UseVisualStyleBackColor = true;
            this.toXMBButt.Click += new System.EventHandler(this.toXMBButt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 540);
            this.Controls.Add(this.toXMBButt);
            this.Controls.Add(this.configLoad);
            this.Controls.Add(this.configSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.cbFPSCount);
            this.Controls.Add(this.cbScrShare);
            this.Controls.Add(this.cbConInput);
            this.Controls.Add(this.ipBox);
            this.Controls.Add(this.buttonConnect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "PS3 Application Controller";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.CheckBox cbConInput;
        private System.Windows.Forms.CheckBox cbScrShare;
        private System.Windows.Forms.CheckBox cbFPSCount;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button configSave;
        private System.Windows.Forms.Button configLoad;
        private System.Windows.Forms.Button toXMBButt;
    }
}

