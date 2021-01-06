using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TextAnalysis
{
	public class IrregularVerbsManager:IIrregularVerbsRepository
	{
		private readonly IMongoCollection<IrregularObject> irregular;
		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalIrregularsCollectionName";

		public IrregularVerbsManager()
		{
			var client = new MongoClient(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:ConnectionString"));
			var database = client.GetDatabase(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:DatabaseName"));
			irregular = database.GetCollection<IrregularObject>(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:IrregularsCollectionName"));
		}

		public List<IrregularObject> GetAllData()
		{
			List<IrregularObject> irregulars = irregular.Find(_irregular => true).Project(mongoIrregularObject => new IrregularObject
			{
				mongoId = mongoIrregularObject.mongoId,
				first = mongoIrregularObject.first,
				second = mongoIrregularObject.second,
				third = mongoIrregularObject.third
			}).ToList();
			Debug.WriteLine("irregulars GetAllWords: " + irregulars);
			return irregulars;
		}

		public IrregularObject GetWordBy(string word)
		{
			word = word.ToLower();
			IrregularObject tmpMongoObject = irregular.Find<IrregularObject>(_irregular => _irregular.first.Equals(word) || _irregular.second.Equals(word) || _irregular.third.Equals(word)).Project(mongoIrregularObject => new IrregularObject
			{
				mongoId = mongoIrregularObject.mongoId,
				first = mongoIrregularObject.first,
				second = mongoIrregularObject.second,
				third = mongoIrregularObject.third
			}).FirstOrDefault();
			Debug.WriteLine("irregulars GetWordBy: " + tmpMongoObject);
			return tmpMongoObject;
		}

		public IrregularObject PostData(IrregularObject irregularObject)
		{
			irregularObject = irregularObject.ToLower();
			irregular.InsertOne(irregularObject);
			Debug.WriteLine("irregulars PostWord: " + irregularObject);
			new TemporalIrregularManager().DeleteWordByWord(datacollection, irregularObject.first);
			return GetWordBy(irregularObject.first);
		}

		public IrregularObject PutData(IrregularObject word, string connectionWord)
		{
			connectionWord = connectionWord.ToLower();
			word = word.ToLower();
			IrregularObject tmpMongoObject = irregular.Find<IrregularObject>(_irregular => _irregular.first.Equals(connectionWord) || _irregular.second.Equals(connectionWord) || _irregular.third.Equals(connectionWord)).Project(mongoIrregularObject => new IrregularObject
			{
				mongoId = mongoIrregularObject.mongoId,
				first = mongoIrregularObject.first,
				second = mongoIrregularObject.second,
				third = mongoIrregularObject.third
			}).FirstOrDefault();

			if (tmpMongoObject.first.Equals(connectionWord))
			{
				tmpMongoObject = word;
			} else if (tmpMongoObject.second.Equals(connectionWord))
			{
				tmpMongoObject = word;
			} else if (tmpMongoObject.third.Equals(connectionWord))
			{
				tmpMongoObject = word;
			}

			int modified = (int)irregular.ReplaceOne(_irregular => _irregular.mongoId.Equals(tmpMongoObject.mongoId), tmpMongoObject).ModifiedCount;
			Debug.WriteLine("irregulars PutWord: " + connectionWord + " " + word);
			new TemporalIrregularManager().DeleteWordByWord(datacollection, tmpMongoObject.first);
			return GetWordBy(tmpMongoObject.first);
		}

		public int DeleteData(string wordToRemove)
		{
			wordToRemove = wordToRemove.ToLower();
			IrregularObject tmpMongoObject = irregular.Find<IrregularObject>(_irregular => _irregular.first.Equals(wordToRemove) || _irregular.second.Equals(wordToRemove) || _irregular.third.Equals(wordToRemove)).Project(mongoIrregularObject => new IrregularObject
			{
				mongoId = mongoIrregularObject.mongoId,
				first = mongoIrregularObject.first,
				second = mongoIrregularObject.second,
				third = mongoIrregularObject.third
			}).FirstOrDefault();

			int deleted = (int)irregular.DeleteOne(_irregular => _irregular.mongoId.Equals(tmpMongoObject.mongoId)).DeletedCount;
			if (deleted > 0)
			{
				Debug.WriteLine("irregulars DeleteWord: " + true);
			}
			return deleted;
		}

		public int DeleteCollection()
		{
			int deleted = (int)irregular.DeleteMany(_irregular => true).DeletedCount;
			if (deleted > 0)
			{
				Debug.WriteLine("irregulars DeleteCollection: " + true);
			}
			return deleted;
		}

		public HashSet<string> GetIntersects(string[] words, string[] excepts = null)
		{
			HashSet<string> irregulars = new HashSet<string>();

			List<IrregularObject> allIrregulars = GetAllData();
			if (words != null && words.Length > 0)
			{
				words = words.ToList().Select(word => word = word.ToLower()).ToArray();
			}

			if (excepts != null && excepts.Length > 0)
			{
				excepts = excepts.ToList().Select(except => except = except.ToLower()).ToArray();
			}

			if (allIrregulars != null && allIrregulars.Count > 0)
			{
				if (excepts != null)
				{
					irregulars.UnionWith(allIrregulars.SelectMany(allWord => words.Where(word => word.Length>2 && allWord.EqualsWord(word) && !allWord.Equals(word))).Except(excepts).ToHashSet());
					//irregulars.UnionWith(allIrregulars.SelectMany(allWord => words.Where(word => word.Length>2 && allWord.ContainsWord(word) && !allWord.Equals(word))).Except(excepts).ToHashSet());
					//irregulars.UnionWith(allIrregulars.SelectMany(allWord => words.Where(word => allWord.LengthMore(2) && allWord.ContainedInWord(word) && !allWord.Equals(word))).Except(excepts).ToHashSet());
					irregulars.UnionWith(allIrregulars.SelectMany(allWord => words.Where(word => allWord.Equals(word))).Except(excepts).ToHashSet());
				}
				else
				{
					irregulars.UnionWith(allIrregulars.SelectMany(allWord => words.Where(word => word.Length > 2 && allWord.EqualsWord(word) && !allWord.Equals(word))).ToHashSet());
					//irregulars.UnionWith(allIrregulars.SelectMany(allWord => words.Where(word => word.Length > 2 && allWord.ContainsWord(word) && !allWord.Equals(word))).ToHashSet());
					//irregulars.UnionWith(allIrregulars.SelectMany(allWord => words.Where(word => allWord.LengthMore(2) && allWord.ContainedInWord(word) && !allWord.Equals(word))).ToHashSet());
					irregulars.UnionWith(allIrregulars.SelectMany(allWord => words.Where(word => allWord.Equals(word))).ToHashSet());
				}
			}
			Debug.WriteLine("irregulars GetIntersects: " + irregulars);
			return irregulars;
		}

		public bool IfWordExists(IrregularObject word)
		{
			word = word.ToLower();
			IrregularObject mongoIrregularObject = irregular.Find(_irregular => _irregular.first.Equals(word.first) && _irregular.second.Equals(word.second) && _irregular.third.Equals(word.third)).Project(mongoIrregularObject => new IrregularObject
			{
				mongoId = mongoIrregularObject.mongoId,
				first = mongoIrregularObject.first,
				second = mongoIrregularObject.second,
				third = mongoIrregularObject.third
			}).FirstOrDefault();

			bool collectionExists = false;
			if (mongoIrregularObject != null)
				collectionExists = true;

			Debug.WriteLine("irregular IfWordExists: " + collectionExists);
			return collectionExists;
		}
	}
}