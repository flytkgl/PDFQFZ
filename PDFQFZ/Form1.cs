using iTextSharp.text;
using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using O2S.Components.PDFRender4NET;
using PDFQFZ.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Collections.Specialized.BitVector32;

namespace PDFQFZ
{
    public partial class Form1 : Form
    {
        int fw;  //Form Width
        int fh;  //Form Height
        int collapsedClientWidth;
        int expandedClientWidth;
        int imgStartPage = 0;
        int imgPageCount=0;

        ToolTip tip = new ToolTip();  //容差的ToolTip显示

        string certDefaultPath = $@"{Application.StartupPath}\pdfqfz.pfx";//证书默认存放地址

        string yzLog = $@"{Application.StartupPath}\yz.log";//获取印章记录

        //@loquat 20250920 增加全局参数
        int fixType;
        string fixStr;
        string fixStr2;
        string signBuiltInPath;
        string signBuiltInPass;
        string signCustomPath;
        string signCustomPass;
        string section = "config";

        DataTable dt = new DataTable();       //PDF列表
        DataTable dtPages = new DataTable();  //PDF文件页列表
        DataTable dtPos = new DataTable();    //PDF各文件印章位置表
        DataTable dtYz = new DataTable();     //印章图片列表
        List<string> commandLinePdfFiles = new List<string>();
        string sourcePath = "";
        string outputPath = "";
        string imgPath = "";
        string previewPath = "";
        string signText = "";
        string password = "";
        string pdfpassword = "";
        //这里设置的默认值是不是没有意义
        int wjType = 1;     //文件类型 文件/目录
        int qfzType = 0;    //骑缝章类型
        int yzType = 0;     //印章类型
        int djType = 0;     //叠加类型
        int qmType = 0;     //签名类型
        int wzType = 3;     //骑缝章位置类型
        int yzIndex = -1;   //选择的印章索引
        int qbflag = 0;     //是否切边标记
        int size = 40;      //尺寸，单位mm，loquat改的自定义签章部分用作宽度，高度维持原图比例
        int rotation = 0;   //旋转角
        int opacity = 60;   //透明度预设
        int wz = 50;        //骑缝章位置
        int yzr = 36;
        int maximg = 500;  //预览区控件的最大尺寸，要么是高，要么是宽
        int maxfgs = 20;   //骑缝章最大分割数
        Bitmap imgYz = null;   //签章图片对象
        Bitmap[] viewPdfimgs = null;         //预览的pdf列表， viewPdfimgs[imgStartPage-1]表示当前在预览的图片
        PageCache<Bitmap> previewPageCache = null;
        PDFFile previewPdfFile = null;
        Bitmap cachedTransparentStamp = null;
        string cachedTransparentStampKey = "";
        readonly object transparentStampCacheSync = new object();
        readonly PreviewOverlayRequestGate previewOverlayRequestGate = new PreviewOverlayRequestGate();
        X509Certificate2 cert = null;        //证书
        float xzbl = 1f;                     //旋转图片导致长宽变化的比例
        private string strIniFilePath = $@"{Application.StartupPath}\config.ini";//获取INI文件路径
        //private bool isSelectionCommitted = false; // 文档预览下拉列表框事件标记位
        private CancellationTokenSource cancellationTokenSource;//处理文件进度取消标记
        private CancellationTokenSource cts;//PDF文件异步处理取消标记
        private CancellationTokenSource previewOverlayCts;//印章预览异步取消标记

        public Form1(string[] args)
        {
            InitializeComponent();
            this.KeyDown += Esc_Key_Down;//接受键盘ESC响应
            // 在这里处理命令行参数
            commandLinePdfFiles = CommandLineFileLoader.CollectExistingPdfFiles(args);
            if (commandLinePdfFiles.Count > 0)
            {
                sourcePath = string.Join(",", commandLinePdfFiles);
                outputPath = System.IO.Path.GetDirectoryName(commandLinePdfFiles[0]);
            }
        }


        //程序加载
        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(strIniFilePath))//读取时先要判读INI文件是否存在
            {
                IniFileHelper iniFileHelper = new IniFileHelper(strIniFilePath);
                string section = "config";
                string WjType = iniFileHelper.ContentValue(section, "wjType");//文件类型
                string QfzType = iniFileHelper.ContentValue(section, "qfzType");//骑缝章类型
                string YzType = iniFileHelper.ContentValue(section, "yzType");//印章类型
                string DjType = iniFileHelper.ContentValue(section, "djType");//叠加类型
                string QmType = iniFileHelper.ContentValue(section, "qmType");//签名类型 0-不签名 1-内置签名 2-自定义签名
                string WzType = iniFileHelper.ContentValue(section, "wzType");//骑缝章位置类型
                string Qbflag = iniFileHelper.ContentValue(section, "qbflag");//是否切边标记
                string Size = iniFileHelper.ContentValue(section, "size");//印章尺寸
                string Rotation = iniFileHelper.ContentValue(section, "rotation");//旋转角度
                string Opacity = iniFileHelper.ContentValue(section, "opacity");//不透明度
                string Wz = iniFileHelper.ContentValue(section, "wz");//骑缝章位置
                string Maxfgs = iniFileHelper.ContentValue(section, "maxfgs");//骑缝章最大分割数
                string YzIndex = iniFileHelper.ContentValue(section, "yzIndex");//选择的印章索引
                signText = iniFileHelper.ContentValue(section, "signText");//签名

                //@loquat 20250920
                string FixType = iniFileHelper.ContentValue(section, "fixType");//输出结果前后缀类型
                fixStr = iniFileHelper.ContentValue(section, "fixStr"); //输出结果前后缀文本
                if (fixStr.Length == 0) { fixStr = "已盖章"; }          //在这里配置默认值
                fixStr2 = iniFileHelper.ContentValue(section, "fixStr2"); //加密后缀
                if (fixStr.Length == 0) { fixStr = "加密"; }            //在这里配置默认值
                fixType = ToIntOrDefault(FixType,0);  //默认值0，不改变原有逻辑
                signBuiltInPath = iniFileHelper.ContentValue(section, "signBuiltInPath");    //内置证书路径
                signBuiltInPass = iniFileHelper.ContentValue(section, "signBuiltInPass");    //内置证书密码
                signCustomPath = iniFileHelper.ContentValue(section, "signCustomPath");      //自定义证书路径
                signCustomPass = iniFileHelper.ContentValue(section, "signCustomPass");      //自定义证书密码

                wjType = ToIntOrDefault(WjType,1);
                qfzType = ToIntOrDefault(QfzType, 0);
                yzType = ToIntOrDefault(YzType, 0);
                djType = ToIntOrDefault(DjType, 0);
                qmType = ToIntOrDefault(QmType, 0);  //默认值还是0
                wzType = ToIntOrDefault(WzType, 3);
                qbflag = ToIntOrDefault(Qbflag, 0);
                size = ToIntOrDefault(Size, 40);
                rotation = ToIntOrDefault(Rotation, 0);
                opacity = ToIntOrDefault(Opacity, 60);
                wz = ToIntOrDefault(Wz, 50);
                maxfgs = ToIntOrDefault(Maxfgs, 20);
                yzIndex = ToIntOrDefault(YzIndex, -1);
            }
            fw = this.Width;
            fh = this.Height;
            collapsedClientWidth = PreviewPanelLayout.GetCollapsedClientWidth(comboPDFlist.Left, 8);
            expandedClientWidth = this.ClientSize.Width + 517;
            ApplyPreviewPanelLayout(yzType, qfzType);
            comboType.SelectedIndex = wjType;
            comboQfz.SelectedIndex = qfzType;
            comboYz.SelectedIndex = yzType;
            comboDJ.SelectedIndex = djType;
            comboQmtype.SelectedIndex = qmType;
            comboBoxWZ.SelectedIndex = wzType;
            comboBoxQB.SelectedIndex = qbflag;
            textCC.Text = size.ToString();
            textRotation.Text = rotation.ToString();
            textOpacity.Text = opacity.ToString();
            textWzbl.Text = wz.ToString();
            textMaxFgs.Text = maxfgs.ToString();

            //@loquat 20250922
            //容差加上ToolTip
            tip.AutoPopDelay = 5000;     // 提示显示时长（毫秒）
            tip.InitialDelay = 100;      // 鼠标进入后出现提示的延迟
            tip.ReshowDelay = 100;       // 提示再次出现的延迟
            tip.SetToolTip(this.txtAllow, "可设置透明色容差，默认20，最高50");

            //@loquat 20250920
            //if qmType == 1 {
            //    textname.Text = signText;
            //}

            pictureBox2.Parent = this.pictureBox1;//设置盖章预览图片的父控件为盖章预览框
            pictureBox2.Location = new Point(220, 380);//盖章预览图片位置
            ApplyIdleStampOverlaySize();
            if (qmType == 0)
            {
                labelname.Text = "签名";
                textname.Text = "";
                textpass.Text = "";
                textname.ReadOnly = true;
                textpass.ReadOnly = true;
            }
            else if (qmType == 1)
            {
                textpass.Text = "";
                if (!File.Exists(certDefaultPath))  //内置证书未生成
                {
                    labelname.Text = "签名";
                    textname.Text = "";
                    textname.ReadOnly = false;     //可以输入签名文本和密码
                }
                else
                {
                    labelname.Text = "证书";
                    textname.Text = certDefaultPath;
                    textname.ReadOnly = true;
                    textpass.Text = signBuiltInPass;  //已经有了，就自动带入上次的密码
                }
                textpass.ReadOnly = false;
            }
            else if (qmType == 2)  //缩小范围
            {
                labelname.Text = "证书";
                if (signCustomPath.Length == 0)  //之前未使用过自定义证书
                {
                    OpenFileDialog file = new OpenFileDialog();
                    file.Filter = "证书文件|*.pfx";
                    if (file.ShowDialog() == DialogResult.OK)  //用户选择了pfx文件
                    {
                        textname.Text = file.FileName;
                    }
                    else                                       //用户为选择pfx文件
                    {
                        comboQmtype.SelectedIndex = 0;         //没选就跳到无数字证书
                        labelname.Text = "签名";
                        textname.ReadOnly = true;
                        textpass.ReadOnly = true;
                    }
                }
                else  //之前使用过自定义证书，那就加载自定义证书和密码
                {
                    textname.Text = signCustomPath;
                    textpass.Text = signCustomPass;
                    textname.ReadOnly = true;
                    textpass.ReadOnly = false;  //用户还是可以手动太密码
                }

            }
            else
            {
                //啥也不干
            }
            dtYz.Columns.Add("Name", typeof(string));
            dtYz.Columns.Add("Value", typeof(string));
            comboBoxYz.DisplayMember = "Name";
            comboBoxYz.ValueMember = "Value";
            comboBoxYz.DataSource = dtYz;

            if (!File.Exists(yzLog))  //不存在yz.log文件
            {
                yzIndex = -1;
                if (File.Exists(strIniFilePath))
                {
                    //为了避免误删印章记录,然后下次打开又不盖章,再打开可能的报错,这里先重置下印章索引
                    IniFileHelper iniFileHelper = new IniFileHelper(strIniFilePath);
                    iniFileHelper.WriteIniString(section, "yzIndex", yzIndex.ToString());
                }
            }
            else
            {
                string line;
                string filename;
                StreamReader sr = new StreamReader(yzLog, false);
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    filename = System.IO.Path.GetFileName(line);//文件名
                    dtYz.Rows.Add(new object[] { filename, line });
                }
                sr.Close();
                sr.Dispose();
            }
            comboBoxYz.SelectedIndex = yzIndex;
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            comboPDFlist.DisplayMember = "Name";
            comboPDFlist.ValueMember = "Value";
            comboPDFlist.DataSource = dt;
            dtPages.Columns.Add("Name", typeof(string));
            dtPages.Columns.Add("Value", typeof(string));
            comboBoxPages.DisplayMember = "Name";
            comboBoxPages.ValueMember = "Value";
            comboBoxPages.DataSource = dtPages;
            dtPos.Columns.Add("Path", typeof(string));
            dtPos.Columns.Add("Page", typeof(int));
            dtPos.Columns.Add("X", typeof(float));
            dtPos.Columns.Add("Y", typeof(float));
            isSaveSources.Enabled = false;
            if (sourcePath != "")
            {
                wjType = 1;
                comboType.SelectedIndex = wjType;
                pathText.Text = sourcePath;
                textBCpath.Text = outputPath;
                dt.Rows.Clear();
                dt.Rows.Add(new object[] { "", "" });
                foreach (string filePath in commandLinePdfFiles)
                {
                    string filename = System.IO.Path.GetFileName(filePath);
                    dt.Rows.Add(new object[] { filename, filePath });
                }

                if (comboPDFlist.Items.Count > 1)
                {
                    comboPDFlist.SelectedIndex = comboPDFlist.Items.Count - 1;
                }
            }
        }

        public static int ToIntOrDefault(string str, int defaultValue = 0)
        {
            return int.TryParse(str, out int result) ? result : defaultValue;
        }

        /// <summary>
        /// 盖章按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            wjType = comboType.SelectedIndex;   //文件类型
            qfzType = comboQfz.SelectedIndex;   //骑缝章类型
            yzType = comboYz.SelectedIndex;     //印章类型
            djType = comboDJ.SelectedIndex;     //叠加类型
            qmType = comboQmtype.SelectedIndex; //签名类型
            wzType = comboBoxWZ.SelectedIndex;  //骑缝章位置类型
            qbflag = comboBoxQB.SelectedIndex;  //是否切边标记
            yzIndex = comboBoxYz.SelectedIndex; //选择的印章索引

            if (qfzType == 1 && yzType == 0 && qmType == 0)
            {
                if (MessageBox.Show("你既不盖骑缝章又不盖印章还不要我签名是想让我帮你关机吗?","你想干嘛", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    MessageBox.Show("滚蛋吧,我才不帮你关呢,哼!");
                    System.Environment.Exit(0);
                }
            }
            else if (yzIndex == -1)
            {
                MessageBox.Show("请先选择印章!");
                return;
            }
            else
            {
                sourcePath = pathText.Text;
                outputPath = textBCpath.Text;
                imgPath = comboBoxYz.SelectedValue.ToString();
                signText = textname.Text;
                password = textpass.Text;
                pdfpassword = textpdfpass.Text;

                if (sourcePath != "" && outputPath != "" && (imgPath != "" || qfzType == 1 && yzType == 0))
                {
                    if (!File.Exists(imgPath))
                    {
                        MessageBox.Show("印章文件读取失败,请重新导入印章。");
                    }
                    else if (!int.TryParse(textCC.Text, out size) || size > 100)
                    {
                        MessageBox.Show("印章尺寸设置错误,请输入正确的尺寸。");
                    }
                    else if (!int.TryParse(textRotation.Text, out rotation))
                    {
                        MessageBox.Show("印章角度设置错误,请输入正确的整数。");
                    }
                    else if (!int.TryParse(textOpacity.Text, out opacity) || opacity > 100)
                    {
                        MessageBox.Show("不透明度设置错误,请输入100以内的整数。");
                    }
                    else if (!int.TryParse(textWzbl.Text, out wz) || wz > 100)
                    {
                        MessageBox.Show("骑缝章位置设置错误,请输入100以内的整数。");
                    }
                    else if (!int.TryParse(textMaxFgs.Text, out maxfgs))
                    {
                        MessageBox.Show("最大分割数设置错误,请输入正确的整数。");
                    }
                    else
                    {
                        pdfGz();
                        //自动保持最后一次盖章的配置信息到配置文件
                        IniFileHelper iniFileHelper = new IniFileHelper(strIniFilePath);
                        
                        iniFileHelper.WriteIniString(section, "wjType", wjType.ToString());
                        iniFileHelper.WriteIniString(section, "qfzType", qfzType.ToString());
                        iniFileHelper.WriteIniString(section, "yzType", yzType.ToString());
                        iniFileHelper.WriteIniString(section, "djType", djType.ToString());
                        iniFileHelper.WriteIniString(section, "qmType", qmType.ToString());
                        iniFileHelper.WriteIniString(section, "wzType", wzType.ToString());
                        iniFileHelper.WriteIniString(section, "qbflag", qbflag.ToString());
                        iniFileHelper.WriteIniString(section, "size", size.ToString());
                        iniFileHelper.WriteIniString(section, "rotation", rotation.ToString());
                        iniFileHelper.WriteIniString(section, "opacity", opacity.ToString());
                        iniFileHelper.WriteIniString(section, "wz", wz.ToString());
                        iniFileHelper.WriteIniString(section, "maxfgs", maxfgs.ToString());
                        iniFileHelper.WriteIniString(section, "yzIndex", yzIndex.ToString());

                        //@loquat
                        //iniFileHelper.WriteIniString(section, "signText", signText);
                        //去pdfGz里，成功才保存签名的3个参数
                    }
                }
                else
                {
                    MessageBox.Show("文件路径不能为空，请先选择路径。");
                }
            }
            
        }

        /// <summary>
        /// 盖章函数
        /// </summary>
        private void pdfGz()
        {
            if (!Directory.Exists(outputPath))//输出目录不存在则新建
            {
                Directory.CreateDirectory(outputPath);
            }

            if (log.Text == "提示:建议使用472像素以上且背景透明的印章图片.\r\n使用合并模式会导致文字不可编辑,并且原数字签名丢失.随意骑缝章和自定义加印章共用右边的预览定位,所以同时使用的时候会冲突,建议分开盖章.")
            {
                log.Text = "";//清空日志
            }
            log.ForeColor = Color.Black;

            try
            {
                //如果要数字签名,先判断证书能否正常加载
                if (qmType!=0)
                {
                    string certPath = null;//证书路径
                    if (qmType == 1)
                    {
                        //如果证书不存在就先生成证书
                        if (!File.Exists(certDefaultPath))
                        {
                            var rSA = RSA.Create(4096); // 生成非对称密钥对
                            var req = new CertificateRequest("CN=" + signText, rSA, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                            X509Certificate2 newCert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));//这里生成的证书经测试不能直接用,必须先保存为pfx文件再重新加载

                            // Create PFX (PKCS #12) with private key
                            File.WriteAllBytes(certDefaultPath, newCert.Export(X509ContentType.Pkcs12, password));//不同版本的.net Pfx跟Pksc12好像并不一样,所以直接指定成Pksc12更安全

                            // Create Base 64 encoded CER (public key only)
                            //File.WriteAllText("D:\\publicKey.cer","-----BEGIN CERTIFICATE-----\r\n" + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks) + "\r\n-----END CERTIFICATE-----");

                        }
                        certPath = certDefaultPath;
                    }
                    else
                    {
                        certPath = signText;//自定义证书路径
                    }

                    try
                    {
                        cert = new X509Certificate2(certPath, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);//注意设置第三个参数,不然有权限问题
                    }
                    catch
                    {
                        MessageBox.Show("证书加载失败,请检查证书路径和密码");
                        return;
                    }
                }

                imgYz = new Bitmap(imgPath);
                //判断印章图片是否PNG格式
                //if (!imgYz.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                //@loquat 20250922 这里改成用户配置
                if (cbxTransColor.Checked ==  true)
                {
                    //如果不是PNG格式,则把白色部分设置为透明
                    imgYz = SetWhiteToTransparent(imgYz);
                }
                
                //再判断是否需要调整整体的透明度
                if (opacity < 100)
                {
                    imgYz = SetImageOpacity(imgYz, opacity);
                }
                //再看是否需要旋转印章
                if (rotation != 0)
                {
                    bool qb = qbflag==0?true: false;
                    int iw = imgYz.Width;
                    imgYz = RotateImg(imgYz, rotation, qb);
                    xzbl = 1f*imgYz.Width/iw;
                }
                // 获取系统临时目录
                string tempDirectory = Path.GetTempPath();

                //目录模式还是文件模式
                if (wjType == 0)  //目录模式
                {
                    DirectoryInfo dir = new DirectoryInfo(sourcePath);
                    var fileInfos = dir.GetFiles("*.pdf",SearchOption.AllDirectories);
                    foreach (var fileInfo in fileInfos)
                    {
                        if (fileInfo.DirectoryName == outputPath)
                        {
                            //如果源文件目录跟输出目录一样,则不处理
                            continue;
                        }
                        if (fileInfo.Extension == ".pdf")
                        {
                            string source = fileInfo.DirectoryName + "\\" + fileInfo.Name;
                            string input = source;
                            string output = outputPath + "\\" + fileInfo.Name;
                            if (isSaveSources.Checked == true)
                            {
                                if (fixType == 1)  //@loquat
                                {
                                    output = fileInfo.DirectoryName + "\\" + fixStr + "_" + Path.GetFileNameWithoutExtension(fileInfo.Name)  + fileInfo.Extension;
                                }
                                else
                                {
                                    output = fileInfo.DirectoryName + "\\" + Path.GetFileNameWithoutExtension(fileInfo.Name) + "_" + fixStr + fileInfo.Extension;
                                }
                                    

                            }
                            if (checkMultiple.Checked == true && File.Exists(output))
                            {
                                // 构建目标路径
                                input = Path.Combine(tempDirectory, fileInfo.Name);

                                // 复制文件（如果目标文件已存在，会覆盖）
                                File.Copy(output, input, true);

                            }

                            bool isSurrcess = PDFWatermark(input, output, source);
                            if (isSurrcess&&djType==1)
                            {
                                PDFToiPDF(output);
                            }
                            if (isSurrcess)
                            {
                                log.Text = log.Text + "成功！“" + fileInfo.Name + "”盖章完成！\r\n";
                                //@loquat 20250920 确保保存的签章信息是成功的配置
                                IniFileHelper iniFileHelper = new IniFileHelper(strIniFilePath);
                                qmType = comboQmtype.SelectedIndex;
                                if (qmType == 1)
                                {
                                    signBuiltInPath = textname.Text;
                                    signBuiltInPass = textpass.Text;
                                    iniFileHelper.WriteIniInt(section, "fixType", fixType);
                                    iniFileHelper.WriteIniString(section, "signBuiltInPath", signBuiltInPath);
                                    iniFileHelper.WriteIniString(section, "signBuiltInPass", signBuiltInPass);
                                }
                                else if (qmType == 2)
                                {
                                    signCustomPath = textname.Text;
                                    signCustomPass = textpass.Text;
                                    iniFileHelper.WriteIniInt(section, "fixType", fixType);
                                    iniFileHelper.WriteIniString(section, "signCustomPath", signCustomPath);
                                    iniFileHelper.WriteIniString(section, "signCustomPass", signCustomPass);
                                }

                            }
                            else
                            {
                                log.Text = log.Text + "失败！“" + fileInfo.Name + "”盖章失败！\r\n";
                            }
                        }
                    }
                }
                else  //文件模式
                {
                    string[] fileArray = sourcePath.Split(',');//字符串转数组
                    foreach (string file in fileArray)
                    {
                        string filename = Path.GetFileName(file);//文件名
                        string output = outputPath + "\\" + filename;//输出文件的绝对路径
                        if (outputPath == Path.GetDirectoryName(file))
                        {   
                            //@loquat
                            if (fixType == 1)
                            {
                                output = outputPath + "\\" + fixStr + "_" + Path.GetFileNameWithoutExtension(file) + ".pdf";//如果跟源文件在同一个目录需要重命名
                            }
                            else
                            {
                                output = outputPath + "\\" + Path.GetFileNameWithoutExtension(file) + "_" + fixStr  + ".pdf";//如果跟源文件在同一个目录需要重命名
                            } 
                                
                        }
                        string source = file;
                        string input = source;
                        if (checkMultiple.Checked == true && File.Exists(output))
                        {
                            // 构建目标路径
                            input = Path.Combine(tempDirectory, filename);

                            // 复制文件（如果目标文件已存在，会覆盖）
                            File.Copy(output, input, true);

                        }
                        bool isSurrcess = PDFWatermark(input, output, source);
                        if (isSurrcess)
                        {
                            if(djType == 1)
                            {
                                PDFToiPDF(output);
                            }
                            if (pdfpassword != "")
                            {
                                string jmoutput;
                                if (fixType == 1)
                                {
                                    jmoutput = outputPath + "\\" + fixStr + "_" + Path.GetFileNameWithoutExtension(file) + "_" + fixStr2 + ".pdf";
                                }
                                else
                                {
                                    jmoutput = outputPath + "\\" + Path.GetFileNameWithoutExtension(file) + "_" + fixStr + "_" + fixStr2 + ".pdf";
                                }
                                    
                                EncryptPDF(output, jmoutput, pdfpassword);
                            }
                        }
                        if (isSurrcess)
                        {
                            log.Text = log.Text + "成功！“" + filename + "”盖章完成！\r\n";
                            //@loquat 20250920 确保保存的签章信息是成功的配置
                            IniFileHelper iniFileHelper = new IniFileHelper(strIniFilePath);
                            qmType = comboQmtype.SelectedIndex;
                            if (qmType == 1)
                            {
                                signBuiltInPath = textname.Text;
                                signBuiltInPass = textpass.Text;
                                iniFileHelper.WriteIniInt(section, "fixType", fixType);
                                iniFileHelper.WriteIniString(section, "signBuiltInPath", signBuiltInPath);
                                iniFileHelper.WriteIniString(section, "signBuiltInPass", signBuiltInPass);
                            }
                            else if (qmType == 2)
                            {
                                signCustomPath = textname.Text;
                                signCustomPass = textpass.Text;
                                iniFileHelper.WriteIniInt(section, "fixType", fixType);
                                iniFileHelper.WriteIniString(section, "signCustomPath", signCustomPath);
                                iniFileHelper.WriteIniString(section, "signCustomPass", signCustomPass);
                            }
                        }
                        else
                        {
                            log.Text = log.Text + "失败！“" + filename + "”盖章失败！\r\n";
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //设置图片白色为透明
        private Bitmap SetWhiteToTransparent(Bitmap src)
        {
            return WhiteTransparencyHelper.Apply(src, GetWhiteTransparencyTolerance());
        }
        //private Bitmap SetWhiteToTransparent(System.Drawing.Bitmap img)
        //{
        //    Bitmap bitmap = new Bitmap(img);
        //    // 遍历图片的每个像素
        //    for (int x = 0; x < bitmap.Width; x++)
        //    {
        //        for (int y = 0; y < bitmap.Height; y++)
        //        {
        //            Color pixelColor = bitmap.GetPixel(x, y);
        //
        //            // 判断像素颜色是否为白色
        //            if (pixelColor.R > 230 && pixelColor.G > 230 && pixelColor.B > 230)  //这个容错蛮高啊
        //            {
        //                // 将白色像素设置为透明
        //                bitmap.SetPixel(x, y, Color.Transparent);
        //            }
        //        }
        //    }
        //
        //    return bitmap;
        //}


        /// <summary>
        /// 解决任意骑缝章时没有选定的页面的问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboQfz_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboQfz.SelectedIndex != -1 && comboQfz.SelectedIndex == 4)
            {
                comboPDFlist.SelectedIndex = comboPDFlist.Items.Count - 1;
                if (comboYz.SelectedIndex == 4 && comboPDFlist.SelectedIndex != -1)
                {
                    MessageBox.Show("提示:随意骑缝章和自定义加印章共用右边的预览定位,所以同时使用的时候会冲突,建议分开盖章");
                }
            }
        }

        /// <summary>
        /// 加盖印章
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboYz_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboYz.SelectedIndex != -1 && comboYz.SelectedIndex != 0)
            {
                int index = comboPDFlist.SelectedIndex;
                int count = comboPDFlist.Items.Count - 1;
                if (index != -1 && count != -1 && index == count)  //未改动时避免不触发事件
                {
                    _ = LoadSelectedPreviewAsync();
                    if (comboYz.SelectedIndex == 4 && comboQfz.SelectedIndex == 4)
                    {
                        MessageBox.Show("提示:随意骑缝章和自定义加印章共用右边的预览定位,所以同时使用的时候会冲突,建议分开盖章");
                    }
                }
                comboPDFlist.SelectedIndex = comboPDFlist.Items.Count - 1;  //加印章时默认选定第一页，尾页加盖除外
            }
        }

        /// <summary>
        /// pdf文件预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void comboPDFlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPDFlist.SelectedIndex != -1 && comboPDFlist.Items.Count > 0)   //手动选择pdf文件
            {
                await LoadSelectedPreviewAsync();
            }
        }
        //设置图片透明度(1-100)
        //设置条件编译
    #if UNSAFE_MODE
        private Bitmap SetImageOpacity(Bitmap srcImage, int opacity)
        {
            // 参数校验
            if (opacity < 0 || opacity > 100)
                throw new ArgumentOutOfRangeException("opacity", "必须介于0到100之间");
            // 计算实际Alpha值 (0-255)
            byte alpha = (byte)(opacity * 255 / 100);
            // 创建目标位图（确保32bppARGB格式）
            Bitmap dstImage = new Bitmap(srcImage.Width, srcImage.Height, PixelFormat.Format32bppArgb);
            // 将原始图像绘制到目标位图（保留透明度信息）
            using (Graphics g = Graphics.FromImage(dstImage))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(srcImage, new System.Drawing.Rectangle(0, 0, dstImage.Width, dstImage.Height));
            }
            // 锁定位图进行快速内存操作
            BitmapData data = dstImage.LockBits(
                new System.Drawing.Rectangle(0, 0, dstImage.Width, dstImage.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb
            );
            unsafe // 需要启用unsafe编译选项
            {
                byte* ptr = (byte*)data.Scan0;
                // 遍历所有像素
                for (int y = 0; y < data.Height; y++)
                {
                    for (int x = 0; x < data.Width; x++)
                    {
                        // 每个像素的四个通道（BGRA格式，索引0-3对应Blue, Green, Red, Alpha）
                        int index = y * data.Stride + x * 4;
                        // 仅修改非完全透明像素（alpha值不为0）
                        if (ptr[index + 3] != 0) // 检查Alpha通道
                        {
                            ptr[index + 3] = alpha; // 设置新的Alpha值
                        }
                    }
                }
            }
            // 解锁并返回处理后的图像
            dstImage.UnlockBits(data);
            return dstImage;
        }
#else   //SetImageOpacity的安全方法，比原来的效率也要高很多倍
        private Bitmap SetImageOpacity(Bitmap src, int opacity)
        {
            byte alpha = (byte)(opacity * 255 / 100);
            BitmapData data = src.LockBits(new System.Drawing.Rectangle(0, 0, src.Width, src.Height),
                                         ImageLockMode.ReadWrite,
                                         PixelFormat.Format32bppArgb);
            try
            {
                byte[] buffer = new byte[data.Stride * data.Height];
                Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
                for (int i = 3; i < buffer.Length; i += 4) // 遍历每个Alpha通道
                {
                    if (buffer[i] != 0) buffer[i] = alpha;
                }
                Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
            }
            finally
            {
                src.UnlockBits(data);
            }
            return src;
        }
#endif 
        //旋转图片
        public Bitmap RotateImg(System.Drawing.Bitmap bitmap, int angle,bool original = true)
        {
            angle = angle % 360;
            //弧度转换 
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //原图的宽和高 
            int w = bitmap.Width;
            int h = bitmap.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));

            //为了尽可能的去除白边,减小印章旋转后尺寸的误差,这里保持原印章宽度,切掉部分角
            if (original)
            {
                H = H * w / W;
                W = w;
            }

            //目标位图 
            Bitmap dsImage = new Bitmap(W, H);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //计算偏移量 
            Point Offset = new Point((W - w) / 2, (H - h) / 2);
            //构造图像显示区域:让图像的中心与窗口的中心点一致 
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(angle);
            //恢复图像在水平和垂直方向的平移 
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(bitmap, rect);
            //重至绘图的所有变换 
            g.ResetTransform();
            g.Save();
            g.Dispose();
            //保存旋转后的图片 
            //dsImage.Save("D:\\tmp\\img\\tmp.png", System.Drawing.Imaging.ImageFormat.Png);

            return dsImage;
        }

        //分割图片
        private static Bitmap[] subImages(Bitmap img, int n)//图片分割
        {
            Bitmap[] nImage = new Bitmap[n];
            int H = img.Height;
            int W = img.Width;
            int w1 = W / 3;//首页骑缝章默认占1/3宽度
            int w = (W- w1) / n;
            n = n - 1;
            int tmpw = W;
            for (int i = 0; i <= n; i++)
            {
                int sw;
                if(i == n)
                {
                    sw = tmpw;
                }else if(i == 0)
                {
                    sw = w1;
                }
                else
                {
                    sw = w;
                }
                Bitmap newbitmap = new Bitmap(sw, H);
                Graphics g = Graphics.FromImage(newbitmap);
                g.DrawImage(img, new System.Drawing.Rectangle(0, 0, sw, H), new System.Drawing.Rectangle(W-tmpw, 0, sw, H), GraphicsUnit.Pixel);
                g.Dispose();
                nImage[i] = newbitmap;
                //newbitmap.Save("D:\\tmp\\img\\" + i + ".png", System.Drawing.Imaging.ImageFormat.Png);//查看图片是否正常
                tmpw = tmpw - sw;
            }
            return nImage;
        }

        //PDF盖章(贴图)
        private bool PDFWatermark(string inputfilepath, string outputfilepath, string sourcepath)
        {
            //float sfbl = 100f * size * xzbl * 2.842f / imgYz.Height;
            float sfbl = (100f * size * xzbl * 72) / (25.4f * imgYz.Width);

            //PdfGState state = new PdfGState();
            //state.FillOpacity = 0.01f*opacity;//印章图片不透明度

            //throw new NotImplementedException();
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            FileStream fileStream = new FileStream(outputfilepath, FileMode.Create);
            try
            {
                pdfReader = new PdfReader(inputfilepath, new System.Text.UTF8Encoding().GetBytes(pdfpassword));//选择需要印章的pdf
                if (qmType != 0)
                {
                    //最后的true表示保留原签名
                    pdfStamper = PdfStamper.CreateSignature(pdfReader, fileStream, '\0', null, true);//加完印章后的pdf
                }
                else
                {
                    pdfStamper = new PdfStamper(pdfReader, fileStream);
                }

                int numberOfPages = pdfReader.NumberOfPages;//pdf页数
                int qfzPages = 0;
                List<int> qfzList = new List<int>();

                bool skipSeamStamp = SeamStampPolicy.ShouldSkipForSinglePage(numberOfPages, qfzType);
                if (!skipSeamStamp && qfzType == 0)
                {
                    for (int i = 1; i <= numberOfPages; i ++)
                    {
                        qfzList.Add(i);
                        qfzPages++;
                    }
                }
                else if(!skipSeamStamp && qfzType == 2)
                {
                    for (int i = 1; i <= numberOfPages; i += 2)
                    {
                        qfzList.Add(i);
                        qfzPages++;
                    }
                }
                else if(!skipSeamStamp && qfzType == 3)
                {
                    for (int i = 2; i <= numberOfPages; i += 2)
                    {
                        qfzList.Add(i);
                        qfzPages++;
                    }
                }
                else if(!skipSeamStamp && qfzType == 4)
                {
                    // 遍历 DataTable 的所有行
                    foreach (DataRow row in dtPos.Rows)
                    {
                        if (row["Path"].ToString()== sourcepath)
                        {
                            // 获取当前行的某一列的值，假设这一列的列名为 "ColumnName"
                            int columnValue = Convert.ToInt32(row["Page"]);

                            // 将获取的值添加到列表中
                            qfzList.Add(columnValue);
                            qfzPages++;
                        }
                    }
                    qfzList.Sort();
                }
                
                PdfContentByte waterMarkContent;

                if(qfzType != 1&& qfzPages > 1)
                {
                    int max = maxfgs;//骑缝章最大分割数
                    int ss = qfzPages / max + 1;
                    int sy = qfzPages - ss * max / 2;
                    int sys = sy / ss;
                    int syy = sy % ss;
                    int pp = max / 2 + sys;
                    Bitmap[] nImage;
                    int startIndex = 0;
                    for (int i = 0; i < ss; i++)
                    {
                        int tmp = pp;
                        if (i < syy)
                        {
                            tmp++;
                        }
                        nImage = subImages(imgYz, tmp);
                        for (int y = 0; y < tmp; y++)
                        {
                            int page = qfzList[startIndex + y];
                            waterMarkContent = pdfStamper.GetOverContent(page);//获取当前页内容
                            int rotation = pdfReader.GetPageRotation(page);//获取当前页的旋转度
                            iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(page);//获取当前页尺寸
                            float pWidth, pHeight;
                            if (rotation == 90 || rotation == 270)
                            {
                                pWidth = psize.Height;
                                pHeight = psize.Width;
                            }
                            else
                            {
                                pWidth = psize.Width;
                                pHeight = psize.Height;
                            }
                            Bitmap qfzImage = null;
                            if(wzType == 3|| wzType == 2)
                            {
                                qfzImage = nImage[y];
                            }
                            else if (wzType == 1|| wzType == 0)
                            {
                                qfzImage = RotateImg(nImage[y], 90, false);
                            }
                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(qfzImage, System.Drawing.Imaging.ImageFormat.Png);//获取骑缝章对应页的部分
                            //image.Transparency = new int[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//这里透明背景的图片会变黑色,所以设置黑色为透明
                            //waterMarkContent.SaveState();//通过PdfGState调整图片整体的透明度
                            //waterMarkContent.SetGState(state);
                            //image.GrayFill = 20;//透明度，灰色填充
                            //image.Rotation//旋转
                            //image.ScaleToFit(140F, 320F);//设置图片的指定大小
                            //image.RotationDegrees = rotation//旋转角度
                            float imageW, imageH;
                            image.ScalePercent(sfbl);//设置图片比例
                            imageW = image.Width * sfbl / 100f;
                            imageH = image.Height * sfbl / 100f;

                            //水印的位置
                            float xPos=0, yPos=0;
                            if (wzType == 3)
                            {
                                xPos = pWidth - imageW;
                                yPos = (pHeight - imageH) * (100 - wz) / 100;
                            }
                            else if (wzType == 2)
                            {
                                xPos = 0;
                                yPos = (pHeight - imageH) * (100 - wz) / 100;
                            }
                            else if (wzType == 1)
                            {
                                xPos = (pWidth - imageW) * wz / 100;
                                yPos = 0;
                            }
                            else if (wzType == 0)
                            {
                                xPos = (pWidth - imageW) * wz / 100;
                                yPos = pHeight - imageH;
                            }
                            image.SetAbsolutePosition(xPos, yPos);
                            waterMarkContent.AddImage(image);
                            //waterMarkContent.RestoreState();
                        }
                        startIndex += tmp;
                    }
                }

                iTextSharp.text.Image img = null;
                float imgW = 0, imgH = 0;
                float stampXPos = 0, stampYPos = 0;
                int signpage = numberOfPages;

                if (StampRenderPolicy.ShouldRenderVisibleStamp(yzType))
                {
                    int no_page = 0;//不需要盖印章的页,0表示所有页都需要盖印章
                    signpage = 0;

                    if (yzType == 1)//首页不加印章
                    {
                        if(numberOfPages > 1)
                        {
                            no_page = 1;
                        }
                        signpage = numberOfPages;
                    }
                    else if (yzType == 2)//尾页不加印章
                    {
                        if (numberOfPages > 1)
                        {
                            no_page = numberOfPages;
                        }
                        signpage = 1;
                    }
                    else if (yzType == 3)//所有页加印章
                    {
                        signpage = numberOfPages;
                    }
                    else if (yzType == 4)//自定义页加印章
                    {
                        signpage = 1;
                        // 遍历 DataTable 的所有行
                        foreach (DataRow row in dtPos.Rows)
                        {
                            if (row["Path"].ToString() == sourcepath)
                            {
                                // 获取当前行的某一列的值，假设这一列的列名为 "ColumnName"
                                int columnValue = Convert.ToInt32(row["Page"]);
                                if(columnValue> signpage)
                                {
                                    signpage = columnValue;
                                }
                            }
                        }
                    }

                    for (int i = 1; i <= numberOfPages; i++)
                    {
                        if (StampPlacementPolicy.ShouldRenderVisibleStampOnPage(i, no_page, signpage, qmType != 0))
                        {
                            waterMarkContent = pdfStamper.GetOverContent(i);//获取当前页内容
                            int rotation = pdfReader.GetPageRotation(i);//获取指定页面的旋转度
                            iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(i);//获取当前页尺寸

                            //waterMarkContent.SaveState();//通过PdfGState调整图片整体的透明度
                            //waterMarkContent.SetGState(state);

                            float wbl = 0;
                            float hbl = 1;
                            DataRow[] arrRow = dtPos.Select("Path = '" + sourcepath + "' and Page = " + i);
                            if (arrRow == null || arrRow.Length == 0)
                            {
                                if(yzType == 4)//自定义页加印章,如果没有印章定位的页就不盖
                                {
                                    continue;
                                }

                                wbl = Convert.ToSingle(textPx.Text);//这里根据比例来定位
                                hbl = 1 - Convert.ToSingle(textPy.Text);//这里根据比例来定位

                                if (checkRandom.Checked == true)
                                {
                                    int random_w = 0, random_h = 0;
                                    Random random = new Random();
                                    random_w = random.Next(-2, 3);//随机偏移
                                    random_h = random.Next(-2, 3);//随机偏移
                                    if ((wbl + 0.01f * random_w) > 0f && (wbl + 0.01f * random_w) < 1f)
                                    {
                                        wbl = wbl + 0.01f * random_w;
                                    }
                                    if ((hbl - 0.01f * random_h) > 0f && (hbl - 0.01f * random_h) < 1f)
                                    {
                                        hbl = hbl - 0.01f * random_h;
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = arrRow[0];
                                wbl = Convert.ToSingle(dr["X"].ToString());
                                hbl = 1 - Convert.ToSingle(dr["Y"].ToString());
                            }
                            img = iTextSharp.text.Image.GetInstance(imgYz, System.Drawing.Imaging.ImageFormat.Png);//创建一个图片对象
                            int RotationDegrees = 0;
                            if (i != signpage && checkRandom.Checked == true)
                            {
                                Random random = new Random();
                                RotationDegrees = random.Next(-2, 3);//每页的印章设置个随机的角度
                            }
                            else
                            {
                                RotationDegrees = 0;
                            }
                            img.RotationDegrees = RotationDegrees;
                            //img.Transparency = new int[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//这里透明背景的图片会变黑色,所以设置黑色为透明
                            img.ScalePercent(sfbl);//设置图片比例
                            imgW = img.Width * sfbl / 100f;
                            imgH = img.Height * sfbl / 100f;
                            if (rotation == 90 || rotation == 270)
                            {
                                stampXPos = (psize.Height - imgW) * wbl;
                                stampYPos = (psize.Width - imgH) * hbl;
                            }
                            else
                            {
                                stampXPos = (psize.Width - imgW) * wbl;
                                stampYPos = (psize.Height - imgH) * hbl;
                            }
                            img.SetAbsolutePosition(stampXPos, stampYPos);
                            waterMarkContent.AddImage(img);
                            //waterMarkContent.RestoreState();

                            //普通印章跟数字印章已经完美重叠,所以就不需要通过以下方法特殊区分了
                            //同时启用印章和数字签名的话用最后一个印章用数字签名代替
                            //if (all)
                            //{
                            //    if (qmType == 0)
                            //    {
                            //        //所有页要盖章,并且不是数字签名

                            //        waterMarkContent.AddImage(img);
                            //        waterMarkContent.RestoreState();
                            //    }
                            //    else if (i != numberOfPages)
                            //    {
                            //        //所有页要盖章,要数字签名,但是不是最后一页
                            //        waterMarkContent.AddImage(img);
                            //        waterMarkContent.RestoreState();
                            //    }
                            //}
                            //else if (qmType == 0)
                            //{
                            //    //只有首页或尾页要盖章,并且不是数字签名
                            //    waterMarkContent.AddImage(img);
                            //    waterMarkContent.RestoreState();
                            //}


                            ////开始增加文本
                            //waterMarkContent.BeginText();

                            //BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_OBLIQUE, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            ////设置字体 大小
                            //waterMarkContent.SetFontAndSize(bf, 9);

                            ////指定添加文字的绝对位置
                            //waterMarkContent.SetTextMatrix(imgLeft, 200);
                            ////增加文本
                            //waterMarkContent.ShowText("GW INDUSTRIAL LTD");

                            ////结束
                            //waterMarkContent.EndText();

                        }
                    }

                    //加数字签名
                }

                if (qmType != 0)
                {

                    Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
                    Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] {cp.ReadCertificate(cert.RawData)};

                    Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair pk = Org.BouncyCastle.Security.DotNetUtilities.GetKeyPair(cert.GetRSAPrivateKey());
                    IExternalSignature externalSignature = new PrivateKeySignature(pk.Private, DigestAlgorithms.SHA256);

                    PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
                    signatureAppearance.SignDate = DateTime.Now;
                    //signatureAppearance.SignatureCreator = "";
                    //signatureAppearance.Reason = "验证身份";
                    //signatureAppearance.Location = "深圳";
                    if (!StampRenderPolicy.ShouldRenderVisibleStamp(yzType))
                    {
                        signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(0, 0, 0, 0), numberOfPages, null);
                    }
                    else
                    {
                        signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;//仅体现图片
                        signatureAppearance.SignatureGraphic = img;//iTextSharp.text.Image.GetInstance(imgPath);
                        //signatureAppearance.Acro6Layers = true;

                        //StringBuilder buf = new StringBuilder();
                        //buf.Append("Digitally Signed by ");
                        //String name = cert.SubjectName.Name;
                        //buf.Append(name).Append('\n');
                        //buf.Append("Date: ").Append(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss zzz"));
                        //string text = buf.ToString();
                        //signatureAppearance.Layer2Text = text;

                        float bk = 2;//数字签名的图片要加上边框才能跟普通印章的位置完全一致
                        signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(stampXPos - bk, stampYPos - bk, stampXPos + imgW + bk, stampYPos + imgH + bk), signpage, null);
                    }

                    MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);
                }
                return true;
            }
            catch (BadPasswordException)
            {
                MessageBox.Show("PDF密码错误.");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            finally
            {

                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();

                if (fileStream != null)
                    fileStream.Close();
            }
        }

        public static void ImageToPDF(Bitmap[] bitmaps,float bl, string trageFullName)
        {
            using (iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(0, 0), 0, 0, 0, 0))
            {
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(trageFullName, FileMode.Create, FileAccess.ReadWrite));
                document.Open();
                iTextSharp.text.Image image;
                for (int i = 0; i < bitmaps.Length; i++)
                {
                    image = iTextSharp.text.Image.GetInstance(bitmaps[i], System.Drawing.Imaging.ImageFormat.Bmp);
                    float Width = image.Width* bl, Height = image.Height* bl;
                    image.ScaleToFit(Width, Height);
                    document.SetPageSize(new iTextSharp.text.Rectangle(0, 0, Width, Height));
                    document.NewPage();
                    document.Add(image);
                }
            }
        }


        public static void SignaturePDF(string inputPath,string outPath, X509Certificate2 cert)
        {
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputPath);//选择需要印章的pdf
                pdfStamper = PdfStamper.CreateSignature(pdfReader, new FileStream(outPath, FileMode.Create), '\0', null, true);
                Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
                Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };

                Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair pk = Org.BouncyCastle.Security.DotNetUtilities.GetKeyPair(cert.GetRSAPrivateKey());
                IExternalSignature externalSignature = new PrivateKeySignature(pk.Private, DigestAlgorithms.SHA256);

                PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
                signatureAppearance.SignDate = DateTime.Now;
                //signatureAppearance.SignatureCreator = "";
                //signatureAppearance.Reason = "验证身份";
                //signatureAppearance.Location = "深圳";

                //signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(0, 0, 0, 0), numberOfPages, null);

                MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {

                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();
            }

        }
        //加密PDF文件
        public static void EncryptPDF(string inputFilePath, string outputFilePath, string password)
        {
            try
            {
                using (FileStream fs = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                {
                    using (PdfReader reader = new PdfReader(inputFilePath))
                    {
                        using (Document document = new Document())
                        {
                            PdfEncryptor.Encrypt(reader, fs, true, password, password, PdfWriter.ALLOW_PRINTING);
                        }
                    }
                }
                File.Delete(inputFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加密码保护时发生错误：" + ex.Message);
            }
        }
        //@loquat 20250920 增加一个双击可重新选择证书
        private void textname_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "证书文件|*.pfx";
            if (file.ShowDialog() == DialogResult.OK)  //用户选择了pfx文件
            {
                textname.Text = file.FileName;
            }
            else                                       //用户为选择pfx文件
            {
                comboQmtype.SelectedIndex = 0;         //没选就跳到无数字证书
                labelname.Text = "签名";
                textname.ReadOnly = true;
                textpass.ReadOnly = true;
            }
        }

        private void SaveSources(object sender, EventArgs e)
        {
            if (isSaveSources.Checked == true)
            {
                textBCpath.Enabled = false;
                OutPath.Enabled = false;
                if (comboType.SelectedIndex == 0 && pathText.Text != "")
                {
                    textBCpath.Text = pathText.Text;
                }
                else if (comboType.SelectedIndex == 1 && pathText.Text != "")
                {
                    textBCpath.Text = System.IO.Path.GetDirectoryName(pathText.Text);
                }
            }
            else
            {
                textBCpath.Enabled = true;
                OutPath.Enabled = true;
                if (comboType.SelectedIndex == 0 && pathText.Text != "")
                {
                    textBCpath.Text = pathText.Text +"\\已盖章"; ;
                }
                else if (comboType.SelectedIndex == 1 && pathText.Text != "")
                {
                    textBCpath.Text = System.IO.Path.GetDirectoryName(pathText.Text) + "\\已盖章";
                }
            }

        }


        /// <summary>
        /// pdf文件转换为图片，这个里面的很多设置后期可以考虑放出来，比如分辨率倍数等
        /// </summary>
        /// <param name="pdfPath"></param>
        public void PDFToiPDF(string pdfPath)
        {
            ////方法1,测试Aspose.pdf转位图在这没有成功，demo测试可以，可能和qfz文件经过iTextSharp或者O2S.Components.PDFRender4NET处理过有关，采用直接读取文件
            //Bitmap[] bitmaps = PDFConverter.ConvertToBitmapArray(pdfPath);//转换的清晰度高，文件大
            //PDFConverter.ConvertBitmapArrayToPDF(bitmaps, pdfPath,true,true,0.0,0.0,0.0,0.0);
            //PdfConverter.ConvertPDFToPDF(pdfPath, pdfPath, true, true, 0.0, 0.0, 0.0, 0.0);
            //上面3个函数调用都可以

            //原版方法2
            PDFFile pdfFile = PDFFile.Open(pdfPath);
            Bitmap[] bitmaps = new Bitmap[pdfFile.PageCount];
            int dpi = 300; //原版方法最好默认300
            for (int i = 0; i < pdfFile.PageCount; i++)
            {
                Bitmap pageImage = pdfFile.GetPageImage(i, dpi);      //这个地方转换导致原有水印和背景透明度丢失，下面的方法解决
                bitmaps[i] = pageImage;
                //pageImage.Save(AppDomain.CurrentDomain.BaseDirectory + @"Image1" + i + ".png", System.Drawing.Imaging.ImageFormat.Png);
                //pageImage.Save(imageOutputPath + imageName + i.ToString() + "." + imageFormat.ToString(), imageFormat);
            }
            pdfFile.Dispose();

            //方法3测试转换为图片
            //PDFConverter PDFI = new PDFConverter(); //过滤了其他文件和含_已盖章的文件，是转换的原始文件，没有盖章前的，也可以输出盖过的文件，这是验证测试用
            //PDFI.ConvertPDFsToImages(pathDir, textBCpath.Text, System.Drawing.Imaging.ImageFormat.Png, 300, 1, pdfFile.PageCount);//后3个参数时dpi 起始页和最后页

            //PdfReader reader = new PdfReader(pdfPath);
            //int pageCount = reader.NumberOfPages;

            //for (int i = 1; i <= pageCount; i++)
            //{
            //    iTextSharp.text.Rectangle pageSize = reader.GetPageSize(i);
            //    float width = pageSize.Width;
            //    float height = pageSize.Height;
            //}
            //reader.Close();
            //reader.Dispose();

            //获取屏幕参数
            //int screenWidth = 1920;
            //int screenHeight = 1080;
            //Screen screen = Screen.PrimaryScreen;
            //screenWidth = screen.Bounds.Width; //1707
            //screenHeight = screen.Bounds.Height;//1067
            //screenWidth = (int)(screenWidth * ScaleX);//2560  ScaleX1.5左右
            //screenHeight = (int)(screenHeight * ScaleY);//1600

            //int dpiXX, dpiYY;
            //SystemDpi(out dpiXX, out dpiYY); //96
            //int dpi = dpiXX;        //300,获取当前分辨率，盖章后，不会出现两侧距离过大或过小，
            //double scale = Scaling(dpiXX); // 缩放比例96返回1

            #region 方法4，转换的文件小，清晰度差一点，可以通过分辨率设置大小及清晰度
            //var pdf = PdfiumViewer.PdfDocument.Load(pdfPath); // 读取pdf
            //var pdfPage = pdf.PageCount; // pdf页码
            //var pdfSize = pdf.PageSizes;


            //if (startPageNum <= 0) { startPageNum = 1; }
            //if (endPageNum > pdf.PageCount) { endPageNum = pdf.PageCount; }
            //if (startPageNum > endPageNum) // 开始>结束
            //{
            //    int tempPageNum = startPageNum;
            //    startPageNum = endPageNum;
            //    endPageNum = startPageNum;
            //}


            // bitmaps = new Bitmap[pdfPage];
            //for (int i = 0; i < pdfPage; i++)
            //{
            //    System.Drawing.Size size = new System.Drawing.Size();
            //    size.Width = (int)pdfSize[i].Width;
            //    size.Height = (int)pdfSize[i].Height;

            //    // 计算适合的图像大小和分辨率
            //    int targetWidth = (int)(size.Width * 2 * scale); // 保持原始宽度,转换成图片的清晰度主要和这个地方乘以倍数有关,也就是分辨率
            //    int targetHeight = (int)(size.Height * 2 * scale); // 保持原始高度，一定要同比例放大，也就是倍数一样，3倍效果好，300多k文件盖后会变成1m多

            //    // 计算适合的水平和垂直分辨率
            //    float dpiX = dpi;
            //    float dpiY = dpi;

            //    var image = pdf.Render(i, targetWidth, targetHeight, (int)dpiX, (int)dpiY, PdfiumViewer.PdfRenderFlags.Annotations);
            //    bitmaps[i] = new Bitmap(image);
            //}
            //pdf.Dispose();
            #endregion

            float bl = 72f / dpi; // 为了尽量保证转换的清晰度，这里需要把电脑的DPI缩放到PDF的DPI,在计算bl时，使用了固定值72，PDF中，1英寸等于72个点（point）

            string tmpPdf = pdfPath;

            if (qmType != 0)        //0 不使用数字签名，这表示使用数字签名时
            {
                tmpPdf = System.IO.Path.GetTempPath() + "PDFQFZ_tmp.pdf";
            }

            //PDFConverter.ImageToPDF2(bitmaps, bl, tmpPdf);
            ImageToPDF(bitmaps, bl, tmpPdf);      //转换位图到图片

            if (qmType != 0)
            {
                SignaturePDF(tmpPdf, pdfPath, cert);
            }
        }
        //建一个全局的目录变量
        string pathDir = "";
        //选择源文件
        private async void SelectPath_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token; // 获取取消标记

            if (comboType.SelectedIndex == 0)   //目录模式
            {
                if (Environment.OSVersion.Version.Major >= 6)                                       //操作系统win7及以上才能使用此效果
                {
                    FolderSelectDialog fsd = new FolderSelectDialog();
                    fsd.Title = "请选择pdf所在的文件夹";
                    fsd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    if (fsd.ShowDialog(this.Handle))
                    {
                        pathText.Text = fsd.FileName;
                        pathDir = fsd.FileName;
                        if (textBCpath.Text == "")
                        {
                            textBCpath.Text = fsd.FileName + "\\已盖章";
                        }
                        dt.Rows.Clear();
                        dt.Rows.Add(new object[] { "", "" });
                        DirectoryInfo dir = new DirectoryInfo(pathText.Text);
                        var fileInfos = dir.GetFiles("*.pdf", SearchOption.AllDirectories);

                        // 初始化状态栏和进度条
                        log.Text = "准备批量处理...\r\n";
                        progressBar1.Visible = true;
                        bt_gz.Enabled = false;//批量加载时防止用户点击按钮
                        
                        //开始异步加载文件
                        try
                        {
                            await Task.Run(() => Load_All_PdfFiles_Async(fileInfos, token), token);
                        }
                        catch (OperationCanceledException)
                        {
                            log.Text = "操作已取消！";
                        }
                        finally
                        {
                            // 取消后的清理工作
                            cancellationTokenSource.Dispose();
                        }
                        bt_gz.Enabled = true;//恢复盖章按钮
                        pathText.Text = fsd.FileName;
                        //foreach (var fileInfo in fileInfos)
                        //{
                        //    if (fileInfo.Extension == ".pdf")
                        //    {
                        //        dt.Rows.Add(new object[] { fileInfo.Name, fileInfo.FullName });
                        //    }
                        //}
                    }
                }
                else
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        pathText.Text = fbd.SelectedPath;
                        pathDir = fbd.SelectedPath;
                        if (textBCpath.Text == "")
                        {
                            textBCpath.Text = fbd.SelectedPath + "\\已盖章";
                        }
                        dt.Rows.Clear();
                        dt.Rows.Add(new object[] { "", "" });
                        DirectoryInfo dir = new DirectoryInfo(pathText.Text);
                        var fileInfos = dir.GetFiles("*.pdf", SearchOption.AllDirectories);
                        foreach (var fileInfo in fileInfos)
                        {
                            if (fileInfo.Extension == ".pdf")
                            {
                                dt.Rows.Add(new object[] { fileInfo.Name, fileInfo.FullName });
                            }
                        }
                    }
                }
            }
            else
            {           //文件模式
                OpenFileDialog file = new OpenFileDialog();
                file.Multiselect = true;
                file.Filter = "PDF文件|*.pdf";
                file.Title = "请选择要盖章的pdf文件";    //不设置默认左上角显示打开
                if (file.ShowDialog() == DialogResult.OK)
                {
                    pathText.Text = string.Join(",", file.FileNames);
                    if (textBCpath.Text == "")
                    {
                        textBCpath.Text = System.IO.Path.GetDirectoryName(file.FileName);
                    }
                    pathDir = textBCpath.Text;
                    dt.Rows.Clear();
                    dt.Rows.Add(new object[] { "", "" });
                    foreach (string filePath in file.FileNames)
                    {
                        string filename = System.IO.Path.GetFileName(filePath);//文件名
                        dt.Rows.Add(new object[] { filename, filePath });
                    }

                    //更新文件后，重新更新pdf预览框
                    if (comboPDFlist.SelectedIndex != -1 && comboPDFlist.Items.Count > 0)//0是空白行,1才是选择的文件行
                    {
                        comboPDFlist.SelectedIndex = comboPDFlist.Items.Count - 1;
                        //// 手动创建一个 EventArgs 对象
                        //EventArgs eventArgs = new EventArgs();
                        //// 调用 comboPDFlist_SelectionChangeCommitted 事件处理程序
                        //comboPDFlist_SelectionChangeCommitted(comboPDFlist, eventArgs);
                    }
                }
            }
        }
        /// <summary>
        /// 加载文件夹文件使用异步方式并辅以进度条显示状态，并可以随时按ESC取消
        /// </summary>
        private async Task Load_All_PdfFiles_Async(FileInfo[] fileInfos, CancellationToken token)
        {
            int totalFiles = fileInfos.Length;

            // 在 UI 线程上更新进度条的最大值和初始值
            this.Invoke((Action)(() =>
            {
                progressBar1.Maximum = totalFiles;
                progressBar1.Value = 0;
                log.Text += "开始批量处理...\r\n";
            }));


            // 遍历文件
            await Task.Run(() =>
            {
                for (int i = 0; i < totalFiles; i++)
                {
                    token.ThrowIfCancellationRequested(); // 检查是否有取消请求,如果有结束遍历动作

                    var fileInfo = fileInfos[i];

                    // 更新UI（此处要使用 Invoke，因为跨线程更新UI）
                    Invoke(new Action(() =>
                    {
                        dt.Rows.Add(new object[] { fileInfo.Name, fileInfo.FullName });

                        // 更新进度条和状态栏
                        pathText.Text = $"正在加载文件 {i + 1}/{totalFiles}\r\n";
                        progressBar1.Value = i + 1;
                    }));

                    // 模拟加载延时（可以去掉或根据需要调整）
                    Task.Delay(100).Wait();
                }
            });

            // 所有文件加载完成后更新状态
            Invoke(new Action(() =>
            {
                log.Text = "加载完成!\r\n";
                progressBar1.Value = totalFiles;
                progressBar1.Visible = false;
            }));
        }
        //按下ESC事件
        private void Esc_Key_Down(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // 检查是否有一个操作在进行中
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel(); // 请求取消当前操作
                    MessageBox.Show("操作已停止");
                    progressBar1.Visible = false;
                    bt_gz.Enabled = true;
                    pathText.Text = "";
                }
            }
        }

        //选择保存目录
        private void OutPath_Click(object sender, EventArgs e)
        {
            if (Environment.OSVersion.Version.Major >= 6)                                       //操作系统win7及以上才能使用此效果
            {
                FolderSelectDialog fsd = new FolderSelectDialog();
                fsd.Title = "请选择保存的文件夹";
                if (string.IsNullOrWhiteSpace(pathText.Text.Trim()))
                {
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    fsd.InitialDirectory = desktopPath;
                }
                else
                {
                    fsd.InitialDirectory = pathText.Text;
                }
                if (fsd.ShowDialog(this.Handle))
                {
                    textBCpath.Text = fsd.FileName;
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBCpath.Text = fbd.SelectedPath;
                }
            }
        }
        //选择印章图片
        private void GzPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "图片文件|*.jpg;*.png";
            file.Multiselect = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                string filename;
                StreamWriter sw = File.AppendText(yzLog);
                foreach (string filePath in file.FileNames)
                {
                    filename = System.IO.Path.GetFileName(filePath);//文件名
                    dtYz.Rows.Add(new object[] { filename, filePath });
                    sw.WriteLine(filePath);
                }
                sw.Close();
                sw.Dispose();

                //// 保存当前选中的项
                //object selectedItem = comboBoxYz.SelectedItem;

                //// 更新ComboBox的数据源
                //List<string> items = new List<string>();
                //foreach (string item in comboBoxYz.Items)
                //{
                //    items.Add(item);
                //}
                //items.Reverse();
                //comboBoxYz.DataSource = items;

                // 将之前保存的选中项重新设置为ComboBox的选中项
                //comboBoxYz.SelectedItem = selectedItem;

                //// 将ComboBox的SelectedIndex设置为0
                comboBoxYz.SelectedIndex = comboBoxYz.Items.Count - 1; //每次导入章图片后，并选定到最后一张，也就是最新导入的图片
            }
        }
        //预览图定位，点击大图片控件的位置，定义小图片控件的位置
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                Point pt = pictureBox1.PointToClient(Control.MousePosition); //先记录鼠标位置
                Size overlaySize = GetCurrentPreviewOverlaySize();
                pt.X = pt.X - overlaySize.Width / 2;
                pt.Y = pt.Y - overlaySize.Height / 2;
                int picw = Math.Max(0, pictureBox1.Width - overlaySize.Width);
                int pich = Math.Max(0, pictureBox1.Height - overlaySize.Height);

                if (pt.X < 0) pt.X = 0;
                if (pt.Y < 0) pt.Y = 0;
                if (pt.X > picw) pt.X = picw;
                if (pt.Y > pich) pt.Y = pich;
                float px = picw == 0 ? 0f : 1f * pt.X / picw;
                float py = pich == 0 ? 0f : 1f * pt.Y / pich;

                textPx.Text = px.ToString("#0.0000");
                textPy.Text = py.ToString("#0.0000");
                DataRow[] arrRow = dtPos.Select("Path = '" + previewPath + "' and Page = " + imgStartPage);
                if (arrRow == null || arrRow.Length == 0)
                {
                    dtPos.Rows.Add(new object[] { previewPath, imgStartPage, px, py });
                }
                else
                {
                    DataRow dr = arrRow[0];
                    dr.BeginEdit();
                    dr["X"] = px;
                    dr["Y"] = py;
                    dr.EndEdit();
                    dtPos.AcceptChanges();
                }

                RefreshPreviewOverlay(px, py, true);
            }
            catch (Exception)
            {
                MessageBox.Show("请先加载pdf再预览，偷个懒不再提供无pdf预览功能");
            }
        }

        //文件/目录模式切换
        private void comboType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            pathText.Text = "";
            textBCpath.Text = "";
            dt.Rows.Clear();
            if (comboType.SelectedIndex == 0)
            {
                label1.Text = "请选择需要盖章的PDF文件所在目录";
                label2.Text = "请选择PDF盖章后所保存的目录";
                isSaveSources.Enabled = true;
            }
            else
            {
                label1.Text = "请选择需要盖章的PDF文件(支持多选)";
                label2.Text = "请选择PDF盖章后所保存的目录";
                isSaveSources.Enabled = false;

            }
        }

        //显示指定PDF页
        public async Task viewPDFPage()
        {
            if (viewPdfimgs == null || previewPageCache == null)
            {
                return;
            }

            Bitmap pageImage;
            try
            {
                pageImage = await GetOrLoadPreviewPageAsync(imgStartPage - 1);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            Point point = new Point(pictureBox1.Location.X + pictureBox1.Width / 2, pictureBox1.Location.Y + pictureBox1.Height / 2);
            if (pageImage.Width < pageImage.Height)
            {
                pictureBox1.Height = maximg;
                pictureBox1.Width = pictureBox1.Height * pageImage.Width / pageImage.Height;
            }
            else
            {
                pictureBox1.Width = maximg;
                pictureBox1.Height = pictureBox1.Width * pageImage.Height / pageImage.Width;
            }
            pictureBox1.Location = new Point(point.X - pictureBox1.Width / 2, point.Y - pictureBox1.Height / 2);
            pictureBox1.Image = pageImage;
            labelPage.Text = imgStartPage + "/" + imgPageCount;
            pictureBox2.Visible = true;

            float px, py;
            DataRow[] arrRow = dtPos.Select("Path = '" + previewPath + "' and Page = " + imgStartPage);
            if (arrRow == null || arrRow.Length == 0)
            {
                if (comboYz.SelectedIndex == 4 || comboQfz.SelectedIndex == 4)  //随意骑缝章或自定义加印章，1尾页加章，2首页加章,3所有也加章，4自定义加鼠标点击
                {
                    pictureBox2.Visible = false;
                }
                else if (comboYz.SelectedIndex == 1 && imgStartPage == 1 && imgPageCount > 1)   //首页不加印章，首页不显示，只有一页则会显示
                {
                    pictureBox2.Visible = false;
                }
                else if (comboYz.SelectedIndex == 2 && imgStartPage == imgPageCount && imgPageCount > 1) //尾页不加印章，尾页不显示章，只有1页也会显示
                {
                    pictureBox2.Visible = false;
                }
                px = Convert.ToSingle(textPx.Text);//这里根据比例来定位
                py = Convert.ToSingle(textPy.Text);//这里根据比例来定位
            }
            else
            {
                DataRow dr = arrRow[0];
                px = Convert.ToSingle(dr["X"].ToString());
                py = Convert.ToSingle(dr["Y"].ToString());
            }
            RefreshPreviewOverlay(px, py, pictureBox2.Visible);

            if (imgStartPage == 1)
            {
                buttonUp.Enabled = false;
            }
            else
            {
                buttonUp.Enabled = true;
            }
            if (imgStartPage == imgPageCount)
            {
                buttonNext.Enabled = false;
            }
            else
            {
                buttonNext.Enabled = true;
            }
            comboBoxPages.Text = "" + imgStartPage;
        }

        //跳转到指定页
        private void comboBoxPages_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
            string pageIndex = comboBoxPages.SelectedValue.ToString();
            imgStartPage = int.Parse(pageIndex);
            _ = viewPDFPage();
        }

        //上一页
        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (viewPdfimgs != null&&imgStartPage > 1)
            {
                imgStartPage--;
                _ = viewPDFPage();
            }
        }
        //下一页
        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (viewPdfimgs != null&&imgStartPage < imgPageCount)
            {
                imgStartPage++;
                _ = viewPDFPage();
            }
        }
        //没啥用的功能，双击显示和隐藏印章位置
        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            if (comboYz.SelectedIndex == 4)
            {
                pictureBox2.Visible = false;
                DataRow[] arrRow = dtPos.Select("Path = '" + previewPath + "' and Page = " + imgStartPage);
                if(arrRow != null || arrRow.Length > 0)
                {
                    try
                    {
                        dtPos.Rows.Remove(arrRow[0]);
                    }
                    catch
                    {

                    }
                    
                }  
            }
        }

        private void pathText_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;//调用DragDrop事件
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pathText_DragDrop(object sender, DragEventArgs e)
        {
            string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);//拖放的多个文件的路径列表

            if (Directory.Exists(filePaths[0])&&comboType.SelectedIndex==0)
            {
                this.pathText.Text = filePaths[0];
                this.textBCpath.Text = filePaths[0] + "\\已盖章";
                dt.Rows.Clear();
                dt.Rows.Add(new object[] { "", "" });
                DirectoryInfo dir = new DirectoryInfo(pathText.Text);
                var fileInfos = dir.GetFiles("*.pdf",SearchOption.AllDirectories);
                foreach (var fileInfo in fileInfos)
                {
                    if (fileInfo.Extension == ".pdf")
                    {
                        dt.Rows.Add(new object[] { fileInfo.Name, fileInfo.FullName });
                    }
                }
            }
            else
            {
                string pdfPaths = "";

                foreach (string filePath in filePaths)
                {
                    string extension = System.IO.Path.GetExtension(filePath);//文件后缀名
                    if (extension == ".pdf")
                    {
                        //todo you code
                        pdfPaths += filePath + ",";
                    }
                }
                if(pdfPaths.Length > 0)
                {
                    pdfPaths = pdfPaths.Substring(0, pdfPaths.Length - 1);
                    this.pathText.Text = pdfPaths;
                    this.textBCpath.Text = System.IO.Path.GetDirectoryName(filePaths[0]);

                    string[] files = pdfPaths.Split(',');
                    dt.Rows.Clear();
                    dt.Rows.Add(new object[] { "", "" });
                    foreach (string file in files)
                    {
                        string filename = System.IO.Path.GetFileName(file.ToString());//文件名
                        dt.Rows.Add(new object[] { filename, file });
                    }

                    //解决拖拽完成后，pdf文件列表空白不显示，预览pdf文件名的问题
                    if (comboPDFlist.SelectedIndex != -1 && comboPDFlist.Items.Count > 0)//0是空白行
                    {
                        comboPDFlist.SelectedIndex = comboPDFlist.Items.Count - 1;

                        //// 手动创建一个 EventArgs 对象
                        //EventArgs eventArgs = new EventArgs();
                        //// 调用 comboPDFlist_SelectionChangeCommitted 事件处理程序
                        //comboPDFlist_SelectionChangeCommitted(comboPDFlist, eventArgs);
                    }

                }
            }
            
        }

        //数字签名类型
        private void comboQmtype_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboQmtype.SelectedIndex == 0)
            {
                labelname.Text = "签名";
                textname.Text = "";
                textpass.Text = "";
                textname.ReadOnly = true;
                textpass.ReadOnly = true;
            }
            else if (comboQmtype.SelectedIndex == 1)
            {
                
                textpass.Text = "";
                if (!File.Exists(certDefaultPath))
                {
                    labelname.Text = "签名";
                    textname.Text = "";
                    textname.ReadOnly = false;
                }
                else
                {
                    labelname.Text = "证书";
                    textname.Text = certDefaultPath;
                    textname.ReadOnly = true;
                }
                    
                textpass.ReadOnly = false;
            }
            else
            {
                labelname.Text = "证书";
                textname.Text = "";
                textpass.Text = "";
                textname.ReadOnly = true;
                textpass.ReadOnly = false;

                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "证书文件|*.pfx";
                if (file.ShowDialog() == DialogResult.OK)
                {
                    textname.Text = file.FileName;
                }
                else
                {
                    comboQmtype.SelectedIndex = 0;
                    labelname.Text = "签名";
                    textname.ReadOnly = true;
                    textpass.ReadOnly = true;
                }
            }
            
        }
        private async Task LoadSelectedPreviewAsync()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }

            cts = new CancellationTokenSource();
            dtPages.Rows.Clear();
            previewPath = comboPDFlist.SelectedValue == null ? "" : comboPDFlist.SelectedValue.ToString();
            ClearPreviewResources();

            if (!string.IsNullOrWhiteSpace(previewPath))
            {
                previewPdfFile = PDFFile.Open(previewPath);
                imgStartPage = 1;
                imgPageCount = previewPdfFile.PageCount;
                viewPdfimgs = new Bitmap[imgPageCount];
                previewPageCache = new PageCache<Bitmap>(pageIndex => previewPdfFile.GetPageImage(pageIndex, 72));

                for (int i = 1; i <= imgPageCount; i++)
                {
                    dtPages.Rows.Add(new object[] { i, i });
                }

                viewPdfimgs[0] = await GetOrLoadPreviewPageAsync(0);
                await viewPDFPage();
                return;
            }

            ResetPreviewDisplay();
        }

        private async Task<Bitmap> GetOrLoadPreviewPageAsync(int pageIndex)
        {
            if (viewPdfimgs[pageIndex] != null)
            {
                return viewPdfimgs[pageIndex];
            }

            CancellationToken token = cts != null ? cts.Token : CancellationToken.None;
            Bitmap pageImage = await Task.Run(() => previewPageCache.GetPage(pageIndex), token);
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            viewPdfimgs[pageIndex] = pageImage;
            return pageImage;
        }

        private Bitmap GetOrLoadPreviewPage(int pageIndex)
        {
            if (viewPdfimgs[pageIndex] != null)
            {
                return viewPdfimgs[pageIndex];
            }

            Bitmap pageImage = previewPageCache.GetPage(pageIndex);
            viewPdfimgs[pageIndex] = pageImage;
            return pageImage;
        }

        private void ClearPreviewResources()
        {
            CancelPreviewOverlayRefresh();
            ReplacePreviewOverlayImage(null);
            pictureBox1.Image = null;

            if (previewPageCache != null)
            {
                previewPageCache.Dispose();
                previewPageCache = null;
            }

            if (previewPdfFile != null)
            {
                previewPdfFile.Dispose();
                previewPdfFile = null;
            }

            ClearTransparentStampCache();
        }

        private void ResetPreviewDisplay()
        {
            viewPdfimgs = null;
            imgStartPage = 1;
            imgPageCount = 1;
            labelPage.Text = "1/1";
            Bitmap bmp = new Bitmap(358, 500);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, new System.Drawing.Rectangle(0, 0, 358, 500));
            g.Dispose();
            pictureBox1.Image = bmp;
            ApplyIdleStampOverlaySize();
            buttonUp.Enabled = false;
            buttonNext.Enabled = false;
        }

        private void ApplyIdleStampOverlaySize()
        {
            pictureBox2.Size = new Size(yzr * 2, yzr * 2);

            if (comboBoxYz.SelectedValue == null)
            {
                return;
            }

            string selectedStampPath = comboBoxYz.SelectedValue.ToString();
            if (string.IsNullOrWhiteSpace(selectedStampPath) || !File.Exists(selectedStampPath))
            {
                return;
            }

            using (Bitmap stampImage = new Bitmap(selectedStampPath))
            {
                Size overlaySize = PreviewStampLayout.CalculateOverlaySize(
                    configuredWidthMm: GetPreviewSizeValue(),
                    imageDpi: stampImage.HorizontalResolution,
                    imagePixelWidth: stampImage.Width,
                    imagePixelHeight: stampImage.Height,
                    previewWidth: pictureBox1.Width,
                    pdfWidth: pictureBox1.Width,
                    fallbackSquareSize: yzr * 2);
                pictureBox2.Size = overlaySize;
            }
        }

        private void previewOverlayStyle_Changed(object sender, EventArgs e)
        {
            if (ReferenceEquals(sender, textCC))
            {
                NormalizeUnsignedPreviewTextBox(textCC, false);
            }
            else if (ReferenceEquals(sender, textOpacity))
            {
                NormalizeUnsignedPreviewTextBox(textOpacity, false);
            }
            else if (ReferenceEquals(sender, textRotation))
            {
                NormalizeSignedPreviewTextBox(textRotation, false);
            }

            RefreshPreviewOverlay();
        }

        private void RefreshPreviewOverlay()
        {
            float px;
            float py;
            bool visible = TryGetCurrentOverlayPosition(out px, out py);
            if (!visible)
            {
                px = 0f;
                py = 0f;
            }

            RefreshPreviewOverlay(px, py, visible);
        }

        private void RefreshPreviewOverlay(float px, float py, bool visible)
        {
            QueuePreviewOverlayRefresh(CreatePreviewOverlayRequest(px, py, visible));
        }

        private PreviewOverlayRequest CreatePreviewOverlayRequest(float px, float py, bool visible)
        {
            return new PreviewOverlayRequest
            {
                Px = px,
                Py = py,
                Visible = visible,
                StampPath = comboBoxYz.SelectedValue == null ? "" : comboBoxYz.SelectedValue.ToString(),
                SizeMm = GetPreviewSizeValue(),
                Opacity = GetPreviewOpacityValue(),
                Rotation = GetPreviewRotationValue(),
                WhiteTransparencyTolerance = GetWhiteTransparencyTolerance(),
                UseWhiteTransparency = cbxTransColor.Checked,
                UseOriginalRotationCrop = qbflag == 0,
                PreviewWidth = pictureBox1.Width,
                PdfWidth = pictureBox1.Image == null ? pictureBox1.Width : pictureBox1.Image.Width,
                FallbackSquareSize = yzr * 2
            };
        }

        private void QueuePreviewOverlayRefresh(PreviewOverlayRequest request)
        {
            int requestId = previewOverlayRequestGate.BeginNext();
            CancellationTokenSource nextPreviewOverlayCts = new CancellationTokenSource();
            CancellationTokenSource previousPreviewOverlayCts = Interlocked.Exchange(ref previewOverlayCts, nextPreviewOverlayCts);
            if (previousPreviewOverlayCts != null)
            {
                previousPreviewOverlayCts.Cancel();
                previousPreviewOverlayCts.Dispose();
            }

            _ = RenderPreviewOverlayAsync(request, requestId, nextPreviewOverlayCts.Token);
        }

        private async Task RenderPreviewOverlayAsync(PreviewOverlayRequest request, int requestId, CancellationToken token)
        {
            PreviewOverlayRenderResult renderResult = null;
            try
            {
                renderResult = await Task.Run(() => CreatePreviewOverlayRenderResult(request, token), token);
                token.ThrowIfCancellationRequested();

                if (!previewOverlayRequestGate.IsCurrent(requestId) || IsDisposed || !IsHandleCreated)
                {
                    return;
                }

                PreviewOverlayRenderResult completedRenderResult = renderResult;
                BeginInvoke(new Action(() => ApplyPreviewOverlayRenderResult(request, requestId, completedRenderResult)));
                renderResult = null;
            }
            catch (OperationCanceledException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            finally
            {
                if (renderResult != null)
                {
                    renderResult.Dispose();
                }
            }
        }

        private PreviewOverlayRenderResult CreatePreviewOverlayRenderResult(PreviewOverlayRequest request, CancellationToken token)
        {
            Size overlaySize = new Size(request.FallbackSquareSize, request.FallbackSquareSize);
            if (string.IsNullOrWhiteSpace(request.StampPath) || !File.Exists(request.StampPath))
            {
                return PreviewOverlayRenderResult.Empty(overlaySize);
            }

            using (Bitmap sourceStamp = new Bitmap(request.StampPath))
            {
                overlaySize = PreviewStampLayout.CalculateOverlaySize(
                    configuredWidthMm: request.SizeMm,
                    imageDpi: sourceStamp.HorizontalResolution,
                    imagePixelWidth: sourceStamp.Width,
                    imagePixelHeight: sourceStamp.Height,
                    previewWidth: request.PreviewWidth,
                    pdfWidth: request.PdfWidth,
                    fallbackSquareSize: request.FallbackSquareSize);

                Bitmap processedStamp = new Bitmap(sourceStamp);
                if (request.UseWhiteTransparency)
                {
                    processedStamp.Dispose();
                    processedStamp = GetTransparentPreviewStamp(request.StampPath, request.WhiteTransparencyTolerance);
                }

                token.ThrowIfCancellationRequested();

                if (request.Opacity < 100)
                {
                    processedStamp = SetImageOpacity(processedStamp, request.Opacity);
                }

                if (request.Rotation != 0)
                {
                    processedStamp = RotateImg(processedStamp, request.Rotation, request.UseOriginalRotationCrop);
                }

                token.ThrowIfCancellationRequested();
                return PreviewOverlayRenderResult.Success(processedStamp, overlaySize);
            }
        }

        private void ApplyPreviewOverlayRenderResult(PreviewOverlayRequest request, int requestId, PreviewOverlayRenderResult renderResult)
        {
            using (renderResult)
            {
                if (renderResult == null || !previewOverlayRequestGate.IsCurrent(requestId) || IsDisposed)
                {
                    return;
                }

                if (!renderResult.HasPreview)
                {
                    ReplacePreviewOverlayImage(null);
                    ApplyIdleStampOverlaySize();
                    pictureBox2.Visible = false;
                    return;
                }

                ReplacePreviewOverlayImage(renderResult.DetachBitmap());
                pictureBox2.Size = renderResult.OverlaySize;
                pictureBox2.Visible = request.Visible;

                if (request.Visible)
                {
                    PositionPreviewOverlay(request.Px, request.Py);
                }
            }
        }

        private void ReplacePreviewOverlayImage(Bitmap newImage)
        {
            if (ReferenceEquals(pictureBox2.Image, newImage))
            {
                return;
            }

            System.Drawing.Image oldImage = pictureBox2.Image;
            pictureBox2.Image = newImage;
            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }

        private void PositionPreviewOverlay(float px, float py)
        {
            Size overlaySize = GetCurrentPreviewOverlaySize();
            int picw = Math.Max(0, pictureBox1.Width - overlaySize.Width);
            int pich = Math.Max(0, pictureBox1.Height - overlaySize.Height);
            int x = picw == 0 ? 0 : Convert.ToInt32(picw * px);
            int y = pich == 0 ? 0 : Convert.ToInt32(pich * py);
            pictureBox2.Location = new Point(x, y);
        }

        private Size GetCurrentPreviewOverlaySize()
        {
            return pictureBox2.Size.IsEmpty ? new Size(yzr * 2, yzr * 2) : pictureBox2.Size;
        }

        private bool TryGetCurrentOverlayPosition(out float px, out float py)
        {
            px = Convert.ToSingle(textPx.Text);
            py = Convert.ToSingle(textPy.Text);

            if (viewPdfimgs == null || previewPageCache == null || imgStartPage <= 0)
            {
                return false;
            }

            DataRow[] arrRow = dtPos.Select("Path = '" + previewPath + "' and Page = " + imgStartPage);
            if (arrRow != null && arrRow.Length > 0)
            {
                DataRow dr = arrRow[0];
                px = Convert.ToSingle(dr["X"].ToString());
                py = Convert.ToSingle(dr["Y"].ToString());
                return true;
            }

            if (comboYz.SelectedIndex == 4 || comboQfz.SelectedIndex == 4)
            {
                return false;
            }

            if (comboYz.SelectedIndex == 1 && imgStartPage == 1 && imgPageCount > 1)
            {
                return false;
            }

            if (comboYz.SelectedIndex == 2 && imgStartPage == imgPageCount && imgPageCount > 1)
            {
                return false;
            }

            return true;
        }

        private int GetPreviewSizeValue()
        {
            return PreviewValueSanitizer.ClampInt(textCC.Text, 40, 0, 100);
        }

        private int GetPreviewOpacityValue()
        {
            return PreviewValueSanitizer.ClampInt(textOpacity.Text, 60, 0, 100);
        }

        private int GetPreviewRotationValue()
        {
            return PreviewValueSanitizer.ClampInt(textRotation.Text, 0, -360, 360);
        }

        private int GetWhiteTransparencyTolerance()
        {
            return WhiteTransparencyHelper.ClampTolerance(PreviewValueSanitizer.ClampInt(txtAllow.Text, 20, 0, 50));
        }

        private Bitmap GetTransparentPreviewStamp(string stampPath, int tolerance)
        {
            string cacheKey = stampPath + "|" + tolerance.ToString();
            lock (transparentStampCacheSync)
            {
                if (cachedTransparentStamp == null || cachedTransparentStampKey != cacheKey)
                {
                    ClearTransparentStampCache();
                    using (Bitmap sourceStamp = new Bitmap(stampPath))
                    {
                        cachedTransparentStamp = WhiteTransparencyHelper.Apply(sourceStamp, tolerance);
                    }

                    cachedTransparentStampKey = cacheKey;
                }

                return new Bitmap(cachedTransparentStamp);
            }
        }

        private void ClearTransparentStampCache()
        {
            lock (transparentStampCacheSync)
            {
                if (cachedTransparentStamp != null)
                {
                    cachedTransparentStamp.Dispose();
                    cachedTransparentStamp = null;
                }

                cachedTransparentStampKey = "";
            }
        }

        private void CancelPreviewOverlayRefresh()
        {
            CancellationTokenSource currentPreviewOverlayCts = Interlocked.Exchange(ref previewOverlayCts, null);
            if (currentPreviewOverlayCts != null)
            {
                currentPreviewOverlayCts.Cancel();
                currentPreviewOverlayCts.Dispose();
            }
        }

        private void previewUnsigned_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void previewSigned_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            bool isMinus = e.KeyChar == '-';
            bool isControl = e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete;
            bool allowLeadingMinus = isMinus && textBox.SelectionStart == 0 && !textBox.Text.Contains("-");
            if (!char.IsDigit(e.KeyChar) && !isControl && !allowLeadingMinus)
            {
                e.Handled = true;
            }
        }

        private void previewSize_Leave(object sender, EventArgs e)
        {
            NormalizeUnsignedPreviewTextBox(textCC, true);
            RefreshPreviewOverlay();
        }

        private void previewOpacity_Leave(object sender, EventArgs e)
        {
            NormalizeUnsignedPreviewTextBox(textOpacity, true);
            RefreshPreviewOverlay();
        }

        private void previewRotation_Leave(object sender, EventArgs e)
        {
            NormalizeSignedPreviewTextBox(textRotation, true);
            RefreshPreviewOverlay();
        }

        private void NormalizeUnsignedPreviewTextBox(TextBox textBox, bool clamp)
        {
            string filtered = PreviewValueSanitizer.KeepDigitsOnly(textBox.Text);
            if (filtered != textBox.Text)
            {
                int selectionStart = textBox.SelectionStart;
                textBox.Text = filtered;
                textBox.SelectionStart = Math.Min(selectionStart, textBox.Text.Length);
            }

            if (clamp)
            {
                int value = ReferenceEquals(textBox, textCC) ? GetPreviewSizeValue() : GetPreviewOpacityValue();
                int fallbackValue = ReferenceEquals(textBox, textCC) ? 40 : 60;
                textBox.Text = (textBox.Text.Length == 0 ? fallbackValue : value).ToString();
            }
        }

        private void NormalizeSignedPreviewTextBox(TextBox textBox, bool clamp)
        {
            string filtered = PreviewValueSanitizer.KeepSignedInteger(textBox.Text);
            if (filtered != textBox.Text)
            {
                int selectionStart = textBox.SelectionStart;
                textBox.Text = filtered;
                textBox.SelectionStart = Math.Min(selectionStart, textBox.Text.Length);
            }

            if (clamp)
            {
                textBox.Text = GetPreviewRotationValue().ToString();
            }
        }
        //根据印章类型切换窗口大小
        private void comboYz_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ApplyPreviewPanelLayout(comboYz.SelectedIndex, comboQfz.SelectedIndex);
        }


        private void comboQfz_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ApplyPreviewPanelLayout(comboYz.SelectedIndex, comboQfz.SelectedIndex);
        }

        private void ApplyPreviewPanelLayout(int stampType, int seamStampType)
        {
            bool showPreviewPanel = PreviewPanelLayout.ShouldShowPreviewPanel(stampType, seamStampType);
            int targetClientWidth = showPreviewPanel ? expandedClientWidth : collapsedClientWidth;
            this.ClientSize = new Size(targetClientWidth, this.ClientSize.Height);
        }
        //只允许录入数字
        private void txtAllow_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 允许数字、退格键（Backspace）和删除键（Delete）
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;   // 拦截非法字符
            }
        }
        //防粘右键粘贴非数字
        private void txtAllow_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string filtered = new string(tb.Text.Where(c => char.IsDigit(c)).ToArray());

            if (tb.Text != filtered)
            {
                int sel = tb.SelectionStart;
                tb.Text = filtered;
                tb.SelectionStart = Math.Min(sel, tb.Text.Length);
            }

            ClearTransparentStampCache();
            RefreshPreviewOverlay();
        }

        private sealed class PreviewOverlayRequest
        {
            public float Px { get; set; }
            public float Py { get; set; }
            public bool Visible { get; set; }
            public string StampPath { get; set; }
            public int SizeMm { get; set; }
            public int Opacity { get; set; }
            public int Rotation { get; set; }
            public int WhiteTransparencyTolerance { get; set; }
            public bool UseWhiteTransparency { get; set; }
            public bool UseOriginalRotationCrop { get; set; }
            public int PreviewWidth { get; set; }
            public int PdfWidth { get; set; }
            public int FallbackSquareSize { get; set; }
        }

        private sealed class PreviewOverlayRenderResult : IDisposable
        {
            private PreviewOverlayRenderResult(Bitmap bitmap, Size overlaySize, bool hasPreview)
            {
                Bitmap = bitmap;
                OverlaySize = overlaySize;
                HasPreview = hasPreview;
            }

            public Bitmap Bitmap { get; private set; }
            public Size OverlaySize { get; }
            public bool HasPreview { get; }

            public static PreviewOverlayRenderResult Empty(Size overlaySize)
            {
                return new PreviewOverlayRenderResult(null, overlaySize, false);
            }

            public static PreviewOverlayRenderResult Success(Bitmap bitmap, Size overlaySize)
            {
                return new PreviewOverlayRenderResult(bitmap, overlaySize, true);
            }

            public Bitmap DetachBitmap()
            {
                Bitmap bitmap = Bitmap;
                Bitmap = null;
                return bitmap;
            }

            public void Dispose()
            {
                if (Bitmap != null)
                {
                    Bitmap.Dispose();
                    Bitmap = null;
                }
            }
        }
    }
}
