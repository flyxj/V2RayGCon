namespace Luna.Views.WinForms
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbStatusBarMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.flyScriptUIContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.tabEditor = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboxScriptName = new System.Windows.Forms.ComboBox();
            this.btnClearOutput = new System.Windows.Forms.Button();
            this.btnKillScript = new System.Windows.Forms.Button();
            this.btnStopScript = new System.Windows.Forms.Button();
            this.btnRunScript = new System.Windows.Forms.Button();
            this.btnRemoveScript = new System.Windows.Forms.Button();
            this.btnSaveScript = new System.Windows.Forms.Button();
            this.pnlScriptEditor = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtBoxOutput = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatusBarMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 29);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbStatusBarMsg
            // 
            this.lbStatusBarMsg.Name = "lbStatusBarMsg";
            this.lbStatusBarMsg.Size = new System.Drawing.Size(134, 24);
            this.lbStatusBarMsg.Text = "0 task running";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tabControl1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(800, 396);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(800, 450);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabEditor);
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 396);
            this.tabControl1.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.button2);
            this.tabGeneral.Controls.Add(this.button3);
            this.tabGeneral.Controls.Add(this.button1);
            this.tabGeneral.Controls.Add(this.flyScriptUIContainer);
            this.tabGeneral.Location = new System.Drawing.Point(4, 28);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(792, 364);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(677, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 30);
            this.button2.TabIndex = 1;
            this.button2.Text = "Refresh";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(677, 78);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(109, 30);
            this.button3.TabIndex = 1;
            this.button3.Text = "Kill all";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(677, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Stop all";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // flyScriptUIContainer
            // 
            this.flyScriptUIContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flyScriptUIContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flyScriptUIContainer.Location = new System.Drawing.Point(8, 6);
            this.flyScriptUIContainer.Name = "flyScriptUIContainer";
            this.flyScriptUIContainer.Size = new System.Drawing.Size(661, 352);
            this.flyScriptUIContainer.TabIndex = 0;
            // 
            // tabEditor
            // 
            this.tabEditor.Controls.Add(this.splitContainer1);
            this.tabEditor.Location = new System.Drawing.Point(4, 28);
            this.tabEditor.Name = "tabEditor";
            this.tabEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabEditor.Size = new System.Drawing.Size(792, 364);
            this.tabEditor.TabIndex = 1;
            this.tabEditor.Text = "Editor";
            this.tabEditor.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(786, 358);
            this.splitContainer1.SplitterDistance = 479;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(479, 358);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Editor";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlScriptEditor, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 24);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(473, 331);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboxScriptName);
            this.panel1.Controls.Add(this.btnClearOutput);
            this.panel1.Controls.Add(this.btnKillScript);
            this.panel1.Controls.Add(this.btnStopScript);
            this.panel1.Controls.Add(this.btnRunScript);
            this.panel1.Controls.Add(this.btnRemoveScript);
            this.panel1.Controls.Add(this.btnSaveScript);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(473, 38);
            this.panel1.TabIndex = 1;
            // 
            // cboxScriptName
            // 
            this.cboxScriptName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxScriptName.FormattingEnabled = true;
            this.cboxScriptName.Location = new System.Drawing.Point(3, 6);
            this.cboxScriptName.Name = "cboxScriptName";
            this.cboxScriptName.Size = new System.Drawing.Size(272, 26);
            this.cboxScriptName.TabIndex = 3;
            this.toolTip1.SetToolTip(this.cboxScriptName, "Load script or set new script name.");
            // 
            // btnClearOutput
            // 
            this.btnClearOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearOutput.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClearOutput.BackgroundImage")));
            this.btnClearOutput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClearOutput.Location = new System.Drawing.Point(442, 5);
            this.btnClearOutput.Name = "btnClearOutput";
            this.btnClearOutput.Size = new System.Drawing.Size(28, 28);
            this.btnClearOutput.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnClearOutput, "Clear output");
            this.btnClearOutput.UseVisualStyleBackColor = true;
            // 
            // btnKillScript
            // 
            this.btnKillScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKillScript.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnKillScript.BackgroundImage")));
            this.btnKillScript.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnKillScript.Location = new System.Drawing.Point(410, 5);
            this.btnKillScript.Name = "btnKillScript";
            this.btnKillScript.Size = new System.Drawing.Size(28, 28);
            this.btnKillScript.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnKillScript, "Kill");
            this.btnKillScript.UseVisualStyleBackColor = true;
            // 
            // btnStopScript
            // 
            this.btnStopScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopScript.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStopScript.BackgroundImage")));
            this.btnStopScript.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStopScript.Location = new System.Drawing.Point(378, 5);
            this.btnStopScript.Name = "btnStopScript";
            this.btnStopScript.Size = new System.Drawing.Size(28, 28);
            this.btnStopScript.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnStopScript, "Stop");
            this.btnStopScript.UseVisualStyleBackColor = true;
            // 
            // btnRunScript
            // 
            this.btnRunScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunScript.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRunScript.BackgroundImage")));
            this.btnRunScript.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRunScript.Location = new System.Drawing.Point(346, 5);
            this.btnRunScript.Name = "btnRunScript";
            this.btnRunScript.Size = new System.Drawing.Size(28, 28);
            this.btnRunScript.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnRunScript, "Run");
            this.btnRunScript.UseVisualStyleBackColor = true;
            // 
            // btnRemoveScript
            // 
            this.btnRemoveScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveScript.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRemoveScript.BackgroundImage")));
            this.btnRemoveScript.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRemoveScript.Location = new System.Drawing.Point(314, 5);
            this.btnRemoveScript.Name = "btnRemoveScript";
            this.btnRemoveScript.Size = new System.Drawing.Size(28, 28);
            this.btnRemoveScript.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnRemoveScript, "Delete lua script.");
            this.btnRemoveScript.UseVisualStyleBackColor = true;
            // 
            // btnSaveScript
            // 
            this.btnSaveScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveScript.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveScript.BackgroundImage")));
            this.btnSaveScript.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSaveScript.Location = new System.Drawing.Point(281, 5);
            this.btnSaveScript.Name = "btnSaveScript";
            this.btnSaveScript.Size = new System.Drawing.Size(28, 28);
            this.btnSaveScript.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnSaveScript, "Add new or modify original lua script.");
            this.btnSaveScript.UseVisualStyleBackColor = true;
            // 
            // pnlScriptEditor
            // 
            this.pnlScriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScriptEditor.Location = new System.Drawing.Point(3, 41);
            this.pnlScriptEditor.Name = "pnlScriptEditor";
            this.pnlScriptEditor.Size = new System.Drawing.Size(467, 312);
            this.pnlScriptEditor.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rtBoxOutput);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 358);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // rtBoxOutput
            // 
            this.rtBoxOutput.BackColor = System.Drawing.SystemColors.Control;
            this.rtBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtBoxOutput.Location = new System.Drawing.Point(3, 24);
            this.rtBoxOutput.Name = "rtBoxOutput";
            this.rtBoxOutput.ReadOnly = true;
            this.rtBoxOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtBoxOutput.Size = new System.Drawing.Size(297, 331);
            this.rtBoxOutput.TabIndex = 0;
            this.rtBoxOutput.Text = "";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabEditor.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbStatusBarMsg;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FlowLayoutPanel flyScriptUIContainer;
        private System.Windows.Forms.TabPage tabEditor;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboxScriptName;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnKillScript;
        private System.Windows.Forms.Button btnStopScript;
        private System.Windows.Forms.Button btnRunScript;
        private System.Windows.Forms.Button btnRemoveScript;
        private System.Windows.Forms.Button btnSaveScript;
        private System.Windows.Forms.Panel pnlScriptEditor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtBoxOutput;
        private System.Windows.Forms.Button btnClearOutput;
    }
}
