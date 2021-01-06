import { Component, OnInit, OnDestroy } from '@angular/core';
import { Login } from 'src/app/models/Login';
import { Store } from 'src/app/redux/store';
import { NgRedux } from 'ng2-redux';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { Unsubscribe } from 'redux';
import { LogService } from 'src/app/services/log.service';
import { UserService } from 'src/app/services/ApiConnections/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit, OnDestroy {
  public login:Login = new Login();
  public loginError="";
  private unsubscribe:Unsubscribe;
  
  constructor(private router: Router,
              private userService: UserService,
              private logger: LogService,
              private redux:NgRedux<Store>) { }
  
  ngOnInit() {
    this.unsubscribe = this.redux.subscribe(()=>{
      if (this.redux.getState().loginUser)
      {
        this.login = this.redux.getState().loginUser;
      }
      if (this.redux.getState().loginError!=null && this.redux.getState().loginError!==''){
        this.loginError = this.redux.getState().loginError;
        const action: Action={type:ActionType.LoginError, payload:""};
        this.redux.dispatch(action);
      }
    });
  }
  
  errmsg: any;  

  public signIn(): void {
    const observable = this.userService.loginCore(this.login)
    observable.subscribe(res => {
      this.logger.debug("LoggedUser: ", res);
      sessionStorage.setItem('access_token', res.userToken);
      let loginUser:Login = new Login(
        res.userNickName,
        "",
        res.userLevel,
        res.userPicture
      );
      const action: Action={type:ActionType.UserLogin, payload:loginUser};
      this.redux.dispatch(action);
      this.sleep().then(() => {
        this.logout();
      });    
                    }, error => {
      this.logger.error('Invalid username or password.', error.message); 
      const action: Action={type:ActionType.LoginError, payload:error.message};
      this.redux.dispatch(action); 
    }); 
  }

  public sleep() {
    let promise = new Promise(resolve => setTimeout(resolve, this.redux.getState().delayTimeForSignOut));
    return promise;
  }


  public logout(): void {
    const action: Action={type:ActionType.UserLogOut};
    this.redux.dispatch(action);
    alert("The time has passed, so please sign in one more time");
    this.router.navigate(["/home"]);
  }

  public ngOnDestroy(): void {
    this.unsubscribe();
  }
}