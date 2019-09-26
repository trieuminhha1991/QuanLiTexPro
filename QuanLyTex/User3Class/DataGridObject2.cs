
namespace QuanLyTex.User3Class
{
	class DataGridObject2
	{
		private string _all;
		private string _fileName;
		private string _Ecersice;
		private string _codeId;
		private string _codeName;
		private string _codeLevel;
		private bool _isSelected;

		public string Ecersice { get => _Ecersice; set => _Ecersice = value; }
		public string CodeId { get => _codeId; set => _codeId = value; }
		public string CodeName { get => _codeName; set => _codeName = value; }
		public bool IsSelected { get => _isSelected; set => _isSelected = value; }
		public string CodeLevel { get => _codeLevel; set => _codeLevel = value; }
		public string FileName { get => _fileName; set => _fileName = value; }
		public string All { get => _all; set => _all = value; }
	}
}
