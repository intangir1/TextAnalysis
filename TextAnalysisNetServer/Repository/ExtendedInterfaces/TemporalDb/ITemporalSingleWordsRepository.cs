using System.Collections.Generic;

namespace TextAnalysis
{
	public interface ITemporalSingleWordsRepository
	{
		List<TemporalObject> GetAllWords(string collectionName);
		TemporalObject PostWord(string collectionName, string type, string word);
		TemporalObject PutWord(string collectionName, string type, string word, string connectionWord);
		int DeleteWord(string collectionName, string word);
		int DeleteCollection(string collectionName);
		bool IfWordExists(string collectionName, string word);
	}
}
