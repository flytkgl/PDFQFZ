using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

        /// <summary>
        /// 读取指定 Section 中的全部键值对。
        /// </summary>
        /// <param name="section">INI 文件中的节名</param>
        /// <returns>
        ///   Dictionary<key, value>，若该节不存在则返回空字典。
        /// </returns>
        public Dictionary<string, string> GetAllSectionPairs(string section)
        {
            // 1. 先获取该节中所有键的列表（key 为 null 时返回所有键名，以 '\0' 分隔）
            const int bufferSize = 65535;               // 足够大的缓冲区
            var keyBuffer = new StringBuilder(bufferSize);
            long keyCount = GetPrivateProfileString(section, null, "", keyBuffer, bufferSize, strIniFilePath);

            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (keyCount <= 0) return result;           // 没有键，直接返回空集合

            // 2. 键名之间使用 '\0' 分隔，最后还有一个额外的 '\0'，需要拆分
            string[] keys = keyBuffer.ToString()
                                     .Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);

            // 3. 逐个键读取对应的值并填充到字典
            foreach (var key in keys)
            {
                var valueBuilder = new StringBuilder(bufferSize);
                GetPrivateProfileString(section, key, "", valueBuilder, bufferSize, strIniFilePath);
                result[key] = valueBuilder.ToString();
            }
            return result;
        }
        //usage
        //var helper = new IniFileHelper(@"C:\Config\app.ini");
        //var allValues = helper.GetAllSectionValues("Database");
        //foreach (var kv in allValues)
        //{
        //      Console.WriteLine($"{kv.Key} = {kv.Value}");
        //}

        /// <summary>
        /// 读取指定节（section）中所有键名。
        /// </summary>
        /// <param name="section">INI 文件中的节名。</param>
        /// <returns>
        /// 包含该节所有键名的字符串数组。若节不存在或没有键，则返回空数组。
        /// </returns>
        private string[] GetSectionKeys(string section)
        {
            const int bufferSize = 65535;               // 足够大的缓冲区
            var keyBuffer = new StringBuilder(bufferSize);
            long keyCount = GetPrivateProfileString(section, null, "", keyBuffer,
                                                    bufferSize, strIniFilePath);
            if (keyCount <= 0) return Array.Empty<string>();

            return keyBuffer.ToString()
                            .Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 获取指定节下所有键对应的值（不返回键名）。
        /// </summary>
        /// <param name="section">INI 文件中的节名。</param>
        /// <returns> 
        /// 包含该节所有键值的字符串数组。若节不存在或没有键，则返回空数组。
        /// </returns>
        public string[] GetAllSectionValues2(string section)
        {
            var keys = GetSectionKeys(section);
            if (keys.Length == 0) return Array.Empty<string>();

            const int bufferSize = 65535;
            var values = new string[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                var valBuilder = new StringBuilder(bufferSize);
                GetPrivateProfileString(section, keys[i], "", valBuilder,
                                        bufferSize, strIniFilePath);
                values[i] = valBuilder.ToString();
            }
            return values;
        }

        /// <summary>
        /// 获取指定节下所有键对应的值（不返回键名）。
        /// </summary>
        /// <param name="section">INI 文件中的节名。</param>
        /// <returns>
        /// <see cref="List{T}"/>，每个元素为该节中某键的字符串值。  
        /// 若节不存在或没有键，则返回空列表。
        /// </returns>
        public List<string> GetAllSectionValues(string section)
        {
            var values = new List<string>();
            var keys = GetSectionKeys(section);
            if (keys.Length == 0) return values;

            const int bufferSize = 65535;
            foreach (var key in keys)
            {
                var valBuilder = new StringBuilder(bufferSize);
                GetPrivateProfileString(section, key, "", valBuilder,
                                        bufferSize, strIniFilePath);
                values.Add(valBuilder.ToString());
            }
            return values;
        }
        //usage
        //var values = ini.GetAllSectionValues("Server");
        //foreach (var v in values)
            //Console.WriteLine(v);

        /// <summary>
        /// 获取指定节中所有键‑值对。
        /// </summary>
        /// <param name="section">INI 文件中的节名。</param>
        /// <returns>
        /// <see cref="Dictionary{TKey,TValue}"/>，键为键名，值为对应的字符串值。  
        /// 若节不存在或没有键，则返回空字典。
        /// </returns>
        public Dictionary<string, string> GetAllSectionKeyPairs(string section)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var keys = GetSectionKeys(section);
            if (keys.Length == 0) return result;

            const int bufferSize = 65535;
            foreach (var key in keys)
            {
                var valBuilder = new StringBuilder(bufferSize);
                GetPrivateProfileString(section, key, "", valBuilder,
                                        bufferSize, strIniFilePath);
                result[key] = valBuilder.ToString();
            }
            return result;
        }

        /// <summary>
        /// 检查指定的节（section）是否存在于 INI 文件中。
        /// </summary>
        /// <param name="sectionName">要检查的节名。</param>
        /// <returns>
        /// true – 节存在且至少有一个键；  
        /// false – 节不存在或该节为空。
        /// </returns>
        public bool SectionExists(string sectionName)
        {
            // 通过 WinAPI 获取该节的键列表长度；长度>0 表示节存在
            const int bufferSize = 2;               // 只需要最小缓冲区
            var sb = new StringBuilder(bufferSize);
            long count = GetPrivateProfileString(sectionName, null, "", sb, bufferSize, strIniFilePath);
            return count > 0;
        }

    }
}
