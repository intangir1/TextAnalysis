package com.likhtman.TextAnalysis;

import android.app.AlertDialog;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatDialogFragment;


public class DialogMessage {
    private static DialogMessage instance = null;
    private int message_number;
    private Context context;
    private AlertDialog alertDialog = null;

    private DialogMessage() {

    }


    public static DialogMessage Get_instance(int _message_number, Context _context){
        if(instance == null){
            instance = new DialogMessage();
        }
        instance.message_number = _message_number;
        instance.context = _context;
        return instance;
    }


    public void Show_dialog(){
        if(alertDialog != null && alertDialog.isShowing()){
            alertDialog.dismiss();
        }

        String title = Get_title();
        String message = Get_message();

        AlertDialog.Builder builder = new AlertDialog.Builder(instance.context);
        builder.setTitle(title);
        builder.setMessage(message);
        builder.setPositiveButton("ok", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int arg1) {
                dialog.dismiss();
            }
        });
        builder.setCancelable(false);
        alertDialog = builder.create();
        alertDialog.show();
    }


    private String Get_message() {
        switch (instance.message_number){
            case (1):
                return instance.context.getResources().getString(R.string.dialog_message_auto_sign_out);
            case (2):
                return instance.context.getResources().getString(R.string.dialog_message_faq);
            case (3):
                return instance.context.getResources().getString(R.string.dialog_message_registered);
            case (4):
                return instance.context.getResources().getString(R.string.dialog_message_vip);
            case (5):
                return instance.context.getString(R.string.dialog_message_server);
            case (6):
                return instance.context.getString(R.string.dialog_message_login);
            case (7):
                return instance.context.getString(R.string.dialog_message_received);
            case (8):
                return instance.context.getString(R.string.dialog_message_get_all_error);
            case (9):
                return instance.context.getString(R.string.dialog_message_color);
            default:
                return "error";
        }
    }

    private String Get_title() {
        switch (instance.message_number){
            case (1):
                return instance.context.getResources().getString(R.string.dialog_title_auto_sign_out);
            case (2):
                return instance.context.getResources().getString(R.string.dialog_title_faq);
            case (3):
                return instance.context.getResources().getString(R.string.dialog_title_registered);
            case (4):
                return instance.context.getResources().getString(R.string.dialog_title_vip);
            case (5):
                return instance.context.getString(R.string.dialog_title_server);
            case (6):
                return instance.context.getString(R.string.dialog_title_login);
            case (7):
                return instance.context.getString(R.string.dialog_title_received);
            case (8):
                return instance.context.getString(R.string.dialog_title_get_all_error);
            case (9):
                return instance.context.getString(R.string.dialog_title_color);
            default:
                return "error";
        }
    }
}