using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Application = Microsoft.Office.Interop.Word.Application;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System;
using System.Threading;

namespace QuanLyTex.User5Class
{
	class TexToWord
	{
		string imageAdd = "";
		List<string> listTableCheck = new List<string>();
		private int indeximg = 0;
		FixTexToWord Fix = new FixTexToWord();
		public string CapText(Match m)
		{
			string x = m.ToString();
			x = x.Replace(" ", "");
			x = x.Insert(x.Length - 1, "}").Insert(1, "{");
			return x;
		}
		public int getIndexSign(List<int> list1, List<int> list2, int n)
		{
			try
			{
				int m = 0;
				for (int j = n; j < list2.Count; j++)
				{
					if (list2[j] < list1[j + 1])
					{
						m = j;
						break;
					}
				}
				return m;
			}
			catch
			{
				return n;
			}
		}
		public string startTex(string tex)
		{
			try
			{
				tex = tex.Replace(@"\lq", @"'").Replace(@"\rq", @"'");
				tex = tex.Replace(@"\allowdisplaybreaks", "").Replace(@"\enskip", "");
				tex = tex.Replace(@"{,}", ",").Replace("’", "'").Replace(@"\noindent", "").Replace(@"\newline ", "\\").Replace(@"\hfill", @"\quad\quad");
				tex = tex.Replace(@"\tag", "").Replace(@"\wideparen", @"\overset\frown").Replace(@"longrightarrow", @"rightarrow");
				tex = tex.Replace(@"\[", "$").Replace(@"\]", "$").Replace("{'}", "'");
				tex = tex.Replace(@"alignat*", "align").Replace(@"alignat", "align");
				tex = tex.Replace("gathered", "align").Replace(@"eqnarray", @"align").Replace(@"equation", @"align").Replace(@"align*", @"align");
				tex = tex.Replace(@"\begin{array}", @"\begin{aligned}").Replace(@"\end{array}", @"\end{aligned}");
				tex = tex.Replace("{ll}", "").Replace("{l}", "").Replace(@"\,", "").Replace(@"\;", "");
				tex = tex.Replace(@"\immini", "");
				tex = tex.Replace(@"\lbrace", "{").Replace(@"\rbrace", "}");
				tex = tex.Replace(@"\left{", @"\left\{").Replace(@"\right}", @"\right\}");
				Regex rx = new Regex(@"\$([ ]{0,2})(\[{0,1})([A-Za-z0-9'.,; ]{1,14})(]{0,1})([ ]{0,2})\$");
				if (rx.IsMatch(tex))
				{
					tex = rx.Replace(tex, new MatchEvaluator(CapText));
				}
				tex = Regex.Replace(tex, "[ ]{2,}", " ");
				while (tex.Contains("$$"))
				{
					int start = tex.IndexOf("$$");
					int end = tex.IndexOf("$$", start + 2);
					if (end > 0)
					{
						if (tex.Substring(start + 2, end - start - 2).Contains("$"))
						{
							tex = tex.Remove(start, 2);
						}
						else
						{
							tex = tex.Remove(end, 2).Insert(end, @"$");
							tex = tex.Remove(start, 2).Insert(start, @"\quad$");
						}
					}
				}
				tex = Regex.Replace(tex, @"\$[ ]{1}\$", "");
				return tex;
			}
			catch
			{
				return tex;
			}
		}
		private string treatTex(string tex)
		{
			tex = startTex(tex);
			tex = tex.Replace(" ", "!!");
			tex = Regex.Replace(tex, @"\s+", "");
			tex = tex.Replace("!!", " ");
			tex = tex.Replace("$$", "");
			tex = tex.Replace(@"\quad", "\t");
			tex = Fix.fixAlignEqnarray(tex);
			tex = Regex.Replace(tex, @"\$[ ]{1}\$", "$");
			tex = Regex.Replace(tex, @"(\\left[ ]{0,2}\[)", @"\left[");
			tex = Regex.Replace(tex, @"(\\left[ ]{0,2}\\\{)", @"\left\{");
			tex = tex.Replace(@"\begin{aligned}", @"\begin{align}").Replace(@"\end{aligned}", @"\end{align}");
			tex = Fix.changeHevaAndHoac(tex);
			tex = Fix.fixEnumerateItemize(tex);
			tex = tex.Replace(@"\\", "\r\n");
			tex = tex.Replace(@"~%", @"\\");
			tex = tex.Replace(@"#!", "&");
			return tex;
		}
		public string runLatexImage(string tiz, bool? fixImage)
		{
			try
			{
				if (fixImage == true)
				{
					string header = @"\documentclass[a6paper]{standalone}
                                                \usepackage{amsmath}
                                                \usepackage{amsfonts}
                                                \usepackage{amssymb}
                                                \usepackage{tikz,tkz-euclide,tkz-tab}
                                                \usetkzobj{all}
                                                \begin{document}" + "\r";
					string footer = "\r" + @"\end{document}";
					string appPath = Directory.GetCurrentDirectory();
					File.WriteAllText(appPath + @"\Bat\input.tex", header + tiz + footer);
					Process p1 = new Process();
					p1.StartInfo.FileName = "batch.bat";
					p1.StartInfo.UseShellExecute = false;
					p1.StartInfo.CreateNoWindow = true;
					p1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					p1.Start();
					p1.WaitForExit();
					string PdfFile = appPath + @"\Bat\input.pdf";
					string PngFile = appPath + @"\Bat\img" + indeximg + ".png";
					indeximg++;
					FileStream file = new FileStream(PdfFile, FileMode.Open, FileAccess.Read);
					var doc = new TallComponents.PDF.Rasterizer.Document(file);
					TallComponents.PDF.Rasterizer.Page pdfPage = doc.Pages[0];
					float resolution = 400;
					float scale = resolution / 72f;
					int bmpWidth = (int)(scale * pdfPage.Width);
					int bmpHeight = (int)(scale * pdfPage.Height);
					Bitmap bitmap = new Bitmap(bmpWidth, bmpHeight);
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.ScaleTransform(scale, scale);
						pdfPage.Draw(graphics);
						bitmap.Save(PngFile, ImageFormat.Png);
					}
					file.Close();
					string imageTex = "(img)" + PngFile + "(endimg)";
					return imageTex;
				}
				else
				{
					string imageTex = "\\& NoiCoHinh\\";
					return imageTex;
				}
			}
			catch
			{
				string imageTex = "\\& NoiCoHinh\\";
				return imageTex;
			}
		}
		public string getPathImage2(string tex, bool? fixImage)
		{
			try
			{
				if (tex.Contains(@"\begin{tikzpicture}"))
				{
					int start, end;
					string tiz, path;
					Regex rg1 = new Regex(@"\\begin\{tikzpicture\}");
					List<int> list1 = new List<int>();
					foreach (Match match in rg1.Matches(tex))
					{
						list1.Add(match.Index);
					}
					Regex rg2 = new Regex(@"\\end\{tikzpicture\}");
					List<int> list2 = new List<int>();
					foreach (Match match in rg2.Matches(tex))
					{
						list2.Add(match.Index);
					}
					for (int i = list1.Count - 1; i >= 0; i--)
					{
						start = list1[i];
						end = list2[i];
						tiz = tex.Substring(start, end + 17 - start);
						path = runLatexImage(tiz, fixImage);
						tex = tex.Remove(start, end + 17 - start).Insert(start, path);
					}
				}
				if (tex.Contains(@"\begin{tabular}"))
				{
					int start, end;
					string tiz, path;
					Regex rg1 = new Regex(@"\\begin\{tabular\}");
					List<int> list1 = new List<int>();
					foreach (Match match in rg1.Matches(tex))
					{
						list1.Add(match.Index);
					}
					Regex rg2 = new Regex(@"\\end\{tabular\}");
					List<int> list2 = new List<int>();
					foreach (Match match in rg2.Matches(tex))
					{
						list2.Add(match.Index);
					}
					for (int i = list1.Count - 1; i >= 0; i--)
					{
						start = list1[i];
						end = list2[i];
						tiz = tex.Substring(start, end + 13 - start);
						path = runLatexImage(tiz, fixImage);
						tex = tex.Remove(start, end + 13 - start).Insert(start, path);
					}
				}
				return tex;
			}
			catch
			{
				return tex;
			}
		}
		public string getPathImage3(string tex, bool? fixImage)
		{
			try
			{
				if (tex.Contains(@"\begin{tikzpicture}"))
				{
					int start, end;
					string tiz, path;
					Regex rg1 = new Regex(@"\\begin\{tikzpicture\}");
					List<int> list1 = new List<int>();
					foreach (Match match in rg1.Matches(tex))
					{
						list1.Add(match.Index);
					}
					Regex rg2 = new Regex(@"\\end\{tikzpicture\}");
					List<int> list2 = new List<int>();
					foreach (Match match in rg2.Matches(tex))
					{
						list2.Add(match.Index);
					}
					for (int i = list1.Count - 1; i >= 0; i--)
					{
						start = list1[i];
						end = list2[i];
						tiz = tex.Substring(start, end + 17 - start);
						path = runLatexImage(tiz, fixImage);
						tex = tex.Remove(start, end + 17 - start).Insert(start, "\r\n" + path + "\r\n");
					}
				}
				if (tex.Contains(@"\begin{tabular}"))
				{
					int start, end;
					string tiz, path;
					Regex rg1 = new Regex(@"\\begin\{tabular\}");
					List<int> list1 = new List<int>();
					foreach (Match match in rg1.Matches(tex))
					{
						list1.Add(match.Index);
					}
					Regex rg2 = new Regex(@"\\end\{tabular\}");
					List<int> list2 = new List<int>();
					foreach (Match match in rg2.Matches(tex))
					{
						list2.Add(match.Index);
					}
					for (int i = list1.Count - 1; i >= 0; i--)
					{
						start = list1[i];
						end = list2[i];
						tiz = tex.Substring(start, end + 13 - start);
						path = runLatexImage(tiz, fixImage);
						tex = tex.Remove(start, end + 13 - start).Insert(start, "\r\n" + path + "\r\n");
					}
				}
				return tex;
			}
			catch
			{
				return tex;
			}
		}
		public List<string> getPathImage(string tex, bool? fixImage)
		{

			List<string> list = new List<string>();
			try
			{
				if (tex.Contains(@"\begin{tikzpicture}"))
				{
					int start, end;
					string tiz, path;
					Regex rg1 = new Regex(@"\\begin\{tikzpicture\}");
					List<int> list1 = new List<int>();
					foreach (Match match in rg1.Matches(tex))
					{
						list1.Add(match.Index);
					}
					Regex rg2 = new Regex(@"\\end\{tikzpicture\}");
					List<int> list2 = new List<int>();
					foreach (Match match in rg2.Matches(tex))
					{
						list2.Add(match.Index);
					}
					for (int i = list1.Count - 1; i >= 0; i--)
					{
						start = list1[i];
						end = list2[i];
						tiz = tex.Substring(start, end + 18 - start);
						path = runLatexImage(tiz, fixImage);
						list.Add(path);
						tex = tex.Remove(start, end + 17 - start);

					}
				}
				if (tex.Contains(@"\begin{tabular}"))
				{
					int start, end;
					string tiz, path;
					Regex rg1 = new Regex(@"\\begin\{tabular\}");
					List<int> list1 = new List<int>();
					foreach (Match match in rg1.Matches(tex))
					{
						list1.Add(match.Index);
					}
					Regex rg2 = new Regex(@"\\end\{tabular\}");
					List<int> list2 = new List<int>();
					foreach (Match match in rg2.Matches(tex))
					{
						list2.Add(match.Index);
					}
					for (int i = list1.Count - 1; i >= 0; i--)
					{
						start = list1[i];
						end = list2[i];
						tiz = tex.Substring(start, end + 13 - start);
						path = runLatexImage(tiz, fixImage);
						list.Add(path);
						tex = tex.Remove(start, end + 13 - start);
					}
				}
				list.Add(tex);
				return list;
			}
			catch
			{
				list.Add(tex);
				return null;
			}
		}


		private string DeleteName(string tex, bool? deleteName, bool? deleteSchool, bool? deleteId, string NameDuAn)
		{
			if (deleteName == true)
			{
				string check = NameDuAn;
				int index = tex.IndexOf(check);
				if (index >= 0)
				{
					string subtex = tex.Substring(0, index);
					int start = subtex.LastIndexOf("[");
					int end = tex.IndexOf("]", index);
					tex = tex.Remove(start, end - start);
				}
			}
			if (deleteSchool == true)
			{
				string check = "Trường";
				int index = tex.IndexOf(check);
				if (index >= 0)
				{
					string subtex = tex.Substring(0, index);
					int start = subtex.LastIndexOf("[");
					int end = tex.IndexOf("]", index);
					tex = tex.Remove(start, end - start);
				}
			}
			if (deleteId == true)
			{
				tex = Regex.Replace(tex, @"\[[0-9][DH][0-9][YBKGT][0-9][ -][ 0-9]]", "");
			}
			return tex;
		}
		private List<string> getIdQuestion(List<string> list, bool? deleteName, bool? deleteSchool, bool? deleteId, string NameDuAn)
		{
			try
			{
				string str = list[list.Count - 1];
				int index1 = str.IndexOf("\r");
				int index2 = str.IndexOf("\n");
				if (index1 > 2 || index2 > 2)
				{
					int index = 0;
					if (index1 > index2 && index2 > 0 || index1 < 0)
					{
						index = index2;
					}
					else
					{
						index = index1;
					}
					string str1 = str.Substring(0, index);
					str1 = DeleteName(str1, deleteName, deleteSchool, deleteId, NameDuAn);
					string str2 = str.Substring(index + 1);
					str1 = str1.Replace("%", "");
					str2 = treatTex(str2);
					list[list.Count - 1] = str1;
					list.Add(str2);
				}
				else
				{
					string str1 = "[Id][Name]";
					string str2 = str;
					str2 = treatTex(str2);
					list[list.Count - 1] = str1;
					list.Add(str2);
				}
				return list;
			}
			catch
			{
				return list;
			}
		}
		public Dictionary<string, dynamic> changeTexToWord(string tex, bool? fixImage, bool? deleteName, bool? deleteSchool, bool? deleteId, string NameDuAn)
		{
			tex = tex.Replace(@"\motcot", @"\choice").Replace(@"\haicot", @"\choice").Replace(@"\boncot", @"\choice");
			imageAdd = "";
			try
			{
				string st1 = "", st2 = "", st3 = "";
				List<string> list1 = new List<string>();
				List<string> list3 = new List<string>();
				Dictionary<string, dynamic> Dic = new Dictionary<string, dynamic>();
				if (tex[0] == 'e')
				{
					Dic.Add("loai", 1);
					tex = tex.Substring(1);
				}
				else if (tex[0] == 'b')
				{
					Dic.Add("loai", 2);
					tex = tex.Substring(1);
				}
				else if (tex[0] == 'v')
				{
					Dic.Add("loai", 3);
					tex = tex.Substring(1);
				}
				else
				{
					Dic.Add("loai", 0);
				}
				if (Dic["loai"] == 0)
				{
					tex = getPathImage3(tex, fixImage);
					tex = treatTex(tex);
					list1.Add(tex);
				}
				else
				{
					int index1 = tex.IndexOf(@"\choice");
					int index2 = tex.IndexOf(@"\loigiai");
					if (index1 > 0 && index2 > 0)
					{
						st1 = tex.Substring(0, index1);
						list1 = getPathImage(st1, fixImage);
						list1 = getIdQuestion(list1, deleteName, deleteSchool, deleteId, NameDuAn);
						st2 = tex.Substring(index1, index2 - index1);
						st2 = getPathImage2(st2, fixImage);
						st2 = treatTex(st2);
						int end = tex.LastIndexOf("}");
						st3 = tex.Substring(index2);
						list3 = getPathImage(st3, fixImage);
						string str3B = list3[list3.Count - 1];
						str3B = treatTex(str3B);
						list3[list3.Count - 1] = str3B;
					}
					if (index1 > 0 && index2 <= 0)
					{
						st1 = tex.Substring(0, index1);
						list1 = getPathImage(st1, fixImage);
						list1 = getIdQuestion(list1, deleteName, deleteSchool, deleteId, NameDuAn);
						st2 = tex.Substring(index1);
						st2 = getPathImage2(st2, fixImage);
						st2 = treatTex(st2);
					}
					if (index1 < 0 && index2 > 0)
					{
						st1 = tex.Substring(0, index2);
						list1 = getPathImage(st1, fixImage);
						list1 = getIdQuestion(list1, deleteName, deleteSchool, deleteId, NameDuAn);
						st3 = tex.Substring(index2);
						list3 = getPathImage(st3, fixImage);
						string str3B = list3[list3.Count - 1];
						str3B = treatTex(str3B);
						list3[list3.Count - 1] = str3B;
					}
					if (index1 <= 0 && index2 <= 0)
					{
						st1 = tex;
						list1 = getPathImage(st1, fixImage);
						list1 = getIdQuestion(list1, deleteName, deleteSchool, deleteId, NameDuAn);
					}
				}
				Dic.Add("question", list1);
				Dic.Add("choice", st2);
				Dic.Add("proof", list3);
				return Dic;
			}
			catch
			{
				Dictionary<string, dynamic> Dic = new Dictionary<string, dynamic>();
				Dic.Add("question", tex);
				Dic.Add("choice", "");
				Dic.Add("proof", "");
				return Dic;
			}
		}
		public List<string> changeChoiceTexToWord(string tex)
		{
			try
			{
				string select = "";
				char dapan = 'A';
				int start = 0;
				int end = 0;
				int indexChoice = 0;
				string input = "";
				List<string> list = new List<string>();
				while (start < tex.LastIndexOf("}"))
				{
					start = tex.IndexOf("{", start);

					int check = 0;
					end = tex.IndexOf("}", start);
					int i = start + 1;
					int j = start + 1;
					while (check < end && check >= 0)
					{
						end = tex.IndexOf(@"}", i);
						check = tex.IndexOf(@"{", j);
						if (check >= 1 && tex[check - 1].ToString() == @"\")
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
					input = tex.Substring(start + 1, end - start - 1);
					if (input.Contains(@"\True"))
					{
						select = dapan.ToString();
					}
					list.Add(input);
					start = end;
					indexChoice++;
					dapan++;
					if (indexChoice == 4)
					{
						break;
					}
				}
				if (select == "")
				{
					select = "NO";
				}
				listTableCheck.Add(select);
				try
				{
					string texSub = tex.Substring(start);
					if (texSub.Contains("(img)"))
					{
						int startimage = texSub.IndexOf("(img)");
						int endimage = texSub.IndexOf("(endimg)");
						imageAdd = texSub.Substring(startimage, endimage + 8 - startimage);
					}
				}
				catch
				{

				}
				if (list.Count == 4)
				{
					return list;
				}
				else
				{
					list = new List<string>() { "Lỗi dấu {}", "Lỗi dấu {}", "Lỗi dấu {}", "Lỗi dấu {}" };
					listTableCheck.Add("NO");
					return list;
				}
			}
			catch
			{
				List<string> list = new List<string>() { "Lỗi dấu {}", "Lỗi dấu {}", "Lỗi dấu {}", "Lỗi dấu {}" };
				listTableCheck.Add("NO");
				return list;
			}
		}

		public void addTextToWord(List<string> list, string path, bool? ToogleTex1, bool? ToogleTex2, bool? fixImage, bool? selectFilter, bool? deleteName, bool? deleteSchool, bool? deleteId, string NameDuAn, bool? addTable, bool? addPdf, bool? Runword, Application app, Dictionary<string, string> dic)
		{
			listTableCheck = new List<string>();
			try
			{
				string appPath = Directory.GetCurrentDirectory();
				string imagepath;
				object missing = System.Reflection.Missing.Value;
				Microsoft.Office.Interop.Word.Document document = app.Documents.Add();
				document.PageSetup.BottomMargin = 36;
				document.PageSetup.TopMargin = 36;
				document.PageSetup.LeftMargin = 60;
				document.PageSetup.RightMargin = 36;
				if (Runword == true)
				{
					document.Application.Visible = false;
				}
				document.Content.Font.Name = "Times New Roman (Headings)";
				document.Content.Font.Size = 12;
				ListTemplate template = document.ListTemplates.Add(true, "template1");
				ListLevel level = template.ListLevels[1];
				level.NumberFormat = "Câu %1.";
				level.NumberStyle = WdListNumberStyle.wdListNumberStyleArabic;
				level.NumberPosition = 0;
				level.TextPosition = 60;
				level.StartAt = 1;
				level.Font.Bold = 1;
				level.Font.Color = WdColor.wdColorBlue;
				level.Font.Italic = 1;
				level.Font.Underline = WdUnderline.wdUnderlineDouble;
				level.Font.Size = 12;
				ListTemplate template2 = document.ListTemplates.Add(true, "template2");
				ListLevel level2 = template2.ListLevels[1];
				level2.NumberFormat = "Bài %1.";
				level2.NumberStyle = WdListNumberStyle.wdListNumberStyleArabic;
				level2.NumberPosition = 0;
				level2.TextPosition = 60;
				level2.StartAt = 1;
				level2.Font.Bold = 1;
				level2.Font.Color = WdColor.wdColorDarkBlue;
				level2.Font.Italic = 1;
				level2.Font.Underline = WdUnderline.wdUnderlineDouble;
				level2.Font.Size = 12;
				ListTemplate template3 = document.ListTemplates.Add(true, "template3");
				ListLevel level3 = template3.ListLevels[1];
				level3.NumberFormat = "Ví dụ %1.";
				level3.NumberStyle = WdListNumberStyle.wdListNumberStyleArabic;
				level3.NumberPosition = 0;
				level3.TextPosition = 60;
				level3.StartAt = 1;
				level3.Font.Bold = 1;
				level3.Font.Color = WdColor.wdColorBlue;
				level3.Font.Italic = 1;
				level3.Font.Underline = WdUnderline.wdUnderlineDouble;
				level3.Font.Size = 12;
				foreach (string item in list)
				{
					int startitem = document.Content.End - 1;
					try
					{
						string chtrue = "";
						Dictionary<string, dynamic> Dic = changeTexToWord(item, fixImage, deleteName, deleteSchool, deleteId, NameDuAn);
						List<string> list1 = Dic["question"];
						string strQuestion = list1[list1.Count - 1];
						var range = document.Range(document.Content.End - 1);
						range.ParagraphFormat.TabStops.ClearAll();
						range = document.Range(document.Content.End - 1);
						if (Dic["loai"] == 1)
						{
							range.ListFormat.ApplyListTemplateWithLevel(template, true, WdListApplyTo.wdListApplyToSelection, missing, 1);
						}
						if (Dic["loai"] == 2)
						{
							range.ListFormat.ApplyListTemplateWithLevel(template2, true, WdListApplyTo.wdListApplyToSelection, missing, 1);
						}
						if (Dic["loai"] == 3)
						{
							range.ListFormat.ApplyListTemplateWithLevel(template3, true, WdListApplyTo.wdListApplyToSelection, missing, 1);
						}
						if (Dic["loai"] == 0)
						{
							range.ListFormat.RemoveNumbers();
						}
						if (Dic["loai"] == 0)
						{
							range.Font.Color = WdColor.wdColorBlack;
							range.ParagraphFormat.TabStops.Add(60, WdTabAlignment.wdAlignTabLeft);
							range.ParagraphFormat.TabStops.Add(90, WdTabAlignment.wdAlignTabLeft);
							range.ParagraphFormat.TabStops.Add(120, WdTabAlignment.wdAlignTabLeft);
							range.ParagraphFormat.TabStops.Add(150, WdTabAlignment.wdAlignTabLeft);
							range.Text = list1[list1.Count - 1];
							range.InsertParagraphAfter();
						}
						else
						{
							range.Text = list1[list1.Count - 2];
							range.Font.Color = WdColor.wdColorBrightGreen;
							range.InsertParagraphAfter();
							range = document.Range(range.End - 1);
							range.ListFormat.RemoveNumbers();
							range.ParagraphFormat.TabStops.Add(90, WdTabAlignment.wdAlignTabLeft);
							range.ParagraphFormat.TabStops.Add(120, WdTabAlignment.wdAlignTabLeft);
							range.ParagraphFormat.TabStops.Add(150, WdTabAlignment.wdAlignTabLeft);
							range.ParagraphFormat.LeftIndent = 60;
							range.Font.Color = WdColor.wdColorBlack;
							range.Text = strQuestion;
							range.InsertParagraphAfter();
							range.ParagraphFormat.TabStops.ClearAll();
							if (list1.Count > 2)
							{
								for (int i = 0; i <= list1.Count - 3; i++)
								{
									try
									{
										range = document.Range(range.End - 1);
										imagepath = list1[i].Remove(list1[i].Length - 8, 8).Remove(0, 5);
										range.ParagraphFormat.LeftIndent = 170;
										InlineShape shape = range.InlineShapes.AddPicture(imagepath);
										shape.Width = shape.Width / 2;
										shape.Height = shape.Height / 2;
										range.InsertParagraphAfter();
									}
									catch
									{
										range = document.Range(range.End - 1);
										range.Font.Bold = 0;
										range.Font.ColorIndex = WdColorIndex.wdBlack;
										range.Text = list1[i] + ".\n";
									}
								}
							}

							if (Dic["choice"] != "")
							{
								range = document.Range(range.End - 1);
								range.ParagraphFormat.LeftIndent = 60;
								List<string> choice = changeChoiceTexToWord(Dic["choice"]);
								int max = choice[1].Length;
								for (int i = 1; i < 4; i++)
								{
									if (choice[i].Length > max)
									{
										max = choice[i].Length;
									}
								}
								range.ParagraphFormat.TabStops.Add(60, WdTabAlignment.wdAlignTabLeft);
								if (max <= 15 || max >= 40)
								{
									range.ParagraphFormat.TabStops.Add(170, WdTabAlignment.wdAlignTabLeft);
								}
								range.ParagraphFormat.TabStops.Add(280, WdTabAlignment.wdAlignTabLeft);
								range.ParagraphFormat.TabStops.Add(390, WdTabAlignment.wdAlignTabLeft);
								char ch = 'A';
								for (int i = 0; i < 4; i++)
								{
									try
									{
										var item2 = choice[i];
										range = document.Range(range.End - 1);
										range.Text = ch + ". ";
										range.Font.Bold = 1;
										range.Font.Name = "Times New Roman(Headings)";
										range.Font.Size = 12;
										if (item2.Contains(@"\True"))
										{
											range.Font.Color = WdColor.wdColorRed;
											chtrue = ch.ToString();
										}
										else
										{
											range.Font.Color = WdColor.wdColorDarkBlue;
										}
										if (item2.Contains("(img)"))
										{
											try
											{
												int start = item2.IndexOf("(img)");
												int end = item2.IndexOf("(endimg)");
												imagepath = item2.Substring(start + 5, end - start - 5);
												range = document.Range(range.End - 1);
												InlineShape shape = range.InlineShapes.AddPicture(imagepath);
												shape.Width = shape.Width / 4;
												shape.Height = shape.Height / 4;
												if (i == 0 || i == 2)
												{
													range.InsertAfter("\t");
												}
												else
												{
													range.InsertAfter("\n");
												}
											}
											catch
											{
												range = document.Range(range.End - 1);
												range.Font.Bold = 0;
												range.Font.Color = WdColor.wdColorBlack;
												range.Text = item2 + ".\n";
											}
										}
										else
										{
											if (item2.Contains(@"\True"))
											{
												item2 = item2.Substring(5);
											}
											range = document.Range(range.End - 1);
											if (max >= 40)
											{
												range.Text = item2 + ".\n";
											}
											if (15 < max && max < 40)
											{
												if (i == 0 || i == 2)
												{
													range.Text = item2 + ".\t";
												}
												else
												{
													range.Text = item2 + ".\n";
												}
											}
											if (max <= 15)
											{
												if (i < 3)
												{
													range.Text = item2 + ".\t";
												}
												if (i == 3)
												{
													range.Text = item2 + ".\n";
												}
											}
											range.Font.Bold = 0;
											range.Font.Color = WdColor.wdColorBlack;
										}
										ch++;
									}
									catch
									{

									}
								}
								if (imageAdd != "")
								{
									try
									{
										range = document.Range(range.End - 1);
										imagepath = imageAdd.Remove(imageAdd.Length - 8, 8).Remove(0, 5);
										range.ParagraphFormat.LeftIndent = 170;
										InlineShape shape = range.InlineShapes.AddPicture(imagepath);
										shape.Width = shape.Width / 2;
										shape.Height = shape.Height / 2;
										range.InsertParagraphAfter();
									}
									catch
									{

									}
								}
							}
							if (Dic["proof"].Count > 0)
							{
								try
								{
									range = document.Range(range.End - 1);
									List<string> list2 = Dic["proof"];
									range.ParagraphFormat.LeftIndent = 60;
									range.ParagraphFormat.TabStops.Add(60, WdTabAlignment.wdAlignTabLeft);
									range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
									range.Font.Color = WdColor.wdColorBlack;
									range.Bold = 1;
									range.Text = "Lời giải.\r\n";
									if (list2.Count > 1)
									{
										for (int i = 0; i <= list2.Count - 2; i++)
										{
											try
											{
												range = document.Range(range.End - 1);
												imagepath = list2[i].Remove(list2[i].Length - 8, 8).Remove(0, 5);
												InlineShape shape = range.InlineShapes.AddPicture(imagepath);
												shape.Width = shape.Width / 2;
												shape.Height = shape.Height / 2;
												range.InsertParagraphAfter();
											}
											catch
											{
												range = document.Range(range.End - 1);
												range.ParagraphFormat.LeftIndent = 60;
												range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
												range.Font.Color = WdColor.wdColorBlack;
												range.Font.Bold = 0;
												range.Text = list2[i];
												range.InsertParagraphAfter();

											}
										}
									}
									if (chtrue != "")
									{
										range = document.Range(range.End - 1);
										range.ParagraphFormat.LeftIndent = 60;
										range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
										range.Font.Color = WdColor.wdColorDarkBlue;
										range.Bold = 1;
										range.Text = "Đáp án đúng: " + chtrue;
										range.InsertParagraphAfter();
									}
									string texProof = list2[list2.Count - 1];
									texProof = texProof.Substring(9, texProof.Length - 9);
									range = document.Range(range.End - 1);
									range.ParagraphFormat.LeftIndent = 60;
									range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
									range.ParagraphFormat.TabStops.Add(120, WdTabAlignment.wdAlignTabLeft);
									range.Font.Color = WdColor.wdColorBlack;
									range.Font.Bold = 0;
									range.Text = texProof;
									range.InsertParagraphAfter();
								}
								catch
								{

								}
							}
						}
						if (ToogleTex1 == true)
						{
							range = document.Range(startitem, document.Content.End - 1);
							range.Select();
							range.Application.Run("Macro");
						}
					}
					catch
					{
						Range range = document.Range(startitem, document.Content.End);
						range.Select();
						range.Application.Run("Macro");
					}
				}
				Range rangeend = document.Content;
				Microsoft.Office.Interop.Word.Find find = rangeend.Find;
				if (selectFilter == true)
				{
					try
					{
						int indexdang = 1;
						int indexsection = 1;
						int indexsubtion = 1;
						rangeend = document.Content;
						find = rangeend.Find;
						find.Execute(FindText: "*{", Replace: WdReplace.wdReplaceAll, ReplaceWith: "{");
						rangeend = document.Content;
						find = rangeend.Find;
						find.Execute(FindText: @"\\Opensolutionfile\{*\}\[*\]", Replace: WdReplace.wdReplaceAll, ReplaceWith: "", MatchWildcards: true);
						find.Execute(FindText: @"\\Closesolutionfile\{*\}", Replace: WdReplace.wdReplaceAll, ReplaceWith: "", MatchWildcards: true);
						rangeend = document.Content;
						find = rangeend.Find;
						find.Text = @"\(img\)*\(endimg\)";
						find.MatchWildcards = true;
						while (find.Execute())
						{
							int start = rangeend.Start;
							string texend = rangeend.Text.Remove(rangeend.Text.Length - 8, 8).Remove(0, 5);
							rangeend.Text = "";
							Range rangenew = document.Range(start);
							rangenew.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
							InlineShape shape = rangenew.InlineShapes.AddPicture(texend);
							find.Wrap = WdFindWrap.wdFindContinue;
						}
						rangeend = document.Content;
						find = rangeend.Find;
						find.Text = @"\\section\{*\}";
						find.MatchWildcards = true;
						while (find.Execute())
						{
							int start = rangeend.Start;
							string texend = rangeend.Text.Substring(9, rangeend.Text.Length - 10);
							rangeend.Text = " \r\n";
							Range rangenew = document.Range(start, start + 1);
							rangenew.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
							rangenew.Text = "BÀI THỨ " + indexsection + ": " + texend.ToUpper();
							rangenew.Font.Bold = 1;
							rangenew.Font.Size = 30;
							rangenew.Font.Name = "Palatino Linotype";
							rangenew.Font.Color = WdColor.wdColorSkyBlue;
							find.Wrap = WdFindWrap.wdFindContinue;
						}
						rangeend = document.Content;
						find = rangeend.Find;
						find.Text = @"\\chapter\{*\}";
						find.MatchWildcards = true;
						while (find.Execute())
						{
							int start = rangeend.Start;
							string texend = rangeend.Text.Substring(9, rangeend.Text.Length - 10);
							rangeend.Text = " \r\n";
							Range rangenew = document.Range(start, start + 1);
							rangenew.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
							rangenew.Text = "CHƯƠNG " + indexsection + ": " + texend.ToUpper();
							rangenew.Font.Bold = 1;
							rangenew.Font.Size = 40;
							rangenew.Font.Name = "Palatino Linotype";
							rangenew.Font.Color = WdColor.wdColorDarkYellow;
							find.Wrap = WdFindWrap.wdFindContinue;
						}
						rangeend = document.Content;
						find = rangeend.Find;
						find.Text = @"\\subsection\{*\}";
						find.MatchWildcards = true;
						while (find.Execute())
						{
							int start = rangeend.Start;
							string texend = rangeend.Text.Substring(12, rangeend.Text.Length - 13);
							rangeend.Text = " \r\n";
							Range rangenew = document.Range(start, start + 1);
							rangenew.Text = "PHẦN " + indexsubtion + ": " + texend.ToUpper();
							rangenew.Font.Bold = 1;
							rangenew.Font.Size = 20;
							rangenew.Font.Color = WdColor.wdColorPaleBlue;
							rangenew.Font.Name = "Palatino Linotype";
							indexsubtion++;
							find.Wrap = WdFindWrap.wdFindContinue;
						}
						rangeend = document.Content;
						find = rangeend.Find;
						find.Text = @"\\subsubsection\{*\}";
						find.MatchWildcards = true;
						while (find.Execute())
						{
							int start = rangeend.Start;
							string texend = rangeend.Text.Substring(15, rangeend.Text.Length - 16);
							rangeend.Text = " \r\n";
							Range rangenew = document.Range(start, start + 1);
							rangenew.Text = indexsubtion + ": " + texend.ToUpper();
							rangenew.Font.Bold = 1;
							rangenew.Font.Size = 20;
							rangenew.Font.Color = WdColor.wdColorDarkRed;
							rangenew.Font.Name = "Palatino Linotype";
							indexsubtion++;
							find.Wrap = WdFindWrap.wdFindContinue;
						}
						string dang = "dang";
						if (dic.ContainsKey("dang")) { dang = dic["dang"]; }
						rangeend = document.Content;
						find = rangeend.Find;
						find.Text = @"(\\begin\{" + dang + @"\})(*)(\\end\{" + dang + @"\})";
						find.MatchWildcards = true;
						while (find.Execute())
						{
							int start = rangeend.Start;
							string texend = rangeend.Text.Substring(12, rangeend.Text.Length - 23);
							rangeend.Text = " \r\n";
							int starttexend = texend.IndexOf("{");
							int endtexend = texend.IndexOf("}");
							string texend1 = "";
							string texend2 = texend;
							if (starttexend >= 0 && endtexend >= 0)
							{
								texend1 = texend.Substring(starttexend + 1, endtexend - starttexend);
								texend2 = texend.Substring(endtexend + 1);
							}
							Range rangenew = document.Range(start, start + 1);
							Microsoft.Office.Interop.Word.Table table = rangeend.Tables.Add(rangenew, 2, 1, WdDefaultTableBehavior.wdWord9TableBehavior, WdAutoFitBehavior.wdAutoFitWindow);
							Microsoft.Office.Interop.Word.Row row = table.Rows[1];
							row.Shading.BackgroundPatternColor = WdColor.wdColorGold;
							row.Range.Text = "Dạng " + indexdang + ":\t" + texend1;
							row.Range.Font.Bold = 1;
							row = table.Rows[2];
							row.Shading.BackgroundPatternColor = WdColor.wdColorGray15;
							row.Range.Text = texend2;
							find.Wrap = WdFindWrap.wdFindContinue;
						}
						rangeend = document.Content;
						find = rangeend.Find;
						find.Text = @"\\begin\{*\}";
						find.MatchWildcards = true;
						while (find.Execute())
						{
							try
							{
								if (dic.ContainsKey("dn") && rangeend.Text.Contains(dic["dn"]))
								{
									rangeend.Text = "\r\nĐỊNH NGHĨA.\t";
									rangeend.Font.Bold = 1;
									rangeend.Font.Size = 12;
									rangeend.Font.Color = WdColor.wdColorBlue;
									find.Wrap = WdFindWrap.wdFindContinue;
								}
								else if (dic.ContainsKey("dl") && rangeend.Text.Contains(dic["dl"]))
								{
									rangeend.Text = "\r\nĐỊNH LÍ.\t";
									rangeend.Font.Bold = 1;
									rangeend.Font.Size = 12;
									rangeend.Font.Color = WdColor.wdColorBlue;
									find.Wrap = WdFindWrap.wdFindContinue;
								}
								else if (dic.ContainsKey("hq") && rangeend.Text.Contains(dic["hq"]))
								{
									rangeend.Text = "\r\nHỆ QUẢ.\t";
									rangeend.Font.Bold = 1;
									rangeend.Font.Size = 12;
									rangeend.Font.Color = WdColor.wdColorDarkGreen;
									find.Wrap = WdFindWrap.wdFindContinue;
								}
								else if (dic.ContainsKey("nx") && rangeend.Text.Contains(dic["nx"]))
								{
									rangeend.Text = "\r\nNHẬN XÉT.\t";
									rangeend.Font.Bold = 1;
									rangeend.Font.Size = 12;
									rangeend.Font.Color = WdColor.wdColorRose;
									find.Wrap = WdFindWrap.wdFindContinue;
								}
								else if (dic.ContainsKey("cy") && rangeend.Text.Contains(dic["cy"]))
								{
									rangeend.Text = "\r\nChú ý";
									rangeend.Font.Bold = 1;
									rangeend.Font.Size = 12;
									rangeend.Font.Color = WdColor.wdColorViolet;
									find.Wrap = WdFindWrap.wdFindContinue;
								}
								else
								{
									rangeend.Text = "\r\n";
									find.Wrap = WdFindWrap.wdFindContinue;
								}
							}
							catch
							{
								rangeend.Text = " ";
							}
						}
					}
					catch
					{

					}
				}
				rangeend = document.Content;
				find = rangeend.Find;
				find.Text = @"\\textit\{*\}";
				find.MatchWildcards = true;
				while (find.Execute())
				{
					string texchange = rangeend.Text.Remove(rangeend.Text.Length - 1, 1).Remove(0, 8);
					rangeend.Text = texchange;
					rangeend.Font.Italic = 1;
					find.Wrap = WdFindWrap.wdFindContinue;
				}
				rangeend = document.Content;
				find = rangeend.Find;
				find.Text = @"\\textbf\{*\}";
				find.MatchWildcards = true;
				while (find.Execute())
				{
					string texchange = rangeend.Text.Remove(rangeend.Text.Length - 1, 1).Remove(0, 8);
					rangeend.Text = texchange;
					rangeend.Font.Bold = 1;
					find.Wrap = WdFindWrap.wdFindContinue;
				}
				rangeend = document.Content;
				find = rangeend.Find;
				find.Execute(@"\\end\{*\}", false, false, true, false, false, missing, missing, missing, "^p", WdReplace.wdReplaceAll);
				find = rangeend.Find;
				find.Execute(@"\\hspace\*\{*\}", false, false, true, false, false, missing, missing, missing, "^p", WdReplace.wdReplaceAll);
				find = rangeend.Find;
				find.Execute(@"\\hspace\{*\}", false, false, true, false, false, missing, missing, missing, "", WdReplace.wdReplaceAll);
				find = rangeend.Find;
				find.Execute(@"\[[A-Z0-9a-z]?\]", false, false, true, false, false, missing, missing, missing, "", WdReplace.wdReplaceAll);
				find = rangeend.Find;
				find.Execute(@"\\\begin\{*\}", false, false, true, false, false, missing, missing, missing, "^p", WdReplace.wdReplaceAll);
				find = rangeend.Find;
				find.Execute("{", false, false, false, false, false, missing, missing, missing, "", WdReplace.wdReplaceAll);
				find = rangeend.Find;
				find.Execute("}", false, false, false, false, false, missing, missing, missing, "", WdReplace.wdReplaceAll);
				find = rangeend.Find;
				find.Execute("^13{2,}", false, false, true, false, false, missing, missing, missing, "^p", WdReplace.wdReplaceAll);
				find = rangeend.Find;
				find.Execute("$", false, false, false, false, false, missing, missing, missing, "", WdReplace.wdReplaceAll);
				find = rangeend.Find;
				find.Execute("&", false, false, false, false, false, missing, missing, missing, "", WdReplace.wdReplaceAll);
				find = rangeend.Find;
				find.Replacement.Font.Bold = 1;
				find.Replacement.Font.Color = WdColor.wdColorDarkBlue;
				find.Execute(@"\dapso", false, false, false, false, false, missing, missing, true, "^tĐáp số", WdReplace.wdReplaceAll);
				find = rangeend.Find;
				find.Text = @"[a-z]{1}\).  ";
				find.MatchWildcards = true;
				while (find.Execute())
				{
					rangeend.Font.Bold = 1;
					rangeend.Font.Color = WdColor.wdColorDarkBlue;
					find.Wrap = WdFindWrap.wdFindStop;
				}
				if (addTable == true)
				{
					try
					{
						rangeend = document.Range(document.Content.End - 1);
						rangeend.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
						rangeend.Font.Color = WdColor.wdColorBlack;
						rangeend.Bold = 1;
						rangeend.Font.Color = WdColor.wdColorDarkRed;
						rangeend.Font.Size = 20;
						rangeend.Text = "BẢNG ĐÁP ÁN.\r\n";
						rangeend = document.Range(rangeend.End - 1);
						rangeend.ParagraphFormat.LeftIndent = 0;
						int numbercol = listTableCheck.Count / 10 + 1;
						Microsoft.Office.Interop.Word.Table table = rangeend.Tables.Add(rangeend, numbercol, 10, WdDefaultTableBehavior.wdWord9TableBehavior, WdAutoFitBehavior.wdAutoFitWindow);
						for (int i = 1; i <= numbercol; i++)
							for (int j = 1; j <= 10; j++)
							{
								Row row = table.Rows[i];
								Cell cell = row.Cells[j];
								if ((i + j) % 2 == 1)
								{
									cell.Shading.BackgroundPatternColor = WdColor.wdColorGray20;
								}
								int index = (i - 1) * 10 + j;
								if (index <= listTableCheck.Count)
								{
									cell.Range.Text = index + "." + listTableCheck[index - 1];
									cell.Range.Font.Size = 12;
								}
							}

					}
					catch
					{

					}
				}
				document.Content.Font.Name = "Times New Roman (Headings)";
				document.Content.Font.Size = 12;
				string pathword = path + ".docx";
				document.SaveAs(pathword, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault);
				if (addPdf == true)
				{
					string pathpdf = path + ".pdf";
					document.SaveAs(pathpdf, WdSaveFormat.wdFormatPDF);
				}
				document.Close();
			}
			catch
			{

			}
		}
	}
}
