using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appdll
{
	public class dataId
	{
		private string _Id;
		private DateTime _dateStart;
		private DateTime _dateEnd;

		public DateTime DateStart { get => _dateStart; set => _dateStart = value; }
		public DateTime DateEnd { get => _dateEnd; set => _dateEnd = value; }
		public string Id { get => _Id; set => _Id = value; }
	}
	public class ListdataId
	{
		private List<dataId> _listId = new List<dataId>()
		{
			new dataId(){Id= "BFEBFBFF000306C3002543134BHD", DateStart=new DateTime(2019, 1, 1),DateEnd=new DateTime(2050, 1, 1) },
		}
		;
		public List<dataId> ListId { get => _listId; }
	}
	public class datacodelicensing
	{
		private string _id;
		private string _idPlace;

		public string Id { get => _id; set => _id = value; }
		public string IdPlace { get => _idPlace; set => _idPlace = value; }
	}
	public class dataPlaceTrailerLicensing
	{
		private List<datacodelicensing> _classList = new List<datacodelicensing>()
		{
				new datacodelicensing(){Id= "01",IdPlace="12jj" },
				new datacodelicensing(){Id= "02",IdPlace="028s8dj" },
				new datacodelicensing(){Id= "03",IdPlace="1" },
				new datacodelicensing(){Id= "04",IdPlace="12098" },
				new datacodelicensing(){Id= "05",IdPlace="#mofi" },
				new datacodelicensing(){Id= "06",IdPlace="1-31=" },
				new datacodelicensing(){Id= "07",IdPlace="fs1" },
				new datacodelicensing(){Id= "08",IdPlace="!_)#" },
				new datacodelicensing(){Id= "09",IdPlace="f13u1" },
				new datacodelicensing(){Id= "12",IdPlace="t" },
				new datacodelicensing(){Id= "11",IdPlace="ru1" },
				new datacodelicensing(){Id= "22",IdPlace="1k4" },
				new datacodelicensing(){Id= "33",IdPlace="tu3n" },
				new datacodelicensing(){Id= "44",IdPlace="#87r" },
				new datacodelicensing(){Id= "55",IdPlace="+45-12" },
				new datacodelicensing(){Id= "66",IdPlace="jk23" },
				new datacodelicensing(){Id= "77",IdPlace="oa%54" },
				new datacodelicensing(){Id= "91",IdPlace="eut" },
				new datacodelicensing(){Id= "0",IdPlace="kl032" },
				new datacodelicensing(){Id= "1",IdPlace="82sd" },
				new datacodelicensing(){Id= "2",IdPlace="1bc3" },
				new datacodelicensing(){Id= "3",IdPlace="jk6h" },
				new datacodelicensing(){Id= "4",IdPlace="usdkc" },
				new datacodelicensing(){Id= "5",IdPlace="ha23" },
				new datacodelicensing(){Id= "6",IdPlace="r5@hc@" },
				new datacodelicensing(){Id= "7",IdPlace="f2@3a" },
				new datacodelicensing(){Id= "8",IdPlace="ru9d" },
				new datacodelicensing(){Id= "9",IdPlace="ki#01" }
		};

		public List<datacodelicensing> ClassList { get => _classList; }
	}
	public class dataPlaceProLicensing
	{
		private List<datacodelicensing> _classList = new List<datacodelicensing>()
		{
				new datacodelicensing(){Id= "01",IdPlace="3021sdm13lkMK@" },
				new datacodelicensing(){Id= "02",IdPlace="!2Y2K$y!@hJDFQ" },
				new datacodelicensing(){Id= "03",IdPlace="129831JFD129" },
				new datacodelicensing(){Id= "04",IdPlace="120938-05IVDMR2" },
				new datacodelicensing(){Id= "05",IdPlace="42382OCMSKDJ20" },
				new datacodelicensing(){Id= "06",IdPlace="120380SDLM20" },
				new datacodelicensing(){Id= "07",IdPlace="123dwg545@" },
				new datacodelicensing(){Id= "08",IdPlace="123csdf23" },
				new datacodelicensing(){Id= "09",IdPlace="124fv456dfwe" },
				new datacodelicensing(){Id= "12",IdPlace="12ruki" },
				new datacodelicensing(){Id= "13",IdPlace="hun@1" },
				new datacodelicensing(){Id= "11",IdPlace="t35()76)" },
				new datacodelicensing(){Id= "23",IdPlace="kc23" },
				new datacodelicensing(){Id= "22",IdPlace="lumx" },
				new datacodelicensing(){Id= "33",IdPlace="tash23$56" },
				new datacodelicensing(){Id= "34",IdPlace="kirimu(9)" },
				new datacodelicensing(){Id= "45",IdPlace="cose" },
				new datacodelicensing(){Id= "54",IdPlace="hoalik" },
				new datacodelicensing(){Id= "55",IdPlace="$hi$nc" },
				new datacodelicensing(){Id= "61",IdPlace="ta$67e" },
				new datacodelicensing(){Id= "65",IdPlace="sdkcvwk" },
				new datacodelicensing(){Id= "68",IdPlace="kidksdm" },
				new datacodelicensing(){Id= "69",IdPlace="cuwakwcm" },
				new datacodelicensing(){Id= "71",IdPlace="wklc13d" },
				new datacodelicensing(){Id= "07",IdPlace="$3872-234" },
				new datacodelicensing(){Id= "01",IdPlace="kanim@#" },
				new datacodelicensing(){Id= "00",IdPlace="jobsulome" },
				new datacodelicensing(){Id= "04",IdPlace="$56$-32" },
				new datacodelicensing(){Id= "91",IdPlace="lomi@3" },
				new datacodelicensing(){Id= "1",IdPlace="inut*562" },
				new datacodelicensing(){Id= "2",IdPlace="56ten!9" },
				new datacodelicensing(){Id= "3",IdPlace="0-97-231" },
				new datacodelicensing(){Id= "4",IdPlace="ru_78$" },
				new datacodelicensing(){Id= "5",IdPlace="hani%782" },
				new datacodelicensing(){Id= "6",IdPlace="Cóni_4723823423" },
				new datacodelicensing(){Id= "7",IdPlace="madco123918" },
				new datacodelicensing(){Id= "8",IdPlace="sfasc12423" },
				new datacodelicensing(){Id= "9",IdPlace="lansekrwe1293@" },
		};

		public List<datacodelicensing> ClassList { get => _classList; }
	}
}
