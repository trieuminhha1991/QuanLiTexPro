using Microsoft.Office.Interop.Word;
using QuanLyTex.User2Class;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Application = Microsoft.Office.Interop.Word.Application;
using Orientation = System.Windows.Controls.Orientation;

namespace QuanLyTex
{
	/// <summary>
	/// Interaction logic for UserControl2.xaml
	/// </summary>
	public partial class UserControl2 : System.Windows.Controls.UserControl
	{
		Application app;
		int indexListBox;
		AcycnUser2 user = new AcycnUser2();
		bool check = false;
		public UserControl2()
		{
			
				InitializeComponent();
		}
		private void OpenWord(object sender, RoutedEventArgs e)
		{
			try
			{
				if (app == null)
				{
					app = new Application();
					app.Visible = true;
				}
				Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
				{
					Filter = "File document (*.doc;*.docx)|*.doc;*.docx|All files (*.*)|*.*",
					InitialDirectory = @"C:\"
				};
				dialog.ShowDialog();
				app.Documents.Open(dialog.FileName);
			}
			catch { }
		}
		private void OpenWords(object sender, RoutedEventArgs e)
		{
			try
			{
				if (app == null)
				{
					app = new Application();
					app.Visible = true;
				}
				Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
				{
					Filter = "File document (*.doc;*.docx)|*.doc;*.docx|All files (*.*)|*.*",
					Multiselect = true,
					InitialDirectory = @"C:\"
				};
				dialog.ShowDialog();
				foreach (string str in dialog.FileNames)
				{
					app.Documents.Open(str);
				}
			}
			catch { }
		}
		private void CloseAllWord(object sender, RoutedEventArgs e)
		{
			try
			{
				Process[] app = Process.GetProcessesByName("WINWORD");
				if (app != null && app.Length > 0)
				{
					foreach (Process item in app)
					{
						item.Kill();
					}
				}
			}
			catch { }
		}
		private void MaterialButton_Click_11(object sender, RoutedEventArgs e)
		{
			try
			{
				Process app = Process.GetCurrentProcess();
				app.Kill();
			}
			catch
			{

			}
		}
		private void MaterialButton_Click_3(object sender, RoutedEventArgs e)
		{
			try
			{
				Process[] app = Process.GetProcessesByName("MATHTYPE");
				if (app != null && app.Length > 0)
				{
					foreach (Process item in app)
					{
						item.Kill();
					}
				}
			}
			catch { }
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
				System.Windows.Controls.CheckBox checkBox = new System.Windows.Controls.CheckBox();
				checkBox.IsChecked = true;
				Stack.Children.Add(checkBox);
				Stack.Children.Add(textBox);
				Item.Content = Stack;
				ListBoxFileSelect.Items.Add(Item);
				indexListBox++;
			}
			catch
			{

			}
		}
		
		private void ListBoxSelectFileAddC(string path)
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
				System.Windows.Controls.CheckBox checkBox = new System.Windows.Controls.CheckBox();
				checkBox.IsChecked = true;
				Stack.Children.Add(checkBox);
				Stack.Children.Add(textBox);
				Item.Content = Stack;
				ListBoxFileSelectC.Items.Add(Item);
				indexListBox++;
			}
			catch
			{

			}
		}
		
		private void SelectFilePdf(object sender, RoutedEventArgs e)
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
						IEnumerable<string> enumerable = Directory.EnumerateFiles(dialog.SelectedPath, "*.docx");
						IEnumerable<string> enumerable2 = Directory.EnumerateFiles(dialog.SelectedPath, "*.doc");
						if (enumerable != null)
						{
							foreach (string str in enumerable)
							{
								ListBoxSelectFileAdd(str);
							}
						}
						if (enumerable2 != null)
						{
							foreach (string str in enumerable2)
							{
								ListBoxSelectFileAdd(str);
							}
						}
						if(enumerable == null&& enumerable2 == null)
						{
							System.Windows.MessageBox.Show("Không có file document nào trong thư mục", "Thoát");
						}
					}
				}
				if (FileSelect2.IsChecked == true)
				{
					Microsoft.Win32.OpenFileDialog dialog2 = new Microsoft.Win32.OpenFileDialog()
					{
						Filter = "File document (*.doc;*.docx)|*.doc;*.docx|All files (*.*)|*.*",
						Multiselect = true,
						InitialDirectory = @"C:\"
					};
					dialog2.ShowDialog();
					foreach (string str in dialog2.FileNames)
					{
						ListBoxSelectFileAdd(str);
					}
				}
				if (FileSelect1.IsChecked == true)
				{
					Microsoft.Win32.OpenFileDialog dialog2 = new Microsoft.Win32.OpenFileDialog()
					{
						Filter = "File document (*.doc;*.docx)|*.doc;*.docx|All files (*.*)|*.*",
						InitialDirectory = @"C:\"
					};
					dialog2.ShowDialog();
					ListBoxSelectFileAdd(dialog2.FileName);
				}
			}catch
			{

			}
		}
	
		private void SelectFileTool(object sender, RoutedEventArgs e)
		{
			try
			{
				if (FileSelect3C.IsChecked == true)
				{
					FolderBrowserDialog dialog = new FolderBrowserDialog
					{
						SelectedPath = @"C:\"
					};
					if (dialog.ShowDialog().ToString().Equals("OK"))
					{
						IEnumerable<string> enumerable = Directory.EnumerateFiles(dialog.SelectedPath, "*.docx");
						IEnumerable<string> enumerable2 = Directory.EnumerateFiles(dialog.SelectedPath, "*.doc");
						if (enumerable != null)
						{
							foreach (string str in enumerable)
							{
								ListBoxSelectFileAddC(str);
							}
						}
						if (enumerable2 != null)
						{
							foreach (string str in enumerable2)
							{
								ListBoxSelectFileAddC(str);
							}
						}
						if (enumerable == null && enumerable2 == null)
						{
							System.Windows.MessageBox.Show("Không có file document nào trong thư mục", "Thoát");
						}
					}
				}
				if (FileSelect2C.IsChecked == true)
				{
					Microsoft.Win32.OpenFileDialog dialog2 = new Microsoft.Win32.OpenFileDialog()
					{
						Filter = "File document (*.doc;*.docx)|*.doc;*.docx|All files (*.*)|*.*",
						Multiselect = true,
						InitialDirectory = @"C:\"
					};
					dialog2.ShowDialog();
					foreach (string str in dialog2.FileNames)
					{
						ListBoxSelectFileAddC(str);
					}
				}
				if (FileSelect1C.IsChecked == true)
				{
					Microsoft.Win32.OpenFileDialog dialog2 = new Microsoft.Win32.OpenFileDialog()
					{
						Filter = "File document (*.doc;*.docx)|*.doc;*.docx|All files (*.*)|*.*",
						InitialDirectory = @"C:\"
					};
					dialog2.ShowDialog();
					ListBoxSelectFileAddC(dialog2.FileName);
				}
			}
			catch
			{

			}
		}
		private void CreatPdf(object sender, RoutedEventArgs e)
		{
			try
			{
				List<string> list = new List<string>();
				int i = 0;
				foreach (ListBoxItem item in ListBoxFileSelect.Items)
				{
					StackPanel Stack = item.Content as StackPanel;
					System.Windows.Controls.CheckBox checkbox = Stack.Children[0] as System.Windows.Controls.CheckBox;
					if (checkbox != null && checkbox.IsChecked == true)
					{
						System.Windows.Controls.TextBox textBox = Stack.Children[1] as System.Windows.Controls.TextBox;
						list.Add(textBox.Text);
					}
					
				}
				string path = Directory.GetCurrentDirectory() + @"\LuuFile";
				if (CreatPdfCheck.IsChecked == true&&ConfigurationManager.AppSettings["A"] == "1")
				{
						user.CreatPdf(list,path);
				}
				if(MatchFile.IsChecked==true && ConfigurationManager.AppSettings["A"] == "1")
				{
					user.MatchFile(list, path);
				}
				if (ConfigurationManager.AppSettings["A"] == "0")
				{
					System.Windows.MessageBox.Show("Chưa đăng kí bản quyền", "Thoát");
				}
				FolderSaveFile2A.Text = path;
			}catch
			{

			}
		}
		private void ToolWord(object sender, RoutedEventArgs e)
		{
			try
			{
				List<string> list = new List<string>();
				int i = 0;
				foreach (ListBoxItem item in ListBoxFileSelectC.Items)
				{
					StackPanel Stack = item.Content as StackPanel;
					System.Windows.Controls.CheckBox checkbox = Stack.Children[0] as System.Windows.Controls.CheckBox;
					if (checkbox != null && checkbox.IsChecked == true)
					{
						System.Windows.Controls.TextBox textBox = Stack.Children[1] as System.Windows.Controls.TextBox;
						list.Add(textBox.Text);
					}
					
				}
				string path = Directory.GetCurrentDirectory() + @"\LuuFile";
				List<string> liststr = new List<string>();
				if (CauHoi.IsChecked == true) { liststr.Add(ExString.Text); }
				if (BaiTap.IsChecked == true) { liststr.Add(BtString.Text); }
				if (Vidu.IsChecked == true) { liststr.Add(VdString.Text); }
				int number = 3;
				if (number1.IsChecked == true) { number = 1; }
				if (number3.IsChecked == true) { number = 5; }
				if (number4.IsChecked == true) { number = 7; }
				if (Question.IsChecked == true && ConfigurationManager.AppSettings["A"] == "1")
				{
					user.Question(list, liststr, path, ColorOne.IsChecked, BoldOne.IsChecked, ItalicOne.IsChecked, StartProof.Text, AddPdf.IsChecked,number);
				}
				if (Proof.IsChecked == true && ConfigurationManager.AppSettings["A"] == "1")
				{
					user.Proof(list,liststr, path, ColorOne.IsChecked, BoldOne.IsChecked, ItalicOne.IsChecked, StartProof.Text, AddPdf.IsChecked, number);
				}
				if (TableCheck.IsChecked == true && ConfigurationManager.AppSettings["A"] == "1")
				{
					user.CreatTable(list,liststr, path, ColorOne.IsChecked, BoldOne.IsChecked, ItalicOne.IsChecked, StartProof.Text, UnderLineTwo.IsChecked, ColorTwo.IsChecked, HghtlightTwo.IsChecked, number);
				}
				if (BTN.IsChecked == true && ConfigurationManager.AppSettings["A"] == "1")
				{
					user.BTNform(list,liststr, path, ColorOne.IsChecked, BoldOne.IsChecked, ItalicOne.IsChecked, StartProof.Text, number);
				}
				if (ConfigurationManager.AppSettings["A"] == "0")
				{
					System.Windows.MessageBox.Show("Chưa đăng kí bản quyền", "Thoát");
				}
				FolderSaveFileC.Text = path;
			}
			catch { }
		}
		private void MaterialButton_Click_6(object sender, RoutedEventArgs e)
		{
			try
			{
				string path = FolderSaveFile2A.Text;
				System.Diagnostics.Process.Start(path);
			}
			catch
			{
				System.Windows.MessageBox.Show("Forder trống", "Thoát");
			}
		}
		private void MaterialButton_Click_8(object sender, RoutedEventArgs e)
		{
			try
			{
				string path = FolderSaveFileC.Text;
				System.Diagnostics.Process.Start(path);
			}
			catch
			{
				System.Windows.MessageBox.Show("Forder trống", "Thoát");
			}
		}

		

		private void FormSelect(object sender, RoutedEventArgs e)
		{
			try
			{
				List<string> list = new List<string>();
				foreach (ListBoxItem item in ListBoxFileSelectC.Items)
				{
					StackPanel Stack = item.Content as StackPanel;
					System.Windows.Controls.CheckBox checkbox = Stack.Children[0] as System.Windows.Controls.CheckBox;
					if (checkbox != null && checkbox.IsChecked == true)
					{
						System.Windows.Controls.TextBox textBox = Stack.Children[1] as System.Windows.Controls.TextBox;
						list.Add(textBox.Text);
					}
					
				}
				string path = Directory.GetCurrentDirectory() + @"\LuuFile";
				int number = 3;
				if (number1.IsChecked == true) { number = 1; }
				if (number3.IsChecked == true) { number = 5; }
				if (number4.IsChecked == true) { number = 7; }
				if (AddPage1.IsChecked==true && ConfigurationManager.AppSettings["A"] == "1")
				{
					user.AddPage(list, path, FilePage.Text,AddPdf.IsChecked,number);
				}
				if (HeaderFooter.IsChecked == true && ConfigurationManager.AppSettings["A"] == "1")
				{
					user.AddHeaderFooter(list, path, FilePage.Text, AddPdf.IsChecked, number,HeaderLeft.Text,FooterLeft.Text);
				}
				if (ConfigurationManager.AppSettings["A"] == "0")
				{
					System.Windows.MessageBox.Show("Chưa đăng kí bản quyền", "Thoát");
				}
				FolderSaveFileB.Text = path;
			}
			catch { }
		}
		private void MaterialButton_Click_9(object sender, RoutedEventArgs e)
		{
			try
			{
				Microsoft.Win32.OpenFileDialog dialog2 = new Microsoft.Win32.OpenFileDialog()
				{
					Filter = "File Document (*.docx)|*.docx;*.doc|All files (*.*)|*.*",
					InitialDirectory = @"C:\"
				};
				dialog2.ShowDialog();
				FilePage.Text = dialog2.FileName;
			}
			catch { }
		}

		private void MaterialButton_Click_10(object sender, RoutedEventArgs e)
		{
			try
			{
				string path = FolderSaveFileB.Text;
				System.Diagnostics.Process.Start(path);
			}
			catch
			{
				System.Windows.MessageBox.Show("Forder trống", "Thoát");
			}
		}

		private void MaterialButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				ListBoxFileSelect.Items.Clear();
			}
			catch
			{

			}
		}

		private void MaterialButton_Click_2(object sender, RoutedEventArgs e)
		{
			try
			{
				ListBoxFileSelectC.Items.Clear();
			}
			catch
			{

			}
		}
	}
}
