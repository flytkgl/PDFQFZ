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
            this.OutPath = new System.Windows.Forms.Button();
            this.GzPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.log = new System.Windows.Forms.TextBox();
            this.comboYz = new System.Windows.Forms.ComboBox();
            this.textPx = new System.Windows.Forms.TextBox();
            this.textPy = new System.Windows.Forms.TextBox();
            this.textCC = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxWZ = new System.Windows.Forms.ComboBox();
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
            this.label9 = new System.Windows.Forms.Label();
            this.textOpacity = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboDJ = new System.Windows.Forms.ComboBox();
            this.isSaveSources = new System.Windows.Forms.CheckBox();
            this.comboBoxYz = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textMaxFgs = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBoxQB = new System.Windows.Forms.ComboBox();
            this.textpdfpass = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBoxPages = new System.Windows.Forms.ComboBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.checkMultiple = new System.Windows.Forms.CheckBox();
            this.checkRandom = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_gz
            // 
            this.bt_gz.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_gz.Location = new System.Drawing.Point(201, 388);
            this.bt_gz.Name = "bt_gz";
            this.bt_gz.Size = new System.Drawing.Size(75, 36);
            this.bt_gz.TabIndex = 0;
            this.bt_gz.Text = "盖章";
            this.bt_gz.UseVisualStyleBackColor = true;
            this.bt_gz.Click += new System.EventHandler(this.button1_Click);
            // 
            // pathText
            // 
            this.pathText.AllowDrop = true;
            this.pathText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pathText.Location = new System.Drawing.Point(97, 73);
            this.pathText.Name = "pathText";
            this.pathText.ReadOnly = true;
            this.pathText.Size = new System.Drawing.Size(357, 26);
            this.pathText.TabIndex = 1;
            this.pathText.DragDrop += new System.Windows.Forms.DragEventHandler(this.pathText_DragDrop);
            this.pathText.DragEnter += new System.Windows.Forms.DragEventHandler(this.pathText_DragEnter);
            // 
            // SelectPath
            // 
            this.SelectPath.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SelectPath.Location = new System.Drawing.Point(13, 71);
            this.SelectPath.Name = "SelectPath";
            this.SelectPath.Size = new System.Drawing.Size(75, 30);
            this.SelectPath.TabIndex = 2;
            this.SelectPath.Text = "选择";
            this.SelectPath.UseVisualStyleBackColor = true;
            this.SelectPath.Click += new System.EventHandler(this.SelectPath_Click);
            // 
            // textBCpath
            // 
            this.textBCpath.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBCpath.Location = new System.Drawing.Point(98, 128);
            this.textBCpath.Name = "textBCpath";
            this.textBCpath.ReadOnly = true;
            this.textBCpath.Size = new System.Drawing.Size(356, 26);
            this.textBCpath.TabIndex = 3;
            // 
            // OutPath
            // 
            this.OutPath.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OutPath.Location = new System.Drawing.Point(13, 126);
            this.OutPath.Name = "OutPath";
            this.OutPath.Size = new System.Drawing.Size(75, 30);
            this.OutPath.TabIndex = 5;
            this.OutPath.Text = "选择";
            this.OutPath.UseVisualStyleBackColor = true;
            this.OutPath.Click += new System.EventHandler(this.OutPath_Click);
            // 
            // GzPath
            // 
            this.GzPath.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GzPath.Location = new System.Drawing.Point(379, 182);
            this.GzPath.Name = "GzPath";
            this.GzPath.Size = new System.Drawing.Size(75, 30);
            this.GzPath.TabIndex = 6;
            this.GzPath.Text = "导入";
            this.GzPath.UseVisualStyleBackColor = true;
            this.GzPath.Click += new System.EventHandler(this.GzPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(11, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "请选择需要盖章的PDF文件(支持多选)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(11, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "请选择PDF盖章后所保存的目录";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(11, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "请选择印章文件";
            // 
            // log
            // 
            this.log.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.log.Location = new System.Drawing.Point(13, 430);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.log.Size = new System.Drawing.Size(441, 119);
            this.log.TabIndex = 10;
            this.log.Text = "提示:建议使用472像素以上且背景透明的印章图片.\r\n使用合并模式会导致文字不可编辑,并且原数字签名丢失.随意骑缝章和自定义加印章共用右边的预览定位,所以同时使用" +
    "的时候会冲突,建议分开盖章.";
            // 
            // comboYz
            // 
            this.comboYz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboYz.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboYz.FormattingEnabled = true;
            this.comboYz.Items.AddRange(new object[] {
            "全都不加印章",
            "首页不加印章",
            "尾页不加印章",
            "所有页加印章",
            "自定义加印章"});
            this.comboYz.Location = new System.Drawing.Point(240, 12);
            this.comboYz.Name = "comboYz";
            this.comboYz.Size = new System.Drawing.Size(113, 28);
            this.comboYz.TabIndex = 12;
            this.comboYz.SelectedIndexChanged += new System.EventHandler(this.comboYz_SelectedIndexChanged);
            this.comboYz.SelectionChangeCommitted += new System.EventHandler(this.comboYz_SelectionChangeCommitted);
            // 
            // textPx
            // 
            this.textPx.Location = new System.Drawing.Point(379, 399);
            this.textPx.Name = "textPx";
            this.textPx.ReadOnly = true;
            this.textPx.Size = new System.Drawing.Size(34, 21);
            this.textPx.TabIndex = 15;
            this.textPx.Text = "0.77";
            this.textPx.Visible = false;
            // 
            // textPy
            // 
            this.textPy.Location = new System.Drawing.Point(419, 399);
            this.textPy.Name = "textPy";
            this.textPy.ReadOnly = true;
            this.textPy.Size = new System.Drawing.Size(35, 21);
            this.textPy.TabIndex = 16;
            this.textPy.Text = "0.88";
            this.textPy.Visible = false;
            // 
            // textCC
            // 
            this.textCC.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textCC.Location = new System.Drawing.Point(84, 289);
            this.textCC.Name = "textCC";
            this.textCC.Size = new System.Drawing.Size(42, 26);
            this.textCC.TabIndex = 19;
            this.textCC.Text = "40";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(126, 292);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 20);
            this.label7.TabIndex = 20;
            this.label7.Text = "mm";
            // 
            // comboBoxWZ
            // 
            this.comboBoxWZ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWZ.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxWZ.FormattingEnabled = true;
            this.comboBoxWZ.Items.AddRange(new object[] {
            "上",
            "下",
            "左",
            "右"});
            this.comboBoxWZ.Location = new System.Drawing.Point(94, 322);
            this.comboBoxWZ.Name = "comboBoxWZ";
            this.comboBoxWZ.Size = new System.Drawing.Size(65, 28);
            this.comboBoxWZ.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(9, 325);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 23;
            this.label6.Text = "骑缝章位置";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(213, 325);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 20);
            this.label8.TabIndex = 25;
            this.label8.Text = "%";
            // 
            // textWzbl
            // 
            this.textWzbl.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textWzbl.Location = new System.Drawing.Point(165, 323);
            this.textWzbl.Name = "textWzbl";
            this.textWzbl.Size = new System.Drawing.Size(42, 26);
            this.textWzbl.TabIndex = 24;
            this.textWzbl.Text = "50";
            // 
            // comboType
            // 
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboType.FormattingEnabled = true;
            this.comboType.Items.AddRange(new object[] {
            "目录模式",
            "文件模式"});
            this.comboType.Location = new System.Drawing.Point(13, 12);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(103, 28);
            this.comboType.TabIndex = 26;
            this.comboType.SelectionChangeCommitted += new System.EventHandler(this.comboType_SelectionChangeCommitted);
            // 
            // comboQfz
            // 
            this.comboQfz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboQfz.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboQfz.FormattingEnabled = true;
            this.comboQfz.Items.AddRange(new object[] {
            "加盖骑缝章",
            "不加骑缝章",
            "单页骑缝章",
            "双页骑缝章",
            "随意骑缝章"});
            this.comboQfz.Location = new System.Drawing.Point(122, 12);
            this.comboQfz.Name = "comboQfz";
            this.comboQfz.Size = new System.Drawing.Size(112, 28);
            this.comboQfz.TabIndex = 27;
            this.comboQfz.SelectedIndexChanged += new System.EventHandler(this.comboQfz_SelectedIndexChanged);
            this.comboQfz.SelectionChangeCommitted += new System.EventHandler(this.comboQfz_SelectionChangeCommitted);
            // 
            // comboQmtype
            // 
            this.comboQmtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboQmtype.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboQmtype.FormattingEnabled = true;
            this.comboQmtype.Items.AddRange(new object[] {
            "不使用数字签名",
            "自生成证书签名",
            "自定义证书签名"});
            this.comboQmtype.Location = new System.Drawing.Point(13, 218);
            this.comboQmtype.Name = "comboQmtype";
            this.comboQmtype.Size = new System.Drawing.Size(148, 28);
            this.comboQmtype.TabIndex = 28;
            this.comboQmtype.SelectionChangeCommitted += new System.EventHandler(this.comboQmtype_SelectionChangeCommitted);
            // 
            // textname
            // 
            this.textname.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textname.Location = new System.Drawing.Point(56, 253);
            this.textname.Name = "textname";
            this.textname.ReadOnly = true;
            this.textname.Size = new System.Drawing.Size(178, 26);
            this.textname.TabIndex = 30;
            // 
            // labelname
            // 
            this.labelname.AutoSize = true;
            this.labelname.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelname.Location = new System.Drawing.Point(9, 256);
            this.labelname.Name = "labelname";
            this.labelname.Size = new System.Drawing.Size(37, 20);
            this.labelname.TabIndex = 29;
            this.labelname.Text = "签名";
            // 
            // textpass
            // 
            this.textpass.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textpass.Location = new System.Drawing.Point(283, 253);
            this.textpass.Name = "textpass";
            this.textpass.PasswordChar = '*';
            this.textpass.ReadOnly = true;
            this.textpass.Size = new System.Drawing.Size(171, 26);
            this.textpass.TabIndex = 32;
            // 
            // labelpass
            // 
            this.labelpass.AutoSize = true;
            this.labelpass.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelpass.Location = new System.Drawing.Point(240, 256);
            this.labelpass.Name = "labelpass";
            this.labelpass.Size = new System.Drawing.Size(37, 20);
            this.labelpass.TabIndex = 31;
            this.labelpass.Text = "密码";
            // 
            // comboPDFlist
            // 
            this.comboPDFlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPDFlist.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboPDFlist.FormattingEnabled = true;
            this.comboPDFlist.Location = new System.Drawing.Point(472, 12);
            this.comboPDFlist.Name = "comboPDFlist";
            this.comboPDFlist.Size = new System.Drawing.Size(247, 28);
            this.comboPDFlist.TabIndex = 33;
            this.comboPDFlist.SelectedIndexChanged += new System.EventHandler(this.comboPDFlist_SelectedIndexChanged);
            this.comboPDFlist.SelectionChangeCommitted += new System.EventHandler(this.comboPDFlist_SelectionChangeCommitted);
            // 
            // buttonNext
            // 
            this.buttonNext.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonNext.Location = new System.Drawing.Point(793, 12);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(60, 30);
            this.buttonNext.TabIndex = 34;
            this.buttonNext.Text = "下一页";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonUp.Location = new System.Drawing.Point(727, 11);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(60, 30);
            this.buttonUp.TabIndex = 35;
            this.buttonUp.Text = "上一页";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // labelPage
            // 
            this.labelPage.AutoSize = true;
            this.labelPage.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPage.Location = new System.Drawing.Point(928, 16);
            this.labelPage.Name = "labelPage";
            this.labelPage.Size = new System.Drawing.Size(31, 20);
            this.labelPage.TabIndex = 36;
            this.labelPage.Text = "0/0";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(790, 400);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(72, 72);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 37;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox2.DoubleClick += new System.EventHandler(this.pictureBox2_DoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(540, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(358, 500);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // textRotation
            // 
            this.textRotation.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textRotation.Location = new System.Drawing.Point(252, 290);
            this.textRotation.Name = "textRotation";
            this.textRotation.Size = new System.Drawing.Size(43, 26);
            this.textRotation.TabIndex = 39;
            this.textRotation.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(215, 293);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 20);
            this.label10.TabIndex = 38;
            this.label10.Text = "旋转";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(297, 292);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 20);
            this.label9.TabIndex = 40;
            this.label9.Text = "°";
            // 
            // textOpacity
            // 
            this.textOpacity.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOpacity.Location = new System.Drawing.Point(335, 323);
            this.textOpacity.Name = "textOpacity";
            this.textOpacity.Size = new System.Drawing.Size(48, 26);
            this.textOpacity.TabIndex = 42;
            this.textOpacity.Text = "100";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(389, 326);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 20);
            this.label12.TabIndex = 41;
            this.label12.Text = "%不透明";
            // 
            // comboDJ
            // 
            this.comboDJ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDJ.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboDJ.FormattingEnabled = true;
            this.comboDJ.Items.AddRange(new object[] {
            "叠加",
            "合并"});
            this.comboDJ.Location = new System.Drawing.Point(359, 12);
            this.comboDJ.Name = "comboDJ";
            this.comboDJ.Size = new System.Drawing.Size(95, 28);
            this.comboDJ.TabIndex = 44;
            // 
            // isSaveSources
            // 
            this.isSaveSources.AutoSize = true;
            this.isSaveSources.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.isSaveSources.Location = new System.Drawing.Point(286, 104);
            this.isSaveSources.Name = "isSaveSources";
            this.isSaveSources.Size = new System.Drawing.Size(168, 24);
            this.isSaveSources.TabIndex = 45;
            this.isSaveSources.Text = "保存在源文件文件夹下";
            this.isSaveSources.UseVisualStyleBackColor = true;
            this.isSaveSources.CheckedChanged += new System.EventHandler(this.SaveSources);
            // 
            // comboBoxYz
            // 
            this.comboBoxYz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxYz.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxYz.FormattingEnabled = true;
            this.comboBoxYz.Items.AddRange(new object[] {
            "目录模式",
            "文件模式"});
            this.comboBoxYz.Location = new System.Drawing.Point(13, 184);
            this.comboBoxYz.Name = "comboBoxYz";
            this.comboBoxYz.Size = new System.Drawing.Size(360, 28);
            this.comboBoxYz.TabIndex = 46;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(8, 292);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 20);
            this.label13.TabIndex = 47;
            this.label13.Text = "印章尺寸";
            // 
            // textMaxFgs
            // 
            this.textMaxFgs.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textMaxFgs.Location = new System.Drawing.Point(135, 355);
            this.textMaxFgs.Name = "textMaxFgs";
            this.textMaxFgs.Size = new System.Drawing.Size(61, 26);
            this.textMaxFgs.TabIndex = 49;
            this.textMaxFgs.Text = "20";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(8, 358);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(121, 20);
            this.label14.TabIndex = 48;
            this.label14.Text = "骑缝章最大分割数";
            // 
            // comboBoxQB
            // 
            this.comboBoxQB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxQB.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxQB.FormattingEnabled = true;
            this.comboBoxQB.Items.AddRange(new object[] {
            "旋转切边",
            "不切边"});
            this.comboBoxQB.Location = new System.Drawing.Point(317, 289);
            this.comboBoxQB.Name = "comboBoxQB";
            this.comboBoxQB.Size = new System.Drawing.Size(137, 28);
            this.comboBoxQB.TabIndex = 50;
            // 
            // textpdfpass
            // 
            this.textpdfpass.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textpdfpass.Location = new System.Drawing.Point(283, 355);
            this.textpdfpass.Name = "textpdfpass";
            this.textpdfpass.PasswordChar = '*';
            this.textpdfpass.Size = new System.Drawing.Size(171, 26);
            this.textpdfpass.TabIndex = 52;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(213, 358);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 20);
            this.label15.TabIndex = 51;
            this.label15.Text = "PDF密码";
            // 
            // comboBoxPages
            // 
            this.comboBoxPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPages.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxPages.FormattingEnabled = true;
            this.comboBoxPages.Items.AddRange(new object[] {
            "叠加",
            "合并"});
            this.comboBoxPages.Location = new System.Drawing.Point(859, 12);
            this.comboBoxPages.Name = "comboBoxPages";
            this.comboBoxPages.Size = new System.Drawing.Size(63, 28);
            this.comboBoxPages.TabIndex = 53;
            this.comboBoxPages.SelectionChangeCommitted += new System.EventHandler(this.comboBoxPages_SelectionChangeCommitted);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(97, 101);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(357, 3);
            this.progressBar1.TabIndex = 56;
            this.progressBar1.Visible = false;
            // 
            // checkMultiple
            // 
            this.checkMultiple.AutoSize = true;
            this.checkMultiple.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkMultiple.Location = new System.Drawing.Point(370, 157);
            this.checkMultiple.Name = "checkMultiple";
            this.checkMultiple.Size = new System.Drawing.Size(84, 24);
            this.checkMultiple.TabIndex = 57;
            this.checkMultiple.Text = "多章连盖";
            this.checkMultiple.UseVisualStyleBackColor = true;
            // 
            // checkRandom
            // 
            this.checkRandom.AutoSize = true;
            this.checkRandom.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkRandom.Location = new System.Drawing.Point(245, 325);
            this.checkRandom.Name = "checkRandom";
            this.checkRandom.Size = new System.Drawing.Size(84, 24);
            this.checkRandom.TabIndex = 58;
            this.checkRandom.Text = "随机参数";
            this.checkRandom.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 561);
            this.Controls.Add(this.checkRandom);
            this.Controls.Add(this.checkMultiple);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.comboBoxPages);
            this.Controls.Add(this.textpdfpass);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.comboBoxQB);
            this.Controls.Add(this.textMaxFgs);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.comboBoxYz);
            this.Controls.Add(this.isSaveSources);
            this.Controls.Add(this.comboDJ);
            this.Controls.Add(this.textOpacity);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label9);
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
            this.Controls.Add(this.comboBoxWZ);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textCC);
            this.Controls.Add(this.textPy);
            this.Controls.Add(this.textPx);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboYz);
            this.Controls.Add(this.log);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GzPath);
            this.Controls.Add(this.OutPath);
            this.Controls.Add(this.textBCpath);
            this.Controls.Add(this.SelectPath);
            this.Controls.Add(this.pathText);
            this.Controls.Add(this.bt_gz);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "PDF加盖骑缝章(V1.31)";
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
        private System.Windows.Forms.Button OutPath;
        private System.Windows.Forms.Button GzPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.ComboBox comboYz;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textPx;
        private System.Windows.Forms.TextBox textPy;
        private System.Windows.Forms.TextBox textCC;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxWZ;
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
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textOpacity;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboDJ;
        private System.Windows.Forms.CheckBox isSaveSources;
        private System.Windows.Forms.ComboBox comboBoxYz;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textMaxFgs;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBoxQB;
        private System.Windows.Forms.TextBox textpdfpass;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBoxPages;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkMultiple;
        private System.Windows.Forms.CheckBox checkRandom;
    }
}

