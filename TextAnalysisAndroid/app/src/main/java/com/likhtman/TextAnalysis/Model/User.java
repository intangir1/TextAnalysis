package com.likhtman.TextAnalysis.Model;

import android.icu.text.DateFormat;
import android.icu.text.SimpleDateFormat;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.Calendar;
import java.util.Date;
import java.util.TimeZone;

public class User {
    private String userID;
    private String userFirstName;
    private String userLastName;
    private String userNickName;
    private String userPassword;
    private String userEmail;
    private String userGender;
    private Date userBirthDate;
    private String userPicture;
    private Short userLevel;
    private String userRole;
    private String userImage;
    private JSONObject jsonObject;


    public User(){

    }


    public User(String _userID, String _userFirstName, String _userLastName, String _userNickName,
                String _userPassword, String _userEmail, String _userGender, Date _userBirthDate, String _userPicture, Short _userLevel, String _userRole, String _userImage){
        setUserID(_userID);
        setUserFirstName(_userFirstName);
        setUserLastName(_userLastName);
        setUserNickName(_userNickName);
        setUserPassword(_userPassword);
        setUserEmail(_userEmail);
        setUserGender(_userGender);
        setUserBirthDate(_userBirthDate);
        setUserPicture(_userPicture);
        setUserLevel(_userLevel);
        setUserRole(_userRole);
        setUserImage(_userImage);
    }

    public User(String _userID, String _userFirstName, String _userLastName, String _userNickName,
                String _userPassword, String _userEmail, String _userGender, Date _userBirthDate, Short _userLevel, String _userRole){
        setUserID(_userID);
        setUserFirstName(_userFirstName);
        setUserLastName(_userLastName);
        setUserNickName(_userNickName);
        setUserPassword(_userPassword);
        setUserEmail(_userEmail);
        setUserGender(_userGender);
        setUserBirthDate(_userBirthDate);
        setUserLevel(_userLevel);
        setUserRole(_userRole);
    }


    public User(String downloadedText){
        try {
            JSONObject jsonObject = new JSONObject(downloadedText);

            try {
                setUserID(jsonObject.getString("userID"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setUserFirstName(jsonObject.getString("userFirstName"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setUserLastName(jsonObject.getString("userLastName"));
            } catch (JSONException e){
                e.printStackTrace();
            }


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
                setUserEmail(jsonObject.getString("userEmail"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setUserGender(jsonObject.getString("userGender"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setUserBirthDate(new Date(jsonObject.getString("userBirthDate")));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setUserPicture(jsonObject.getString("userPicture"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setUserLevel((short) jsonObject.getInt("userLevel"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setUserRole(jsonObject.getString("userRole"));
            } catch (JSONException e){
                e.printStackTrace();
            }


            try {
                setUserImage(jsonObject.getString("userImage"));
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
        Try_JSON("userID", userID);
        Try_JSON("userFirstName", userFirstName);
        Try_JSON("userLastName", userLastName);
        Try_JSON("userNickName", userNickName);
        Try_JSON("userPassword", userPassword);
        Try_JSON("userEmail", userEmail);
        Try_JSON("userGender", userGender);
        String formatted_date = new SimpleDateFormat("yyyy-MM-dd'T'hh:mm:ss").format(userBirthDate);
        Try_JSON("userBirthDate", formatted_date);
        Try_JSON("userLevel", userLevel);
        Try_JSON("userRole", userRole);

        if(getUserPicture()!=null && !getUserPicture().equals(""))
            Try_JSON("userPicture", userPicture);
        if(getUserImage()!=null && !getUserImage().equals(""))
            Try_JSON("userImage", userImage);

        return jsonObject;
    }


    public String getUserID() {
        return userID;
    }

    public void setUserID(String userID) {
        this.userID = userID;
    }

    public String getUserFirstName() {
        return userFirstName;
    }

    public void setUserFirstName(String userFirstName) {
        this.userFirstName = userFirstName;
    }

    public String getUserLastName() {
        return userLastName;
    }

    public void setUserLastName(String userLastName) {
        this.userLastName = userLastName;
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

    public String getUserEmail() {
        return userEmail;
    }

    public void setUserEmail(String userEmail) {
        this.userEmail = userEmail;
    }

    public String getUserGender() {
        return userGender;
    }

    public void setUserGender(String userGender) {
        this.userGender = userGender;
    }

    public Date getUserBirthDate() {
        return userBirthDate;
    }

    public void setUserBirthDate(Date userBirthDate) {
        this.userBirthDate = userBirthDate;
    }

    public String getUserPicture() {
        return userPicture;
    }

    public void setUserPicture(String userPicture) {
        this.userPicture = userPicture;
    }

    public Short getUserLevel() {
        return userLevel;
    }

    public void setUserLevel(Short userLevel) {
        this.userLevel = userLevel;
    }

    public String getUserRole() {
        return userRole;
    }

    public void setUserRole(String userRole) {
        userRole = userRole;
    }

    public String getUserImage() {
        return userImage;
    }

    public void setUserImage(String userImage) {
        this.userImage = userImage;
    }


    @Override
    public String toString() {
        return "User{" +
                "userID='" + userID + '\'' +
                ", userFirstName='" + userFirstName + '\'' +
                ", userLastName='" + userLastName + '\'' +
                ", userNickName='" + userNickName + '\'' +
                ", userPassword='" + userPassword + '\'' +
                ", userEmail='" + userEmail + '\'' +
                ", userGender='" + userGender + '\'' +
                ", userBirthDate=" + userBirthDate +
                ", userPicture='" + userPicture + '\'' +
                ", userLevel=" + userLevel +
                ", userRole='" + userRole + '\'' +
                ", userImage='" + userImage + '\'' +
                '}';
    }
}
