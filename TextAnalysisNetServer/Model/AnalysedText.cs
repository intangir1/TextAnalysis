using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TextAnalysis
{
	[DataContract]
	public class AnalysedText
	{
		private HashSet<string> _repeated = new HashSet<string>();
		private HashSet<string> _archaisms = new HashSet<string>();
		private HashSet<string> _slangs = new HashSet<string>();
		private HashSet<string> _irregulars = new HashSet<string>();
		private HashSet<string> _expressions = new HashSet<string>();

		public AnalysedText() { }

		public AnalysedText(
			HashSet<string> tmpRepeated,
			HashSet<string> tmpArchaisms,
			HashSet<string> tmpSlangs,
			HashSet<string> tmpIrregulars,
			HashSet<string> tmpExpressions)
		{
			repeated.UnionWith(tmpRepeated);
			archaisms.UnionWith(tmpArchaisms);
			slangs.UnionWith(tmpSlangs);
			irregulars.UnionWith(tmpIrregulars);
			expressions.UnionWith(tmpExpressions);
		}

		[DataMember]
		public HashSet<string> repeated
		{
			get { return _repeated; }
			set { _repeated = value; }
		}

		[DataMember]
		public HashSet<string> archaisms
		{
			get { return _archaisms; }
			set { _archaisms = value; }
		}

		[DataMember]
		public HashSet<string> slangs
		{
			get { return _slangs; }
			set { _slangs = value; }
		}

		[DataMember]
		public HashSet<string> irregulars
		{
			get { return _irregulars; }
			set { _irregulars = value; }
		}

		[DataMember]
		public HashSet<string> expressions
		{
			get { return _expressions; }
			set { _expressions = value; }
		}

		public bool IfDataExists()
		{
			if (repeated.Count > 0)
				return true;
			else if (archaisms.Count > 0)
				return true;
			else if (slangs.Count > 0)
				return true;
			else if (irregulars.Count > 0)
				return true;
			else if (expressions.Count > 0)
				return true;
			return false;
		}

		public override string ToString()
		{
			string repeatedStr = string.Join(", ", repeated);
			string archaismsStr = string.Join(", ", archaisms);
			string slangsStr = string.Join(", ", slangs);
			string irregularsStr = string.Join(", ", irregulars);
			string expressionsStr = string.Join(", ", expressions);

			return
				repeatedStr + ". " + archaismsStr + ". " + slangsStr + ". " + irregularsStr + ". " + expressionsStr;
		}
	}
}
