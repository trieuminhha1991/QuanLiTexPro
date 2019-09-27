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
		public void startWordToTex(Application app,string pathFooter, string pathForm, string path, string pathTex, string pathDoc, string loigiai, List<string> listStr, bool? All, bool? fixHe, bool? ColorOne, bool? BoldOne, bool? ItalicOne, bool? UnderLineTwo, bool? HghtlightTwo, bool? ColorTwo, bool? ColorThree, bool? RunHide)
		{
			object missing = System.Reflection.Missing.Value;
			var docOld1 = app.Documents.Open(path,ReadOnly:true);
			Document docOld = app.Documents.Add();
			Document doc= app.Documents.Add();
			docOld.Content.FormattedText = docOld1.Content.FormattedText;
			docOld1.Close();
			if (RunHide == true)
			{
				docOld.Application.Visible = false;
				doc.Application.Visible = false;
			}
			Range range = docOld.Content;
			range.ListFormat.ConvertNumbersToText();
			if (docOld.Content.Tables.Count >= 1)
			{
				for (int i = 1; i <= docOld.Content.Tables.Count; i++)
				{
					Table item = docOld.Content.Tables[i];
					item.Delete();
				}
			} 
			int indeximage = 0;
			
			List<int> list = new List<int>();
			list.Add(0);
			//find.Execute("(^13[ ]{1,})", false, false, true, false, false, false, WdFindWrap.wdFindAsk, false, "^p", WdReplace.wdReplaceAll);
			foreach (string item in listStr)
			{
				range = docOld.Content;
				Find find1 = range.Find;
				find1.Execute(FindText: "(" + item + ")([ ]{1,})([0-9]{1,3})",Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \3");
				find1.Execute(FindText: "(" + item + ")([0-9]{1,3})", Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \2");
				if (ColorOne == true)
				{
					range = docOld.Content;
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
				range = docOld.Content;
				find1 = range.Find;
				find1.Text = item + " [0-9]{1,3}";
				if (ColorOne == true) { find1.Font.Color = WdColor.wdColorDarkBlue; }
				if (BoldOne == true) { find1.Font.Bold = 1; }
				if (ItalicOne == true) { find1.Font.Italic = 1; }
				find1.Execute(Wrap: WdFindWrap.wdFindContinue, MatchWildcards: true, Format: true, Replace:WdReplace.wdReplaceAll, ReplaceWith: @"^92begin{ex}");
			}
			range = docOld.Content;
			Find find = range.Find;
			while(find.Execute(FindText:@"\begin{ex}",Wrap:WdFindWrap.wdFindStop))
			{
				list.Add(range.Start);
			}
			list.Add(docOld.Content.End);
			list.Sort();
			for (int i =0; i< list.Count-1; i++)
			{
				try
				{
					if (i == 0)
					{
						if (All == true)
						{
							if (list[i] < list[i + 1])
							{
								Range rangeOld = docOld.Range(list[i], list[i + 1]);
								range = doc.Range(docOld.Content.End - 1);
								range.FormattedText = rangeOld.FormattedText;
								range.Select();
								range.Application.Run("Macro");
							}
						}
					}
					else
					{
						Range rangeOld = docOld.Range(list[i], list[i + 1]);
						Range rangenew = doc.Range(doc.Content.End - 1);
						rangenew.FormattedText = rangeOld.FormattedText;
						rangenew.Select();
						rangenew.Application.Run("Macro");
						rangenew.InsertAfter("}\r\n\\end{ex}\r\n");
						Find find2 = rangenew.Find;
						find2.Execute(FindText: @"([^t^13])([ ]{1,})", Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1", MatchWildcards: true);
						find2 = rangenew.Find;
						find2.Replacement.Font.Bold = 1;
						find2.Execute(FindText: @"([^t^13])([ABCD])(.)", Format: true, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"^p\2.");
						find2 = rangenew.Find;
						find2.Replacement.Font.Bold = 1;
						find2.Execute(FindText: @"([^t^13])([ABCD])([ ]{1,})(.)", Format: true, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"^p\2.");
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
					}
				}
				catch { }
			}
			docOld.Close(SaveChanges:false);
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
			find.Execute(FindText: @"([\\]{2})(^13)(\\end\{*\})([\\]{2})", ReplaceWith: @"^p\3", Replace: WdReplace.wdReplaceAll, MatchWildcards: true);
			foreach (Microsoft.Office.Interop.Word.InlineShape item in doc.InlineShapes)
			{
				try
				{
					range = item.Range;
					item.Select();
					range.Text = "\r\nNơi có hình\r\n";
				}
				catch{ }
			}
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\loigiai{^p.\\^p^p", ReplaceWith: @"\loigiai{^p", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\loigiai{^p.\\", ReplaceWith: @"\loigiai{^p", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"\end{center}\\", ReplaceWith: @"\end{center}", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"^p\\^p", ReplaceWith: @"^p", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"^13{1,}", ReplaceWith: @"^p", Replace: WdReplace.wdReplaceAll);
			range = doc.Content;
			find = range.Find;
			find.Execute(FindText: @"Nơi có hình}", ReplaceWith: @"Nơi có hình", Replace: WdReplace.wdReplaceAll);
			doc.Content.Font.Name= "Times New Roman (Headings)";
			string textHeader = File.ReadAllText(pathForm);
			string Footer = File.ReadAllText(pathFooter);
			string text = textHeader+doc.Content.Text+ Footer;
			File.AppendAllText(pathTex, text);
			doc.SaveAs(pathDoc, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault);
			doc.Close(); ;
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

