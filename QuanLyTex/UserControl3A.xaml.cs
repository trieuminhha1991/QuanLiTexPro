using QuanLyTex.User3Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using WpfApp1;
using Xceed.Wpf.DataGrid;


namespace QuanLyTex
{
	/// <summary>
	/// Interaction logic for UserControl3A.xaml
	/// </summary>
	public partial class UserControl3A : System.Windows.Controls.UserControl
	{

		private string appPath = "";
		private List<string> listPath = new List<string>();
		private List<string> listEcersiceY = new List<string>();
		private List<string> listEcersiceB = new List<string>();
		private List<string> listEcersiceK = new List<string>();
		private List<string> listEcersiceG = new List<string>();
		private List<string> listEcersiceT = new List<string>();
		User3MapTex classMap = new User3MapTex();
		DataGridObject2 data = new DataGridObject2();
		public UserControl3A()
		{
			InitializeComponent();
			DataContext = new User1Data();
		}
		
		private void Checked(object sender, RoutedEventArgs e)
		{
			System.Windows.Controls.CheckBox Check = (System.Windows.Controls.CheckBox)sender;
			DataGridObject2 data = DataGrid.CurrentItem as DataGridObject2;
			string st = data.CodeLevel;
			int Total = int.Parse(TotalExer2.Text);
			if (Check.IsChecked == true)
			{
				Total++;
				if(st=="Y")
				{
					int TotalY = int.Parse(TotalExerY.Text);
					TotalY++;
					TotalExerY.Text = TotalY.ToString();
				}
				if (st == "B")
				{
					int TotalB = int.Parse(TotalExerB.Text);
					TotalB++;
					TotalExerB.Text = TotalB.ToString();
				}
				if (st == "K")
				{
					int TotalK = int.Parse(TotalExerK.Text);
					TotalK++;
					TotalExerK.Text = TotalK.ToString();
				}
				if (st == "G")
				{
					int TotalG = int.Parse(TotalExerG.Text);
					TotalG++;
					TotalExerG.Text = TotalG.ToString();
				}
				if (st == "T")
				{
					int TotalT = int.Parse(TotalExerT.Text);
					TotalT++;
					TotalExerT.Text = TotalT.ToString();
				}
			}
			else
			{
				Total--;
				if (st == "Y")
				{
					int TotalY = int.Parse(TotalExerY.Text);
					TotalY--;
					TotalExerY.Text = TotalY.ToString();
				}
				if (st == "B")
				{
					int TotalB = int.Parse(TotalExerB.Text);
					TotalB--;
					TotalExerB.Text = TotalB.ToString();
				}
				if (st == "K")
				{
					int TotalK = int.Parse(TotalExerK.Text);
					TotalK--;
					TotalExerK.Text = TotalK.ToString();
				}
				if (st == "G")
				{
					int TotalG = int.Parse(TotalExerG.Text);
					TotalG--;
					TotalExerG.Text = TotalG.ToString();
				}
				if (st == "T")
				{
					int TotalT = int.Parse(TotalExerT.Text);
					TotalT--;
					TotalExerT.Text = TotalT.ToString();
				}
			}
			TotalExer2.Text = Total.ToString();
			DataGridCollectionViewSource source = GridTotal.FindResource("cvsDataGrid") as DataGridCollectionViewSource;
			DataGrid.CurrentItem = null;
		}
		private void SelectFile(object sender, RoutedEventArgs e)
		{
			try
			{
				if (FileSelect1.IsChecked == true)
				{
					FolderBrowserDialog dialog = new FolderBrowserDialog
					{
						SelectedPath = @"C:\"
					};
					if (dialog.ShowDialog().ToString() == "OK")
					{
						appPath = dialog.SelectedPath;
						IEnumerable<string> enumerable = Directory.EnumerateFiles(appPath, "*.tex");
						if (enumerable != null)
						{
							int i = listPath.Count;
							foreach (string str in enumerable)
							{

								TextBlock textBlock = new TextBlock();
								System.Windows.Controls.TextBox textBox = new System.Windows.Controls.TextBox();
								textBox.Text = System.IO.Path.GetFileName(str);
								textBox.Height = 23;
								textBox.Width = 100;
								textBox.FontSize = 12;
								if (i % 2 == 1)
								{
									textBox.Background = Brushes.AliceBlue;
								}
								textBlock.Inlines.Add(textBox);
								ListBoxFileSelect.Items.Add(textBlock);
								listPath.Add(str);
								i++;
								creatDatagrid(str);
							}
						}
						else
						{
							System.Windows.MessageBox.Show("Không có file Tex nào trong thư mục", "Thoát");
						}
					}
				}
				if (FileSelect2.IsChecked == true)
				{
					Microsoft.Win32.OpenFileDialog dialog2 = new Microsoft.Win32.OpenFileDialog()
					{
						Filter = "File Latex (*.tex)|*.tex|All files (*.*)|*.*",
						Multiselect = true,
						InitialDirectory = @"C:\"
					};
					dialog2.ShowDialog();
					if (dialog2.FileNames != null)
					{
						int i = listPath.Count;
						foreach (string str in dialog2.FileNames)
						{
							TextBlock textBlock = new TextBlock();
							textBlock.Name = "TextBlock" + i;
							System.Windows.Controls.TextBox textBox = new System.Windows.Controls.TextBox();
							textBox.Text = System.IO.Path.GetFileName(str);
							textBox.Height = 23;
							textBox.Width = 200;
							textBox.FontSize = 12;
							if (i % 2 == 1)
							{
								textBox.Background = Brushes.AliceBlue;
							}
							textBlock.Inlines.Add(textBox);
							ListBoxFileSelect.Items.Add(textBlock);
							listPath.Add(str);
							i++;
							creatDatagrid(str);
						}
					}
					else
					{
						System.Windows.MessageBox.Show("Không có file Tex nào được chọn", "Thoát");
					}
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
		private void creatDatagrid(string path)
		{
			try
			{
				string fileName= System.IO.Path.GetFileName(path);
				DataGridCollectionViewSource source = GridTotal.FindResource("cvsDataGrid") as DataGridCollectionViewSource;
				string str = "";
				string str1, str2, str3, str4, str5, srt6;
				if (BoxClass.SelectedValue != "")
				{
					str1 = "[" + BoxClass.SelectedValue.Replace(BoxClass.Delimiter, "") + "]";
				}
				else
				{
					str1 = @"[0-9]";
				};
				if (BoxSubject.SelectedValue != "")
				{
					str2 = "[" + BoxSubject.SelectedValue.Replace(BoxSubject.Delimiter, "") + "]";
				}
				else
				{
					str2 = @"[DH]";
				};
				if (BoxChapter.SelectedValue != "")
				{
					str3 = "[" + BoxChapter.SelectedValue.Replace(BoxChapter.Delimiter, "") + "]";
				}
				else
				{
					str3 = @"[0-9]";
				};
				str4 = @"[GKBYT]";
				if (BoxSection.SelectedValue != "")
				{
					str5 = "[" + BoxSection.SelectedValue.Replace(BoxSection.Delimiter, "") + "]";
				}
				else
				{
					str5 = @"[0-9]";
				};
				srt6 = @"[0-9]";

				if (selectId5.IsChecked == true)
				{
					str = @"\[" + str1 + str2 + str3 + str4 + str5;
				}
				if (selectId6.IsChecked == true)
				{
					str = @"\[" + str1 + str2 + str3 + str4 + str5 + "-" + srt6 + "]";
				}
				string type = "ex";
				Regex rx = new Regex(str);
				var mapTex = classMap.FilterId(path, type, rx);
				var listData = new List<DataGridObject2>();
				foreach (var item in mapTex)
				{
					data.All= item["all"];
					data.FileName = fileName;
					data.Ecersice = item["exersice"];
					var CodeId = item["codeId"];
					if (CodeId != "")
					{
						data.CodeId = CodeId;
						data.CodeLevel = item["codeLevel"];
						data.CodeName = item["codeName"];
						data.IsSelected = false;
						//data.CodeLevel= item["codeLevel"];
					}
					if (source.Source != null)
					{
						DataGridCollectionViewBase sourceView = (DataGridCollectionViewBase)DataGrid.ItemsSource;
						sourceView.AddNew();
					}
					listData.Add(data);
					data = new DataGridObject2();
				}
				if (source.Source == null)
				{
					source.Source = listData;
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
		private void CreatingNewItem(object sender, DataGridCreatingNewItemEventArgs e)
		{
			e.NewItem = data;
			e.Handled = true;
		}
		private void CommittingNewItem(object sender, DataGridCommittingNewItemEventArgs e)
		{
			List<DataGridObject2> source = e.CollectionView.SourceCollection as List<DataGridObject2>;
			source.Add((DataGridObject2)e.Item);
			// the new item is always added at the end of the list.     
			e.Index = source.Count - 1;
			e.NewCount = source.Count;
			e.Handled = true;
		}
		private void CancelingNewItem(object sender, DataGridItemHandledEventArgs e)
		{
			e.Handled = true;
		}
		private bool checkTexBox()
		{
			bool check = true;
			int total = int.Parse(NumberY.Text) + int.Parse(NumberB.Text) + int.Parse(NumberK.Text) + int.Parse(NumberG.Text) + int.Parse(NumberT.Text);
			if (int.Parse(NumberExer.Text) < total)
			{
				check = false;
				System.Windows.MessageBox.Show("Tổng số câu mức độ là"+ total +"nhỏ hơn số câu cần trong đề", "Thoát");
			}
			if (int.Parse(NumberExer.Text)< int.Parse(TotalExer2.Text))
			{
				check = false;
				System.Windows.MessageBox.Show("Số lượng câu đã chọn nhỏ hơn số lượng câu trong đề", "Thoát");
			}
			if(int.Parse(NumberY.Text)< int.Parse(TotalExerY.Text))
			{
				check = false;
				System.Windows.MessageBox.Show("Số lượng câu 'Y' đã chọn nhỏ hơn số lượng câu 'Y' cần", "Thoát");
			}
			if (int.Parse(NumberB.Text) < int.Parse(TotalExerB.Text))
			{
				check = false;
				System.Windows.MessageBox.Show("Số lượng câu 'B' đã chọn không bằng số lượng câu 'B' cần", "Thoát");
			}
			if (int.Parse(NumberK.Text) < int.Parse(TotalExerK.Text))
			{
				check = false;
				System.Windows.MessageBox.Show("Số lượng câu 'K' đã chọn nhỏ hơn số lượng câu 'K' cần", "Thoát");
			}
			if (int.Parse(NumberG.Text) < int.Parse(TotalExerG.Text))
			{
				check = false;
				System.Windows.MessageBox.Show("Số lượng câu 'G' đã chọn nhỏ hơn số lượng câu 'G' cần", "Thoát");
			}
			if (int.Parse(NumberT.Text) < int.Parse(TotalExerT.Text))
			{
				check = false;
				System.Windows.MessageBox.Show("Số lượng câu 'T' đã chọn nhỏ hơn số lượng câu 'T' cần", "Thoát");
			}
			return check;
		}
		private void SelectEcersiceAll()
		{
			try
			{
				listEcersiceY = new List<string>();
				listEcersiceB = new List<string>();
				listEcersiceK = new List<string>();
				listEcersiceG = new List<string>();
				listEcersiceT = new List<string>();
				DataGridCollectionViewSource source = GridTotal.FindResource("cvsDataGrid") as DataGridCollectionViewSource;
				List<DataGridObject2> list = source.Source as List<DataGridObject2>;
				foreach (var item in list)
				{
					if (item.IsSelected == true)
					{
						if (item.CodeLevel == "T")
						{
							listEcersiceT.Add(item.All);
						}
						if (item.CodeLevel == "G")
						{
							listEcersiceG.Add(item.All);
						}
						if (item.CodeLevel == "K")
						{
							listEcersiceK.Add(item.All);
						}
						if (item.CodeLevel == "B")
						{
							listEcersiceB.Add(item.All);
						}
						if (item.CodeLevel == "Y")
						{
							listEcersiceY.Add(item.All);
						}
					}
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}
		private void CreatExam(object sender, RoutedEventArgs e)
		{
			try
			{
				bool check = checkTexBox();
				if (check == true)
				{
					SelectEcersiceAll();
					CreatExamTex creatExam = new CreatExamTex();
					int numberExam = (int)NumberExam.Value;
					var listExam = creatExam.mixExamTex(listEcersiceY, listEcersiceB, listEcersiceK, listEcersiceG, listEcersiceT, numberExam,FormReversed.IsChecked, int.Parse(TotalExerY.Text), int.Parse(TotalExerB.Text), int.Parse(TotalExerK.Text), int.Parse(TotalExerG.Text), int.Parse(TotalExerT.Text));
					string path = Directory.GetCurrentDirectory() + @"\Exam";
					string pathFolder = creatExam.newFileTex(listExam, path);
					SaveFile.Text = pathFolder;
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}

		private void ViewFolder(object sender, RoutedEventArgs e)
		{

			System.Diagnostics.Process.Start(SaveFile.Text);
		}
	}
}
