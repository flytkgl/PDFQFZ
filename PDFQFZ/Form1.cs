using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Drawing.Imaging;

namespace PDFQFZ
{
    public partial class Form1 : Form
    {
        int fw, fh;
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
            if(comboQfz.SelectedIndex == 1&&comboYz.SelectedIndex == 0)
            {
                if (MessageBox.Show("你既不盖骑缝章又不盖印章是想让我帮你关机吗?","你想干嘛", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    MessageBox.Show("滚蛋吧,我才不帮你关呢,哼!");
                    System.Environment.Exit(0);
                }
            }
            else
            {
                if (textGZpath.Text != "" && textBCpath.Text != "" && pathText.Text != "")
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

            if (!Directory.Exists(outputpath))//输出目录不存在则新建
            {
                Directory.CreateDirectory(outputpath);
            }

            log.Text = "";//清空日志

            try
            {
                if(comboType.SelectedIndex == 0)
                {
                    DirectoryInfo dir = new DirectoryInfo(pathText.Text);
                    var fileInfos = dir.GetFiles();
                    foreach (var fileInfo in fileInfos)
                    {
                        if(fileInfo.Extension == "pdf")
                        {
                            string source = sourcepath + "\\" + fileInfo.Name;
                            string output = outputpath + "\\" + fileInfo.Name;
                            bool isSurrcess = PDFWatermark(source, output);
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
                        bool isSurrcess = PDFWatermark(source, output);
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

        private bool PDFWatermark(string inputfilepath, string outputfilepath)
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
            float tmd = 0.6f;//印章图片整体透明度

            //throw new NotImplementedException();
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);//选择需要印章的pdf
                pdfStamper = new PdfStamper(pdfReader, new FileStream(outputfilepath, FileMode.Create));//加完印章后的pdf
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
                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(nImage[x - 1], ImageFormat.Bmp);//获取骑缝章对应页的部分
                            image.Transparency = new int[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//这里透明背景的图片会变黑色,所以设置黑色为透明
                            waterMarkContent.SaveState();//通过PdfGState调整图片整体的透明度
                            PdfGState state = new PdfGState();
                            state.FillOpacity = tmd;
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
                            if (rotation == 90 || rotation == 270)
                            {
                                image.SetAbsolutePosition(psize.Height - imageW, (psize.Width - imageH) * (100 - qfzwzbl) / 100);
                            }
                            else
                            {
                                image.SetAbsolutePosition(psize.Width - imageW, (psize.Height - imageH) * (100 - qfzwzbl) / 100);
                            }
                            waterMarkContent.AddImage(image);
                            waterMarkContent.RestoreState();
                        }
                        startpage += tmp;
                    }
                }

                bool all = false;
                int index = 0;
                if (zyz == 1)
                {
                    index = numberOfPages;
                }
                else if (zyz == 2)
                {
                    index = 1;
                }
                else if (zyz == 3)
                {
                    all = true;
                }

                for (int i = 1; i <= numberOfPages; i++)
                {
                    if (all || i == index)
                    {
                        waterMarkContent = pdfStamper.GetOverContent(i);//获取当前页内容
                        int rotation = pdfReader.GetPageRotation(i);//获取指定页面的旋转度
                        iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(i);//获取当前页尺寸
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ModelPicName);//创建一个图片对象
                        img.Transparency = new int[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//这里透明背景的图片会变黑色,所以设置黑色为透明
                        waterMarkContent.SaveState();//通过PdfGState调整图片整体的透明度
                        PdfGState state = new PdfGState();
                        state.FillOpacity = tmd;
                        waterMarkContent.SetGState(state);

                        float sfbl, imgW, imgH;
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
                            imgH = sfbl;
                            img.ScaleToFit(imgW, imgH);//设置图片的指定大小
                        }

                        //把图片增加到内容页的指定位子  b width c height  e bottom f left
                        //waterMarkContent.AddImage(img,0,100f,100f,0,10f,20f);

                        float wbl = Convert.ToSingle(textPx.Text);//这里根据比例来定位
                        float hbl = 1 - Convert.ToSingle(textPy.Text);//这里根据比例来定位
                        if (rotation == 90 || rotation == 270)
                        {
                            img.SetAbsolutePosition((psize.Height - imgW) * wbl, (psize.Width - imgH) * hbl);
                        }
                        else
                        {
                            img.SetAbsolutePosition((psize.Width - imgW) * wbl, (psize.Height - imgH) * hbl);
                        }
                        waterMarkContent.AddImage(img);
                        waterMarkContent.RestoreState();

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

                return true;
            }
            catch (Exception ex)
            {
                ex.Message.Trim();
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
                }
            }
        }

        private void OutPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBCpath.Text = fbd.SelectedPath;
            }

        }

        private void GzPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "图片文件|*.jpg;*.png";
            if (file.ShowDialog() == DialogResult.OK)
            {
                textGZpath.Text = file.FileName;
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            pathText.Text = "";
            textBCpath.Text = "";
            textGZpath.Text = "";
            log.Text = "提示:印章图片默认是72dpi(那40mm印章对应的像素就是113),打印效果会很模糊,建议使用300dpi以上的印章图片然后调整印章比例,如300dpi(40mm印章对应像素472)对应的比例是72/300=24%,所以比例直接填写24即可,如果想直接设置图片尺寸也是可以的,但是要注意只支持正方形的图片";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboType.SelectedIndex = 1;
            comboQfz.SelectedIndex = 0;
            comboYz.SelectedIndex = 0;
            comboBoxBL.SelectedIndex = 1;
            fw = this.Width;
            fh = this.Height;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Point pt = pictureBox1.PointToClient(Control.MousePosition);
            float picw = pictureBox1.Width;
            float pich = pictureBox1.Height;
            float px = pt.X / picw;
            float py = pt.Y / pich;
            textPx.Text = px.ToString("#0.0000");
            textPy.Text = py.ToString("#0.0000");
        }

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

        private void buttonYLT_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "图片文件|*.jpg;*.png";
            if (file.ShowDialog() == DialogResult.OK)
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(file.FileName);
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

        private void comboType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            pathText.Text = "";
            textBCpath.Text = "";
            if(comboType.SelectedIndex == 0)
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
