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
            if(textGZpath.Text!=""&& textBCpath.Text!=""&& pathText.Text!="")
            {
                pdfGz();
            }
            else
            {
                MessageBox.Show("文件路径不能为空，请先选择路径。");
            }
        }

        private void pdfGz()
        {
            DirectoryInfo dir = new DirectoryInfo(pathText.Text);
            var fileInfos = dir.GetFiles();
            string sourcepath = pathText.Text;//需盖章目录
            string outputpath = textBCpath.Text;//保存目录
            string watermark = textGZpath.Text;  // 水印图片

            if (!Directory.Exists(outputpath))//输出目录不存在则新建
            {
                Directory.CreateDirectory(outputpath);
            }

            log.Text = "";//清空日志

            try
            {
                foreach (var fileInfo in fileInfos)
                {
                    string source = sourcepath + "\\" + fileInfo.Name.ToString();
                    string output = outputpath + "\\" + fileInfo.Name.ToString();
                    bool isSurrcess = PDFWatermark(source, output, watermark);
                    if (isSurrcess)
                    {
                        log.Text = log.Text + "成功！“" + fileInfo.Name.ToString() + "”盖章完成！\r\n";
                    }
                    else
                    {
                        log.Text = log.Text + "失败！“" + fileInfo.Name.ToString() + "”盖章失败！\r\n";
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

        private bool PDFWatermark(string inputfilepath, string outputfilepath, string ModelPicName)

        {
            //throw new NotImplementedException();
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);//选择需要印章的pdf

                int numberOfPages = pdfReader.NumberOfPages;//pdf页数

                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);

                float width = psize.Width;

                float height = psize.Height;

                
                Bitmap[] nImage = subImages(ModelPicName, numberOfPages);

                pdfStamper = new PdfStamper(pdfReader, new FileStream(outputfilepath, FileMode.Create));//加完印章后的pdf

                PdfContentByte waterMarkContent;

                int zyz = comboBox1.SelectedIndex;//如何加印章
                bool all = false;
                int index = 0;
                if (zyz == 3)
                {
                    all = true;
                }else if(zyz == 2)
                {
                    index = 1;
                }else if(zyz == 1)
                {
                    index = numberOfPages;
                }

                //每一页加水印,也可以设置某一页加水印
                for (int i = 0; i < numberOfPages;)
                {
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(nImage[i], ImageFormat.Bmp);

                    //image.GrayFill = 20;//透明度，灰色填充
                    //image.Rotation//旋转
                    //image.ScaleToFit(140F, 320F);//设置图片的指定大小
                    //image.RotationDegrees//旋转角度
                    int bili;
                    if(!int.TryParse(textBili.Text, out bili))
                    {
                        bili = 24;
                    }
                    float result = bili / 100f;//印章图片由72dpi转300dpi
                    image.ScalePercent(bili);//设置图片比例
                    image.Transparency = new int[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//这里透明背景的图片会变黑色,所以设置黑色为透明

                    i++;//pdf页面是从1开始的,所以这里要+1
                    int rotation = pdfReader.GetPageRotation(i);//获取指定页面的旋转度

                    //水印的位置
                    if (rotation == 90 || rotation == 270)
                    {
                        image.SetAbsolutePosition(height - image.Width * result, (width - image.Height * result) / 2);
                    }
                    else
                    {
                        image.SetAbsolutePosition(width - image.Width * result, (height - image.Height * result) / 2);
                    }
                    //switch (rotation)
                    //{
                    //    case 90:
                    //        image.SetAbsolutePosition(height - image.Width * bili, (width - image.Height * bili) / 2);
                    //        break;
                    //    case 180:
                    //        image.SetAbsolutePosition(width - image.Width * bili, (height - image.Height * bili) / 2);
                    //        break;
                    //    case 270:
                    //        image.SetAbsolutePosition(height - image.Width * bili, (width - image.Height * bili) / 2);
                    //        break;
                    //    default:
                    //        image.SetAbsolutePosition(width - image.Width * bili, (height - image.Height * bili) / 2);
                    //        break;
                    //}
                    
                    waterMarkContent = pdfStamper.GetOverContent(i);

                    waterMarkContent.AddImage(image);

                    if (all || i == index)//如果是最后一页加入指定的图片
                    {
                        //创建一个图片对象                    
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ModelPicName);

                        //按比例缩放
                        img.ScalePercent(bili);

                        //把图片增加到内容页的指定位子  b width c height  e bottom f left
                        //waterMarkContent.AddImage(img,0,100f,100f,0,10f,20f);

                        float wbl = Convert.ToSingle(textPx.Text);//这里根据比例来定位
                        float hbl = 1-Convert.ToSingle(textPy.Text);//这里根据比例来定位
                        if (rotation == 90 || rotation == 270)
                        {
                            img.SetAbsolutePosition((height - img.Width * result) * wbl, (width - img.Height * result) * hbl);
                        }
                        else
                        {
                            img.SetAbsolutePosition((width - img.Width * result) * wbl, (height - img.Height * result) * hbl);
                        }
                        waterMarkContent.AddImage(img);

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
                //strMsg = "success";

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
            log.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
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

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
            {
                this.Size = new Size(fw+200, fh);
            }
            else
            {
                this.Size = new Size(fw, fh);
            }
        }
    }
}
