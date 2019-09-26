
namespace QuanLyTex.User1Class
{
    class DataGrid1
    {
        private string _className;
        private string _chapterName;
        private string _sectionName;
        private string _levelId;
        private string _codeId;
        private int _numberExersice;
        private int _numberExersiceSelect;
        private bool _IsSelected;
        public string ClassName { get => _className; set { _className = value; } }
        public string ChapterName { get => _chapterName; set => _chapterName = value; }
        public string SectionName { get => _sectionName; set => _sectionName = value; }
        public string CodeId { get => _codeId; set => _codeId = value; }
        public int NumberExersice { get => _numberExersice; set => _numberExersice = value; }
        public bool IsSelected { get => _IsSelected; set => _IsSelected = value; }
        public string LevelId { get => _levelId; set => _levelId = value; }
        public int NumberExersiceSelect { get => _numberExersiceSelect; set => _numberExersiceSelect = value; }
        }
}
