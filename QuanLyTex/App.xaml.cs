
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
			ListdataId list = new ListdataId();
			List<string> liststr = list.ListId;
			Licensing licen = new Licensing();
			string check = licen.getStringhardware() + licen.getHardDriverId();
			if (!liststr.Contains(check))
			{
				System.Windows.MessageBox.Show("Bạn đã kích hoạt bản quyền không đúng theo quy trình, cám ơn bạn", "Thoát");
				Application a = Application.Current;
				a.Shutdown();
			}
			Xceed.Wpf.Toolkit.Licenser.LicenseKey = "WTK38-1SF9R-3H0GS-0GFA";
			Xceed.Wpf.DataGrid.Licenser.LicenseKey = "DGP67-FHP9Y-USSHH-E0LA";
			base.OnStartup(e);
		}
	}
}
