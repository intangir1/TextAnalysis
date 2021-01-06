package com.likhtman.TextAnalysis.Model;

import org.json.JSONException;
import org.json.JSONObject;

public class FullText {
    private String textToCheck;
    private JSONObject jsonObject;

    public FullText(){

    }


    public FullText(String _textToCheck){
        setTextToCheck(_textToCheck);
    }


    private <T> void Try_JSON(String name, T input){
        try {
            jsonObject.put(name, input);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }


    public JSONObject toJSON(){
        jsonObject = new JSONObject();
        Try_JSON("textToCheck", textToCheck);
        return jsonObject;
    }

    public String getTextToCheck() {
        return textToCheck;
    }

    public void setTextToCheck(String textToCheck) {
        this.textToCheck = textToCheck;
    }

    @Override
    public String toString() {
        return "FullText{" +
                "textToCheck='" + textToCheck + '\'' +
                '}';
    }
}
