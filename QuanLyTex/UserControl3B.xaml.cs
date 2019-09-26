using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace QuanLyTex
{
	/// <summary>
	/// Interaction logic for UserControl3B.xaml
	/// </summary>
	public partial class UserControl3B : System.Windows.Controls.UserControl
	{
		UserControl3A user = new UserControl3A();
		List<string> listPath = new List<string>();
		public UserControl3B()
		{
			InitializeComponent();
			SaveFile.Text = user.SaveFile.Text;
			if (SaveFile.Text != ""&& SaveFile.Text!=null)
			{
				IEnumerable<string> enumerable = Directory.EnumerateFiles(SaveFile.Text, "*.tex");
				foreach (string str in enumerable)
				{
					AddItemListBox(str);
				}
			}
		}
		private void  AddItemListBox(string str)
		{
			string text = System.IO.Path.GetFileName(str);
			ListBoxItem Item = new ListBoxItem();
			StackPanel Stack = new StackPanel();
			Stack.Orientation = System.Windows.Controls.Orientation.Horizontal;
			System.Windows.Controls.TextBlock textBox = new System.Windows.Controls.TextBlock();
			textBox.Text = str;
			textBox.Height = 23;
			textBox.Width = 500;
			textBox.FontSize = 12;
			Stack.Children.Add(textBox);
			System.Windows.Controls.Button button = new System.Windows.Controls.Button();
			button.Background = Brushes.AliceBlue;
			button.Height = 23;
			button.Width = 40;
			button.Content = "Mã đề";
			System.Windows.Controls.TextBox texBox = new System.Windows.Controls.TextBox();
			texBox.Height = 23;
			texBox.Width = 50;
			texBox.Text = text;
			texBox.Background = Brushes.Blue;
			Stack.Children.Add(button);
			Stack.Children.Add(texBox);
			Item.Content = Stack;
			ListBoxFileSelect.Items.Add(Item);
		}
		private void SelectFile(object sender, RoutedEventArgs e)
		{
			try
			{
				FolderBrowserDialog dialog = new FolderBrowserDialog
				{
					SelectedPath = @"C:\"
				};
				if (dialog.ShowDialog().ToString() == "OK")
				{
					ListBoxFileSelect = new System.Windows.Controls.ListBox();
					string appPath = dialog.SelectedPath;
					IEnumerable<string> enumerable = Directory.EnumerateFiles(appPath, "*.tex");
					if (enumerable != null)
					{
						foreach (string str in enumerable)
						{
							AddItemListBox(str);
						}
					}
					else
					{
						System.Windows.MessageBox.Show("Không có file Tex nào trong thư mục", "Thoát");
					}
				}
			}
			catch (Exception a)
			{
				System.Windows.MessageBox.Show(a.Message, "Thoát");
			}
		}

		private void Form2_Checked(object sender, RoutedEventArgs e)
		{

		}
		private void SeeForm(object sender, RoutedEventArgs e)
		{

		}
		private void EditForm(object sender, RoutedEventArgs e)
		{

		}
	}
}
