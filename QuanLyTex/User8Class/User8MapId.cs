using Microsoft.Office.Interop.Word;
using QuanLyTex.User1Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application = Microsoft.Office.Interop.Word.Application;

namespace QuanLyTex.User8Class
{
	class datalist
	{
		int start;
		int end;
		public int Start { get => start; set => start = value; }
		public int End { get => end; set => end = value; }
	}
	class User8MapId
	{
		private List<int> listindexall = new List<int>();
		private List<string> liststrall = new List<string>();
		public void FilterId(Application app,string rx, string path, string Type, Document doc, bool? color, bool? bold, bool? italic,bool? Hide)
		{
			try
			{
				List<dynamic> list = new List<dynamic>();
				List<int> listindex = new List<int>();
				Document docOld = app.Documents.Open(FileName:path,Visible: !Hide,ReadOnly:true);
				Document doc1 = app.Documents.Add(Visible: !Hide);
				doc1.Content.FormattedText = docOld.Content.FormattedText;
				docOld.Close();
				Range range = doc1.Content;
				range.ListFormat.ConvertNumbersToText();
				range.Font.Underline = WdUnderline.wdUnderlineNone;
				Find find = range.Find;
				find.Execute(FindText: "(" + Type + ")([ ]{1,})([0-9]{1,3})", Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \3");
				find = range.Find;
				find.Execute(FindText: "(" + Type + ")([0-9]{1,3})", Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \2");
				if(color==true)
				{
					range = doc1.Content;
					find = range.Find;
					find.Text = Type + @" [0-9]{1,3}";
					find.Font.Bold = 1;
					find.MatchWildcards = true;
					find.Wrap = WdFindWrap.wdFindStop;
					while (find.Execute(Format: true))
					{
						if (range.Font.Color != WdColor.wdColorBlack)
						{
							range.Font.Color = WdColor.wdColorDarkBlue;
						}
					}
				}
				range = doc1.Content;
				find = range.Find;
				find.Text = Type + @" [0-9]{1,3}";
				find.Font.Bold = 1;
				if (color == true) { find.Font.Color = WdColor.wdColorDarkBlue; }
				if (italic == true) { find.Font.Italic = 1; }
				find.MatchWildcards = true;
				find.Wrap = WdFindWrap.wdFindStop;
				while (find.Execute(Format:true))
				{
						listindex.Add(range.Start);
				}
				listindex.Add(doc1.Content.End);
				for (int i = 0; i <= listindex.Count - 2; i++)
				{
					try
					{
						Range rangenew = doc1.Range(listindex[i], listindex[i + 1]);
						if(i== listindex.Count - 2)
						{
							if(rangenew.Tables.Count>=1)
							{
								for (int j = 1; i <= rangenew.Tables.Count; i++)
								{
									Table item = rangenew.Tables[j];
									item.Delete();
								}
							}
						}
						Find findnew = rangenew.Find;
						findnew.Text = rx;
						findnew.MatchWildcards = true;
						if (findnew.Execute())
						{
							Range range1=doc1.Range(listindex[i], listindex[i + 1]);
							listindexall.Add(doc.Content.End - 1);
							liststrall.Add(rangenew.Text);
							Range rangenew2 = doc.Range(doc.Content.End - 1);
							rangenew2.FormattedText= range1.FormattedText;
						}
					}
					catch
					{

					}
				}
				doc1.Close(SaveChanges:WdSaveOptions.wdDoNotSaveChanges);
			}
			catch (Exception e)
			{
			}
		}
		public int mapId(Document doc,bool?Hide,List<string> list,Application app,string rx, string type,bool? color,bool? bold, bool? italic)
		{
			try
			{
				listindexall = new List<int>();
				liststrall = new List<string>();
				foreach (string str4 in list)
				{
					FilterId(app,rx, str4, type,doc,color,bold,italic,Hide);
				}
				return listindexall.Count-1;
			}
			catch (Exception e)
			{
				return 0;
			}
		}
		public void mapSort(Document doc, bool? Hide,Application app, string type,bool? blsort, bool? bl1, bool? bl2, bool? bl3, bool? bl4,bool? bl5, string path, string rx, bool? color, bool? bold, bool? italic,bool? BankEcer,bool? Id6,bool? BTNid)
		{
			try
			{
				object missing = System.Reflection.Missing.Value;
				List<SortId> listsort = new List<SortId>();
				Dictionary<string, dynamic> Dic = new Dictionary<string, dynamic>();
				listindexall.Add(doc.Content.End);
				for (int i = 0; i <= listindexall.Count - 2; i++)
				{
					try
					{
						string check = liststrall[i];
						datalist data = new datalist();
						data.Start = listindexall[i];
						data.End = listindexall[i+1];
						check = check.Replace("[", "").Replace("]", "");
						if (Dic.ContainsKey(check))
						{
							Dic[check].Add(data);
						}
						else
						{
							List<datalist> list = new List<datalist>();
							list.Add(data);
							Dic.Add(check, list);
							SortId sort = new SortId();
							sort.ClassId = int.Parse(check[0].ToString());
							if (check[1] == 'D') { sort.ObjectId = 1; } else { sort.ObjectId = 2; }
							sort.CharterId = int.Parse(check[2].ToString());
							if (BTNid == false)
							{
								switch (check[3])
								{
									case 'Y':
										sort.LevelId = 1;
										break;
									case 'B':
										sort.LevelId = 2;
										break;
									case 'K':
										sort.LevelId = 3;
										break;
									case 'G':
										sort.LevelId = 5;
										break;
									case 'T':
										sort.LevelId = 4;
										break;
								}
								sort.SectionId = int.Parse(check[4].ToString());
							}
							else
							{
								sort.SectionId = int.Parse(check[4].ToString());
								sort.SectionId = int.Parse(check[check.Length - 1].ToString());
							}
							sort.CodeId = check;
							listsort.Add(sort);
						}
					}
					catch
					{

					}
				}
				if (blsort == true)
				{
					if (bl1 == true)
					{
						listsort = listsort.OrderBy(m => m.ClassId).ThenBy(m => m.ObjectId).ThenBy(m => m.CharterId).ThenBy(m => m.SectionId).ThenBy(m => m.LevelId).ToList<SortId>();
					}
					if (bl2 == true)
					{
						listsort = listsort.OrderBy(m => m.ObjectId).ThenBy(m => m.ClassId).ThenBy(m => m.CharterId).ThenBy(m => m.SectionId).ThenBy(m => m.LevelId).ToList<SortId>();
					}
					if (bl3 == true)
					{
						listsort = listsort.OrderBy(m => m.ClassId).ThenBy(m => m.ObjectId).ThenBy(m => m.CharterId).ThenBy(m => m.LevelId).ThenBy(m => m.SectionId).ToList<SortId>();
					}
					if (bl4 == true)
					{
						listsort = listsort.OrderBy(m => m.LevelId).ThenBy(m => m.ClassId).ThenBy(m => m.ObjectId).ThenBy(m => m.CharterId).ThenBy(m => m.SectionId).ToList<SortId>();
					}
					if (bl5 == true)
					{
						foreach (SortId item in listsort)
						{
							try
							{
								Document docnew = app.Documents.Add(Visible:!Hide);
								string codeId = item.CodeId;
								List<datalist> list = Dic[codeId];
								DateTime time = DateTime.Now;
								string TimeName = time.ToString("h.mm.ss");
								string path2 = Directory.GetCurrentDirectory() + @"\LuuFile" + @"\[" + codeId + "][" + TimeName + "].docx";
								foreach (datalist data in list)
								{
									try
									{
										Range range1 = doc.Range(data.Start, data.End);
										Range rangenew2 = docnew.Range(docnew.Content.End - 1);
										rangenew2.FormattedText=range1.FormattedText;
									}
									catch { }
								}
								ListTemplate template = docnew.ListTemplates.Add(true, "template1");
								ListLevel level = template.ListLevels[1];
								level.NumberFormat = "Câu %1.";
								level.NumberStyle = WdListNumberStyle.wdListNumberStyleArabic;
								level.NumberPosition = 0;
								level.TextPosition = 60;
								level.StartAt = 1;
								level.Font.Bold = 1;
								level.Font.Color = WdColor.wdColorDarkBlue;
								level.Font.Italic = 1;
								level.Font.Underline = WdUnderline.wdUnderlineDouble;
								level.Font.Size = 12;
								Range range = docnew.Content;
								Find find = range.Find;
								find.Font.Bold = 1;
								if (color == true) { find.Font.Color = WdColor.wdColorDarkBlue; }
								if (italic == true) { find.Font.Italic = 1; }
								while (find.Execute(Wrap: WdFindWrap.wdFindContinue, FindText: type + @" [0-9]{1,3}", MatchWildcards: true))
								{
									range.ListFormat.ApplyListTemplateWithLevel(template, true, WdListApplyTo.wdListApplyToSelection, missing, 1);
									range.Text = " ";
								}
								docnew.Content.Font.Name = "Times New Roman (Headings)";
								docnew.SaveAs(path2, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault);
								docnew.Close();
							}
							catch { }
						}
					}
					else
					{
						int indexconten = doc.Content.End - 1;
						foreach (SortId item in listsort)
						{
							string codeId = item.CodeId;
							List<datalist> list = Dic[codeId];
							foreach (datalist data in list)
							{
								try
								{
									Range range = doc.Range(data.Start, data.End);
									Range range2 = doc.Range(doc.Content.End-1);
									range2.FormattedText = range.FormattedText;
								}
								catch { }
							}
						}
						Range rangecontend = doc.Range(0, indexconten);
						rangecontend.Delete();
					}
				}
				if (BankEcer == true)
				{
					try
					{
						foreach (SortId item in listsort)
						{
							string codeId = item.CodeId;
							List<datalist> list = Dic[codeId];
							string id = "Id5ex";
							if (Id6 == true) { id = "Id6ex"; }
							string pathcodeId = Directory.GetCurrentDirectory() + @"\NganHangWord\"+ id+@"\" + item.ClassId + @"\" + codeId + ".docx";
							if (!File.Exists(pathcodeId))
							{
								Document doccodenew = app.Documents.Add(Visible:!Hide);
								ListTemplate template = doccodenew.ListTemplates.Add(true, "template1");
								ListLevel level = template.ListLevels[1];
								level.NumberFormat = "Câu %1.";
								level.NumberStyle = WdListNumberStyle.wdListNumberStyleArabic;
								level.NumberPosition = 0;
								level.TextPosition = 60;
								level.StartAt = 1;
								level.Font.Bold = 1;
								level.Font.Color = WdColor.wdColorDarkBlue;
								level.Font.Italic = 1;
								level.Font.Underline = WdUnderline.wdUnderlineDouble;
								level.Font.Size = 12;
								doccodenew.SaveAs(pathcodeId, WdSaveFormat.wdFormatDocumentDefault);
								doccodenew.Close();
							}
							Document doccode = app.Documents.Open(FileName:pathcodeId, Visible: !Hide);
							foreach (datalist data in list)
							{
								try
								{
									Range range1 = doc.Range(data.Start, data.End);
									Range rangenew2 = doccode.Range(doccode.Content.End - 1);
									rangenew2.FormattedText= range1.FormattedText;
									rangenew2.Find.Execute(FindText: type + @" [0-9]{1,3}", Replace: WdReplace.wdReplaceOne, ReplaceWith: "", MatchWildcards: true);
									foreach (ListTemplate it in doccode.ListTemplates)
									{
										if (it.Name == "template1")
										{
											rangenew2.ListFormat.ApplyListTemplateWithLevel(it, true, WdListApplyTo.wdListApplyToSelection, missing, 1);
											break;
										}
									}
								}
								catch { }
							}
							doccode.Content.Font.Name = "Times New Roman (Headings)";
							doccode.Close(SaveChanges:true);
						}
					}
					catch
					{

					}
				}
			}
			catch (Exception e)
			{
			}
		}

	}
}
