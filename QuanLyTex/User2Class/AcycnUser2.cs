using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Application = Microsoft.Office.Interop.Word.Application;

namespace QuanLyTex.User2Class
{
	class AcycnUser2
	{
		public async System.Threading.Tasks.Task CreatPdfItem(List<string> list, string path)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					Application app = new Application();
					app.Visible = true;
					foreach (string item in list)
					{
						try
						{
							string pathitem = path + @"\" + System.IO.Path.GetFileNameWithoutExtension(item) + ".pdf";
							var document = app.Documents.Open(item,ReadOnly:true);
							document.SaveAs(pathitem, WdSaveFormat.wdFormatPDF);
							document.Close();
						}
						catch { }
					}
					app.Quit();

				}
				catch
				{ }
			});
		}
		public async System.Threading.Tasks.Task CreatPdf(List<string> listpath,string path)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					int count = listpath.Count;
					if (count < 3)
					{
							try
							{
								CreatPdfItem(listpath, path);
							}
							catch
							{ }
					}
					if (count < 20 && count >= 3)
					{
						for (int i = 0; i <= 2; i++)
						{ 
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % 3 == i)
														.Select(pair => pair.value )
														.ToList();
								CreatPdfItem(listnew, path);
							}
							catch
							{ }
						}
					}
					if (count >=20)
					{
						for (int i = 0; i <= 4; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % 5 == i)
														.Select(pair => pair.value)
														.ToList();
								CreatPdfItem(listnew, path);
							}
							catch
							{ }
						}
					}
					System.Windows.MessageBox.Show("Tạo Pdf được thực hiện bất đồng bộ, các file được lưu trong thư mục LuuFile, sẽ không có thông báo thành công hay không", "Thoát");
				}
				catch
				{
				}
			});
		}
		public async System.Threading.Tasks.Task MatchFile(List<string> listpath,string path)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					Application app = new Application();
					app.Visible = false;
					string pathend = path + @"\NewFile.docx";
					Document doc = app.Documents.Add();
					foreach (string item in listpath)
					{
						try
						{
							var document = app.Documents.Open(item,ReadOnly:true);
							Range rangenew = doc.Range(doc.Content.End - 1, doc.Content.End - 1);
							rangenew.FormattedText=document.Content;
							document.Close();
						}
						catch { }
					}
					doc.SaveAs2(pathend, WdSaveFormat.wdFormatDocumentDefault);
					app.Quit();
					System.Windows.MessageBox.Show("Ghép xong toàn bộ file", "Thoát");
				}
				catch
				{
					System.Windows.MessageBox.Show("Xử lí bị lỗi", "Thoát");
				}
			});
		}
		public async System.Threading.Tasks.Task AddPageItem(List<string> list, string path, string pathpage, bool? AddPdf,Document doc)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					Application app = new Application();
					app.Visible = false;
					foreach (string item in list)
					{
						try
						{
							Document document = app.Documents.Open(item);
							document.Application.Visible = false;
							Range rangenew = document.Range(0, 0);
							rangenew.FormattedText=doc.Content;
							if (AddPdf == true)
							{
								string pathitem = path + @"\" + System.IO.Path.GetFileNameWithoutExtension(item) + ".pdf";
								document.SaveAs2(pathitem, WdSaveFormat.wdFormatPDF);
							}
							document.Close();
						}
						catch { }
					}
					app.Quit();
				}
				catch
				{
				}
			});
		}
		public async System.Threading.Tasks.Task AddPage(List<string> listpath, string path,string pathpage,bool? AddPdf,int number)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					Application app = new Application();
					app.Visible = false;
					Document doc = app.Documents.Open(pathpage, ReadOnly: true);
					doc.Application.Visible = false;
					int count = listpath.Count;
					if (count < 3)
					{
						try
						{
							AddPageItem(listpath, path, pathpage, AddPdf, doc);
						}
						catch
						{ }
					}
					if (count < 10 && count >= 3)
					{
						for (int i = 0; i <= 2; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % 3 == i)
														.Select(pair => pair.value)
														.ToList();
								AddPageItem(listnew, path, pathpage, AddPdf,doc);
							}
							catch
							{ }
						}
					}
					if (count >= 10)
					{
						for (int i = 0; i < number; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % number == i)
														.Select(pair => pair.value)
														.ToList();
								AddPageItem(listnew, path, pathpage, AddPdf,doc);
							}
							catch
							{ }
						}
					}
					System.Windows.MessageBox.Show("Tạo trang đầu được thực hiện bất đồng bộ, các file được lưu trong thư mục LuuFile, sẽ không có thông báo thành công hay không", "Thoát");
					doc.Close();
					app.Quit();
				}
				catch
				{
				}
			});
		}
		public async System.Threading.Tasks.Task AddHeaderFooterItem(List<string> list, string path, string pathpage, bool? AddPdf, string headerleft, string footerleft)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					Application app = new Application();
					app.Visible = true;
					foreach (string item in list)
					{
						try
						{
							Document document = app.Documents.Open(item);
							foreach (Microsoft.Office.Interop.Word.Section wordSection in document.Sections)
							{
								//Get the footer range and add the footer details.
								Microsoft.Office.Interop.Word.Range footerRange = wordSection.Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
								footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkRed;
								footerRange.Font.Name = "Palatino Linotype";
								footerRange.Font.Size = 10;
								footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
								footerRange.Text = headerleft;
							}
							foreach (Microsoft.Office.Interop.Word.Section wordSection in document.Sections)
							{
								//Get the footer range and add the footer details.
								Microsoft.Office.Interop.Word.Range footerRange = wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
								footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkBlue;
								footerRange.Font.Size = 10;
								footerRange.Font.Name = "Palatino Linotype";
								footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
								footerRange.Text = footerleft;
							}
							document.Close();
						}
						catch { }
					}
					app.Quit();
				}
				catch
				{
				}
			});
		}
		public async System.Threading.Tasks.Task AddHeaderFooter(List<string> listpath, string path, string pathpage, bool? AddPdf, int number,string headerleft,string footerleft)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					int count = listpath.Count;
					if (count < 3)
					{
						try
						{
							AddHeaderFooterItem(listpath, path, pathpage, AddPdf, headerleft, footerleft);
						}
						catch
						{ }
					}
					if (count < 10 && count >= 3)
					{
						for (int i = 0; i <= 2; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % 3 == i)
														.Select(pair => pair.value)
														.ToList();
								AddHeaderFooterItem(listnew, path, pathpage, AddPdf, headerleft, footerleft);
							}
							catch
							{ }
						}
					}
					if (count >= 10)
					{
						for (int i = 0; i < number; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % number == i)
														.Select(pair => pair.value)
														.ToList();
								AddHeaderFooterItem(listnew, path, pathpage, AddPdf, headerleft, footerleft);
							}
							catch
							{ }
						}
					}
					System.Windows.MessageBox.Show("Tạo Hearder Footer được thực hiện bất đồng bộ, các file được lưu trong thư mục LuuFile, sẽ không có thông báo thành công hay không", "Thoát");
				}
				catch
				{
				}
			});
		}
		public async System.Threading.Tasks.Task QuestionItem(List<string> listpath,List<string> liststr,string pathsave,bool? color1,bool? bold1,bool? italic1,string loigiai,bool? AddPdf)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					Application app = new Application();
					app.Visible = false;
					foreach (string path in listpath)
					{
						try
						{
							List<int> list = new List<int>();
							var docOld = app.Documents.Open(path, ReadOnly: true);
							Document doc = app.Documents.Add();
							doc.Content.FormattedText = docOld.Content.FormattedText;
							docOld.Close();
							Range range = doc.Content;
							range.ListFormat.ConvertNumbersToText();
							range = doc.Content;
							range.Font.Underline = WdUnderline.wdUnderlineNone;
							foreach (string item in liststr)
							{
								Find find = range.Find;
								find.Execute(FindText: "(" + item + ")([ ]{1,})([0-9]{1,3})", Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \3");
								find = range.Find;
								find.Execute(FindText: "(" + item + ")([0-9]{1,3})", Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \2");
								if (color1 == true)
								{
									range = doc.Content;
									find = range.Find;
									find.Text = item + " [0-9]{1,3}";
									while (find.Execute(Wrap: WdFindWrap.wdFindStop, MatchWildcards: true))
									{
										if (range.Font.Color != WdColor.wdColorAutomatic && range.Font.Color != WdColor.wdColorBlack)
										{
											range.Font.Color = WdColor.wdColorDarkBlue;
										}
									}
								}
								range = doc.Content;
								find = range.Find;
								find.Text = item + " [0-9]{1,3}";
								if (color1 == true) { find.Font.Color = WdColor.wdColorDarkBlue; }
								if (bold1 == true) { find.Font.Bold = 1; }
								if (italic1 == true) { find.Font.Italic = 1; }
								while (find.Execute(Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Format: true))
								{
									list.Add(range.Start);
								}
							}
							list.Add(doc.Content.End);
							list.Sort();
							for (int i = list.Count - 2; i >=0; i--)
							{
								try
								{
									int end = list[i + 1];
									range = doc.Range(list[i], list[i + 1]);
									Find find = range.Find;
									find.Font.Bold = 1;
									if (find.Execute(FindText: loigiai, Format: true))
									{
										end = range.Start;
									}
									if (end < list[i + 1])
									{
										range = doc.Range(end, list[i + 1]);
										range.Delete();
									}
								}catch
								{

								}
							}
							string pathitem = pathsave + @"\" + System.IO.Path.GetFileNameWithoutExtension(path) + "DeBai.docx";
							doc.Content.Font.Name = "Times New Roman (Headings)";
							doc.SaveAs(pathitem, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault);
							if (AddPdf == true)
							{
								pathitem = pathsave + @"\" + System.IO.Path.GetFileNameWithoutExtension(path) + ".pdf";
								doc.SaveAs2(pathitem, WdSaveFormat.wdFormatPDF);
							}
							doc.Close();
						}
						catch
						{ }
					}
					app.Quit();
				}
				catch
				{

				}
			});
		}
		public async System.Threading.Tasks.Task Question(List<string> listpath, List<string> liststr, string pathsave, bool? color1, bool? bold1, bool? italic1, string loigiai, bool? AddPdf, int number)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					int count = listpath.Count;
					if (count < 3)
					{
						try
						{
							QuestionItem(listpath, liststr, pathsave,color1, bold1, italic1, loigiai, AddPdf);
						}
						catch
						{ }
					}
					if (count < 10 && count >= 3)
					{
						for (int i = 0; i <= 2; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % 3 == i)
														.Select(pair => pair.value)
														.ToList();
								QuestionItem(listnew, liststr, pathsave, color1, bold1, italic1, loigiai, AddPdf);
							}
							catch
							{ }
						}
					}
					if (count >= 10)
					{
						for (int i = 0; i < number; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % number == i)
														.Select(pair => pair.value)
														.ToList();
								QuestionItem(listnew, liststr, pathsave, color1, bold1, italic1, loigiai, AddPdf);
							}
							catch
							{ }
						}
					}
					System.Windows.MessageBox.Show("Lấy phần đề bài được thực hiện bất đồng bộ, các file được lưu trong thư mục LuuFile, sẽ không có thông báo thành công hay không", "Thoát");
				}
				catch
				{
				}
			});
		}
		public async System.Threading.Tasks.Task ProofItem(List<string> listpath, List<string> liststr, string pathsave, bool? color1, bool? bold1, bool? italic1, string loigiai,bool? AddPdf)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					Application app = new Application();
					app.Visible = false;
					foreach (string path in listpath)
					{
						try
						{
							List<int> list = new List<int>();
							List<int> listend = new List<int>();
							var docOld = app.Documents.Open(path, ReadOnly: true);
							Document doc = app.Documents.Add();
							doc.Content.FormattedText = docOld.Content.FormattedText;
							docOld.Close();
							Range range = doc.Content;
							range.ListFormat.ConvertNumbersToText();
							range = doc.Content;
							range.Font.Underline = WdUnderline.wdUnderlineNone;
							foreach (string item in liststr)
							{
								range = doc.Content;
								Find find = range.Find;
								find.Execute(FindText: "(" + item + ")([ ]{1,})([0-9]{1,3})", Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \3");
								find = range.Find;
								find.Execute(FindText: "(" + item + ")([0-9]{1,3})", Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \2");
								if (color1 == true)
								{
									range = doc.Content;
									find = range.Find;
									find.Text = item + " [0-9]{1,3}";
									while (find.Execute(Wrap: WdFindWrap.wdFindStop, MatchWildcards: true))
									{
										if (range.Font.Color != WdColor.wdColorAutomatic && range.Font.Color != WdColor.wdColorBlack)
										{
											range.Font.Color = WdColor.wdColorDarkBlue;
										}
									}
								}
								range = doc.Content;
								find = range.Find;
								find.Text = item + " [0-9]{1,3}";
								if (color1 == true) { find.Font.Color = WdColor.wdColorDarkBlue; }
								if (bold1 == true) { find.Font.Bold = 1; }
								if (italic1 == true) { find.Font.Italic = 1; }
								while (find.Execute(Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Format: true))
								{
									list.Add(range.Start);
									listend.Add(range.End);
								}
							}
							list.Add(doc.Content.End);
							list.Sort();
							for (int i = list.Count - 2; i >= 0; i--)
							{
								try
								{
									int end = range.End;
									range = doc.Range(list[i], list[i + 1]);
									Find find = range.Find;
									find.Font.Bold = 1;
									if (find.Execute(FindText: loigiai, Format: true))
									{
										end = range.Start;
									}
									else
									{
										end = list[i + 1];
									}
									Range rangenew = doc.Range(listend[i]+2, end);
									range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
									rangenew.Text = "";
								}
								catch { }
							}
							string pathitem = pathsave + @"\" + System.IO.Path.GetFileNameWithoutExtension(path) + "LoiGiai.docx";
							range = doc.Content;
							Find find1 = range.Find;
							find1.Execute(FindText: "^p.^p", Replace: WdReplace.wdReplaceAll, ReplaceWith: "^p");
							doc.Content.Font.Name = "Times New Roman (Headings)";
							doc.SaveAs(pathitem, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault);
							if (AddPdf == true)
							{
								pathitem = pathsave + @"\" + System.IO.Path.GetFileNameWithoutExtension(path) + ".pdf";
								doc.SaveAs2(pathitem, WdSaveFormat.wdFormatPDF);
							}
							doc.Close();
						}
						catch { }
					}
					app.Quit();
				}
				catch
				{

				}
			});
		}
		public async System.Threading.Tasks.Task Proof(List<string> listpath, List<string> liststr, string pathsave, bool? color1, bool? bold1, bool? italic1, string loigiai, bool? AddPdf,int number)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					int count = listpath.Count;
					if (count < 3)
					{
						try
						{
							ProofItem(listpath, liststr, pathsave, color1, bold1, italic1, loigiai, AddPdf);
						}
						catch
						{ }
					}
					if (count < 10 && count >= 3)
					{
						for (int i = 0; i <= 2; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % 3 == i)
														.Select(pair => pair.value)
														.ToList();
								ProofItem(listnew, liststr, pathsave, color1, bold1, italic1, loigiai, AddPdf);
							}
							catch
							{ }
						}
					}
					if (count >= 10)
					{
						for (int i = 0; i <= number; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % number == i)
														.Select(pair => pair.value)
														.ToList();
								ProofItem(listnew, liststr, pathsave, color1, bold1, italic1, loigiai, AddPdf);
							}
							catch
							{ }
						}
					}
					System.Windows.MessageBox.Show("Lấy phần lời giải được thực hiện bất đồng bộ, các file được lưu trong thư mục LuuFile, sẽ không có thông báo thành công hay không", "Thoát");
				}
				catch
				{
				}
			});
		}
		public async System.Threading.Tasks.Task CreatTableItem(List<string> listpath, List<string> liststr, string pathsave, bool? color1, bool? bold1, bool? italic1, string loigiai,bool? UnderLineTwo,bool? ColorTwo,bool? HghtlightTwo)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					Application app = new Application();
					app.Visible = false;
					foreach (string path in listpath)
					{
						try
						{
							List<int> list = new List<int>();
							List<char> listproof = new List<char>();
							List<string> listtex = new List<string>();
							var doc = app.Documents.Open(path);
							Range range = doc.Content;
							range.ListFormat.ConvertNumbersToText();
							range = doc.Content;
							range.Font.Underline = WdUnderline.wdUnderlineNone;
							range = doc.Content;
							Find find = range.Find;
							find.Execute(FindText: @"([ABCD])(.)", MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1.");
							foreach (string item in liststr)
							{
								range = doc.Content;
								find = range.Find;
								find.Execute(FindText: "(" + item + ")([ ]{1,})([0-9]{1,3})", Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \3");
								find = range.Find;
								find.Execute(FindText: "(" + item + ")([0-9]{1,3})", Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \2");
								if (color1 == true)
								{
									range = doc.Content;
									find = range.Find;
									find.Text = item + " [0-9]{1,3}";
									while (find.Execute(Wrap: WdFindWrap.wdFindStop, MatchWildcards: true))
									{
										if (range.Font.Color != WdColor.wdColorAutomatic && range.Font.Color != WdColor.wdColorBlack)
										{
											range.Font.Color = WdColor.wdColorDarkBlue;
										}
									}
								}
								range = doc.Content;
								find = range.Find;
								find.Text = item + " [0-9]{1,3}";
								if (color1 == true) { find.Font.Color = WdColor.wdColorDarkBlue; }
								if (bold1 == true) { find.Font.Bold = 1; }
								if (italic1 == true) { find.Font.Italic = 1; }
								while (find.Execute(Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Format: true))
								{
									list.Add(range.Start);
									listtex.Add(range.Text);
								}
							}
							list.Add(doc.Content.End);
							list.Sort();
							for (int i = 0; i < list.Count-1; i++)
							{
								int end = range.End;
								range = doc.Range(list[i], list[i + 1]);
								find = range.Find;
								find.Execute(FindText: @"([ABCD])(.)", Format: true, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"^p\1.");
								find = range.Find;
								find.Execute(FindText: @"([ABCD])([ ]{1,})(.)", Format: true, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"^p\1.");
								find = range.Find;
								find.Font.Bold = 1;
								if (ColorTwo == true) { find.Font.Color = WdColor.wdColorRed; }
								if (UnderLineTwo == true) { find.Font.Underline = WdUnderline.wdUnderlineSingle; }
								if (HghtlightTwo == true) { find.Highlight = 1; }
								if (find.Execute(FindText: "([ABCD]).", Format: true, MatchWildcards: true))
								{
									string text = range.Text;
									char[] chars = { 'A', 'B', 'C', 'D' };
									listproof.Add(text[0]);
								}
								else
								{
									listproof.Add('N');
								}
							}
							doc.Content.InsertParagraph();
							Range rangeend = doc.Range(doc.Content.End - 1, doc.Content.End - 1);
							rangeend.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
							rangeend.Font.Color = WdColor.wdColorBlack;
							rangeend.Bold = 1;
							rangeend.Font.Color = WdColor.wdColorDarkRed;
							rangeend.Font.Size = 20;
							rangeend.Text = "BẢNG ĐÁP ÁN.\r\n";
							rangeend = doc.Range(rangeend.End - 1);
							rangeend.ParagraphFormat.LeftIndent = 0;
							int numbercol = listtex.Count / 10 + 1;
							Microsoft.Office.Interop.Word.Table table = rangeend.Tables.Add(rangeend, numbercol * 2, 10, WdDefaultTableBehavior.wdWord9TableBehavior, WdAutoFitBehavior.wdAutoFitWindow);
							for (int i = 1; i <= numbercol; i++)
								for (int j = 1; j <= 10; j++)
								{
									Row row = table.Rows[2*i-1];
									Cell cell = row.Cells[j];
									if ((i + j) % 2 == 1)
									{
										cell.Shading.BackgroundPatternColor = WdColor.wdColorGray20;
									}
									int index = (i - 1) * 10 + j;
									if (index <= listtex.Count)
									{
										cell.Range.Text = "" + listtex[index - 1];
										cell.Range.Font.Size = 12;
									}
									row = table.Rows[2*i];
									cell = row.Cells[j];
									if ((i + j) % 2 == 1)
									{
										cell.Shading.BackgroundPatternColor = WdColor.wdColorGray20;
									}
									index = (i - 1) * 10 + j;
									if (index <= listtex.Count)
									{
										cell.Range.Text = "" + listproof[index - 1];
										cell.Range.Font.Size = 12;
									}
								}
							doc.Content.Font.Name = "Times New Roman (Headings)";
							doc.Close(SaveChanges: true);
						}
						catch { }
					}
					app.Quit();
				}
				catch
				{

				}
			});
		}
		public async System.Threading.Tasks.Task CreatTable(List<string> listpath, List<string> liststr, string pathsave, bool? color1, bool? bold1, bool? italic1, string loigiai, bool? UnderLineTwo, bool? ColorTwo, bool? HghtlightTwo, int number)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					int count = listpath.Count;
					if (count < 3)
					{
						try
						{
							CreatTableItem(listpath, liststr, pathsave, color1, bold1, italic1, loigiai, UnderLineTwo, ColorTwo, HghtlightTwo);
						}
						catch
						{ }
					}
					if (count < 10 && count >= 3)
					{
						for (int i = 0; i <= 2; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % 3 == i)
														.Select(pair => pair.value)
														.ToList();
								CreatTableItem(listnew, liststr, pathsave, color1, bold1, italic1, loigiai, UnderLineTwo, ColorTwo, HghtlightTwo);
							}
							catch
							{ }
						}
					}
					if (count >= 10)
					{
						for (int i = 0; i < number; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % number == i)
														.Select(pair => pair.value)
														.ToList();
								CreatTableItem(listnew, liststr, pathsave, color1, bold1, italic1, loigiai, UnderLineTwo, ColorTwo, HghtlightTwo);
							}
							catch
							{ }
						}
					}
					System.Windows.MessageBox.Show("Tạo bảng đáp án được thực hiện bất đồng bộ, các file được lưu trong thư mục LuuFile, sẽ không có thông báo thành công hay không", "Thoát");
				}
				catch
				{
				}
			});
		}
		public async System.Threading.Tasks.Task BTNformItem(List<string> listpath,List<string> liststr, string pathsave, bool? color1, bool? bold1, bool? italic1, string loigiai)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					Application app = new Application();
					app.Visible = false;
					foreach (string path in listpath)
					{
						try
						{
							var doc = app.Documents.Open(path);
							Range range = doc.Content;
							range.ParagraphFormat.TabStops.ClearAll();
							range.ParagraphFormat.TabStops.Add(60, WdTabAlignment.wdAlignTabLeft);
							range.ParagraphFormat.TabStops.Add(170, WdTabAlignment.wdAlignTabLeft);
							range.ParagraphFormat.TabStops.Add(280, WdTabAlignment.wdAlignTabLeft);
							range.ParagraphFormat.TabStops.Add(390, WdTabAlignment.wdAlignTabLeft);
							range.ParagraphFormat.LeftIndent = 60;
							range.ListFormat.ConvertNumbersToText();
							range.Font.Underline = WdUnderline.wdUnderlineNone;
							Find find = range.Find;
							find.Execute(FindText: @"([ABCD])(.)", MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1.");
							foreach (string item in liststr)
							{
								range = doc.Content;
								find = range.Find;
								find.Execute(FindText: "(" + item + ")([ ]{1,})([0-9]{1,3})", Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \3");
								find = range.Find;
								find.Execute(FindText: "(" + item + ")([0-9]{1,3})", Wrap: WdFindWrap.wdFindStop, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1 \2");
								if (color1 == true)
								{
									range = doc.Content;
									find = range.Find;
									find.Text = item + " [0-9]{1,3}";
									while (find.Execute(Wrap: WdFindWrap.wdFindStop, MatchWildcards: true))
									{
										if (range.Font.Color != WdColor.wdColorAutomatic && range.Font.Color != WdColor.wdColorBlack)
										{
											range.Font.Color = WdColor.wdColorDarkBlue;
										}
									}
								}
							}
							range = doc.Content;
							find = range.Find;
							find.Execute(FindText: @"([^t^13])([ ]{1,})", Replace: WdReplace.wdReplaceAll, ReplaceWith: @"\1", MatchWildcards: true);
							find = range.Find;
							find.Execute(FindText: @"([ABCD])(.)", Format: true, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"^p\1.");
							find = range.Find;
							find.Execute(FindText: @"([ABCD])([ ]{1,})(.)", Format: true, MatchWildcards: true, Replace: WdReplace.wdReplaceAll, ReplaceWith: @"^p\1.");
							range = doc.Content;
							find = range.Find;
							find.Text = @"([ABCD])(.)";
							while (find.Execute(Wrap: WdFindWrap.wdFindStop, Format: true, MatchWildcards: true))
							{
								if (range.Font.Color != WdColor.wdColorBlack && range.Font.Color != WdColor.wdColorRed)
								{
									range.Font.Color = WdColor.wdColorDarkBlue;
								}
							}
							doc.Close(SaveChanges: true);
						}
						catch { }
					}
					app.Quit();
				}
				catch
				{

				}
			});
		}
		public async System.Threading.Tasks.Task BTNform(List<string> listpath, List<string> liststr, string pathsave, bool? color1, bool? bold1, bool? italic1, string loigiai, int number)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					int count = listpath.Count;
					if (count < 3)
					{
						try
						{
							BTNformItem( listpath,  liststr,  pathsave,  color1,  bold1,  italic1,  loigiai);
						}
						catch
						{ }
					}
					if (count < 10 && count >= 3)
					{
						for (int i = 0; i <= 2; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % 3 == i)
														.Select(pair => pair.value)
														.ToList();
								BTNformItem(listnew, liststr, pathsave, color1, bold1, italic1, loigiai);
							}
							catch
							{ }
						}
					}
					if (count >= 10)
					{
						for (int i = 0; i < number; i++)
						{
							try
							{
								List<string> listnew = listpath.Select((value, index) => new { value, index })
														.Where(pair => pair.index % number == i)
														.Select(pair => pair.value)
														.ToList();
								BTNformItem(listnew, liststr, pathsave, color1, bold1, italic1, loigiai);
							}
							catch
							{ }
						}
					}
					System.Windows.MessageBox.Show("Chuyển về form BTN được thực hiện bất đồng bộ, các file được lưu trong thư mục LuuFile, sẽ không có thông báo thành công hay không", "Thoát");
				}
				catch
				{
				}
			});
		}
	}
}
