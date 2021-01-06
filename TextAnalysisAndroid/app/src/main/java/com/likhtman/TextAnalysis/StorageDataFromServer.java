package com.likhtman.TextAnalysis;


import android.app.Activity;
import android.app.FragmentManager;
import android.app.FragmentTransaction;
import android.content.Context;

import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;

import com.likhtman.TextAnalysis.Activities.MainActivity;
import com.likhtman.TextAnalysis.Callbacks.CallbacksOpenDefaultFragment;
import com.likhtman.TextAnalysis.Fragments.CheckTextFragment;
import com.likhtman.TextAnalysis.Model.Login;

import org.json.JSONException;
import org.json.JSONObject;

public class StorageDataFromServer {

    private final String default_picture_uri = "android.resource://" + BuildConfig.APPLICATION_ID + "/drawable/placeholder";
    private final String default_nickname = "incognito";

    private static StorageDataFromServer instance = null;
    private Login login = new Login(default_nickname, default_picture_uri, (short)0);
    private boolean was_updated = false;

    private String userToken = "";


    private StorageDataFromServer(){

    }


    public static StorageDataFromServer Get_instance(){
        if(instance == null){
            instance = new StorageDataFromServer();
        }
        return instance;
    }


    public void Clear_login(Activity activity){
        setLogin(new Login(default_nickname, default_picture_uri, (short)0));
        setWas_updated(false);
        userToken = "";

        ((CallbacksOpenDefaultFragment)activity).Open_default_fragment();

    }


    public Login getLogin() {
        return login;
    }

    public void setUserToken(String downloadedText){
        try {
            JSONObject jsonObject = new JSONObject(downloadedText);

            try {
                userToken = jsonObject.getString("userToken");
            } catch (JSONException e){
                e.printStackTrace();
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public String getUserToken(){
        return userToken;
    }

    public void setLogin(Login login) {
        this.login = login;
        setWas_updated(false);
    }

    public boolean isWas_updated() {
        return was_updated;
    }

    public void setWas_updated(boolean was_updated) {
        this.was_updated = was_updated;
    }
}
