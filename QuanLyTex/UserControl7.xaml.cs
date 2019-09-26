using QuanLyTex.User3Class;
using QuanLyTex.User7Class;
using System;
using System.IO;
using System.Windows;


namespace QuanLyTex
{
	/// <summary>
	/// Interaction logic for UserControl3A.xaml
	/// </summary>
	public partial class UserControl7 : System.Windows.Controls.UserControl
	{
		User3MapTex classMap = new User3MapTex();
		DataGridObject2 data = new DataGridObject2();
		public UserControl7()
		{
			InitializeComponent();
		}
		private void SelectFile(object sender, RoutedEventArgs e)
		{
			try
			{
				if (FileSelect2.IsChecked == true)
				{
					Microsoft.Win32.OpenFileDialog dialog2 = new Microsoft.Win32.OpenFileDialog()
					{
						Filter = "File Latex (*.docx)|*.docx|All files (*.*)|*.*",
						InitialDirectory = @"C:\"
					};
					FileSelect.Text = dialog2.FileName;
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}		
		
		private void creatExam(object sender, RoutedEventArgs e)
		{
			try
			{
				int Id = 0;
				if (Id5.IsChecked == true) { Id = 5; }
				if (Id6.IsChecked == true) { Id = 6; }
				int LocationId = 1;
				if (Location2.IsChecked == true) { LocationId = 2; }
				DateTime time = DateTime.Now;
				string TimeName = time.ToString("h.mm.ss");
				string path = Directory.GetCurrentDirectory() + @"\LuuFile" + @"\" + TimeName;
				Directory.CreateDirectory(path);
				CreatExamWord creat = new CreatExamWord();
				creat.CreatExam(NumberExer.Text,NumberExam.Text,FileSelect.Text, path, DevideLevel.IsChecked, Form.IsChecked, Matrix.IsChecked, Header.IsChecked, ExString.Text, ProofString.Text,Id,LocationId, ColorOne.IsChecked, BoldOne.IsChecked, ItalicOne.IsChecked, ColorThree.IsChecked, UnderLineTwo.IsChecked, ColorTwo.IsChecked, HghtlightTwo.IsChecked);
				SaveFile.Text = Directory.GetCurrentDirectory() + @"\LuuFile" + @"\" + TimeName;
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
