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
    public partial class UserControl5B : UserControl
    {
		string appPath = Directory.GetCurrentDirectory();
		int indexListBox = 0;
		WordToTex change = new WordToTex();
		public UserControl5B()
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
				if (checkbox != null&&checkbox.IsChecked == true )
				{
					System.Windows.Controls.TextBox textBox = Stack.Children[0] as System.Windows.Controls.TextBox;
					list.Add(textBox.Text);
				}
				i++;
			}
			return list;
		}
		private void startWordToTex(object sender, RoutedEventArgs e)
		{
			try
			{
				List<string> listPath = getListPath();
				foreach (string path in listPath)
				{
					try
					{
						string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
						DateTime time = DateTime.Now;
						string TimeName = time.ToString("h.mm.ss");
						string pathTex = Directory.GetCurrentDirectory() + @"\LuuFile" + @"\" + fileName + TimeName + @".tex";
						string pathDoc = Directory.GetCurrentDirectory() + @"\LuuFile" + @"\" + fileName + TimeName + @".docx";
						string pathImage = Directory.GetCurrentDirectory() + @"\LuuFile" + @"\image" + TimeName;
						string pathFooter = Directory.GetCurrentDirectory() + @"\MauFile\Footer.tex";
						Directory.CreateDirectory(pathImage);
						List<string> liststr = new List<string>();
						if (CauHoi.IsChecked == true) { liststr.Add(ExString.Text); }
						if (BaiTap.IsChecked == true) { liststr.Add(BtString.Text); }
						if (Vidu.IsChecked == true) { liststr.Add(VdString.Text); }
						if (ConfigurationManager.AppSettings["A"] != "0")
						{ 
							change.startWordToTex(pathFooter, FormFile.Text, path, pathTex, pathDoc, pathImage, StartProof.Text, liststr, All.IsChecked, HevaHoac.IsChecked, Tiz.IsChecked, ColorOne.IsChecked, BoldOne.IsChecked, ItalicOne.IsChecked, UnderLineTwo.IsChecked, HghtlightTwo.IsChecked, ColorTwo.IsChecked, ColorThree.IsChecked, RunTexToWord.IsChecked);
							FolderSaveFile.Text = Directory.GetCurrentDirectory() + @"\LuuFile";
						}
						if (ConfigurationManager.AppSettings["A"] == "0")
						{
							System.Windows.MessageBox.Show("Chưa đăng kí bản quyền", "Thoát");
						}
					}
					catch
					{

					}
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
