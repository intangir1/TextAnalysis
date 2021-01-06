import { Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { MyNavigator } from 'src/app/models/MyNavigator';
import { Store } from 'src/app/redux/store';
import { NgRedux } from 'ng2-redux';
import { Router } from '@angular/router';
import { Unsubscribe } from 'redux';
import { Login } from 'src/app/models/Login';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { baseUrl } from 'src/environments/environment';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {
  private unsubscribe:Unsubscribe;
  public loginUser:Login;
  public isLoggedIn:boolean;
  public myLevel=0;
  public mUrl="";

  @ViewChild('signUpModal') private signUpModal: ElementRef;
  @ViewChild('signInModal') private signInModal: ElementRef;

  @ViewChild('signUp') private signUp: ElementRef;
  @ViewChild('signIn') private signIn: ElementRef;
  
  
  constructor(private router: Router,
              private redux:NgRedux<Store>) { }

  public searchByWord:string;

  public ngOnInit() {
    this.unsubscribe = this.redux.subscribe(()=>{
      this.loginUser = this.redux.getState().loginUser;
      this.isLoggedIn = this.redux.getState().isLoggedIn;
      if (this.isLoggedIn){
        this.signUpModal.nativeElement.click();
        this.signInModal.nativeElement.click();
        this.myLevel=this.loginUser.userLevel;
        this.mUrl=baseUrl;
      } else {
        this.myLevel=0;
        this.mUrl="";
      }
      if (this.redux.getState().needSignIn){
        this.signIn.nativeElement.click();
        const action: Action={type:ActionType.NeedSignIn, payload:false};
        this.redux.dispatch(action);
      }
    });
  }

  public logout(): void {
    const action: Action={type:ActionType.UserLogOut};
    this.redux.dispatch(action);
    this.myLevel=0;
    this.router.navigate(["/home"]);
  }
  
  navigators = [
    new MyNavigator("/home", 'Home'),
    new MyNavigator("/add_words", 'Add Words'),
    new MyNavigator("/add_words_admin", 'Add Words Admin'),
    new MyNavigator("/user_statistics", 'User Statistics'),
    new MyNavigator("", 'SignUp'),
    new MyNavigator("", 'SignIn'),
    new MyNavigator("", 'SignOut')
  ];
  
  public ngOnDestroy(): void {
    this.unsubscribe();
  }

}