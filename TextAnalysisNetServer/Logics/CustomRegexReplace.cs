using System;
using System.Text.RegularExpressions;

namespace TextAnalysis
{
	public static class CustomRegexReplace
	{
		public static string RegexReplaceForAngular(string stringToReplace, string replaceBy, string id, string myIdCounter)
		{
			string idPlusMyIdCounter = Guid.NewGuid().ToString();
			string result = "";
			Regex regex = new Regex(replaceBy, RegexOptions.IgnoreCase);
			MatchCollection matches = regex.Matches(stringToReplace);
			if (matches.Count > 0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					if (!stringToReplace.Contains(matches[i].ToString() + "</span>"))
					{
						result = Regex.Replace(stringToReplace, matches[i].ToString(), "<span id='" + idPlusMyIdCounter + "'>" + matches[i].ToString() + "</span>");
						stringToReplace = result;
					}
					else
					{
						result = stringToReplace;
					}
				}
			}
			else
			{
				result = stringToReplace;
			}
			result = Regex.Replace(result, idPlusMyIdCounter, id+myIdCounter);
			return result;
		}

		public static string RegexReplaceForAndroid(string stringToReplace, string replaceBy, string id, string tag)
		{
			string idPlus = Guid.NewGuid().ToString();
			string result = "";
			Regex regex = new Regex(replaceBy, RegexOptions.IgnoreCase);
			MatchCollection matches = regex.Matches(stringToReplace);
			if (matches.Count > 0)
			{
				for (int i = 0; i < matches.Count; i++)
				{
					if (!stringToReplace.Contains(matches[i].ToString() + "_") && !stringToReplace.Contains(matches[i].ToString() + "^") && !stringToReplace.Contains(matches[i].ToString() + "@") && !stringToReplace.Contains(matches[i].ToString() + "#"))
					{
						if (!id.Equals("expressions") && !id.Equals("archaisms") && !id.Equals("slangs") && !id.Equals("repeated"))
						{
							result = Regex.Replace(stringToReplace, matches[i].ToString(), tag + idPlus + matches[i].ToString() + tag);
						}
						else
						{
							result = Regex.Replace(stringToReplace, matches[i].ToString(), tag + matches[i].ToString() + tag);
						}
						stringToReplace = result;
					}
					else
					{
						result = stringToReplace;
					}
				}
			}
			else
			{
				result = stringToReplace;
			}
			result = Regex.Replace(result, idPlus, id);
			return result;
		}
	}
}
