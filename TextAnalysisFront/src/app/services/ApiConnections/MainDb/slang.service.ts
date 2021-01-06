import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { bySlangs } from 'src/environments/environment';
import { ISingleWordsRepository } from 'src/app/repository/baseInterfaces/ISingleWordsRepository';
import { FullText } from 'src/app/models/FullText';

@Injectable({
  providedIn: 'root'
})
export class SlangService implements ISingleWordsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }

  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<string[]>(bySlangs, { headers: he });
    observable.subscribe(returnedSlangs=>{
      const action: Action={type:ActionType.GetAllSlangs, payload:returnedSlangs};
      this.redux.dispatch(action);
      this.logger.debug("Get All Slangs: ", returnedSlangs);
    }, error => {
      const action: Action={type:ActionType.GetAllSlangsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All Slangs Error: ", error.message);
    });
  }

  public PostWord(word:string, mongoId:string): void {
    let he = { headers: new HttpHeaders({ 'Content-Type': 'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token'),'frontType': 'angular' }), responseType: 'text' as 'json' };
    let observable = this.http.post<string>(bySlangs, new FullText(word), he);
    observable.subscribe(returnedSlangs=>{
      const action: Action={type:ActionType.PostSlang, payload:returnedSlangs};
      this.redux.dispatch(action);
      this.logger.debug("Post Slang: ", returnedSlangs);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word + " has been posted");
    }, error => {
      const action: Action={type:ActionType.PostSlangError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post Slang Error: ", error.message);
      alert("Error: The " + word + " hasn't been posted");
    });
  }

  public PutWord(word_to_replace:string, word:string, mongoId:string): void {
    let he = { headers: new HttpHeaders({ 'Content-Type': 'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token'),'frontType': 'angular' }), responseType: 'text' as 'json' };
    let observable = this.http.put<string>(bySlangs+word_to_replace, new FullText(word), he);
    observable.subscribe(returnedSlangs=>{
      const action: Action={type:ActionType.PutSlang, payload:returnedSlangs};
      this.redux.dispatch(action);
      this.logger.debug("Put Slang: ", returnedSlangs);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word + " has been put");
    }, error => {
      const action: Action={type:ActionType.PutSlangError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put Slang Error: ", error.message);
      alert("Error: The " + word + " hasn't been put");
    });
  }

  public DeleteWord(wordToRemove:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<string[]>(bySlangs+wordToRemove, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete Slang result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteSlang, payload:wordToRemove };
        this.redux.dispatch(action);
        this.logger.debug("Delete Slang: ", wordToRemove);
      }
    }, error => {
      const action: Action={type:ActionType.DeleteSlangError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete Slang Error: ", error.message);
    });
  }

  public DeleteCollection(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<string>(bySlangs, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All Slangs result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllSlangs, payload:"Slang" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All Slangs: ", "Slang");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllSlangsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All Slangs Error: ", error.message);
    });
  }
}