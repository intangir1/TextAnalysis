package com.likhtman.TextAnalysis.Model;

import org.json.JSONException;
import org.json.JSONObject;

public class Login {
    private String userNickName;
    private String userPassword;
    private Short userLevel = 0;
    private String userPicture;
    private String usertoken;
    private JSONObject jsonObject;

    public Login(){

    }


    public Login(String _userNickName, String _userPicture, Short _userLevel){
        setUserNickName(_userNickName);
        setUserPicture(_userPicture);
        setUserLevel(_userLevel);
    }


    public Login(String _userNickName, String _userPassword){
        setUserNickName(_userNickName);
        setUserPassword(_userPassword);
    }


    public Login(String _userNickName, String _userPassword, Short _userLevel, String _userPicture){
        setUserNickName(_userNickName);
        setUserPassword(_userPassword);
        setUserLevel(_userLevel);
        setUserPicture(_userPicture);
    }


    public Login(String downloadedText){
        try {
            JSONObject jsonObject = new JSONObject(downloadedText);

            try {
                setUserNickName(jsonObject.getString("userNickName"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setUserPassword(jsonObject.getString("userPassword"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setUserLevel((short) jsonObject.getInt("userLevel"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setUserPicture(jsonObject.getString("userPicture"));
            } catch (JSONException e){
                e.printStackTrace();
            }

            try {
                setUsertoken(jsonObject.getString("usertoken"));
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
        Try_JSON("userNickName", userNickName);
        Try_JSON("userPassword", userPassword);
        Try_JSON("userLevel", userLevel);

        if(getUserPicture()!=null && !getUserPicture().equals(""))
            Try_JSON("userPicture", userPicture);

        return jsonObject;
    }


    public String getUserNickName() {
        return userNickName;
    }

    public void setUserNickName(String userNickName) {
        this.userNickName = userNickName;
    }

    public String getUserPassword() {
        return userPassword;
    }

    public void setUserPassword(String userPassword) {
        this.userPassword = userPassword;
    }

    public Short getUserLevel() {
        return userLevel;
    }

    public void setUserLevel(Short userLevel) {
        this.userLevel = userLevel;
    }

    public String getUserPicture() {
        return userPicture;
    }

    public void setUserPicture(String userPicture) {
        this.userPicture = userPicture;
    }

    public String getUsertoken() {
        return usertoken;
    }

    public void setUsertoken(String usertoken) {
        this.usertoken = usertoken;
    }


    @Override
    public String toString() {
        return "Login{" +
                "userNickName='" + userNickName + '\'' +
                ", userPassword='" + userPassword + '\'' +
                ", userLevel=" + userLevel +
                ", userPicture='" + userPicture + '\'' +
                '}';
    }
}
