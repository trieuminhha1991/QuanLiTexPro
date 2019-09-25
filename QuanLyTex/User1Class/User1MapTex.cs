using QuanLyTex.User1Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace QuanLyTex
{
	class User1MapTex
	{
		public List<dynamic> FilterId(string file, string Type, Regex rx)
		{
			try
			{
				int startIndex = 0;
				List<dynamic> list = new List<dynamic>();
				string str = @"\begin{" + Type + "}";
				string str2 = @"\end{" + Type + "}";
				string str3 = File.ReadAllText(file);
				while (startIndex <= str3.LastIndexOf(str))
				{
					Dictionary<string, string> Dic = new Dictionary<string, string>();
					startIndex = str3.IndexOf(str, startIndex);
					int num2 = str3.IndexOf(str2, startIndex) + str2.Length;
					string input = str3.Substring(startIndex, num2 - startIndex);
					if (rx.Matches(input).Count > 0)
					{
						if (rx.Matches(input).Count == 1)
						{
							Dic.Add("exersice", input);
							Match first = rx.Match(input);
							string firstValue = first.Value.Replace("[", "").Replace("]", "");
							Dic.Add("codeId", firstValue);
						}
						if (rx.Matches(input).Count > 1)
						{
							if(Type=="ex")
							{
								string codeId = "";
								foreach (Match match in rx.Matches(input))
								{
									if (match.Value.Contains("T"))
									{
										codeId = match.Value.Replace("[", "").Replace("]", "");
										break;
									}
									else if (!codeId.Contains("G") && match.Value.Contains("G"))
									{
										codeId = match.Value.Replace("[", "").Replace("]", "");
									}
									else if (!codeId.Contains("G") && !codeId.Contains("K") && match.Value.Contains("K"))
									{
										codeId = match.Value.Replace("[", "").Replace("]", "");
									}
									else if (!codeId.Contains("G") && !codeId.Contains("K") && !codeId.Contains("B") && match.Value.Contains("B"))
									{
										codeId = match.Value.Replace("[", "").Replace("]", "");
									}
									else if (!codeId.Contains("G") && !codeId.Contains("K") && !codeId.Contains("B"))
									{
										codeId = match.Value.Replace("[", "").Replace("]", "");
									}
								}
								Dic.Add("codeId", codeId);
								Dic.Add("exersice", input);
							}
							else
							{
								string codeId = "";
								foreach (Match match in rx.Matches(input))
								{
									codeId += match.Value.Replace("[", "").Replace("]", "") + ";";
								}
								codeId = codeId.Remove(codeId.Length - 1, 1);
								Dic.Add("exersice", input);
								Dic.Add("codeId", codeId);
							}
						}
						list.Add(Dic);
					}
					startIndex = num2;
				}
				return list;
			}
			catch (Exception e)
			{
				return null;
			}
		}
		public List<dynamic> mapNewFile(Regex rx,  string type, List<string> list)
		{

			List<dynamic> listnew = new List<dynamic>();
			if (list != null && list.Count > 0)
			{
				foreach (string str in list)
				{
					List<dynamic> collection = FilterId(str, type, rx);
					if (collection.Count > 0)
					{
						listnew.AddRange(collection);
					}
				}
			}
			return listnew;
		}
		public Dictionary<string, dynamic> mapSort(List<dynamic> listMapEx,string type)
		{
			try
			{
				if (type == "ex")
				{
					
					List<SortId> listsort = new List<SortId>();
					Dictionary<string, dynamic> Dic = new Dictionary<string, dynamic>();
					List<string> list = new List<string>();
					foreach (var item in listMapEx)
					{
						string stringId = item["codeId"];
						string stringEcer = item["exersice"];
						if (list.Contains(stringId))
						{
							Dic[stringId].Add(stringEcer);
						}
						else
						{
							List<string> listEcer = new List<string>();
							listEcer.Add(stringEcer);
							Dic.Add(stringId, listEcer);
							list.Add(stringId);
							SortId sort = new SortId();
							sort.ClassId = int.Parse(stringId[0].ToString());
							if (stringId[1] == 'D') { sort.ObjectId = 1; } else { sort.ObjectId = 2; }
							sort.CharterId= int.Parse(stringId[2].ToString());
							switch (stringId[3])
							{
								case 'Y':
									sort.LevelId=1;
									break;
								case 'B':
									sort.LevelId = 2;
									break;
								case 'K':
									sort.LevelId = 3;
									break;
								case 'G':
									sort.LevelId = 5;
									break;
								case 'T':
									sort.LevelId = 4;
									break;
							}
							sort.SectionId= int.Parse(stringId[4].ToString());
							sort.CodeId = stringId;
							listsort.Add(sort);
						}
					}
					Dic.Add("listid", listsort);
					Dic.Add("listCodeId", list);
					return Dic;
				}
				else
				{
					List<SortId> listsort = new List<SortId>();
					Dictionary<string, dynamic> Dic = new Dictionary<string, dynamic>();
					List<string> list = new List<string>();
					foreach (var item in listMapEx)
					{
						string stringId = item["codeId"];
						string stringEcer = item["exersice"];
						if (stringId.Contains(";"))
						{
							string[] stringarray = stringId.Split(';');
							foreach (string st in stringarray)
							{
								if (list.Contains(stringId))
								{
									Dic[stringId].Add(stringEcer);
								}
								else
								{
									List<string> listEcer = new List<string>();
									listEcer.Add(stringEcer);
									Dic.Add(stringId, listEcer);
									list.Add(stringId);
									SortId sort = new SortId();
									sort.ClassId = int.Parse(stringId[0].ToString());
									if (stringId[1] == 'D') { sort.ObjectId = 1; } else { sort.ObjectId = 2; }
									sort.CharterId = int.Parse(stringId[2].ToString());
									switch (stringId[3])
									{
										case 'Y':
											sort.LevelId = 1;
											break;
										case 'B':
											sort.LevelId = 2;
											break;
										case 'K':
											sort.LevelId = 3;
											break;
										case 'G':
											sort.LevelId = 5;
											break;
										case 'T':
											sort.LevelId = 4;
											break;
									}
									sort.SectionId = int.Parse(stringId[4].ToString());
									sort.CodeId = stringId;
									listsort.Add(sort);
								}
							}
						}
						else
						{
							if (list.Contains(stringId))
							{
								Dic[stringId].Add(stringEcer);
							}
							else
							{
								List<string> listEcer = new List<string>();
								listEcer.Add(stringEcer);
								Dic.Add(stringId, listEcer);
								list.Add(stringId);
								SortId sort = new SortId();
								sort.ClassId = int.Parse(stringId[0].ToString());
								if (stringId[1] == 'D') { sort.ObjectId = 1; } else { sort.ObjectId = 2; }
								sort.CharterId = int.Parse(stringId[2].ToString());
								switch (stringId[3])
								{
									case 'Y':
										sort.LevelId = 1;
										break;
									case 'B':
										sort.LevelId = 2;
										break;
									case 'K':
										sort.LevelId = 3;
										break;
									case 'G':
										sort.LevelId = 5;
										break;
									case 'T':
										sort.LevelId = 4;
										break;
								}
								sort.SectionId = int.Parse(stringId[4].ToString());
								sort.CodeId = stringId;
								listsort.Add(sort);
							}
						}
					}
					Dic.Add("listid", listsort);
					Dic.Add("listCodeId", list);
					return Dic;
				}
			}
			catch (Exception e)
			{
				return null;
			}
		}
		
		public void newFileTex(List<string> list, string Path, string FormHd, string FormFt)
		{
			try
			{
				if (Path != "")
				{
					string  st= FormHd + "\n";
					foreach (string str in list)
					{
						st+= str + "\n";
					}
					st+= FormFt + "\n";
					File.WriteAllText(Path, st);
					if (!File.Exists(Path))
					{
						MessageBox.Show("Tạo file thất bại", "Thất bại");
					}
				}
				else
				{
					System.Windows.MessageBox.Show("Chưa chọn file lọc", "Thoát");
				}
			}
			catch (Exception e)
			{
			}
		}
	}
}
