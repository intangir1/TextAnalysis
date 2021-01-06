using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TextAnalysis
{
	public static class CheckStringFormat
	{
		public static bool IsBase64String(string str)
		{
			str = str.Trim();
			Debug.WriteLine("IsBase64String Has been Trimed: " + str);
			return (str.Length % 4 == 0) && Regex.IsMatch(str, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

		}
	}
}
