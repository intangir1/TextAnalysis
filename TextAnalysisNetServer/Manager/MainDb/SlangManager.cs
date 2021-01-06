using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TextAnalysis
{
	public class SlangManager:ISlangRepository
	{
		private readonly IMongoCollection<MongoSingleObject> slang;
		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalSlangsCollectionName";

		public SlangManager()
		{
			var client = new MongoClient(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:ConnectionString"));
			var database = client.GetDatabase(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:DatabaseName"));
			slang = database.GetCollection<MongoSingleObject>(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:SlangsCollectionName"));
		}

		public List<string> GetAllWords()
		{
			List<string> slangs = slang.Find(_slang => true).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).ToList().Select(x => x.word).ToList();
			Debug.WriteLine("slangs GetAllWords: " + slangs);
			return slangs;
		}

		public string GetWordBy(string word)
		{
			word = word.ToLower();
			string addedSlang = slang.Find(_slang => _slang.word.Equals(word)).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).FirstOrDefault().word;
			Debug.WriteLine("slang GetWordBy: " + addedSlang);
			return addedSlang;
		}

		public string PostWord(string word)
		{
			word = word.ToLower();
			MongoSingleObject mongoSingleObject = new MongoSingleObject(word);
			slang.InsertOne(mongoSingleObject);
			Debug.WriteLine("slang PostWord: " + word);
			new TemporalDynamicSingleWordsManager().DeleteWordByWord(datacollection, word);
			return GetWordBy(word);
		}

		public string PutWord(string word, string connectionWord)
		{
			connectionWord = connectionWord.ToLower();
			word = word.ToLower();
			MongoSingleObject tmpMongoSingleObject = slang.Find(_slang => _slang.word.Equals(connectionWord)).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).FirstOrDefault();

			tmpMongoSingleObject.word = word;
			int modified = (int)slang.ReplaceOne(_slang => _slang.mongoId.Equals(tmpMongoSingleObject.mongoId), tmpMongoSingleObject).ModifiedCount;
			Debug.WriteLine("archaism PutWord: " + connectionWord + " " + word);
			new TemporalDynamicSingleWordsManager().DeleteWordByWord(datacollection, word);
			return GetWordBy(word);
		}

		public int DeleteWord(string wordToRemove)
		{
			wordToRemove = wordToRemove.ToLower();
			MongoSingleObject tmpMongoSingleObject = slang.Find(_slang => _slang.word.Equals(wordToRemove)).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).FirstOrDefault();

			int deleted = (int)slang.DeleteOne(_slang => _slang.mongoId.Equals(tmpMongoSingleObject.mongoId)).DeletedCount;
			if (deleted > 0)
			{
				Debug.WriteLine("slang DeleteWord: " + true);
			}
			return deleted;
		}

		public int DeleteCollection()
		{
			int deleted = (int)slang.DeleteMany(_slang => true).DeletedCount;
			if (deleted > 0)
			{
				Debug.WriteLine("slang DeleteCollection: " + true);
			}
			return deleted;
		}

		public HashSet<string> GetIntersects(string[] words, string[] excepts = null)
		{
			HashSet<string> slangs = new HashSet<string>();
			List<string> allSlangs = GetAllWords();
			if (words != null && words.Length > 0)
			{
				words = words.ToList().Select(word => word = word.ToLower()).ToArray();
			}

			if (excepts != null && excepts.Length > 0)
			{
				excepts = excepts.ToList().Select(except => except = except.ToLower()).ToArray();
			}

			if (allSlangs!=null && allSlangs.Count > 0)
			{
				if (excepts != null)
				{
					slangs.UnionWith(allSlangs.SelectMany(allWord => words.Where(word => word.Length > 2 && allWord.Contains(word) && !allWord.Equals(word))).Except(excepts).ToHashSet());
					slangs.UnionWith(words.SelectMany(word => allSlangs.Where(allWord => allWord.Length > 2 && word.Contains(allWord) && !allWord.Equals(word))).Except(excepts).ToHashSet());
					slangs.UnionWith(allSlangs.Select(i => i.ToString()).Intersect(words).Except(excepts).ToHashSet());
				}
				else
				{
					slangs.UnionWith(allSlangs.SelectMany(allWord => words.Where(word => word.Length > 2 && allWord.Contains(word) && !allWord.Equals(word))).ToHashSet());
					slangs.UnionWith(words.SelectMany(word => allSlangs.Where(allWord => allWord.Length > 2 && word.Contains(allWord) && !allWord.Equals(word))).ToHashSet());
					slangs.UnionWith(allSlangs.Select(i => i.ToString()).Intersect(words).ToHashSet());
				}
			}
			Debug.WriteLine("slangs GetIntersects: " + slangs);
			return slangs;
		}

		public bool IfWordExists(string word)
		{
			word = word.ToLower();
			MongoSingleObject slangExists = slang.Find(_slang => _slang.word.Equals(word)).Project(tmpMongoSingleObject => new MongoSingleObject
			{
				mongoId = tmpMongoSingleObject.mongoId,
				word = tmpMongoSingleObject.word
			}).FirstOrDefault();

			bool collectionExists = false;
			if (slangExists != null)
				collectionExists = true;

			Debug.WriteLine("slang IfWordExists: " + collectionExists);
			return collectionExists;
		}
	}
}