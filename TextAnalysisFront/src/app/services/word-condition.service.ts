import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class WordConditionService {

  constructor() { }

  public GetClearWord(word:string):string
	{
    let prefixesPattern = "^(re|dis|over|un|mis|out|de|fore|inter|pre|sub|trans|under)";
		let suffixesPattern = "(ing|ed|ies)$";
    let rgxAcceptableEndings = "(see|add|ass|ess|all|ill|ell|oll|iss|ree|ess|uzz|oss|lee|iff)$";
    let exceptionsEndingPattern = "(thing)$";
    

    let tempWord:string;

    let prefixesRegex = new RegExp(prefixesPattern);
    let suffixesRegex = new RegExp(suffixesPattern);
    let acceptableEndingsRegex = new RegExp(rgxAcceptableEndings);
    let exceptionsEndingRegex = new RegExp(exceptionsEndingPattern);
    
    tempWord = word.replace(prefixesRegex, "");
    
    if(tempWord!==word && this.WordAcccepted(tempWord)){
      word = tempWord;
    }

    tempWord = word.replace(suffixesRegex, "");


    if(tempWord!==word && !exceptionsEndingRegex.test(word) && this.WordAcccepted(tempWord)){
      word = tempWord;
      if (word.length>2){
        let ending = Array.from(word.substring(word.length - 2));

        let unique = () => {
          const s = new Set();
          return ending.filter((item) => {
            if (s.has(item)) {
              return false;
            }
            s.add(item);
            return true;
          });
        }
		  	if (unique.length == 1 && !acceptableEndingsRegex.test(word)){
		  		word = word.substring(0, word.length - 1);
		  	}
		  }
    }
    
		return word;
  }

  public WordAcccepted(word:string):boolean{
    let exceptionsPattern = "(go|be|do)";
    let exceptionsRegex = new RegExp(exceptionsPattern);
    if(word!==null && word!==undefined && word!=="" && (word.length>2 || exceptionsRegex.test(word))){
      return true;
    }
    return false;
  }
}
