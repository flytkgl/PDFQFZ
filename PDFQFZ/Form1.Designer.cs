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
            this.clear = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textPx = new System.Windows.Forms.TextBox();
            this.textPy = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBili = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonYLT = new System.Windows.Forms.Button();
            this.comboBoxBL = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_gz
            // 
            this.bt_gz.Location = new System.Drawing.Point(299, 143);
            this.bt_gz.Name = "bt_gz";
            this.bt_gz.Size = new System.Drawing.Size(65, 23);
            this.bt_gz.TabIndex = 0;
            this.bt_gz.Text = "盖章";
            this.bt_gz.UseVisualStyleBackColor = true;
            this.bt_gz.Click += new System.EventHandler(this.button1_Click);
            // 
            // pathText
            // 
            this.pathText.Location = new System.Drawing.Point(96, 29);
            this.pathText.Name = "pathText";
            this.pathText.ReadOnly = true;
            this.pathText.Size = new System.Drawing.Size(339, 21);
            this.pathText.TabIndex = 1;
            // 
            // SelectPath
            // 
            this.SelectPath.Location = new System.Drawing.Point(12, 28);
            this.SelectPath.Name = "SelectPath";
            this.SelectPath.Size = new System.Drawing.Size(75, 23);
            this.SelectPath.TabIndex = 2;
            this.SelectPath.Text = "选择";
            this.SelectPath.UseVisualStyleBackColor = true;
            this.SelectPath.Click += new System.EventHandler(this.SelectPath_Click);
            // 
            // textBCpath
            // 
            this.textBCpath.Location = new System.Drawing.Point(97, 72);
            this.textBCpath.Name = "textBCpath";
            this.textBCpath.ReadOnly = true;
            this.textBCpath.Size = new System.Drawing.Size(338, 21);
            this.textBCpath.TabIndex = 3;
            // 
            // textGZpath
            // 
            this.textGZpath.Location = new System.Drawing.Point(96, 116);
            this.textGZpath.Name = "textGZpath";
            this.textGZpath.ReadOnly = true;
            this.textGZpath.Size = new System.Drawing.Size(339, 21);
            this.textGZpath.TabIndex = 4;
            // 
            // OutPath
            // 
            this.OutPath.Location = new System.Drawing.Point(12, 71);
            this.OutPath.Name = "OutPath";
            this.OutPath.Size = new System.Drawing.Size(75, 23);
            this.OutPath.TabIndex = 5;
            this.OutPath.Text = "选择";
            this.OutPath.UseVisualStyleBackColor = true;
            this.OutPath.Click += new System.EventHandler(this.OutPath_Click);
            // 
            // GzPath
            // 
            this.GzPath.Location = new System.Drawing.Point(12, 115);
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
            this.label1.Location = new System.Drawing.Point(10, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "请选择需要盖章的PDF文件所在目录";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "请选择PDF盖章后想保存在的目录";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 100);
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
            this.log.Size = new System.Drawing.Size(423, 148);
            this.log.TabIndex = 10;
            this.log.Text = "提示:印章图片默认是72dpi(那40mm印章对应的像素就是113),打印效果会很模糊,建议使用300dpi以上的印章图片然后调整印章比例,如300dpi(40m" +
    "m印章对应像素472)对应的比例是72/300=24%,所以比例直接填写24即可,如果想直接设置图片尺寸也是可以的,但是要注意只支持正方形的图片";
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(370, 143);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(65, 23);
            this.clear.TabIndex = 11;
            this.clear.Text = "清空";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "只盖骑缝章(单页无效)",
            "尾页加印章",
            "首页加印章",
            "所有页加印章"});
            this.comboBox1.Location = new System.Drawing.Point(148, 144);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(145, 20);
            this.comboBox1.TabIndex = 12;
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PDFQFZ.Properties.Resources.ylt;
            this.pictureBox1.Location = new System.Drawing.Point(492, 43);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(177, 250);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(490, 302);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "X:";
            // 
            // textPx
            // 
            this.textPx.Location = new System.Drawing.Point(513, 298);
            this.textPx.Name = "textPx";
            this.textPx.ReadOnly = true;
            this.textPx.Size = new System.Drawing.Size(60, 21);
            this.textPx.TabIndex = 15;
            this.textPx.Text = "0";
            // 
            // textPy
            // 
            this.textPy.Location = new System.Drawing.Point(609, 298);
            this.textPy.Name = "textPy";
            this.textPy.ReadOnly = true;
            this.textPy.Size = new System.Drawing.Size(60, 21);
            this.textPy.TabIndex = 16;
            this.textPy.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(586, 302);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "Y:";
            // 
            // textBili
            // 
            this.textBili.Location = new System.Drawing.Point(96, 143);
            this.textBili.Name = "textBili";
            this.textBili.Size = new System.Drawing.Size(23, 21);
            this.textBili.TabIndex = 19;
            this.textBili.Text = "100";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(125, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "%";
            // 
            // buttonYLT
            // 
            this.buttonYLT.Location = new System.Drawing.Point(513, 14);
            this.buttonYLT.Name = "buttonYLT";
            this.buttonYLT.Size = new System.Drawing.Size(132, 23);
            this.buttonYLT.TabIndex = 21;
            this.buttonYLT.Text = "设置自定义预览图";
            this.buttonYLT.UseVisualStyleBackColor = true;
            this.buttonYLT.Click += new System.EventHandler(this.buttonYLT_Click);
            // 
            // comboBoxBL
            // 
            this.comboBoxBL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBL.FormattingEnabled = true;
            this.comboBoxBL.Items.AddRange(new object[] {
            "印章比例",
            "印章尺寸"});
            this.comboBoxBL.Location = new System.Drawing.Point(13, 144);
            this.comboBoxBL.Name = "comboBoxBL";
            this.comboBoxBL.Size = new System.Drawing.Size(74, 20);
            this.comboBoxBL.TabIndex = 22;
            this.comboBoxBL.SelectionChangeCommitted += new System.EventHandler(this.comboBoxBL_SelectionChangeCommitted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 331);
            this.Controls.Add(this.comboBoxBL);
            this.Controls.Add(this.buttonYLT);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBili);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textPy);
            this.Controls.Add(this.textPx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.clear);
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
            this.Text = "PDF加盖骑缝章 V1.2";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textPx;
        private System.Windows.Forms.TextBox textPy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBili;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonYLT;
        private System.Windows.Forms.ComboBox comboBoxBL;
    }
}

