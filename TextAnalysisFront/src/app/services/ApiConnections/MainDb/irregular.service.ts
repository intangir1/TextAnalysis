import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { LogService } from '../../log.service';
import { Store } from 'src/app/redux/store';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { byIrregulars } from 'src/environments/environment';
import { IrregularObject } from 'src/app/models/IrregularObject';
import { IIrregularsRepository } from 'src/app/repository/extendedInterfaces/IIrregularsRepository';

@Injectable({
  providedIn: 'root'
})
export class IrregularService implements IIrregularsRepository{

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }

  public GetAllWords(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<IrregularObject[]>(byIrregulars, { headers: he });
    observable.subscribe(returnedIrregulars=>{
      const action: Action={type:ActionType.GetAllIrregulars, payload:returnedIrregulars};
      this.redux.dispatch(action);
      this.logger.debug("Get All Irregulars: ", returnedIrregulars);
    }, error => {
      const action: Action={type:ActionType.GetAllIrregularsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Get All Irregulars Error: ", error.message);
    });
  }

  public PostWord(word:IrregularObject, mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.post<IrregularObject>(byIrregulars, word, { headers: he });
    observable.subscribe(returnedIrregulars=>{
      const action: Action={type:ActionType.PostIrregular, payload:returnedIrregulars};
      this.redux.dispatch(action);
      this.logger.debug("Post Irregular: ", returnedIrregulars);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word.first + "/" + word.second + "/" + word.third + " has been posted");
    }, error => {
      const action: Action={type:ActionType.PostIrregularError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Post Irregular Error: ", error.message);
      alert("Error: The " + word.first + "/" + word.second + "/" + word.third + " hasn't been posted");
    });
  }

  public PutWord(word_to_replace:string, word:IrregularObject, mongoId:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.put<IrregularObject>(byIrregulars+word_to_replace, word, { headers: he });
    observable.subscribe(returnedIrregulars=>{
      const action: Action={type:ActionType.PutIrregular, payload:returnedIrregulars};
      this.redux.dispatch(action);
      this.logger.debug("Put Irregular: ", returnedIrregulars);

      const action2: Action={type:ActionType.DeleteTemporalWord, payload:mongoId};
      this.redux.dispatch(action2);
      this.logger.debug("Delete Temporal Word: ", mongoId);
      alert("The " + word.first + "/" + word.second + "/" + word.third + " has been put");
    }, error => {
      const action: Action={type:ActionType.PutIrregularError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Put Irregular Error: ", error.message);
      alert("Error: The " + word.first + "/" + word.second + "/" + word.third + " hasn't been put");
    });
  }

  public DeleteWord(wordToRemove:string): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<IrregularObject>(byIrregulars+wordToRemove, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete Irregular result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteIrregular, payload:wordToRemove };
        this.redux.dispatch(action);
        this.logger.debug("Delete Irregular: ", wordToRemove);
      }
    }, error => {
      const action: Action={type:ActionType.DeleteIrregularError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete Irregular Error: ", error.message);
    });
  }

  public DeleteCollection(): void {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.delete<IrregularObject>(byIrregulars, { observe: 'response', headers: he});
    observable.subscribe(res=>{
      this.logger.debug("Delete All Irregulars result status: ", res.status);
      if (res.status===204){
        const action: Action={type:ActionType.DeleteAllIrregulars, payload:"Irregular" };
        this.redux.dispatch(action);
        this.logger.debug("Delete All Irregulars: ", "Irregular");
      }
    }, error => {
      const action: Action={type:ActionType.DeleteAllIrregularsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("Delete All Irregulars Error: ", error.message);
    });
  }
}