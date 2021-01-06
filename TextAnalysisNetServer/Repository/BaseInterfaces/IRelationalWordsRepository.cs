using System.Collections.Generic;

namespace TextAnalysis
{
	public interface IRelationalWordsRepository
	{
		List<List<string>> GetAllWords();
		List<string> GetWordsBy(string word);
		List<string> PostWord(string word, string connectionWord = "");
		List<string> PostCollection(List<string> words);
		List<string> PostCollection(List<string> antonimWords, string connectionWord="");
		List<string> PutWord(string word, string connectionWord = "");
		List<string> InsertWord(string word, string connectionWord = "");
		List<string> DeleteWord(string word);
		int DeleteCollection(string word);
		int DeleteCollection();
		bool IfWordExists(string word);

	}
}
