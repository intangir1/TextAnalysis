package com.likhtman.TextAnalysis;

import android.app.Activity;
import android.os.AsyncTask;

public class AutomaticSignOut extends AsyncTask <Activity, Void, String>{
    private final int MILLISECONDS_IN_MINUTES = 60000;
    private Activity activity = null;


    @Override
    protected void onPreExecute() {
        super.onPreExecute();
    }


    @Override
    protected String doInBackground(Activity... activities) {

        final int minutes = 30;
        try{
            Thread.sleep(minutes * MILLISECONDS_IN_MINUTES);
//            Thread.sleep(3000);
            activity = activities[0];
            return "executed";
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        return null;

    }


    @Override
    protected void onPostExecute(String s) {
        super.onPostExecute(s);
        if(s != null && s.equals("executed")){
            DialogMessage.Get_instance(1, activity).Show_dialog();
            StorageDataFromServer.Get_instance().Clear_login(activity);
        }
    }
}
