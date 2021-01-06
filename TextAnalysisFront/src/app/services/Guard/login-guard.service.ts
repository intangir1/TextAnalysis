import { Injectable } from '@angular/core';
import { NgRedux } from 'ng2-redux';
import { CanActivate, Router } from '@angular/router';
import { Store } from '../../redux/store';
import { Action } from '../../redux/action';
import { ActionType } from '../../redux/action-type';
import { LogService } from '.././log.service';

@Injectable({
  providedIn: 'root'
})
export class LoginGuardService implements CanActivate {

  public constructor(private redux:NgRedux<Store>,
                     private router: Router,
                     private logger:LogService){}

  public canActivate(): boolean {
    if (this.redux.getState().isLoggedIn){
      this.logger.debug("user ", "approved");
        return true;
    }
    // const action: Action={type:ActionType.SetIsLoggedIn, payload:false};
    // this.redux.dispatch(action);
    this.router.navigate(["/home"]);
    this.logger.error("user ", "not approved");
    return false;
  }
}