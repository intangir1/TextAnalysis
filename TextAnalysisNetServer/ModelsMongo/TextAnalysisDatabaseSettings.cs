namespace TextAnalysis
{
	public class TextAnalysisDatabaseSettings: ITextAnalysisDatabaseSettings
	{
		public string ConnectionString { get; set; }

		public string DatabaseName { get; set; }

		public string UsersCollectionName { get; set; }

		public string UserAnaliticsCollectionName { get; set; }

		public string SynonimsCollectionName { get; set; }

		public string AntonimsCollectionName { get; set; }

		public string ArchaismsCollectionName { get; set; }

		public string SlangsCollectionName { get; set; }

		public string IrregularsCollectionName { get; set; }

		public string EstablishedExpressionsCollectionName { get; set; }

		public string TemporalSynonimsCollectionName { get; set; }

		public string TemporalAntonimsCollectionName { get; set; }

		public string TemporalArchaismsCollectionName { get; set; }

		public string TemporalSlangsCollectionName { get; set; }

		public string TemporalIrregularsCollectionName { get; set; }

		public string TemporalEstablishedExpressionsCollectionName { get; set; }
	}
}
