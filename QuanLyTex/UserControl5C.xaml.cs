using QuanLyTex.User5Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Orientation = System.Windows.Controls.Orientation;
using Application = Microsoft.Office.Interop.Word.Application;
using System.Diagnostics;
using System.Configuration;
using System.Linq;

namespace QuanLyTex
{
	/// <summary>
	/// Interaction logic for UserControl5A.xaml
	/// </summary>
	/// 
	public partial class UserControl5C : System.Windows.Controls.UserControl
	{
		TexToWordEquation TexTo = new TexToWordEquation();
		int indexListBox = 0;
		public UserControl5C()
		{
			InitializeComponent();
		}
		private void ListBoxSelectFileAdd(string path)
		{
			try
			{
				ListBoxItem Item = new ListBoxItem();
				StackPanel Stack = new StackPanel();
				Stack.Orientation = Orientation.Horizontal;
				System.Windows.Controls.TextBox textBox = new System.Windows.Controls.TextBox();
				textBox.Text = path;
				textBox.Height = 23;
				textBox.Width = 550;
				textBox.FontSize = 12;
				if (indexListBox % 2 == 1)
				{
					textBox.Background = Brushes.AliceBlue;
				}
				Stack.Children.Add(textBox);
				System.Windows.Controls.CheckBox checkBox = new System.Windows.Controls.CheckBox();
				checkBox.IsChecked = true;
				Stack.Children.Add(checkBox);
				Item.Content = Stack;
				ListBoxFileSelect.Items.Add(Item);
				indexListBox++;
			}
			catch
			{

			}
		}
		private void ResetFile(object sender, RoutedEventArgs e)
		{
			try
			{
				ListBoxFileSelect.Items.Clear();
			}
			catch
			{

			}
		}
		private void SelectFileClick(object sender, RoutedEventArgs e)
		{
			try
			{
				if (FileSelect3.IsChecked == true)
				{
					FolderBrowserDialog dialog = new FolderBrowserDialog
					{
						SelectedPath = @"C:\"
					};
					if (dialog.ShowDialog().ToString().Equals("OK"))
					{
						IEnumerable<string> enumerable = Directory.EnumerateFiles(dialog.SelectedPath, "*.tex");
						if (enumerable != null)
						{
							foreach (string str in enumerable)
							{
								ListBoxSelectFileAdd(str);
							}
						}
						else
						{
							System.Windows.MessageBox.Show("Không có file Tex nào trong thư mục", "Thoát");
						}
					}
				}
				else if (FileSelect2.IsChecked == true)
				{
					Microsoft.Win32.OpenFileDialog dialog2 = new Microsoft.Win32.OpenFileDialog()
					{
						Filter = "File Latex (*.tex)|*.tex|All files (*.*)|*.*",
						Multiselect = true,
						InitialDirectory = @"C:\"
					};
					dialog2.ShowDialog();
					foreach (string str in dialog2.FileNames)
					{
						ListBoxSelectFileAdd(str);
					}
				}
				else if (FileSelect1.IsChecked == true)
				{
					Microsoft.Win32.OpenFileDialog dialog2 = new Microsoft.Win32.OpenFileDialog()
					{
						Filter = "File Latex (*.tex)|*.tex|All files (*.*)|*.*",
						InitialDirectory = @"C:\"
					};
					dialog2.ShowDialog();
					ListBoxSelectFileAdd(dialog2.FileName);
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
		public List<string> getListPath()
		{
			List<string> list = new List<string>();
			int i = 0;
			foreach (ListBoxItem item in ListBoxFileSelect.Items)
			{
				StackPanel Stack = item.Content as StackPanel;
				System.Windows.Controls.CheckBox checkbox = Stack.Children[1] as System.Windows.Controls.CheckBox;
				if (checkbox != null && checkbox.IsChecked == true)
				{
					System.Windows.Controls.TextBox textBox = Stack.Children[0] as System.Windows.Controls.TextBox;
					list.Add(textBox.Text);
				}
				i++;
			}
			return list;
		}
		public string getTex(string path)
		{
			try
			{
				string tex = File.ReadAllText(path);
				int start = tex.IndexOf(@"\begin{document}");
				if (start > 0)
				{
					tex = tex.Substring(start + 16);
				}
				return tex;
			}
			catch
			{
				return null;
			}
		}
		public List<string> FilterId(string tex, bool? select, string exString, string btString, string vdString, bool? ex, bool? bt, bool? vd)
		{
			try
			{
				List<string> list = new List<string>();
				int startTex = tex.IndexOf(@"\begin{document}");
				if (startTex > 0)
				{
					tex = tex.Substring(startTex + 16);
				}
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
		public async System.Threading.Tasks.Task startListTexToWorditem(List<string> listPath, string exString, string btString, string vdString, bool? ex, bool? bt, bool? vd, bool? toogleTex1, bool? toogleTex2, bool? Tiz, bool? all, bool? DeleteName, bool? DeleteSchool, bool? DeleteId, string NameDuAn, bool? AddTableCheck, bool? AddFilePdf, bool? RunTexToWord, Dictionary<string, string> dic)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					for (int i = 0; i < listPath.Count; i++)
					{
						try
						{
							if (i % 5 == 0)
							{
								Process[] appprocess = Process.GetProcessesByName("MATHTYPE");
								if (appprocess != null && appprocess.Length > 0)
								{
									foreach (Process item in appprocess)
									{
										item.Kill();
									}
								}
							}
							string path = listPath[i];
							string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
							string tex = getTex(path);
							List<string> list = FilterId(tex, all, exString, btString, vdString, ex, bt, vd);
							//DateTime time = DateTime.Now;
							//string TimeName = time.ToString("h.mm.ss");
							string path2 = Directory.GetCurrentDirectory() + @"\LuuFile" + @"\" + fileName;
							var app = new Application
							{
								Visible = true
							};
							TexTo.addTextToWord(list, path2, toogleTex1, toogleTex2, Tiz, all, DeleteName, DeleteSchool, DeleteId, NameDuAn, AddTableCheck, AddFilePdf, RunTexToWord, app, dic);
							app.Quit();
							string path3 = Directory.GetCurrentDirectory() + @"\Bat";
							string path4 = Directory.GetCurrentDirectory();
							System.IO.DirectoryInfo di = new DirectoryInfo(path3);
							foreach (FileInfo file in di.GetFiles())
							{
								file.Delete();
							}
							System.Windows.Forms.MessageBoxEx.Show("Tex to word thành công file" + fileName + ", xem trong thư mục LuuFile", 2000);
						}
						catch { }
					}
					System.Windows.MessageBox.Show("Chuyển file word thành công, xem trong thư mục LuuFile", "Thành công");
				}
				catch
				{
					System.Windows.MessageBox.Show("Chuyển file không thành công", "Thành công");
				}
			});
		}
		public async System.Threading.Tasks.Task startListTexToWord(List<string> listpath, string exString, string btString, string vdString, bool? ex, bool? bt, bool? vd, bool? toogleTex1, bool? toogleTex2, bool? Tiz, bool? all, bool? DeleteName, bool? DeleteSchool, bool? DeleteId, string NameDuAn, bool? AddTableCheck, bool? AddFilePdf, bool? RunTexToWord, Dictionary<string, string> dic,int number)
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
							startListTexToWorditem(listpath, exString, btString, vdString, ex, bt, vd, toogleTex1, toogleTex2, Tiz, all, DeleteName, DeleteSchool, DeleteId, NameDuAn, AddTableCheck, AddFilePdf, RunTexToWord, dic);
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
								startListTexToWorditem(listnew, exString, btString, vdString, ex, bt, vd, toogleTex1, toogleTex2, Tiz, all, DeleteName, DeleteSchool, DeleteId, NameDuAn, AddTableCheck, AddFilePdf, RunTexToWord, dic);
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
								startListTexToWorditem(listnew, exString, btString, vdString, ex, bt, vd, toogleTex1, toogleTex2, Tiz, all, DeleteName, DeleteSchool, DeleteId, NameDuAn, AddTableCheck, AddFilePdf, RunTexToWord, dic);
							}
							catch
							{ }
						}
					}
				}
				catch
				{
				}
			});
		}
		private async void startTexToWord(object sender, RoutedEventArgs e)
		{
			List<string> listPath = getListPath();
			try
			{
				Dictionary<string, string> dic = new Dictionary<string, string>();
				dic.Add("dl", DlString.Text);
				dic.Add("hq", HqString.Text);
				dic.Add("dn", DnString.Text);
				dic.Add("nx", NxString.Text);
				dic.Add("dang", DangString.Text);
				dic.Add("cy", CyString.Text);
				int number = 3;
				if (number1.IsChecked == true) { number = 1; }
				if (number3.IsChecked == true) { number = 5; }
				if (number4.IsChecked == true) { number = 7; }
				startListTexToWord(listPath, ExString.Text, BtString.Text, VdString.Text, CauHoi.IsChecked, BaiTap.IsChecked, ViDu.IsChecked, true, false, Tiz.IsChecked, all.IsChecked, DeleteName.IsChecked, DeleteSchool.IsChecked, DeleteId.IsChecked, NameDuAn.Text, AddTableCheck.IsChecked, AddFilePdf.IsChecked, RunTexToWord.IsChecked, dic,number);
				FolderSaveFile.Text = Directory.GetCurrentDirectory() + @"\LuuFile";
				System.Windows.MessageBox.Show("Chức năng sẽ chạy theo phương thức bất đồng bộ, các thầy cô có thể thực hiện các chức năng khác trong lúc chờ chạy xong", "Thành công");
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
		private void openFolder(object sender, RoutedEventArgs e)
		{
			try
			{
				string path = FolderSaveFile.Text;
				System.Diagnostics.Process.Start(path);
			}
			catch
			{
				System.Windows.MessageBox.Show("Forder trống", "Thoát");
			}
		}
	}
}
