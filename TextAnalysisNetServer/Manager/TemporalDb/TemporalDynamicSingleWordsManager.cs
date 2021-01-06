using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TextAnalysis
{
	public class TemporalDynamicSingleWordsManager : ITemporalSingleWordsRepository
	{
		private readonly MongoClient client;
		private readonly IMongoDatabase database;
		private IMongoCollection<TemporalObject> mongoCollection;

		public TemporalDynamicSingleWordsManager()
		{
			client = new MongoClient(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:ConnectionString"));
			database = client.GetDatabase(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:DatabaseName"));
		}

		public List<TemporalObject> GetAllWords(string collectionName)
		{
			mongoCollection = database.GetCollection<TemporalObject>(Startup.staticConfiguration.GetValue<string>(collectionName));
			List<TemporalObject> mongoCollections = mongoCollection.Find(_mongoCollection => true).Project(temporalMongoObject => new TemporalObject
			{
				mongoId = temporalMongoObject.mongoId,
				action = temporalMongoObject.action,
				connectionWord = temporalMongoObject.connectionWord,
				inputedWord = temporalMongoObject.inputedWord,
				type = temporalMongoObject.type
			}).ToList();
			Debug.WriteLine("mongoCollection GetAllWords: " + mongoCollections);
			return mongoCollections;
		}

		public TemporalObject GetWordBy(string collectionName, string word)
		{
			word = word.ToLower();
			mongoCollection = database.GetCollection<TemporalObject>(Startup.staticConfiguration.GetValue<string>(collectionName));
			TemporalObject addedmongoCollection = mongoCollection.Find(_mongoCollection => _mongoCollection.inputedWord.Equals(word)).Project(temporalMongoObject => new TemporalObject
			{
				mongoId = temporalMongoObject.mongoId,
				action = temporalMongoObject.action,
				connectionWord = temporalMongoObject.connectionWord,
				inputedWord = temporalMongoObject.inputedWord,
				type = temporalMongoObject.type
			}).FirstOrDefault();
			Debug.WriteLine("mongoCollection GetWordBy: " + addedmongoCollection);
			return addedmongoCollection;
		}

		public TemporalObject PostWord(string collectionName, string type, string word)
		{
			type = type.ToLower();
			word = word.ToLower();
			mongoCollection = database.GetCollection<TemporalObject>(Startup.staticConfiguration.GetValue<string>(collectionName));
			TemporalObject temporalMongoObject = new TemporalObject("Post", type, word);
			mongoCollection.InsertOne(temporalMongoObject);
			Debug.WriteLine("mongoCollection PostWord: " + word);
			return GetWordBy(collectionName, word);
		}

		public TemporalObject PutWord(string collectionName, string type, string tmpInputedWord, string tmpConnectionWord)
		{
			type = type.ToLower();
			tmpConnectionWord = tmpConnectionWord.ToLower();
			tmpInputedWord = tmpInputedWord.ToLower();
			mongoCollection = database.GetCollection<TemporalObject>(Startup.staticConfiguration.GetValue<string>(collectionName));
			TemporalObject temporalMongoObject = new TemporalObject("Put", type, tmpInputedWord, tmpConnectionWord);
			mongoCollection.InsertOne(temporalMongoObject);
			Debug.WriteLine("mongoCollection PutWord: " + tmpConnectionWord + ", " + tmpInputedWord);
			return GetWordBy(collectionName, tmpInputedWord);
		}

		public int DeleteWordByWord(string collectionName, string wordToRemove)
		{
			wordToRemove = wordToRemove.ToLower();
			mongoCollection = database.GetCollection<TemporalObject>(Startup.staticConfiguration.GetValue<string>(collectionName));
			TemporalObject tmpTemporalMongoObject = mongoCollection.Find(_mongoCollection => _mongoCollection.inputedWord.Equals(wordToRemove)).Project(temporalMongoObject => new TemporalObject
			{
				mongoId = temporalMongoObject.mongoId,
				action = temporalMongoObject.action,
				connectionWord = temporalMongoObject.connectionWord,
				inputedWord = temporalMongoObject.inputedWord,
				type = temporalMongoObject.type
			}).FirstOrDefault();

			int deleted = 0;
			if (tmpTemporalMongoObject != null)
			{
				deleted = (int)mongoCollection.DeleteOne(_mongoCollection => _mongoCollection.mongoId.Equals(tmpTemporalMongoObject.mongoId)).DeletedCount;
			}
			if (deleted > 0)
			{
				Debug.WriteLine("mongoCollection DeleteWord: " + true);
			}
			return deleted;
		}

		public int DeleteWord(string collectionName, string mongoId)
		{
			int deleted = 0;
			mongoCollection = database.GetCollection<TemporalObject>(Startup.staticConfiguration.GetValue<string>(collectionName));
			if (mongoId != null && !mongoId.Trim().Equals("") && !mongoId.Trim().Equals(string.Empty))
			{
				deleted = (int)mongoCollection.DeleteOne(_mongoCollection => _mongoCollection.mongoId.Equals(mongoId)).DeletedCount;
			}
			if (deleted > 0)
			{
				Debug.WriteLine("mongoCollection DeleteWord: " + true);
			}
			return deleted;
		}

		public int DeleteCollection(string collectionName)
		{
			mongoCollection = database.GetCollection<TemporalObject>(Startup.staticConfiguration.GetValue<string>(collectionName));
			int deleted = (int)mongoCollection.DeleteMany(_mongoCollection => true).DeletedCount;
			if (deleted > 0)
			{
				Debug.WriteLine("mongoCollection DeleteCollection: " + true);
			}
			return deleted;
		}

		public bool IfWordExists(string collectionName, string word)
		{
			word = word.ToLower();
			mongoCollection = database.GetCollection<TemporalObject>(Startup.staticConfiguration.GetValue<string>(collectionName));
			TemporalObject mongoCollections = mongoCollection.Find(_mongoCollection => _mongoCollection.inputedWord.Equals(word)).Project(temporalMongoObject => new TemporalObject
			{
				mongoId = temporalMongoObject.mongoId,
				action = temporalMongoObject.action,
				connectionWord = temporalMongoObject.connectionWord,
				inputedWord = temporalMongoObject.inputedWord,
				type = temporalMongoObject.type
			}).FirstOrDefault();

			bool collectionExists=false;
			if (mongoCollections!=null)
				collectionExists = true;

			Debug.WriteLine("mongoCollection IfWordExists: " + collectionExists);
			return collectionExists;
		}
	}
}
