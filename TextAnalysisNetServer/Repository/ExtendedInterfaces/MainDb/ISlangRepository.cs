using System.Collections.Generic;

namespace TextAnalysis
{
	public interface ISlangRepository : ISingleWordsRepository {
		HashSet<string> GetIntersects(string[] words, string[] excepts = null);
	}
}
