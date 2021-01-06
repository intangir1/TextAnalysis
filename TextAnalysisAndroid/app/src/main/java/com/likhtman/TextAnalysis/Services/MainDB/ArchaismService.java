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

public class ArchaismService extends Application {
    final String TAG = ArchaismService.this.getClass().getSimpleName();

    private CallbacksForNonRelationalWords callbacksForNonRelationalWords;
    private ProgressDialog progMsg;
    private Activity activity;
    private static ArchaismService instance = null;

    private ArchaismService(){

    }

    public static ArchaismService Get_instance(CallbacksForNonRelationalWords _callbacksForNonRelationalWords, Activity _activity){
        if(instance == null){
            instance = new ArchaismService();
        }
        instance.activity = _activity;
        instance.callbacksForNonRelationalWords = _callbacksForNonRelationalWords;
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
                    callbacksForNonRelationalWords.Return_array(result, 2);
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
        apiGetManager.execute(DataStore.byArchaism);
    }

}
