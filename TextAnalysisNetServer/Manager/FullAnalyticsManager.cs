using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TextAnalysis
{
	public class FullAnalyticsManager: IFullAnalyticsRepository
	{
		private IArchaismRepository archaismRepository;
		private ISlangRepository slangRepository;
		private IIrregularVerbsRepository irregularVerbsRepository;
		private IExpressionsRepository expressionsRepository;

		private HashSet<string> repeatedContains;
		private HashSet<string> archaismsContains;
		private HashSet<string> slangsContains;
		private HashSet<string> irregularsContains;
		private HashSet<string> expressionsContains;

		private int idCounter = 0;

		public FullAnalyticsManager()
		{
			archaismRepository = new ArchaismManager();
			slangRepository = new SlangManager();
			irregularVerbsRepository = new IrregularVerbsManager();
			expressionsRepository = new ExpressionsManager();

			repeatedContains = new HashSet<string>();
			archaismsContains = new HashSet<string>();
			slangsContains = new HashSet<string>();
			irregularsContains = new HashSet<string>();
			expressionsContains = new HashSet<string>();
		}

		private HashSet<string> StringFullIntersectPercent(string[] words, int limit)
		{
			if (words != null && words.Length > 0)
			{
				words = words.ToList().Select(word => word = word.ToLower()).ToArray();
			}
			Dictionary<string, int> countsDictionary = new Dictionary<string, int>();
			List<IrregularObject> irregulars = irregularVerbsRepository.GetAllData();
			irregularsContains = (from irregular in irregulars
								  from word in words
								  where (WordCondition.WordAcccepted(word) && irregular.ContainedInWord(word))
								  select new { irr = irregular, val = word })
								  .GroupBy(x => x.irr)
								  .Where(x => x.Count() * 100 / words.Count() >= limit)
								  .SelectMany(x => x)
								  .ToList().Select(y=>y.val).ToHashSet();

			for (int placeInWordsArray = 0; placeInWordsArray < words.Count(); placeInWordsArray++)
			{
				if (WordCondition.WordAcccepted(words[placeInWordsArray]) && !irregularsContains.Contains(words[placeInWordsArray]))
				{
					var dictKey = countsDictionary.Where(element => words[placeInWordsArray].ToLower().Contains(element.Key)).FirstOrDefault().Key;
					if (!countsDictionary.ContainsKey(words[placeInWordsArray].ToLower()) && (dictKey == null || dictKey.Equals("") || dictKey.Equals(string.Empty)))
					{
						countsDictionary.Add(words[placeInWordsArray].ToLower(), 1);
					}
					else if (countsDictionary.ContainsKey(words[placeInWordsArray].ToLower()))
					{
						countsDictionary[words[placeInWordsArray].ToLower()] = countsDictionary[words[placeInWordsArray].ToLower()] +1;
					}
					else
					{
						countsDictionary[dictKey.ToLower()] = countsDictionary[dictKey.ToLower()] + 1;
					}
				}
			}
			HashSet<string> dictionary = countsDictionary.Where(x => x.Value * 100 / words.Count() >= limit).Select(x => x.Key).ToHashSet();
			//irregularsContains.IntersectWith(dictionary);
			return dictionary;
		}

		private HashSet<string> StringFullIntersectBoolean(string[] words, int limit=2)
		{
			if (words != null && words.Length > 0)
			{
				words = words.ToList().Select(word => word = word.ToLower()).ToArray();
			}
			Dictionary<string, bool> booleanDictionary = new Dictionary<string, bool>();
			List<IrregularObject> irregulars = irregularVerbsRepository.GetAllData();
			irregularsContains = (from irregular in irregulars
								  from word in words
								  where (WordCondition.WordAcccepted(word) && irregular.ContainedInWord(word))
								  select new { irr = irregular, val = word })
								  .GroupBy(x => x.irr)
								  .Where(x => x.Count()>=limit)
								  .SelectMany(x => x)
								  .ToList().Select(y => y.val).ToHashSet();

			for (int placeInWordsArray = 0; placeInWordsArray < words.Count(); placeInWordsArray++)
			{
				if (WordCondition.WordAcccepted(words[placeInWordsArray]) && !irregularsContains.Contains(words[placeInWordsArray]))
				{
					var dictKey = booleanDictionary.Where(element => words[placeInWordsArray].ToLower().Contains(element.Key)).FirstOrDefault().Key;
					if (!booleanDictionary.ContainsKey(words[placeInWordsArray].ToLower()) && (dictKey == null || dictKey.Equals("") || dictKey.Equals(string.Empty)))
					{
						booleanDictionary.Add(words[placeInWordsArray], false);
					}
					else if (booleanDictionary.ContainsKey(words[placeInWordsArray].ToLower()))
					{
						booleanDictionary[words[placeInWordsArray].ToLower()] = true;
					}
					else
					{
						booleanDictionary[dictKey.ToLower()] = true;
					}
				}
			}
			HashSet<string> dictionary = booleanDictionary.Where(x => x.Value == true).Select(x => x.Key).ToHashSet();
			//irregularsContains.IntersectWith(dictionary);
			return dictionary;
		}

		private AnalysedText AnalyseTwoSentences(TwoSenteces twoSenteces)
		{
			string[] firstSentence = Split.SplitTextToWords(twoSenteces.firstSentence);
			string[] firstSentenceClear = CleanSentence.GetCleanWordsArray(firstSentence);
			string[] clearTwoArraysSplitted = firstSentenceClear;
			string[] secondSentence = { };
			string[] secondSentenceClear = { };

			if (twoSenteces.secondSentence!=null && !twoSenteces.secondSentence.Equals("") && !twoSenteces.secondSentence.Equals(String.Empty))
			{
				secondSentence = Split.SplitTextToWords(twoSenteces.secondSentence);
				secondSentenceClear = CleanSentence.GetCleanWordsArray(secondSentence);
				clearTwoArraysSplitted = firstSentenceClear.Concat(secondSentenceClear).ToArray();
			}

			repeatedContains.UnionWith(StringFullIntersectBoolean(clearTwoArraysSplitted));
			archaismsContains.UnionWith(archaismRepository.GetIntersects(firstSentence));
			slangsContains.UnionWith(slangRepository.GetIntersects(firstSentence));
			//irregularsContains.UnionWith(irregularVerbsRepository.GetIntersects(firstSentenceClear));
			expressionsContains.UnionWith(expressionsRepository.GetIntersects(twoSenteces.firstSentence));

			if (twoSenteces.secondSentence != null && !twoSenteces.secondSentence.Equals("") && !twoSenteces.secondSentence.Equals(String.Empty))
			{
				archaismsContains.UnionWith(archaismRepository.GetIntersects(secondSentence, firstSentence));
				slangsContains.UnionWith(slangRepository.GetIntersects(secondSentence, firstSentence));
				//irregularsContains.UnionWith(irregularVerbsRepository.GetIntersects(secondSentenceClear, firstSentenceClear));
				expressionsContains.UnionWith(expressionsRepository.GetIntersects(twoSenteces.secondSentence));
			}

			AnalysedText analysedText = new AnalysedText(repeatedContains, archaismsContains, slangsContains, irregularsContains, expressionsContains);
			Debug.WriteLine("twoSenteces AnalyseTwoSentences: " + analysedText);
			return analysedText;
		}

		private string ReplaceInsideString(string type, string stringToReplace, string replaceBy, AnalysedText analysedText, string myIdCounter)
		{
			string result = "";
			if (!stringToReplace.Contains(replaceBy + "</span>"))
			{
				if (type.Equals("expressions") && analysedText.expressions.Contains(replaceBy))
				{
					result = CustomRegexReplace.RegexReplaceForAngular(stringToReplace, replaceBy, "e1x2p3r4e5s6s7i8o9n0s", myIdCounter);
				}
				else if (type.Equals("archaisms") && analysedText.archaisms.Contains(replaceBy))
				{
					result = CustomRegexReplace.RegexReplaceForAngular(stringToReplace, replaceBy, "a1r2c3h4a5i6s7m8s", myIdCounter);
				}
				else if (type.Equals("slangs") && analysedText.slangs.Contains(replaceBy))
				{
					result = CustomRegexReplace.RegexReplaceForAngular(stringToReplace, replaceBy, "s1l2a3n4g5s", myIdCounter);
				}
				else if (type.Equals("irregulars") && analysedText.irregulars.Contains(replaceBy))
				{
					IrregularObject irregularObject = new IrregularVerbsManager().GetWordBy(replaceBy);
					if (irregularObject != null)
					{
						result = CustomRegexReplace.RegexReplaceForAngular(stringToReplace, replaceBy, irregularObject.first, myIdCounter);
					}
					else
					{
						result = stringToReplace;
					}
				}
				else if(type.Equals("repeated") && analysedText.repeated.Contains(replaceBy))
				{
					result = CustomRegexReplace.RegexReplaceForAngular(stringToReplace, replaceBy, "r1e2p3e4a5t6e7d", myIdCounter);
				}
			}
			else
				result = stringToReplace;
			return result;
		}

		private string ReplaceInsideStringAndroid(string type, string stringToReplace, string replaceBy, AnalysedText analysedText)
		{
			string result = "";
			if (!stringToReplace.Contains(replaceBy + "_") && !stringToReplace.Contains(replaceBy + "^") && !stringToReplace.Contains(replaceBy + "@") && !stringToReplace.Contains(replaceBy + "#"))
			{
				if (type.Equals("irregulars") && analysedText.irregulars.Contains(replaceBy))
				{
					IrregularObject irregularObject = new IrregularVerbsManager().GetWordBy(replaceBy);
					if (irregularObject != null)
					{
						result = CustomRegexReplace.RegexReplaceForAndroid(stringToReplace, replaceBy, "@" + irregularObject.first + "@", "#");
					}
					else
					{
						result = stringToReplace;
					}
				}
				else if (type.Equals("archaisms") && analysedText.archaisms.Contains(replaceBy))
				{
					result = CustomRegexReplace.RegexReplaceForAndroid(stringToReplace, replaceBy, "archaisms", "_");
				}
				else if (type.Equals("slangs") && analysedText.slangs.Contains(replaceBy))
				{
					result = CustomRegexReplace.RegexReplaceForAndroid(stringToReplace, replaceBy, "slangs", "^");
				}
				else if (type.Equals("expressions") && analysedText.expressions.Contains(replaceBy))
				{
					result = CustomRegexReplace.RegexReplaceForAndroid(stringToReplace, replaceBy, "expressions", "@");
				}
				else if (type.Equals("repeated") && analysedText.repeated.Contains(replaceBy))
				{
					result = CustomRegexReplace.RegexReplaceForAndroid(stringToReplace, replaceBy, "repeated", "#");
				}
			}
			else
				result = stringToReplace;
			return result;
		}

		public string AnalyseFullText(string text, int limit, string type)
		{
			string[] words = Split.SplitTextToWords(text);
			string[] wordsClear = CleanSentence.GetCleanWordsArray(words);
			archaismsContains.UnionWith(archaismRepository.GetIntersects(words));
			slangsContains.UnionWith(slangRepository.GetIntersects(words));
			//irregularsContains.UnionWith(irregularVerbsRepository.GetIntersects(wordsClear));
			expressionsContains.UnionWith(expressionsRepository.GetIntersects(text));
			repeatedContains.UnionWith(StringFullIntersectPercent(wordsClear, limit));

			AnalysedText analysedText = new AnalysedText(repeatedContains, archaismsContains, slangsContains, irregularsContains, expressionsContains);

			if (analysedText.IfDataExists())
			{
				for (int placeInExpressionsArray = 0; placeInExpressionsArray < analysedText.expressions.Count; placeInExpressionsArray++)
				{
					if (type.Equals("angular"))
					{
						text = this.ReplaceInsideString("expressions", text, analysedText.expressions.ElementAt(placeInExpressionsArray), analysedText, idCounter + "");
					}
					else
					{
						text = this.ReplaceInsideStringAndroid("expressions", text, analysedText.expressions.ElementAt(placeInExpressionsArray), analysedText);
					}
					idCounter++;
				}

				for (int placeInArchaismsArray = 0; placeInArchaismsArray < analysedText.archaisms.Count; placeInArchaismsArray++)
				{
					if (type.Equals("angular"))
					{
						text = this.ReplaceInsideString("archaisms", text, analysedText.archaisms.ElementAt(placeInArchaismsArray), analysedText, idCounter + "");
					}
					else
					{
						text = this.ReplaceInsideStringAndroid("archaisms", text, analysedText.archaisms.ElementAt(placeInArchaismsArray), analysedText);
					}
					idCounter++;
				}

				for (int placeInSlangsArray = 0; placeInSlangsArray < analysedText.slangs.Count; placeInSlangsArray++)
				{
					if (type.Equals("angular"))
					{
						text = this.ReplaceInsideString("slangs", text, analysedText.slangs.ElementAt(placeInSlangsArray), analysedText, idCounter + "");
					}
					else
					{
						text = this.ReplaceInsideStringAndroid("slangs", text, analysedText.slangs.ElementAt(placeInSlangsArray), analysedText);
					}
					idCounter++;
				}

				for (int placeInIrregularsArray = 0; placeInIrregularsArray < analysedText.irregulars.Count; placeInIrregularsArray++)
				{
					if (type.Equals("angular"))
					{
						text = this.ReplaceInsideString("irregulars", text, analysedText.irregulars.ElementAt(placeInIrregularsArray), analysedText, idCounter + "");
					}
					else
					{
						text = this.ReplaceInsideStringAndroid("irregulars", text, analysedText.irregulars.ElementAt(placeInIrregularsArray), analysedText);
					}
					idCounter++;
				}

				for (int placeInRepeatedArray = 0; placeInRepeatedArray < analysedText.repeated.Count; placeInRepeatedArray++)
				{
					if (type.Equals("angular"))
					{
						text = this.ReplaceInsideString("repeated", text, analysedText.repeated.ElementAt(placeInRepeatedArray), analysedText, idCounter + "");
					}
					else
					{
						text = this.ReplaceInsideStringAndroid("repeated", text, analysedText.repeated.ElementAt(placeInRepeatedArray), analysedText);
					}
					idCounter++;
				}
			}
			return text;
		}

		public string CompareAllSentencesWords(string[] text, string type)
		{
			int size = 2;
			if (text.Count() > 1)
			{
				size = text.Count();
			}

			for (int placeInTextArray = 0; placeInTextArray < size - 1; placeInTextArray++)
			{
				TwoSenteces twoSenteces;
				if (text.Count() == 1)
				{
					twoSenteces = new TwoSenteces(text[placeInTextArray], "");
				}
				else
				{
					twoSenteces = new TwoSenteces(text[placeInTextArray], text[placeInTextArray + 1]);
				}
				AnalysedText analysedText = AnalyseTwoSentences(twoSenteces);
				if (analysedText.IfDataExists())
				{
					for (int placeInRepeatedArray = 0; placeInRepeatedArray < analysedText.repeated.Count; placeInRepeatedArray++)
					{
						if (type.Equals("angular"))
						{
							text[placeInTextArray] = this.ReplaceInsideString("repeated", text[placeInTextArray], analysedText.repeated.ElementAt(placeInRepeatedArray), analysedText, idCounter + "");
							if (text.Count() > 1)
							{
								text[placeInTextArray + 1] = this.ReplaceInsideString("repeated", text[placeInTextArray + 1], analysedText.repeated.ElementAt(placeInRepeatedArray), analysedText, idCounter + "+1");
							}
						}
						else
						{
							text[placeInTextArray] = this.ReplaceInsideStringAndroid("repeated", text[placeInTextArray], analysedText.repeated.ElementAt(placeInRepeatedArray), analysedText);
							if (text.Count() > 1)
							{
								text[placeInTextArray + 1] = this.ReplaceInsideStringAndroid("repeated", text[placeInTextArray + 1], analysedText.repeated.ElementAt(placeInRepeatedArray), analysedText);
							}
						}
						idCounter++;
					}

					for (int placeInArchaismsArray = 0; placeInArchaismsArray < analysedText.archaisms.Count; placeInArchaismsArray++)
					{
						if (type.Equals("angular"))
						{
							text[placeInTextArray] = this.ReplaceInsideString("archaisms", text[placeInTextArray], analysedText.archaisms.ElementAt(placeInArchaismsArray), analysedText, idCounter + "");
							if (text.Count() > 1)
							{
								text[placeInTextArray + 1] = this.ReplaceInsideString("archaisms", text[placeInTextArray + 1], analysedText.archaisms.ElementAt(placeInArchaismsArray), analysedText, idCounter + "1");
							}
						}
						else
						{
							text[placeInTextArray] = this.ReplaceInsideStringAndroid("archaisms", text[placeInTextArray], analysedText.archaisms.ElementAt(placeInArchaismsArray), analysedText);
							if (text.Count() > 1)
							{
								text[placeInTextArray + 1] = this.ReplaceInsideStringAndroid("archaisms", text[placeInTextArray + 1], analysedText.archaisms.ElementAt(placeInArchaismsArray), analysedText);
							}
						}
						idCounter++;
					}

					for (int placeInExpressionsArray = 0; placeInExpressionsArray < analysedText.expressions.Count; placeInExpressionsArray++)
					{
						if (type.Equals("angular"))
						{
							text[placeInTextArray] = this.ReplaceInsideString("expressions", text[placeInTextArray], analysedText.expressions.ElementAt(placeInExpressionsArray), analysedText, idCounter + "");
							if (text.Count() > 1)
							{
								text[placeInTextArray + 1] = this.ReplaceInsideString("expressions", text[placeInTextArray + 1], analysedText.expressions.ElementAt(placeInExpressionsArray), analysedText, idCounter + "1");
							}
						}
						else
						{
							text[placeInTextArray] = this.ReplaceInsideStringAndroid("expressions", text[placeInTextArray], analysedText.expressions.ElementAt(placeInExpressionsArray), analysedText);
							if (text.Count() > 1)
							{
								text[placeInTextArray + 1] = this.ReplaceInsideStringAndroid("expressions", text[placeInTextArray + 1], analysedText.expressions.ElementAt(placeInExpressionsArray), analysedText);
							}
						}
						idCounter++;
					}

					for (int placeInIrregularsArray = 0; placeInIrregularsArray < analysedText.irregulars.Count; placeInIrregularsArray++)
					{
						if (type.Equals("angular"))
						{
							text[placeInTextArray] = this.ReplaceInsideString("irregulars", text[placeInTextArray], analysedText.irregulars.ElementAt(placeInIrregularsArray), analysedText, idCounter + "");
							if (text.Count() > 1)
							{
								text[placeInTextArray + 1] = this.ReplaceInsideString("irregulars", text[placeInTextArray + 1], analysedText.irregulars.ElementAt(placeInIrregularsArray), analysedText, idCounter + "1");
							}
						}
						else
						{
							text[placeInTextArray] = this.ReplaceInsideStringAndroid("irregulars", text[placeInTextArray], analysedText.irregulars.ElementAt(placeInIrregularsArray), analysedText);
							if (text.Count() > 1)
							{
								text[placeInTextArray + 1] = this.ReplaceInsideStringAndroid("irregulars", text[placeInTextArray + 1], analysedText.irregulars.ElementAt(placeInIrregularsArray), analysedText);
							}
						}
						idCounter++;
					}

					for (int placeInSlangsArray = 0; placeInSlangsArray < analysedText.slangs.Count; placeInSlangsArray++)
					{
						if (type.Equals("angular"))
						{
							text[placeInTextArray] = this.ReplaceInsideString("slangs", text[placeInTextArray], analysedText.slangs.ElementAt(placeInSlangsArray), analysedText, idCounter + "");
							if (text.Count() > 1)
							{
								text[placeInTextArray + 1] = this.ReplaceInsideString("slangs", text[placeInTextArray + 1], analysedText.slangs.ElementAt(placeInSlangsArray), analysedText, idCounter + "1");
							}
						}
						else
						{
							text[placeInTextArray] = this.ReplaceInsideStringAndroid("slangs", text[placeInTextArray], analysedText.slangs.ElementAt(placeInSlangsArray), analysedText);
							if (text.Count() > 1)
							{
								text[placeInTextArray + 1] = this.ReplaceInsideStringAndroid("slangs", text[placeInTextArray + 1], analysedText.slangs.ElementAt(placeInSlangsArray), analysedText);
							}
						}
						idCounter++;
					}
				}
			}
			var result = String.Join(" ", text.ToArray());
			return result;
		}
	}
}