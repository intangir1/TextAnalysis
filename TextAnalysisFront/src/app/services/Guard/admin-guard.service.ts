import { Injectable } from '@angular/core';
import { NgRedux } from 'ng2-redux';
import { CanActivate, Router } from '@angular/router';
import { Store } from '../../redux/store';
import { LogService } from '.././log.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuardService  implements CanActivate {

  public constructor(private redux:NgRedux<Store>,
                     private router: Router,
                     private logger:LogService){}

  public canActivate(): boolean {
    if (this.redux.getState().loginUser.userLevel===4){
      this.logger.debug("admin ", "approved");
      return true;
    }
    
    this.router.navigate(["/home"]);
    this.logger.error("admin ", "not approved");
    return false;
  }
}