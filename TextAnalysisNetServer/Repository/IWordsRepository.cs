using System.Collections.Generic;

namespace TextAnalysis
{
	public interface IWordsRepository
	{
		AnalysedText AddSimpleWords(string path);
		List<IrregularObject> AddIrregulars(string path);
		List<List<string>> AddRelationalWords(string path);
		int DeleteAllWords();
		List<TemporalObject> AddTemporalSimpleWords(string path);
		List<TemporalObjectForIrregular> AddTemporalIrregulars(string path);
		List<TemporalObject> AddTemporalRelationalWords(string path);
		int DeleteAllTemporalWords();
		List<object> GetAllTemporalWords();
		List<object> GetAllWords();
	}
}
