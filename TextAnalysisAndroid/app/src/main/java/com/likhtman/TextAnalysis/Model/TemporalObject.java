package com.likhtman.TextAnalysis.Model;

import org.json.JSONException;
import org.json.JSONObject;

public class TemporalObject {

    private String mongoId;
    private String action;
    private String connectionWord;
    private String inputedWord;
    private String type;
    private JSONObject jsonObject;


    public TemporalObject(){

    }


    public TemporalObject(String _mongoId, String _action, String _connectionWord, String _inputedWord, String _type){
        setMongoId(_mongoId);
        setAction(_action);
        setConnectionWord(_connectionWord);
        setInputedWord(_inputedWord);
        setType(_type);
    }


    public TemporalObject(String downloadedText){
        try {
            JSONObject obj = new JSONObject(downloadedText);

            try {
                setMongoId(obj.getString("mongoId"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setAction(obj.getString("action"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setConnectionWord(obj.getString("connectionWord"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setInputedWord(obj.getString("inputedWord"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setType(obj.getString("type"));
            } catch (JSONException e){
                e.printStackTrace();
            }


        } catch (JSONException e) {
            e.printStackTrace();
        }
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
        Try_JSON("mongoId", mongoId);
        Try_JSON("action", action);
        Try_JSON("connectionWord", connectionWord);
        Try_JSON("inputedWord", inputedWord);
        Try_JSON("type", type);

        return jsonObject;
    }


    public boolean Equals(String str){
        return this.action.equals(str) || this.connectionWord.equals(str) || this.inputedWord.equals(str);
    }


    public String getMongoId() {
        return mongoId;
    }

    public void setMongoId(String mongoId) {
        this.mongoId = mongoId;
    }

    public String getAction() {
        return action;
    }

    public void setAction(String action) {
        this.action = action;
    }

    public String getConnectionWord() {
        return connectionWord;
    }

    public void setConnectionWord(String connectionWord) {
        this.connectionWord = connectionWord;
    }

    public String getInputedWord() {
        return inputedWord;
    }

    public void setInputedWord(String inputedWord) {
        this.inputedWord = inputedWord;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }


    @Override
    public String toString() {
        return "TemporalObject{" +
                "mongoId='" + mongoId + '\'' +
                ", action='" + action + '\'' +
                ", connectionWord='" + connectionWord + '\'' +
                ", inputedWord='" + inputedWord + '\'' +
                ", type='" + type + '\'' +
                '}';
    }

}
