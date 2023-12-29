using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace PDFQFZ.Library
{
    internal class IniFileHelper
    {
        string strIniFilePath;  // ini配置文件路径

        // 返回0表示失败，非0为成功
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 返回取得字符串缓冲区的长度
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern long GetPrivateProfileString(string section, string key, string strDefault, StringBuilder retVal, int size, string filePath);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileInt(string section, string key, int nDefault, string filePath);

        /// <summary>
        /// 无参构造函数
        /// </summary>
        /// <returns></returns>
        //public IniFileHelper()
        //{
        //    this.strIniFilePath = System.Windows.Forms.Application.StartupPath + "\\config.ini";
        //}


        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="strIniFilePath">ini配置文件路径</param>
        /// <returns></returns>
        public IniFileHelper(string strIniFilePath)
        {
            if (strIniFilePath != null)
            {
                this.strIniFilePath = strIniFilePath;
            }
        }


        /// <summary>
        /// 获取ini配置文件中的字符串
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">键名</param>
        /// <param name="strDefault">默认值</param>
        /// <param name="retVal">结果缓冲区</param>
        /// <param name="size">结果缓冲区大小</param>
        /// <returns>成功true,失败false</returns>
        public bool GetIniString(string section, string key, string strDefault, StringBuilder retVal, int size)
        {
            long liRet = GetPrivateProfileString(section, key, strDefault, retVal, size, strIniFilePath);
            return (liRet >= 1);
        }


        /// <summary>
        /// 获取ini配置文件中的整型值
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">键名</param>
        /// <param name="nDefault">默认值</param>
        /// <returns></returns>
        public int GetIniInt(string section, string key, int nDefault)
        {
            return GetPrivateProfileInt(section, key, nDefault, strIniFilePath);
        }


        /// <summary>
        /// 往ini配置文件写入字符串
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">键名</param>
        /// <param name="val">要写入的字符串</param>
        /// <returns>成功true,失败false</returns>
        public bool WriteIniString(string section, string key, string val)
        {
            long liRet = WritePrivateProfileString(section, key, val, strIniFilePath);
            return (liRet != 0);
        }


        /// <summary>
        /// 往ini配置文件写入整型数据
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">键名</param>
        /// <param name="val">要写入的数据</param>
        /// <returns>成功true,失败false</returns>
        public bool WriteIniInt(string section, string key, int val)
        {
            return WriteIniString(section, key, val.ToString());
        }

        /// <summary>
        /// 自定义读取INI文件中的内容方法
        /// </summary>
        /// <param name="Section">键</param>
        /// <param name="key">值</param>
        /// <returns></returns>
        public string ContentValue(string Section, string key)
        {

            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strIniFilePath);
            return temp.ToString();
        }
    }
}
