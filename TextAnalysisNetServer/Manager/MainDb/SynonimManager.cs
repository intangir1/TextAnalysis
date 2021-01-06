using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TextAnalysis
{
	public class SynonimManager : ISynonimRepository
	{
		private readonly IMongoCollection<MongoObject> synonim;
		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalSynonimsCollectionName";

		public SynonimManager()
		{
			var client = new MongoClient(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:ConnectionString"));
			var database = client.GetDatabase(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:DatabaseName"));
			synonim = database.GetCollection<MongoObject>(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:SynonimsCollectionName"));
		}

		public List<List<string>> GetAllWords()
		{
			List<List<string>> synonims = synonim.Find(_synonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Select(x => x.strings).ToList();
			Debug.WriteLine("synonims GetAllWords: " + synonims);
			return synonims;
		}

		public List<string> GetWordsBy(string word)
		{
			word = word.ToLower();
			List<string> synonims = synonim.Find(_synonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Select(x => x.strings).Where(s => s.Contains(word)).FirstOrDefault();
			Debug.WriteLine("synonims GetWordsBy: " + synonims);
			return synonims;
		}

		public List<string> PostWord(string word, string connectionWord = "")
		{
			word = word.ToLower();

			MongoObject mongoObject = new MongoObject(word);
			synonim.InsertOne(mongoObject);
			Debug.WriteLine("synonims PostWord: " + word);
			new TemporalDynamicRelationalWordsManager().DeleteWordByWord(datacollection, word);
			return GetWordsBy(word);
		}

		public List<string> PostCollection(List<string> antonimWords, string connectionWord)
		{
			throw new System.NotImplementedException();
		}

		public List<string> PostCollection(List<string> words)
		{
			MongoObject mongoObject = new MongoObject(words);
			synonim.InsertOne(mongoObject);
			Debug.WriteLine("synonims PostCollection: " + words);
			return GetWordsBy(words[0]);
		}

		public List<string> PutWord(string word, string connectionWord)
		{
			word = word.ToLower();
			connectionWord = connectionWord.ToLower();

			MongoObject tmpMongoObject = synonim.Find(_synonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Where(s => s.strings.Contains(connectionWord)).FirstOrDefault();

			tmpMongoObject.strings = tmpMongoObject.strings.Select(s => s.Replace(connectionWord, word)).ToList();
			int modified = (int)synonim.ReplaceOne(_synonim => _synonim.mongoId.Equals(tmpMongoObject.mongoId), tmpMongoObject).ModifiedCount;
			Debug.WriteLine("synonims PutWord: " + word + " " + word);
			new TemporalDynamicRelationalWordsManager().DeleteWordByWord(datacollection, word);
			return GetWordsBy(word);
		}

		public List<string> InsertWord(string word, string connectionWord)
		{
			word = word.ToLower();
			connectionWord = connectionWord.ToLower();
			MongoObject tmpMongoObject = synonim.Find(_synonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Where(s => s.strings.Contains(connectionWord)).FirstOrDefault();

			tmpMongoObject.strings.Add(word);
			int modified = (int)synonim.ReplaceOne(_synonim => _synonim.mongoId.Equals(tmpMongoObject.mongoId), tmpMongoObject).ModifiedCount;
			Debug.WriteLine("synonims InsertWord: " + connectionWord + " " + word);
			new TemporalDynamicRelationalWordsManager().DeleteWordByWord(datacollection, word);
			return GetWordsBy(word);
		}

		public List<string> DeleteWord(string wordToRemove)
		{
			wordToRemove = wordToRemove.ToLower();
			MongoObject tmpMongoObject = synonim.Find(_synonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Where(s => s.strings.Contains(wordToRemove)).FirstOrDefault();

			tmpMongoObject.strings.Remove(wordToRemove);
			if (tmpMongoObject.strings.Count == 0)
			{
				int deleted = DeleteCollection(wordToRemove);
				if (deleted > 0) {
					Debug.WriteLine("synonims DeleteWord: " + "Whole object was deleted");
				}
				return GetWordsBy(wordToRemove);
			}
			else
			{
				int modified = (int)synonim.ReplaceOne(_synonim => _synonim.mongoId.Equals(tmpMongoObject.mongoId), tmpMongoObject).ModifiedCount;

				MongoObject tmpMongoObject2 = synonim.Find(Builders<MongoObject>.Filter.Eq(_synonim => _synonim.mongoId, tmpMongoObject.mongoId)).Project(mongoObject => new MongoObject
				{
					mongoId = mongoObject.mongoId,
					mongoIdForAntonim = mongoObject.mongoIdForAntonim,
					strings = mongoObject.strings
				}).FirstOrDefault();
				Debug.WriteLine("synonims DeleteWord: " + tmpMongoObject2.strings);
				return tmpMongoObject2.strings;
			}
		}

		public int DeleteCollection(string word)
		{
			word = word.ToLower();
			MongoObject tmpMongoSynonim = synonim.Find(_synonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Where(s => s.strings.Contains(word)).FirstOrDefault();

			MongoObject tmpMongoAntonim = synonim.Find(Builders<MongoObject>.Filter.Eq(_antonim => _antonim.mongoId, tmpMongoSynonim.mongoIdForAntonim)).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = null,
				strings = mongoObject.strings
			}).FirstOrDefault();

			int deleted = (int)synonim.DeleteOne(_synonim => _synonim.mongoId.Equals(tmpMongoSynonim.mongoId)).DeletedCount;

			if (deleted > 0)
			{
				int modified = (int)synonim.ReplaceOne(_synonim => _synonim.mongoId.Equals(tmpMongoAntonim.mongoId), tmpMongoAntonim).ModifiedCount;
				Debug.WriteLine("synonims DeleteCollection: " + true);
			}
			return deleted;
		}

		public int DeleteCollection()
		{
			int deleted = (int)synonim.DeleteMany(_synonim => true).DeletedCount;
			return deleted;
		}

		public bool IfWordExists(string word)
		{
			word = word.ToLower();
			bool synonimExists = synonim.Find(_synonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Select(x => x.strings).Any(s => s.Contains(word));
			Debug.WriteLine("synonims IfWordExists: " + synonimExists);
			return synonimExists;
		}
	}
}
