using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextAnalysis
{
	public static class Split
	{
		public static string[] SplitTextToWords(string text)
		{
			string textNoHtml = Regex.Replace(text, "<.*?>", String.Empty);
			Regex everyPointPattern = new Regex("[;:,?!.]+");
			textNoHtml = everyPointPattern.Replace(textNoHtml, string.Empty);
			string[] words = textNoHtml.Split(' ');
			return words;
		}

		public static string[] SplitTextToSentences(string text)
		{
			string endOfSentencepattern = @"[?!.]+";
			string[] words = (string[])Regex.Split(text, endOfSentencepattern).Select(item => item.Trim());
			return words;
		}
	}
}
