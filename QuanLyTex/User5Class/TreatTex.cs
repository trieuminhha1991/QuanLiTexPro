using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuanLyTex.User5Class
{
	class TreatTex
	{
		public string CapText(Match m)
		{
			string x = m.ToString();
			x = x.Replace(" ", "");
			x = x.Insert(x.Length - 1, "}").Insert(1, "{");
			return x;
		}
		public void treatTex(string tex,string pathFile)
		{
		}

	}
}
