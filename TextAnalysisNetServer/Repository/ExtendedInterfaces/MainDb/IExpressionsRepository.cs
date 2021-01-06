using System.Collections.Generic;

namespace TextAnalysis
{
	public interface IExpressionsRepository : ISingleWordsRepository
	{
		HashSet<string> GetIntersects(string text);
	}
}
