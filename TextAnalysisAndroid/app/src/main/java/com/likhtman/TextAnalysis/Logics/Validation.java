package com.likhtman.TextAnalysis.Logics;

import android.util.Log;

public class Validation {
    final String TAG = Validation.this.getClass().getSimpleName();

    private static Validation instance = null;

    private Validation(){

    }


    public static Validation Get_instance(){
        if(instance == null){
            instance = new Validation();
        }
        return instance;
    }



    public Boolean Email_is_valid(String input){
        String email_pattern = "[a-zA-Z0-9._-]+@[a-z]+\\.+[a-z]+";

        if(input.matches(email_pattern)){
            Log.d(TAG, "Email validated");
            return true;
        } else{
            Log.d(TAG, "Email not validated");
            return false;
        }
    }


    public Boolean Id_is_valid(String input){
        String regex = "^\\d{5,9}$";
        if(!input.matches(regex)){
            return false;
        }

        while (input.length() < 9){
            input = '0' + input;
        }

        int mone = 0;
        int incNum;
        for (int i = 0; i < 9; i++) {
            incNum = input.charAt(i) - '0';
            incNum *= (i % 2) + 1;
            if (incNum > 9)
                incNum -= 9;
            mone += incNum;
        }
        if (mone % 10 == 0)
        {
            Log.d(TAG, "ID validated");
            return true;
        }

        else
        {
            Log.d(TAG, "ID not validated");
            return false;
        }
    }


}
