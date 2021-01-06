package com.likhtman.TextAnalysis.Services;

import android.app.Activity;
import android.app.Application;
import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;
import android.os.Build;
import android.util.Log;
import android.widget.Toast;

import com.likhtman.TextAnalysis.AutomaticSignOut;
import com.likhtman.TextAnalysis.Callbacks.CallbacksOpenDefaultFragment;
import com.likhtman.TextAnalysis.Callbacks.CallbacksWebApi;
import com.likhtman.TextAnalysis.DataStore;
import com.likhtman.TextAnalysis.DialogMessage;
import com.likhtman.TextAnalysis.Logics.TextSplitter;
import com.likhtman.TextAnalysis.Managers.HttpClient.ApiPostManager;
import com.likhtman.TextAnalysis.Managers.HttpClient.ApiPutManager;
import com.likhtman.TextAnalysis.Model.Login;
import com.likhtman.TextAnalysis.Model.User;
import com.likhtman.TextAnalysis.R;
import com.likhtman.TextAnalysis.StorageDataFromServer;

import org.json.JSONObject;

public class UserService extends Application {
    final String TAG = UserService.this.getClass().getSimpleName();

    private ProgressDialog progMsg;
    private static UserService instance = null;
    private Activity activity;

    private UserService(){

    }


    public static UserService Get_instance(Activity _activity){
        if(instance == null){
            instance = new UserService();
        }
        instance.activity = _activity;
//        instance.context = tmpContext;
        return instance;
    }


    public void LoginCore(Login loginUser){
        ApiPostManager apiPostManager = new ApiPostManager(new CallbacksWebApi() {
            @Override
            public void onAboutToBegin() {
                Log.d(TAG, "LoginCore - start");

                progMsg = new ProgressDialog(instance.activity);
                progMsg.setMessage(activity.getApplicationContext().getString(R.string.loading_message));
                progMsg.show();
            }

            @Override
            public void onSuccess(String downloadedText, int httpStatusCode) {
                Log.d(TAG, "LoginCore - success");

                if (progMsg.isShowing())
                    progMsg.dismiss();

                Login login = new Login(downloadedText);
                StorageDataFromServer.Get_instance().setLogin(login);
                StorageDataFromServer.Get_instance().setUserToken(downloadedText);
                StorageDataFromServer.Get_instance().setWas_updated(false);

                AsyncTask<Activity,Void,String> auto_sign_out = new AutomaticSignOut();
                try{
                    if(Build.VERSION.SDK_INT >= 11/*HONEYCOMB*/) {
                        auto_sign_out.executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR, activity);
                    } else {
                        auto_sign_out.execute(activity);
                    }
                } catch (Exception e){
                    e.printStackTrace();
                }

                Toast.makeText(activity, activity.getApplicationContext().getString(R.string.toast_success_log_in),Toast.LENGTH_SHORT).show();
                ((CallbacksOpenDefaultFragment)activity).Open_default_fragment();
            }

            @Override
            public void onError(int httpStatusCode, String errorMessage) {
                Log.d(TAG, "LoginCore - error: " + errorMessage);

                if (progMsg.isShowing())
                    progMsg.dismiss();

                if(httpStatusCode > 0){
                    DialogMessage.Get_instance(6, activity).Show_dialog();
                } else{
                    DialogMessage.Get_instance(5, activity).Show_dialog();
                }
            }
        });

        String url = DataStore.loginUrl;
        JSONObject json = loginUser.toJSON();
        String strJson = json.toString();


        String[] queryToTextToDownloader = new String[]{url, strJson};
        //apiPostManager.execute(queryToTextToDownloader);


        if(Build.VERSION.SDK_INT >= 11/*HONEYCOMB*/) {
            apiPostManager.executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR, queryToTextToDownloader);
        } else {
            apiPostManager.execute(queryToTextToDownloader);
        }
    }


    public void SignUp(User userModel){
        ApiPostManager apiPostManager = new ApiPostManager(new CallbacksWebApi() {
            @Override
            public void onAboutToBegin() {
                Log.d(TAG, "SignUp - start");

                progMsg = new ProgressDialog(instance.activity);
                progMsg.setMessage(activity.getApplicationContext().getString(R.string.loading_message));
                progMsg.show();
            }

            @Override
            public void onSuccess(String downloadedText, int httpStatusCode) {
                Log.d(TAG, "SignUp - success");

                if (progMsg.isShowing())
                    progMsg.dismiss();

                Login login = new Login(downloadedText);
                LoginCore(login);
            }

            @Override
            public void onError(int httpStatusCode, String errorMessage) {
                Log.d(TAG, "SignUp - error: " + errorMessage);

                if (progMsg.isShowing())
                    progMsg.dismiss();

                DialogMessage.Get_instance(5, activity).Show_dialog();
            }
        });


        String url = DataStore.usersUrl;
        JSONObject json = userModel.toJSON();
        String strJson = json.toString();


        String[] queryToTextToDownloader = new String[]{url, strJson};

        if(Build.VERSION.SDK_INT >= 11/*HONEYCOMB*/) {
            apiPostManager.executeOnExecutor(AsyncTask.THREAD_POOL_EXECUTOR, queryToTextToDownloader);
        } else {
            apiPostManager.execute(queryToTextToDownloader);
        }
    }


    public void UpdateUser(User user){
        ApiPutManager apiPutManager = new ApiPutManager(new CallbacksWebApi() {
            @Override
            public void onAboutToBegin() {
                Log.d(TAG, "UpdateUser - start");

                progMsg = new ProgressDialog(instance.activity);
                progMsg.setMessage(activity.getApplicationContext().getString(R.string.loading_message));
                progMsg.show();
            }

            @Override
            public void onSuccess(String downloadedText, int httpStatusCode) {
                Log.d(TAG, "UpdateUser - success");

                if (progMsg.isShowing())
                    progMsg.dismiss();

                User user = new User(downloadedText);
            }

            @Override
            public void onError(int httpStatusCode, String errorMessage) {
                Log.e(TAG, "UpdateUser - error: " + errorMessage);

                if (progMsg.isShowing())
                    progMsg.dismiss();
            }
        });
        apiPutManager.execute(DataStore.loginUrl, user.toJSON().toString());
    }

}
