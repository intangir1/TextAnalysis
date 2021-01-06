import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { bySynonim } from 'src/environments/environment';
import { IRelationalWordsRepository } from 'src/app/repository/baseInterfaces/IRelationalWordsRepository';
import { FullText } from 'src/app/models/FullText';

@Injectable({
  providedIn: 'root'
})
export class SynonimService implements IRelationalWordsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }

  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<Array<Array<string>>>(bySynonim, { headers: he });
    observable.subscribe(returnedSynonims=>{
      const action: Action={type:ActionType.GetAllSynonims, payload:returnedSynonims};
      this.redux.dispatch(action);
      this.logger.debug("Get All Synonims: ", returnedSynonims);
    }, error => {
      const action: Action={type:ActionType.GetAllSynonimsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All Synonims Error: ", error.message);
    });
  }

  public GetWordsBy(word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<string[]>(bySynonim+word, { headers: he });
    observable.subscribe(returnedSynonims=>{
      if (returnedSynonims==null){
        const action: Action={type:ActionType.DeleteAllSynonims, payload:"Synonim" };
        this.redux.dispatch(action);
      } else{
        const action: Action={type:ActionType.GetSynonim, payload:returnedSynonims};
        this.redux.dispatch(action);
      }
      this.logger.debug("Get Synonim: ", returnedSynonims);
    }, error => {
      const action: Action={type:ActionType.GetSynonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get Synonim Error: ", error.message);
    });
  }

  public PostWord(word:string, mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.post<string[]>(bySynonim, new FullText(word), { headers: he });
    observable.subscribe(returnedSynonims=>{
      const action: Action={type:ActionType.PostSynonim, payload:returnedSynonims};
      this.redux.dispatch(action);
      this.logger.debug("Post Synonim: ", returnedSynonims);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word + " has been posted");
    }, error => {
      const action: Action={type:ActionType.PostSynonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post Synonim Error: ", error.message);
      alert("Error: The " + word + " hasn't been posted");
    });
  }

  public PutWord(word_to_replace:string, word:string, mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<string[]>(bySynonim+word_to_replace, new FullText(word), { headers: he });
    observable.subscribe(returnedSynonims=>{
      const action: Action={type:ActionType.PutSynonim, payload:returnedSynonims};
      this.redux.dispatch(action);
      this.logger.debug("Put Synonim: ", returnedSynonims);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word + " has been put");
    }, error => {
      const action: Action={type:ActionType.PutSynonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put Synonim Error: ", error.message);
      alert("Error: The " + word + " hasn't been put");
    });
  }

  public InsertWord(word_to_find:string, word_to_add:string, mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<string[]>(bySynonim+"insert/"+word_to_find, new FullText(word_to_add), { headers: he });
    observable.subscribe(returnedSynonims=>{
      const action: Action={type:ActionType.InsertSynonim, payload:returnedSynonims};
      this.redux.dispatch(action);
      this.logger.debug("Insert Synonim: ", returnedSynonims);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word_to_add + " has been inserted");
    }, error => {
      const action: Action={type:ActionType.InsertSynonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Insert Synonim Error: ", error.message);
      alert("Error: The " + word_to_add + " hasn't been inserted");
    });
  }

  public DeleteWord(wordToRemove:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<string[]>(bySynonim+wordToRemove, { headers: he });
    observable.subscribe(returnedSynonims=>{
      const action: Action={type:ActionType.DeleteSynonim, payload:returnedSynonims};
      this.redux.dispatch(action);
      this.logger.debug("Delete Synonim: ", returnedSynonims);
    }, error => {
      const action: Action={type:ActionType.DeleteSynonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete Synonim Error: ", error.message);
    });
  }

  public DeleteCollection(word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<string>(bySynonim+"delete_collection/"+word, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All Synonims result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllSynonims, payload:"Synonim" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All Synonims: ", "Synonim");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllSynonimsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All Synonims Error: ", error.message);
    });
  }
}