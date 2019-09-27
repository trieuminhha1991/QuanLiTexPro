using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using QuanLyTex.User1Class;
using Xceed.Wpf.DataGrid;
using appdll;
using System.Configuration;

namespace QuanLyTex
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class UserControl1 : System.Windows.Controls.UserControl
	{
		string app = Directory.GetCurrentDirectory();
		string appPath = "";
		Dictionary<string, dynamic> Dic = new Dictionary<string, dynamic>();
		User1MapTex classlist = new User1MapTex();
		List<string> listPath = new List<string>();
		string strfile = "";
		public UserControl1()
		{
			InitializeComponent();
			DataContext = new User1Data();
			fileForm.Text=app+@"\MauFile\MacDinh\ChuyenDe.tex"; 
		}
		public void SelectFile_Click(object sender, RoutedEventArgs e)
		{
			appPath = "";
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
						appPath = dialog.SelectedPath;
						IEnumerable<string> enumerable = Directory.EnumerateFiles(dialog.SelectedPath, "*.tex");
						if (enumerable != null && enumerable.Count() > 0)
						{
							FileSelect.Text = dialog.SelectedPath;
							foreach (string str4 in enumerable)
							{
								listPath.Add(str4);
							}
						}
						else
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
						Filter = "File Latex (*.tex)|*.tex|All files (*.*)|*.*",
						Multiselect = true,
						InitialDirectory = @"C:\"
					};
					dialog2.ShowDialog();
					foreach (string str in dialog2.FileNames)
					{
						FileSelect.Text = FileSelect.Text + Path.GetFullPath(str) + ";";
						appPath = Directory.GetParent(str).ToString();
						listPath.Add(str);
					}
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
		private void SelectForm_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
				{
					Filter = "File Latex (*.tex)|*.tex|All files (*.*)|*.*"
				};
				dialog.ShowDialog();
				fileForm.Text = Path.GetFullPath(dialog.FileName);
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
		private void EditForm(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start(fileForm.Text);
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
		private List<string> CreatRegex()
		{
			List<string> array = new List<string>(); ;
			string strfile = "";
			string str = "";
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
				str4name = str4;
			}
			else
			{
				str4 = @"[GKBY]";
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
			if (selectId5.IsChecked == true)
			{
				str = @"\[" + str1 + str2 + str3 + str4 + str5;
				strfile = str1name + str2name + str3name + str4name + str5name;
				array.Add(str);
				array.Add(strfile);
			}
			if (selectId6.IsChecked == true)
			{
				str = @"\[" + str1 + str2 + str3 + str4 + str5 + "-" + srt6 + "]";
				strfile = str1name + str2name + str3name + str4name + str5name + "-" + str6name;
				array.Add(str);
				array.Add(strfile);
			}
			return array;
		}
		private void FilterBasic_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (System.Windows.MessageBox.Show("Tiến hành lọc Id", "Xác nhận lọc Id", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					AcynsUser1 TexTo = new AcynsUser1();
					Dic = new Dictionary<string, dynamic>();
					string str = "";
					string type = "ex";
					if (Boxbt.IsChecked == true) { type = "bt"; };
					List<string> listex = new List<string>();
					List<string> listregex = CreatRegex();
					str = listregex[0];
					strfile= listregex[1];
					Regex rx = new Regex(str);
					if (BankExcer.IsChecked == true && ConfigurationManager.AppSettings["A"] == "1")
					{
						AcynsUser1 bank = new AcynsUser1();
						bank.BankEcer(app, selectId5.IsChecked, selectId6.IsChecked, listPath, rx, type);
					}
					Dictionary<string, dynamic> mapEx = new Dictionary<string, dynamic>();
					List<dynamic> listMapEx = classlist.mapNewFile(rx, type, listPath);
					string strHeader = "";
					string strFooter = "";
					if (Form.IsChecked == true)
					{
						strHeader = File.ReadAllText(fileForm.Text);
						string FooterPath = app + @"\MauFile\Footer.tex";
						strFooter = File.ReadAllText(FooterPath);
					}
					if (sortoder.IsChecked == true&& ConfigurationManager.AppSettings["A"] == "1")
					{
						mapEx = classlist.mapSort(listMapEx, type);
						List<SortId> listsort = mapEx["listid"];
						if (Sort1.IsChecked == true)
						{
							listsort = listsort.OrderBy(m => m.ClassId).ThenBy(m => m.ObjectId).ThenBy(m => m.CharterId).ThenBy(m => m.SectionId).ThenBy(m => m.LevelId).ToList<SortId>();
						}
						if (Sort2.IsChecked == true)
						{
							listsort = listsort.OrderBy(m => m.ObjectId).ThenBy(m => m.ClassId).ThenBy(m => m.CharterId).ThenBy(m => m.SectionId).ThenBy(m => m.LevelId).ToList<SortId>();
						}
						if (Sort3.IsChecked == true)
						{
							listsort = listsort.OrderBy(m => m.ClassId).ThenBy(m => m.ObjectId).ThenBy(m => m.CharterId).ThenBy(m => m.LevelId).ThenBy(m => m.SectionId).ToList<SortId>();
						}
						if (Sort4.IsChecked == true)
						{
							listsort = listsort.OrderBy(m => m.LevelId).ThenBy(m => m.ClassId).ThenBy(m => m.ObjectId).ThenBy(m => m.CharterId).ThenBy(m => m.SectionId).ToList<SortId>();
						}
						foreach (SortId item in listsort)
						{
							string codeId = item.CodeId;
							List<string> list = mapEx[codeId];
							listex.AddRange(list);
						}
					}
					else
					{
						foreach (Dictionary<string, string> item in listMapEx)
						{
							string strthu = item["exersice"];
							listex.Add(strthu);
						}
					}
					if (DevideFile.IsChecked == true&&ConfigurationManager.AppSettings["A"] == "1")
					{
						TexTo.DevideFile(type, listMapEx, appPath, strHeader, strFooter, Boxbt.IsChecked, AutoWord.IsChecked, Devide1.IsChecked, Devide2.IsChecked, Devide3.IsChecked, Devide4.IsChecked);
					}
					if (listex != null&& listex.Count > 0 && ConfigurationManager.AppSettings["A"] == "1")
					{
						if (commentorder.IsChecked == true)
						{
							User1Before classBefore = new User1Before();
							listex = classBefore.CommentOrder(listex);
						};
						string path1 = appPath + @"\LuuFile[" + strfile + "]" + type + ".tex";
						classlist.newFileTex(listex, path1, strHeader, strFooter);
						FileTexEx.Text = path1; NumberEx.Text = listex.Count.ToString();
						if (AutoWord.IsChecked == true)
						{
							List<string> listExNew = new List<string>();
							char typechar = 'e';
							if (Boxbt.IsChecked == true)
							{
								typechar = 'b';
							}
							foreach (string item in listex)
							{

								string itemnew = typechar + item.Remove(item.Length - 8, 8).Remove(0, 11);
								listExNew.Add(itemnew);
							}
							string pathnem = appPath + @"\LuuFile[" + strfile + "]" + type;
							if (DevideFile.IsChecked == false)
							{
								TexTo.startListTexToWord1(listExNew, pathnem);
							}
						}
						else
						{
							
						}
						System.Windows.MessageBox.Show("Tạo file thành công, các chức năng BankEx, tách file, tex to word sẽ chạy bất đồng bộ, các file sẽ được lưu trong folder LuuFile, sẽ có thông báo khi thành công", "Thành công");
					}
					else
					{
						System.Windows.MessageBox.Show("Tạo file không thành công", "Thoát");
					}
					if (ConfigurationManager.AppSettings["A"] == "0")
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
		private void FilterAdvandedEx_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (System.Windows.MessageBox.Show("Tiến hành lọc Id", "Xác nhận lọc Id", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					Dic = new Dictionary<string, dynamic>();
					List<dynamic> listMapEx = new List<dynamic>();
					string str = "";
					string type = "ex";
					List<string> listex = new List<string>();
					List<string> listbt = new List<string>();
					List<string> listregex = CreatRegex();
					str = listregex[0];
					strfile = listregex[1];
					Regex rx = new Regex(str);
					listMapEx = classlist.mapNewFile(rx, type, listPath);
					if ((listMapEx == null || listMapEx.Count == 0))
					{
						System.Windows.MessageBox.Show("Không có câu hỏi nào trắc nghiệm được lọc ra", "Thoát");
					}
					List<string> listLabel = new List<string>();
					List<SortId> listsort = new List<SortId>();
					Dic = new Dictionary<string, dynamic>();
					var listDataGrid = new List<DataGrid1>();
					foreach (var item in listMapEx)
					{
						string stringId = item["codeId"];
						string stringEcer = item["exersice"];
						if (listLabel.Contains(stringId))
						{
							Dic[stringId].Add(stringEcer);
						}
						else
						{
							List<string> listEcer = new List<string>();
							listEcer.Add(stringEcer);
							Dic.Add(stringId, listEcer);
							listLabel.Add(stringId);
							SortId sort = new SortId();
							sort.ClassId = int.Parse(stringId[0].ToString());
							if (stringId[1] == 'D') { sort.ObjectId = 1; } else { sort.ObjectId = 2; }
							sort.CharterId = int.Parse(stringId[2].ToString());
							switch (stringId[3])
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
							sort.SectionId = int.Parse(stringId[4].ToString());
							sort.CodeId = stringId;
							listsort.Add(sort);
						}
					}
					Dic.Add("listid", listsort);
					Dic.Add("listCodeId", listLabel);
					foreach (var item in listLabel)
					{
						DataGrid1 getDataGrid1 = new DataGrid1();
						int classId = Int32.Parse(item[0].ToString());
						if (classId > 3)
						{
							getDataGrid1.ClassName = "Lớp " + classId;
						}
						if (classId <= 3)
						{
							getDataGrid1.ClassName = "Lớp 1" + classId;
						}
						char strChapterName1 = item[1];
						char strChapterName2 = item[2];
						if (strChapterName1 == 'H')
						{
							getDataGrid1.ChapterName = "Hình  chương " + strChapterName2;
						}
						if (strChapterName1 == 'D')
						{
							getDataGrid1.ChapterName = "Đại  chương " + strChapterName2;
						}
						char strSectionName = item[4];
						getDataGrid1.SectionName = "Bài " + strSectionName;
						char levelName = item[3];
						if (levelName == 'Y')
						{
							getDataGrid1.LevelId = "Yếu";
						}
						if (levelName == 'B')
						{
							getDataGrid1.LevelId = "Trung bình";
						}
						if (levelName == 'K')
						{
							getDataGrid1.LevelId = "Khá";
						}
						if (levelName == 'G')
						{
							getDataGrid1.LevelId = "Giỏi";
						}
						getDataGrid1.CodeId = item;
						getDataGrid1.NumberExersice = Dic[item].Count;
						getDataGrid1.NumberExersiceSelect = 0;
						getDataGrid1.IsSelected = false;
						listDataGrid.Add(getDataGrid1);
					}
					DataGridCollectionViewSource source = GridTotal.FindResource("cvsDataGrid") as DataGridCollectionViewSource;
					source.Source = listDataGrid;
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
		private void FilterAdvandedStart_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				AcynsUser1 TexTo = new AcynsUser1();
				Dictionary<string, dynamic> mapEx = new Dictionary<string, dynamic>();
				string path1 = appPath + @"\LuuFile" + strfile + "]ex.tex";
				List<string> listend = new List<string>();
				List<SortId> listcodeId = new List<SortId>();
				DataGridCollectionViewSource source = GridTotal.FindResource("cvsDataGrid") as DataGridCollectionViewSource;
				foreach (var item in DataGrid.Items)
				{
					DataGrid.CurrentItem = item;
					Xceed.Wpf.DataGrid.DataRow row = DataGrid.GetContainerFromItem(DataGrid.CurrentItem) as Xceed.Wpf.DataGrid.DataRow;
					if (row != null)
					{
						int numberExerciseselect = 0;
						string codeId = row.Cells[0].Content.ToString();
						var itit = row.Cells[3].Content;
						if (row.Cells[2].Content != null)
						{
							numberExerciseselect = (int)row.Cells[3].Content;
						}
						int numberExercise = (int)row.Cells[2].Content;
						if (numberExerciseselect > numberExercise)
						{
							listend = null;
							break;
						}
						else
						{
							bool isCheck = (bool)row.Cells[4].Content;
							if (isCheck==true && numberExerciseselect > 0)
							{
								List<string> list = new List<string>();
								List<string> listItem = Dic[codeId];
								int itemCount = listItem.Count;
								Random random = new Random();
								if (sortoder.IsChecked == false)
								{
									for (int i = 0; i < numberExerciseselect; i++)
									{
										list.Add(listItem[random.Next(0, itemCount)]);
									}
									listend.AddRange(list);
								}
								else
								{
									for (int i = 0; i < numberExerciseselect; i++)
									{
										list.Add(listItem[random.Next(0, itemCount)]);
									}
									mapEx.Add(codeId, list);
								}
							}
						}
					}
				}
				
				if(sortoder.IsChecked ==true && ConfigurationManager.AppSettings["A"] == "1")
				{
					List<SortId> listsort = Dic["listid"];
					if (Sort1.IsChecked == true)
					{
						listsort = listsort.OrderBy(m => m.ClassId).ThenBy(m => m.ObjectId).ThenBy(m => m.CharterId).ThenBy(m => m.SectionId).ThenBy(m => m.LevelId).ToList<SortId>();
					}
					if (Sort2.IsChecked == true)
					{
						listsort = listsort.OrderBy(m => m.ObjectId).ThenBy(m => m.ClassId).ThenBy(m => m.CharterId).ThenBy(m => m.SectionId).ThenBy(m => m.LevelId).ToList<SortId>();
					}
					if (Sort3.IsChecked == true)
					{
						listsort = listsort.OrderBy(m => m.ClassId).ThenBy(m => m.ObjectId).ThenBy(m => m.CharterId).ThenBy(m => m.LevelId).ThenBy(m => m.SectionId).ToList<SortId>();
					}
					if (Sort4.IsChecked == true)
					{
						listsort = listsort.OrderBy(m => m.LevelId).ThenBy(m => m.ClassId).ThenBy(m => m.ObjectId).ThenBy(m => m.CharterId).ThenBy(m => m.SectionId).ToList<SortId>();
					}
					foreach (SortId item in listsort)
					{
						string codeId = item.CodeId;
						if (mapEx.ContainsKey(codeId))
						{
							List<string> list = mapEx[codeId];
							listend.AddRange(list);
							listcodeId.Add(item);
						}
					}
				}
				mapEx.Add("listid", listcodeId);
				string strHeader = "";
				string strFooter = "";
				if (Form.IsChecked == true)
				{
					strHeader = File.ReadAllText(fileForm.Text);
					string FooterPath = app + @"\MauFile\Footer.tex";
					strFooter = File.ReadAllText(FooterPath);
				}
				if (listend != null && listend.Count > 0&&DevideFile.IsChecked == true && ConfigurationManager.AppSettings["A"] == "1")
				{
					TexTo.DevideFile2("ex", mapEx, appPath, strHeader, strFooter, Boxbt.IsChecked, AutoWord.IsChecked, Devide1.IsChecked, Devide2.IsChecked, Devide3.IsChecked, Devide4.IsChecked);
				}
				if (listend != null&&listend.Count>0 && ConfigurationManager.AppSettings["A"] == "1")
				{
					if (commentorder.IsChecked == true)
					{
						User1Before classBefore = new User1Before();
						listend = classBefore.CommentOrder(listend);
					}
					if (Boxex.IsChecked == true)
					{
						string pathnew = appPath + @"\LuuFile[" + strfile + "]ex.tex";
						classlist.newFileTex(listend, pathnew, strHeader, strFooter);
						FileTexEx.Text = path1;
						NumberEx.Text = listend.Count.ToString();
						if (AutoWord.IsChecked == true)
						{
							List<string> listExNew = new List<string>();
							foreach (string item in listend)
							{
								string itemnew = "e" + item.Remove(item.Length - 8, 8).Remove(0, 11);
								listExNew.Add(itemnew);
							}
							string path2nem = appPath + @"\LuuFile[" + strfile + "]ex";
							if (DevideFile.IsChecked ==  false)
							{
								TexTo.startListTexToWord1(listExNew, path2nem);
							}
						}
					}
					System.Windows.MessageBox.Show("Tạo file thành công, các chức năng BankEx sẽ ko sử dụng khi lọc nâng cao,chức năng tách file, tex to word sẽ chạy bất đồng bộ, các file sẽ được lưu trong folder LuuFile,sẽ có thông báo khi thành công", "Thành công");
				}
				else
				{
					System.Windows.MessageBox.Show("Tạo file không thành công", "Thoát");
				}
				if (ConfigurationManager.AppSettings["A"] == "0")
				{
					System.Windows.MessageBox.Show("Chưa đăng kí bản quyền", "Thoát");
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
	}
}
