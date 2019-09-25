using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuanLyTex.User5Class
{
	class TreatTex
	{
		public string fixHe(string tex,bool? fixHeHoac)
		{
			tex = tex.Replace("\t", "");
			int start = 0;
			int end = 0;
			while(tex.Contains(@"\left\{\begin{align}")|| tex.Contains(@"\left[\begin{align}"))
			{
				start = tex.IndexOf(@"\left\{\begin{align}");
				if(tex.IndexOf(@"\left[\begin{align}", end)<start&& tex.IndexOf(@"\left[\begin{align}", end)>0)
				{
					start = tex.IndexOf(@"\left[\begin{align}", end);
				}
				end = 1;
				int check = 0;
				int i = start+1;
				while (check <end)
				{
					end = tex.IndexOf(@"\end{align}\night.", i);
					check = tex.IndexOf(@"\left\{\begin{align}", i);
					if(check<0)
					{
						check= tex.IndexOf(@"\left[\begin{align}", i + 1);
					}
					if(check<0)
					{
						break;
					}
					i = end + 1;
				}
				string texSub = tex.Substring(start, end+18 - start);
				texSub = Regex.Replace(texSub, @"\s+", "");
				if (fixHeHoac == true)
				{
					texSub = texSub.Replace(@"\left\{\begin{align}", @"\heva{").Replace(@"\left[\begin{align}", @"\hoac{").Replace(@"\\\end{align}\right.", @"}");
				}
				else
				{
					texSub = texSub.Replace(@"\left\{\begin{align}", @"\left\{\begin{aligned}").Replace(@"\left[\begin{align}", @"\left[\begin{aligned}").Replace(@"\end{align}\night.", @"\end{aligned}\night.");
				}
				tex = tex.Remove(start, end+18 - start).Insert(start, texSub);
			}
			return tex;
		}
		public List<string> FilterId(string tex,bool?select,bool? ex,bool? bt,bool? vd)
		{
			try
			{
				List<string> list = new List<string>();
				int startTex = tex.IndexOf(@"\begin{document}");
				if (startTex > 0)
				{
					tex = tex.Substring(startTex + 16);
				}
				string exString = "ex";
				string btString = "bt";
				string vdString = "vd";
				string str = @"\begin{";
				string str2 = @"\end{";
				int startIndex = 0;
				string inputAdd;
				if (select == true)
				{
					int startIndex0 = tex.IndexOf(str + exString, startIndex);
					if ((startIndex0 < 0) || (tex.IndexOf(str + btString) < startIndex0 && tex.IndexOf(str + btString) > 0))
					{
						startIndex0 = tex.IndexOf(str + btString);
					}
					if ((startIndex0 < 0) || (tex.IndexOf(str + vdString) < startIndex0 && tex.IndexOf(str + vdString) > 0))
					{
						startIndex0 = tex.IndexOf(str + vdString);
					}
					inputAdd = tex.Substring(0, startIndex0);
					list.Add(inputAdd);
				}
				while (startIndex >= 0)
				{
					try
					{
						int check = startIndex;
						int i = 0;
						startIndex = tex.IndexOf(str + exString, check);
						if ((startIndex < 0) || (tex.IndexOf(str + btString, check) < startIndex && tex.IndexOf(str + btString, check) > 0))
						{
							startIndex = tex.IndexOf(str + btString, check);
							i = 1;
						}
						if ((startIndex < 0) || (tex.IndexOf(str + vdString, check) < startIndex && tex.IndexOf(str + vdString, check) > 0))
						{
							startIndex = tex.IndexOf(str + vdString, check);
							i = 2;
						}
						if (startIndex >= 0)
						{
							int endIndex = startIndex + 5;
							if (i == 0)
							{
								endIndex = tex.IndexOf(str2 + exString, startIndex);
							}
							if (i == 1)
							{
								endIndex = tex.IndexOf(str2 + btString, startIndex);
							}
							if (i == 2)
							{
								endIndex = tex.IndexOf(str2 + vdString, startIndex);
							}
							if (endIndex > 0)
							{
								if (i == 0 && ex == true)
								{
									int start = tex.IndexOf("}", startIndex);
									inputAdd = "e" + tex.Substring(start + 1, endIndex - start - 1);
									list.Add(inputAdd);
								}
								if (i == 1 && bt == true)
								{
									int start = tex.IndexOf("}", startIndex);
									inputAdd = "b" + tex.Substring(start + 1, endIndex - start - 1);
									list.Add(inputAdd);
								}
								if (i == 2 && vd == true)
								{
									int start = tex.IndexOf("}", startIndex);
									inputAdd = "v" + tex.Substring(start + 1, endIndex - start - 1);
									list.Add(inputAdd);
								}
								startIndex = endIndex + 2;
								if (select == true)
								{
									int endindex = tex.IndexOf("}", endIndex);
									int endindex2 = tex.IndexOf(str + exString, startIndex);
									if (tex.IndexOf(str + btString, startIndex) < startIndex && tex.IndexOf(str + btString, startIndex) > 0)
									{
										endindex2 = tex.IndexOf(str + btString, startIndex);
									}
									if (tex.IndexOf(str + vdString, startIndex) < startIndex && tex.IndexOf(str + vdString, startIndex) > 0)
									{
										endindex2 = tex.IndexOf(str + vdString, startIndex);
									}
									if (endindex2 > 0)
									{
										string subTex = tex.Substring(endindex + 1, endindex2 - endindex - 1);
										string subTex2 = Regex.Replace(subTex, @"\s+", "");
										if (subTex2.Length > 5)
										{
											list.Add(subTex);
										}
									}
								}
							}
							else
							{
								break;
							}
						}
					}
					catch
					{
						startIndex = startIndex + 1;
					}
				}
				return list;
			}
			catch
			{
				List<string> list = new List<string>();
				list.Add(tex);
				return list;
			}
		}
		public string startTreatTex(string tex,bool? fixHeHoac)
		{
			tex = tex.Replace("\n", @"\\" + "\n");
			tex = tex.Replace(@"\left\{ \begin{align}", @"\left\{\begin{align}");
			tex = tex.Replace(@"\left\{ \begin{matrix}", @"\left\{\begin{align}");
			tex = tex.Replace(@"\left[ \begin{align}", @"\left[\begin{align}");
			tex = tex.Replace(@"\left[ \begin{matrix}", @"\left[\begin{align}");
			tex = tex.Replace(@"\end{align} \right.", @"\end{align}\right.");
			tex = tex.Replace(@"\end{matrix} \right.", @"\end{align}\right.");
			tex = Regex.Replace(tex, @"[\{]{3,}", "{");
			tex = Regex.Replace(tex, @"[\}]{3,}", "}");
			tex = Regex.Replace(tex, @"[ ]{2,}", " ");
			tex = tex.Replace("${", "$");
			tex = tex.Replace("}$", "$");
			tex = fixHe(tex, fixHeHoac);
			int start = tex.IndexOf("\n");
			if(start >70)
			{
				start = 7;
			}
			string texSub = tex.Substring(0, start);
			texSub = texSub.Replace("(", "[").Replace("[", "%[").Replace(")", "]");
			texSub = @"\begin{ex}%" + texSub;
			tex = tex.Remove(0, start).Insert(0, texSub);
			return tex;
		}
		public string startAllTex(string tex)
		{
			while(tex.Contains("{{"))
			{
				int start = tex.IndexOf("{{");
				int end = 1;
				int check = 0;
				int i = start;
				while (end > check)
				{
					end = tex.IndexOf("}", i);
					check = tex.IndexOf("{", i + 2);
					if(check<0)
					{
						break;
					}
					i = end + 1;
				}
				tex = tex.Remove(end + 1, 1).Remove(start + 1, 1);
			}
			tex = tex.Replace(@"\[", "$").Replace(@"\]", "$");
			return tex;
		}
	}
}
