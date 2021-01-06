namespace TextAnalysis
{
	public interface ITextAnalysisDatabaseSettings
	{
		string ConnectionString { get; set; }

		string DatabaseName { get; set; }

		string UsersCollectionName { get; set; }

		string UserAnaliticsCollectionName { get; set; }

		string SynonimsCollectionName { get; set; }

		string AntonimsCollectionName { get; set; }

		string ArchaismsCollectionName { get; set; }

		string SlangsCollectionName { get; set; }

		string IrregularsCollectionName { get; set; }

		string EstablishedExpressionsCollectionName { get; set; }

		string TemporalSynonimsCollectionName { get; set; }

		string TemporalAntonimsCollectionName { get; set; }

		string TemporalArchaismsCollectionName { get; set; }

		string TemporalSlangsCollectionName { get; set; }

		string TemporalIrregularsCollectionName { get; set; }

		string TemporalEstablishedExpressionsCollectionName { get; set; }
	}
}
