using Newtonsoft.Json;
using QuanLyTex.User5Class;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application = Microsoft.Office.Interop.Word.Application;

namespace QuanLyTex.User1Class
{
	class Result
	{
		string codestr;
		List<string> CodeIds;

		public string Codestr { get => codestr; set => codestr = value; }
		public List<string> CodeIds1 { get => CodeIds; set => CodeIds = value; }
	}
	class AcynsUser1
    {

		TexToWord TexTo = new TexToWord();
		public async System.Threading.Tasks.Task startListTexToWord1(List<string> list, string path)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					var app = new Application
					{
						Visible = true
					};
					Dictionary<string, string> dic = new Dictionary<string, string>();
					dic.Add("dn", "dn");
					dic.Add("dl", "dl");
					dic.Add("hq", "hq");
					dic.Add("cy", "cy");
					dic.Add("nx", "nx");
					dic.Add("dang", "dang");
					TexTo.addTextToWord(list, path, true,false, true, false, true,false,false, "EX", true, true, true, app, dic);
					app.Quit();
				}
				catch
				{

				}
			});
		}
		public async System.Threading.Tasks.Task startListTexToWord2(Dictionary<string,dynamic> dic,string appPath,string begin)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					foreach (var item in dic)
					{
						string pathnem2 = appPath + @"\LuuFile[" + item.Key + "]" + begin;
						List<string> list = item.Value;
						var app = new Application
						{
							Visible = true
						};
						Dictionary<string, string> dicnew = new Dictionary<string, string>();
						dicnew.Add("dn", "dn");
						dicnew.Add("dl", "dl");
						dicnew.Add("hq", "hq");
						dicnew.Add("cy", "cy");
						dicnew.Add("nx", "nx");
						dicnew.Add("dang", "dang");
						TexTo.addTextToWord(list, pathnem2, true, false, true, false, true, false, false, "EX", true, true, true, app, dicnew);
						app.Quit();
					}
					System.Windows.MessageBox.Show("Thực hiện chức năng tex to word thành công", "Thành công");
				}
				catch
				{
					System.Windows.MessageBox.Show("Thực hiện chức năng tex to word không thành công");
				}
			});
		}
		public async System.Threading.Tasks.Task DevideFile(string type, List<dynamic> listMapEx, string appPath, string strHeader,string strFooter,bool?Boxbt,bool? AutoWord,bool?Devide1,bool?Devide2,bool?Devide3,bool?Devide4)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					User1MapTex classlist = new User1MapTex();
					Dictionary<string, dynamic>  mapEx = classlist.mapSort(listMapEx, type);
					List<SortId> listsort = mapEx["listid"];
					List<Result> results=new List<Result>();
					if (Devide1 == true)
					{
						results = (from p in listsort group p.CodeId by p.ClassId into g select new Result { Codestr = "Lớp " + g.Key, CodeIds1 = g.ToList() }).ToList(); ;
					}
					if (Devide2 == true)
					{
						results = (from p in listsort group p.CodeId by p.LevelId into g select new Result { Codestr = "Mức độ " + g.Key, CodeIds1 = g.ToList() }).ToList(); ;
					}
					if (Devide3 == true)
					{
						results = (from p in listsort group p.CodeId by p.ObjectId into g select new Result { Codestr = "Môn " + g.Key, CodeIds1 = g.ToList() }).ToList(); ;
					}
					if (Devide4 == true)
					{
						results = (from p in listsort group p.CodeId by p.SectionId into g select new Result { Codestr = "Bài " + g.Key, CodeIds1 = g.ToList() }).ToList(); ;
					}
					Dictionary<string, dynamic> DicList = new Dictionary<string, dynamic>();
					foreach (Result item in results)
					{
						List<string> list = new List<string>();
						List<string> listExNew = item.CodeIds1;
						foreach (string item2 in listExNew)
						{
							list.AddRange(mapEx[item2]);
						}
						string pathnew = appPath + @"\LuuFile[" + item.Codestr + "]" + type + ".tex";
						classlist.newFileTex(list, pathnew, strHeader, strFooter);
						char typechar = 'e';
						if (Boxbt == true)
						{
							typechar = 'b';
						}
						if (AutoWord == true)
						{
							List<string> listExNew2 = new List<string>();
							foreach (string item2 in list)
							{
								string itemnew = typechar + item2.Remove(item2.Length - 8, 8).Remove(0, 11);
								listExNew2.Add(itemnew);
							}

							DicList.Add(item.Codestr, listExNew2);
						}
					}
					if (AutoWord == true)
					{
						string begin = "ex";
						AcynsUser1 TexTo = new AcynsUser1();
						TexTo.startListTexToWord2(DicList, appPath, begin);
					}
					System.Windows.MessageBox.Show("Tách file thành công, file sẽ được lưu trong thư mục LuuFile", "Thoát");
				}
				catch
				{
					System.Windows.MessageBox.Show("Tách file không thành công", "Thoát");
				}
			});
		}
		public async System.Threading.Tasks.Task DevideFile2(string type, Dictionary<string, dynamic> mapEx, string appPath, string strHeader, string strFooter, bool? Boxbt, bool? AutoWord, bool? Devide1, bool? Devide2, bool? Devide3, bool? Devide4)
		{
			await System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					User1MapTex classlist = new User1MapTex();
					List<SortId> listsort = mapEx["listid"];
					List<Result> results = new List<Result>();
					if (Devide1 == true)
					{
						results = (from p in listsort group p.CodeId by p.ClassId into g select new Result { Codestr = "Lớp " + g.Key, CodeIds1 = g.ToList() }).ToList(); 
					}
					if (Devide2 == true)
					{
						results = (from p in listsort group p.CodeId by p.LevelId into g select new Result { Codestr = "Mức độ " + g.Key, CodeIds1 = g.ToList() }).ToList(); 
					}
					if (Devide3 == true)
					{
						results = (from p in listsort group p.CodeId by p.ObjectId into g select new Result { Codestr = "Môn " + g.Key, CodeIds1 = g.ToList() }).ToList(); 
					}
					if (Devide4 == true)
					{
						results = (from p in listsort group p.CodeId by p.SectionId into g select new Result { Codestr = "Bài " + g.Key, CodeIds1 = g.ToList() }).ToList(); 
					}
					Dictionary<string, dynamic> DicList = new Dictionary<string, dynamic>();
					foreach (Result item in results)
					{
						List<string> list = new List<string>();
						List<string> listExNew = item.CodeIds1;
						foreach (string item2 in listExNew)
						{
							list.AddRange(mapEx[item2]);
						}
						string pathnew = appPath + @"\LuuFile[" + item.Codestr + "]" + type + ".tex";
						classlist.newFileTex(list, pathnew, strHeader, strFooter);
						char typechar = 'e';
						if (Boxbt == true)
						{
							typechar = 'b';
						}
						if (AutoWord == true)
						{
							List<string> listExNew2 = new List<string>();
							foreach (string item2 in list)
							{
								string itemnew = typechar + item2.Remove(item2.Length - 8, 8).Remove(0, 11);
								listExNew2.Add(itemnew);
							}

							DicList.Add(item.Codestr, listExNew2);
						}
					}
					System.Windows.MessageBox.Show("Tách file thành công, file sẽ được lưu trong thư mục LuuFile", "Thoát");
					if (AutoWord == true)
					{
						string begin = "ex";
						AcynsUser1 TexTo = new AcynsUser1();
						TexTo.startListTexToWord2(DicList, appPath, begin);
					}
				}
				catch
				{
					System.Windows.MessageBox.Show("Tách file không thành công", "Thoát");
				}
			});
		}
		public async Task BankEcer(string appPath, bool? id5, bool? id6, List<string> listPathOld, Regex rx,string type)
		{
			User1MapTex classlist = new User1MapTex();
			await Task.Run(() =>
			{
				try
				{
					if (id5 == true)
					{
						List<string> listPath = new List<string>();
						string texold = "";
						string pathtex = appPath + "\\NganHangTex\\Id5" + type + "\\TenFileDaLoc.txt";
						List<string> listtex = File.ReadAllText(pathtex).Split('@').ToList();
						foreach (string item in listPathOld)
						{
							string itemname = Path.GetFileName(item);
							if (listtex.Contains(itemname))
							{
								texold += itemname + ";";
							}
							else
							{
								listPath.Add(item);
								File.AppendAllText(pathtex, "@" + itemname);
							}
						}
						List<dynamic> listMapEx = classlist.mapNewFile(rx, type, listPath);
						Dictionary<string, dynamic> mapEx = classlist.mapSort(listMapEx, type);
						List<SortId> listsort = mapEx["listid"];
						foreach (SortId item in listsort)
						{
							try
							{
								string codeId = item.CodeId;
								string pathcodeId = appPath + "\\NganHangTex\\Id5" + type + "\\" + item.ClassId + "\\" + codeId  + ".tex";
								List<string> list = mapEx[codeId];
								foreach (string item2 in list)
								{
									File.AppendAllText(pathcodeId, "%!Cau!%\n" + item2);
								}
							}
							catch
							{
								
							}
						}
						System.Windows.MessageBox.Show("Đưa vào ngân hàng tex thành công,Những file đã lọc trong quá khứ: " + texold, "Thoát");
					}
					if (id6 == true)
					{
						List<string> listPath = new List<string>();
						string texold = "";
						string pathtex = appPath + "\\NganHangTex\\Id6" + type + "\\TenFileDaLoc.txt";
						List<string> listtex = File.ReadAllText(pathtex).Split('@').ToList();
						foreach (string item in listPathOld)
						{
							string itemname = Path.GetFileName(item);
							if (listtex.Contains(itemname))
							{
								texold += itemname + ";";
							}
							else
							{
								listPath.Add(item);
								File.AppendAllText(pathtex, "@" + itemname);
							}
						}
						List<dynamic> listMapEx = classlist.mapNewFile(rx, type, listPath);
						Dictionary<string, dynamic> mapEx = classlist.mapSort(listMapEx, type);
						List<SortId> listsort = mapEx["listid"];
						foreach (SortId item in listsort)
						{
							string codejson = "";
							string path = "";
							if (item.ClassId < 3) { codejson += "1" + item.ClassId; } else { codejson += item.ClassId; }
							if (item.ObjectId == 1) { codejson += "D"; } else { codejson += "H"; }
							codejson += item.CharterId + "F" + item.SectionId;
							if (item.ClassId < 3)
							{
								path = appPath + @"\DangBai1" + item.ClassId + @"\" + codejson + @".json";
							}
							else
							{
								 path = appPath + @"\DangBai" + item.ClassId + @"\" + codejson + @".json";
							}
							string codename = "";
							try
							{
								string json2 = File.ReadAllText(path);
								List<Item> Items = JsonConvert.DeserializeObject<List<Item>>(json2);
								foreach (Item item2 in Items)
								{
									if (item2.FormId == item.CodeId[item.CodeId.Length-1])
									{
										codename = item2.FormName;
										break;
									}
								}
								string codeId = item.CodeId;
								string pathcodeId = appPath + "\\NganHangTex\\Id5" + type + "\\" + item.ClassId + "\\" + codeId + "_" + codename + ".tex";
								List<string> list = mapEx[codeId];
								foreach (string item2 in list)
								{
									File.AppendAllText(pathcodeId, "\n" + item2);
								}
							}
							catch
							{
								System.Windows.MessageBox.Show("Chưa có ID6 cho THCS", "Thoát");
							}
						}
						System.Windows.MessageBox.Show("Đưa vào ngân hàng tex thành công,Những file đã lọc trong quá khứ: " + texold, "Thoát");
					}
				}
				catch
				{
					System.Windows.MessageBox.Show("Đưa vào ngân hàng tex không thành công", "Thoát");
				}
			});
		}
	}
}
