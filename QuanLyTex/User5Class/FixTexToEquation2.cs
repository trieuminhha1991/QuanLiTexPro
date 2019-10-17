using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuanLyTex.User5Class
{
	class FixTexToEquation2
	{
		public string fixEnumerateItemize(string tex)
		{
			char i = 'a';
			try
			{
				tex = tex.Replace(@"\begin{enumerate}", "").Replace(@"\begin{itemize}", "").Replace(@"\begin{enumEX}", "").Replace(@"\begin{listEX}", "");
				tex = tex.Replace(@"\end{enumerate}", "\r\n").Replace(@"\end{itemize}", "\r\n").Replace(@"\end{enumEX}", "r\n").Replace(@"\end{listEX}", "\r\n");
				while (tex.Contains(@"\item"))
				{
					int start = tex.IndexOf(@"\item");
					tex = tex.Remove(start, 5).Insert(start, "\r\n" + i + ").  ");
					i++;
				}
				return tex;
			}
			catch
			{
				return tex;
			}
		}

		public string changeHevaAndHoac(string tex)
		{
			try
			{
				tex = tex.Replace(@"\begin{cases}", @"\heva{").Replace(@"\end{cases}", "}");
				//tex = tex.Replace(@"\left\{ \begin{aligned}", @"\heva{").Replace(@"\left[ \begin{aligned}", @"\hoac{").Replace(@"\end{aligned} \right.", "}");
				tex = tex.Replace(@"\\}", "}");
				tex = Regex.Replace(tex, @"\\(heva)[ ]{1,3}\{", @"\heva{");
				tex = Regex.Replace(tex, @"\\(hoac)[ ]{1,3}\{", @"\hoac{");
				int start = 0;
				int end = 0;
				try
				{
					while (tex.Contains(@"\heva{"))
					{
						start = tex.IndexOf(@"\heva{");
						end = tex.IndexOf(@"}", start);
						int check = 0;
						int i = start + 6;
						int j = start + 6;
						while (check < end && check >= 0)
						{
							end = tex.IndexOf(@"}", i);
							check = tex.IndexOf(@"{", j);
							if (check > 1 && tex[check - 1].ToString() == @"\")
							{
								check = tex.IndexOf("{", check + 1);
							}
							if (end >= 1 && tex[end - 1].ToString() == @"\")
							{
								end = tex.IndexOf("}", end + 1);
							}
							i = end + 1;
							j = check + 1;
						}
						string texSub = tex.Substring(start + 6, end - start - 6);
						texSub = texSub.Replace(@"\\", @"~%").Replace("&", "#!").Replace("$", "");
						texSub = @"\left\{ \eqarray{" + texSub + @"} \right.";
						tex = tex.Remove(start, end + 1 - start).Insert(start, texSub);
					}
				}
				catch
				{

				}
				while (tex.Contains(@"\hoac{"))
				{
					start = tex.IndexOf(@"\hoac{");
					end = tex.IndexOf(@"}", start);
					int check = 0;
					int i = start + 6;
					int j = start + 6;
					while (check < end && check >= 0)
					{
						end = tex.IndexOf(@"}", i);
						check = tex.IndexOf(@"{", j);
						if (check > 1 && tex[check - 1].ToString() == @"\")
						{
							check = tex.IndexOf("{", check + 1);
						}
						if (end >= 1 && tex[end - 1].ToString() == @"\")
						{
							end = tex.IndexOf("}", end + 1);
						}
						i = end + 1;
						j = check + 1;
					}
					string texSub = tex.Substring(start + 6, end - start - 6);
					texSub = texSub.Replace(@"\\", @"~%").Replace("&", "#!");
					texSub = @"\left[ \begin{align}" + texSub + @"\end{align} \right.";
					tex = tex.Remove(start, end + 1 - start).Insert(start, texSub);
				}
				return tex;
			}
			catch
			{
				return tex;
			}
		}
		public string fixAlignEqnarray(string tex)
		{
			try
			{
				if (tex.Contains(@"\begin{align}"))
				{
					Regex rg1 = new Regex(@"\\begin\{align}");
					//Regex rg1 = new Regex(@"(\\)(begin)(\{align\*})");
					List<int> list1 = new List<int>();
					foreach (Match match in rg1.Matches(tex))
					{
						list1.Add(match.Index);
					}
					Regex rg2 = new Regex(@"\\end\{align}");
					//Regex rg2 = new Regex(@"(\\)(end)(\{align\*})");
					List<int> list2 = new List<int>();
					foreach (Match match in rg2.Matches(tex))
					{
						list2.Add(match.Index);
					}
					if (list1.Count != list2.Count)
					{
						return tex;
					}
					for (int i = list1.Count - 1; i >= 0; i--)
					{
						string input = tex.Substring(list1[i], list2[i] + 11 - list1[i]);
						input = input.Replace("$", "");
						input = "\r\n$" + input.Replace(@"\\", @"~%").Replace("&", "#!").Replace(@"\begin{align}", @"\eqarray{").Replace(@"\end{align}", @"}") + "$\r\n";
						tex = tex.Remove(list1[i], list2[i] + 11 - list1[i]).Insert(list1[i], input);
					}
				}
				return tex;
			}
			catch
			{
				return tex;
			}
		}
	}
}
