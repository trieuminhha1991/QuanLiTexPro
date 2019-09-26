using System;
using System.Collections.Generic;
using System.IO;


namespace WpfApp1
{
	class CreatExamTex
	{
		public int[] GenRandomPermutation(int n)
		{
			Random r = new Random();
			int[] a = new int[n];
			for (int i = 0; i < n; i++)
			{
				a[i] = i;
			}
			for (int i = 0; i < n; i++)
			{
				int j = r.Next(n);
				int t = a[0];
				a[0] = a[j];
				a[j] = t;
			}
			return a;
		}
		public List<string> mixExamTex(List<string> listY, List<string> listB, List<string> listK, List<string> listG, List<string> listT, int numberExam,bool? Reversed,int numberY,int numberB, int numberK, int numberG,int numberT)
		{
			try
			{
				int i = 1;
				List<string> listExam = new List<string>();
				string texExam = "";
				int[] a;
				while (i <= numberExam)
				{
					texExam = "";
					if (listY != null&& listY.Count> 0)
					{
						a = GenRandomPermutation(listY.Count);
						for (int y = 0; y <= numberY; y++)
						{
							texExam += listY[a[y]]+"\n";
						}
					}
					if (listB != null && listB.Count > 0)
					{
						a = GenRandomPermutation(listB.Count);
						for (int b = 0; b <= numberB; b++)
						{
							texExam += listB[a[b]] + "\n";
						}
					}
					if (listK != null && listK.Count > 0)
					{
						a = GenRandomPermutation(listK.Count);
						for (int k = 0; k <= numberK; k++)
						{
							texExam += listK[a[k]] + "\n";
						}
					}
					Random randomT = new Random();
					if (listT != null && listT.Count > 0)
					{
						a = GenRandomPermutation(listT.Count);
						for (int k = 0; k <= numberT; k++)
						{
							texExam += listK[a[k]] + "\n";
						}
					}
					Random randomG = new Random();
					if (listG != null && listG.Count > 0)
					{
						a = GenRandomPermutation(listG.Count);
						for (int g = 0; g <= numberG; g++)
						{
							texExam += listG[a[g]] + "\n";
						}
					}
					listExam.Add(texExam);
					i++;
				}
				return listExam;
			}
			catch (Exception a)
			{
				return null;
			}
		}
		public string newFileTex(List<string> list, string Path)
		{
			try
			{
				DateTime time = DateTime.Now;
				String folderName = time.ToString("yyyy.MM.dd");
				String pathFolder = Path + @"\DeThi" + folderName;
				Directory.CreateDirectory(pathFolder);
				string filePath;
				for(int i=1; i<=list.Count;i++)
				{
					filePath = pathFolder + @"\Đề " + i + ".tex";
					File.AppendAllText(filePath, list[i-1]);
				}
				return pathFolder;
			}
			catch (Exception e)
			{
				return null;
			}
		}
	}
}
