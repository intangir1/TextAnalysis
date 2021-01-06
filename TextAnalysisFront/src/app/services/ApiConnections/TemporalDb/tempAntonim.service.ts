import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { byTempAntonims } from 'src/environments/environment';
import { TemporalObject } from 'src/app/models/TemporalObject';
import { IRelationalWordsRepository } from 'src/app/repository/baseInterfaces/IRelationalWordsRepository';
import { FullText } from 'src/app/models/FullText';

@Injectable({
  providedIn: 'root'
})
export class TempAntonimService implements IRelationalWordsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }
  GetWordsBy(word: string): void {
    throw new Error("Method not implemented.");
  }
  
  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<TemporalObject[]>(byTempAntonims, { headers: he });
    observable.subscribe(returnedAntonims=>{
      const action: Action={type:ActionType.GetAllTempAntonims, payload:returnedAntonims};
      this.redux.dispatch(action);
      this.logger.debug("Get All TempAntonims: ", returnedAntonims);
    }, error => {
      const action: Action={type:ActionType.GetAllTempAntonimsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All TempAntonims Error: ", error.message);
    });
  }

  public PostWord(synonimWord:string, antonimWord:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.post<TemporalObject>(byTempAntonims+synonimWord, new FullText(antonimWord), { headers: he });
    observable.subscribe(returnedAntonims=>{
      const action: Action={type:ActionType.PostTempAntonim, payload:returnedAntonims};
      this.redux.dispatch(action);
      this.logger.debug("Post TempAntonim: ", returnedAntonims);
      alert("The " + antonimWord + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PostTempAntonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post TempAntonim Error: ", error.message);
      alert("Error: The " + antonimWord + " hasn't been submitted for review");
    });
  }

  public PutWord(word_to_replace:string, word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<TemporalObject>(byTempAntonims+word_to_replace, new FullText(word), { headers: he });
    observable.subscribe(returnedAntonims=>{
      const action: Action={type:ActionType.PutTempAntonim, payload:returnedAntonims};
      this.redux.dispatch(action);
      this.logger.debug("Put TempAntonim: ", returnedAntonims);
      alert("The " + word + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.PutTempAntonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put TempAntonim Error: ", error.message);
      alert("Error: The " + word + " hasn't been submitted for review");
    });
  }

  public InsertWord(word_to_find:string, word_to_add:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<TemporalObject>(byTempAntonims+"insert/"+word_to_find, new FullText(word_to_add), { headers: he });
    observable.subscribe(returnedAntonims=>{
      const action: Action={type:ActionType.InsertTempAntonim, payload:returnedAntonims};
      this.redux.dispatch(action);
      this.logger.debug("Insert TempAntonim: ", returnedAntonims);
      alert("The " + word_to_add + " has been submitted for review");
    }, error => {
      const action: Action={type:ActionType.InsertTempAntonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Insert TempAntonim Error: ", error.message);
      alert("Error: The " + word_to_add + " hasn't been submitted for review");
    });
  }

  public DeleteWord(mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObject>(byTempAntonims+mongoId, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete TempAntonim result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteTempAntonim, payload:mongoId };
        this.redux.dispatch(action);
        this.logger.debug("Delete TempAntonim: ", mongoId);
        const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
        alert("The request with id " + mongoId + " has been rejected");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteTempAntonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete TempAntonim Error: ", error.message);
    });
  }

  public DeleteCollection(word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<TemporalObject>(byTempAntonims+word, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All TempAntonims result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllTempAntonims, payload:"Antonim" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All TempAntonims: ", "Antonim");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllTempAntonimsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All TempAntonims Error: ", error.message);
    });
  }
}