namespace V2RayGConTest.Views
{
    partial class FormExtractLinks
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
            this.rTboxInput = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rTboxOutput = new System.Windows.Forms.RichTextBox();
            this.btnSS = new System.Windows.Forms.Button();
            this.btnVmess = new System.Windows.Forms.Button();
            this.btnV2ray = new System.Windows.Forms.Button();
            this.btnSSR = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rTboxInput
            // 
            this.rTboxInput.Location = new System.Drawing.Point(12, 28);
            this.rTboxInput.Name = "rTboxInput";
            this.rTboxInput.Size = new System.Drawing.Size(243, 172);
            this.rTboxInput.TabIndex = 0;
            this.rTboxInput.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(259, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output";
            // 
            // rTboxOutput
            // 
            this.rTboxOutput.Location = new System.Drawing.Point(261, 28);
            this.rTboxOutput.Name = "rTboxOutput";
            this.rTboxOutput.Size = new System.Drawing.Size(243, 172);
            this.rTboxOutput.TabIndex = 2;
            this.rTboxOutput.Text = "";
            // 
            // btnSS
            // 
            this.btnSS.Location = new System.Drawing.Point(510, 28);
            this.btnSS.Name = "btnSS";
            this.btnSS.Size = new System.Drawing.Size(75, 23);
            this.btnSS.TabIndex = 4;
            this.btnSS.Text = "SS";
            this.btnSS.UseVisualStyleBackColor = true;
            this.btnSS.Click += new System.EventHandler(this.btnSS_Click);
            // 
            // btnVmess
            // 
            this.btnVmess.Location = new System.Drawing.Point(510, 55);
            this.btnVmess.Name = "btnVmess";
            this.btnVmess.Size = new System.Drawing.Size(75, 23);
            this.btnVmess.TabIndex = 4;
            this.btnVmess.Text = "vmess";
            this.btnVmess.UseVisualStyleBackColor = true;
            // 
            // btnV2ray
            // 
            this.btnV2ray.Location = new System.Drawing.Point(510, 84);
            this.btnV2ray.Name = "btnV2ray";
            this.btnV2ray.Size = new System.Drawing.Size(75, 23);
            this.btnV2ray.TabIndex = 4;
            this.btnV2ray.Text = "v2ray";
            this.btnV2ray.UseVisualStyleBackColor = true;
            // 
            // btnSSR
            // 
            this.btnSSR.Location = new System.Drawing.Point(510, 113);
            this.btnSSR.Name = "btnSSR";
            this.btnSSR.Size = new System.Drawing.Size(75, 23);
            this.btnSSR.TabIndex = 4;
            this.btnSSR.Text = "SSR";
            this.btnSSR.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(329, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Total:";
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Location = new System.Drawing.Point(376, 9);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(11, 12);
            this.labelTotal.TabIndex = 6;
            this.labelTotal.Text = "0";
            // 
            // FormExtractLinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 210);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnV2ray);
            this.Controls.Add(this.btnVmess);
            this.Controls.Add(this.btnSSR);
            this.Controls.Add(this.btnSS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rTboxOutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rTboxInput);
            this.Name = "FormExtractLinks";
            this.Text = "FormExtractLinks";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rTboxInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rTboxOutput;
        private System.Windows.Forms.Button btnSS;
        private System.Windows.Forms.Button btnVmess;
        private System.Windows.Forms.Button btnV2ray;
        private System.Windows.Forms.Button btnSSR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelTotal;
    }
}