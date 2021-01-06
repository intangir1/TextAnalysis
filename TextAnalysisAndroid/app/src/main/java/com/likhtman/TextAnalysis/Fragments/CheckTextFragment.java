package com.likhtman.TextAnalysis.Fragments;

import android.app.Activity;
import android.os.Bundle;

import androidx.fragment.app.Fragment;

import android.text.Editable;
import android.text.InputFilter;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextWatcher;
import android.text.method.LinkMovementMethod;
import android.text.method.ScrollingMovementMethod;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.RadioGroup;
import android.widget.SeekBar;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.material.textfield.TextInputLayout;
import com.likhtman.TextAnalysis.Activities.MainActivity;
import com.likhtman.TextAnalysis.Callbacks.CallbacksTextReturn;
import com.likhtman.TextAnalysis.Callbacks.CallbacksWordsArrayReturn;
import com.likhtman.TextAnalysis.Logics.RepeatsAnalyze;
import com.likhtman.TextAnalysis.Logics.SpanCreation;
import com.likhtman.TextAnalysis.Logics.TextSplitter;
import com.likhtman.TextAnalysis.Model.FullText;
import com.likhtman.TextAnalysis.R;
import com.likhtman.TextAnalysis.Services.TextService;
import com.likhtman.TextAnalysis.StorageDataFromServer;

import org.json.JSONArray;
import org.json.JSONException;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.regex.Pattern;


public class CheckTextFragment extends Fragment implements CallbacksTextReturn, CallbacksWordsArrayReturn {
    final String TAG = CheckTextFragment.this.getClass().getSimpleName();

    TextInputLayout lay_input;
    LinearLayout layout_result;
    TextView txt_result;
    LinearLayout lay_by_words_info;

    SeekBar sb_number_of_percent;
    TextView txt_number_of_percent;


    private Activity parentActivity;
    private RadioGroup rdgrp_modes;

    private boolean is_by_words;
    private String analizedText;
    private String lastTime;
    private int percent_words;
    private String blockCharacterSet;


    private InputFilter filter = new InputFilter() {

        @Override
        public CharSequence filter(CharSequence source, int start, int end, Spanned dest, int dstart, int dend) {

            if (end != 0 && source != null && source != "" && blockCharacterSet.contains(("" + source))) {
                Toast.makeText(parentActivity, "This character is prohibited!",
                        Toast.LENGTH_SHORT).show();
                return "";
            }
            return null;
        }
    };


    private void Init_visuals(View view) {
        lay_input = (TextInputLayout) view.findViewById(R.id.lay_input);
        layout_result = (LinearLayout) view.findViewById(R.id.lay_result);
        lay_by_words_info = (LinearLayout)view.findViewById(R.id.lay_by_words_info);
        rdgrp_modes = (RadioGroup) view.findViewById(R.id.rdgrp_modes);
        txt_result = (TextView) view.findViewById(R.id.txt_result);
        sb_number_of_percent = (SeekBar)view.findViewById(R.id.sb_number_of_percent);
        txt_number_of_percent = (TextView)view.findViewById(R.id.txt_number_of_percent);
    }


    private void Init_default() {
        blockCharacterSet = "#_@^";
        analizedText = "";
        lastTime = "";
        is_by_words = false;

        lay_input.getEditText().setFilters(new InputFilter[]{filter});

        lay_by_words_info.setVisibility(View.INVISIBLE);
        rdgrp_modes.check(rdgrp_modes.getChildAt(0).getId());
        percent_words = 15;

        sb_number_of_percent.incrementProgressBy(1);
        sb_number_of_percent.setMax(30);
        sb_number_of_percent.setProgress(percent_words);

        txt_number_of_percent.setText("You chose: " + percent_words);

        txt_result.setMovementMethod(new ScrollingMovementMethod());
    }


    private void Init_listeners() {

        lay_input.getEditText().setMovementMethod(new ScrollingMovementMethod());

        lay_input.getEditText().addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

                String newStr = s.toString();
                newStr = newStr.replaceAll( "[@^_#]*", "" );
                if(!s.toString().equals( newStr )) {
                    Toast.makeText(parentActivity, "The prohibited characters were removed!",
                            Toast.LENGTH_SHORT).show();
                    lay_input.getEditText().setText( newStr );
                    lay_input.getEditText().setSelection(lay_input.getEditText().getText().length());
                } else if(s.length() > 1){
                    String signal = s.toString().substring(s.length() - 1);
                    if(signal.matches("[.?!]")){
                        Activate_check();
                    }
                }   else if(s.length() == 0){
                    txt_result.setText("");
                }

            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });


        rdgrp_modes.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId) {
                int index = group.indexOfChild(group.findViewById(checkedId));
                switch (index){
                    case 0:
                        is_by_words = false;
                        lay_by_words_info.setVisibility(View.INVISIBLE);
                        Log.d(TAG, "'By sentence' mode on");
                        break;
                    case 1:
                        is_by_words = true;
                        lay_by_words_info.setVisibility(View.VISIBLE);
                        Log.d(TAG, "'By words' mode on");
                        break;
                    default:
                        Log.e(TAG, "Wrong mode number");
                        break;
                }

                lay_input.getEditText().setText(lay_input.getEditText().getText().toString());
            }
        });

        sb_number_of_percent.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            int min = 1;

            @Override
            public void onProgressChanged(SeekBar seekBar, int i, boolean b) {
                if(i == 0) {
                    i = min;
                    sb_number_of_percent.setProgress(min);
                }
                percent_words = i;
                txt_number_of_percent.setText("You chose: " + percent_words);
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {

            }

            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
                lay_input.getEditText().setText(lay_input.getEditText().getText().toString());
            }
        });

    }


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        parentActivity = getActivity();
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = null;
        view = inflater.inflate(R.layout.fragment_check_text, container, false);
        return view;
    }


    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        Init_visuals(view);
        Init_default();
        Init_listeners();

    }


    private void Activate_check() {
        if(is_by_words){
            ByWords(lay_input.getEditText().getText().toString().trim());
        } else{
            BySentence(lay_input.getEditText().getText().toString().trim());        // clean last time???
        }
    }


    private void BySentence(String text) {
        analizedText = text;
        if (analizedText.replace("[^a-z0-9-]g", "").equals(lastTime.replace("[^a-z0-9-]g", ""))) {
            return;
        }
        lastTime = analizedText;

        String[] splitted = TextSplitter.Get_instance().Split_text(text);

//            if (splitted[splitted.length - 1].equals("")) {
//                Arrays.copyOf(splitted, splitted.length - 1);
//            }
//            if (!this.isLoggedIn){

//        String result = RepeatsAnalyze.GetInstance().CompareAllSentencesWordsAndroid(splitted);
//        Color_data(result);

        if(StorageDataFromServer.Get_instance().getLogin().getUserLevel() > 0){
            JSONArray myJSONArray = null;
            try {
                myJSONArray = new JSONArray(splitted);
                Log.d(TAG, "JSONarray for 'BySentence' created");
            } catch (JSONException e) {
                e.printStackTrace();
                Log.e(TAG, "JSONarray for 'BySentence' creation error");
                return;
            }
            TextService.Get_instance(this, parentActivity).CompareAllSentencesWordsAndroid(myJSONArray);
        } else{
            String result = RepeatsAnalyze.GetInstance().CompareAllSentencesWords(splitted);
            Color_data(result);
        }

//            } else{
//                this.textService.CompareAllSentencesWords(splitted);
//            }

//        }
    }


    private void Color_data(String data) {
        Log.d(TAG, "Color data started");

        txt_result.setText("");
        String[] s = data.split(" ");

        for (int i = 0; i < s.length; i++) {

            if(Pattern.compile("^(@)|^[.!?';,\"](@)").matcher(s[i]).find()){
                String expression = s[i];
                do{
                    i++;
                    expression += " " + s[i];
                } while(!Pattern.compile("(@)$|(@)[.!?';,\"]$").matcher(s[i]).find());       // ne contains! a imenno zakan4ivaeca na
                SpannableStringBuilder span = SpanCreation.GetInstance(this).Get_span_expression(expression);
                txt_result.setMovementMethod(LinkMovementMethod.getInstance());
                txt_result.append(span);
                if (i + 1 < s.length && s[i+1].matches(".*[#_@^].*")){
                    txt_result.append(" ");
                }
            } else if (Pattern.compile(".*[#_@^].*").matcher(s[i]).find()) {

                SpannableStringBuilder span = SpanCreation.GetInstance(this).Get_span(s[i]);

                txt_result.setMovementMethod(LinkMovementMethod.getInstance());
                txt_result.append(span);
                if (i + 1 < s.length && s[i+1].matches(".*[#_@^].*")){
                    txt_result.append(" ");
                }
            } else if (i + 1 < s.length && s[i+1].matches(".*[#_@^].*")) {
                txt_result.append(" " + s[i] + " ");
            } else {
                txt_result.append(" " + s[i]);
            }
        }
        SpanCreation.GetInstance(this).Clear_repeated();
    }


    private void ByWords(String text) {
        lastTime = "";
        if(StorageDataFromServer.Get_instance().getLogin().getUserLevel() > 0){
            FullText fullText = new FullText(text);
            TextService.Get_instance(this, parentActivity).ByWords(fullText, percent_words);
        } else{
            String result = RepeatsAnalyze.GetInstance().CompareWordsAndroid(text, percent_words);
            Color_data(result);
        }

    }

    @Override
    public void Return_text(String result) {
        Log.d(TAG, "Text result received from server");
        Color_data(result);
    }

    @Override
    public void Return_synonims(ArrayList<String> words) {
        Log.d(TAG, "List of synonims received from server");

        TextView txt_synonims = parentActivity.findViewById(R.id.txt_synonims);
        String text = parentActivity.getResources().getString(R.string.list_of_synonims);;
        if(!words.isEmpty()){
            int size = words.size();
            for(int i = 0; i < size; i++){
                text += " " + words.get(i);
                if(i == size - 1){
                    text += ".";
                } else{
                    text += ",";
                }
            }
        } else{
            text += "-EMPTY-";
        }
        txt_synonims.setText(text);
    }

    @Override
    public void Return_antonims(ArrayList<String> words) {
        Log.d(TAG, "List of antonims received from server");

        TextView txt_antonyms = parentActivity.findViewById(R.id.txt_antonyms);
        String text = parentActivity.getResources().getString(R.string.list_of_antonyms);
        if(!words.isEmpty()){
            int size = words.size();
            for(int i = 0; i < size; i++){
                text += " " + words.get(i);
                if(i == size - 1){
                    text += ".";
                } else{
                    text += ",";
                }
            }
        } else{
            text += "-EMPTY-";
        }
        txt_antonyms.setText(text);
    }
}