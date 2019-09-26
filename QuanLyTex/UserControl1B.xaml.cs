using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.DataGrid;

namespace QuanLyTex
{
	public class Items
	{
		public int FormId { get; set; }
		public string FormName { get; set; }
	}
	/// <summary>
	/// Interaction logic for UserControl1B.xaml
	/// </summary>
	public partial class UserControl1B : UserControl
	{
		public class Form
		{
			public int FormId { get; set; }
			public string FormName { get; set; }
		}
		public class Section
		{
			public int SectionId { get; set; }
			public string SectionName { get; set; }
		}
		public class Chapter
		{
			public int ChapterId { get; set; }
			public char ObjectName { get; set; }
			public string ChapterName { get; set; }

		}
		public class Exercise
		{
			public string Question { get; set; }
			public string Choice { get; set; }
			public string Id { get; set; }
		}
		string appPath = Directory.GetCurrentDirectory();
		String tex = "";
		string Type = "";
		public UserControl1B()
		{
			InitializeComponent();
		}

		private void SelecFile(object sender, RoutedEventArgs e)
		{
			try
			{
				FileSelect.Text = null;
				Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
				{
					Filter = "File Latex (*.tex)|*.tex|All files (*.*)|*.*",
					InitialDirectory = @"C:\"
				};
				dialog.ShowDialog();
				FileSelect.Text = dialog.FileName;
				tex = File.ReadAllText(dialog.FileName);
				if (ExCheck.IsChecked == true)
				{
					Type = ExString.Text;
				}
				if (BtCheck.IsChecked == true)
				{
					Type = BtString.Text;
				}
				if (VdCheck.IsChecked == true)
				{
					Type = VdString.Text;
				}
				List<string> list = new List<string>(); ;
				int i = 0;
				string str = @"\begin{" + Type + "}";
				string str2 = @"\end{" + Type + "}";
				int startIndex = 0;
				while (tex.IndexOf(str, startIndex)>=0)
				{
					string add = "%AddId" + i;
					startIndex = tex.IndexOf(str, startIndex);
					int endIndex = tex.IndexOf(str2, startIndex);
					string input = tex.Substring(startIndex, endIndex + str2.Length - startIndex);
					tex = tex.Insert(startIndex+str.Length, add);
					list.Add(input);
					startIndex = endIndex + add.Length;
					i++;
				}
				List<Exercise> list2 = new List<Exercise>();
				foreach (var item in list)
				{
					Exercise itemExer = FilterId(item);
					list2.Add(itemExer);
				}
				DataGrid.ItemsSource = list2;
			}
			catch
			{

			}
		}
		public Exercise FilterId(string tex)
		{
			try
			{
				Exercise exer = new Exercise();
				string st1 = "", st2 = "", st3 = "";
				int index1 = tex.IndexOf(@"\choice");
				int index2 = tex.IndexOf(@"\loigiai");
				if (index1 > 0 && index2 > 0)
				{
					exer.Question = tex.Substring(0, index1);
					exer.Choice = tex.Substring(index1, index2- index1);
				}
				if (index1 > 0 && index2 <= 0)
				{
					exer.Question = tex.Substring(0, index1);
					exer.Choice = tex.Substring(index1);
				}
				if (index1 < 0 && index2 > 0)
				{
					exer.Question = tex.Substring(0, index2);
					exer.Choice = "Tự luận";
				}
				if (index1 <= 0 && index2 <= 0)
				{
					exer.Question = tex.Substring(0, index2);
				}
				return exer;
			}
			catch (Exception e)
			{
				Exercise exer = new Exercise();
				exer.Question = tex;
				return exer;
			}
		}
		private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DataGrid.SelectedItem = DataGrid.CurrentItem;
			Popup1B.Show();
			if (Id6.IsChecked==true)
			{
				OrderBox.Visibility = Visibility.Visible;
			}
		}
		private void MaterialButton_Click(object sender, RoutedEventArgs e)
		{
			DataGrid.SelectedItem = null;
			DataGrid.CurrentItem = null;
		}
		

		private void ChapterName_DropDownOpened(object sender, EventArgs e)
		{
			try
			{
				ChapterName.Items.Clear();
				ComboBoxItem classitem = ClassName.SelectedItem as ComboBoxItem;
				string classname = classitem.Content.ToString();
				char classId = classname[0];
				ComboBoxItem ojectitem = OjectName.SelectedItem as ComboBoxItem;
				string ojectname = ojectitem.Content.ToString();
				char ojectId = ojectname[0];
				string path3 = appPath + @"\Id\Lop\Class1" + classId + @".json";
				string json3 = File.ReadAllText(path3);
				List<Chapter> chapter = JsonConvert.DeserializeObject<List<Chapter>>(json3);
				foreach (var item in chapter)
				{
					if (item.ObjectName == ojectId)
					{
						ComboBoxItem comboitem = new ComboBoxItem();
						comboitem.Content = item.ChapterId + "-" + item.ChapterName;
						ChapterName.Items.Add(comboitem);
					}
				}
			}
			catch
			{

			}
		}
		private void SectionName_DropDownOpened(object sender, EventArgs e)
		{
			try
			{
				SectionName.Items.Clear();
				if (ChapterName.SelectedValue != null)
				{
					ComboBoxItem classitem = ClassName.SelectedItem as ComboBoxItem;
					string classname = classitem.Content.ToString();
					char classId = classname[0];
					ComboBoxItem ojectitem = OjectName.SelectedItem as ComboBoxItem;
					string ojectname = ojectitem.Content.ToString();
					char ojectId = ojectname[0];
					ComboBoxItem chapteritem = ChapterName.SelectedItem as ComboBoxItem;
					string chaptername = chapteritem.Content.ToString();
					char chapterId = chaptername[0];
					string path2 = appPath + @"\Id\Lop\1" + classId + ojectId + chapterId + @".json";
					string json3 = File.ReadAllText(path2);
					List<Section> chapter = JsonConvert.DeserializeObject<List<Section>>(json3);
					foreach (var item in chapter)
					{
						ComboBoxItem comboitem = new ComboBoxItem();
						comboitem.Content = item.SectionId + "-" + item.SectionName;
						SectionName.Items.Add(comboitem);
					}
				}
			}
			catch
			{

			}
		}
		private void OrderName_DropDownOpened(object sender, EventArgs e)
		{
			try
			{
				OrderName.Items.Clear();
				if (SectionName.SelectedValue != null)
				{
					ComboBoxItem classitem = ClassName.SelectedItem as ComboBoxItem;
					string classname = classitem.Content.ToString();
					int classId = int.Parse(classname[0].ToString());
					ComboBoxItem ojectitem = OjectName.SelectedItem as ComboBoxItem;
					string ojectname = ojectitem.Content.ToString();
					char ojectId = ojectname[0];
					ComboBoxItem chapteritem = ChapterName.SelectedItem as ComboBoxItem;
					string chaptername = chapteritem.Content.ToString();
					char chapterId = chaptername[0];
					ComboBoxItem sectionitem = SectionName.SelectedItem as ComboBoxItem;
					string sectionname = sectionitem.Content.ToString();
					char sectionId = sectionname[0];
					if (classId < 4)
					{
						string path = appPath + @"\Id\DangBai1" + classId + @"\1" + classId + ojectId + chapterId + "F" + sectionId + @".json";
						string json = File.ReadAllText(path);
						List<Items> items = JsonConvert.DeserializeObject<List<Items>>(json);
						foreach (var item in items)
						{
							ComboBoxItem comboitem = new ComboBoxItem();
							comboitem.Content = item.FormId + "-" + item.FormName;
							OrderName.Items.Add(comboitem);
						}
					}
					else
					{
						System.Windows.MessageBox.Show("Chưa có Id6 cho cấp 2", "Thoát");
					}
				}
			}
			catch
			{

			}
		}

		private void MaterialButton_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				string st = ";[";
				ComboBoxItem classitem = ClassName.SelectedItem as ComboBoxItem;
				string classname = classitem.Content.ToString();
				st +=classname[0];
				ComboBoxItem ojectitem = OjectName.SelectedItem as ComboBoxItem;
				string ojectname = ojectitem.Content.ToString();
				st += ojectname[0];
				ComboBoxItem chapteritem = ChapterName.SelectedItem as ComboBoxItem;
				string chaptername = chapteritem.Content.ToString();
				st += chaptername[0];
				ComboBoxItem levelitem = LevelName.SelectedItem as ComboBoxItem;
				string levelname = levelitem.Content.ToString();
				st += levelname[0];
				ComboBoxItem sectionitem = SectionName.SelectedItem as ComboBoxItem;
				string sectionname = sectionitem.Content.ToString();
				st += sectionname[0];
				if (Id6.IsChecked == true)
				{
					ComboBoxItem orderitem = OrderName.SelectedItem as ComboBoxItem;
					string ordername = orderitem.Content.ToString();
					st += "-" + ordername[0];
				}
				Xceed.Wpf.DataGrid.DataRow row = DataGrid.GetContainerFromItem(DataGrid.SelectedItem) as Xceed.Wpf.DataGrid.DataRow;
				Cell cell = row.Cells[3];
				cell.Content += st+"]";
				DataGrid.SelectedItem = null;
				DataGrid.CurrentItem = null;
				Popup1B.Close();
			}
			catch
			{
				System.Windows.MessageBox.Show("Chưa chọn đủ mục Id", "Thoát");
			}
		}
		private void StartId(object sender, RoutedEventArgs e)
		{
			try
			{
				string texnew = tex;
				int i = 0;
				string texcheck = "\\begin{" + Type + "}";
				foreach (var item in DataGrid.Items)
				{
					DataGrid.CurrentItem = item;
					Xceed.Wpf.DataGrid.DataRow row = DataGrid.GetContainerFromItem(DataGrid.CurrentItem) as Xceed.Wpf.DataGrid.DataRow;
					string codeId = "";
					if (row.Cells[3].Content != null)
					{
						codeId = row.Cells[3].Content.ToString();
					}
					codeId = codeId.Replace(";","%");
					texnew = texnew.Replace("%AddId" + i, codeId);
					i++;
				}
				File.WriteAllText(FileSelect.Text, texnew);
			}
			catch
			{

			}
		}

		private void MaterialButton_Click_2(object sender, RoutedEventArgs e)
		{
			try
			{
				string path = FileSelect.Text;
				System.Diagnostics.Process.Start(path);
			}
			catch
			{
				System.Windows.MessageBox.Show("Forder trống", "Thoát");
			}
		}
	}
}
