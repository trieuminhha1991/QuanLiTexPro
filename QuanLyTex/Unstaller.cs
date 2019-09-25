using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTex
{
	class Unstaller
	{
		public void DeleteApp(string path)
		{
			try
			{
				System.Reflection.Assembly assem = System.Reflection.Assembly.LoadFrom(@"C:\Windows\System32");
				string produceCode = "{ " + assem.GetType().GUID.ToString() + "}";
				string args = @"/x " + produceCode + " /qr";
				System.Diagnostics.Process p = new System.Diagnostics.Process();
				p.StartInfo.FileName = @"C:\Windows\System32\msiexec.exe";
				p.StartInfo.Arguments = args;
				p.StartInfo.UseShellExecute = true;
				p.Start();
				p.WaitForExit();
			}
			catch { }
		}
	}
}
