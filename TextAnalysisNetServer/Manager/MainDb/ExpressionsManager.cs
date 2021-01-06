using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TextAnalysis
{
	public class ExpressionsManager: IExpressionsRepository
	{
		private readonly IMongoCollection<MongoSingleObject> expression;
		private const string datacollection = "TextAnalysisDatabaseSettings:TemporalEstablishedExpressionsCollectionName";

		public ExpressionsManager()
		{
			var client = new MongoClient(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:ConnectionString"));
			var database = client.GetDatabase(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:DatabaseName"));
			expression = database.GetCollection<MongoSingleObject>(Startup.staticConfiguration.GetValue<string>("TextAnalysisDatabaseSettings:EstablishedExpressionsCollectionName"));
		}

		public List<string> GetAllWords()
		{
			List<string> expressions = expression.Find(_expression => true).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).ToList().Select(x => x.word).ToList();
			Debug.WriteLine("expression GetAllWords: " + expressions);
			return expressions;
		}

		public string GetWordBy(string word)
		{
			word = word.ToLower();
			string addedExpression = expression.Find(_expression => _expression.word.Equals(word)).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).FirstOrDefault().word;
			Debug.WriteLine("expression GetWordBy: " + addedExpression);
			return addedExpression;
		}

		public string PostWord(string word)
		{
			word = word.ToLower();
			MongoSingleObject mongoSingleObject = new MongoSingleObject(word);
			expression.InsertOne(mongoSingleObject);
			Debug.WriteLine("expression PostWord: " + word);
			new TemporalDynamicSingleWordsManager().DeleteWordByWord(datacollection, word);
			return GetWordBy(word);
		}

		public string PutWord(string word, string connectionWord)
		{
			connectionWord = connectionWord.ToLower();
			word = word.ToLower();
			MongoSingleObject tmpMongoSingleObject = expression.Find(_expression => _expression.word.Equals(connectionWord)).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).FirstOrDefault();

			tmpMongoSingleObject.word = word;
			int modified = (int)expression.ReplaceOne(_expression => _expression.mongoId.Equals(tmpMongoSingleObject.mongoId), tmpMongoSingleObject).ModifiedCount;
			Debug.WriteLine("archaism PutWord: " + connectionWord + " " + word);
			new TemporalDynamicSingleWordsManager().DeleteWordByWord(datacollection, word);
			return GetWordBy(word);
		}

		public int DeleteWord(string wordToRemove)
		{
			wordToRemove = wordToRemove.ToLower();
			MongoSingleObject tmpMongoSingleObject = expression.Find(_expression => _expression.word.Equals(wordToRemove)).Project(mongoSingleObject => new MongoSingleObject
			{
				mongoId = mongoSingleObject.mongoId,
				word = mongoSingleObject.word
			}).FirstOrDefault();

			int deleted = (int)expression.DeleteOne(_expression => _expression.mongoId.Equals(tmpMongoSingleObject.mongoId)).DeletedCount;
			if (deleted > 0)
			{
				Debug.WriteLine("expression DeleteWord: " + true);
			}
			return deleted;
		}

		public int DeleteCollection()
		{
			int deleted = (int)expression.DeleteMany(_expression => true).DeletedCount;
			if (deleted > 0)
			{
				Debug.WriteLine("expression DeleteCollection: " + true);
			}
			return deleted;
		}

		public HashSet<string> GetIntersects(string text)
		{
			text = text.ToLower();
			HashSet<string> expressions = new HashSet<string>();
			List<string> allExpressions = GetAllWords();
			if (allExpressions != null && allExpressions.Count > 0)
			{
				expressions.UnionWith(allExpressions.Where(allWord => allWord.Length > 2 && text.Contains(allWord)).ToHashSet());
			}
			Debug.WriteLine("expression GetIntersects: " + expressions);
			return expressions;
		}

		public bool IfWordExists(string word)
		{
			word = word.ToLower();
			MongoSingleObject expressionExists = expression.Find(_expression => _expression.word.Equals(word)).Project(tmpMongoSingleObject => new MongoSingleObject
			{
				mongoId = tmpMongoSingleObject.mongoId,
				word = tmpMongoSingleObject.word
			}).FirstOrDefault();

			bool collectionExists = false;
			if (expressionExists != null)
				collectionExists = true;

			Debug.WriteLine("expression IfWordExists: " + collectionExists);
			return collectionExists;
		}
	}
}
