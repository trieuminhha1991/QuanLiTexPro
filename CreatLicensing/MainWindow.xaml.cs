using appdll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
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

namespace CreatLicensing
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}
		public string getStringhardware()
		{
			string cpuInfo = string.Empty;
			ManagementClass mc = new ManagementClass("win32_processor");
			ManagementObjectCollection moc = mc.GetInstances();

			foreach (ManagementObject mo in moc)
			{
				if (cpuInfo == "")
				{
					//Get only the first CPU's ID
					cpuInfo = mo.Properties["processorID"].Value.ToString();
					break;
				}
			}
			return cpuInfo.Replace(" ", "");
		}
		public string getHardDriverId()
		{
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
			string serial_number = "";
			foreach (ManagementObject wmi_HD in searcher.Get())
			{
				serial_number = wmi_HD["SerialNumber"].ToString();
			}
			return serial_number.Replace(" ","");
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (traler.IsChecked==true)
			{
				license.Text = licensingFuntionTrailer(Id.Text);
			}
			if (Pro.IsChecked == true)
			{
				license.Text = licensingFuntionPro(Id.Text);
			}
		}
		public string licensingFuntionTrailer(string id)
		{
			string licensing = "";
			byte[] array = Encoding.ASCII.GetBytes(id);
			foreach (byte item in array)
			{
				int index = int.Parse(item.ToString());
				for (int i = 1; i < 1000; i++)
				{
					if (i * i < index)
					{
						int j = i * 2 + i * 3 + 6;
						licensing += j;
					}
					else
					{
						break;
					}
				}
			}
			dataPlaceTrailerLicensing dataTrailer2 = new dataPlaceTrailerLicensing();
			List<datacodelicensing> data = dataTrailer2.ClassList;
			foreach (datacodelicensing item in data)
			{
				licensing = licensing.Replace(item.Id, item.IdPlace);
			}
			dataPlaceTrailer dataTrailer = new dataPlaceTrailer();
			List<datacode> data2 = dataTrailer.ClassList;
			foreach (datacode item in data2)
			{
				licensing = licensing.Replace(item.Id, item.IdPlace);
			}
			return licensing;
		}
		public string licensingFuntionPro(string id)
		{
			string licensing = "";
			byte[] array = Encoding.ASCII.GetBytes(id);
			foreach (byte item in array)
			{
				int index = int.Parse(item.ToString());
				for (int i = 1; i < 1000; i++)
				{
					if (i * i < index)
					{
						int j = i * 2 + i * 3 + 6;
						licensing += j;
					}
					else
					{
						break;
					}
				}
			}
			dataPlaceProLicensing dataTrailer2 = new dataPlaceProLicensing();
			List<datacodelicensing> data = dataTrailer2.ClassList;
			foreach (datacodelicensing item in data)
			{
				licensing = licensing.Replace(item.Id, item.IdPlace);
			}
			dataPlacePro dataTrailer = new dataPlacePro();
			List<datacode> data2 = dataTrailer.ClassList;
			foreach (datacode item in data2)
			{
				licensing = licensing.Replace(item.Id, item.IdPlace);
			}
			return licensing;
		}
	}
}
