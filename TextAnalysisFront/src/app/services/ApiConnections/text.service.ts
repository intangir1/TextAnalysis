import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Action } from '../../redux/action';
import { ActionType } from '../../redux/action-type';
import { Store } from '../../redux/store';
import { NgRedux } from 'ng2-redux';
import { LogService } from '.././log.service';
import { byAllSentencesWords, byAnalyseFullText } from 'src/environments/environment';
import { IFullAnalyticsRepository } from 'src/app/repository/IFullAnalyticsRepository';
import { FullText } from 'src/app/models/FullText';

@Injectable({
  providedIn: 'root'
})
export class TextService implements IFullAnalyticsRepository{

  public constructor(
    private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }

  he = { headers: new HttpHeaders({ 'Content-Type': 'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token'),'frontType': 'angular' }), responseType: 'text' as 'json' };

  public AnalyseFullText(fullText:FullText, limit:number): void {
    let observable = this.http.post<string>(byAnalyseFullText+limit, fullText, this.he);
    observable.subscribe(returnedText=>{
      const action: Action={type:ActionType.CompareAllSentencesWords, payload:returnedText};
      this.redux.dispatch(action);
      this.logger.debug("Compare All Words: ", returnedText);
    }, error => {
      const action: Action={type:ActionType.CompareAllSentencesWordsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Compare All Words error: ", error.message);
    });
  }
  
  public CompareAllSentencesWords(text:string[]): void {
    let observable = this.http.post<string>(byAllSentencesWords, text, this.he);
    observable.subscribe(returnedText=>{
      const action: Action={type:ActionType.CompareAllSentencesWords, payload:returnedText};
      this.redux.dispatch(action);
      this.logger.debug("Compare All Sentences Words: ", returnedText);
    }, error => {
      const action: Action={type:ActionType.CompareAllSentencesWordsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Compare All Sentences Words error: ", error.message);
    });
  }
}