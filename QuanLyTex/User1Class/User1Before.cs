using System;
using System.Collections.Generic;


namespace QuanLyTex
{
	class User1Before
	{
		public List<string> CommentOrder(List<string> list)
		{
			try
			{
				int num = 1;
				List<string> list2 = new List<string>();
				foreach (string str in list)
				{
					string str1 = str.Replace(@"\begin{ex}", @"\begin{ex}%[Câu " + num + "]");
					list2.Add(str1);
					num++;
				}
				return list2;
			}
			catch (Exception e)
			{
				return list;
			}
		}
	}
}
