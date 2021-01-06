using System.Collections.Generic;

namespace TextAnalysis
{
	public interface ITemporalRelationalWordsRepository
	{
		List<TemporalObject> GetAllWords(string collectionName);
		TemporalObject PostWord(string collectionName, string type, string word = "", string connectionWord = "");
		TemporalObject PutWord(string collectionName, string type, string word, string connectionWord);
		TemporalObject InsertWord(string collectionName, string type, string word, string connectionWord);
		int DeleteWord(string collectionName, string word);
		int DeleteCollection(string collectionName);
		bool IfWordExists(string collectionName, string word);
	}
}
