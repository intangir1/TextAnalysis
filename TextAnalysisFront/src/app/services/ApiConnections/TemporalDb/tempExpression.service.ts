import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { byTempExpressions } from 'src/environments/environment';
import { ISingleWordsRepository } from 'src/app/repository/baseInterfaces/ISingleWordsRepository';
import { TemporalObject } from 'src/app/models/TemporalObject';
import { FullText } from 'src/app/models/FullText';

@Injectable({
  providedIn: 'root'
})
export class TempExpressionService implements ISingleWordsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }

  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<TemporalObject[]>(byTempExpressions, { headers: he });
    observable.subscribe(returnedExpressions=>{
      const action: Action={type:ActionType.GetAllTempExpressions, payload:returnedExpressions};
      this.redux.dispatch(action);
      this.logger.debug("Get All TempExpressions: ", returnedExpressions);
    }, error => {
      const action: Action={type:ActionType.GetAllTempExpressionsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All TempExpressions Error: ", error.message);
    });
  }

  public PostWord(word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.post<TemporalObject>(byTempExpressions, new FullText(word), { headers: he });
    observable.subscribe(returnedExpressions=>{
      const action: Action={type:ActionType.PostTempExpression, payload:returnedExpressions};
      this.redux.dispatch(action);
      this.logger.debug("Post TempExpression: ", returnedExpressions);
      alert("The " + word + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PostTempExpressionError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post TempExpression Error: ", error.message);
      alert("Error: The " + word + " hasn't been submitted for review");
    });
  }

  public PutWord(word_to_replace:string, word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<TemporalObject>(byTempExpressions+word_to_replace, new FullText(word), { headers: he });
    observable.subscribe(returnedExpressions=>{
      const action: Action={type:ActionType.PutTempExpression, payload:returnedExpressions};
      this.redux.dispatch(action);
      this.logger.debug("Put TempExpression: ", returnedExpressions);
      alert("The " + word + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PutTempExpressionError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put TempExpression Error: ", error.message);
      alert("Error: The " + word + " hasn't been submitted for review");
    });
  }

  public DeleteWord(mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObject>(byTempExpressions+mongoId, { observe: 'response', headers: he });
    observable.subscribe(res=>{
      this.logger.debug("Delete TempExpression result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteTempExpression, payload:mongoId };
        this.redux.dispatch(action);
        this.logger.debug("Delete TempExpression: ", mongoId);
        const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
        this.redux.dispatch(action2);
        this.logger.debug("Delete Temporal Word: ", mongoId);
        alert("The request with id " + mongoId + " has been rejected");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteTempExpressionError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete TempExpression Error: ", error.message);
    });
  }

  public DeleteCollection(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObject>(byTempExpressions, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All TempExpressions result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllTempExpressions, payload:"Expression" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All TempExpressions: ", "Expression");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllTempExpressionsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All TempExpressions Error: ", error.message);
    });
  }
}