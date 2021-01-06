import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { byTempArchaism } from 'src/environments/environment';
import { ISingleWordsRepository } from 'src/app/repository/baseInterfaces/ISingleWordsRepository';
import { TemporalObject } from 'src/app/models/TemporalObject';
import { FullText } from 'src/app/models/FullText';

@Injectable({
  providedIn: 'root'
})
export class TempArchaismService implements ISingleWordsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }

  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<TemporalObject[]>(byTempArchaism, { headers: he });
    observable.subscribe(returnedArchaisms=>{
      const action: Action={type:ActionType.GetAllTempArchaisms, payload:returnedArchaisms};
      this.redux.dispatch(action);
      this.logger.debug("Get All TempArchaisms: ", returnedArchaisms);
    }, error => {
      const action: Action={type:ActionType.GetAllTempArchaismsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All TempArchaisms Error: ", error.message);
    });
  }

  public PostWord(word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.post<TemporalObject>(byTempArchaism, new FullText(word), { headers: he });
    observable.subscribe(returnedArchaisms=>{
      const action: Action={type:ActionType.PostTempArchaism, payload:returnedArchaisms};
      this.redux.dispatch(action);
      this.logger.debug("Post TempArchaism: ", returnedArchaisms);
      alert("The " + word + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PostTempArchaismError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post TempArchaism Error: ", error.message);
      alert("Error: The " + word + " hasn't been submitted for review");
    });
  }

  public PutWord(word_to_replace:string, word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<TemporalObject>(byTempArchaism+word_to_replace, new FullText(word), { headers: he });
    observable.subscribe(returnedArchaisms=>{
      const action: Action={type:ActionType.PutTempArchaism, payload:returnedArchaisms};
      this.redux.dispatch(action);
      this.logger.debug("Put TempArchaism: ", returnedArchaisms);
      alert("The " + word + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PutTempArchaismError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put TempArchaism Error: ", error.message);
      alert("Error: The " + word + " hasn't been submitted for review");
    });
  }

  public DeleteWord(mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObject>(byTempArchaism+mongoId, { observe: 'response', headers: he });
    observable.subscribe(res=>{
      this.logger.debug("Delete TempArchaism result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteTempArchaism, payload:mongoId };
        this.redux.dispatch(action);
        this.logger.debug("Delete TempArchaism: ", mongoId);
        const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
        this.redux.dispatch(action2);
        this.logger.debug("Delete Temporal Word: ", mongoId);
        alert("The request with id " + mongoId + " has been rejected");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteTempArchaismError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete TempArchaism Error: ", error.message);
    });
  }

  public DeleteCollection(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObject>(byTempArchaism, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All TempArchaisms result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllTempArchaisms, payload:"Archaism" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All TempArchaisms: ", "Archaism");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllTempArchaismsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All TempArchaisms Error: ", error.message);
    });
  }
}