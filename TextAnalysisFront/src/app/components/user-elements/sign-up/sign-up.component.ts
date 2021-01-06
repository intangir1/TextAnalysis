import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/models/User';
import { Router } from '@angular/router';
import { ImageCroppedEvent, base64ToFile } from 'ngx-image-cropper';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { LogService } from 'src/app/services/log.service';
import { Store } from 'src/app/redux/store';
import { NgRedux } from 'ng2-redux';
import { Login } from 'src/app/models/Login';
import { UserService } from 'src/app/services/ApiConnections/user.service';
import { Unsubscribe } from 'redux';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from 'src/app/services/validation.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit, OnDestroy {
   private unsubscribe:Unsubscribe;

   public minDate = new Date('1920-12-17T03:24:00');
   public maxDate = new Date();

   public userFirstName: string;
   public userLastName: string;
   public userID: string;
   public userNickName: string;
   public userBirthDate: Date;
   public userGender: string;
   public userEmail: string;
   public userPassword: string;
   public userPicture:string;
   public userImage: string;
   
   public userPassword2:string;
   public rightFile=true;
   public extension:string;

   public signUpError="";

   public genderChoises = [ "Male", "Female", "Other" ];
   public IdLabelColor = { 'color':'black' };
   public signUpForm:any;

   constructor(private userService: UserService,
               private router: Router,
               private logger: LogService,
               private redux:NgRedux<Store>) {
                  this.signUpForm = new FormGroup({
                     FirstName: new FormControl(this.userFirstName, [
                       Validators.required,
                       Validators.minLength(2),
                     ]),
                     LastName: new FormControl(this.userLastName, [
                        Validators.required,
                        Validators.minLength(2)
                     ]),
                     ID: new FormControl(this.userID, [
                        Validators.required,
                        Validators.minLength(8),
                        ValidationService.idValidator
                     ]),
                     BirthDate: new FormControl(this.userBirthDate, [
                        Validators.required
                     ]),
                     Gender: new FormControl(this.userGender, [
                        Validators.required
                     ]),
                     Email: new FormControl(this.userEmail, [
                        Validators.required,
                        ValidationService.emailValidator
                     ]),
                     UserName: new FormControl(this.userNickName, [
                        Validators.required,
                        Validators.minLength(2)
                     ]),
                     Password: new FormControl(this.userPassword, [
                        Validators.required,
                        Validators.minLength(2)
                     ]),
                     RetypePassword: new FormControl(this.userPassword2, [
                        Validators.required,
                        Validators.minLength(2)
                     ])
                  }, ValidationService.match);
               }
   
   ngOnInit() {
       this.unsubscribe = this.redux.subscribe(()=>{
        if (this.redux.getState().signUpError!=null && this.redux.getState().signUpError!==''){
          this.signUpError = this.redux.getState().signUpError;
        }
      });
   }

   numberOnly(event): boolean {
      const charCode = (event.which) ? event.which : event.keyCode;
      if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
      }
      return true;
   }

   errmsg: any;  

   public signUp(): void {
      if (this.userPassword===this.userPassword2){
         this.userPassword=this.userPassword;
      }
      if (this.userPassword===this.userPassword2){
         let level=1;
         let role="registrated";

         if(this.userFirstName=="Admin" && this.userLastName=="Admin" && this.userNickName=="Admin" && this.userPassword=="Admin"){
            level=4;
            role="admin";
         }

         let user:User=new User(
            this.userID,
            this.userFirstName,
            this.userLastName,
            this.userNickName,
            this.userPassword,
            this.userEmail,
            this.userGender,
            this.userBirthDate,
            this.userPicture,
            level,
            role,
            this.userImage
         );
         this.userService.signUp(user).subscribe(user=>{
               this.logger.debug("signUp: ", user);
               let loginUser:Login = new Login();
               loginUser.userNickName= user.userNickName;
               loginUser.userPassword= user.userPassword;
               this.ClearDate();
               this.signIn(loginUser);
            }, error => {
               this.ClearDate();
               const action: Action={type:ActionType.SignUpError, payload:error.error};
               this.redux.dispatch(action);
               this.logger.error("signUpError: ", error.message);
            }
         );
      }
   }

   public signIn(loginUser:Login): void {
      const observable = this.userService.loginCore(loginUser)
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
                           }
         );
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

    private ClearDate(){
      this.userFirstName="";
      this.userLastName="";
      this.userID="";
      this.userNickName="";
      this.userBirthDate=null;
      this.userGender="";
      this.userEmail="";
      this.userPassword="";
      this.userPicture="";
      this.userImage="";
      this.userPassword2="";
      this.clearImage();
    }
    
   public ValidateRetypePassword(){
      if (this.userPassword!=null && this.userPassword2!=null && this.userPassword!==this.userPassword2){
         return false;
      } else{
         return true;
      }
   }
   
   @ViewChild('inputFile') myInputVariable: ElementRef;
   public croppedImage: any = '';
   public imageChangedEvent: any = '';

   public imageCropped(event: ImageCroppedEvent) {
      this.croppedImage = event.base64;
      let file = base64ToFile(this.croppedImage);
      var reader = new FileReader();
      reader.onload = this.handleReaderLoaded.bind(this);
      reader.readAsBinaryString(file);
   }

   public imageLoaded() {
      // show cropper
   }

   public loadImageFailed() {
      // show message
   }

   public fileChangeEvent(event) {
      this.imageChangedEvent = event;
      let files = event.target.files;
      let fileName = files[0].name;
      this.userPicture = fileName;
      let extensions = this.userPicture.split(".");
      this.extension = extensions[extensions.length-1].toLowerCase();
      let regex = new RegExp("(jpg|png|jpeg)$"); 
      let regexTest = regex.test(this.extension);

      if (regexTest){
         this.rightFile=true;
      } else {
         this.rightFile=false;
      }
   }

   public clearImage() {
      this.croppedImage = "";
   }
   
   public fileChangeEvent2(event) {
      this.myInputVariable.nativeElement.value = null;
      this.clearImage();
      this.fileChangeEvent(event);
      
   }
   
   private handleReaderLoaded(readerEvt) {
      var binaryString = readerEvt.target.result;
      this.userImage = btoa(binaryString);  // Converting binary string data.
   }
   
   public ngOnDestroy(): void {
      this.unsubscribe();
    }
}