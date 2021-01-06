package com.likhtman.TextAnalysis.Services.MainDB;

import android.app.Activity;
import android.app.Application;
import android.app.ProgressDialog;
import android.util.Log;

import com.likhtman.TextAnalysis.Callbacks.CallbacksForNonRelationalWords;
import com.likhtman.TextAnalysis.Callbacks.CallbacksWebApi;
import com.likhtman.TextAnalysis.DataStore;
import com.likhtman.TextAnalysis.DialogMessage;
import com.likhtman.TextAnalysis.Managers.HttpClient.ApiGetManager;
import com.likhtman.TextAnalysis.R;

import org.json.JSONArray;

import java.util.ArrayList;

public class SlangService extends Application {
    final String TAG = SlangService.this.getClass().getSimpleName();

    private CallbacksForNonRelationalWords callbacksForNonRelationalWords;
    private Activity activity;
    private ProgressDialog progMsg;
    private static SlangService instance = null;

    private SlangService(){

    }

    public static SlangService Get_instance(CallbacksForNonRelationalWords _callbacksForNonRelationalWords, Activity _activity){
        if(instance == null){
            instance = new SlangService();
        }
        instance.callbacksForNonRelationalWords = _callbacksForNonRelationalWords;
        instance.activity = _activity;
        return instance;
    }


    public void GetAllWords(){
        ApiGetManager apiGetManager = new ApiGetManager(new CallbacksWebApi() {
            @Override
            public void onAboutToBegin() {
                Log.d(TAG, "GetAllWords - start");

                progMsg = new ProgressDialog(instance.activity);
                progMsg.setMessage(activity.getApplicationContext().getString(R.string.loading_message));
                progMsg.show();
            }

            @Override
            public void onSuccess(String downloadedText, int httpStatusCode) {
                Log.d(TAG, "GetAllWords - success");

                if (progMsg.isShowing())
                    progMsg.dismiss();

                ArrayList<String> result = new ArrayList<String>();

                JSONArray jArray = null;
                try{
                    jArray = new JSONArray(downloadedText);
                    for(int i = 0; i < jArray.length(); i++){
                        String word = jArray.getString(i);
                        result.add(word);
                    }
                    callbacksForNonRelationalWords.Return_array(result, 1);
                } catch (Exception e){
                    e.printStackTrace();
                }
            }


            @Override
            public void onError(int httpStatusCode, String errorMessage) {
                Log.e(TAG, "GetAllWords - error: " + errorMessage);

                if (progMsg.isShowing())
                    progMsg.dismiss();

                DialogMessage.Get_instance(8, activity).Show_dialog();
            }
        });
        apiGetManager.execute(DataStore.bySlangs);
    }
}
