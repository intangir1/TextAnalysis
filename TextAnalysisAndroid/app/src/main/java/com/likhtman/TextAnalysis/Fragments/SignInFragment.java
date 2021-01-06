package com.likhtman.TextAnalysis.Fragments;

import android.app.Activity;
import android.os.Bundle;

import androidx.fragment.app.Fragment;

import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;

import com.google.android.material.textfield.TextInputEditText;
import com.google.android.material.textfield.TextInputLayout;
import com.likhtman.TextAnalysis.Model.Login;
import com.likhtman.TextAnalysis.R;
import com.likhtman.TextAnalysis.Services.UserService;

public class SignInFragment extends Fragment {
    final String TAG = SignInFragment.this.getClass().getSimpleName();

    private Activity parentActivity;

    Button btn_enter;
    TextInputEditText etxt_name;
    TextInputEditText etxt_pass;

    TextInputLayout lay_name;
    TextInputLayout lay_pass;


    private void Init_visuals(View view) {
        btn_enter = (Button)view.findViewById(R.id.enterIN);

        lay_name = (TextInputLayout)view.findViewById(R.id.nameIN);
        lay_pass = (TextInputLayout)view.findViewById(R.id.passIN);

        etxt_name = (TextInputEditText)view.findViewById(R.id.etxt_nameIn);
        etxt_pass = (TextInputEditText)view.findViewById(R.id.etxt_passIn);
    }


    private void Init_listeners() {
        etxt_name.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                Errors_update(lay_name);
                Button_enter_update();
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });

        etxt_pass.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                Errors_update(lay_pass);
                Button_enter_update();
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });

        btn_enter.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String nick = etxt_name.getText().toString();
                String pass = etxt_pass.getText().toString();
                Login login = new Login(nick, pass);
                UserService.Get_instance(getActivity()).LoginCore(login);
            }
        });
    }


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        parentActivity = getActivity();

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = null;
        view = inflater.inflate(R.layout.fragment_sign_in, container, false);
        return view;
    }


    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        Init_visuals(view);
        Init_listeners();
    }


    private void Button_enter_update(){
        if (lay_name.isErrorEnabled() || lay_pass.isErrorEnabled() || etxt_name.getText().toString().length() == 0 || etxt_pass.getText().toString().length() == 0){
            btn_enter.setEnabled(false);
        } else{
            btn_enter.setEnabled(true);
        }

    }


    private void Set_error(TextInputLayout lay, String message) {
        lay.setErrorEnabled(true);
        lay.setError(message);
    }


    private void Errors_update(TextInputLayout lay) {

        if(lay.isErrorEnabled()){
            lay.setErrorEnabled(false);
        }

        if(lay.getEditText().getText().toString().length() == 0){
            Set_error(lay, "Input required");
        } else if(lay.getEditText().getText().toString().length() < 2) {
            Set_error(lay, "Minimum 2 characters required");
        }
    }


}