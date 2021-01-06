using System.Collections.Generic;

namespace TextAnalysis
{
	public interface IIrregularVerbsRepository
	{
		List<IrregularObject> GetAllData();
		IrregularObject PostData(IrregularObject irregularObject);
		IrregularObject PutData(IrregularObject word, string connectionWord);
		int DeleteData(string word);
		int DeleteCollection();
		bool IfWordExists(IrregularObject word);
		HashSet<string> GetIntersects(string[] words, string[] excepts = null);
	}
}
