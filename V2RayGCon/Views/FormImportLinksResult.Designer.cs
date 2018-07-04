namespace V2RayGCon.Views
{
    partial class FormImportLinksResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportLinksResult));
            this.lvResult = new System.Windows.Forms.ListView();
            this.index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Link = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.msg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvResult
            // 
            resources.ApplyResources(this.lvResult, "lvResult");
            this.lvResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.index,
            this.Link,
            this.status,
            this.msg});
            this.lvResult.FullRowSelect = true;
            this.lvResult.GridLines = true;
            this.lvResult.HideSelection = false;
            this.lvResult.Name = "lvResult";
            this.lvResult.UseCompatibleStateImageBehavior = false;
            this.lvResult.View = System.Windows.Forms.View.Details;
            this.lvResult.Click += new System.EventHandler(this.lvResult_Click);
            // 
            // index
            // 
            resources.ApplyResources(this.index, "index");
            // 
            // Link
            // 
            resources.ApplyResources(this.Link, "Link");
            // 
            // status
            // 
            resources.ApplyResources(this.status, "status");
            // 
            // msg
            // 
            resources.ApplyResources(this.msg, "msg");
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCopy
            // 
            resources.ApplyResources(this.btnCopy, "btnCopy");
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // FormImportLinksResult
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvResult);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImportLinksResult";
            this.Shown += new System.EventHandler(this.FormImportLinksResult_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvResult;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ColumnHeader index;
        private System.Windows.Forms.ColumnHeader Link;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.ColumnHeader msg;
        private System.Windows.Forms.Button btnCopy;
    }
}