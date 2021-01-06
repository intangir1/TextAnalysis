using System.Collections.Generic;

namespace TextAnalysis
{
	public interface ISingleWordsRepository
	{
		List<string> GetAllWords();
		string PostWord(string word);
		string PutWord(string word, string connectionWord);
		int DeleteWord(string word);
		int DeleteCollection();
		bool IfWordExists(string word);
	}
}
