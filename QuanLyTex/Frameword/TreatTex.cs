using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTex.Frameword
{
	class TreatTex
	{
		public int treatNextNgoacKep(string tex,int start)
		{
			int i = start; int j = start;
			while(j>=i&&j>=0&&i>=0)
			{
				i = tex.IndexOf('{',i+1);
				j= tex.IndexOf('}',j+1);
			}
			return j;
		}
		public int treatPreviousNgoacKep(string tex, int end)
		{
			int i = end; int j = end;
			string subtex;
			while (j <= i&&j>=0&&i>=0)
			{
				subtex = tex.Substring(0, i-1);
				i = subtex.LastIndexOf('}');
				j = subtex.LastIndexOf('{');
			}
			return j;
		}
	}
}
