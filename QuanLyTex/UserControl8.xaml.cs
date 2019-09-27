using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Word.Application;
using Microsoft.Office.Interop.Word;
using QuanLyTex.User8Class;
using System.Linq;
using System.Configuration;

namespace QuanLyTex
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class UserControl8 : System.Windows.Controls.UserControl
	{
		string appPath = Directory.GetCurrentDirectory();
		List<string> listPath = new List<string>();
		public UserControl8()
		{
			InitializeComponent();
			DataContext = new User1Data();
		}
		public void SelectFile_Click(object sender, RoutedEventArgs e)
		{
			listPath = new List<string>();
			try
			{
				if (FileSelect1.IsChecked == true)
				{
					FolderBrowserDialog dialog = new FolderBrowserDialog
					{
						SelectedPath = @"C:\"
					};
					if (dialog.ShowDialog().ToString().Equals("OK"))
					{
						IEnumerable<string> enumerable = Directory.EnumerateFiles(dialog.SelectedPath, "*.docx");
						if (enumerable != null && enumerable.Count() > 0)
						{
							FileSelect.Text = dialog.SelectedPath;
							foreach (string str4 in enumerable)
							{
								listPath.Add(str4);
							}
						}
						IEnumerable<string> enumerable2 = Directory.EnumerateFiles(dialog.SelectedPath, "*.doc");
						if (enumerable2 != null && enumerable2.Count() > 0)
						{
							FileSelect.Text = dialog.SelectedPath;
							foreach (string str4 in enumerable2)
							{
								listPath.Add(str4);
							}
						}
						if(enumerable==null&& enumerable2==null)
						{
							System.Windows.MessageBox.Show("Không có file tex nào trong thư mục", "Thoát");
						}
					}
				}
				else if (FileSelect2.IsChecked == true)
				{
					FileSelect.Text = null;
					Microsoft.Win32.OpenFileDialog dialog2 = new Microsoft.Win32.OpenFileDialog()
					{
						Filter = "File document (*.doc;*.docx)|*.doc;*.docx|All files (*.*)|*.*",
						Multiselect = true,
						InitialDirectory = @"C:\"
					};
					dialog2.ShowDialog();
					foreach (string str in dialog2.FileNames)
					{
						FileSelect.Text = FileSelect.Text + Path.GetFullPath(str) + ";";
						listPath.Add(str);
					}
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
		public async void FilterBasic_Acynce(bool? Hide, List<string> list,string name,string rx, string type, bool? color, bool? bold, bool? italic,string appstr,bool?sort,bool? sort1,bool?sort2,bool?sort3,bool?sort4,bool?sort5,bool? bankex,bool? Id6,bool?str2)
		{
			int Lengthindex = 0;
			string path = "";
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					object missing = System.Reflection.Missing.Value;
					User8MapId classlist = new User8MapId();
					Application app = new Application();
					if (Hide == false)
					{
						app.Visible = true;
					}
					else { app.Visible = false; }
					Document document = app.Documents.Add(Visible: !Hide);
					Lengthindex=classlist.mapId(document, Hide, list, app, rx, type, color, bold, italic);
					DateTime time = DateTime.Now;
					string TimeName = time.ToString("h.mm.ss");
					path = appstr + @"\LuuFile" + @"\[" + name + "][" + TimeName + "].docx";
					if (sort == true || bankex == true)
					{
						classlist.mapSort(document, Hide, app, type, sort, sort1, sort2, sort3, sort4, sort5, path, rx, color, bold, italic, bankex, Id6, str2);
					}
					ListTemplate template = document.ListTemplates.Add();
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
					Range range = document.Content;
					Find find = range.Find;
					find.Font.Bold = 1;
					if (color == true) { find.Font.Color = WdColor.wdColorDarkBlue; }
					if (italic== true) { find.Font.Italic = 1; }
					while (find.Execute(Wrap: WdFindWrap.wdFindContinue, FindText: type + @" [0-9]{1,3}", MatchWildcards: true))
					{
						range.ListFormat.ApplyListTemplateWithLevel(template, true, WdListApplyTo.wdListApplyToSelection, missing, 1);
						range.Text = " ";
					}
					document.Content.Font.Name = "Times New Roman (Headings)";
					document.SaveAs(path, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocumentDefault);
					document.Close(SaveChanges: true);
					System.Windows.MessageBox.Show("Thực hiện lọc word thành công, các file được lưu trong Folder LuuFile trong thư mục app");
					app.Quit();
				}
				catch
				{ }
			});
			NumberEx.Text = ""+ Lengthindex;
			FileTexEx.Text = path;
		}
		private void FilterBasic_Click(object sender, RoutedEventArgs e)
		{
			
			try
			{
				if (System.Windows.MessageBox.Show("Tiến hành lọc Id", "Xác nhận lọc Id", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					string type1 = "",  str = "", strfile = "";
					if (Boxex.IsChecked == true) { type1 = TextStart.Text; };
					
					List<string> listbt = new List<string>();
					string str1, str2, str3, str4, str5, srt6, str1name, str2name, str3name, str4name, str5name, str6name;
					if (BoxClass.SelectedValue != "")
					{
						str1 = "[" + BoxClass.SelectedValue.Replace(BoxClass.Delimiter, "") + "]";
						str1name = str1;
					}
					else
					{
						str1 = @"[0-9]";
						str1name = "[F]";
					};
					if (BoxSubject.SelectedValue != "")
					{
						str2 = "[" + BoxSubject.SelectedValue.Replace(BoxSubject.Delimiter, "") + "]";
						str2name = str2;
					}
					else
					{
						str2 = @"[DH]";
						str2name = "[F]";
					};
					if (BoxChapter.SelectedValue != "")
					{
						str3 = "[" + BoxChapter.SelectedValue.Replace(BoxChapter.Delimiter, "") + "]";
						str3name = str3;
					}
					else
					{
						str3 = @"[0-9]";
						str3name = "[F]";
					};
					if (BoxLevel.SelectedValue != "")
					{
						str4 = "[" + BoxLevel.SelectedValue.Replace(BoxLevel.Delimiter, "") + "]";
						if (strId2.IsChecked == true)
						{
							str4 = str4.Replace('Y', '1').Replace('B', '2').Replace('K', '3').Replace('G', '4').Replace("T", "");
						}
						str4name = str4;
					}
					else
					{
						str4 = @"[GKBYT]";
						if(strId2.IsChecked == true) { str4 = @"[1-4]"; }
						str4name = "[F]";
					};
					if (BoxLesson.SelectedValue != "")
					{
						str5 = "[" + BoxLesson.SelectedValue.Replace(BoxLesson.Delimiter, "") + "]";
						str5name = str5;
					}
					else
					{
						str5 = @"[0-9]";
						str5name = "[F]";
					};
					if (BoxExerciseFormat.SelectedValue != "")
					{
						srt6 = "[" + BoxLesson.SelectedValue.Replace(BoxLesson.Delimiter, "") + "]";
						str6name = srt6;
					}
					else
					{
						srt6 = @"[0-9]";
						str6name = "[F]";
					}
					if (strId.IsChecked == true)
					{
						if (selectId5.IsChecked == true)
						{
							str = str1 + str2 + str3 + str4 + str5;
							strfile = str1name + str2name + str3name + str4name + str5name;
						}
						if (selectId6.IsChecked == true)
						{
							str = str1 + str2 + str3 + str4 + str5 + "-" + srt6;
							strfile = str1name + str2name + str3name + str4name + str5name + "-" + str6name;
						}
					}
					if(strId2.IsChecked==true)
					{
						if (selectId5.IsChecked == true)
						{
							str = str1 + str2 + str3 +"-"+ str5+"-" + str4;
							strfile = str1name + str2name + str3name+"-" + str5name+"-" + str4name;
						}
						if (selectId6.IsChecked == true)
						{
							str = str1 + str2 + str3 + "-" + str5+"."+ srt6 + "-" + str4;
							strfile = str1name + str2name + str3name + "-" + str5name+"."+ str6name + "-" + str4name;
						}
					}
					if (type1!=""&& Boxex.IsChecked== true&&ConfigurationManager.AppSettings["A"] == "1")
					{
						FilterBasic_Acynce(HideWord.IsChecked, listPath, strfile, str, type1, ColorOne.IsChecked, BoldOne.IsChecked,
							ItalicOne.IsChecked, appPath, sortoder.IsChecked, Sort1.IsChecked, Sort2.IsChecked, Sort3.IsChecked, Sort4.IsChecked, Sort5.IsChecked, BankEx.IsChecked, selectId6.IsChecked, strId2.IsChecked);
						System.Windows.MessageBox.Show("Chức năng thực hiện theo phương thức bất đồng bộ hóa, khi nào thực hiện xong sẽ có thông báo, trong lúc chờ đợi, các thầy cô có thể sử dụng các chức năng khác", "Thoát");
					}
					if(ConfigurationManager.AppSettings["A"] == "0")
					{
						System.Windows.MessageBox.Show("Chưa đăng kí bản quyền", "Thoát");
					}
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
	}
}
