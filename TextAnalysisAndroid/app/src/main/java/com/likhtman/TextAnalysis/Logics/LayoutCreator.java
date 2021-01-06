package com.likhtman.TextAnalysis.Logics;

import android.app.Activity;
import android.graphics.Color;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.DragEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.SpinnerAdapter;
import android.widget.TextView;

import androidx.appcompat.widget.AppCompatImageView;

import com.github.florent37.expansionpanel.ExpansionHeader;
import com.github.florent37.expansionpanel.ExpansionLayout;
import com.google.android.material.textfield.TextInputLayout;
import com.likhtman.TextAnalysis.Activities.MainActivity;
import com.likhtman.TextAnalysis.Fragments.AddWordsFragment;
import com.likhtman.TextAnalysis.Model.FullText;
import com.likhtman.TextAnalysis.Model.IrregularObject;
import com.likhtman.TextAnalysis.R;
import com.likhtman.TextAnalysis.Services.TemporalDB.TempAntonimService;
import com.likhtman.TextAnalysis.Services.TemporalDB.TempArchaismService;
import com.likhtman.TextAnalysis.Services.TemporalDB.TempExpressionService;
import com.likhtman.TextAnalysis.Services.TemporalDB.TempIrregularService;
import com.likhtman.TextAnalysis.Services.TemporalDB.TempSlangService;
import com.likhtman.TextAnalysis.Services.TemporalDB.TempSynonimService;

import fr.ganfra.materialspinner.MaterialSpinner;

import static com.likhtman.TextAnalysis.Utils.ResizeUtils.dpToPx;

public class LayoutCreator {
    final String TAG = LayoutCreator.this.getClass().getSimpleName();

    private static LayoutCreator instance = null;
    private ExpansionLayout expansionLayout;
    private Activity activity;

    private LayoutCreator(){

    }

    public static LayoutCreator GetInstance(Activity _activity){
        if(instance == null){
            instance = new LayoutCreator();
        }
        instance.expansionLayout = null;
        instance.activity = _activity;
        return instance;
    }


    public ExpansionHeader Create_expansion_header(String label) {

        final ExpansionHeader expansionHeader = new ExpansionHeader(instance.activity);
        expansionHeader.setBackgroundColor(Color.WHITE);
        expansionHeader.setPadding(dpToPx(instance.activity, 16), dpToPx(instance.activity, 8), dpToPx(instance.activity, 16), dpToPx(instance.activity, 8));

        final RelativeLayout layout = new RelativeLayout(instance.activity);
        expansionHeader.addView(layout, ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT); //equivalent to addView(linearLayout)

        //image
        final ImageView expansionIndicator = new AppCompatImageView(instance.activity);
        expansionIndicator.setImageResource(R.drawable.ic_expansion_header_indicator_grey_24dp);
        final RelativeLayout.LayoutParams imageLayoutParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);
        imageLayoutParams.addRule(RelativeLayout.ALIGN_PARENT_RIGHT);
        imageLayoutParams.addRule(RelativeLayout.CENTER_VERTICAL);
        layout.addView(expansionIndicator, imageLayoutParams);

        //label
        final TextView text = new TextView(instance.activity);
        text.setText(label.toUpperCase());
        text.setTextColor(Color.parseColor("#3E3E3E"));
//        text.setBackgroundColor(Color.YELLOW);
        text.setPadding(0, dpToPx(instance.activity, 10), 0, dpToPx(instance.activity, 10));

        final RelativeLayout.LayoutParams textLayoutParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);
        textLayoutParams.addRule(RelativeLayout.ALIGN_PARENT_LEFT);
        textLayoutParams.addRule(RelativeLayout.CENTER_VERTICAL);

        layout.addView(text, textLayoutParams);

        expansionHeader.setExpansionHeaderIndicator(expansionIndicator);

        return expansionHeader;
    }


    public ExpansionLayout Build_layout_synonims(View expl_synonims_view, AddWordsFragment context, String label, SpinnerAdapter synonims_adapter) {
        expansionLayout = (ExpansionLayout) expl_synonims_view;
        Button btn_post_relative = ((Button)expl_synonims_view.findViewById(R.id.btn_post_relative));
        Button btn_put_relative = ((Button)expl_synonims_view.findViewById(R.id.btn_put_relative));
        Button btn_insert_relative = ((Button)expl_synonims_view.findViewById(R.id.btn_insert_relative));
        EditText editText = (EditText) ((TextInputLayout) expl_synonims_view.findViewById(R.id.etxt_relative)).getEditText();
        MaterialSpinner spinner = expl_synonims_view.findViewById(R.id.spinner_relative);

        btn_post_relative.setEnabled(false);
        btn_put_relative.setEnabled(false);
        btn_insert_relative.setEnabled(false);

        ((TextInputLayout)expl_synonims_view.findViewById(R.id.etxt_relative)).setHint(label);
        spinner.setAdapter(synonims_adapter);

        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                if(position < 0){
                    btn_put_relative.setEnabled(false);
                    btn_insert_relative.setEnabled(false);
                } else {
                    String text = editText.getText().toString();
                    if(text.length() > 0 && !text.trim().equals("")){
                        btn_put_relative.setEnabled(true);
                        btn_insert_relative.setEnabled(true);
                    }
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });



        editText.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String text = editText.getText().toString();
                if(text.length() > 0 && !text.trim().equals("")){
                    btn_post_relative.setEnabled(true);
                    if(spinner.getSelectedItemPosition() > 0){
                        btn_put_relative.setEnabled(true);
                        btn_insert_relative.setEnabled(true);
                    }
                } else{
                    btn_post_relative.setEnabled(false);
                    btn_put_relative.setEnabled(false);
                    btn_insert_relative.setEnabled(false);
                }
            }
        });

        btn_post_relative.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                TempSynonimService.Get_instance(context, instance.activity).PostWord(fullText);
            }
        });



        btn_put_relative.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                String connection_word = spinner.getSelectedItem().toString();
                TempSynonimService.Get_instance(context, instance.activity).PutWord(connection_word, fullText);
            }
        });



        btn_insert_relative.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                String connection_word = spinner.getSelectedItem().toString();
                TempSynonimService.Get_instance(context, instance.activity).InsertWord(connection_word, fullText);
            }
        });
        return expansionLayout;
    }


    public ExpansionLayout Build_layout_antonims(View expl_antonims_view, AddWordsFragment context, String label, SpinnerAdapter antonims_adapter) {
        expansionLayout = (ExpansionLayout) expl_antonims_view;
        Button btn_post_relative = ((Button)expl_antonims_view.findViewById(R.id.btn_post_relative));
        Button btn_put_relative = ((Button)expl_antonims_view.findViewById(R.id.btn_put_relative));
        Button btn_insert_relative = ((Button)expl_antonims_view.findViewById(R.id.btn_insert_relative));
        EditText editText = (EditText) ((TextInputLayout) expl_antonims_view.findViewById(R.id.etxt_relative)).getEditText();
        MaterialSpinner spinner = expl_antonims_view.findViewById(R.id.spinner_relative);

        btn_post_relative.setEnabled(false);
        btn_put_relative.setEnabled(false);
        btn_insert_relative.setEnabled(false);

        ((TextInputLayout)expl_antonims_view.findViewById(R.id.etxt_relative)).setHint(label);
        spinner.setAdapter(antonims_adapter);

        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                String text = editText.getText().toString();
                if(position < 0 || text.length() == 0 || text.trim().equals("")){
                    btn_post_relative.setEnabled(false);
                    btn_put_relative.setEnabled(false);
                    btn_insert_relative.setEnabled(false);
                } else {
                    btn_post_relative.setEnabled(true);
                    btn_put_relative.setEnabled(true);
                    btn_insert_relative.setEnabled(true);
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        editText.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String text = editText.getText().toString();
                if(text.length() > 0 && !text.trim().equals("") && spinner.getSelectedItemPosition() > 0){
                    btn_post_relative.setEnabled(true);
                    btn_put_relative.setEnabled(true);
                    btn_insert_relative.setEnabled(true);
                } else{
                    btn_post_relative.setEnabled(false);
                    btn_put_relative.setEnabled(false);
                    btn_insert_relative.setEnabled(false);
                }
            }
        });

        btn_post_relative.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                String connection_word = spinner.getSelectedItem().toString();
                TempAntonimService.Get_instance(context, instance.activity).PostWord(connection_word, fullText);
            }
        });

        btn_put_relative.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                String connection_word = spinner.getSelectedItem().toString();
                TempAntonimService.Get_instance(context, instance.activity).PutWord(connection_word, fullText);
            }
        });

        btn_insert_relative.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                String connection_word = spinner.getSelectedItem().toString();
                TempAntonimService.Get_instance(context, instance.activity).InsertWord(connection_word, fullText);
            }
        });
        return expansionLayout;
    }


    public ExpansionLayout Build_layout_slangs(View expl_slangs_view, AddWordsFragment context, String label, SpinnerAdapter slangs_adapter) {
        expansionLayout = (ExpansionLayout) expl_slangs_view;
        ((TextInputLayout)expl_slangs_view.findViewById(R.id.etxt_others)).setHint(label);

        MaterialSpinner spinner = ((MaterialSpinner)expl_slangs_view.findViewById(R.id.spinner_others));
        Button btn_post_others = ((Button)expl_slangs_view.findViewById(R.id.btn_post_others));
        Button btn_put_others = ((Button)expl_slangs_view.findViewById(R.id.btn_put_others));
        EditText editText = (EditText) ((TextInputLayout) expl_slangs_view.findViewById(R.id.etxt_others)).getEditText();

        btn_post_others.setEnabled(false);
        btn_put_others.setEnabled(false);

        editText.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String text = editText.getText().toString();
                if(text.length() > 0 && !text.trim().equals("")){
                    btn_post_others.setEnabled(true);
                    if(spinner.getSelectedItemPosition() > 0){
                        btn_put_others.setEnabled(true);
                    }
                } else {
                    btn_post_others.setEnabled(false);
                    btn_put_others.setEnabled(false);
                }
            }
        });

        spinner.setAdapter(slangs_adapter);

        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                if(position < 0){
                    btn_put_others.setEnabled(false);
                } else{
                    String text = editText.getText().toString();
                    if(text.length() > 0 && !text.trim().equals("")){
                        btn_put_others.setEnabled(true);
                    }
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        btn_post_others.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                TempSlangService.Get_instance(context, instance.activity).PostWord(fullText);
            }
        });

        btn_put_others.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                String connection_word = spinner.getSelectedItem().toString();
                TempSlangService.Get_instance(context, instance.activity).PutWord(connection_word, fullText);
            }
        });
        return expansionLayout;
    }


    public ExpansionLayout Build_layout_archaisms(View expl_archaisms_view, AddWordsFragment context, String label, SpinnerAdapter archaisms_adapter) {
        expansionLayout = (ExpansionLayout) expl_archaisms_view;
        ((TextInputLayout)expl_archaisms_view.findViewById(R.id.etxt_others)).setHint(label);

        MaterialSpinner spinner = ((MaterialSpinner)expl_archaisms_view.findViewById(R.id.spinner_others));
        Button btn_post_others = ((Button)expl_archaisms_view.findViewById(R.id.btn_post_others));
        Button btn_put_others = ((Button)expl_archaisms_view.findViewById(R.id.btn_put_others));
        EditText editText = (EditText) ((TextInputLayout) expl_archaisms_view.findViewById(R.id.etxt_others)).getEditText();

        btn_post_others.setEnabled(false);
        btn_put_others.setEnabled(false);

        editText.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String text = editText.getText().toString();
                if(text.length() > 0 && !text.trim().equals("")){
                    btn_post_others.setEnabled(true);
                    if(spinner.getSelectedItemPosition() > 0){
                        btn_put_others.setEnabled(true);
                    }
                } else {
                    btn_post_others.setEnabled(false);
                    btn_put_others.setEnabled(false);
                }
            }
        });

        spinner.setAdapter(archaisms_adapter);

        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                if(position < 0){
                    btn_put_others.setEnabled(false);
                } else{
                    String text = editText.getText().toString();
                    if(text.length() > 0 && !text.trim().equals("")){
                        btn_put_others.setEnabled(true);
                    }
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        btn_post_others.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                TempArchaismService.Get_instance(context, instance.activity).PostWord(fullText);
            }
        });

        btn_put_others.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                String connection_word = spinner.getSelectedItem().toString();
                TempArchaismService.Get_instance(context, instance.activity).PutWord(connection_word, fullText);
            }
        });
        return expansionLayout;
    }


    public ExpansionLayout Build_layout_expressions(View expl_expressions_view, AddWordsFragment context, String label, SpinnerAdapter expressions_adapter) {
        expansionLayout = (ExpansionLayout) expl_expressions_view;
        ((TextInputLayout)expl_expressions_view.findViewById(R.id.etxt_others)).setHint(label);

        MaterialSpinner spinner = ((MaterialSpinner)expl_expressions_view.findViewById(R.id.spinner_others));
        Button btn_post_others = ((Button)expl_expressions_view.findViewById(R.id.btn_post_others));
        Button btn_put_others = ((Button)expl_expressions_view.findViewById(R.id.btn_put_others));
        EditText editText = (EditText) ((TextInputLayout) expl_expressions_view.findViewById(R.id.etxt_others)).getEditText();

        btn_post_others.setEnabled(false);
        btn_put_others.setEnabled(false);

        editText.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String text = editText.getText().toString();
                if(text.length() > 0 && !text.trim().equals("")){
                    btn_post_others.setEnabled(true);
                    if(spinner.getSelectedItemPosition() > 0){
                        btn_put_others.setEnabled(true);
                    }
                } else {
                    btn_post_others.setEnabled(false);
                    btn_put_others.setEnabled(false);
                }
            }
        });

        spinner.setAdapter(expressions_adapter);

        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                if(position < 0){
                    btn_put_others.setEnabled(false);
                } else{
                    String text = editText.getText().toString();
                    if(text.length() > 0 && !text.trim().equals("")){
                        btn_put_others.setEnabled(true);
                    }
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        btn_post_others.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                TempExpressionService.Get_instance(context, instance.activity).PostWord(fullText);
            }
        });

        btn_put_others.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input = editText.getText().toString();
                FullText fullText = new FullText(input);
                String connection_word = spinner.getSelectedItem().toString();
                TempExpressionService.Get_instance(context, instance.activity).PutWord(connection_word, fullText);
            }
        });
        return expansionLayout;
    }


    public ExpansionLayout Build_layout_irregulars(View expl_irregulars_view, AddWordsFragment context, String label, SpinnerAdapter irregulars_adapter) {
        expansionLayout = (ExpansionLayout) expl_irregulars_view;
        ((TextInputLayout)expl_irregulars_view.findViewById(R.id.etxt_irregulars_first)).setHint("First Form");
        ((TextInputLayout)expl_irregulars_view.findViewById(R.id.etxt_irregulars_second)).setHint("Second Form");
        ((TextInputLayout)expl_irregulars_view.findViewById(R.id.etxt_irregulars_third)).setHint("Third Form");

        MaterialSpinner spinner = ((MaterialSpinner)expl_irregulars_view.findViewById(R.id.spinner_irregulars));
        Button btn_post_irregulars = ((Button)expl_irregulars_view.findViewById(R.id.btn_post_irregulars));
        Button btn_put_irregulars = ((Button)expl_irregulars_view.findViewById(R.id.btn_put_irregulars));
        EditText editText_first = (EditText) ((TextInputLayout) expl_irregulars_view.findViewById(R.id.etxt_irregulars_first)).getEditText();
        EditText editText_second = (EditText) ((TextInputLayout) expl_irregulars_view.findViewById(R.id.etxt_irregulars_second)).getEditText();
        EditText editText_third = (EditText) ((TextInputLayout) expl_irregulars_view.findViewById(R.id.etxt_irregulars_third)).getEditText();

        btn_post_irregulars.setEnabled(false);
        btn_put_irregulars.setEnabled(false);

        editText_first.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String text_first = editText_first.getText().toString();
                String text_second = editText_second.getText().toString();
                String text_third = editText_third.getText().toString();
                if(text_first.length() > 0 && !text_first.trim().equals("") && text_second.length() > 0 && !text_second.trim().equals("") && text_third.length() > 0 && !text_third.trim().equals("") ){
                    btn_post_irregulars.setEnabled(true);
                    if(spinner.getSelectedItemPosition() > 0){
                        btn_put_irregulars.setEnabled(true);
                    }
                } else {
                    btn_post_irregulars.setEnabled(false);
                    btn_put_irregulars.setEnabled(false);
                }
            }
        });

        editText_second.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String text_first = editText_first.getText().toString();
                String text_second = editText_second.getText().toString();
                String text_third = editText_third.getText().toString();
                if(text_first.length() > 0 && !text_first.trim().equals("") && text_second.length() > 0 && !text_second.trim().equals("") && text_third.length() > 0 && !text_third.trim().equals("") ){
                    btn_post_irregulars.setEnabled(true);
                    if(spinner.getSelectedItemPosition() > 0){
                        btn_put_irregulars.setEnabled(true);
                    }
                } else {
                    btn_post_irregulars.setEnabled(false);
                    btn_put_irregulars.setEnabled(false);
                }
            }
        });

        editText_third.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                String text_first = editText_first.getText().toString();
                String text_second = editText_second.getText().toString();
                String text_third = editText_third.getText().toString();
                if(text_first.length() > 0 && !text_first.trim().equals("") && text_second.length() > 0 && !text_second.trim().equals("") && text_third.length() > 0 && !text_third.trim().equals("") ){
                    btn_post_irregulars.setEnabled(true);
                    if(spinner.getSelectedItemPosition() > 0){
                        btn_put_irregulars.setEnabled(true);
                    }
                } else {
                    btn_post_irregulars.setEnabled(false);
                    btn_put_irregulars.setEnabled(false);
                }
            }
        });

        spinner.setAdapter(irregulars_adapter);

        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                if(position < 0){
                    btn_put_irregulars.setEnabled(false);
                } else{
                    String text_first = editText_first.getText().toString();
                    String text_second = editText_second.getText().toString();
                    String text_third = editText_third.getText().toString();
                    if(text_first.length() > 0 && !text_first.trim().equals("") && text_second.length() > 0 && !text_second.trim().equals("") && text_third.length() > 0 && !text_third.trim().equals("") ){
                        btn_put_irregulars.setEnabled(true);
                    }
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        btn_post_irregulars.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input_one =  editText_first.getText().toString();
                String input_two =  editText_second.getText().toString();
                String input_three =  editText_third.getText().toString();

                IrregularObject irregularObject = new IrregularObject(input_one, input_two, input_three);

                TempIrregularService.Get_instance(context, instance.activity).PostWord(irregularObject);
            }
        });

        btn_put_irregulars.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String input_one =  editText_first.getText().toString();
                String input_two =  editText_second.getText().toString();
                String input_three =  editText_third.getText().toString();

                IrregularObject irregularObject = new IrregularObject(input_one, input_two, input_three);

                String connection_word =  spinner.getSelectedItem().toString();

                TempIrregularService.Get_instance(context, instance.activity).PutWord(connection_word, irregularObject);
            }
        });
        return expansionLayout;
    }

}
