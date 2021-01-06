namespace TextAnalysis
{
	public interface IFullAnalyticsRepository
	{
		string AnalyseFullText(string text, int limit, string type);
		string CompareAllSentencesWords(string[] text, string type);
	}
}
