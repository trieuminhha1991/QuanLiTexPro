using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using Application = Microsoft.Office.Interop.Word.Application;
using QuanLyTex.User5Class;
using Microsoft.VisualBasic.Devices;
using System.Drawing;
using System.IO;
using Microsoft.Office.Core;
using System.Text;

namespace WpfApp1
{
	class WordToTex
	{
		TreatTex treat = new TreatTex();
		public void startWordToTex(string pathFooter, string pathForm, string path, string pathTex, string pathDoc, string pathimage, string loigiai, List<string> listStr, bool? All, bool? fixHe, bool? fixImage, bool? ColorOne, bool? BoldOne, bool? ItalicOne, bool? UnderLineTwo, bool? HghtlightTwo, bool? ColorTwo, bool? ColorThree, bool? RunHide)
		{
			object missing = System.Reflection.Missing.Value;
			var app = new Application();
			app.Visible = true;
			var doc = app.Documents.Open(path);
			if (RunHide == true)
			{
				doc.Application.Visible = false;
			}
			Range range = doc.Content;
			range.ListFormat.ConvertNumbersToText();
			if (doc.Content.Tables.Count >= 1)
			{
				for (int i = 1; i <= doc.Content.Tables.Count; i++)
				{
					Table item = doc.Content.Tables[i];
					item.Delete();
				}
			} 
			int indeximage = 0;
			
			List<int> list = new List<int>();
			list.Add(0);
			//find.Execute("(^13[ ]{1,})", false, false, true, false, false, false, WdFindWrap.wdFindAsk, false, "^p", WdReplace.wdReplaceAll);
			foreach (string item in listStr)
			{
				Find find1 = range.Find;
				find1.Text = "(" + item + ")([ ]{1,})([0-9]{1,3})";
				find1.Execute(Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \3");
				if (ColorOne == true)
				{
					range = doc.Content;
					find1 = range.Find;
					find1.Font.Bold = 1;
					find1.Text = item + " [0-9]{1,3}";
					while (find1.Execute(Wrap: WdFindWrap.wdFindStop, MatchWildcards: true))
					{
						if (range.Font.Color != WdColor.wdColorAutomatic && range.Font.Color != WdColor.wdColorBlack)
						{
							range.Font.Color = WdColor.wdColorDarkBlue;
						}
					}
				}
				range = doc.Content;
				find1 = range.Find;
				find1.Text = item + " [0-9]{1,3}";
				if (ColorOne == true) { find1.Font.Color = WdColor.wdColorDarkBlue; }
				if (BoldOne == true) { find1.Font.Bold = 1; }
				if (ItalicOne == true) { find1.Font.Italic = 1; }
				find1.Execute(Wrap: WdFindWrap.wdFindContinue, MatchWildcards: true, Format: true, Replace:WdReplace.wdReplaceAll, ReplaceWith: @"^92begin{ex}");
			}
			range = doc.Content;
			Find find = range.Find;
			while(find.Execute(FindText:@"\begin{ex}",Wrap:WdFindWrap.wdFindStop))
			{
				list.Add(range.Start);
			}
			list.Add(doc.Content.End);
			list.Sort();
			for (int i = list.Count - 2; i >=0; i--)
			{
				if (i == 0)
				{
					if (list[i] < list[i + 1])
					{
						range = doc.Range(list[i], list[i + 1]);
						range.Select();
						range.Application.Run("Macro");
					}
				}
				else
				{
					Range rangenew = doc.Range(list[i] , list[i + 1]);
					rangenew.Select();
					rangenew.Application.Run("Macro");
					rangenew.InsertAfter("}\r\n\\end{ex}\r\n");
					Find find2 = rangenew.Find;
					find2.Execute(FindText: @"([^t^13])([ ]{1,})", Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1", MatchWildcards: true);
					find2.Replacement.Font.Bold = 1;
					find2.Execute(FindText: @"([^t^13])([ABCD])(.)", Format: true, MatchWildcards: true,Replace:WdReplace.wdReplaceAll,ReplaceWith:@"^p\2.");
					find2 = rangenew.Find;
					find2.Execute(FindText: @"([ABCD])(.)", Format: true, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1.");
					if (ColorThree == true)
					{
						find2 = rangenew.Find;
						find2.Font.Bold = 1;
						find2.Text = "([ABCD])(.)";
						while (find2.Execute(Format: true, Wrap: WdFindWrap.wdFindStop, MatchWildcards: true))
						{
							if (rangenew.Font.Color != WdColor.wdColorAutomatic && rangenew.Font.Color != WdColor.wdColorRed)
							{
								rangenew.Font.Color = WdColor.wdColorDarkBlue;
							}
						}
					}
					find2 = rangenew.Find;
					find2.Text = "(?)";
					find2.Font.Color = WdColor.wdColorAutomatic;
					if (find2.Execute(Format: true, MatchWildcards: true, Replace: WdReplace.wdReplaceOne, ReplaceWith: @"!!!\1"))
					{
						int end = rangenew.Start;
						Range rangenew2 = doc.Range(list[i], end);
						rangenew2.Find.Execute(FindText: ":", ReplaceWith: "", Replace: WdReplace.wdReplaceAll);
						rangenew2.Find.Execute(FindText: ")", ReplaceWith: "]", Replace: WdReplace.wdReplaceAll);
						rangenew2.Find.Execute(FindText: "(", ReplaceWith: "%[", Replace: WdReplace.wdReplaceAll);
					}
					else
					{
						find2 = rangenew.Find;
						find2.Text = "(?)";
						find2.Font.Color = WdColor.wdColorBlack;
						if (find2.Execute(Format: true, MatchWildcards: true, Replace: WdReplace.wdReplaceOne, ReplaceWith: @"!!!\1"))
						{
							int end = rangenew.Start;
							Range rangenew2 = doc.Range(list[i], end);
							rangenew2.Find.Execute(FindText: ":", ReplaceWith: "", Replace: WdReplace.wdReplaceAll);
							rangenew2.Find.Execute(FindText: ")", ReplaceWith: "]", Replace: WdReplace.wdReplaceAll);
							rangenew2.Find.Execute(FindText: "(", ReplaceWith: "%[", Replace: WdReplace.wdReplaceAll);
						}
					}
				}
				
			}
			
			//range = doc.Content;
			//find = range.Find;
			//find.Text = "^g";
			//int indeximage = 0;
			//while (find.Execute(Format: true,Wrap: WdFindWrap.wdFindContinue))
			//{
			//	string pathimagesub = pathimage + @"\img" + indeximage + ".png";
			//	range.Copy();
			//	Computer computer = new Computer();
			//	System.Windows.Forms.IDataObject data = computer.Clipboard.GetDataObject();
			//	if (data.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap))
			//	{
			//		Image image = (Image)data.GetData(System.Windows.Forms.DataFormats.Bitmap, true);
			//		image.Save(pathimagesub, System.Drawing.Imaging.ImageFormat.Png);
			//		indeximage++;
			//	}
			//	range.Text = @"\r\n\begin{center}
			//					 \includegraphics{"+ pathimagesub + @"}
			//					\end{center}\r\n";
			//	indeximage++;
			//}
			range = doc.Content;
			find = range.Find;
			if (ColorTwo == true)
			{
				find.Font.Color = WdColor.wdColorRed;
			}
			if (UnderLineTwo == true)
			{
				find.Font.Underline = WdUnderline.wdUnderlineSingle;
			}
			if (HghtlightTwo == true)
			{
				find.Highlight = 1;
			}
			find.Text = "([ABCD].)";
			find.MatchWildcards = true;
			find.Font.Bold = 1;
			find.Replacement.Font.Color = WdColor.wdColorDarkBlue;
			find.Execute(ReplaceWith: @"\1^92True", Replace: WdReplace.wdReplaceAll,Format:true);
			range = doc.Content;
			find = range.Find;
			find.Text = "^13[A].";
			find.MatchWildcards = true;
			find.Font.Bold = 1;
			if (ColorThree == true)
			{
				find.Font.Color = WdColor.wdColorDarkBlue;
			}
			find.Execute(ReplaceWith: @"!!!^92choice!!!{", Replace: WdReplace.wdReplaceAll, Format: true);
			range = doc.Content;
			find = range.Find;
			find.Text = @"^13[BCD].";
			find.MatchWildcards = true;
			find.Font.Bold = 1;
			if (ColorThree == true)
			{
				find.Font.Color = WdColor.wdColorDarkBlue;
			}
			find.Execute(ReplaceWith: @"}!!!{", Replace: WdReplace.wdReplaceAll, Format: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"^13{1,}", ReplaceWith: @"^p", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"([^t^13])\}", ReplaceWith: @"}", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"^p", ReplaceWith: @"\\^p", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"!!!", ReplaceWith: @"^p", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: "^p" + loigiai, ReplaceWith: @"}^p\loigiai{^p", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"(\{{2,})([A-Za-z0-9 ]{1,10})(\}{2,})", ReplaceWith: @"\2", Replace: WdReplace.wdReplaceAll, MatchWildcards:true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"(\{{2})([A-Za-z0-9 ]{1,10})(\})([', ])(\})([', ])", ReplaceWith: @"\2\4\6", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\frac", ReplaceWith: @"\dfrac", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"{.", ReplaceWith: @"{", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\{[ ]{1,}.", ReplaceWith: @"{", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @".[ ]{1,}\}", ReplaceWith: @".}", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"[.]{1,}\}", ReplaceWith: @"}", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\[", ReplaceWith: @"$", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\]", ReplaceWith: @"$", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\leftrightarrow", ReplaceWith: @"\Leftrightarrow", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"begin{align}", ReplaceWith: @"begin{aligned}", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"begin{matrix}", ReplaceWith: @"begin{aligned}", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"begin{aligned}\\", ReplaceWith: @"begin{aligned}", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"end{align}", ReplaceWith: @"end{aligned}", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"end{matrix}", ReplaceWith: @"end{aligned}", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\\^p\end{aligned}", ReplaceWith: @"\end{aligned}", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"(\{{2})([A-Za-z0-9]{1,})(\})(?)(\{)([A-Za-z0-9])(\}{2})", ReplaceWith: @"\2\4\6", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"(\{{2})([A-Za-z0-9]{1,})(\})(?)(\{)([A-Za-z0-9]{2,})(\}{2})", ReplaceWith: @"\2\4{\6}", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"$$", ReplaceWith: @"", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"(\\underset\{)(*)(\}\{\\mathop\{)(*)([ ]{1,}\}\})", ReplaceWith: @"\4^92limits_{\2}", Replace: WdReplace.wdReplaceAll,MatchWildcards:true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\to", ReplaceWith: @"\rightarrow", Replace: WdReplace.wdReplaceAll);
			if (fixHe == true)
			{
				range = doc.Content;
				find = range.Find;
				find.Execute(FindText: @"\left\{ \begin{aligned}", ReplaceWith: @"\heva{", Replace: WdReplace.wdReplaceAll);
				find.Execute(FindText: @"\left[ \begin{aligned}", ReplaceWith: @"\hoac{", Replace: WdReplace.wdReplaceAll);
				find.Execute(FindText: @"\end{aligned} \right.", ReplaceWith: @"}", Replace: WdReplace.wdReplaceAll);
			}
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"$\begin{aligned}", ReplaceWith: @"\begin{align*}^p", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\end{aligned}$", ReplaceWith: @"^p\end{align*}", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"^13{1,}", ReplaceWith: @"^p", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\\backslash", ReplaceWith: @"^92", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"^13{1,}", ReplaceWith: @"^p", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\\\\", ReplaceWith: @"\\", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"([\\]{2})([ ]{1,})([\\]{2})", ReplaceWith: @"^92^92", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\\}", ReplaceWith: @"}", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"([\\]{2})(^13)(\\end\{*\})([\\]{2})", ReplaceWith: @"^p\3", Replace: WdReplace.wdReplaceAll, MatchWildcards: true); range = doc.Content;
			foreach (Microsoft.Office.Interop.Word.Shape item in doc.Shapes)
			{
				try
				{
					if (item.Type == MsoShapeType.msoPicture)
					{
						InlineShape s = item.ConvertToInlineShape();
						string pathimagesub = pathimage + @"\img" + indeximage + ".png";
						range = s.Range;
						range.Copy();
						Computer computer = new Computer();
						System.Windows.Forms.IDataObject data = computer.Clipboard.GetDataObject();
						if (data.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap))
						{
							Image image = (Image)data.GetData(System.Windows.Forms.DataFormats.Bitmap, true);
							image.Save(pathimagesub, System.Drawing.Imaging.ImageFormat.Png);
							indeximage++;
						}
						range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
						range.Text = "\begin{center}\r\n\\includegraphics{" + pathimagesub.Replace(@"\", "/") + "}\r\n\\end{center}";
					}
					else
					{ item.Delete(); }
				}
				catch { }
			}
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\loigiai{^p.\\^p^p", ReplaceWith: @"\loigiai^p", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\end{center}\\", ReplaceWith: @"\end{center}", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"^p\\^p", ReplaceWith: @"^p", Replace: WdReplace.wdReplaceAll);
			doc.Content.Font.Name= "Times New Roman (Headings)";
			string textHeader = File.ReadAllText(pathForm);
			string Footer = File.ReadAllText(pathFooter);
			string text = textHeader+doc.Content.Text+ Footer;
			File.AppendAllText(pathTex, text);
			doc.SaveAs(pathDoc, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault);
			doc.Close(SaveChanges: false); ;
			app.Quit();
			//doc.Close(SaveChanges: false);
			//string texAll = range.Text;
			//List<string> listText = treat.FilterId(texAll, All, ex, baitap, vidu);
			//foreach (string item in listText)
			//{
			//	string texnew = treat.startTreatTex(item, fixHe);
			//	texnew = treat.startAllTex(texnew);
			//	File.AppendAllText(pathTex, item);
			//}
		}
	}
}

