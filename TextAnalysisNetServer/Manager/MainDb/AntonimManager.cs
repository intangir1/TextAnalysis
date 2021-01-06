using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TextAnalysis
{
	public class AntonimManager: IAntonimRepository
	{
		private readonly IMongoCollection<MongoObject> antonim;
		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalAntonimsCollectionName";

		public AntonimManager()
		{
			var client = new MongoClient(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:ConnectionString"));
			var database = client.GetDatabase(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:DatabaseName"));
			antonim = database.GetCollection<MongoObject>(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:AntonimsCollectionName"));
		}

		public List<List<string>> GetAllWords()
		{
			List<List<string>> antonims = antonim.Find(_antonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Select(x => x.strings).ToList();
			Debug.WriteLine("antonim GetAllWords: " + antonims);
			return antonims;
		}

		public List<string> GetWordsBy(string word)
		{
			word = word.ToLower();
			MongoObject mongoObjectSynonim = antonim.Find(_antonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Select(x => x).Where(s => s.strings.Contains(word)).FirstOrDefault();

			List<string> antonims = new List<string>();
			if (mongoObjectSynonim != null) {
				antonims = antonim.Find(Builders<MongoObject>.Filter.Eq(_antonim => _antonim.mongoId, mongoObjectSynonim.mongoIdForAntonim)).Project(mongo_object => new MongoObject
				{
					mongoId = mongo_object.mongoId,
					mongoIdForAntonim = mongo_object.mongoIdForAntonim,
					strings = mongo_object.strings
				}).ToList().Select(x => x.strings).FirstOrDefault();
			}
			Debug.WriteLine("antonim GetWordsBy: " + antonims);
			return antonims;
		}

		public List<string> PostWord(string word)
		{
			throw new System.NotImplementedException();
		}

		public List<string> PostWord(string word, string connectionWord)
		{
			word = word.ToLower();
			connectionWord = connectionWord.ToLower();
			MongoObject mongoObject = new MongoObject();


			MongoObject mongoObjectSynonim = antonim.Find(_synonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Select(x => x).Where(s => s.strings.Contains(connectionWord)).FirstOrDefault();

			mongoObject.mongoIdForAntonim = mongoObjectSynonim.mongoId;

			antonim.InsertOne(mongoObject);

			MongoObject tmpMongoAntonim = antonim.Find(_antonims => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Where(s => s.strings.Contains(word)).FirstOrDefault();

			mongoObjectSynonim.mongoIdForAntonim = tmpMongoAntonim.mongoId;

			int modified = (int)antonim.ReplaceOne(_synonim => _synonim.mongoId.Equals(mongoObjectSynonim.mongoId), mongoObjectSynonim).ModifiedCount;

			Debug.WriteLine("antonim GetWordsBy: " + tmpMongoAntonim.strings);
			new TemporalDynamicRelationalWordsManager().DeleteWordByWord(datacollection, word);
			return tmpMongoAntonim.strings;
		}

		public List<string> PostCollection(List<string> words)
		{
			throw new System.NotImplementedException();
		}

		public List<string> PostCollection(List<string> antonimWords, string connectionWord)
		{
			connectionWord = connectionWord.ToLower();
			antonimWords.ForEach(antonim => antonim.ToLower());

			MongoObject mongoObject = new MongoObject(antonimWords);
			MongoObject mongoObjectSynonim = antonim.Find(_synonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Select(x => x).Where(s => s.strings.Contains(connectionWord)).FirstOrDefault();

			mongoObject.mongoIdForAntonim = mongoObjectSynonim.mongoId;

			antonim.InsertOne(mongoObject);

			MongoObject tmpMongoAntonim = antonim.Find(_antonims => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Where(s => s.strings.Contains(antonimWords[0])).FirstOrDefault();

			mongoObjectSynonim.mongoIdForAntonim = tmpMongoAntonim.mongoId;

			int modified = (int)antonim.ReplaceOne(_synonim => _synonim.mongoId.Equals(mongoObjectSynonim.mongoId), mongoObjectSynonim).ModifiedCount;

			Debug.WriteLine("antonim GetWordsBy: " + tmpMongoAntonim.strings);
			return tmpMongoAntonim.strings;
		}

		public List<string> PutWord(string word, string connectionWord)
		{
			word = word.ToLower();
			connectionWord = connectionWord.ToLower();

			MongoObject tmpMongoObject = antonim.Find(_synonims => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Where(s => s.strings.Contains(connectionWord)).FirstOrDefault();

			tmpMongoObject.strings = tmpMongoObject.strings.Select(s => s.Replace(connectionWord, word)).ToList();
			int modified = (int)antonim.ReplaceOne(_synonim => _synonim.mongoId.Equals(tmpMongoObject.mongoId), tmpMongoObject).ModifiedCount;
			Debug.WriteLine("antonim PutWord: " + connectionWord + " " + word);
			new TemporalDynamicRelationalWordsManager().DeleteWordByWord(datacollection, word);
			return GetWordsBy(word);
		}

		public List<string> InsertWord(string word, string connectionWord)
		{
			word = word.ToLower();
			connectionWord = connectionWord.ToLower();

			MongoObject tmpMongoSynonim = antonim.Find(_synonims => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Where(s => s.strings.Contains(connectionWord)).FirstOrDefault();

			MongoObject tmpMongoAntonim = antonim.Find(Builders<MongoObject>.Filter.Eq(_antonim => _antonim.mongoId, tmpMongoSynonim.mongoIdForAntonim)).Project(mongo_object => new MongoObject
			{
				mongoId = mongo_object.mongoId,
				mongoIdForAntonim = mongo_object.mongoIdForAntonim,
				strings = mongo_object.strings
			}).FirstOrDefault();

			tmpMongoAntonim.strings.Add(word);
			int modified = (int)antonim.ReplaceOne(_synonim => _synonim.mongoId.Equals(tmpMongoAntonim.mongoId), tmpMongoAntonim).ModifiedCount;

			Debug.WriteLine("antonim InsertWord: " + connectionWord + " " + word);
			new TemporalDynamicRelationalWordsManager().DeleteWordByWord(datacollection, word);
			return GetWordsBy(word);
		}

		public List<string> DeleteWord(string wordToRemove)
		{
			wordToRemove = wordToRemove.ToLower();
			MongoObject tmpMongoObject = antonim.Find(_synonims => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Where(s => s.strings.Contains(wordToRemove)).FirstOrDefault();

			tmpMongoObject.strings.Remove(wordToRemove);
			if (tmpMongoObject.strings.Count == 0)
			{
				int deleted = new SynonimManager().DeleteCollection(wordToRemove);
				if (deleted > 0)
				{
					Debug.WriteLine("synonims DeleteWord: " + "Whole object was deleted");
				}
				return GetWordsBy(wordToRemove);
			}
			else
			{
				int modified = (int)antonim.ReplaceOne(_synonim => _synonim.mongoId.Equals(tmpMongoObject.mongoId), tmpMongoObject).ModifiedCount;

				MongoObject tmpMongoObject2 = antonim.Find(Builders<MongoObject>.Filter.Eq(_synonim => _synonim.mongoId, tmpMongoObject.mongoId)).Project(mongoObject => new MongoObject
				{
					mongoId = mongoObject.mongoId,
					mongoIdForAntonim = mongoObject.mongoIdForAntonim,
					strings = mongoObject.strings
				}).FirstOrDefault();

				Debug.WriteLine("antonim DeleteWord: " + tmpMongoObject2.strings);
				return tmpMongoObject2.strings;
			}
		}

		public int DeleteCollection(string word)
		{
			word = word.ToLower();
			MongoObject tmpMongoSynonim = antonim.Find(_synonims => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Where(s => s.strings.Contains(word)).FirstOrDefault();

			MongoObject tmpMongoAntonim = antonim.Find(Builders<MongoObject>.Filter.Eq(_antonim => _antonim.mongoId, tmpMongoSynonim.mongoIdForAntonim)).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).FirstOrDefault();

			tmpMongoSynonim.mongoIdForAntonim = null;
			tmpMongoAntonim.mongoIdForAntonim = null;

			int modifiedSy = (int)antonim.ReplaceOne(_synonim => _synonim.mongoId.Equals(tmpMongoSynonim.mongoId), tmpMongoSynonim).ModifiedCount;

			int modifiedAn = (int)antonim.ReplaceOne(_antonim => _antonim.mongoId.Equals(tmpMongoAntonim.mongoId), tmpMongoAntonim).ModifiedCount;

			if (modifiedSy + modifiedAn > 0)
			{
				Debug.WriteLine("antonim DeleteCollection: " + true);
			}

			return modifiedSy + modifiedAn;
		}

		public int DeleteCollection()
		{
			int deleted = (int)antonim.DeleteMany(_antonim => true).DeletedCount;
			return deleted;
		}

		public bool IfWordExists(string word)
		{
			word = word.ToLower();
			bool antonimExists = false;
			MongoObject mongoObjectSynonim = antonim.Find(_antonim => true).Project(mongoObject => new MongoObject
			{
				mongoId = mongoObject.mongoId,
				mongoIdForAntonim = mongoObject.mongoIdForAntonim,
				strings = mongoObject.strings
			}).ToList().Select(x => x).Where(s => s.strings.Contains(word)).FirstOrDefault();

			MongoObject antonims=null;
			if (mongoObjectSynonim != null)
				antonims = antonim.Find(Builders<MongoObject>.Filter.Eq(_antonim => _antonim.mongoId, mongoObjectSynonim.mongoIdForAntonim)).FirstOrDefault();
			
			if (antonims != null)
				antonimExists = true;

			Debug.WriteLine("antonim IfWordExists: " + antonimExists);
			return antonimExists;
		}
	}
}
