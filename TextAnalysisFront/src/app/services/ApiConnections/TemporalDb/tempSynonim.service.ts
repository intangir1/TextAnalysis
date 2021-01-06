import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { byTempSynonim } from 'src/environments/environment';
import { TemporalObject } from 'src/app/models/TemporalObject';
import { IRelationalWordsRepository } from 'src/app/repository/baseInterfaces/IRelationalWordsRepository';
import { FullText } from 'src/app/models/FullText';

@Injectable({
  providedIn: 'root'
})
export class TempSynonimService implements IRelationalWordsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }
  GetWordsBy(word: string): void {
    throw new Error("Method not implemented.");
  }

  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<TemporalObject[]>(byTempSynonim, { headers: he });
    observable.subscribe(returnedSynonims=>{
      const action: Action={type:ActionType.GetAllTempSynonims, payload:returnedSynonims};
      this.redux.dispatch(action);
      this.logger.debug("Get All TempSynonims: ", returnedSynonims);
    }, error => {
      const action: Action={type:ActionType.GetAllTempSynonimsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All TempSynonims Error: ", error.message);
    });
  }

  public PostWord(word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.post<TemporalObject>(byTempSynonim, new FullText(word), { headers: he });
    observable.subscribe(returnedSynonims=>{
      const action: Action={type:ActionType.PostTempSynonim, payload:returnedSynonims};
      this.redux.dispatch(action);
      this.logger.debug("Post TempSynonim: ", returnedSynonims);
      alert("The " + word + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PostTempSynonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post TempSynonim Error: ", error.message);
      alert("Error: The " + word + " hasn't been submitted for review");
    });
  }

  public PutWord(word_to_replace:string, word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<TemporalObject>(byTempSynonim+word_to_replace, new FullText(word), { headers: he });
    observable.subscribe(returnedSynonims=>{
      const action: Action={type:ActionType.PutTempSynonim, payload:returnedSynonims};
      this.redux.dispatch(action);
      this.logger.debug("Put TempSynonim: ", returnedSynonims);
      alert("The " + word + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PutTempSynonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put TempSynonim Error: ", error.message);
      alert("Error: The " + word + " hasn't been submitted for review");
    });
  }

  public InsertWord(word_to_find:string, word_to_add:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<TemporalObject>(byTempSynonim+"insert/"+word_to_find, new FullText(word_to_add), { headers: he });
    observable.subscribe(returnedSynonims=>{
      const action: Action={type:ActionType.InsertTempSynonim, payload:returnedSynonims};
      this.redux.dispatch(action);
      this.logger.debug("Insert TempSynonim: ", returnedSynonims);
      alert("The " + word_to_add + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.InsertTempSynonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Insert TempSynonim Error: ", error.message);
      alert("Error: The " + word_to_add + " hasn't been submitted for review");
    });
  }

  public DeleteWord(mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObject>(byTempSynonim+mongoId, { observe: 'response', headers: he });
    observable.subscribe(res=>{
      this.logger.debug("Delete TempSynonim result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteTempSynonim, payload:mongoId };
        this.redux.dispatch(action);
        this.logger.debug("Delete TempSynonim: ", mongoId);
        const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
        this.redux.dispatch(action2);
        this.logger.debug("Delete Temporal Word: ", mongoId);
        alert("The request with id " + mongoId + " has been rejected");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteTempSynonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete TempSynonim Error: ", error.message);
    });
  }

  public DeleteCollection(word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObject>(byTempSynonim+word, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All TempSynonims result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllTempSynonims, payload:"Synonim" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All TempSynonims: ", "Synonim");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllTempSynonimsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All TempSynonims Error: ", error.message);
    });
  }
}