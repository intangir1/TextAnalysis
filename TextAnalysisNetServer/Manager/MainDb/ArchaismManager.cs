using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TextAnalysis
{
	public class ArchaismManager:IArchaismRepository
	{
		private readonly IMongoCollection<MongoSingleObject> archaism;
		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalArchaismsCollectionName";

		public ArchaismManager()
		{
			var client = new MongoClient(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:ConnectionString"));
			var database = client.GetDatabase(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:DatabaseName"));
			archaism = database.GetCollection<MongoSingleObject>(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:ArchaismsCollectionName"));
		}

		public List<string> GetAllWords()
		{
			List<string> archaisms = archaism.Find(_archaism => true).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).ToList().Select(x => x.word).ToList();
			Debug.WriteLine("archaism GetAllWords: " + archaisms);
			return archaisms;
		}

		public string GetWordBy(string word)
		{
			word = word.ToLower();
			string addedArchaism = archaism.Find(_archaism => _archaism.word.Equals(word)).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).FirstOrDefault().word;
			Debug.WriteLine("archaism GetWordBy: " + addedArchaism);
			return addedArchaism;
		}

		public string PostWord(string word)
		{
			word = word.ToLower();
			MongoSingleObject mongoSingleObject = new MongoSingleObject(word);
			archaism.InsertOne(mongoSingleObject);
			Debug.WriteLine("archaism PostWord: " + word);
			new TemporalDynamicSingleWordsManager().DeleteWordByWord(datacollection, word);
			return GetWordBy(word);
		}

		public string PutWord(string word, string connectionWord)
		{
			connectionWord = connectionWord.ToLower();
			word = word.ToLower();
			MongoSingleObject tmpMongoSingleObject = archaism.Find(_archaism => _archaism.word.Equals(connectionWord)).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).FirstOrDefault();

			tmpMongoSingleObject.word = word;
			int modified = (int)archaism.ReplaceOne(_archaism => _archaism.mongoId.Equals(tmpMongoSingleObject.mongoId), tmpMongoSingleObject).ModifiedCount;
			Debug.WriteLine("archaism PutWord: " + connectionWord + " " + word);
			new TemporalDynamicSingleWordsManager().DeleteWordByWord(datacollection, word);
			return GetWordBy(word);
		}

		public int DeleteWord(string wordToRemove)
		{
			wordToRemove = wordToRemove.ToLower();
			MongoSingleObject tmpMongoSingleObject = archaism.Find(_archaism => _archaism.word.Equals(wordToRemove)).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).FirstOrDefault();

			int deleted = (int)archaism.DeleteOne(_archaism => _archaism.mongoId.Equals(tmpMongoSingleObject.mongoId)).DeletedCount;
			if (deleted > 0)
			{
				Debug.WriteLine("archaism DeleteWord: " + true);
			}
			return deleted;
		}

		public int DeleteCollection()
		{
			int deleted = (int)archaism.DeleteMany(_archaism => true).DeletedCount;
			if (deleted > 0)
			{
				Debug.WriteLine("archaism DeleteCollection: " + true);
			}
			return deleted;
		}

		public HashSet<string> GetIntersects(string[] words, string[] excepts = null)
		{
			HashSet<string> archaisms = new HashSet<string>();
			List<string> allArchaisms = GetAllWords();
			if (words != null && words.Length > 0)
			{
				words = words.ToList().Select(word => word = word.ToLower()).ToArray();
			}

			if (excepts != null && excepts.Length > 0)
			{
				excepts = excepts.ToList().Select(except => except = except.ToLower()).ToArray();
			}

			if (allArchaisms != null && allArchaisms.Count > 0) {
				if (excepts != null)
				{
					///archaisms.UnionWith(allArchaisms.SelectMany(allWord => words.Where(word => word.Length>2 && allWord.Contains(word) && !allWord.Equals(word))).Except(excepts).ToHashSet());
					//archaisms.UnionWith(words.SelectMany(word => allArchaisms.Where(allWord => allWord.Length > 2 && word.Contains(allWord) && !allWord.Equals(word))).Except(excepts).ToHashSet());
					archaisms.UnionWith(allArchaisms.Select(i => i.ToString()).Intersect(words).Except(excepts).ToHashSet());
				}
				else
				{
					//archaisms.UnionWith(allArchaisms.SelectMany(allWord => words.Where(word => word.Length > 2 && allWord.Contains(word) && WordCondition.WordContainedNotIgnor(word) && !allWord.Equals(word))).ToHashSet());
					//archaisms.UnionWith(words.SelectMany(word => allArchaisms.Where(allWord => allWord.Length > 2 && word.Contains(allWord) && WordCondition.WordContainedNotIgnor(word) && !allWord.Equals(word))).ToHashSet());
					archaisms.UnionWith(allArchaisms.Select(i => i.ToString()).Intersect(words).ToHashSet());
				}
			}
			Debug.WriteLine("archaism GetIntersects: " + archaisms);
			return archaisms;
		}

		public bool IfWordExists(string word)
		{
			word = word.ToLower();
			MongoSingleObject archaismExists = archaism.Find(_archaism => _archaism.word.Equals(word)).Project(tmpMongoSingleObject => new MongoSingleObject
			{
				mongoId = tmpMongoSingleObject.mongoId,
				word = tmpMongoSingleObject.word
			}).FirstOrDefault();

			bool collectionExists = false;
			if (archaismExists != null)
				collectionExists = true;

			Debug.WriteLine("archaism IfWordExists: " + collectionExists);
			return collectionExists;
		}
	}
}