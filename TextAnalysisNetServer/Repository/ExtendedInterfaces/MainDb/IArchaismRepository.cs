using System.Collections.Generic;

namespace TextAnalysis
{
	public interface IArchaismRepository: ISingleWordsRepository
	{
		HashSet<string> GetIntersects(string[] words, string[] excepts = null);
	}
}
