import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { byAntonims } from 'src/environments/environment';
import { IRelationalWordsRepository } from 'src/app/repository/baseInterfaces/IRelationalWordsRepository';
import { FullText } from 'src/app/models/FullText';

@Injectable({
  providedIn: 'root'
})
export class AntonimService implements IRelationalWordsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }
  
  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<Array<Array<string>>>(byAntonims, { headers: he });
    observable.subscribe(returnedAntonims=>{
      const action: Action={type:ActionType.GetAllAntonims, payload:returnedAntonims};
      this.redux.dispatch(action);
      this.logger.debug("Get All Antonims: ", returnedAntonims);
    }, error => {
      const action: Action={type:ActionType.GetAllAntonimsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All Antonims Error: ", error.message);
    });
  }

  public GetWordsBy(word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<string[]>(byAntonims+word, { headers: he });
    observable.subscribe(returnedAntonims=>{
      if (returnedAntonims==null){
        const action: Action={type:ActionType.DeleteAllAntonims, payload:"Antonim" };
        this.redux.dispatch(action);
      } else{
        const action: Action={type:ActionType.GetAntonim, payload:returnedAntonims};
        this.redux.dispatch(action);
      }
      this.logger.debug("Get Antonim: ", returnedAntonims);
    }, error => {
      const action: Action={type:ActionType.GetAntonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get Antonim Error: ", error.message);
    });
  }

  public PostWord(synonimWord:string, antonimWord:string, mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.post<string[]>(byAntonims+synonimWord, new FullText(antonimWord), { headers: he });
    observable.subscribe(returnedAntonims=>{
      const action: Action={type:ActionType.PostAntonim, payload:returnedAntonims};
      this.redux.dispatch(action);
      this.logger.debug("Post Antonim: ", returnedAntonims);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + antonimWord + " has been posted");
    }, error => {
      const action: Action={type:ActionType.PostAntonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post Antonim Error: ", error.message);
      alert("Error: The " + antonimWord + " hasn't been posted");
    });
  }

  public PutWord(word_to_replace:string, word:string, mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<string[]>(byAntonims+word_to_replace, new FullText(word), { headers: he });
    observable.subscribe(returnedAntonims=>{
      const action: Action={type:ActionType.PutAntonim, payload:returnedAntonims};
      this.redux.dispatch(action);
      this.logger.debug("Put Antonim: ", returnedAntonims);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word + " has been put");
    }, error => {
      const action: Action={type:ActionType.PutAntonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put Antonim Error: ", error.message);
      alert("Error: The " + word + " hasn't been put");
    });
  }

  public InsertWord(word_to_find:string, word_to_add:string, mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<string[]>(byAntonims+"insert/"+word_to_find, new FullText(word_to_add), { headers: he });
    observable.subscribe(returnedAntonims=>{
      const action: Action={type:ActionType.InsertAntonim, payload:returnedAntonims};
      this.redux.dispatch(action);
      this.logger.debug("Insert Antonim: ", returnedAntonims);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word_to_add + " has been inserted");
    }, error => {
      const action: Action={type:ActionType.InsertAntonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Insert Antonim Error: ", error.message);
      alert("Error: The " + word_to_add + " hasn't been inserted");
    });
  }

  public DeleteWord(wordToRemove:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<string[]>(byAntonims+wordToRemove, { headers: he });
    observable.subscribe(returnedAntonims=>{
      const action: Action={type:ActionType.DeleteAntonim, payload:returnedAntonims};
      this.redux.dispatch(action);
      this.logger.debug("Delete Antonim: ", returnedAntonims);
    }, error => {
      const action: Action={type:ActionType.DeleteAntonimError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete Antonim Error: ", error.message);
    });
  }

  public DeleteCollection(word:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<string>(byAntonims+"delete_collection/"+word, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All Antonims result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllAntonims, payload:"Antonim" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All Antonims: ", "Antonim");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllAntonimsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All Antonims Error: ", error.message);
    });
  }
}