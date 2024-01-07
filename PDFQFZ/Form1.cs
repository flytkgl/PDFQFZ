using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using O2S.Components.PDFRender4NET;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.exceptions;
using Org.BouncyCastle.Crypto.Generators;
using PDFQFZ.Library;
using System.Text;

namespace PDFQFZ
{
    public partial class Form1 : Form
    {
        int fw, fh,imgStartPage=0, imgPageCount=0;
        string certDefaultPath = $@"{Application.StartupPath}\pdfqfz.pfx";//证书默认存放地址
        string yzLog = $@"{Application.StartupPath}\yz.log";//获取印章记录
        DataTable dt = new DataTable();//PDF列表
        DataTable dtPos = new DataTable();//PDF各文件印章位置表
        DataTable dtYz = new DataTable();//PDF列表
        string sourcePath = "",outputPath = "",imgPath = "",previewPath = "",signText = "", password="",pdfpassword="";
        int wjType = 1, qfzType = 0, yzType = 0, djType = 0, qmType = 0, wzType = 3, yzIndex = -1, qbflag = 0, size = 40, rotation = 0, opacity = 100, wz = 50, yzr = 30, maximg = 500, maxfgs = 20;
        Bitmap imgYz = null;
        X509Certificate2 cert = null;//证书
        float xzbl = 1f;
        private string strIniFilePath = $@"{Application.StartupPath}\config.ini";//获取INI文件路径


        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)//把DLL打包到EXE需要用到
        {
            string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) return null;
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            return System.Reflection.Assembly.Load(bytes);
        }
        public Form1()
        {
            //在InitializeComponent()之前调用

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            InitializeComponent();
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
                string QmType = iniFileHelper.ContentValue(section, "qmType");//签名类型
                string WzType = iniFileHelper.ContentValue(section, "wzType");//骑缝章位置类型
                string Qbflag = iniFileHelper.ContentValue(section, "qbflag");//是否切边标记
                string Size = iniFileHelper.ContentValue(section, "size");//印章尺寸
                string Rotation = iniFileHelper.ContentValue(section, "rotation");//旋转角度
                string Opacity = iniFileHelper.ContentValue(section, "opacity");//不透明度
                string Wz = iniFileHelper.ContentValue(section, "wz");//骑缝章位置
                string Maxfgs = iniFileHelper.ContentValue(section, "maxfgs");//骑缝章最大分割数
                string YzIndex = iniFileHelper.ContentValue(section, "yzIndex");//选择的印章索引
                signText = iniFileHelper.ContentValue(section, "signText");//签名
                wjType = Int32.Parse(WjType);
                qfzType = Int32.Parse(QfzType);
                yzType = Int32.Parse(YzType);
                djType = Int32.Parse(DjType);
                qmType = Int32.Parse(QmType);
                wzType = Int32.Parse(WzType);
                qbflag = Int32.Parse(Qbflag);
                size = Int32.Parse(Size);
                rotation = Int32.Parse(Rotation);
                opacity = Int32.Parse(Opacity);
                wz = Int32.Parse(Wz);
                maxfgs = Int32.Parse(Maxfgs);
                yzIndex = Int32.Parse(YzIndex);
            }
            fw = this.Width;
            fh = this.Height;
            if (yzType != 0 || qfzType == 4)
            {
                this.Size = new Size(fw + 517, fh);
            }
            else
            {
                this.Size = new Size(fw, fh);
            }
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
            textname.Text = signText;
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
            dtYz.Columns.Add("Name", typeof(string));
            dtYz.Columns.Add("Value", typeof(string));
            comboBoxYz.DisplayMember = "Name";
            comboBoxYz.ValueMember = "Value";
            comboBoxYz.DataSource = dtYz;
            if (!File.Exists(yzLog))
            {
                yzIndex = -1;
                //为了避免误删印章记录,然后下次打开又不盖章,再打开可能的报错,这里先重置下印章索引
                IniFileHelper iniFileHelper = new IniFileHelper(strIniFilePath);
                string section = "config";
                iniFileHelper.WriteIniString(section, "yzIndex", yzIndex.ToString());

                File.Create(yzLog).Close();
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
            dtPos.Columns.Add("Path", typeof(string));
            dtPos.Columns.Add("Page", typeof(int));
            dtPos.Columns.Add("X", typeof(float));
            dtPos.Columns.Add("Y", typeof(float));
            isSaveSources.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wjType = comboType.SelectedIndex;
            qfzType = comboQfz.SelectedIndex;
            yzType = comboYz.SelectedIndex;
            djType = comboDJ.SelectedIndex;
            qmType = comboQmtype.SelectedIndex;
            wzType = comboBoxWZ.SelectedIndex;
            qbflag = comboBoxQB.SelectedIndex;
            yzIndex = comboBoxYz.SelectedIndex;

            if (qfzType == 1&& yzType == 0 && qmType == 0)
            {
                if (MessageBox.Show("你既不盖骑缝章又不盖印章还不要我签名是想让我帮你关机吗?","你想干嘛", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    MessageBox.Show("滚蛋吧,我才不帮你关呢,哼!");
                    System.Environment.Exit(0);
                }
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
                        MessageBox.Show("印章尺寸设置错误,请输入正确的比例或尺寸。");
                    }
                    else if(!int.TryParse(textRotation.Text,out rotation))
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
                        string section = "config";
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
                        iniFileHelper.WriteIniString(section, "signText", signText);
                    }
                }
                else
                {
                    MessageBox.Show("文件路径不能为空，请先选择路径。");
                }
            }
            
        }

        private void pdfGz()
        {
            if (!Directory.Exists(outputPath))//输出目录不存在则新建
            {
                Directory.CreateDirectory(outputPath);
            }

            log.Text = "";//清空日志

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
                //默认把印章的白色部分重置为透明
                imgYz = SetWhiteToTransparent(imgYz);
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

                //目录模式还是文件模式
                if (wjType == 0)
                {
                    DirectoryInfo dir = new DirectoryInfo(sourcePath);
                    var fileInfos = dir.GetFiles("*.pdf",SearchOption.AllDirectories);
                    foreach (var fileInfo in fileInfos)
                    {
                        if(fileInfo.Extension == ".pdf")
                        {
                            string source = fileInfo.DirectoryName + "\\" + fileInfo.Name;
                            string output = outputPath + "\\" + fileInfo.Name;
                            if (isSaveSources.Checked == true)
                            {
                                output = fileInfo.DirectoryName + "\\" + System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name) + "_已盖章" + fileInfo.Extension;

                            }

                            bool isSurrcess = PDFWatermark(source, output);
                            if (isSurrcess&&djType==1)
                            {
                                PDFToiPDF(output);
                            }
                            if (isSurrcess)
                            {
                                log.Text = log.Text + "成功！“" + fileInfo.Name + "”盖章完成！\r\n";
                            }
                            else
                            {
                                log.Text = log.Text + "失败！“" + fileInfo.Name + "”盖章失败！\r\n";
                            }
                        }
                    }
                }
                else
                {
                    string[] fileArray = sourcePath.Split(',');//字符串转数组
                    foreach (string file in fileArray)
                    {
                        string filename = System.IO.Path.GetFileName(file);//文件名
                        string output = outputPath + "\\" + filename;//输出文件的绝对路径
                        if (outputPath == System.IO.Path.GetDirectoryName(file))
                        {
                            output = outputPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + "_QFZ.pdf";//如果跟源文件在同一个目录需要重命名
                        }
                        string source = file;
                        bool isSurrcess = PDFWatermark(source, output);
                        if (isSurrcess)
                        {
                            if(djType == 1)
                            {
                                PDFToiPDF(output);
                            }
                            if (pdfpassword != "")
                            {
                                string jmoutput = outputPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + "_QFZ_加密.pdf";
                                EncryptPDF(output, jmoutput, pdfpassword);
                            }
                        }
                        if (isSurrcess)
                        {
                            log.Text = log.Text + "成功！“" + filename + "”盖章完成！\r\n";
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
        //设置JPG图片白色为透明
        private Bitmap SetWhiteToTransparent(System.Drawing.Bitmap img)
        {
            Bitmap bitmap = new Bitmap(img);
            // 遍历图片的每个像素
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);

                    // 判断像素颜色是否为白色
                    if (pixelColor.R > 230 && pixelColor.G > 230 && pixelColor.B > 230)
                    {
                        // 将白色像素设置为透明
                        bitmap.SetPixel(x, y, Color.Transparent);
                    }
                }
            }

            return bitmap;
        }
        //设置图片透明度
        private Bitmap SetImageOpacity(System.Drawing.Bitmap srcImage, int opacity)
        {
            opacity = opacity * 255/100;
            Bitmap pic = new Bitmap(srcImage);
            for (int w = 0; w < pic.Width; w++)
            {
                for (int h = 0; h < pic.Height; h++)
                {
                    Color c = pic.GetPixel(w, h);
                    Color newC;
                    if (!c.Equals(Color.FromArgb(0, 0, 0, 0)))
                    {
                        newC = Color.FromArgb(opacity, c);
                    }
                    else
                    {
                        newC = c;
                    }
                    pic.SetPixel(w, h, newC);
                }
            }

            return pic;
        }
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
            int w = W / n;
            n = n - 1;
            int tmpw = W;
            Random random = new Random();
            for (int i = 0; i <= n; i++)
            {
                int sw;
                if(i == n)
                {
                    sw = tmpw;
                }else if(i == 0)
                {
                    sw =w* random.Next(11,15)/ 10;
                }
                else
                {
                    sw = w * random.Next(8, 12) / 10;
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

        //PDF盖章
        private bool PDFWatermark(string inputfilepath, string outputfilepath)
        {
            float sfbl = 100f * size * xzbl * 2.842f / imgYz.Height;

            //PdfGState state = new PdfGState();
            //state.FillOpacity = 0.01f*opacity;//印章图片不透明度

            //throw new NotImplementedException();
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath, new System.Text.UTF8Encoding().GetBytes(pdfpassword));//选择需要印章的pdf
                if (qmType != 0)
                {
                    //最后的true表示保留原签名
                    pdfStamper = PdfStamper.CreateSignature(pdfReader, new FileStream(outputfilepath, FileMode.Create), '\0', null, true);//加完印章后的pdf
                }
                else
                {
                    pdfStamper = new PdfStamper(pdfReader,new FileStream(outputfilepath,FileMode.Create));
                }
                
                int numberOfPages = pdfReader.NumberOfPages;//pdf页数
                int qfzPages = 0;
                List<int> qfzList = new List<int>();
                if (qfzType == 0)
                {
                    for (int i = 1; i <= numberOfPages; i ++)
                    {
                        qfzList.Add(i);
                        qfzPages++;
                    }
                }
                else if(qfzType == 2)
                {
                    for (int i = 1; i <= numberOfPages; i += 2)
                    {
                        qfzList.Add(i);
                        qfzPages++;
                    }
                }
                else if(qfzType == 3)
                {
                    for (int i = 2; i <= numberOfPages; i += 2)
                    {
                        qfzList.Add(i);
                        qfzPages++;
                    }
                }
                else if(qfzType == 4)
                {
                    // 遍历 DataTable 的所有行
                    foreach (DataRow row in dtPos.Rows)
                    {
                        if (row["Path"].ToString()== inputfilepath)
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

                if (yzType!=0|| qmType != 0)
                {
                    iTextSharp.text.Image img = null;
                    float imgW=0, imgH=0;
                    float xPos=0, yPos=0;
                    bool all = false;
                    int signpage = 0;

                    if (yzType != 0)
                    {
                        img = iTextSharp.text.Image.GetInstance(imgYz, System.Drawing.Imaging.ImageFormat.Png);//创建一个图片对象
                        //img.Transparency = new int[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//这里透明背景的图片会变黑色,所以设置黑色为透明
                        //img.RotationDegrees = rotation;
                        img.ScalePercent(sfbl);//设置图片比例
                        imgW = img.Width * sfbl / 100f;
                        imgH = img.Height * sfbl / 100f;
                    }

                    if (yzType == 1)
                    {
                        signpage = numberOfPages;
                    }
                    else if (yzType == 2)
                    {
                        signpage = 1;
                    }
                    else if (yzType == 3)
                    {
                        signpage = numberOfPages;
                        all = true;
                    }
                    else if (yzType == 4)
                    {
                        signpage = numberOfPages;
                        all = true;
                    }

                    for (int i = 1; i <= numberOfPages; i++)
                    {
                        if (all || i == signpage)
                        {
                            waterMarkContent = pdfStamper.GetOverContent(i);//获取当前页内容
                            int rotation = pdfReader.GetPageRotation(i);//获取指定页面的旋转度
                            iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(i);//获取当前页尺寸

                            //waterMarkContent.SaveState();//通过PdfGState调整图片整体的透明度
                            //waterMarkContent.SetGState(state);

                            float wbl = 0;
                            float hbl = 1;
                            DataRow[] arrRow = dtPos.Select("Path = '" + inputfilepath + "' and Page = " + i);
                            if (arrRow == null || arrRow.Length == 0)
                            {
                                if(yzType == 4)//自定义页加印章,如果没有印章定位的页就不盖
                                {
                                    continue;
                                }
                                wbl = Convert.ToSingle(textPx.Text);//这里根据比例来定位
                                hbl = 1 - Convert.ToSingle(textPy.Text);//这里根据比例来定位
                            }
                            else
                            {
                                DataRow dr = arrRow[0];
                                wbl = Convert.ToSingle(dr["X"].ToString());
                                hbl = 1 - Convert.ToSingle(dr["Y"].ToString());
                            }
                            if (rotation == 90 || rotation == 270)
                            {
                                xPos = (psize.Height - imgW) * wbl;
                                yPos = (psize.Width - imgH) * hbl;
                            }
                            else
                            {
                                xPos = (psize.Width - imgW) * wbl;
                                yPos = (psize.Height - imgH) * hbl;
                            }
                            img.SetAbsolutePosition(xPos, yPos);
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


                            //如果不是所有页都盖章,那对应页盖完后直接跳出循环
                            if (!all&&i == signpage)
                            {
                                break;
                            }

                        }
                    }

                    //加数字签名
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
                        if (yzType == 0)
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
                            signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(xPos- bk, yPos- bk, xPos + imgW + bk, yPos + imgH + bk), signpage, null);
                        }

                        MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);
                    }

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
                    textBCpath.Text = pathText.Text +"\\QFZ"; ;
                }
                else if (comboType.SelectedIndex == 1 && pathText.Text != "")
                {
                    textBCpath.Text = System.IO.Path.GetDirectoryName(pathText.Text) + "\\QFZ";
                }
            }

        }

        public void PDFToiPDF(string pdfPath)
        {
            PDFFile pdfFile = PDFFile.Open(pdfPath);
            Bitmap[] bitmaps = new Bitmap[pdfFile.PageCount];
            int dpi = 300;
            for (int i = 0; i < pdfFile.PageCount; i++)
            {
                Bitmap pageImage = pdfFile.GetPageImage(i, dpi);
                bitmaps[i] = pageImage;
                //pageImage.Save("D:\\tmp\\img\\" + i + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
            pdfFile.Dispose();

            float bl = 72f / dpi;//为了尽量保证转换的清晰度,这里需要把电脑的DPI缩放到PDF的DPI

            string tmpPdf = pdfPath;

            if (qmType != 0)
            {
                tmpPdf = System.IO.Path.GetTempPath() + "PDFQFZ_tmp.pdf";
            }
            
            ImageToPDF(bitmaps, bl, tmpPdf);

            if (qmType != 0)
            {
                SignaturePDF(tmpPdf,pdfPath, cert);
            }

        }

        //选择源文件
        private void SelectPath_Click(object sender, EventArgs e)
        {
            if (comboType.SelectedIndex == 0)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    pathText.Text = fbd.SelectedPath;
                    if (textBCpath.Text == "")
                    {
                        textBCpath.Text = fbd.SelectedPath + "\\QFZ";
                    }
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
            }
            else
            {
                OpenFileDialog file = new OpenFileDialog();
                file.Multiselect = true;
                file.Filter = "PDF文件|*.pdf";
                if (file.ShowDialog() == DialogResult.OK)
                {
                    pathText.Text = string.Join(",", file.FileNames);
                    if (textBCpath.Text == "")
                    {
                        textBCpath.Text = System.IO.Path.GetDirectoryName(file.FileName);
                    }
                    dt.Rows.Clear();
                    dt.Rows.Add(new object[] { "", "" });
                    foreach (string filePath in file.FileNames)
                    {
                        string filename = System.IO.Path.GetFileName(filePath);//文件名
                        dt.Rows.Add(new object[] { filename, filePath });
                    }
                }
            }
        }
        //选择保存目录
        private void OutPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBCpath.Text = fbd.SelectedPath;
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
            }
        }
        //预览图定位
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Point pt = pictureBox1.PointToClient(Control.MousePosition);
            pt.X = pt.X - yzr;
            pt.Y = pt.Y - yzr;
            int picw = pictureBox1.Width - 2 * yzr;
            int pich = pictureBox1.Height - 2 * yzr;

            if (pt.X < 0) pt.X = 0;
            if (pt.Y < 0) pt.Y = 0;
            if (pt.X > picw) pt.X = picw;
            if (pt.Y > pich) pt.Y = pich;
            float px = 1f * pt.X / picw;
            float py = 1f * pt.Y / pich;
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

            Point pt1 = pictureBox1.Location;
            pictureBox2.Visible = true;
            pictureBox2.Location = new Point(pt1.X + pt.X, pt1.Y + pt.Y);

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
        //上一页
        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (previewPath != ""&&imgStartPage > 1)
            {
                imgStartPage--;
                PDFFile pdfFile = PDFFile.Open(previewPath);
                Bitmap pageImage = pdfFile.GetPageImage(imgStartPage-1, 56 * 1);
                pdfFile.Dispose();
                pictureBox1.Image = pageImage;
                labelPage.Text = imgStartPage + "/" + imgPageCount;

                float px, py;
                DataRow[] arrRow = dtPos.Select("Path = '" + previewPath + "' and Page = " + imgStartPage);
                if (arrRow == null || arrRow.Length == 0)
                {
                    if (comboYz.SelectedIndex == 4 || comboQfz.SelectedIndex == 4)
                    {
                        pictureBox2.Visible = false;
                    }
                    px = Convert.ToSingle(textPx.Text);//这里根据比例来定位
                    py = Convert.ToSingle(textPy.Text);//这里根据比例来定位
                }
                else
                {
                    pictureBox2.Visible = true;
                    DataRow dr = arrRow[0];
                    px = Convert.ToSingle(dr["X"].ToString());
                    py = Convert.ToSingle(dr["Y"].ToString());
                }
                int X = Convert.ToInt32((pictureBox1.Width - 2 * yzr) * px);
                int Y = Convert.ToInt32((pictureBox1.Height - 2 * yzr) * py);
                Point pt1 = pictureBox1.Location;

                pictureBox2.Location = new Point(pt1.X + X, pt1.Y + Y);

            }
        }
        //下一页
        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (previewPath != ""&&imgStartPage < imgPageCount)
            {
                imgStartPage++;
                PDFFile pdfFile = PDFFile.Open(previewPath);
                Bitmap pageImage = pdfFile.GetPageImage(imgStartPage-1, 56 * 1);
                pdfFile.Dispose();
                pictureBox1.Image = pageImage;
                labelPage.Text = imgStartPage + "/" + imgPageCount;

                float px,py;
                DataRow[] arrRow = dtPos.Select("Path = '" + previewPath + "' and Page = " + imgStartPage);
                if (arrRow == null || arrRow.Length == 0)
                {
                    if (comboYz.SelectedIndex == 4 || comboQfz.SelectedIndex == 4)
                    {
                        pictureBox2.Visible = false;
                    }
                    px = Convert.ToSingle(textPx.Text);//这里根据比例来定位
                    py = Convert.ToSingle(textPy.Text);//这里根据比例来定位
                }
                else
                {
                    pictureBox2.Visible = true;
                    DataRow dr = arrRow[0];
                    px = Convert.ToSingle(dr["X"].ToString());
                    py = Convert.ToSingle(dr["Y"].ToString());
                }
                int X = Convert.ToInt32((pictureBox1.Width - 2 * yzr) * px);
                int Y = Convert.ToInt32((pictureBox1.Height - 2 * yzr) * py);
                Point pt1 = pictureBox1.Location;

                pictureBox2.Location = new Point(pt1.X + X, pt1.Y + Y);

            }
        }

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
                this.textBCpath.Text = filePaths[0] + "\\QFZ";
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
        //选择PDF预览文件
        private void comboPDFlist_SelectionChangeCommitted(object sender, EventArgs e)
        {
            previewPath = comboPDFlist.SelectedValue.ToString();
            if (previewPath != "")
            {
                PDFFile pdfFile = PDFFile.Open(previewPath);
                Bitmap pageImage = pdfFile.GetPageImage(0, 56 * 1);
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
                imgStartPage = 1;
                imgPageCount = pdfFile.PageCount;
                pdfFile.Dispose();
                labelPage.Text = imgStartPage + "/" + imgPageCount;

                float px, py;
                DataRow[] arrRow = dtPos.Select("Path = '" + previewPath + "' and Page = " + imgStartPage);
                if (arrRow == null || arrRow.Length == 0)
                {
                    if (comboYz.SelectedIndex == 4 || comboQfz.SelectedIndex == 4)
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
                int X = Convert.ToInt32((pictureBox1.Width - 2 * yzr) * px);
                int Y = Convert.ToInt32((pictureBox1.Height - 2 * yzr) * py);
                Point pt1 = pictureBox1.Location;

                pictureBox2.Location = new Point(pt1.X + X, pt1.Y + Y);
            }
            else
            {
                imgStartPage = 1;
                imgPageCount = 1;
                labelPage.Text = "1/1";
                Bitmap bmp = new Bitmap(358, 500);
                Graphics g = Graphics.FromImage(bmp);
                g.FillRectangle(Brushes.White, new System.Drawing.Rectangle(0, 0, 358, 500));
                g.Dispose();
                pictureBox1.Image = bmp;
            }
            
        }
        //根据印章类型切换窗口大小
        private void comboYz_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboYz.SelectedIndex != 0 || comboQfz.SelectedIndex == 4)
            {
                this.Size = new Size(fw + 517, fh);
            }
            else
            {
                this.Size = new Size(fw, fh);
            }
        }


        private void comboQfz_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboYz.SelectedIndex != 0 || comboQfz.SelectedIndex == 4)
            {
                this.Size = new Size(fw + 517, fh);
            }
            else
            {
                this.Size = new Size(fw, fh);
            }
        }
    }
}
