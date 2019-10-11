
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Windows;
using Application = System.Windows.Application;

namespace QuanLyTex
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{

		protected override void OnStartup(StartupEventArgs e)
		{
			if (!File.Exists(@"TexWordTrailer.exe.config"))
			{
				System.Windows.MessageBox.Show("Bạn đã kích hoạt bản quyền không đúng theo quy trình, cám ơn bạn", "Thoát");
				System.Windows.Application a = Application.Current;
				a.Shutdown();
			}
			if (ConfigurationManager.AppSettings["K"] == "0" && Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\window", "window", 0) == null)
			{
				string appPath = Directory.GetCurrentDirectory();
				string tex = File.ReadAllText(appPath + @"\Tex.txt");
				string[] arraystring = Directory.GetFiles(@"C:\Program Files (x86)\MathType", "Texvc (base rules).tdl", SearchOption.AllDirectories);
				foreach (string item in arraystring)
				{
					File.WriteAllText(item, tex);
				}
				try
				{
					var client = new TcpClient("time.nist.gov", 13);
					var streamReader = new StreamReader(client.GetStream());
					var response = streamReader.ReadToEnd();
					var utcDateTimeString = response.Substring(7, 17);
					var timenow = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
					var timeEnd = timenow.AddDays(7);
					string timenowstring = timenow.ToString("yy-MM-dd HH:mm:ss");
					string timeEndstring = timeEnd.ToString("yy-MM-dd HH:mm:ss");
					Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
					config.AppSettings.Settings.Remove("C");
					config.AppSettings.Settings.Add("C", timenowstring);
					config.AppSettings.Settings.Remove("D");
					config.AppSettings.Settings.Add("D", timeEndstring);
					config.AppSettings.Settings.Remove("K");
					config.AppSettings.Settings.Add("K", "1");
					config.Save();
					ConfigurationManager.RefreshSection("appSettings");
					Configuration con = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
					con.AppSettings.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
					con.Save(ConfigurationSaveMode.Full, true);
					ConfigurationManager.RefreshSection("appSettings");
					Microsoft.Win32.RegistryKey key;
					key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("window");
					key.SetValue("window", "1");
					key.Close();
					System.Windows.Forms.MessageBoxEx.Show("Đã có bản quyền, app sẽ tự động đóng lại, hãy chạy lại app", 3000);
					Application a = Application.Current;
					a.Shutdown();
				}
				catch
				{
					System.Windows.MessageBox.Show("Hãy kết nối mạng trước khi chạy app", "Thoát");
					Application a = Application.Current;
					a.Shutdown();
				}
			}
			else
			{
				try
				{
					var client = new TcpClient("time.nist.gov", 13);
					var streamReader = new StreamReader(client.GetStream());
					var response = streamReader.ReadToEnd();
					var utcDateTimeString = response.Substring(7, 17);
					var timenow = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
					string timestartstring = ConfigurationManager.AppSettings["C"];
					string timesendstring = ConfigurationManager.AppSettings["D"];
					DateTime timestart = DateTime.ParseExact(timestartstring, "yy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
					DateTime timesend = DateTime.ParseExact(timesendstring, "yy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
					if (DateTime.Compare(timestart, timenow) > 0 || DateTime.Compare(timenow, timesend) > 0)
					{
						System.Windows.MessageBox.Show("Thời gian sử dụng trailer đã kết thúc", "Thoát");
						Application a = Application.Current;
						a.Shutdown();
					}
				}
				catch
				{
					System.Windows.MessageBox.Show("Chưa kết nối mạng hoặc kích hoạt bản quyền không đúng", "Thoát");
					Application a = Application.Current;
					a.Shutdown();
				}
			}
			Xceed.Wpf.Toolkit.Licenser.LicenseKey = "WTK38-1SF9R-3H0GS-0GFA";
			Xceed.Wpf.DataGrid.Licenser.LicenseKey = "DGP67-FHP9Y-USSHH-E0LA";
			base.OnStartup(e);
		}
	}
}
