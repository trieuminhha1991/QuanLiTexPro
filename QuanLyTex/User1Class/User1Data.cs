using System.Collections.Generic;


namespace QuanLyTex
{
    class classMap { private int _classID; private string _className; public int classId { get => _classID; set { _classID = value; } }  public string className { get => _className; set { _className = value; } } }
    class ojectMap { private string _ojectId; private string _ojectName; public string ojectId { get => _ojectId; set { _ojectId = value; } } public string ojectName { get => _ojectName; set { _ojectName = value; } } }
    class chapterMap { private int _chapterId; private string _chapterName; public int chapterId { get => _chapterId; set { _chapterId = value; } }  public string chapterName { get => _chapterName; set { _chapterName = value; } } }
    class orderMap { private int _orderLession; private string _orderName; public int orderLession { get => _orderLession; set { _orderLession = value; } } public string orderName { get => _orderName; set { _orderName = value; } } }
    class levelMap { private string _levelQuestion; private string _levelName; public string levelQuestion { get => _levelQuestion; set { _levelQuestion = value; } }  public string levelName { get => _levelName; set { _levelName = value; } } }
    class exerciseFormatMap { private string _exerciseQuestion; private string _exerciseName; public string exerciseQuestion { get => _exerciseQuestion; set { _exerciseQuestion = value; } } public string exerciseName { get => _exerciseName; set { _exerciseName = value; } } }
    class User1Data
    {
        private List<classMap> _classList = new List<classMap>()
        {
                new classMap(){classId= 6,className="6- Lớp 6." },
                new classMap(){classId= 7,className="7- Lớp 7." },
                new classMap(){classId= 8,className="8- Lớp 8." },
                new classMap(){classId= 9,className="9- Lớp 9." },
                new classMap(){classId= 0,className="10- Lớp 10." },
                new classMap(){classId= 1,className="11- Lớp 11." },
                new classMap(){classId= 2,className="12- Lớp 12." },
        };
        private List<ojectMap> _ojectList = new List<ojectMap>()
        {
               new ojectMap(){ojectId= "D",ojectName="Đ- Đại số." },
               new ojectMap(){ojectId= "H",ojectName="H- Hình học." },
        };
        private List<chapterMap> _chapterList = new List<chapterMap>()
        {
				new chapterMap(){chapterId= 0,chapterName="0- Chương 0." },
			   new chapterMap(){chapterId= 1,chapterName="1- Chương 1." },
               new chapterMap(){chapterId= 2,chapterName="2- Chương 2." },
               new chapterMap(){chapterId= 3,chapterName="3- Chương 3." },
               new chapterMap(){chapterId= 4,chapterName="4- Chương 4." },
               new chapterMap(){chapterId= 5,chapterName="5- Chương 5." },
               new chapterMap(){chapterId= 6,chapterName="6- Chương 6." },
               new chapterMap(){chapterId= 7,chapterName="7- Chương 7." },
               new chapterMap(){chapterId= 8,chapterName="8- Chương 8." },
               new chapterMap(){chapterId= 9,chapterName="9- Chương 9." },
        };
        private List<orderMap> _orderList = new List<orderMap>()
        {
               new orderMap(){orderLession= 1,orderName="1- Bài 1." },
               new orderMap(){orderLession= 2,orderName="2- Bài 2." },
               new orderMap(){orderLession= 3,orderName="3- Bài 3." },
               new orderMap(){orderLession= 4,orderName="4- Bài 4." },
               new orderMap(){orderLession= 5,orderName="5- Bài 5." },
               new orderMap(){orderLession= 6,orderName="6- Bài 6." },
               new orderMap(){orderLession= 7,orderName="7- Bài 7." },
               new orderMap(){orderLession= 8,orderName="8- Bài 8." },
               new orderMap(){orderLession= 9,orderName="9- Bài 9." },
        };
        private List<levelMap> _levelList = new List<levelMap>()
        {
               new levelMap(){levelQuestion= "Y",levelName="Y- Yếu." },
               new levelMap(){levelQuestion= "B",levelName="B- Trung bình." },
               new levelMap(){levelQuestion= "K",levelName="K- Khá." },
               new levelMap(){levelQuestion= "G",levelName="G-Giỏi ." },
               new levelMap(){levelQuestion= "T",levelName="TT- Thực tế." },
        };
        private List<exerciseFormatMap> _exerciseList = new List<exerciseFormatMap>()
        {
               new exerciseFormatMap(){exerciseQuestion= "1",exerciseName="Dạng 1." },
               new exerciseFormatMap(){exerciseQuestion= "2",exerciseName="Dạng 2." },
               new exerciseFormatMap(){exerciseQuestion= "3",exerciseName="Dạng 3." },
               new exerciseFormatMap(){exerciseQuestion= "4",exerciseName="Dạng 4 ." },
               new exerciseFormatMap(){exerciseQuestion= "5",exerciseName="Dạng 5." },
               new exerciseFormatMap(){exerciseQuestion= "6",exerciseName="Dạng 6." },
               new exerciseFormatMap(){exerciseQuestion= "7",exerciseName="Dạng 7." },
               new exerciseFormatMap(){exerciseQuestion= "8",exerciseName="Dạng 8." },
               new exerciseFormatMap(){exerciseQuestion= "9",exerciseName="Dạng 9." },
        };
        public List<classMap> classList { get => _classList; }
        public List<ojectMap> ojectList { get => _ojectList; }
        public List<chapterMap> chapterList { get => _chapterList; }
        public List<orderMap> orderList { get => _orderList; }
        public List<levelMap> levelList { get => _levelList; }
        public List<exerciseFormatMap> exerciseList { get => _exerciseList; }

    }
}
