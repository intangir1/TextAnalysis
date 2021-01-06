using System.Linq;

namespace TextAnalysis
{
	public static class CleanSentence
	{
		public static string[] GetCleanWordsArray(string[] words)
		{
			string[] clean_words;
			clean_words = words.Clone() as string[];
			clean_words = clean_words.Select(word => WordCondition.GetClearWord(word)).ToArray();
			return clean_words;
		}
	}
}
