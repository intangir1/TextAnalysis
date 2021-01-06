import { Injectable } from '@angular/core';
import { Store } from '../redux/store';
import { NgRedux } from 'ng2-redux';
import { LogService } from './log.service';
import { KeyedCollection } from '../dictionary/KeyedCollection';
import { Action } from '../redux/action';
import { ActionType } from '../redux/action-type';
import { IFullAnalyticsRepository } from '../repository/IFullAnalyticsRepository';
import { FullText } from '../models/FullText';
import { WordConditionService } from './word-condition.service';

@Injectable({
  providedIn: 'root'
})
export class LocalAnalyzerService implements IFullAnalyticsRepository{

  private idCounter:number=0;
  private everyPointPattern:RegExp = new RegExp('[:;,.!?]+', 'g');

  public constructor(
    private wordConditionService:WordConditionService,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }
  
  public AnalyseFullText(fullText:FullText, limit:number){
    let text = fullText.textToCheck;
    let words:string[] = this.splitSentence(text);
    let clearWords:string[] = this.GetCleanWordsArray(words);
    let repeated = this.StringFullIntersectPercent(clearWords, limit);
    if (repeated!=null){
      for (let placeInRepeatedArray=0; placeInRepeatedArray<repeated.length; placeInRepeatedArray++){
        text = this.ReplaceInsideString(text, repeated[placeInRepeatedArray], this.idCounter+"");
        this.idCounter++;
      }
    }
    const action: Action={type:ActionType.CompareAllSentencesWords, payload:text};
    this.redux.dispatch(action);
    this.logger.debug("Compare Text Words: ", text);
  }
  
  public CompareAllSentencesWords(text:string[]): void {
    let size = 2;
		if (text.length > 1)
		{
			size = text.length;
		}
    for(let placeInTextArray=0; placeInTextArray<size-1; placeInTextArray++){
      let firstSentence = text[placeInTextArray].replace(/<[^>]+>/g, '');
      let firstSplitted = this.splitSentence(firstSentence);
      let clearFirstSplitted:string[] = this.GetCleanWordsArray(firstSplitted);
      let clearTwoArraysSplitted:string[] = clearFirstSplitted; 
      if (text.length > 1)
		  {
        let secondSplitted = this.splitSentence(text[placeInTextArray+1]);
        let clearSecondSplitted:string[] = this.GetCleanWordsArray(secondSplitted);
        clearTwoArraysSplitted = clearFirstSplitted.concat(clearSecondSplitted);
		  }
      
      let repeatedSentencesWords: Set<string> = new Set<string>();

      this.StringFullIntersectBoolean(clearTwoArraysSplitted).forEach(el => repeatedSentencesWords.add(el));
      
      let repeated=Array.from(repeatedSentencesWords.values());
      if (repeated!=null){
        for (let placeInRepeatedArray=0; placeInRepeatedArray<repeated.length; placeInRepeatedArray++){
          text[placeInTextArray] = this.ReplaceInsideString(text[placeInTextArray], repeated[placeInRepeatedArray], this.idCounter+"");
          if (text.length > 1)
		      {
            text[placeInTextArray+1] = this.ReplaceInsideString(text[placeInTextArray+1], repeated[placeInRepeatedArray], this.idCounter+"+1");
		      }
          this.idCounter++;
        }
      }
    }
    const action: Action={type:ActionType.CompareAllSentencesWords, payload:text.join(" ")};
    this.redux.dispatch(action);
    this.logger.debug("Compare All Sentences Words: ", text.join(" "));
  }
  
  public ReplaceInsideString(stringToReplace:string, replaceBy:string, idCounter:string){
    let str = stringToReplace.replace(new RegExp(replaceBy, "gi"), match => {
      if(!stringToReplace.includes(replaceBy + '</span>'))
        return '<span id="r1e2p3e4a5t6e7d' + idCounter + '">' + match + '</span>';
      else
        return match;
    });
    return str;
  }

  private splitSentence(sentence:string):string[]
	{
    sentence = sentence.replace(this.everyPointPattern, "");
    let words = sentence.trim().split(' ');
    this.logger.debug("splitSentence: ", words);
		return words;
  }


  private GetCleanWordsArray(words:string[]):string[]
	{
    let clean_words:string[] = Array.from(words);
		clean_words = clean_words.map(cw => this.wordConditionService.GetClearWord(cw));
		return clean_words;
  }
  
  private StringFullIntersectBoolean(words:string[]):string[]{
    let countsDictionary:KeyedCollection<boolean> = new KeyedCollection<boolean>();
    for (let placeInWordsArray=0; placeInWordsArray<words.length; placeInWordsArray++){
      if (this.wordConditionService.WordAcccepted(words[placeInWordsArray].toLowerCase())){
        let keyIncludesThis = countsDictionary.WordIncludesKey(words[placeInWordsArray]);
        if (!countsDictionary.ContainsKey(words[placeInWordsArray].toLowerCase()) && !keyIncludesThis) {
          countsDictionary.Add(words[placeInWordsArray].toLowerCase(), false);
        } else if (countsDictionary.ContainsKey(words[placeInWordsArray].toLowerCase())) {
          countsDictionary.Add(words[placeInWordsArray].toLowerCase(), true);
        } else {
          countsDictionary.Add(keyIncludesThis.toLowerCase(), true);
        }
      }
    }
    let keys: string[] = countsDictionary.Keys();
    let repeatedSentencesWords: Set<string> = new Set<string>();
    for (let i=0; i<keys.length; i++) {
      let key_In_I_Place = keys[i];
      if (countsDictionary.ContainsKey(key_In_I_Place) && countsDictionary.Remove(key_In_I_Place)===true) {
        repeatedSentencesWords.add(key_In_I_Place);
      }
    };
    let repeated=Array.from(repeatedSentencesWords.values());
    return repeated;
  }
  
  private StringFullIntersectPercent(words:string[], limit:number):string[]{
    let countsDictionary:KeyedCollection<number> = new KeyedCollection<number>();
    for (let placeInWordsArray=0; placeInWordsArray<words.length; placeInWordsArray++){
      if (this.wordConditionService.WordAcccepted(words[placeInWordsArray].toLowerCase())){
        let keyIncludesThis = countsDictionary.WordIncludesKey(words[placeInWordsArray]);
        if (!countsDictionary.ContainsKey(words[placeInWordsArray].toLowerCase()) && !keyIncludesThis) {
          countsDictionary.Add(words[placeInWordsArray].toLowerCase(), 1);
        } else if (countsDictionary.ContainsKey(words[placeInWordsArray].toLowerCase())) {
          countsDictionary.Add(words[placeInWordsArray].toLowerCase(), countsDictionary.Item(words[placeInWordsArray].toLowerCase())+1);
        } else {
          countsDictionary.Add(keyIncludesThis.toLowerCase(), countsDictionary.Item(keyIncludesThis.toLowerCase())+1);
        }
      }
    }
    let keys: string[] = countsDictionary.Keys();
    let repeatedSentencesWords: Set<string> = new Set<string>();
    for (let i=0; i<keys.length; i++) {
      let key_In_I_Place = keys[i];
      if (countsDictionary.ContainsKey(key_In_I_Place) && countsDictionary.Remove(key_In_I_Place)*100/words.length>=limit) {
        repeatedSentencesWords.add(key_In_I_Place);
      }
    };
    let repeated=Array.from(repeatedSentencesWords.values());
    return repeated;
  }
}