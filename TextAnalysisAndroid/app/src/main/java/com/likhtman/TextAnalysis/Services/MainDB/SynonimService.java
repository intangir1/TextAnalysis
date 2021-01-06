package com.likhtman.TextAnalysis.Services.MainDB;

import android.app.Activity;
import android.app.Application;
import android.app.ProgressDialog;
import android.util.Log;
import android.widget.Toast;

import androidx.fragment.app.FragmentActivity;

import com.likhtman.TextAnalysis.Callbacks.CallbacksForRelational;
import com.likhtman.TextAnalysis.Callbacks.CallbacksWebApi;
import com.likhtman.TextAnalysis.Callbacks.CallbacksWordsArrayReturn;
import com.likhtman.TextAnalysis.DataStore;
import com.likhtman.TextAnalysis.DialogMessage;
import com.likhtman.TextAnalysis.Managers.HttpClient.ApiGetManager;
import com.likhtman.TextAnalysis.R;

import org.json.JSONArray;
import org.json.JSONException;

import java.net.HttpURLConnection;
import java.util.ArrayList;

public class SynonimService extends Application {
    final String TAG = SynonimService.this.getClass().getSimpleName();

    private ProgressDialog progMsg;
    private static SynonimService instance = null;
    private Activity activity;

    private SynonimService(){

    }


    public static SynonimService Get_instance(Activity _activity){
        if(instance == null){
            instance = new SynonimService();
        }
        instance.activity = _activity;
        return instance;
    }


    public void GetAllWords(CallbacksForRelational _callbacksForRelational){
        CallbacksForRelational callbacksForRelational = _callbacksForRelational;
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

                if (progMsg.isShowing()){
                    progMsg.dismiss();
                }

                ArrayList<ArrayList<String>> result = new ArrayList<ArrayList<String>>();
                JSONArray jArray = null;
                try {
                    jArray = new JSONArray(downloadedText);
                    for(int i = 0; i < jArray.length(); i++){
                        ArrayList<String> inner_result = new ArrayList<>();
                        JSONArray jArrayInner = jArray.getJSONArray(i);
                        for(int j = 0; j < jArrayInner.length(); j++){
                            String word = jArrayInner.getString(j);
                            inner_result.add(word);
                        }
                        result.add(inner_result);
                    }
                    callbacksForRelational.Return_all_words(result);
                } catch (JSONException e) {
                    e.printStackTrace();
                }
            }


            @Override
            public void onError(int httpStatusCode, String errorMessage) {
                Log.e(TAG, "GetAllWords - error: " + errorMessage);

                if (progMsg.isShowing()){
                    progMsg.dismiss();
                }

                DialogMessage.Get_instance(8, activity).Show_dialog();
            }

        });
        apiGetManager.execute(DataStore.bySynonim);
    }


    public void GetWordsBy(String word, CallbacksWordsArrayReturn _callbacksWordsArrayReturn){
        CallbacksWordsArrayReturn callbacksWordsArrayReturn = _callbacksWordsArrayReturn;

        ApiGetManager apiGetManager = new ApiGetManager(new CallbacksWebApi() {
            @Override
            public void onAboutToBegin() {
                Log.d(TAG, "GetWordsBy - start");

                progMsg = new ProgressDialog(instance.activity);
                progMsg.setMessage(activity.getApplicationContext().getString(R.string.loading_message));
                progMsg.show();
            }

            @Override
            public void onSuccess(String downloadedText, int httpStatusCode) {
                Log.d(TAG, "GetWordsBy - success");

                if (progMsg.isShowing())
                    progMsg.dismiss();

                ArrayList<String> result = new ArrayList<String>();

                if(httpStatusCode == HttpURLConnection.HTTP_NO_CONTENT){
                    Toast.makeText(instance.activity, "No synonims were found", Toast.LENGTH_SHORT).show();
                    callbacksWordsArrayReturn.Return_synonims(result);
                } else {
                    JSONArray jArray = null;
                    try {
                        jArray = new JSONArray(downloadedText);
                        for (int i = 0; i < jArray.length(); i++) {
                            String word = jArray.getString(i);
                            result.add(word);
                        }
                        callbacksWordsArrayReturn.Return_synonims(result);
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                }
            }


            @Override
            public void onError(int httpStatusCode, String errorMessage) {
                Log.e(TAG, "GetWordsBy - error: " + errorMessage);

                if (progMsg.isShowing())
                    progMsg.dismiss();

                DialogMessage.Get_instance(5, activity).Show_dialog();
            }
        });
        apiGetManager.execute(DataStore.bySynonim + word);
    }
}
