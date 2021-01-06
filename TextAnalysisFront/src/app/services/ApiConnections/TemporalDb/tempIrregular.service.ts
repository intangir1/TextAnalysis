import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { byTempIrregulars } from 'src/environments/environment';
import { IrregularObject } from 'src/app/models/IrregularObject';
import { TemporalObjectForIrregular } from 'src/app/models/TemporalObjectForIrregular';
import { IIrregularsRepository } from 'src/app/repository/extendedInterfaces/IIrregularsRepository';

@Injectable({
  providedIn: 'root'
})
export class TempIrregularService implements IIrregularsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }

  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<TemporalObjectForIrregular[]>(byTempIrregulars, { headers: he });
    observable.subscribe(returnedIrregulars=>{
      const action: Action={type:ActionType.GetAllTempIrregulars, payload:returnedIrregulars};
      this.redux.dispatch(action);
      this.logger.debug("Get All TempIrregulars: ", returnedIrregulars);
    }, error => {
      const action: Action={type:ActionType.GetAllTempIrregularsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All TempIrregulars Error: ", error.message);
    });
  }

  public PostWord(word:IrregularObject): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.post<TemporalObjectForIrregular>(byTempIrregulars, word, { headers: he });
    observable.subscribe(returnedIrregulars=>{
      const action: Action={type:ActionType.PostTempIrregular, payload:returnedIrregulars};
      this.redux.dispatch(action);
      this.logger.debug("Post TempIrregular: ", returnedIrregulars);
      alert("The " + word.first + "/" + word.second + "/" + word.third + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PostTempIrregularError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post TempIrregular Error: ", error.message);
      alert("Error: The " + word.first + "/" + word.second + "/" + word.third + " hasn't been submitted for review");
    });
  }

  public PutWord(word_to_replace:string, word:IrregularObject): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<TemporalObjectForIrregular>(byTempIrregulars+word_to_replace, word, { headers: he });
    observable.subscribe(returnedIrregulars=>{
      const action: Action={type:ActionType.PutTempIrregular, payload:returnedIrregulars};
      this.redux.dispatch(action);
      this.logger.debug("Put TempIrregular: ", returnedIrregulars);
      alert("The " + word.first + "/" + word.second + "/" + word.third + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PutTempIrregularError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put TempIrregular Error: ", error.message);
      alert("Error: The " + word.first + "/" + word.second + "/" + word.third + " hasn't been submitted for review");
    });
  }

  public DeleteWord(mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObjectForIrregular>(byTempIrregulars+mongoId, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete TempIrregular result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteTempIrregular, payload:mongoId };
        this.redux.dispatch(action);
        this.logger.debug("Delete TempIrregular: ", mongoId);
        const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
        this.redux.dispatch(action2);
        this.logger.debug("Delete Temporal Word: ", mongoId);
        alert("The request with id " + mongoId + " has been rejected");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteTempIrregularError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete TempIrregular Error: ", error.message);
    });
  }

  public DeleteCollection(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObjectForIrregular>(byTempIrregulars, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All TempIrregulars result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllTempIrregulars, payload:"Irregular" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All TempIrregulars: ", "Irregular");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllTempIrregularsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All TempIrregulars Error: ", error.message);
    });
  }
}