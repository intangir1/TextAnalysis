package com.likhtman.TextAnalysis.Services.TemporalDB;

import android.app.Activity;
import android.app.Application;
import android.app.ProgressDialog;
import android.util.Log;

import com.likhtman.TextAnalysis.Callbacks.CallbacksForTemporalObjects;
import com.likhtman.TextAnalysis.Callbacks.CallbacksWebApi;
import com.likhtman.TextAnalysis.DataStore;
import com.likhtman.TextAnalysis.DialogMessage;
import com.likhtman.TextAnalysis.Managers.HttpClient.ApiPostManager;
import com.likhtman.TextAnalysis.Managers.HttpClient.ApiPutManager;
import com.likhtman.TextAnalysis.Model.FullText;
import com.likhtman.TextAnalysis.Model.TemporalObject;
import com.likhtman.TextAnalysis.R;

public class TempExpressionService  extends Application {
    final String TAG = TempExpressionService.this.getClass().getSimpleName();

    private CallbacksForTemporalObjects callbacksForTemporalObjects;
    private ProgressDialog progMsg;
    private Activity activity;
    private static TempExpressionService instance = null;

    private TempExpressionService(){

    }


    public static TempExpressionService Get_instance(CallbacksForTemporalObjects _callbacksForTemporalObjects, Activity _activity){
        if(instance == null){
            instance = new TempExpressionService();
        }
        instance.callbacksForTemporalObjects = _callbacksForTemporalObjects;
        instance.activity = _activity;
        return instance;
    }


    public void PostWord(FullText word){
        ApiPostManager postManager = new ApiPostManager(new CallbacksWebApi() {
            @Override
            public void onAboutToBegin() {
                Log.d(TAG, "PostWord - start");

                progMsg = new ProgressDialog(instance.activity);
                progMsg.setMessage(activity.getApplicationContext().getString(R.string.loading_message));
                progMsg.show();
            }

            @Override
            public void onSuccess(String downloadedText, int httpStatusCode) {
                Log.d(TAG, "PostWord - success");

                if (progMsg.isShowing())
                    progMsg.dismiss();

                TemporalObject temporalObject = new TemporalObject(downloadedText);
                callbacksForTemporalObjects.Return_normal_object(temporalObject);

            }

            @Override
            public void onError(int httpStatusCode, String errorMessage) {
                Log.e(TAG, "PostWord - error: " + errorMessage);

                if (progMsg.isShowing())
                    progMsg.dismiss();

                if(httpStatusCode > 0){
                    DialogMessage.Get_instance(7, activity).Show_dialog();
                } else{
                    DialogMessage.Get_instance(5, activity).Show_dialog();
                }
            }
        });
        postManager.execute(DataStore.byTempExpressions, word.toJSON().toString());
    }


    public void PutWord(String word_to_replace, FullText word){
        ApiPutManager putManager = new ApiPutManager(new CallbacksWebApi() {
            @Override
            public void onAboutToBegin() {
                Log.d(TAG, "PutWord - start");

                progMsg = new ProgressDialog(getApplicationContext());
                progMsg.setMessage(activity.getApplicationContext().getString(R.string.loading_message));
                progMsg.show();
            }

            @Override
            public void onSuccess(String downloadedText, int httpStatusCode) {
                Log.d(TAG, "PutWord - success");

                if (progMsg.isShowing())
                    progMsg.dismiss();

                TemporalObject temporalObject = new TemporalObject(downloadedText);
                callbacksForTemporalObjects.Return_normal_object(temporalObject);

            }

            @Override
            public void onError(int httpStatusCode, String errorMessage) {
                Log.e(TAG, "PutWord - error: " + errorMessage);

                if (progMsg.isShowing())
                    progMsg.dismiss();

                if(httpStatusCode > 0){
                    DialogMessage.Get_instance(7, activity).Show_dialog();
                } else{
                    DialogMessage.Get_instance(5, activity).Show_dialog();
                }
            }
        });
        putManager.execute(DataStore.byTempExpressions + word_to_replace, word.toJSON().toString());
    }
}
