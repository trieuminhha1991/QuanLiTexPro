using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuanLyTex.User5Class
{
	class FixTexToWord
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
				tex = Regex.Replace(tex,@"\\(heva)[ ]{1,3}\{", @"\heva{");
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
						while (check < end&&check>=0)
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
						texSub = texSub.Replace(@"\\", @"~%").Replace("&", "#!").Replace("$","");
						texSub = @"\left\{ \begin{align}" + texSub + @"\end{align} \right.";
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
						if (input.Length > 350 && !input.Contains(@"\heva{") && !input.Contains(@"\hoac{") && !input.Contains(@"\begin{aligned}") && !input.Contains(@"\begin{aligned*}") && !input.Contains(@"\begin{case}") && !input.Contains(@"\begin{matrix}") && !input.Contains(@"\begin{array}"))
						{
							input = input.Replace(@"\begin{align}", "").Replace(@"\end{align}", "").Replace(@"\\", "$\r\n$");
							input = input.Replace("&", "");
						}
						else if (input.Length > 250 && (input.Contains(@"\Leftrightarrow") || input.Contains(@"\Rightarrow")))
						{
							input = input.Replace(@"\\", "");
							input = input.Replace(@"\begin{align}", "").Replace(@"\end{align}", "").Replace(@"\Leftrightarrow", "$\r\n$\\Leftrightarrow").Replace(@"\Rightarrow", "$\r\n$\\Rightarrow");
							input = input.Replace("&", "");
							input = "{}" + input;
						}
						input = "\r\n$" + input.Replace(@"\\", @"~%").Replace("&", "#!") + "$\r\n";
						tex = tex.Remove(list1[i], list2[i] + 11 - list1[i]).Insert(list1[i], input);
					}
				}
				int start2 = 0;
				while (tex.IndexOf(@"\begin{aligned}", start2) >0)
				{
					try
					{
						int m= tex.IndexOf(@"\begin{aligned}", start2);
						int i = tex.IndexOf(@"\begin{aligned}", m+1);
						int n = tex.IndexOf(@"\end{aligned}", m);
						int j = n;
						while (j > 0 && i > 0 && i < j)
						{
							i = tex.IndexOf(@"\begin{aligned}", i + 1);
							j = tex.IndexOf(@"\end{aligned}", j + 1);
							if(j>0)
							{
								n = j;
							}
						}
						string input = tex.Substring(m, n+13-m);
						input = input.Replace("$", "");
						if (input.Length > 350 && !input.Contains(@"\heva{") && !input.Contains(@"\hoac{") && !input.Contains(@"\begin{case}") && !input.Contains(@"\begin{matrix}") && !input.Contains(@"\begin{array}"))
						{
							input = input.Replace(@"\begin{aligned}", "").Replace(@"\end{aligned}", "").Replace(@"\\", "$\r\n$");
							input = input.Replace("&", "");
						}
						else if (input.Length > 250 && (input.Contains(@"\Leftrightarrow") || input.Contains(@"\Rightarrow")))
						{
							input = input.Replace(@"\begin{aligned}", "").Replace(@"\end{aligned}", "").Replace(@"\Leftrightarrow", "$\r\n$\\Leftrightarrow").Replace(@"\Rightarrow", "$\r\n$\\Rightarrow");
							input = input.Replace(@"\\", "");
							input = input.Replace("&", "");
							input = "{}" + input;
						}
						//if(input[13]=='1')
						//{
						//	input = input.Remove(13, 1);
						//	int starttab = 0;
						//	int idex = 0;
						//	while (tex.IndexOf(@"\\", starttab) > 0)
						//	{
						//		idex = starttab;
						//		starttab = input.IndexOf("&", idex);
						//		int startline = input.IndexOf(@"\\", starttab);
						//		int starttab2 = input.IndexOf("&", starttab + 1);
						//		if (starttab2 < startline)
						//		{
						//			input = input.Remove(idex, 1);
						//		}
						//		starttab = startline;
						//	}
						//}
						input = input.Replace(@"\\", @"~%").Replace("&", "#!");
						tex = tex.Remove(m, n + 13 - m).Insert(m, input);
						start2 = n;
					}
					catch
					{
						break;
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
