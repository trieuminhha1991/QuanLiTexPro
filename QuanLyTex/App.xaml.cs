
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using appdll;
namespace QuanLyTex
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		
		protected override void OnStartup(StartupEventArgs e)
        {
			if (ConfigurationManager.AppSettings["A"] == "1")
			{
				string[] array = ConfigurationManager.AppSettings.AllKeys;
				if (Array.Exists(array, E => E == "C")&& Array.Exists(array, E => E == "D")&& Array.Exists(array, E => E == "E"))
				{
					Licensing lic = new Licensing();
					string datestartstr = ConfigurationManager.AppSettings["C"];
					string dateendstr = ConfigurationManager.AppSettings["D"];
					string datecheckstr = ConfigurationManager.AppSettings["E"];
					DateTime datestart = DateTime.ParseExact(datestartstr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
					DateTime dateend = DateTime.ParseExact(dateendstr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
					DateTime date = DateTime.ParseExact(datecheckstr, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
					DateTime datenow = DateTime.Now;
					if (DateTime.Compare(date, datenow) > 0 || DateTime.Compare(datestart, datenow) > 0)
					{
						System.Windows.MessageBox.Show("Bản đã chỉnh sửa lại ngày tháng của máy mình đúng không, phần mềm sẽ tự động thoát, cám ơn bạn", "Thoát");
						Application a = Application.Current;
						a.Shutdown();
					}
					if (DateTime.Compare(datenow, dateend) > 0)
					{
						System.Windows.MessageBox.Show("Đã quá thời hạn sử dụng bản trailer, xin hãy cài lại bản Pro", "Thoát");
						Application a = Application.Current;
						a.Shutdown();
					}
					Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
					config.AppSettings.Settings.Remove("E");
					config.AppSettings.Settings.Add("E", datenow.ToString("MM/dd/yyyy"));
					config.Save();
					ConfigurationManager.RefreshSection("appSettings");
				}
				else
				{
					System.Windows.MessageBox.Show("Bạn đã kích hoạt bản quyền không đúng theo quy trình, cám ơn bạn", "Thoát");
					Application a = Application.Current;
					a.Shutdown();
				}
			}
			if (ConfigurationManager.AppSettings["A"] != "0")
			{
				
				string[] array = ConfigurationManager.AppSettings.AllKeys;
				if (Array.Exists(array, E => E == "F"))
				{
					int checkstr = int.Parse(ConfigurationManager.AppSettings["F"]);
					if (checkstr % 8 == 0)
					{
						if (Array.Exists(array, E => E == "B") && Array.Exists(array, E => E == "D") && Array.Exists(array, E => E == "F"))
						{
							Licensing lic = new Licensing();
							ListdataId list = new ListdataId();
							List<dataId> listlic = list.ListId;
							List<string> listId = (from p in listlic select p.Id).ToList();
							string license = ConfigurationManager.AppSettings["H"];
							string hardId = lic.getStringhardware() + lic.getHardDriverId();
							string licecstrue = lic.licensingFuntionTrailer(hardId);
							if (!listId.Contains(hardId) || license != licecstrue)
							{
								System.Windows.MessageBox.Show("Bản quyền của bạn không đúng, chương trình sẽ tự động thoát, cám ơn bạn", "Thoát");
								Application a = Application.Current;
								a.Shutdown();
							}
							else
							{
								Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
								config.AppSettings.Settings.Remove("F");
								config.AppSettings.Settings.Add("F", (checkstr + 1).ToString());
								config.Save();
								ConfigurationManager.RefreshSection("appSettings");
							}
						}
					}
					else
					{
						Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
						config.AppSettings.Settings.Remove("F");
						config.AppSettings.Settings.Add("F", (checkstr + 1).ToString());
						config.Save();
						ConfigurationManager.RefreshSection("appSettings");
					}
				}
				else
				{
					System.Windows.MessageBox.Show("Bản quyền của bạn không đúng, chương trình sẽ tự động thoát, cám ơn bạn", "Thoát");
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
