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
        public Form1()
        {
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

                //每一页加水印,也可以设置某一页加水印
                for (int i = 0; i < numberOfPages; i++)
                {
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(nImage[i], ImageFormat.Bmp);

                    //image.GrayFill = 20;//透明度，灰色填充
                    //image.Rotation//旋转
                    //image.RotationDegrees//旋转角度
                    float bili = 0.24f;//印章图片由72dpi转300dpi
                    image.ScalePercent(bili * 100);//设置图片比例
                    image.Transparency = new int[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//这里透明背景的图片会变黑色,所以设置黑色为透明
                    //水印的位置
                    if (width < height) {
                        image.SetAbsolutePosition(width - image.Width * bili, (height - image.Height * bili) / 2);
                    }
                    else
                    {
                        image.SetAbsolutePosition(height - image.Width * bili, (width - image.Height * bili) / 2);
                    }
                    
                    waterMarkContent = pdfStamper.GetOverContent(i+1);

                    waterMarkContent.AddImage(image);
                    

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
    }
}
