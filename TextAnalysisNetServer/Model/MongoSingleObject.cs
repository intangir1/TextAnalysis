using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Runtime.Serialization;

namespace TextAnalysis
{
	[DataContract]
	public class MongoSingleObject
	{
		private string _mongoId;
		private string _word;

		public MongoSingleObject()
		{

		}

		public MongoSingleObject(string _tmpMongoId, string tmpWord)
		{
			mongoId = _tmpMongoId;
			word = tmpWord;
		}

		public MongoSingleObject(string tmpWord)
		{
			word = tmpWord;
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
		public string word
		{
			get { return _word.ToLower(); }
			set { _word = value.ToLower(); }
		}

		public override string ToString()
		{
			return mongoId + ". " + word;
		}
	}
}
