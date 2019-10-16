using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using WpfApp1;
using Orientation = System.Windows.Controls.Orientation;
using UserControl = System.Windows.Controls.UserControl;
using Application = Microsoft.Office.Interop.Word.Application;
using System.Configuration;

namespace QuanLyTex
{
	/// <summary>
	/// Interaction logic for UserControl15B.xaml
	/// </summary>
	public partial class UserControl5D : UserControl
	{
		string appPath = Directory.GetCurrentDirectory();
		int indexListBox = 0;

		public UserControl5D()
		{
			InitializeComponent();
			FormFile.Text = appPath + @"\MauFile\MacDinh\ChuyenDe.tex";
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
						if (enumerable != null && enumerable2 != null)
						{
							System.Windows.MessageBox.Show("Không có file document nào trong thư mục", "Thoát");
						}
					}
				}
				else if (FileSelect2.IsChecked == true)
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
				else if (FileSelect1.IsChecked == true)
				{
					Microsoft.Win32.OpenFileDialog dialog2 = new Microsoft.Win32.OpenFileDialog()
					{
						Filter = "File docx (*.doc;*.docx)|*.doc;*.docx|All files (*.*)|*.*",
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
		public async void startListTexToWord(string pathFooter, string FormFile, List<string> listpath, string apppath, string StartProof, List<string> liststr, bool? All, bool? HevaHoac, bool? ColorOne, bool? BoldOne, bool? ItalicOne, bool? UnderLineTwo, bool? HghtlightTwo, bool? ColorTwo, bool? ColorThree, bool? RunTexToWord)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				foreach (string path in listpath)
				{
					try
					{
						string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
						//DateTime time = DateTime.Now;
						//string TimeName = time.ToString("h.mm.ss");
						string pathTex = Directory.GetCurrentDirectory() + @"\LuuFile" + @"\" + fileName + @".tex";
						string pathDoc = Directory.GetCurrentDirectory() + @"\LuuFile" + @"\" + fileName + @".docx";
						var app = new Application();
						app.Visible = true;
						WordToTex change = new WordToTex();
						change.startWordToTex(app, pathFooter, FormFile, path, pathTex, pathDoc, StartProof, liststr, All, HevaHoac, ColorOne, BoldOne, ItalicOne, UnderLineTwo, HghtlightTwo, ColorTwo, ColorThree, RunTexToWord);
						FolderSaveFile.Text = Directory.GetCurrentDirectory() + @"\LuuFile";
						System.Windows.Forms.MessageBoxEx.Show("Chuyển thành công file" + fileName + ", file được lưu trong thư mục LuuFile", 2000);
						app.Quit();
					}
					catch
					{

					}
				}
			});
		}
		private void startWordToTex(object sender, RoutedEventArgs e)
		{
			try
			{
				List<string> listPath = getListPath();
				try
				{
					string pathFooter = Directory.GetCurrentDirectory() + @"\MauFile\Footer.tex";
					List<string> liststr = new List<string>();
					if (CauHoi.IsChecked == true) { liststr.Add(ExString.Text); }
					if (BaiTap.IsChecked == true) { liststr.Add(BtString.Text); }
					if (Vidu.IsChecked == true) { liststr.Add(VdString.Text); }
					startListTexToWord(pathFooter, FormFile.Text, listPath, Directory.GetCurrentDirectory(), StartProof.Text, liststr, All.IsChecked, HevaHoac.IsChecked, ColorOne.IsChecked, BoldOne.IsChecked, ItalicOne.IsChecked, UnderLineTwo.IsChecked, HghtlightTwo.IsChecked, ColorTwo.IsChecked, ColorThree.IsChecked, RunTexToWord.IsChecked);
					FolderSaveFile.Text = Directory.GetCurrentDirectory() + @"\LuuFile";
					System.Windows.MessageBox.Show("Chức nang chạy bất đồng bộ, trong thời gian chờ đợi, thầy cô có thể thực hiện các chức năng khác", "Thoát");
				}
				catch
				{

				}
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
	}
}
