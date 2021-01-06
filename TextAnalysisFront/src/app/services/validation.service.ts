import { Injectable } from '@angular/core';
import { FormGroup, ValidationErrors } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ValidationService {
  static getValidatorErrorMessage(validatorName: string, validatorValue?: any) {
    let config = {
      required: 'Required',
      minlength: `Minimum length ${validatorValue.requiredLength}`,
      idValidator: 'Is invalid Id number',
      emailValidator: 'Invalid email address',
      match: 'Invalid password. Password must be at least 6 characters long, and contain a number.'
    };
    return config[validatorName];
  }

  static idValidator(control) {
    let IDnum = control.value;
    if (IDnum==null || IDnum==='' || IDnum.length < 8){
      return null;
    }
   
    // Validate correct input
    if (IDnum.length > 9){
      return { invalidId: true }; 
    }
    if (isNaN(IDnum) ){
      return { invalidId: true };
    }
   
    // The number is too short - add leading 0000
    if (IDnum.length < 9)
    {
       while(IDnum.length < 9)
       {
          IDnum = '0' + IDnum;         
       }
    }

    // CHECK THE ID NUMBER
    var mone = 0, incNum;
    for (var i=0; i < 9; i++)
    {
       incNum = Number(IDnum.charAt(i));
       incNum *= (i%2)+1;
       if (incNum > 9)
          incNum -= 9;
       mone += incNum;
    }
    if (mone%10 == 0)
       return null;
    else
       return { invalidId: true };
  }

  static match(control: FormGroup): ValidationErrors | null {
    let password = control.get('Password').value;
    let retypePassword = control.get('RetypePassword').value;
    if(password!=null && password!=undefined && password.length>1 && retypePassword!=null && retypePassword!=undefined && retypePassword.length>1){
      if (password !== retypePassword) {
        control.get('Password').setErrors({'mustMatch': true});
        control.get('RetypePassword').setErrors({'mustMatch': true});
        return { mustMatch: true };
      } else {
        control.get('Password').setErrors(null);
        control.get('RetypePassword').setErrors(null);
        // control.setErrors({'mustMatch': null});
        control.setErrors(null);
        return null;
      }
    } else {
      //control.get('Password').setErrors(null);
      //control.get('RetypePassword').setErrors(null);
      //control.setErrors({'mustMatch': null});
      //control.setErrors(null);
      return null;
    }
  }

  static emailValidator(control) {
    if(control!=null && control.value!=null && control.value.length>0){
      // RFC 2822 compliant regex
      if (control.value.match(/[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/)) {
        return null;
      } else {
        return { invalidEmailAddress: true };
      }
    } else {
      return null;
    }
  }

  
}
