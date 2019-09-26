using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace QuanLyTex
{
	public class Item
	{
		public int FormId { get; set; }
		public string FormName { get; set; }
	}
	public class Section
	{
		public int SectionId { get; set; }
		public string SectionName { get; set; }
	}
	class User3MapTex
	{
		
		public class Chapter
		{
			public int ChapterId { get; set; }
			public char ObjectName { get; set; }
			public string ChapterName { get; set; }

		}
		public List<dynamic> FilterId(string file, string Type, Regex rx)
		{
			int startIndex = 0;
			List<dynamic> list = new List<dynamic>();
			string str = @"\begin{" + Type + "}";
			string str2 = @"\end{" + Type + "}";
			string str3 = File.ReadAllText(file);
			string appPath = Directory.GetCurrentDirectory() + @"\Id";

			while (startIndex <= str3.LastIndexOf(str))
			{

				Dictionary<string, dynamic> Dic = new Dictionary<string, dynamic>();
				startIndex = str3.IndexOf(str, startIndex);
				int num2 = str3.IndexOf(str2, startIndex) + str2.Length;
				string input = str3.Substring(startIndex, num2 - startIndex);
				string question = "";
				Dic.Add("all", input);
				if (input.Contains(@"\loigiai"))
				{
					int start = input.IndexOf(@"\loigiai");
					question = input.Substring(0, start);
				}
				else
				{
					question = input;
				}
				Dic.Add("exersice", question);
				try
				{
					if (rx.Matches(input).Count > 0)
					{
						string codeId = "";
						string codeLevel = "";
						string codeName = "";
						if (rx.Matches(input).Count == 1)
						{
							Match first = rx.Match(input);
							codeId = first.Value.Replace("[", "").Replace("]", "");
							codeLevel = codeId[3].ToString();
						}
						if (rx.Matches(input).Count > 1)
						{
							Dic.Add("exersice", input);
							List<string> listId = new List<string>();

							foreach (Match match in rx.Matches(input))
							{
								if (match.Value.Contains("T"))
								{
									codeId = match.Value;
									codeLevel = "T";
									break;
								}
								else if (!codeId.Contains("G") && match.Value.Contains("G"))
								{
									codeId = match.Value;
									codeLevel = "G";
								}
								else if (!codeId.Contains("G") && !codeId.Contains("K") && match.Value.Contains("K"))
								{
									codeId = match.Value;
									codeLevel = "K";
								}
								else if (!codeId.Contains("G") && !codeId.Contains("K") && !codeId.Contains("B") && match.Value.Contains("B"))
								{
									codeId = match.Value;
									codeLevel = "B";
								}
								else if (!codeId.Contains("G") && !codeId.Contains("K") && !codeId.Contains("B"))
								{
									codeId = match.Value;
									codeLevel = "Y";
								}
							}
						}
						codeId = codeId.Replace("[", "").Replace("]", "");
						Dic.Add("codeId", codeId);
						Dic.Add("codeLevel", codeLevel);
						string codeIdSub = codeId.Substring(0, 5);
						string codeIdClass = codeId.Substring(0, 3);
						int indexClass = int.Parse(codeId[0].ToString());
						int indexChapter = int.Parse(codeId[2].ToString());
						char indexSubject = codeId[1];
						int indexSection = int.Parse(codeId[4].ToString());
						codeName += "Lớp: 1" + indexClass + "\r\n";
						if (indexSubject == 'D')
						{
							codeName += "Phân môn: Đại số\r\n";
						}
						else
						{
							codeName += "Phân môn: Hình học\r\n";
						}
						string path = appPath + @"\DangBai1" + indexClass + @"\1" + codeIdClass + "F" + indexSection + @".json";
						string path2 = appPath + @"\Lop\1" + codeIdClass + @".json";
						string path3 = appPath + @"\Lop\Class1" + indexClass + @".json";
						string json3 = File.ReadAllText(path3);
						List<Chapter> chapter = JsonConvert.DeserializeObject<List<Chapter>>(json3);
						foreach (var item in chapter)
						{
							if (item.ChapterId == indexChapter && item.ObjectName == indexSubject)
							{
								codeName += "Chương " + indexChapter + ":" + item.ChapterName + "\r\n";
								break;
							}
						}
						string json2 = File.ReadAllText(path2);
						List<Section> Sections = JsonConvert.DeserializeObject<List<Section>>(json2);
						foreach (var item in Sections)
						{
							if (item.SectionId == indexSection)
							{
								codeName += item.SectionName + "\r\n";
								break;
							}
						}
						if (codeId.Length == 7)
						{
							int codeIdEnd = int.Parse(codeId[6].ToString());
							string json = File.ReadAllText(path);
							List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
							foreach (var item in items)
							{
								if (item.FormId == codeIdEnd)
								{
									codeName += "Dạng " + codeIdEnd + ":" + item.FormName + "\r\n";
									break;
								}
							}
						}
						Dic.Add("codeName", codeName);
					}
				}
				catch
				{

				}
				list.Add(Dic);
				startIndex = num2;
			}
			return list;
		}
		public List<dynamic> FilterId2(string pathone, string Type, Regex rx)
		{
			List<string> listEcersiceY = new List<string>();
			List<string> listEcersiceB = new List<string>();
			List<string> listEcersiceK = new List<string>();
			List<string> listEcersiceG = new List<string>();
			List<string> listEcersiceT = new List<string>();
			List<dynamic> list = new List<dynamic>();
			string str = @"\begin{" + Type + "}";
			string str2 = @"\end{" + Type + "}";
			string appPath = Directory.GetCurrentDirectory() + @"\Id";
			IEnumerable<string> enumerable = Directory.EnumerateFiles(pathone, "*.docx");
			foreach (string file in enumerable)
			{
				int startIndex = 0;
				string fileName = Path.GetFileNameWithoutExtension(file);
				if (rx.IsMatch(fileName))
				{
					string str3 = File.ReadAllText(file);
					char checkchar = fileName[3];
					while (str3.IndexOf(str, startIndex)>0)
					{
						try
						{
							startIndex = str3.IndexOf(str, startIndex);
							int endIndex = str3.IndexOf(str2, startIndex);
							string input = str3.Substring(startIndex, endIndex - startIndex+ str2.Length);
							if (checkchar == 'Y') { listEcersiceY.Add(input); }
							if (checkchar == 'B') { listEcersiceB.Add(input); }
							if (checkchar == 'K') { listEcersiceK.Add(input); }
							if (checkchar == 'G') { listEcersiceG.Add(input); }
							if (checkchar == 'T') { listEcersiceT.Add(input); }
							startIndex = endIndex;
						}
						catch
						{

						}
					}
				}
			}
			list.Add(listEcersiceY);
			list.Add(listEcersiceB);
			list.Add(listEcersiceK);
			list.Add(listEcersiceG);
			list.Add(listEcersiceT);
			return list;
		}
		public List<dynamic> FilterId3(string pathone, string Type, Regex rx)
		{
			int startIndex = 0;
			List<dynamic> list = new List<dynamic>();
			string str = @"\begin{" + Type + "}";
			string str2 = @"\end{" + Type + "}";
			string appPath = Directory.GetCurrentDirectory() + @"\Id";
			IEnumerable<string> enumerable = Directory.EnumerateFiles(pathone, "*.docx");
			foreach (string file in enumerable)
			{
				string fileName = Path.GetFileNameWithoutExtension(file);
				if (rx.IsMatch(fileName))
				{
					string str3 = File.ReadAllText(file);
					while (startIndex <= str3.LastIndexOf(str))
					{

						Dictionary<string, dynamic> Dic = new Dictionary<string, dynamic>();
						startIndex = str3.IndexOf(str, startIndex);
						int num2 = str3.IndexOf(str2, startIndex) + str2.Length;
						string input = str3.Substring(startIndex, num2 - startIndex);
						string question = "";
						Dic.Add("all", input);
						if (input.Contains(@"\loigiai"))
						{
							int start = input.IndexOf(@"\loigiai");
							question = input.Substring(0, start);
						}
						else
						{
							question = input;
						}
						Dic.Add("exersice", question);
						try
						{
							if (rx.Matches(input).Count > 0)
							{
								string codeId = "";
								string codeLevel = "";
								string codeName = "";
								if (rx.Matches(input).Count == 1)
								{
									Match first = rx.Match(input);
									codeId = first.Value.Replace("[", "").Replace("]", "");
									codeLevel = codeId[3].ToString();
								}
								if (rx.Matches(input).Count > 1)
								{
									Dic.Add("exersice", input);
									List<string> listId = new List<string>();

									foreach (Match match in rx.Matches(input))
									{
										if (match.Value.Contains("T"))
										{
											codeId = match.Value;
											codeLevel = "T";
											break;
										}
										else if (!codeId.Contains("G") && match.Value.Contains("G"))
										{
											codeId = match.Value;
											codeLevel = "G";
										}
										else if (!codeId.Contains("G") && !codeId.Contains("K") && match.Value.Contains("K"))
										{
											codeId = match.Value;
											codeLevel = "K";
										}
										else if (!codeId.Contains("G") && !codeId.Contains("K") && !codeId.Contains("B") && match.Value.Contains("B"))
										{
											codeId = match.Value;
											codeLevel = "B";
										}
										else if (!codeId.Contains("G") && !codeId.Contains("K") && !codeId.Contains("B"))
										{
											codeId = match.Value;
											codeLevel = "Y";
										}
									}
								}
								codeId = codeId.Replace("[", "").Replace("]", "");
								Dic.Add("codeId", codeId);
								Dic.Add("codeLevel", codeLevel);
								string codeIdSub = codeId.Substring(0, 5);
								string codeIdClass = codeId.Substring(0, 3);
								int indexClass = int.Parse(codeId[0].ToString());
								int indexChapter = int.Parse(codeId[2].ToString());
								char indexSubject = codeId[1];
								int indexSection = int.Parse(codeId[4].ToString());
								codeName += "Lớp: 1" + indexClass + "\r\n";
								if (indexSubject == 'D')
								{
									codeName += "Phân môn: Đại số\r\n";
								}
								else
								{
									codeName += "Phân môn: Hình học\r\n";
								}
								string path = appPath + @"\DangBai1" + indexClass + @"\1" + codeIdClass + "F" + indexSection + @".json";
								string path2 = appPath + @"\Lop\1" + codeIdClass + @".json";
								string path3 = appPath + @"\Lop\Class1" + indexClass + @".json";
								string json3 = File.ReadAllText(path3);
								List<Chapter> chapter = JsonConvert.DeserializeObject<List<Chapter>>(json3);
								foreach (var item in chapter)
								{
									if (item.ChapterId == indexChapter && item.ObjectName == indexSubject)
									{
										codeName += "Chương " + indexChapter + ":" + item.ChapterName + "\r\n";
										break;
									}
								}
								string json2 = File.ReadAllText(path2);
								List<Section> Sections = JsonConvert.DeserializeObject<List<Section>>(json2);
								foreach (var item in Sections)
								{
									if (item.SectionId == indexSection)
									{
										codeName += item.SectionName + "\r\n";
										break;
									}
								}
								if (codeId.Length == 7)
								{
									int codeIdEnd = int.Parse(codeId[6].ToString());
									string json = File.ReadAllText(path);
									List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
									foreach (var item in items)
									{
										if (item.FormId == codeIdEnd)
										{
											codeName += "Dạng " + codeIdEnd + ":" + item.FormName + "\r\n";
											break;
										}
									}
								}
								Dic.Add("codeName", codeName);
								list.Add(Dic);
								startIndex = num2;
							}
						}
						catch
						{

						}
					}
				}
			}
			return list;
		}
		public void newFileTex(List<string> list, string Path, string FormHd, string FormFt)
		{
			try
			{
				if (Path != "")
				{
					if (File.Exists(Path))
					{
						File.Delete(Path);
					}
					File.WriteAllText(Path, FormHd + "\n");
					foreach (string str in list)
					{
						File.AppendAllText(Path, str + "\n");
					}
					if (File.Exists(Path))
					{
						MessageBox.Show("Đã tạo file thành công", "Thành công");
					}
					else
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
				MessageBox.Show(e.Message, "Thoát");
			}
		}
	}
}
