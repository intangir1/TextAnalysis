import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Action } from '../../redux/action';
import { ActionType } from '../../redux/action-type';
import { Store } from '../../redux/store';
import { NgRedux } from 'ng2-redux';
import { User } from '../../models/User';
import { LogService } from '.././log.service';
import { usersAnaliticsUrl } from 'src/environments/environment';
import { IUserStatisticsRepository } from 'src/app/repository/IUserStatisticsRepository';

@Injectable({
  providedIn: 'root'
})
export class UserStatisticsService implements IUserStatisticsRepository {

  public constructor(private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { 
  }

  public GetAllUserAnalitics() {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<User[]>(usersAnaliticsUrl, { headers: he });
    observable.subscribe(users=>{
      const action: Action={type:ActionType.GetAllUserAnalitics, payload:users};
      this.redux.dispatch(action);
      this.logger.debug("GetAllUserAnalitics: ", users);
    }, error => {
      const action: Action={type:ActionType.GetAllUserAnaliticsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("GetAllUserAnaliticsError: ", error.message);
    });
  }

  public GetUserAnaliticsByDates(startDate: Date, endDate: Date) {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<User[]>(usersAnaliticsUrl+startDate+'/'+endDate, { headers: he });
    observable.subscribe(users=>{
      const action: Action={type:ActionType.GetAllUserAnalitics, payload:users};
      this.redux.dispatch(action);
      this.logger.debug("GetUserAnaliticsByDates: ", users);
    }, error => {
      const action: Action={type:ActionType.GetAllUserAnaliticsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("GetUserAnaliticsByDatesError: ", error.message);
    });
  }

  public GetUserAnaliticsByStart(startDate: Date) {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<User[]>(usersAnaliticsUrl+"start/"+startDate.toISOString(), { headers: he });
    observable.subscribe(users=>{
      const action: Action={type:ActionType.GetAllUserAnalitics, payload:users};
      this.redux.dispatch(action);
      this.logger.debug("GetUserAnaliticsByStart: ", users);
    }, error => {
      const action: Action={type:ActionType.GetAllUserAnaliticsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("GetUserAnaliticsByStartError: ", error.message);
    });
  }

  public GetUserAnaliticsByEnd(endDate: Date) {
    let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
    let observable = this.http.get<User[]>(usersAnaliticsUrl+"end/"+endDate.toISOString(), { headers: he });
    observable.subscribe(users=>{
      const action: Action={type:ActionType.GetAllUserAnalitics, payload:users};
      this.redux.dispatch(action);
      this.logger.debug("GetUserAnaliticsByEnd: ", users);
    }, error => {
      const action: Action={type:ActionType.GetAllUserAnaliticsError, payload:error.message};
      this.redux.dispatch(action);
      this.logger.error("GetUserAnaliticsByEndError: ", error.message);
    });
  }
}