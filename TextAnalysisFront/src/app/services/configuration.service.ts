import { Injectable } from '@angular/core';
import { NgRedux } from 'ng2-redux';
import { Store } from '../redux/store';
import { LogService } from './log.service';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {

  public constructor(private redux:NgRedux<Store>,
    private logger:LogService){}

  public IsAdmin(): boolean {
    if (this.redux.getState().loginUser && this.redux.getState().loginUser.userLevel===4){
      this.logger.debug("IsAdmin ", "true");
      return true;
    }
    this.logger.error("IsAdmin ", "false");
    return false;
  }
}
