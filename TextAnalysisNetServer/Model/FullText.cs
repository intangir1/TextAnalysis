using System.Runtime.Serialization;

namespace TextAnalysis
{
	[DataContract]
	public class FullText
	{
		private string _textToCheck;

		[DataMember]
		public string textToCheck
		{
			get { return _textToCheck; }
			set { _textToCheck = value; }
		}

		public override string ToString()
		{
			return
				textToCheck;
		}
	}
}
