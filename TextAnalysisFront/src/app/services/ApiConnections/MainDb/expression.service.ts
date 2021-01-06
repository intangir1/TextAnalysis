import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { byExpressions } from 'src/environments/environment';
import { ISingleWordsRepository } from 'src/app/repository/baseInterfaces/ISingleWordsRepository';
import { FullText } from 'src/app/models/FullText';

@Injectable({
  providedIn: 'root'
})
export class ExpressionService implements ISingleWordsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }

  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<string[]>(byExpressions, { headers: he });
    observable.subscribe(returnedExpressions=>{
      const action: Action={type:ActionType.GetAllExpressions, payload:returnedExpressions};
      this.redux.dispatch(action);
      this.logger.debug("Get All Expressions: ", returnedExpressions);
    }, error => {
      const action: Action={type:ActionType.GetAllExpressionsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All Expressions Error: ", error.message);
    });
  }

  public PostWord(word:string, mongoId:string): void {
    let he = { headers: new HttpHeaders({ 'Content-Type': 'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token'),'frontType': 'angular' }), responseType: 'text' as 'json' };
    let observable = this.http.post<string>(byExpressions, new FullText(word), he);
    observable.subscribe(returnedExpressions=>{
      const action: Action={type:ActionType.PostExpression, payload:returnedExpressions};
      this.redux.dispatch(action);
      this.logger.debug("Post Expression: ", returnedExpressions);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word + " has been posted");
    }, error => {
      const action: Action={type:ActionType.PostExpressionError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post Expression Error: ", error.message);
      alert("Error: The " + word + " hasn't been posted");
    });
  }

  public PutWord(word_to_replace:string, word:string, mongoId:string): void {
    let he = { headers: new HttpHeaders({ 'Content-Type': 'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token'),'frontType': 'angular' }), responseType: 'text' as 'json' };
    let observable = this.http.put<string>(byExpressions+word_to_replace, new FullText(word), he);
    observable.subscribe(returnedExpressions=>{
      const action: Action={type:ActionType.PutExpression, payload:returnedExpressions};
      this.redux.dispatch(action);
      this.logger.debug("Put Expression: ", returnedExpressions);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word + " has been put");
    }, error => {
      const action: Action={type:ActionType.PutExpressionError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put Expression Error: ", error.message);
      alert("Error: The " + word + " hasn't been put");
    });
  }

  public DeleteWord(wordToRemove:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<string[]>(byExpressions+wordToRemove, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete Expression result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteExpression, payload:wordToRemove };
        this.redux.dispatch(action);
        this.logger.debug("Delete Expression: ", wordToRemove);
      }
    }, error => {
      const action: Action={type:ActionType.DeleteExpressionError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete Expression Error: ", error.message);
    });
  }

  public DeleteCollection(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<string>(byExpressions, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All Expressions result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllExpressions, payload:"Expression" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All Expressions: ", "Expression");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllExpressionsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All Expressions Error: ", error.message);
    });
  }
}