using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTex
{
    class User1MapFile
    {
        public Dictionary<string, object> mapNewFile(string str,string path,string path2)
        {
            User1Filter filter = new User1Filter();
            Regex rx = new Regex(str);
            List<string> list = new List<string>();
            if (path != null)
            {
                IEnumerable<string> enumerable = Directory.EnumerateFiles(path, "*.tex");
                foreach (string str4 in enumerable)
                {
                    List<string> collection = filter.FilterId(str4, Type, rx);
                    list.AddRange(collection);
                }
            }
            isChecked = this.FileSelect2.IsChecked;
            flag8 = true;
            if (path2!= null)
            {
                str3 = "";
                char[] separator = new char[] { ';' };
                string[] strArray = this.FileSelect.Text.Split(separator);
                foreach (string str5 in strArray)
                {
                    object[] objArray1 = new object[] { str3, Directory.GetParent(str5), @"\", str2, ".tex" };
                    str3 = string.Concat(objArray1);
                    List<string> collection = new Class1().FilterId(str5, Type, rx);
                    list.AddRange(collection);
                }
            }
            dictionary.Add("list", list);
            dictionary.Add("Path", str3);
            return dictionary;
        }
    }
}
