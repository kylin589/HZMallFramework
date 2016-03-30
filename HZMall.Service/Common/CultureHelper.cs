using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using BrCms.Framework.Data;
using BrCms.Framework.Infrastructure;

namespace HZMall.Service.Common
{
    public static class CultureHelper
    {
        private readonly static IDatabaseHelper _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
        /// <summary>
        /// 所有语言区域
        /// </summary>
        private static readonly List<string> _validCultures = new List<string> { "af", "af-ZA", "sq", "sq-AL", "gsw-FR", "am-ET", "ar", "ar-DZ", "ar-BH", "ar-EG", "ar-IQ", "ar-JO", "ar-KW", "ar-LB", "ar-LY", "ar-MA", "ar-OM", "ar-QA", "ar-SA", "ar-SY", "ar-TN", "ar-AE", "ar-YE", "hy", "hy-AM", "as-IN", "az", "az-Cyrl-AZ", "az-Latn-AZ", "ba-RU", "eu", "eu-ES", "be", "be-BY", "bn-BD", "bn-IN", "bs-Cyrl-BA", "bs-Latn-BA", "br-FR", "bg", "bg-BG", "ca", "ca-ES", "zh-HK", "zh-MO", "zh-CN", "zh-Hans", "zh-SG", "zh-TW", "zh-Hant", "co-FR", "hr", "hr-HR", "hr-BA", "cs", "cs-CZ", "da", "da-DK", "prs-AF", "div", "div-MV", "nl", "nl-BE", "nl-NL", "en", "en-AU", "en-BZ", "en-CA", "en-029", "en-IN", "en-IE", "en-JM", "en-MY", "en-NZ", "en-PH", "en-SG", "en-ZA", "en-TT", "en-GB", "en-US", "en-ZW", "et", "et-EE", "fo", "fo-FO", "fil-PH", "fi", "fi-FI", "fr", "fr-BE", "fr-CA", "fr-FR", "fr-LU", "fr-MC", "fr-CH", "fy-NL", "gl", "gl-ES", "ka", "ka-GE", "de", "de-AT", "de-DE", "de-LI", "de-LU", "de-CH", "el", "el-GR", "kl-GL", "gu", "gu-IN", "ha-Latn-NG", "he", "he-IL", "hi", "hi-IN", "hu", "hu-HU", "is", "is-IS", "ig-NG", "id", "id-ID", "iu-Latn-CA", "iu-Cans-CA", "ga-IE", "xh-ZA", "zu-ZA", "it", "it-IT", "it-CH", "ja", "ja-JP", "kn", "kn-IN", "kk", "kk-KZ", "km-KH", "qut-GT", "rw-RW", "sw", "sw-KE", "kok", "kok-IN", "ko", "ko-KR", "ky", "ky-KG", "lo-LA", "lv", "lv-LV", "lt", "lt-LT", "wee-DE", "lb-LU", "mk", "mk-MK", "ms", "ms-BN", "ms-MY", "ml-IN", "mt-MT", "mi-NZ", "arn-CL", "mr", "mr-IN", "moh-CA", "mn", "mn-MN", "mn-Mong-CN", "ne-NP", "no", "nb-NO", "nn-NO", "oc-FR", "or-IN", "ps-AF", "fa", "fa-IR", "pl", "pl-PL", "pt", "pt-BR", "pt-PT", "pa", "pa-IN", "quz-BO", "quz-EC", "quz-PE", "ro", "ro-RO", "rm-CH", "ru", "ru-RU", "smn-FI", "smj-NO", "smj-SE", "se-FI", "se-NO", "se-SE", "sms-FI", "sma-NO", "sma-SE", "sa", "sa-IN", "sr", "sr-Cyrl-BA", "sr-Cyrl-SP", "sr-Latn-BA", "sr-Latn-SP", "nso-ZA", "tn-ZA", "si-LK", "sk", "sk-SK", "sl", "sl-SI", "es", "es-AR", "es-BO", "es-CL", "es-CO", "es-CR", "es-DO", "es-EC", "es-SV", "es-GT", "es-HN", "es-MX", "es-NI", "es-PA", "es-PY", "es-PE", "es-PR", "es-ES", "es-US", "es-UY", "es-VE", "sv", "sv-FI", "sv-SE", "syr", "syr-SY", "tg-Cyrl-TJ", "tzm-Latn-DZ", "ta", "ta-IN", "tt", "tt-RU", "te", "te-IN", "th", "th-TH", "bo-CN", "tr", "tr-TR", "tk-TM", "ug-CN", "uk", "uk-UA", "wen-DE", "ur", "ur-PK", "uz", "uz-Cyrl-UZ", "uz-Latn-UZ", "vi", "vi-VN", "cy-GB", "wo-SN", "sah-RU", "ii-CN", "yo-NG" };
        /// <summary>
        /// 目前实现的语言区域
        /// </summary>
        private static readonly List<string> _cultures = new List<string> {
            "th-TH" ,    //泰国
            "zh-CN",    // 默认区域
            "en-US",    //美国
            "zh-HK"    //香港,更改为默认区域
        };
        /// <summary>
        /// store cultures resources
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> dicResources = new Dictionary<string, Dictionary<string, string>>();
        /// <summary>
        /// 资源锁
        /// </summary>
        private static object lockObj = new object();
        /// <summary>
        ///  资源文件目录
        /// </summary>
        //private static readonly string ResourcePath = Path.Combine(HttpRuntime.AppDomainAppPath, "bin/Resources");
        //private static readonly string ResourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin/Resources");

        /// <summary>
        /// 初始化
        /// </summary>
        static CultureHelper()
        {
            //加载资源文件
            //DirectoryInfo dir = new DirectoryInfo(ResourcePath);

            //foreach (FileInfo file in dir.GetFiles())
            //{
            //    LoadResource(file.Name);
            //}
            ////添加监控
            //using (FileSystemWatcher wather = new FileSystemWatcher(ResourcePath))
            //{
            //    wather.Filter = "*.resx";
            //    wather.EnableRaisingEvents = true;
            //    wather.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
            //                           | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            //    wather.Changed += (s, e) =>
            //    {
            //        LoadResource(e.Name);
            //    };
            //}
        }

        /// <summary>
        /// 加载指定语言的资源文件
        /// </summary>
        /// <param name="fileName">资源文件名</param>
        private static void LoadResource(string fileName)
        {
            //lock (lockObj)
            //{
            //    using (ResXResourceReader reader = new ResXResourceReader(Path.Combine(ResourcePath, fileName)))
            //    {
            //        //目前暂定资源文件格式为 Resource.语言代码.resx ,例如 Resource.zh-CH.resx
            //        string[] arr = fileName.Split('.');
            //        if (arr.Length == 3)
            //        {
            //            string culture = arr[1];
            //            if (dicResources.Keys.Contains(culture))
            //            {
            //                dicResources.Remove(culture);
            //            }
            //            //添加资源文件到集合
            //            Dictionary<string, string> dic = new Dictionary<string, string>();
            //            foreach (DictionaryEntry entity in reader)
            //            {
            //                dic.Add(entity.Key.ToString(), entity.Value.ToString());
            //            }
            //            dicResources.Add(culture, dic);
            //        }
            //    }
            //}
        }
        /// <summary>
        /// 获取默认的语言区域,暂时默认返回中文（1中文,2英语,3泰语,4中文繁体）
        /// </summary>
        /// <returns></returns>
        public static int GetLanguageID()
        {
            int languageID = 4;//默认区域
            string cultures = GetCurrentCulture();
            switch (cultures)
            {

                case "zh-CN":
                    languageID = 1;
                    break;
                case "en-US":
                    languageID = 2;
                    break;
                case "th-TH":
                    languageID = 3;
                    break;
                case "zh-HK":
                    languageID = 4;
                    break;
                default:
                    languageID = 4;
                    break;
            }
            return languageID; // return Default culture
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageID"></param>
        /// <returns></returns>
        public static string GetLanguage(int languageID)
        {
            string cultures = string.Empty;
            switch (languageID)
            {

                case 1:
                    cultures = "zh-CN";
                    break;
                case 2:
                    cultures = "en-US";
                    break;
                case 3:
                    cultures = "th-TH";
                    break;
                case 4:
                    cultures = "zh-HK";
                    break;
                default:
                    cultures = "zh-HK";
                    break;
            }
            return cultures;
        }

        /// <summary>
        /// 是否从右到左排序
        /// </summary>
        public static bool IsRighToLeft()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;

        }
        /// <summary>
        /// 获取已经实现的语言区域
        /// </summary>
        /// <remarks>会根据当前参数,匹配已实现语言的列表,如果存在则返回该语言,如果不存在返回默认语言</remarks>
        /// <param name="name" />语言区域</param>
        public static string GetImplementedCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
                return GetDefaultCulture();

            //add by liujc.过滤泰文
            if (name.ToLower() == "th-TH")
            {
                return GetDefaultCulture();
            }

            if (_validCultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                return GetDefaultCulture();
            if (_cultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
                return name;
            // Find a close match. For example, if you have "en-US" defined and the user requests "en-GB", 
            // the function will return closes match that is "en-US" because at least the language is the same (ie English)  
            var n = GetNeutralCulture(name);
            foreach (var c in _cultures)
                if (c.StartsWith(n))
                    return c;
            // else 
            // It is not implemented
            return GetDefaultCulture(); // return Default culture as no match found
        }
        /// <summary>
        /// 获取默认的语言区域（默认为第一个）
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultCulture()
        {
            return _cultures[3]; // return Default culture,update by liujc
        }
        /// <summary>
        /// 获取当前语言区域
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentCulture()
        {
            string currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            if (string.IsNullOrEmpty(currentCulture))
            {
                currentCulture = GetDefaultCulture();
            }
            return currentCulture;
        }
        /// <summary>
        /// 获取当前统一标准的语言区域（例如zh_CN,zh_CHS,统一为zh）
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentNeutralCulture()
        {
            return GetNeutralCulture(Thread.CurrentThread.CurrentCulture.Name);
        }
        /// <summary>
        /// 获取指定言语区域的统一标准区域（例如zh_CN,zh_CHS,统一为zh）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetNeutralCulture(string name)
        {
            if (!name.Contains("-")) return name;

            return name.Split('-')[0]; // Read first part only. E.g. "en", "es"
        }
     

       
 

       

    }
}