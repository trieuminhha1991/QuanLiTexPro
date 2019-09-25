using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTex
{
    class User1Form
    {
        public Dictionary<string,string>  splitForm(string path)
        {
            path = path;
            string str3 = File.ReadAllText(path);
            string contents = str3.Substring(0, str3.IndexOf("%header", 0));
            int index = str3.IndexOf("%footer", 0);
            int num2 = str3.Length - 1;
            string str5 = str3.Substring(index, num2 - index);
            string pathHeader = Directory.GetParent(path) + @"\Header.txt";
            string pathFooter = Directory.GetParent(path) + @"\Footer.txt";
            if (File.Exists(pathHeader))
            {
                File.Delete(pathHeader);
            }
            if (File.Exists(pathFooter))
            {
                File.Delete(pathFooter);
            }
            File.WriteAllText(pathHeader, contents);
            File.WriteAllText(pathFooter, str5);
            var Dictionary = new Dictionary<string,string>();
            Dictionary.Add("path", path);
            Dictionary.Add("pathHeader", pathHeader);
            Dictionary.Add("pathFooter", pathFooter);
            return Dictionary;
        }

    }
}
