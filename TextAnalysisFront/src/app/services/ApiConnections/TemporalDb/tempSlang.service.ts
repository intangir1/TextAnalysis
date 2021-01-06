import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { byTempSlangs } from 'src/environments/environment';
import { ISingleWordsRepository } from 'src/app/repository/baseInterfaces/ISingleWordsRepository';
import { TemporalObject } from 'src/app/models/TemporalObject';
import { FullText } from 'src/app/models/FullText';

@Injectable({
  providedIn: 'root'
})
export class TempSlangService implements ISingleWordsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }

  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<TemporalObject[]>(byTempSlangs, { headers: he });
    observable.subscribe(returnedSlangs=>{
      const action: Action={type:ActionType.GetAllTempSlangs, payload:returnedSlangs};
      this.redux.dispatch(action);
      this.logger.debug("Get All TempSlangs: ", returnedSlangs);
    }, error => {
      const action: Action={type:ActionType.GetAllTempSlangsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All TempSlangs Error: ", error.message);
    });
  }

  public PostWord(word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.post<TemporalObject>(byTempSlangs, new FullText(word), { headers: he });
    observable.subscribe(returnedSlangs=>{
      const action: Action={type:ActionType.PostTempSlang, payload:returnedSlangs};
      this.redux.dispatch(action);
      this.logger.debug("Post TempSlang: ", returnedSlangs);
      alert("The " + word + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PostTempSlangError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post TempSlang Error: ", error.message);
      alert("Error: The " + word + " hasn't been submitted for review");
    });
  }

  public PutWord(word_to_replace:string, word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<TemporalObject>(byTempSlangs+word_to_replace, new FullText(word), { headers: he });
    observable.subscribe(returnedSlangs=>{
      const action: Action={type:ActionType.PutTempSlang, payload:returnedSlangs};
      this.redux.dispatch(action);
      this.logger.debug("Put TempSlang: ", returnedSlangs);
      alert("The " + word + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PutTempSlangError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put TempSlang Error: ", error.message);
      alert("Error: The " + word + " hasn't been submitted for review");
    });
  }

  public DeleteWord(mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObject>(byTempSlangs+mongoId, { observe: 'response', headers: he });
    observable.subscribe(res=>{
      this.logger.debug("Delete TempSlang result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteTempSlang, payload:mongoId };
        this.redux.dispatch(action);
        this.logger.debug("Delete TempSlang: ", mongoId);
        const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
        this.redux.dispatch(action2);
        this.logger.debug("Delete Temporal Word: ", mongoId);
        alert("The request with id " + mongoId + " has been rejected");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteTempSlangError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete TempSlang Error: ", error.message);
    });
  }

  public DeleteCollection(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObject>(byTempSlangs, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All TempSlangs result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllTempSlangs, payload:"Slang" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All TempSlangs: ", "Slang");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllTempSlangsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All TempSlangs Error: ", error.message);
    });
  }
}