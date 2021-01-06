using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TextAnalysis
{
	[DataContract]
	public class MongoObject
	{
		private string _mongoId;
		private string _mongoIdForAntonim;
		private List<string> _strings;

		public MongoObject()
		{

		}

		public MongoObject(string _tmpMongoId, List<string> _tmpStrings)
		{
			mongoId = _tmpMongoId;
			strings = _tmpStrings;
		}

		public MongoObject(string _word)
		{
			strings = new List<string>();
			strings.Add(_word.ToLower());
		}

		public MongoObject(List<string> _tmpStrings)
		{
			strings = _tmpStrings;
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
		[BsonRepresentation(BsonType.ObjectId)]
		[BsonIgnoreIfDefault]    // <--- this is what was missing
		public string mongoIdForAntonim
		{
			get { return _mongoIdForAntonim; }
			set { _mongoIdForAntonim = value; }
		}

		[DataMember]
		public List<string> strings
		{
			get {
				_strings.ForEach(word => word.ToLower());
				return _strings;
			}
			set {
				value.ForEach(word => word.ToLower()); 
				_strings = value;
			}
		}

		public override string ToString()
		{
			string stringsStr = string.Join(", ", strings.ToArray());
			return
				mongoId + ". " + mongoIdForAntonim + ". " + stringsStr;
		}
	}
}