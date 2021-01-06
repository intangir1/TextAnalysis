using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TextAnalysis
{
	public class TemporalIrregularManager: ITemporalIrregularRepository
	{
		private readonly MongoClient client;
		private readonly IMongoDatabase database;
		private IMongoCollection<TemporalObjectForIrregular> mongoCollection;

		public TemporalIrregularManager()
		{
			client = new MongoClient(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:ConnectionString"));
			database = client.GetDatabase(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:DatabaseName"));
		}

		public List<TemporalObjectForIrregular> GetAllWords(string collectionName)
		{
			mongoCollection = database.GetCollection<TemporalObjectForIrregular>(Startup.staticConfiguration.GetValue<string>(collectionName));
			List<TemporalObjectForIrregular> mongoCollections = mongoCollection.Find(_mongoCollection => true).Project(temporalMongoObjectForIrregular => new TemporalObjectForIrregular
			{
				mongoId = temporalMongoObjectForIrregular.mongoId,
				action = temporalMongoObjectForIrregular.action,
				connectionWord = temporalMongoObjectForIrregular.connectionWord,
				inputedWord = temporalMongoObjectForIrregular.inputedWord,
				type = temporalMongoObjectForIrregular.type
			}).ToList();
			Debug.WriteLine("mongoCollection GetAllWords: " + mongoCollections);
			return mongoCollections;
		}

		public TemporalObjectForIrregular GetWordBy(string collectionName, string word)
		{
			word = word.ToLower();
			mongoCollection = database.GetCollection<TemporalObjectForIrregular>(Startup.staticConfiguration.GetValue<string>(collectionName));
			TemporalObjectForIrregular addedmongoCollection = mongoCollection.Find(_mongoCollection => _mongoCollection.inputedWord.first.Equals(word) || _mongoCollection.inputedWord.second.Equals(word) || _mongoCollection.inputedWord.third.Equals(word)).Project(temporalMongoObjectForIrregular => new TemporalObjectForIrregular
			{
				mongoId = temporalMongoObjectForIrregular.mongoId,
				action = temporalMongoObjectForIrregular.action,
				connectionWord = temporalMongoObjectForIrregular.connectionWord,
				inputedWord = temporalMongoObjectForIrregular.inputedWord,
				type = temporalMongoObjectForIrregular.type
			}).FirstOrDefault();
			Debug.WriteLine("mongoCollection GetWordBy: " + addedmongoCollection);
			return addedmongoCollection;
		}

		public TemporalObjectForIrregular PostWord(string collectionName, IrregularObject word)
		{
			word = word.ToLower();
			mongoCollection = database.GetCollection<TemporalObjectForIrregular>(Startup.staticConfiguration.GetValue<string>(collectionName));
			TemporalObjectForIrregular temporalMongoObject = new TemporalObjectForIrregular("Post", "Irregular", word);
			mongoCollection.InsertOne(temporalMongoObject);
			Debug.WriteLine("mongoCollection PostWord: " + word);
			return GetWordBy(collectionName, word.first);
		}

		public TemporalObjectForIrregular PutWord(string collectionName, IrregularObject word, string connectionWord)
		{
			connectionWord = connectionWord.ToLower();
			word = word.ToLower();
			mongoCollection = database.GetCollection<TemporalObjectForIrregular>(Startup.staticConfiguration.GetValue<string>(collectionName));
			TemporalObjectForIrregular temporalMongoObject = new TemporalObjectForIrregular("Put", "Irregular", word, connectionWord);
			mongoCollection.InsertOne(temporalMongoObject);
			Debug.WriteLine("mongoCollection PutWord: " + connectionWord + ", " + word);
			return GetWordBy(collectionName, word.first);
		}

		public int DeleteWordByWord(string collectionName, string wordToRemove)
		{
			wordToRemove = wordToRemove.ToLower();
			mongoCollection = database.GetCollection<TemporalObjectForIrregular>(Startup.staticConfiguration.GetValue<string>(collectionName));
			TemporalObjectForIrregular tmpTemporalMongoObject = mongoCollection.Find(_mongoCollection => _mongoCollection.inputedWord.first.Equals(wordToRemove) || _mongoCollection.inputedWord.second.Equals(wordToRemove) || _mongoCollection.inputedWord.third.Equals(wordToRemove)).Project(temporalMongoObjectForIrregular => new TemporalObjectForIrregular
			{
				mongoId = temporalMongoObjectForIrregular.mongoId,
				action = temporalMongoObjectForIrregular.action,
				connectionWord = temporalMongoObjectForIrregular.connectionWord,
				inputedWord = temporalMongoObjectForIrregular.inputedWord,
				type = temporalMongoObjectForIrregular.type
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
			mongoCollection = database.GetCollection<TemporalObjectForIrregular>(Startup.staticConfiguration.GetValue<string>(collectionName));
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
			mongoCollection = database.GetCollection<TemporalObjectForIrregular>(Startup.staticConfiguration.GetValue<string>(collectionName));
			int deleted = (int)mongoCollection.DeleteMany(_mongoCollection => true).DeletedCount;
			if (deleted > 0)
			{
				Debug.WriteLine("mongoCollection DeleteCollection: " + true);
			}
			return deleted;
		}

		public bool IfWordExists(string collectionName, IrregularObject word)
		{
			word = word.ToLower();
			mongoCollection = database.GetCollection<TemporalObjectForIrregular>(Startup.staticConfiguration.GetValue<string>(collectionName));
			TemporalObjectForIrregular mongoCollections = mongoCollection.Find(_mongoCollection => _mongoCollection.inputedWord.first.Equals(word.first) && _mongoCollection.inputedWord.second.Equals(word.second) && _mongoCollection.inputedWord.third.Equals(word.third)).Project(temporalMongoObjectForIrregular => new TemporalObjectForIrregular
			{
				mongoId = temporalMongoObjectForIrregular.mongoId,
				action = temporalMongoObjectForIrregular.action,
				connectionWord = temporalMongoObjectForIrregular.connectionWord,
				inputedWord = temporalMongoObjectForIrregular.inputedWord,
				type = temporalMongoObjectForIrregular.type
			}).FirstOrDefault();

			bool collectionExists = false;
			if (mongoCollections != null)
				collectionExists = true;

			Debug.WriteLine("mongoCollection IfWordExists: " + collectionExists);
			return collectionExists;
		}
	}
}
