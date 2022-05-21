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
            this.comboYz = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textPx = new System.Windows.Forms.TextBox();
            this.textPy = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBili = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonYLT = new System.Windows.Forms.Button();
            this.comboBoxBL = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textWzbl = new System.Windows.Forms.TextBox();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.comboQfz = new System.Windows.Forms.ComboBox();
            this.comboQmtype = new System.Windows.Forms.ComboBox();
            this.textname = new System.Windows.Forms.TextBox();
            this.labelname = new System.Windows.Forms.Label();
            this.textpass = new System.Windows.Forms.TextBox();
            this.labelpass = new System.Windows.Forms.Label();
            this.comboPDFlist = new System.Windows.Forms.ComboBox();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.labelPage = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textRotation = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_gz
            // 
            this.bt_gz.Location = new System.Drawing.Point(370, 198);
            this.bt_gz.Name = "bt_gz";
            this.bt_gz.Size = new System.Drawing.Size(65, 23);
            this.bt_gz.TabIndex = 0;
            this.bt_gz.Text = "盖章";
            this.bt_gz.UseVisualStyleBackColor = true;
            this.bt_gz.Click += new System.EventHandler(this.button1_Click);
            // 
            // pathText
            // 
            this.pathText.AllowDrop = true;
            this.pathText.Location = new System.Drawing.Point(96, 57);
            this.pathText.Name = "pathText";
            this.pathText.ReadOnly = true;
            this.pathText.Size = new System.Drawing.Size(339, 21);
            this.pathText.TabIndex = 1;
            this.pathText.DragDrop += new System.Windows.Forms.DragEventHandler(this.pathText_DragDrop);
            this.pathText.DragEnter += new System.Windows.Forms.DragEventHandler(this.pathText_DragEnter);
            // 
            // SelectPath
            // 
            this.SelectPath.Location = new System.Drawing.Point(12, 56);
            this.SelectPath.Name = "SelectPath";
            this.SelectPath.Size = new System.Drawing.Size(75, 23);
            this.SelectPath.TabIndex = 2;
            this.SelectPath.Text = "选择";
            this.SelectPath.UseVisualStyleBackColor = true;
            this.SelectPath.Click += new System.EventHandler(this.SelectPath_Click);
            // 
            // textBCpath
            // 
            this.textBCpath.Location = new System.Drawing.Point(97, 100);
            this.textBCpath.Name = "textBCpath";
            this.textBCpath.ReadOnly = true;
            this.textBCpath.Size = new System.Drawing.Size(338, 21);
            this.textBCpath.TabIndex = 3;
            // 
            // textGZpath
            // 
            this.textGZpath.AllowDrop = true;
            this.textGZpath.Location = new System.Drawing.Point(96, 144);
            this.textGZpath.Name = "textGZpath";
            this.textGZpath.ReadOnly = true;
            this.textGZpath.Size = new System.Drawing.Size(339, 21);
            this.textGZpath.TabIndex = 4;
            this.textGZpath.DragDrop += new System.Windows.Forms.DragEventHandler(this.textGZpath_DragDrop);
            this.textGZpath.DragEnter += new System.Windows.Forms.DragEventHandler(this.textGZpath_DragEnter);
            // 
            // OutPath
            // 
            this.OutPath.Location = new System.Drawing.Point(12, 99);
            this.OutPath.Name = "OutPath";
            this.OutPath.Size = new System.Drawing.Size(75, 23);
            this.OutPath.TabIndex = 5;
            this.OutPath.Text = "选择";
            this.OutPath.UseVisualStyleBackColor = true;
            this.OutPath.Click += new System.EventHandler(this.OutPath_Click);
            // 
            // GzPath
            // 
            this.GzPath.Location = new System.Drawing.Point(12, 143);
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
            this.label1.Location = new System.Drawing.Point(10, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "请选择需要盖章的PDF文件(支持多选)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "请选择PDF盖章后所保存的目录";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "请选择印章文件";
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(12, 226);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.log.Size = new System.Drawing.Size(423, 121);
            this.log.TabIndex = 10;
            this.log.Text = "提示:印章图片默认是72dpi(那40mm印章对应的像素就是113),打印效果会很模糊,建议使用300dpi以上的印章图片然后调整印章比例,如300dpi(40m" +
    "m印章对应像素472)对应的比例是72/300=24%,所以比例直接填写24即可";
            // 
            // comboYz
            // 
            this.comboYz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboYz.FormattingEnabled = true;
            this.comboYz.Items.AddRange(new object[] {
            "不加印章",
            "尾页加印章",
            "首页加印章",
            "所有页加印章",
            "自定义加印章"});
            this.comboYz.Location = new System.Drawing.Point(290, 12);
            this.comboYz.Name = "comboYz";
            this.comboYz.Size = new System.Drawing.Size(145, 20);
            this.comboYz.TabIndex = 12;
            this.comboYz.SelectionChangeCommitted += new System.EventHandler(this.comboYz_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(490, 305);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "X:";
            // 
            // textPx
            // 
            this.textPx.Location = new System.Drawing.Point(513, 301);
            this.textPx.Name = "textPx";
            this.textPx.ReadOnly = true;
            this.textPx.Size = new System.Drawing.Size(60, 21);
            this.textPx.TabIndex = 15;
            this.textPx.Text = "0.77";
            // 
            // textPy
            // 
            this.textPy.Location = new System.Drawing.Point(609, 301);
            this.textPy.Name = "textPy";
            this.textPy.ReadOnly = true;
            this.textPy.Size = new System.Drawing.Size(60, 21);
            this.textPy.TabIndex = 16;
            this.textPy.Text = "0.88";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(586, 305);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "Y:";
            // 
            // textBili
            // 
            this.textBili.Location = new System.Drawing.Point(96, 198);
            this.textBili.Name = "textBili";
            this.textBili.Size = new System.Drawing.Size(23, 21);
            this.textBili.TabIndex = 19;
            this.textBili.Text = "40";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(125, 202);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "mm";
            // 
            // buttonYLT
            // 
            this.buttonYLT.Location = new System.Drawing.Point(525, 300);
            this.buttonYLT.Name = "buttonYLT";
            this.buttonYLT.Size = new System.Drawing.Size(109, 23);
            this.buttonYLT.TabIndex = 21;
            this.buttonYLT.Text = "设置自定义预览图";
            this.buttonYLT.UseVisualStyleBackColor = true;
            this.buttonYLT.Visible = false;
            this.buttonYLT.Click += new System.EventHandler(this.buttonYLT_Click);
            // 
            // comboBoxBL
            // 
            this.comboBoxBL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBL.FormattingEnabled = true;
            this.comboBoxBL.Items.AddRange(new object[] {
            "印章比例",
            "印章尺寸"});
            this.comboBoxBL.Location = new System.Drawing.Point(13, 199);
            this.comboBoxBL.Name = "comboBoxBL";
            this.comboBoxBL.Size = new System.Drawing.Size(74, 20);
            this.comboBoxBL.TabIndex = 22;
            this.comboBoxBL.SelectionChangeCommitted += new System.EventHandler(this.comboBoxBL_SelectionChangeCommitted);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(241, 203);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 23;
            this.label6.Text = "骑缝章位置";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(339, 203);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 12);
            this.label8.TabIndex = 25;
            this.label8.Text = "%";
            // 
            // textWzbl
            // 
            this.textWzbl.Location = new System.Drawing.Point(310, 199);
            this.textWzbl.Name = "textWzbl";
            this.textWzbl.Size = new System.Drawing.Size(23, 21);
            this.textWzbl.TabIndex = 24;
            this.textWzbl.Text = "50";
            // 
            // comboType
            // 
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.FormattingEnabled = true;
            this.comboType.Items.AddRange(new object[] {
            "目录模式",
            "文件模式"});
            this.comboType.Location = new System.Drawing.Point(13, 12);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(120, 20);
            this.comboType.TabIndex = 26;
            this.comboType.SelectionChangeCommitted += new System.EventHandler(this.comboType_SelectionChangeCommitted);
            // 
            // comboQfz
            // 
            this.comboQfz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboQfz.FormattingEnabled = true;
            this.comboQfz.Items.AddRange(new object[] {
            "加盖骑缝章",
            "不加骑缝章"});
            this.comboQfz.Location = new System.Drawing.Point(139, 12);
            this.comboQfz.Name = "comboQfz";
            this.comboQfz.Size = new System.Drawing.Size(145, 20);
            this.comboQfz.TabIndex = 27;
            // 
            // comboQmtype
            // 
            this.comboQmtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboQmtype.FormattingEnabled = true;
            this.comboQmtype.Items.AddRange(new object[] {
            "不使用数字签名",
            "自生成证书签名",
            "自定义证书签名"});
            this.comboQmtype.Location = new System.Drawing.Point(12, 173);
            this.comboQmtype.Name = "comboQmtype";
            this.comboQmtype.Size = new System.Drawing.Size(111, 20);
            this.comboQmtype.TabIndex = 28;
            this.comboQmtype.SelectionChangeCommitted += new System.EventHandler(this.comboQmtype_SelectionChangeCommitted);
            // 
            // textname
            // 
            this.textname.Location = new System.Drawing.Point(164, 172);
            this.textname.Name = "textname";
            this.textname.ReadOnly = true;
            this.textname.Size = new System.Drawing.Size(110, 21);
            this.textname.TabIndex = 30;
            // 
            // labelname
            // 
            this.labelname.AutoSize = true;
            this.labelname.Location = new System.Drawing.Point(129, 176);
            this.labelname.Name = "labelname";
            this.labelname.Size = new System.Drawing.Size(29, 12);
            this.labelname.TabIndex = 29;
            this.labelname.Text = "签名";
            // 
            // textpass
            // 
            this.textpass.Location = new System.Drawing.Point(315, 172);
            this.textpass.Name = "textpass";
            this.textpass.PasswordChar = '*';
            this.textpass.ReadOnly = true;
            this.textpass.Size = new System.Drawing.Size(120, 21);
            this.textpass.TabIndex = 32;
            // 
            // labelpass
            // 
            this.labelpass.AutoSize = true;
            this.labelpass.Location = new System.Drawing.Point(280, 176);
            this.labelpass.Name = "labelpass";
            this.labelpass.Size = new System.Drawing.Size(29, 12);
            this.labelpass.TabIndex = 31;
            this.labelpass.Text = "密码";
            // 
            // comboPDFlist
            // 
            this.comboPDFlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPDFlist.FormattingEnabled = true;
            this.comboPDFlist.Location = new System.Drawing.Point(458, 12);
            this.comboPDFlist.Name = "comboPDFlist";
            this.comboPDFlist.Size = new System.Drawing.Size(244, 20);
            this.comboPDFlist.TabIndex = 33;
            this.comboPDFlist.SelectionChangeCommitted += new System.EventHandler(this.comboPDFlist_SelectionChangeCommitted);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(619, 328);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(50, 23);
            this.buttonNext.TabIndex = 34;
            this.buttonNext.Text = "下一页";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(492, 328);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(53, 23);
            this.buttonUp.TabIndex = 35;
            this.buttonUp.Text = "上一页";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // labelPage
            // 
            this.labelPage.AutoSize = true;
            this.labelPage.Location = new System.Drawing.Point(571, 333);
            this.labelPage.Name = "labelPage";
            this.labelPage.Size = new System.Drawing.Size(23, 12);
            this.labelPage.TabIndex = 36;
            this.labelPage.Text = "0/0";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(619, 253);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 37;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox2.DoubleClick += new System.EventHandler(this.pictureBox2_DoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(492, 43);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(177, 250);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // textRotation
            // 
            this.textRotation.Location = new System.Drawing.Point(204, 200);
            this.textRotation.Name = "textRotation";
            this.textRotation.Size = new System.Drawing.Size(23, 21);
            this.textRotation.TabIndex = 39;
            this.textRotation.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(148, 204);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 38;
            this.label10.Text = "旋转角度";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 361);
            this.Controls.Add(this.textRotation);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.labelPage);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.comboPDFlist);
            this.Controls.Add(this.textpass);
            this.Controls.Add(this.labelpass);
            this.Controls.Add(this.textname);
            this.Controls.Add(this.labelname);
            this.Controls.Add(this.comboQmtype);
            this.Controls.Add(this.comboQfz);
            this.Controls.Add(this.comboType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textWzbl);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxBL);
            this.Controls.Add(this.buttonYLT);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBili);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textPy);
            this.Controls.Add(this.textPx);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboYz);
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
            this.Text = "PDF加盖骑缝章 V1.9";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
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
        private System.Windows.Forms.ComboBox comboYz;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textPx;
        private System.Windows.Forms.TextBox textPy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBili;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonYLT;
        private System.Windows.Forms.ComboBox comboBoxBL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textWzbl;
        private System.Windows.Forms.ComboBox comboType;
        private System.Windows.Forms.ComboBox comboQfz;
        private System.Windows.Forms.ComboBox comboQmtype;
        private System.Windows.Forms.TextBox textname;
        private System.Windows.Forms.Label labelname;
        private System.Windows.Forms.TextBox textpass;
        private System.Windows.Forms.Label labelpass;
        private System.Windows.Forms.ComboBox comboPDFlist;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Label labelPage;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox textRotation;
        private System.Windows.Forms.Label label10;
    }
}

