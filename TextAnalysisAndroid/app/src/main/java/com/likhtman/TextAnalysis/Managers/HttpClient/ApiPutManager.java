package com.likhtman.TextAnalysis.Managers.HttpClient;

import android.os.AsyncTask;

import com.likhtman.TextAnalysis.Callbacks.CallbacksWebApi;
import com.likhtman.TextAnalysis.StorageDataFromServer;

import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

public class ApiPutManager extends AsyncTask<String, Void, String> {
    private CallbacksWebApi callbacks; // Notify to activity what happened.
    private int httpStatusCode; // Http status code.
    private String errorMessage; // Error message.
    private String link;
    // Take given link:
    private String requestBody;
    // Constructor:
    public ApiPutManager(CallbacksWebApi callbacks) {
        this.callbacks = callbacks;
    }

    // Executes before doInBackground in the UI's thread:
    @Override
    protected void onPreExecute() {
        callbacks.onAboutToBegin();
    }

    // Executes in the background in a different thread than the UI's thread:
    @Override
    protected String doInBackground(String... params) {
        InputStream inputStream = null;
        InputStreamReader inputStreamReader = null;
        BufferedReader bufferedReader = null;
        try {
            // Take given link:
            link = params[0];
            // Take given link:
            requestBody = params[1];

            URL url = null;
            // Create a url:
            try {
                url = new URL(link);
            } catch (MalformedURLException e1) {
                e1.printStackTrace();
            }

            HttpURLConnection connection = null;

            try {
                connection = (HttpURLConnection) url.openConnection();
                connection.setRequestMethod("PUT");

                connection.setRequestProperty("Content-Type","application/json");
                connection.setRequestProperty("Authorization","Bearer " + StorageDataFromServer.Get_instance().getUserToken());
                connection.setRequestProperty("frontType", "android");

                connection.setDoOutput(true);
                OutputStream outputStream = new BufferedOutputStream(connection.getOutputStream());
                BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(outputStream, "utf-8"));
                writer.write(requestBody);
                writer.flush();
                writer.close();
                outputStream.close();
            } catch (IOException e1) {
                e1.printStackTrace();
            }

            // get stream
            try {
                httpStatusCode = connection.getResponseCode();
                if (httpStatusCode < HttpURLConnection.HTTP_BAD_REQUEST) {
                    inputStream = connection.getInputStream();
                } else {
                    inputStream = connection.getErrorStream();
                }
            } catch (IOException e1) {
                e1.printStackTrace();
            }
            inputStream = connection.getInputStream();
            inputStreamReader = new InputStreamReader(inputStream);
            bufferedReader = new BufferedReader(inputStreamReader);

            // Read downloaded text:
            StringBuilder downloadedText = new StringBuilder();
            String oneLine = bufferedReader.readLine();
            while(oneLine != null) {
                downloadedText.append(oneLine);
                oneLine = bufferedReader.readLine();
                if(oneLine != null){
                    downloadedText.append("\n");
                }
            }

            // Return result:
            return downloadedText.toString();
        }
        catch(Exception ex) {
            errorMessage = ex.getMessage(); // Can be null.
            return null;
        }
        finally { // Close readers:
            if(bufferedReader != null)
                try { bufferedReader.close(); } catch (Exception e) { }
            if(inputStreamReader != null)
                try { inputStreamReader.close(); } catch (Exception e) { }
            if(inputStream != null)
                try { inputStream.close(); } catch (Exception e) { }
        }
    }

    // Executes after doInBackground in the UI's thread:
    protected void onPostExecute(String downloadedText) {
        if(downloadedText != null) // Don't check errorMessage cause it can be null even if there is an error.
        {
            callbacks.onSuccess(downloadedText, httpStatusCode);
        }
        else
            callbacks.onError(httpStatusCode, errorMessage);
    }

}