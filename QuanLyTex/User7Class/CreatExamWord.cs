using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using Application = Microsoft.Office.Interop.Word.Application;


namespace QuanLyTex.User7Class
{
    class CreatExamWord
    {
		public int[] GenRandomPermutation(int n)
		{
			Random r = new Random();
			int[] a = new int[n];
			for (int i = 0; i < n; i++)
			{
				a[i] = i;
			}
			for (int i = 0; i < n; i++)
			{
				int j = r.Next(n);
				int t = a[0];
				a[0] = a[j];
				a[j] = t;
			}
			return a;
		}

		public void CreatExam(string numberexer,string number,string path,string pathSave,bool? DevideLevel,bool? Form,bool? Matrix,bool? Header, string ExString,string ProofStringbool,int Id,int LocationId,bool? ColorOne,bool? BoldOne,bool? ItalicOne,bool? ColorThree,bool? UnderLineTwo,bool? ColorTwo,bool? HightlightTwo)
		{
			int numberexerint = int.Parse(numberexer);
			string checkId = "";
			string indexNoId = "";
			if (Id == 5)
			{
				checkId = "[0-9][DH][1-9][YBKGT][0-9]";
				if (LocationId == 1)
				{
					checkId = @"\[" + checkId + @"\]";
				}
				if (LocationId == 2)
				{
					checkId = @"\(" + checkId + @"\)";
				}
			}
			if (Id == 6)
			{
				checkId = @"[0-9][DH][1-9][YBKGT][0-9]\-[0-9]";
				if (LocationId == 1)
				{
					checkId = @"\[" + checkId + @"\]";
				}
				if (LocationId == 2)
				{
					checkId = @"\(" + checkId + @"\)";
				}
			}
			var app = new Application();
			app.Visible = true;
			Document doc = app.Documents.Open(path);
			Range range = doc.Content;
			range.ListFormat.ConvertNumbersToText();
			List<int> listex = new List<int>();
			List<dynamic> list = new List<dynamic>();
			List<string> listId = new List<string>();
			List<dynamic> listY = new List<dynamic>();
			List<dynamic> listB = new List<dynamic>();
			List<dynamic> listK = new List<dynamic>();
			List<dynamic> listG = new List<dynamic>();
			range = doc.Content;
			Microsoft.Office.Interop.Word.Find find = range.Find;
			if (ColorOne == true) { find.Font.Color = WdColor.wdColorBlue; }
			if (BoldOne == true) { find.Font.Bold = 1; }
			if (ItalicOne == true) { find.Font.Italic = 1; }
			find.Text = ExString + "[ ]{1,}[0-9]{1,2}";
			find.MatchWildcards = true;
			while (find.Execute(Wrap: WdFindWrap.wdFindStop))
			{
				listex.Add(range.Start);
			}
			listex.Add(doc.Content.End);
			if (listex.Count - 1 != numberexerint)
			{
				System.Windows.MessageBox.Show("Số lượng câu trong file chưa đủ", "Thoát");
			}
			else
			{
				for (int i = 0; i < listex.Count - 2; i++)
				{
					Dictionary<string, dynamic> dic = new Dictionary<string, dynamic>();
					dic.Add("start", listex[i]);
					dic.Add("end", listex[i + 1] - 1);
					range = doc.Range(listex[i], listex[i + 1] - 1);
					find = range.Find;
					find.MatchWildcards = true;
					if (ColorTwo == true)
					{
						find.Font.Color = WdColor.wdColorRed;
					}
					if (UnderLineTwo == true)
					{
						find.Font.Underline = WdUnderline.wdUnderlineSingle;
					}
					if (HightlightTwo == true)
					{
						find.Highlight = 1;
					}
					find.Text = "([ABCD])";
					if (find.Execute(Wrap: WdFindWrap.wdFindStop))
					{
						range.Font.Color = WdColor.wdColorDarkBlue;
						dic.Add("true", range.Text);
					}
					else
					{
						dic.Add("true", "");
					}
					if (checkId != "")
					{
						range = doc.Range(listex[i], listex[i + 1] - 1);
						find = range.Find;
						find.MatchWildcards = true;
						find.Text = checkId;
						if (find.Execute())
						{
							string stringId = range.Text;
							listId.Add(stringId);
							switch (stringId[3])
							{
								case 'Y':
									listY.Add(dic);
									break;
								case 'B':
									listB.Add(dic);
									break;
								case 'K':
									listK.Add(dic);
									break;
								case 'G':
									listG.Add(dic);
									break;
								case 'T':
									listG.Add(dic);
									break;
							}
						}
						else
						{
							indexNoId += i + "";
						}
					}
					else
					{
						list.Add(dic);
					}
				}
				if (checkId == "")
				{
					int numberExer = int.Parse(number);
					for (int i = 0; i < numberExer; i++)
					{
						CreatOneExamBasic(i, list, pathSave, doc, Form, Header, ProofStringbool, ColorThree, app);
					}
				}
				if (indexNoId != "")
				{
					System.Windows.MessageBox.Show("Các câu thứ " + indexNoId + " Chưa thiết lập Id, xin hãy thiết lập đầy đủ Id", "Thoát");
				}
				else
				{
					int numberExer = int.Parse(number);
					for (int i = 0; i < numberExer; i++)
					{
						CreatOneExamBAdvande(i, listY, listB, listK, listG, pathSave, doc, Form, Header, ProofStringbool, ColorThree, app);
						if(Matrix==true)
						{
							CreatMatrix(listId,app, pathSave);
						}
					}
				}
			}
			//app.Quit();
		}
		public void CreatMatrix(List<string> list,Application app,string pathSave)
		{
			Document doc = app.Documents.Add();
			
		}
		public void CreatOneExamBasic(int indexDe, List<dynamic> list,string pathSave, Document doc, bool? Form, bool? Header, string ProofStringbool, bool? ColorThree,Application app)
		{
			Document docnew = app.Documents.Add();
			docnew.Content.Font.Name = "Times New Roman (Headings)";
			docnew.Content.Font.Size = 12;
			Document docproof = app.Documents.Add();
			docproof.Content.Font.Name = "Times New Roman (Headings)";
			docproof.Content.Font.Size = 12;
			ListTemplate template = docproof.ListTemplates.Add(true, "template1");
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
			List<string> listtrue = new List<string>();
			int[] a = GenRandomPermutation(list.Count);
			for(int i=0;i<a.Length; i++)
			{
				itemCreatExam(list, i, a, listtrue, doc, docnew, docproof, template, ProofStringbool, Form, ColorThree);
			}
			Range rangeend = docproof.Range(docproof.Content.End - 1);
			rangeend.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
			rangeend.Font.Color = WdColor.wdColorBlack;
			rangeend.Bold = 1;
			rangeend.Font.Color = WdColor.wdColorDarkRed;
			rangeend.Font.Size = 20;
			rangeend.Text = "BẢNG ĐÁP ÁN.\r\n";
			rangeend = docproof.Range(rangeend.End - 1);
			rangeend.ParagraphFormat.LeftIndent = 0;
			int numbercol = listtrue.Count / 10 + 1;
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
					if (index <= listtrue.Count)
					{
						cell.Range.Text = index + "." + listtrue[index - 1];
						cell.Range.Font.Size = 12;
					}
				}
			string pathProof = pathSave + @"\Loigiai_De" + indexDe + ".docx";
			string pathnew = pathSave + @"\De" + indexDe + ".docx";
			docnew.SaveAs2(pathnew, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault);
			docproof.SaveAs2(pathProof, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault);
		}
		public void CreatOneExamBAdvande(int indexDe, List<dynamic> listY, List<dynamic> listB, List<dynamic> listK, List<dynamic> listG, string pathSave, Document doc, bool? Form, bool? Header, string ProofStringbool, bool? ColorThree, Application app)
		{

			Document docnew = app.Documents.Add();
			docnew.Content.Font.Name = "Times New Roman (Headings)";
			docnew.Content.Font.Size = 12;
			Document docproof = app.Documents.Add();
			docproof.Content.Font.Name = "Times New Roman (Headings)";
			docproof.Content.Font.Size = 12;
			ListTemplate template = docproof.ListTemplates.Add(true, "template1");
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
			List<string> listtrue = new List<string>();
			int[] a = GenRandomPermutation(listY.Count);
			for (int i = 0; i < a.Length; i++)
			{
				itemCreatExam(listY, i, a, listtrue, doc, docnew, docproof, template, ProofStringbool, Form, ColorThree);
			}
			a = GenRandomPermutation(listB.Count);
			for (int i = 0; i < a.Length; i++)
			{
				itemCreatExam(listB, i, a, listtrue, doc, docnew, docproof, template, ProofStringbool, Form, ColorThree);
			}
			a = GenRandomPermutation(listK.Count);
			for (int i = 0; i < a.Length; i++)
			{
				itemCreatExam(listK, i, a, listtrue, doc, docnew, docproof, template, ProofStringbool, Form, ColorThree);
			}
			a = GenRandomPermutation(listG.Count);
			for (int i = 0; i < a.Length; i++)
			{
				itemCreatExam(listG, i, a, listtrue, doc, docnew, docproof, template, ProofStringbool, Form, ColorThree);
			}
			Range rangeend = docproof.Range(docproof.Content.End - 1);
			rangeend.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
			rangeend.Font.Color = WdColor.wdColorBlack;
			rangeend.Bold = 1;
			rangeend.Font.Color = WdColor.wdColorDarkRed;
			rangeend.Font.Size = 20;
			rangeend.Text = "BẢNG ĐÁP ÁN.\r\n";
			rangeend = docproof.Range(rangeend.End - 1);
			rangeend.ParagraphFormat.LeftIndent = 0;
			int numbercol = listtrue.Count / 10 + 1;
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
					if (index <= listtrue.Count)
					{
						cell.Range.Text = index + "." + listtrue[index - 1];
						cell.Range.Font.Size = 12;
					}
				}
			string pathProof = pathSave + @"\Loigiai_De" + indexDe + ".docx";
			string pathnew = pathSave + @"\De" + indexDe + ".docx";
			docnew.SaveAs2(pathnew, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault);
			docproof.SaveAs2(pathProof, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault);
		}
		public void itemCreatExam(List<dynamic> list, int i, int[] a, List<string> listtrue, Document doc, Document docnew, Document docproof, ListTemplate template, string ProofStringbool, bool? Form, bool? ColorThree)
		{
			Dictionary<string, dynamic> dic = list[a[i]];
			int start = dic["start"];
			int end = dic["end"];
			string truestr = dic["true"];
			if (Form == false)
			{
				listtrue.Add(truestr);
			}
			Range range = doc.Range(start, end);
			int start2 = docnew.Content.End - 1;
			Range rangenew = docnew.Range(docnew.Content.End - 1);
			rangenew.FormattedText=range.FormattedText;
			int end2 = rangenew.End - 1;
			Find find = rangenew.Find;
			find.Text = ProofStringbool;
			if (find.Execute())
			{
				Range rangeFind = docnew.Range(rangenew.Start, docnew.Content.End - 1);
				Range rangeProof = docproof.Range(docproof.Content.End - 1);
				rangeProof.ListFormat.ApplyListTemplateWithLevel(template, true, WdListApplyTo.wdListApplyToSelection, 1);
				rangeProof.Text = "Lời giải câu" + i;
				rangeProof.InsertParagraph();
				docproof.Range(docproof.Content.End - 1).FormattedText= rangeFind.FormattedText;
				rangeFind.Delete();
			}
			if (Form == true)
			{
				List<string> listint = new List<string>() { "A", "B", "C", "D" };
				rangenew = docnew.Range(start2, docnew.Content.End - 1);
				find = rangenew.Find;
				find.MatchWildcards = true;
				find.Execute(FindText: @"([^t^13])([ ]{1,})", Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1");
				rangenew = docnew.Range(start2, docnew.Content.End - 1);
				find = rangenew.Find;
				find.MatchWildcards = true;
				find.Text = "(^13A.)(*)([^t^13])(B.)(*)([^t^13])(C.)(*)([^t^13])(D.)(*)([^t^13])";
				while (find.Execute(Wrap: WdFindWrap.wdFindContinue, Format: true))
				{
					Range rangeCheck = doc.Range(rangenew.Start + 1, rangenew.Start + 2);
					bool check = false;
					if (rangeCheck.Font.Bold == 1)
					{
						check = true;
					}
					if (ColorThree == true)
					{
						check = false;
						if (rangeCheck.Font.Color == WdColor.wdColorDarkBlue)
						{
							check = true;
						}
					}
					if (check == true)
					{
						string chechstring;
						int[] b = GenRandomPermutation(4);
						int[] c = new int[4];
						for (int j = 0; j < b.Length; j++)
						{
							if (b[j] == 0)
							{
								chechstring = "A";
								if (chechstring == truestr)
								{
									truestr = listint[j];
								}
							}
							if (b[j] == 1)
							{
								chechstring = "B";
								if (chechstring == truestr)
								{
									truestr = listint[j];
								}
							}
							if (b[j] == 2)
							{
								chechstring = "C";
								if (chechstring == truestr)
								{
									truestr = listint[j];
								}
							}
							if (b[j] == 3)
							{
								chechstring = "D";
								if (chechstring == truestr)
								{
									truestr = listint[j];
								}
							}
							c[j] = b[j] * 3 + 2;
						}
						listtrue.Add(truestr);
						find.Replacement.Text = @"\1\" + c[0] + @"\3\4\" + c[1] + @"\6\7\" + c[2] + @"\9\10\" + c[3] + @"\12";
						break;
					}
				}
				rangenew = docnew.Range(start2, docnew.Content.End - 1);
				find = rangenew.Find;
				find.MatchWildcards = true;
				find.Text = "[^t^13]" + truestr + ".";
				find.Replacement.Font.Color = WdColor.wdColorRed;
				find.Execute(Replace: WdReplace.wdReplaceAll, Format: true);
			}
		}
	}
}
