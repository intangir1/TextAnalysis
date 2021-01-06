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
import com.likhtman.TextAnalysis.Model.IrregularObject;
import com.likhtman.TextAnalysis.Model.TemporalObjectForIrregular;
import com.likhtman.TextAnalysis.R;

public class TempIrregularService extends Application {
    final String TAG = TempIrregularService.this.getClass().getSimpleName();

    private CallbacksForTemporalObjects callbacksForTemporalObjects;
    private ProgressDialog progMsg;
    private Activity activity;
    private static TempIrregularService instance = null;

    private TempIrregularService(){

    }


    public static TempIrregularService Get_instance(CallbacksForTemporalObjects _callbacksForTemporalObjects, Activity _activity){
        if(instance == null){
            instance = new TempIrregularService();
        }
        instance.callbacksForTemporalObjects = _callbacksForTemporalObjects;
        instance.activity = _activity;
        return instance;
    }


    public void PostWord(IrregularObject word){
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

                TemporalObjectForIrregular temporalObjectForIrregular = new TemporalObjectForIrregular(downloadedText);
                callbacksForTemporalObjects.Return_irregular_object(temporalObjectForIrregular);
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
        postManager.execute(DataStore.byTempIrregulars, word.toJSON().toString());
    }


    public void PutWord(String word_to_replace, IrregularObject word){
        ApiPutManager putManager = new ApiPutManager(new CallbacksWebApi() {
            @Override
            public void onAboutToBegin() {
                Log.d(TAG, "PutWord - start");

                progMsg = new ProgressDialog(instance.activity);
                progMsg.setMessage(activity.getApplicationContext().getString(R.string.loading_message));
                progMsg.show();
            }

            @Override
            public void onSuccess(String downloadedText, int httpStatusCode) {
                Log.d(TAG, "PutWord - success");

                if (progMsg.isShowing())
                    progMsg.dismiss();

                TemporalObjectForIrregular temporalObjectForIrregular = new TemporalObjectForIrregular(downloadedText);
                callbacksForTemporalObjects.Return_irregular_object(temporalObjectForIrregular);

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
        putManager.execute(DataStore.byTempIrregulars + word_to_replace, word.toJSON().toString());
    }
}
