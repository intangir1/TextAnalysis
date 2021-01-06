package com.likhtman.TextAnalysis.Services.MainDB;

import android.app.Activity;
import android.app.Application;
import android.app.ProgressDialog;
import android.util.Log;

import com.likhtman.TextAnalysis.Callbacks.CallbacksForIrregularWords;
import com.likhtman.TextAnalysis.Callbacks.CallbacksWebApi;
import com.likhtman.TextAnalysis.DataStore;
import com.likhtman.TextAnalysis.DialogMessage;
import com.likhtman.TextAnalysis.Managers.HttpClient.ApiGetManager;
import com.likhtman.TextAnalysis.Model.IrregularObject;
import com.likhtman.TextAnalysis.R;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;

public class IrregularService extends Application {
    final String TAG = IrregularService.this.getClass().getSimpleName();

    private CallbacksForIrregularWords callbacksForIrregularWords;
    private ProgressDialog progMsg;
    private Activity activity;
    private static IrregularService instance = null;

    private IrregularService(){

    }

    public static IrregularService Get_instance(CallbacksForIrregularWords _callbacksForIrregularWords, Activity _activity){
        if(instance == null){
            instance = new IrregularService();
        }
        instance.callbacksForIrregularWords = _callbacksForIrregularWords;
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

                ArrayList<IrregularObject> result = new ArrayList<IrregularObject>();

                JSONArray jArray = null;
                try{
                    jArray = new JSONArray(downloadedText);
                    for(int i = 0; i < jArray.length(); i++){
                        JSONObject obj = jArray.getJSONObject(i);
                        IrregularObject irregularObject = new IrregularObject(obj);
                        result.add(irregularObject);
                    }
                    callbacksForIrregularWords.Return_irregulars_array(result);
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
        apiGetManager.execute(DataStore.byIrregulars);
    }
}
