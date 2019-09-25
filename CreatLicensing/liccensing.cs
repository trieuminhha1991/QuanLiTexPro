using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatLicensing
{
	class datacode
	{
		private string _id;
		private string _idPlace;

		public string Id { get => _id; set => _id = value; }
		public string IdPlace { get => _idPlace; set => _idPlace = value; }
	}
	class dataPlaceTrailer
	{
		private List<datacode> _classList = new List<datacode>()
		{
				new datacode(){Id= "a",IdPlace="9" },
				new datacode(){Id= "b",IdPlace="1" },
				new datacode(){Id= "1",IdPlace="3" },
				new datacode(){Id= "2",IdPlace="5" },
				new datacode(){Id= "3",IdPlace="1" },
				new datacode(){Id= "0",IdPlace="!" },
				new datacode(){Id= "4",IdPlace="h" },
				new datacode(){Id= "5",IdPlace="9" },
				new datacode(){Id= "6",IdPlace="y" },
				new datacode(){Id= "7",IdPlace="l" },
				new datacode(){Id= "8",IdPlace="c" },
				new datacode(){Id= "9",IdPlace="u" },
				new datacode(){Id= "d",IdPlace="1" },
		};

		public List<datacode> ClassList { get => _classList; }
	}
	class dataPlacePro
	{
		private List<datacode> _classList = new List<datacode>()
		{
				new datacode(){Id= "a",IdPlace="32" },
				new datacode(){Id= "b",IdPlace="12" },
				new datacode(){Id= "1",IdPlace="v" },
				new datacode(){Id= "2",IdPlace="4f" },
				new datacode(){Id= "3",IdPlace="0h" },
				new datacode(){Id= "0",IdPlace="im" },
				new datacode(){Id= "4",IdPlace="!0" },
				new datacode(){Id= "5",IdPlace="8j" },
				new datacode(){Id= "6",IdPlace="9n" },
				new datacode(){Id= "7",IdPlace="10l" },
				new datacode(){Id= "8",IdPlace="4z" },
				new datacode(){Id= "9",IdPlace="0u" },
				new datacode(){Id= "d",IdPlace="5n" },
		};

		public List<datacode> ClassList { get => _classList; }
	}
}
