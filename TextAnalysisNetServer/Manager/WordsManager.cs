using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextAnalysis
{
	public class WordsManager : IWordsRepository
	{
		ISingleWordsRepository singleWordsRepository = new ArchaismManager();
		IIrregularVerbsRepository irregularVerbsRepository = new IrregularVerbsManager();
		IRelationalWordsRepository relationalWordsRepository = new AntonimManager();

		ITemporalSingleWordsRepository temporalSingleWordsRepository = new TemporalDynamicSingleWordsManager();
		ITemporalRelationalWordsRepository temporalRelationalWordsRepository = new TemporalDynamicRelationalWordsManager();
		ITemporalIrregularRepository temporalIrregularRepository = new TemporalIrregularManager();

		public AnalysedText AddSimpleWords(string path)
		{
			path = Path.GetFullPath(Path.Combine(path, @"Words\SimpleWords"));
			AnalysedText analysedText = new AnalysedText();
			foreach (string file in Directory.EnumerateFiles(path, "*.txt"))
			{
				foreach (string line in File.ReadLines(file))
				{
					if (line.Equals("archaysms"))
					{
						singleWordsRepository = new ArchaismManager();
						Console.WriteLine("ArchaismManager");
						continue;
					}
					else if (line.Equals("expressions"))
					{
						singleWordsRepository = new ExpressionsManager();
						Console.WriteLine("ExpressionsManager");
						continue;
					}
					else if (line.Equals("slangs"))
					{
						singleWordsRepository = new SlangManager();
						Console.WriteLine("SlangManager");
						continue;
					}
					if (!singleWordsRepository.IfWordExists(line.Trim()))
					{
						singleWordsRepository.PostWord(line.Trim());
					}
				}
			}
			analysedText.archaisms = new HashSet<string>(new ArchaismManager().GetAllWords());
			analysedText.expressions = new HashSet<string>(new ExpressionsManager().GetAllWords());
			analysedText.slangs = new HashSet<string>(new SlangManager().GetAllWords());
			Console.WriteLine(analysedText);
			return analysedText;
		}

		public List<IrregularObject> AddIrregulars(string path)
		{
			path = Path.GetFullPath(Path.Combine(path, @"Words\IrregularWords\irregulars.txt"));
			string[] lines = File.ReadAllLines(path);
			foreach (string line in lines)
			{
				if (line == null || line.Equals(string.Empty) || line.Trim().Equals(string.Empty))
				{
					continue;
				}
				string lineNoSpaces = System.Text.RegularExpressions.Regex.Replace(line, @"\s+", " ");
				string[] words = lineNoSpaces.Split(' ');
				IrregularObject irregularObject = new IrregularObject(words[0].Trim(), words[1].Trim(), words[2].Trim());
				if (!irregularVerbsRepository.IfWordExists(irregularObject))
				{
					irregularVerbsRepository.PostData(irregularObject);
				}
			}
			List<IrregularObject> irregulars = new IrregularVerbsManager().GetAllData();
			return irregulars;
		}

		public List<List<string>> AddRelationalWords(string path)
		{
			path = Path.GetFullPath(Path.Combine(path, @"Words\RelationalWords"));
			string type = "";
			foreach (string file in Directory.EnumerateFiles(path, "*.txt"))
			{
				foreach (string line in File.ReadLines(file))
				{
					if (line.Equals("synonims"))
					{
						type = line;
						relationalWordsRepository = new SynonimManager();
						Console.WriteLine("SynonimManager");
						continue;
					}
					else if (line.Equals("antonyms"))
					{
						type = line;
						relationalWordsRepository = new AntonimManager();
						Console.WriteLine("AntonymManager");
						continue;
					}

					string lineNoSpaces = System.Text.RegularExpressions.Regex.Replace(line, @"\s+", " ");
					string[] words = lineNoSpaces.ToLower().Split(' ');
					if (type.Equals("synonims"))
					{
						List<string> list = new List<string>(words);
						if (!relationalWordsRepository.IfWordExists(words[0]))
						{
							relationalWordsRepository.PostCollection(list);
						}
					} else if (type.Equals("antonyms"))
					{
						string antonim = words[words.Length - 1];
						Array.Resize(ref words, words.Length - 1);
						List<string> list = new List<string>(words);
						if (!relationalWordsRepository.IfWordExists(words[0]))
						{
							relationalWordsRepository.PostCollection(list, antonim);
						}
					}
				}
			}
			List<List<string>> synonims = new SynonimManager().GetAllWords();
			Console.WriteLine(synonims);
			return synonims;
		}

		public int DeleteAllWords()
		{
			int deleted = new ArchaismManager().DeleteCollection() +
			new ExpressionsManager().DeleteCollection() +
			new SlangManager().DeleteCollection() +
			new IrregularVerbsManager().DeleteCollection() +
			new SynonimManager().DeleteCollection();
			return deleted;
		}

		public List<TemporalObject> AddTemporalSimpleWords(string path)
		{
			path = Path.GetFullPath(Path.Combine(path, @"Words\SimpleWords"));
			string datacollection = "TextAnalysisDatabaseSettings:TemporalArchaismsCollectionName";
			string type = "Archaism";
			foreach (string file in Directory.EnumerateFiles(path, "*.txt"))
			{
				foreach (string line in File.ReadLines(file))
				{
					if (line.ToLower().Equals("archaysms"))
					{
						datacollection = "TextAnalysisDatabaseSettings:TemporalArchaismsCollectionName";
						type = "Archaism";
						Console.WriteLine("ArchaismManager");
						continue;
					}
					else if (line.ToLower().Equals("expressions"))
					{
						datacollection = "TextAnalysisDatabaseSettings:TemporalEstablishedExpressionsCollectionName";
						type = "Expression";
						Console.WriteLine("ExpressionsManager");
						continue;
					}
					else if (line.ToLower().Equals("slangs"))
					{
						datacollection = "TextAnalysisDatabaseSettings:TemporalSlangsCollectionName";
						type = "Slang";
						Console.WriteLine("SlangManager");
						continue;
					}
					if (!singleWordsRepository.IfWordExists(line.Trim()))
					{
						temporalSingleWordsRepository.PostWord(datacollection, type, line.Trim());
					}
				}
			}
			List<TemporalObject> temporalObject = new List<TemporalObject>();
			temporalObject.AddRange(temporalSingleWordsRepository.GetAllWords("TextAnalysisDatabaseSettings:TemporalArchaismsCollectionName"));
			temporalObject.AddRange(temporalSingleWordsRepository.GetAllWords("TextAnalysisDatabaseSettings:TemporalEstablishedExpressionsCollectionName"));
			temporalObject.AddRange(temporalSingleWordsRepository.GetAllWords("TextAnalysisDatabaseSettings:TemporalSlangsCollectionName"));
			Console.WriteLine(temporalObject);
			return temporalObject;
		}

		public List<TemporalObjectForIrregular> AddTemporalIrregulars(string path)
		{
			path = Path.GetFullPath(Path.Combine(path, @"Words\IrregularWords\irregulars.txt"));
			string datacollection = "TextAnalysisDatabaseSettings:TemporalIrregularsCollectionName";
			string[] lines = File.ReadAllLines(path);
			foreach (string line in lines)
			{
				if (line == null || line.Equals(string.Empty) || line.Trim().Equals(string.Empty))
				{
					continue;
				}
				string lineNoSpaces = System.Text.RegularExpressions.Regex.Replace(line, @"\s+", " ");
				string[] words = lineNoSpaces.Split(' ');
				IrregularObject irregularObject = new IrregularObject(words[0].Trim(), words[1].Trim(), words[2].Trim());
				if (!temporalIrregularRepository.IfWordExists(datacollection, irregularObject))
				{
					temporalIrregularRepository.PostWord(datacollection, irregularObject);
				}
			}
			List<TemporalObjectForIrregular> irregulars = temporalIrregularRepository.GetAllWords(datacollection);
			return irregulars;
		}

		public List<TemporalObject> AddTemporalRelationalWords(string path)
		{
			path = Path.GetFullPath(Path.Combine(path, @"Words\RelationalWords"));
			string datacollection = "";
			string type = "";
			foreach (string file in Directory.EnumerateFiles(path, "*.txt"))
			{
				foreach (string line in File.ReadLines(file))
				{
					if (line.Equals("synonims"))
					{
						datacollection = "TextAnalysisDatabaseSettings:TemporalSynonimsCollectionName";
						type = "Synonim";
						relationalWordsRepository = new SynonimManager();
						Console.WriteLine("SynonimManager");
						continue;
					}
					else if (line.Equals("antonyms"))
					{
						datacollection = "TextAnalysisDatabaseSettings:TemporalAntonimsCollectionName";
						type = "Antonim";
						relationalWordsRepository = new AntonimManager();
						Console.WriteLine("AntonymManager");
						continue;
					}

					string lineNoSpaces = System.Text.RegularExpressions.Regex.Replace(line, @"\s+", " ");
					string[] words = lineNoSpaces.ToLower().Split(' ');
					if (type.Equals("Synonim"))
					{
						for (int i = 0; i < words.Length; i++)
						{
							if (!temporalRelationalWordsRepository.IfWordExists(datacollection, words[0].Trim()))
							{
								temporalRelationalWordsRepository.PostWord(datacollection, type, words[i], "");
							}
						}
					}
					else if (type.Equals("Antonim"))
					{
						string antonim = words[words.Length - 1];
						Array.Resize(ref words, words.Length - 1);
						for (int i=0;i< words.Length; i++)
						{
							if (!temporalRelationalWordsRepository.IfWordExists(datacollection, words[0].Trim()))
							{
								temporalRelationalWordsRepository.PostWord(datacollection, type, words[i].Trim(), antonim.Trim().ToLower());
							}
						}
					}
				}
			}
			List<TemporalObject> synonims = new TemporalDynamicRelationalWordsManager().GetAllWords("TextAnalysisDatabaseSettings:TemporalSynonimsCollectionName");
			Console.WriteLine(synonims);
			return synonims;
		}

		public int DeleteAllTemporalWords()
		{
			int deleted = temporalSingleWordsRepository.DeleteCollection("TextAnalysisDatabaseSettings:TemporalArchaismsCollectionName") +
			temporalSingleWordsRepository.DeleteCollection("TextAnalysisDatabaseSettings:TemporalEstablishedExpressionsCollectionName") +
			temporalSingleWordsRepository.DeleteCollection("TextAnalysisDatabaseSettings:TemporalSlangsCollectionName") +
			temporalIrregularRepository.DeleteCollection("TextAnalysisDatabaseSettings:TemporalIrregularsCollectionName") +
			temporalRelationalWordsRepository.DeleteCollection("TextAnalysisDatabaseSettings:TemporalSynonimsCollectionName");
			return deleted;
		}

		public List<object> GetAllTemporalWords()
		{
			List<object> temporalObject = new List<object>();
			temporalObject.AddRange(temporalRelationalWordsRepository.GetAllWords("TextAnalysisDatabaseSettings:TemporalSynonimsCollectionName"));
			temporalObject.AddRange(temporalSingleWordsRepository.GetAllWords("TextAnalysisDatabaseSettings:TemporalArchaismsCollectionName"));
			temporalObject.AddRange(temporalSingleWordsRepository.GetAllWords("TextAnalysisDatabaseSettings:TemporalEstablishedExpressionsCollectionName"));
			temporalObject.AddRange(temporalSingleWordsRepository.GetAllWords("TextAnalysisDatabaseSettings:TemporalSlangsCollectionName"));
			temporalObject.AddRange(temporalIrregularRepository.GetAllWords("TextAnalysisDatabaseSettings:TemporalIrregularsCollectionName"));
			Console.WriteLine(temporalObject);
			return temporalObject;
		}

		public List<object> GetAllWords()
		{
			List<object> myObject = new List<object>();
			myObject.AddRange(new ArchaismManager().GetAllWords());
			myObject.AddRange(new ExpressionsManager().GetAllWords());
			myObject.AddRange(new SlangManager().GetAllWords());
			myObject.AddRange(new IrregularVerbsManager().GetAllData());
			myObject.AddRange(new SynonimManager().GetAllWords());
			Console.WriteLine(myObject);
			return myObject;
		}
	}
}
