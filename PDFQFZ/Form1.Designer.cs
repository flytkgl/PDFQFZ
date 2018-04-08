namespace PDFQFZ
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.bt_gz = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.pathText = new System.Windows.Forms.TextBox();
            this.SelectPath = new System.Windows.Forms.Button();
            this.textBCpath = new System.Windows.Forms.TextBox();
            this.textGZpath = new System.Windows.Forms.TextBox();
            this.OutPath = new System.Windows.Forms.Button();
            this.GzPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.log = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bt_gz
            // 
            this.bt_gz.Location = new System.Drawing.Point(194, 142);
            this.bt_gz.Name = "bt_gz";
            this.bt_gz.Size = new System.Drawing.Size(75, 23);
            this.bt_gz.TabIndex = 0;
            this.bt_gz.Text = "盖章";
            this.bt_gz.UseVisualStyleBackColor = true;
            this.bt_gz.Click += new System.EventHandler(this.button1_Click);
            // 
            // pathText
            // 
            this.pathText.Location = new System.Drawing.Point(15, 28);
            this.pathText.Name = "pathText";
            this.pathText.Size = new System.Drawing.Size(339, 21);
            this.pathText.TabIndex = 1;
            // 
            // SelectPath
            // 
            this.SelectPath.Location = new System.Drawing.Point(372, 28);
            this.SelectPath.Name = "SelectPath";
            this.SelectPath.Size = new System.Drawing.Size(75, 23);
            this.SelectPath.TabIndex = 2;
            this.SelectPath.Text = "选择";
            this.SelectPath.UseVisualStyleBackColor = true;
            this.SelectPath.Click += new System.EventHandler(this.SelectPath_Click);
            // 
            // textBCpath
            // 
            this.textBCpath.Location = new System.Drawing.Point(15, 72);
            this.textBCpath.Name = "textBCpath";
            this.textBCpath.Size = new System.Drawing.Size(339, 21);
            this.textBCpath.TabIndex = 3;
            // 
            // textGZpath
            // 
            this.textGZpath.Location = new System.Drawing.Point(15, 115);
            this.textGZpath.Name = "textGZpath";
            this.textGZpath.Size = new System.Drawing.Size(339, 21);
            this.textGZpath.TabIndex = 4;
            // 
            // OutPath
            // 
            this.OutPath.Location = new System.Drawing.Point(373, 72);
            this.OutPath.Name = "OutPath";
            this.OutPath.Size = new System.Drawing.Size(75, 23);
            this.OutPath.TabIndex = 5;
            this.OutPath.Text = "选择";
            this.OutPath.UseVisualStyleBackColor = true;
            this.OutPath.Click += new System.EventHandler(this.OutPath_Click);
            // 
            // GzPath
            // 
            this.GzPath.Location = new System.Drawing.Point(373, 115);
            this.GzPath.Name = "GzPath";
            this.GzPath.Size = new System.Drawing.Size(75, 23);
            this.GzPath.TabIndex = 6;
            this.GzPath.Text = "选择";
            this.GzPath.UseVisualStyleBackColor = true;
            this.GzPath.Click += new System.EventHandler(this.GzPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "请选择需要盖章的PDF文件所在目录";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "请选择PDF盖章后想保存在的目录";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "请选择印章文件";
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(12, 171);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.log.Size = new System.Drawing.Size(435, 108);
            this.log.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 291);
            this.Controls.Add(this.log);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GzPath);
            this.Controls.Add(this.OutPath);
            this.Controls.Add(this.textGZpath);
            this.Controls.Add(this.textBCpath);
            this.Controls.Add(this.SelectPath);
            this.Controls.Add(this.pathText);
            this.Controls.Add(this.bt_gz);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "PDF加盖骑缝章";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_gz;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox pathText;
        private System.Windows.Forms.Button SelectPath;
        private System.Windows.Forms.TextBox textBCpath;
        private System.Windows.Forms.TextBox textGZpath;
        private System.Windows.Forms.Button OutPath;
        private System.Windows.Forms.Button GzPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox log;
    }
}

