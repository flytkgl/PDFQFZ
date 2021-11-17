# PDFQFZ
PDF加盖骑缝章的小工具

选择需要盖章的PDF文件所在文件夹。
选择保存文件夹。
选择完整的印章图片（注意要调整到合适的大小，工具不会自动调整图片大小）,工具会根据PDF页数做等份切割。
点盖章，骚等后在保存文件夹中即可看到盖好骑缝章PDF文件。
![img](./pdfqfz.jpg)



数字签名相关

创建证书
1.如果你安装了VS或者安装了Windows Software Development Kit (SDK),可以使用makecert.exe命令生成
Powershell相关命令示例:
makecert -r -n "CN=PDFQFZ" -b 01/01/2020 -e 01/01/2100 -sv pdfqfz.pvk pdfqfz.cer
cert2spc pdfqfz.cer pdfqfz.spc
pvk2pfx -pvk pdfqfz.pvk -spc pdfqfz.spc -pfx pdfqfz.pfx -pi PDFQFZ -f //PDFQFZ是第一个命令时你输入的密码

2.另一个需要.NET 4.7.2环境的C#创建代码
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public class CertificateUtil
{
    static void MakeCert()
    {
        var ecdsa = ECDsa.Create(); // generate asymmetric key pair
        var req = new CertificateRequest("cn=foobar", ecdsa, HashAlgorithmName.SHA256);
        var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

        // Create PFX (PKCS #12) with private key
        File.WriteAllBytes("c:\\temp\\mycert.pfx", cert.Export(X509ContentType.Pfx, "P@55w0rd"));

        // Create Base 64 encoded CER (public key only)
        File.WriteAllText("c:\\temp\\mycert.cer",
            "-----BEGIN CERTIFICATE-----\r\n"
            + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
            + "\r\n-----END CERTIFICATE-----");
    }
}

3.通过IE浏览器导出

IE浏览器-设置-Internet选项-内容-证书-个人证书-导出-下一步-是-导出私钥-下一步-个人信息交换-下一步-设置密码-下一步-输入文件名-下一步-完成