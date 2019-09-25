using appdll;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyTex
{
	/// <summary>
	/// Interaction logic for UserControl4.xaml
	/// </summary>
	public partial class UserControl4 : UserControl
	{
		public UserControl4()
		{
			InitializeComponent();
			string sAttr = ConfigurationManager.AppSettings["A"];
			if(sAttr!="0")
			{
				try
				{
					BanQuyen.Visibility = Visibility.Visible;
					HardId.Text= ConfigurationManager.AppSettings["B"];
					if (sAttr == "1") { Liccense.Text = "Trailer"; }
					if (sAttr == "2") { Liccense.Text = "Pro"; }
					DateStart.Text= ConfigurationManager.AppSettings["C"];
					DateEnd.Text= ConfigurationManager.AppSettings["D"];
					KichHoat.Visibility = Visibility.Hidden;
				}
				catch
				{

				}
			}
		}

		private void MaterialButton_Click(object sender, RoutedEventArgs e)
		{
			ListdataId list = new ListdataId();
			List<dataId> listlic = list.ListId;
			List<string> listId = (from p in listlic select p.Id).ToList();
			Licensing lic = new Licensing();
			string hardId = lic.getStringhardware() + lic.getHardDriverId();
			if(!listId.Contains(hardId))
			{
				MaId.Text = "Máy của bạn không có quyền sử dụng Id";
			}
			else
			{
				MaId.Text = hardId;
			}
		}

		private void MaterialButton_Click_1(object sender, RoutedEventArgs e)
		{
			try
			{
				ListdataId list = new ListdataId();
				List<dataId> listlic = list.ListId;
				string chechstring = liccensing.Text;
				string hartID = MaId.Text;
				List<dataId> datalist = (from p in listlic where p.Id == hartID select p).ToList();
				dataId data = datalist[0];
				Licensing lic = new Licensing();
				string licecstrue = lic.licensingFuntionTrailer(hartID);
				if (chechstring == licecstrue)
				{
					Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
					config.AppSettings.Settings["A"].Value = "1";
					DateTime timenow = DateTime.Now;
					DateTime timestart = data.DateStart;
					DateTime timeEnd = data.DateEnd;
					string TimeNameNow = timenow.ToString("MM/dd/yyyy");
					string TimeNameStart = timestart.ToString("MM/dd/yyyy");
					string TimeNameEnd = timeEnd.ToString("MM/dd/yyyy");
					config.AppSettings.Settings.Remove("B");
					config.AppSettings.Settings.Add("B", hartID);
					config.AppSettings.Settings.Remove("C");
					config.AppSettings.Settings.Add("C", TimeNameStart);
					config.AppSettings.Settings.Remove("D");
					config.AppSettings.Settings.Add("D", TimeNameEnd);
					config.AppSettings.Settings.Remove("E");
					config.AppSettings.Settings.Add("E", TimeNameNow);
					config.AppSettings.Settings.Remove("F");
					config.AppSettings.Settings.Add("F", "40");
					config.AppSettings.Settings.Remove("H");
					config.AppSettings.Settings.Add("H", chechstring);
					config.Save();
					ConfigurationManager.RefreshSection("appSettings");
					HardId.Text = ConfigurationManager.AppSettings["B"];
					string sAttr = ConfigurationManager.AppSettings["A"];
					if (sAttr == "1") { Liccense.Text = "Trailer"; }
					if (sAttr == "2") { Liccense.Text = "Pro"; }
					DateStart.Text = ConfigurationManager.AppSettings["C"];
					DateEnd.Text = ConfigurationManager.AppSettings["D"];
					BanQuyen.Visibility = Visibility.Visible;
					KichHoat.Visibility = Visibility.Hidden;
					System.Windows.MessageBox.Show("Kích hoạt bản quyền thành công", "Thoát");
				}
				else
				{
					System.Windows.MessageBox.Show("Kích hoạt bản quyền không thành công", "Thoát");
				}
			}
			catch
			{
				System.Windows.MessageBox.Show("Kích hoạt bản quyền không thành công", "Thoát");
			}
		}
	}
}
