using System.Collections.Generic;

namespace TextAnalysis
{
	public interface ITemporalIrregularRepository
	{
		List<TemporalObjectForIrregular> GetAllWords(string collectionName);
		TemporalObjectForIrregular PostWord(string collectionName, IrregularObject word);
		TemporalObjectForIrregular PutWord(string collectionName, IrregularObject word, string connectionWord);
		int DeleteWord(string collectionName, string wordToRemove);
		int DeleteCollection(string collectionName);
		bool IfWordExists(string collectionName, IrregularObject word);
	}
}
