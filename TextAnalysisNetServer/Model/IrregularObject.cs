using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Runtime.Serialization;

namespace TextAnalysis
{
	[DataContract]
	public class IrregularObject
	{
		private string _mongoId;
		private string _first;
		private string _second;
		private string _third;

		public IrregularObject()
		{

		}

		public IrregularObject(string _tmpMongoId, string tmpFirst, string tmpSecond, string tmpThird)
		{
			mongoId = _tmpMongoId;
			first = tmpFirst;
			second = tmpSecond;
			third = tmpThird;
		}

		public IrregularObject(string tmpFirst, string tmpSecond, string tmpThird)
		{
			first = tmpFirst;
			second = tmpSecond;
			third = tmpThird;
		}

		[DataMember]
		[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
		[BsonRepresentation(BsonType.ObjectId)]
		[BsonIgnoreIfDefault]    // <--- this is what was missing
		public string mongoId
		{
			get { return _mongoId; }
			set { _mongoId = value; }
		}

		[DataMember]
		public string first
		{
			get {
				if (_first != null && _first != string.Empty && _first != "")
				{
					return _first.ToLower();
				}
				else
				{
					return _first;
				}
			}
			set { _first = value.ToLower(); }
		}

		[DataMember]
		public string second
		{
			get {
				if (_second != null && _second != string.Empty && _second != "")
				{
					return _second.ToLower();
				}
				else
				{
					return _second;
				}
			}
			set { _second = value.ToLower(); }
		}

		[DataMember]
		public string third
		{
			get {
				if (_third != null && _third != string.Empty && _third != "")
				{
					return _third.ToLower();
				}
				else
				{
					return _third;
				}
			}
			set { _third = value.ToLower(); }
		}

		public override string ToString()
		{
			return mongoId + ". " + first + ". " + second + ". " + third;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is IrregularObject))
			{
				return false;
			}

			return first.Equals(((IrregularObject)obj).first)
					&& second.Equals(((IrregularObject)obj).second)
					&& third.Equals(((IrregularObject)obj).third);
		}

		public bool ContainsWord(string word)
		{
			return first.Contains(word) || second.Contains(word) || third.Contains(word);
		}

		public bool EqualsWord(string word)
		{
			return this.first.Equals(word) || this.second.Equals(word) || this.third.Equals(word);
		}

		public bool ContainedInWord(string word)
		{
			return word.Contains(first) || word.Contains(second) || word.Contains(third);
		}

		public bool ContainsIntersect(string word1, string word2)
		{
			if (!word1.Equals(word2) && ContainedInWord(word1) && ContainedInWord(word2))
			{
				return true;
			}
			return false;
		}

		public bool LengthMore(int num)
		{
			return first.Length>num || second.Length > num || third.Length > num;
		}

		public IrregularObject ToLower()
		{
			if(first!=null && first != string.Empty && first != "")
			{
				first = first.ToLower();
			}
			if (second != null && second != string.Empty && second != "")
			{
				second = second.ToLower();
			}
			if (third != null && third != string.Empty && third != "")
			{
				third = third.ToLower();
			}
			return this;
		}
	}
}
