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

namespace PDFQFZ
{
    public partial class Form1 : Form
    {
        int fw, fh,imgStartPage=0, imgPageCount=0;
        string certDefaultPath = $@"{Application.StartupPath}\pdfqfz.pfx";//证书默认存放地址
        DataTable dt = new DataTable();//PDF列表
        DataTable dtPos = new DataTable();//PDF各文件印章位置表
        string pdfInputPath = null;//选中的PDF文件路径




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

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboQfz.SelectedIndex == 1&&comboYz.SelectedIndex == 0 && comboQmtype.SelectedIndex == 0)
            {
                if (MessageBox.Show("你既不盖骑缝章又不盖印章还不要我签名是想让我帮你关机吗?","你想干嘛", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    MessageBox.Show("滚蛋吧,我才不帮你关呢,哼!");
                    System.Environment.Exit(0);
                }
            }
            else
            {
                if (pathText.Text != "" && textBCpath.Text != "" && (textGZpath.Text != "" || comboQfz.SelectedIndex==1 && comboYz.SelectedIndex==0))
                {
                    float res;
                    if (!float.TryParse(textBili.Text, out res) || (comboBoxBL.SelectedIndex == 0 && res > 100.00f))
                    {
                        MessageBox.Show("印章尺寸错误,请输入正确的比例或尺寸。");
                    }
                    else if(!float.TryParse(textWzbl.Text,out res)|| res>100.00f)
                    {
                        MessageBox.Show("骑缝章位置错误,请输入100以内的整数。");
                    }
                    else
                    {
                        pdfGz();
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
            
            string sourcepath = pathText.Text;//需盖章目录
            string outputpath = textBCpath.Text;//保存目录
            X509Certificate2 cert = null;//证书


            if (!Directory.Exists(outputpath))//输出目录不存在则新建
            {
                Directory.CreateDirectory(outputpath);
            }

            log.Text = "";//清空日志

            try
            {
                //如果要数字签名,先判断证书能否正常加载
                if (comboQmtype.SelectedIndex!=0)
                {
                    string certPath = null;//证书路径
                    if (comboQmtype.SelectedIndex == 1)
                    {
                        //如果证书不存在就先生成证书
                        if (!File.Exists(certDefaultPath))
                        {
                            var rSA = RSA.Create(4096); // 生成非对称密钥对
                            var req = new CertificateRequest("CN=" + textname.Text, rSA, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                            X509Certificate2 newCert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));//这里生成的证书经测试不能直接用,必须先保存为pfx文件再重新加载

                            // Create PFX (PKCS #12) with private key
                            File.WriteAllBytes(certDefaultPath, newCert.Export(X509ContentType.Pkcs12, textpass.Text));//不同版本的.net Pfx跟Pksc12好像并不一样,所以直接指定成Pksc12更安全

                            // Create Base 64 encoded CER (public key only)
                            //File.WriteAllText("D:\\publicKey.cer","-----BEGIN CERTIFICATE-----\r\n" + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks) + "\r\n-----END CERTIFICATE-----");

                        }
                        certPath = certDefaultPath;
                    }
                    else
                    {
                        certPath = textname.Text;//自定义证书路径
                    }

                    try
                    {
                        cert = new X509Certificate2(certPath, textpass.Text, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);//注意设置第三个参数,不然有权限问题
                    }
                    catch
                    {
                        MessageBox.Show("证书加载失败,请检查证书路径和密码");
                        return;
                    }
                }

                //目录模式还是文件模式
                if(comboType.SelectedIndex == 0)
                {
                    DirectoryInfo dir = new DirectoryInfo(pathText.Text);
                    var fileInfos = dir.GetFiles();
                    foreach (var fileInfo in fileInfos)
                    {
                        if(fileInfo.Extension == ".pdf")
                        {
                            string source = sourcepath + "\\" + fileInfo.Name;
                            string output = outputpath + "\\" + fileInfo.Name;
                            bool isSurrcess = PDFWatermark(source, output, cert);
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
                    string[] fileArray = pathText.Text.Split(',');//字符串转数组
                    foreach (string file in fileArray)
                    {
                        string filename = System.IO.Path.GetFileName(file);//文件名
                        string output = outputpath + "\\" + filename;//输出文件的绝对路径
                        if (outputpath == Path.GetDirectoryName(file))
                        {
                            output = outputpath + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + "_QFZ.pdf";//如果跟源文件在同一个目录需要重命名
                        }
                        string source = file;
                        bool isSurrcess = PDFWatermark(source, output, cert);
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
        //分割图片
        private static Bitmap[] subImages(String imgPath, int n)//图片分割
        {
            Bitmap[] nImage = new Bitmap[n];
            Bitmap img = new Bitmap(imgPath);
            int h = img.Height;
            int w = img.Width;
            int sw = w / n;
            for (int i = 0; i < n; i++)
            {
                Bitmap newbitmap = new Bitmap(sw, h);
                Graphics g = Graphics.FromImage(newbitmap);
                g.DrawImage(img, new System.Drawing.Rectangle(0, 0, sw, h), new System.Drawing.Rectangle(sw * i, 0, sw, h), GraphicsUnit.Pixel);
                g.Dispose();
                nImage[i] = newbitmap;
                //newbitmap.Save("D://" + i +".png", ImageFormat.Png);//查看图片是否正常
            }
            return nImage;
        }
        //PDF盖章
        private bool PDFWatermark(string inputfilepath, string outputfilepath, X509Certificate2 cert)
        {
            string ModelPicName = textGZpath.Text;//印章文件路径
            int zyz = comboYz.SelectedIndex;//加印章方式
            int zqfz = comboQfz.SelectedIndex;//加骑缝章方式
            int sftype = comboBoxBL.SelectedIndex;//印章缩放方式
            float sfsize, qfzwzbl;
            float.TryParse(textBili.Text, out sfsize);//印章缩放尺寸
            float.TryParse(textWzbl.Text, out qfzwzbl);//骑缝章位置比例
            float picbl = 1.003f;//别问我这个数值怎么来的
            float picmm = 2.842f;//别问我这个数值怎么来的

            PdfGState state = new PdfGState();
            state.FillOpacity = 0.6f;//印章图片整体透明度

            //throw new NotImplementedException();
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);//选择需要印章的pdf
                if (comboQmtype.SelectedIndex != 0)
                {
                    //最后的true表示保留原签名
                    pdfStamper = PdfStamper.CreateSignature(pdfReader, new FileStream(outputfilepath, FileMode.Create), '\0', null, true);//加完印章后的pdf
                }
                else
                {
                    pdfStamper = new PdfStamper(pdfReader,new FileStream(outputfilepath,FileMode.Create));
                }
                
                int numberOfPages = pdfReader.NumberOfPages;//pdf页数
                
                PdfContentByte waterMarkContent;

                if(zqfz == 0&& numberOfPages > 1)
                {
                    int max = 20;//骑缝章最大分割数
                    int ss = numberOfPages / max + 1;
                    int sy = numberOfPages - ss * max / 2;
                    int sys = sy / ss;
                    int syy = sy % ss;
                    int pp = max / 2 + sys;
                    Bitmap[] nImage;
                    int startpage = 0;
                    for (int i = 0; i < ss; i++)
                    {
                        int tmp = pp;
                        if (i < syy)
                        {
                            tmp++;
                        }
                        nImage = subImages(ModelPicName, tmp);
                        for (int x =  1; x <= tmp; x++)
                        {
                            int page = startpage + x;
                            waterMarkContent = pdfStamper.GetOverContent(page);//获取当前页内容
                            int rotation = pdfReader.GetPageRotation(page);//获取当前页的旋转度
                            iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(page);//获取当前页尺寸
                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(nImage[x - 1], System.Drawing.Imaging.ImageFormat.Bmp);//获取骑缝章对应页的部分
                            image.Transparency = new int[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//这里透明背景的图片会变黑色,所以设置黑色为透明
                            waterMarkContent.SaveState();//通过PdfGState调整图片整体的透明度
                            waterMarkContent.SetGState(state);
                            //image.GrayFill = 20;//透明度，灰色填充
                            //image.Rotation//旋转
                            //image.ScaleToFit(140F, 320F);//设置图片的指定大小
                            //image.RotationDegrees//旋转角度
                            float sfbl, imageW, imageH;
                            if (sftype == 0)
                            {
                                sfbl = sfsize * picbl;//别问我为什么要乘这个
                                imageW = image.Width * sfbl / 100f;
                                imageH = image.Height * sfbl / 100f;
                                //image.ScalePercent(sfbl);//设置图片比例
                                image.ScaleToFit(imageW, imageH);//设置图片的指定大小
                            }
                            else
                            {
                                sfbl = sfsize * picmm;//别问我为什么要乘这个
                                imageW = sfbl / tmp;
                                imageH = sfbl;
                                image.ScaleToFit(imageW, imageH);//设置图片的指定大小
                            }

                            //水印的位置
                            float xPos, yPos;
                            if (rotation == 90 || rotation == 270)
                            {
                                xPos = psize.Height - imageW;
                                yPos = (psize.Width - imageH) * (100 - qfzwzbl) / 100;
                            }
                            else
                            {
                                xPos = psize.Width - imageW;
                                yPos = (psize.Height - imageH) * (100 - qfzwzbl) / 100;
                            }
                            image.SetAbsolutePosition(xPos,yPos);
                            waterMarkContent.AddImage(image);
                            waterMarkContent.RestoreState();
                        }
                        startpage += tmp;
                    }
                }

                if (zyz!=0|| comboQmtype.SelectedIndex != 0)
                {
                    iTextSharp.text.Image img = null;
                    float sfbl, imgW=0, imgH=0;
                    float xPos=0, yPos=0;
                    bool all = false;
                    int signpage = 0;

                    if (zyz != 0)
                    {
                        img = iTextSharp.text.Image.GetInstance(ModelPicName);//创建一个图片对象
                        img.Transparency = new int[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//这里透明背景的图片会变黑色,所以设置黑色为透明

                        if (sftype == 0)
                        {
                            sfbl = sfsize * picbl;//别问我为什么要乘这个
                            imgW = img.Width * sfbl / 100f;
                            imgH = img.Height * sfbl / 100f;
                            //img.ScalePercent(sfbl);//设置图片比例
                            img.ScaleToFit(imgW, imgH);//设置图片的指定大小
                        }
                        else
                        {
                            sfbl = sfsize * picmm;//别问我为什么要乘这个
                            imgW = sfbl;
                            imgH = sfbl* img.Height/ img.Width;
                            img.ScaleToFit(imgW, imgH);//设置图片的指定大小,实际只能指定宽度,高度还是按照图片比例计算的
                        }
                    }

                    if (zyz == 1)
                    {
                        signpage = numberOfPages;
                    }
                    else if (zyz == 2)
                    {
                        signpage = 1;
                    }
                    else if (zyz == 3)
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

                            waterMarkContent.SaveState();//通过PdfGState调整图片整体的透明度
                            waterMarkContent.SetGState(state);

                            float wbl = 0;
                            float hbl = 1;
                            DataRow[] arrRow = dtPos.Select("Path = '" + inputfilepath + "' and Page = " + i);
                            if (arrRow == null || arrRow.Length == 0)
                            {
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
                            waterMarkContent.RestoreState();

                            //普通印章跟数字印章已经完美重叠,所以就不需要通过以下方法特殊区分了
                            //同时启用印章和数字签名的话用最后一个印章用数字签名代替
                            //if (all)
                            //{
                            //    if (comboQmtype.SelectedIndex == 0)
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
                            //else if (comboQmtype.SelectedIndex == 0)
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
                    if (comboQmtype.SelectedIndex != 0)
                    {

                        Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
                        Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] {cp.ReadCertificate(cert.RawData)};

                        Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair pk = Org.BouncyCastle.Security.DotNetUtilities.GetKeyPair(cert.PrivateKey);
                        IExternalSignature externalSignature = new PrivateKeySignature(pk.Private, DigestAlgorithms.SHA256);

                        PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
                        signatureAppearance.SignDate = DateTime.Now;
                        //signatureAppearance.SignatureCreator = "";
                        //signatureAppearance.Reason = "验证身份";
                        //signatureAppearance.Location = "深圳";
                        if (zyz == 0)
                        {
                            signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(0, 0, 0, 0), numberOfPages, null);
                        }
                        else
                        {
                            signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;//仅体现图片
                            signatureAppearance.SignatureGraphic = img;//iTextSharp.text.Image.GetInstance(ModelPicName);
                            //signatureAppearance.Acro6Layers = true;

                            //StringBuilder buf = new StringBuilder();
                            //buf.Append("Digitally Signed by ");
                            //String name = cert.SubjectName.Name;
                            //buf.Append(name).Append('\n');
                            //buf.Append("Date: ").Append(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss zzz"));
                            //string text = buf.ToString();
                            //signatureAppearance.Layer2Text = text;

                            float bk = 2;//数字签名的图片要加上边框才能跟普通印章的位置完全一致
                            signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(xPos- bk, yPos- bk, xPos + imgW + bk, yPos + imgH + bk), signpage, "Signature");
                        }

                        MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);
                    }

                }
                return true;
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
                    var fileInfos = dir.GetFiles();
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
                        textBCpath.Text = Path.GetDirectoryName(file.FileName);
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
            if (file.ShowDialog() == DialogResult.OK)
            {
                textGZpath.Text = file.FileName;
            }
        }
        //重置按钮
        private void clear_Click(object sender, EventArgs e)
        {
            pathText.Text = "";
            textBCpath.Text = "";
            textGZpath.Text = "";
            imgPageCount = 0;
            imgStartPage = 0;
            dt.Rows.Clear();
            dtPos.Rows.Clear();
            pdfInputPath = null;
            log.Text = "提示:印章图片默认是72dpi(那40mm印章对应的像素就是113),打印效果会很模糊,建议使用300dpi以上的印章图片然后调整印章比例,如300dpi(40mm印章对应像素472)对应的比例是72/300=24%,所以比例直接填写24即可";

        }
        //程序加载
        private void Form1_Load(object sender, EventArgs e)
        {
            comboType.SelectedIndex = 1;
            comboQfz.SelectedIndex = 0;
            comboYz.SelectedIndex = 0;
            comboBoxBL.SelectedIndex = 1;
            comboQmtype.SelectedIndex = 0;
            fw = this.Width;
            fh = this.Height;
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            comboPDFlist.DisplayMember = "Name";
            comboPDFlist.ValueMember = "Value";
            comboPDFlist.DataSource = dt;
            dtPos.Columns.Add("Path", typeof(string));
            dtPos.Columns.Add("Page", typeof(int));
            dtPos.Columns.Add("X", typeof(float));
            dtPos.Columns.Add("Y", typeof(float));
        }
        //预览图定位
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Point pt = pictureBox1.PointToClient(Control.MousePosition);
            float picw = pictureBox1.Width;
            float pich = pictureBox1.Height;
            float px = pt.X / picw;
            float py = pt.Y / pich;
            textPx.Text = px.ToString("#0.0000");
            textPy.Text = py.ToString("#0.0000");
            DataRow[] arrRow = dtPos.Select("Path = '"+ pdfInputPath + "' and Page = "+ imgStartPage);
            if (arrRow == null || arrRow.Length == 0)
            {
                dtPos.Rows.Add(new object[] { pdfInputPath, imgStartPage, px, py });
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

            pictureBox2.Location = new Point(pt1.X+pt.X-10, pt1.Y + pt.Y - 10);

        }
        //印章比例类型
        private void comboBoxBL_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBoxBL.SelectedIndex != 0)
            {
                label7.Text = "mm";
                textBili.Text = "40";
            }
            else
            {
                label7.Text = "%";
                textBili.Text = "100";
            }
        }
        //设置预览图按钮
        private void buttonYLT_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "图片文件|*.jpg;*.png";
            if (file.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(file.FileName);
                float imgW = img.Width;
                float imgH = img.Height;
                float imgBl = imgW / imgH;
                if (imgBl < 1)
                {
                    pictureBox1.Width = Convert.ToInt32(250 * imgBl);
                    pictureBox1.Height = 250;
                    pictureBox1.Location = new Point(450+(250- pictureBox1.Width)/2, 43);
                }
                else
                {
                    pictureBox1.Width = 250;
                    pictureBox1.Height = Convert.ToInt32(250 / imgBl);
                    pictureBox1.Location = new Point(450, 43 + (250 - pictureBox1.Height) / 2);
                }
                pictureBox1.Image = img;
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
            }
            else
            {
                label1.Text = "请选择需要盖章的PDF文件(支持多选)";
                label2.Text = "请选择PDF盖章后所保存的目录";
            }
            
        }
        //上一页
        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (pdfInputPath != null&&imgStartPage > 1)
            {
                imgStartPage--;
                PDFFile pdfFile = PDFFile.Open(pdfInputPath);
                Bitmap pageImage = pdfFile.GetPageImage(imgStartPage-1, 56 * 1);
                pdfFile.Dispose();
                pictureBox1.Image = pageImage;
                labelPage.Text = imgStartPage + "/" + imgPageCount;

                float px, py;
                DataRow[] arrRow = dtPos.Select("Path = '" + pdfInputPath + "' and Page = " + imgStartPage);
                if (arrRow == null || arrRow.Length == 0)
                {
                    px = Convert.ToSingle(textPx.Text);//这里根据比例来定位
                    py = Convert.ToSingle(textPy.Text);//这里根据比例来定位
                }
                else
                {
                    DataRow dr = arrRow[0];
                    px = Convert.ToSingle(dr["X"].ToString());
                    py = Convert.ToSingle(dr["Y"].ToString());
                }
                int X = Convert.ToInt32(pictureBox1.Width*px);
                int Y = Convert.ToInt32(pictureBox1.Height*py);
                Point pt1 = pictureBox1.Location;

                pictureBox2.Location = new Point(pt1.X + X - 10, pt1.Y + Y - 10);

            }
        }
        //下一页
        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (pdfInputPath != null&&imgStartPage < imgPageCount)
            {
                imgStartPage++;
                PDFFile pdfFile = PDFFile.Open(pdfInputPath);
                Bitmap pageImage = pdfFile.GetPageImage(imgStartPage-1, 56 * 1);
                pdfFile.Dispose();
                pictureBox1.Image = pageImage;
                labelPage.Text = imgStartPage + "/" + imgPageCount;

                float px,py;
                DataRow[] arrRow = dtPos.Select("Path = '" + pdfInputPath + "' and Page = " + imgStartPage);
                if (arrRow == null || arrRow.Length == 0)
                {
                    px = Convert.ToSingle(textPx.Text);//这里根据比例来定位
                    py = Convert.ToSingle(textPy.Text);//这里根据比例来定位
                }
                else
                {
                    DataRow dr = arrRow[0];
                    px = Convert.ToSingle(dr["X"].ToString());
                    py = Convert.ToSingle(dr["Y"].ToString());
                }
                int X = Convert.ToInt32(pictureBox1.Width * px);
                int Y = Convert.ToInt32(pictureBox1.Height * py);
                Point pt1 = pictureBox1.Location;

                pictureBox2.Location = new Point(pt1.X + X - 10, pt1.Y + Y - 10);

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
            pdfInputPath = comboPDFlist.SelectedValue.ToString();
            PDFFile pdfFile = PDFFile.Open(pdfInputPath);
            Bitmap pageImage = pdfFile.GetPageImage(0, 56 * 1);
            pictureBox1.Image = pageImage;
            imgStartPage = 1;
            imgPageCount = pdfFile.PageCount;
            pdfFile.Dispose();
            labelPage.Text = imgStartPage + "/" + imgPageCount;

            float px, py;
            DataRow[] arrRow = dtPos.Select("Path = '" + pdfInputPath + "' and Page = " + imgStartPage);
            if (arrRow == null || arrRow.Length == 0)
            {
                px = Convert.ToSingle(textPx.Text);//这里根据比例来定位
                py = Convert.ToSingle(textPy.Text);//这里根据比例来定位
            }
            else
            {
                DataRow dr = arrRow[0];
                px = Convert.ToSingle(dr["X"].ToString());
                py = Convert.ToSingle(dr["Y"].ToString());
            }
            int X = Convert.ToInt32(pictureBox1.Width * px);
            int Y = Convert.ToInt32(pictureBox1.Height * py);
            Point pt1 = pictureBox1.Location;

            pictureBox2.Location = new Point(pt1.X + X - 10, pt1.Y + Y - 10);
        }
        //根据印章类型切换窗口大小
        private void comboYz_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboYz.SelectedIndex != 0)
            {
                this.Size = new Size(fw + 267, fh);
            }
            else
            {
                this.Size = new Size(fw, fh);
            }
        }
    }
}
