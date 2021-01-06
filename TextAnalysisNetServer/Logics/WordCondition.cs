using System.Linq;
using System.Text.RegularExpressions;

namespace TextAnalysis
{
	public static class WordCondition
	{
		private static string prefixesPattern = @"^(re|dis|over|un|mis|out|de|fore|inter|pre|sub|trans|under)";
		private static string suffixesPattern = @"(ing|ed|ies)$";
		private static string rgx_acceptable_endings = @"(see|add|ass|ess|all|ill|ell|oll|iss|ree|ess|uzz|oss|lee|iff)$";
		private static string exceptionsPattern = @"(go|be|do)";
		private static string exceptionsEndingPattern = "(thing)$";
		private static string notContainsPattern = @"(with|the|and|she)";

		private static Regex prefixesRegex = new Regex(prefixesPattern);
		private static Regex suffixesRegex = new Regex(suffixesPattern);
		private static Regex exceptionsEndingRegex = new Regex(exceptionsEndingPattern);

		public static string GetClearWord(string word)
		{
			string tempWord;

			tempWord = prefixesRegex.Replace(word, "");

			if (!tempWord.Equals(word) && WordAcccepted(tempWord))
			{
				word = tempWord;
			}

			tempWord = suffixesRegex.Replace(word, "");

			if (!tempWord.Equals(word) && !Regex.IsMatch(word, exceptionsEndingPattern) && WordAcccepted(tempWord))
			{
				word = tempWord;
				if (word.Length > 2)
				{
					string ending = word.Substring(word.Length - 2);
					if (ending.Distinct().Count() == 1 && !Regex.IsMatch(word, rgx_acceptable_endings))
					{
						word = word.Substring(0, word.Length - 1);
					}
				}
			}
			return word;
		}

		public static bool WordAcccepted(string word){
			if(word!=null && !word.Equals("") && (word.Length>2 || Regex.IsMatch(word, exceptionsPattern))){
			  return true;
			}
			return false;
		}

		public static bool WordContainedNotIgnor(string word)
		{
			if (word != null && !word.Equals("") && !Regex.IsMatch(word, notContainsPattern))
			{
				return true;
			}
			return false;
		}
	}
}
