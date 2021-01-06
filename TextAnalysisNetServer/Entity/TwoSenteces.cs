using System.Runtime.Serialization;

namespace TextAnalysis
{
	[DataContract]
	public class TwoSenteces
	{
		private string _firstSentence;
		private string _secondSentence;

		public TwoSenteces()
		{
		}

		public TwoSenteces(string tmpFirstSentence, string tmpSecondSentence)
		{
			firstSentence = tmpFirstSentence;
			secondSentence = tmpSecondSentence;
		}

		[DataMember]
		public string firstSentence
		{
			get { return _firstSentence; }
			set { _firstSentence = value; }
		}

		[DataMember]
		public string secondSentence
		{
			get { return _secondSentence; }
			set { _secondSentence = value; }
		}

		public override string ToString()
		{
			return
				firstSentence + " " + secondSentence;
		}
	}
}
