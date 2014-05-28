namespace PS3ApplicationControllerProject
{
    partial class keyInputUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.keyName = new System.Windows.Forms.Label();
            this.keyValue = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // keyName
            // 
            this.keyName.AutoSize = true;
            this.keyName.Location = new System.Drawing.Point(3, 6);
            this.keyName.Name = "keyName";
            this.keyName.Size = new System.Drawing.Size(52, 13);
            this.keyName.TabIndex = 0;
            this.keyName.Text = "keyName";
            // 
            // keyValue
            // 
            this.keyValue.Location = new System.Drawing.Point(102, 3);
            this.keyValue.Name = "keyValue";
            this.keyValue.Size = new System.Drawing.Size(100, 20);
            this.keyValue.TabIndex = 1;
            this.keyValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyValue_KeyDown);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(208, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(46, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // keyInputUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.keyValue);
            this.Controls.Add(this.keyName);
            this.Name = "keyInputUC";
            this.Size = new System.Drawing.Size(258, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label keyName;
        public System.Windows.Forms.TextBox keyValue;
        private System.Windows.Forms.Button button1;
    }
}
