package com.likhtman.TextAnalysis.Services;

import android.app.Activity;
import android.app.Application;
import android.app.ProgressDialog;
import android.util.Log;

import com.likhtman.TextAnalysis.Callbacks.CallbacksTextReturn;
import com.likhtman.TextAnalysis.Callbacks.CallbacksWebApi;
import com.likhtman.TextAnalysis.DataStore;
import com.likhtman.TextAnalysis.Logics.TextSplitter;
import com.likhtman.TextAnalysis.Managers.HttpClient.ApiPostManager;
import com.likhtman.TextAnalysis.Model.AnalysedText;
import com.likhtman.TextAnalysis.Model.FullText;
import com.likhtman.TextAnalysis.R;

import org.json.JSONArray;

import java.util.Arrays;

public class TextService extends Application {
    final String TAG = TextService.this.getClass().getSimpleName();

    private CallbacksTextReturn callbacksTextReturn;
    private ProgressDialog progMsg;
    private Activity activity;
    private static TextService instance = null;

    private TextService(){

    }


    public static TextService Get_instance(CallbacksTextReturn _callbacksTextReturn, Activity _activity){
        if(instance == null){
            instance = new TextService();
        }
        instance.callbacksTextReturn = _callbacksTextReturn;
        instance.activity = _activity;
        return instance;
    }





    public void ByWords(FullText text, int limit){
        ApiPostManager apiPostManager = new ApiPostManager(new CallbacksWebApi() {
            @Override
            public void onAboutToBegin() {
                Log.d(TAG, "ByWords - start");

                progMsg = new ProgressDialog(instance.activity);
                progMsg.setMessage(activity.getApplicationContext().getString(R.string.loading_message));
                progMsg.show();
            }

            @Override
            public void onSuccess(String downloadedText, int httpStatusCode) {
                Log.d(TAG, "ByWords - success");

                if (progMsg.isShowing())
                    progMsg.dismiss();


                callbacksTextReturn.Return_text(downloadedText);

            }

            @Override
            public void onError(int httpStatusCode, String errorMessage) {
                Log.e(TAG, "ByWords - error: " + errorMessage);

                if (progMsg.isShowing())
                    progMsg.dismiss();
            }
        });
        apiPostManager.execute(DataStore.byWords + limit, text.toJSON().toString());
    }


    public void CompareAllSentencesWordsAndroid(JSONArray text){
        ApiPostManager apiPostManager = new ApiPostManager(new CallbacksWebApi() {
            @Override
            public void onAboutToBegin() {
                Log.d(TAG, "CompareAllSentencesWordsAndroid - start");

                progMsg = new ProgressDialog(instance.activity);
                progMsg.setMessage(activity.getApplicationContext().getString(R.string.loading_message));
                progMsg.show();
            }

            @Override
            public void onSuccess(String downloadedText, int httpStatusCode) {
                Log.d(TAG, "CompareAllSentencesWordsAndroid - success");

                if (progMsg.isShowing())
                    progMsg.dismiss();

                callbacksTextReturn.Return_text(downloadedText);

            }

            @Override
            public void onError(int httpStatusCode, String errorMessage) {
                Log.e(TAG, "CompareAllSentencesWordsAndroid - error: " + errorMessage);

                if (progMsg.isShowing())
                    progMsg.dismiss();
            }
        });
        apiPostManager.execute(DataStore.byAllSentencesWords, text.toString());
    }
}
