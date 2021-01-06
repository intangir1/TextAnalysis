package com.likhtman.TextAnalysis.Model;

import org.json.JSONException;
import org.json.JSONObject;

public class IrregularObject {

    private String mongoId;
    private String first;
    private String second;
    private String third;
    private JSONObject jsonObject;


    public IrregularObject(){

    }


    public IrregularObject(String _mongoId, String _first, String _second, String _third){
        setMongoId(_mongoId);
        setFirst(_first);
        setSecond(_second);
        setThird(_third);
    }

    public IrregularObject(String _first, String _second, String _third){
        setFirst(_first);
        setSecond(_second);
        setThird(_third);
    }


    public IrregularObject(JSONObject obj){
        try {
            setMongoId(obj.getString("mongoId"));
        } catch (JSONException e){
            e.printStackTrace();
        }


        try {
            setFirst(obj.getString("first"));
        } catch (JSONException e){
            e.printStackTrace();
        }


        try {
            setSecond(obj.getString("second"));
        } catch (JSONException e){
            e.printStackTrace();
        }


        try {
            setThird(obj.getString("third"));
        } catch (JSONException e){
            e.printStackTrace();
        }
    }

    public IrregularObject(String downloadedText){
        try {
            JSONObject obj = new JSONObject(downloadedText);

            try {
                setMongoId(obj.getString("mongoId"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setFirst(obj.getString("first"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setSecond(obj.getString("second"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setThird(obj.getString("third"));
            } catch (JSONException e){
                e.printStackTrace();
            }


        } catch (JSONException e) {
            e.printStackTrace();
        }
    }


    public boolean Equals(String str){
        return this.first.equals(str) || this.second.equals(str) || this.third.equals(str);
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
        Try_JSON("first", first);
        Try_JSON("second", second);
        Try_JSON("third", third);
        return jsonObject;
    }


    public String getMongoId() {
        return mongoId;
    }

    public void setMongoId(String mongoId) {
        this.mongoId = mongoId;
    }


    public String getFirst() {
        return first;
    }

    public void setFirst(String first) {
        this.first = first;
    }

    public String getSecond() {
        return second;
    }

    public void setSecond(String second) {
        this.second = second;
    }

    public String getThird() {
        return third;
    }

    public void setThird(String third) {
        this.third = third;
    }

    public String Get_all_forms(){
        return getFirst() + " - " + getSecond() + " - " + getThird();
    }


    @Override
    public String toString() {
        return "IrregularObject{" +
                "mongoId='" + mongoId + '\'' +
                ", first='" + first + '\'' +
                ", second='" + second + '\'' +
                ", third='" + third + '\'' +
                '}';
    }
}
