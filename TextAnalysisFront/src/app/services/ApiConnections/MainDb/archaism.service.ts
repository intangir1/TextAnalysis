import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { byArchaism } from 'src/environments/environment';
import { ISingleWordsRepository } from 'src/app/repository/baseInterfaces/ISingleWordsRepository';
import { FullText } from 'src/app/models/FullText';

@Injectable({
  providedIn: 'root'
})
export class ArchaismService implements ISingleWordsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }

  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<string[]>(byArchaism, { headers: he });
    observable.subscribe(returnedArchaisms=>{
      const action: Action={type:ActionType.GetAllArchaisms, payload:returnedArchaisms};
      this.redux.dispatch(action);
      this.logger.debug("Get All Archaisms: ", returnedArchaisms);
    }, error => {
      const action: Action={type:ActionType.GetAllArchaismsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All Archaisms Error: ", error.message);
    });
  }

  public PostWord(word:string, mongoId:string): void {
    let he = { headers: new HttpHeaders({ 'Content-Type': 'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token'),'frontType': 'angular' }), responseType: 'text' as 'json' };
    let observable = this.http.post<string>(byArchaism, new FullText(word), he);
    observable.subscribe(returnedArchaisms=>{
      const action: Action={type:ActionType.PostArchaism, payload:returnedArchaisms};
      this.redux.dispatch(action);
      this.logger.debug("Post Archaism: ", returnedArchaisms);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word + " has been posted");
    }, error => {
      const action: Action={type:ActionType.PostArchaismError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post Archaism Error: ", error.message);
      alert("Error: The " + word + " hasn't been posted");
    });
  }

  public PutWord(word_to_replace:string, word:string, mongoId:string): void {
    let he = { headers: new HttpHeaders({ 'Content-Type': 'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token'),'frontType': 'angular' }), responseType: 'text' as 'json' };
    let observable = this.http.put<string>(byArchaism+word_to_replace, new FullText(word), he);
    observable.subscribe(returnedArchaisms=>{
      const action: Action={type:ActionType.PutArchaism, payload:returnedArchaisms};
      this.redux.dispatch(action);
      this.logger.debug("Put Archaism: ", returnedArchaisms);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word + " has been put");
    }, error => {
      const action: Action={type:ActionType.PutArchaismError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put Archaism Error: ", error.message);
      alert("Error: The " + word + " hasn't been put");
    });
  }

  public DeleteWord(wordToRemove:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<string[]>(byArchaism+wordToRemove, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete Archaism result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteArchaism, payload:wordToRemove };
        this.redux.dispatch(action);
        this.logger.debug("Delete Archaism: ", wordToRemove);
      }
    }, error => {
      const action: Action={type:ActionType.DeleteArchaismError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete Archaism Error: ", error.message);
    });
  }

  public DeleteCollection(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<string>(byArchaism, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All Archaisms result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllArchaisms, payload:"Archaism" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All Archaisms: ", "Archaism");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllArchaismsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All Archaisms Error: ", error.message);
    });
  }
}