using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Runtime.Serialization;

namespace TextAnalysis
{
	[DataContract]
	public class TemporalObject
	{
		private string _mongoId;
		private string _action;
		private string _connectionWord;
		private string _inputedWord;
		private string _type;

		public TemporalObject()
		{

		}

		public TemporalObject(string tmpAction, string tmpType, string tmpInputedWord)
		{
			action = tmpAction;
			type = tmpType;
			inputedWord = tmpInputedWord;
			connectionWord = "";
		}

		public TemporalObject(string tmpAction, string tmpType, string tmpInputedWord, string tmpConnectionWord)
		{
			action = tmpAction;
			type = tmpType;
			inputedWord = tmpInputedWord;
			connectionWord = tmpConnectionWord;
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
		public string action
		{
			get { return _action.ToLower(); }
			set { _action = value.ToLower(); }
		}

		[DataMember]
		public string connectionWord
		{
			get { return _connectionWord.ToLower(); }
			set { _connectionWord = value.ToLower(); }
		}

		[DataMember]
		public string inputedWord
		{
			get { return _inputedWord.ToLower(); }
			set { _inputedWord = value.ToLower(); }
		}

		[DataMember]
		public string type
		{
			get { return _type.ToLower(); }
			set { _type = value.ToLower(); }
		}

		public override string ToString()
		{
			return
				action + ". " + connectionWord + ". " + inputedWord + ". " + type;
		}
	}
}
