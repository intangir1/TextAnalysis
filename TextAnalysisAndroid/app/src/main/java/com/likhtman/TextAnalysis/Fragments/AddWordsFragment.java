package com.likhtman.TextAnalysis.Fragments;

import android.app.Activity;
import android.graphics.Color;
import android.os.Bundle;

import androidx.fragment.app.Fragment;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Toast;

import com.github.florent37.expansionpanel.ExpansionHeader;
import com.github.florent37.expansionpanel.ExpansionLayout;

import com.github.florent37.expansionpanel.viewgroup.ExpansionLayoutCollection;
import com.likhtman.TextAnalysis.Callbacks.CallbacksForIrregularWords;
import com.likhtman.TextAnalysis.Callbacks.CallbacksForNonRelationalWords;
import com.likhtman.TextAnalysis.Callbacks.CallbacksForRelational;
import com.likhtman.TextAnalysis.Callbacks.CallbacksForTemporalObjects;
import com.likhtman.TextAnalysis.Logics.LayoutCreator;
import com.likhtman.TextAnalysis.Model.IrregularObject;
import com.likhtman.TextAnalysis.Model.TemporalObject;
import com.likhtman.TextAnalysis.Model.TemporalObjectForIrregular;
import com.likhtman.TextAnalysis.R;
import com.likhtman.TextAnalysis.Services.MainDB.AntonimService;
import com.likhtman.TextAnalysis.Services.MainDB.ArchaismService;
import com.likhtman.TextAnalysis.Services.MainDB.ExpressionService;
import com.likhtman.TextAnalysis.Services.MainDB.IrregularService;
import com.likhtman.TextAnalysis.Services.MainDB.SlangService;
import com.likhtman.TextAnalysis.Services.MainDB.SynonimService;

import java.util.ArrayList;


public class AddWordsFragment extends Fragment implements CallbacksForIrregularWords, CallbacksForRelational,CallbacksForNonRelationalWords, CallbacksForTemporalObjects {
    final String TAG = AddWordsFragment.this.getClass().getSimpleName();

    private ViewGroup dynamicLayoutContainer;
    private Activity parent_activity;

    private View expl_synonims_view;
    private View expl_antonims_view;
    private View expl_irregulars_view;
    private View expl_slangs_view;
    private View expl_archaisms_view;
    private View expl_expressions_view;


    private ArrayAdapter<String> synonims_adapter;
    private ArrayAdapter<String> antonims_adapter;
    private ArrayAdapter<String> irregulars_adapter;
    private ArrayAdapter<String> slangs_adapter;
    private ArrayAdapter<String> archaisms_adapter;
    private ArrayAdapter<String> expressions_adapter;


    private ArrayList<String> relational_list;
    private ArrayList<String> irregulars_list;
    private ArrayList<String> slangs_list;
    private ArrayList<String> archaisms_list;
    private ArrayList<String> expressions_list;


    private void Init_logics() {
        relational_list = new ArrayList<String>();
        irregulars_list = new ArrayList<String>();
        slangs_list = new ArrayList<String>();
        archaisms_list = new ArrayList<String>();
        expressions_list = new ArrayList<String>();

        synonims_adapter = new ArrayAdapter<String>(parent_activity, android.R.layout.simple_spinner_dropdown_item, relational_list);
        antonims_adapter = new ArrayAdapter<String>(parent_activity, android.R.layout.simple_spinner_dropdown_item, relational_list);
        irregulars_adapter = new ArrayAdapter<String>(parent_activity, android.R.layout.simple_spinner_dropdown_item, irregulars_list);
        slangs_adapter = new ArrayAdapter<String>(parent_activity, android.R.layout.simple_spinner_dropdown_item, slangs_list);
        archaisms_adapter = new ArrayAdapter<String>(parent_activity, android.R.layout.simple_spinner_dropdown_item, archaisms_list);
        expressions_adapter = new ArrayAdapter<String>(parent_activity, android.R.layout.simple_spinner_dropdown_item, expressions_list);
        synonims_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        antonims_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        irregulars_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        slangs_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        archaisms_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        expressions_adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = null;
        view = inflater.inflate(R.layout.fragment_add_words, container, false);
        return view;
    }


    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        parent_activity = getActivity();
        dynamicLayoutContainer = view.findViewById(R.id.dynamicLayoutContainer);

        Init_logics();

        expl_synonims_view = LayoutInflater.from(getContext()).inflate(R.layout.expansion_layout_relative, dynamicLayoutContainer,false);
        expl_antonims_view = LayoutInflater.from(getContext()).inflate(R.layout.expansion_layout_relative, dynamicLayoutContainer,false);
        expl_irregulars_view = LayoutInflater.from(getContext()).inflate(R.layout.expansion_layout_irregulars, dynamicLayoutContainer,false);
        expl_slangs_view = LayoutInflater.from(getContext()).inflate(R.layout.expansion_layout_others, dynamicLayoutContainer,false);
        expl_archaisms_view = LayoutInflater.from(getContext()).inflate(R.layout.expansion_layout_others, dynamicLayoutContainer,false);
        expl_expressions_view = LayoutInflater.from(getContext()).inflate(R.layout.expansion_layout_others, dynamicLayoutContainer,false);

        final ExpansionLayout ex_synonims = Create_dynamic_layout("synonims");
        final ExpansionLayout ex_antonims = Create_dynamic_layout("antonims");
        final ExpansionLayout ex_slangs = Create_dynamic_layout("slangs");
        final ExpansionLayout ex_archaisms = Create_dynamic_layout("archaisms");
        final ExpansionLayout ex_expressions = Create_dynamic_layout("expressions");
        final ExpansionLayout ex_irregulars = Create_dynamic_layout("irregulars");

        final ExpansionLayoutCollection expansionLayoutCollection = new ExpansionLayoutCollection();
        expansionLayoutCollection.add(ex_synonims).add(ex_antonims).add(ex_archaisms).add(ex_slangs).add(ex_expressions).add(ex_irregulars);
        expansionLayoutCollection.openOnlyOne(true);

    }


    private ExpansionLayout Create_dynamic_layout(String label) {
        ExpansionHeader expansionHeader = LayoutCreator.GetInstance(parent_activity).Create_expansion_header(label);
        dynamicLayoutContainer.addView(expansionHeader, ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);

        ExpansionLayout expansionLayout = null;

        AddWordsFragment context = this;
        CallbacksForRelational callbacksForRelational = this;
        CallbacksForNonRelationalWords callbacksForNonRelationalWords = this;
        CallbacksForIrregularWords callbacksForIrregularWords = this;


        switch (label){
            case "synonims":
                Log.d(TAG, "Synonims layer creation started");

                expansionLayout = LayoutCreator.GetInstance(parent_activity).Build_layout_synonims(expl_synonims_view, context, label, synonims_adapter);
                expansionLayout.addListener(new ExpansionLayout.Listener() {
                    @Override
                    public void onExpansionChanged(ExpansionLayout expansionLayout, boolean expanded) {
                        if(relational_list.isEmpty() && expanded){
                            SynonimService.Get_instance(parent_activity).GetAllWords(callbacksForRelational);
                        }
                    }
                });
                break;
            case "antonims":
                Log.d(TAG, "Antonims layer creation started");

                expansionLayout = LayoutCreator.GetInstance(parent_activity).Build_layout_antonims(expl_antonims_view, context, label, antonims_adapter);
                expansionLayout.addListener(new ExpansionLayout.Listener() {
                    @Override
                    public void onExpansionChanged(ExpansionLayout expansionLayout, boolean expanded) {
                        if(relational_list.isEmpty() && expanded){
                            AntonimService.Get_instance(parent_activity).GetAllWords(callbacksForRelational);
                        }
                    }
                });
                break;
            case "slangs":
                Log.d(TAG, "Slangs layer creation started");

                expansionLayout = LayoutCreator.GetInstance(parent_activity).Build_layout_slangs(expl_slangs_view, context, label, slangs_adapter);
                expansionLayout.addListener(new ExpansionLayout.Listener() {
                    @Override
                    public void onExpansionChanged(ExpansionLayout expansionLayout, boolean expanded) {
                        if(slangs_list.isEmpty() && expanded){
                            SlangService.Get_instance(callbacksForNonRelationalWords, parent_activity).GetAllWords();
                        }
                    }
                });
                break;
            case "archaisms":
                Log.d(TAG, "Archaisms layer creation started");

                expansionLayout = LayoutCreator.GetInstance(parent_activity).Build_layout_archaisms(expl_archaisms_view, context, label, archaisms_adapter);
                expansionLayout.addListener(new ExpansionLayout.Listener() {
                    @Override
                    public void onExpansionChanged(ExpansionLayout expansionLayout, boolean expanded) {
                        if(archaisms_list.isEmpty() && expanded){
                            ArchaismService.Get_instance(callbacksForNonRelationalWords, parent_activity).GetAllWords();
                        }
                    }
                });
                break;
            case "expressions":
                Log.d(TAG, "Expressions layer creation started");

                expansionLayout = LayoutCreator.GetInstance(parent_activity).Build_layout_expressions(expl_expressions_view, context, label, expressions_adapter);
                expansionLayout.addListener(new ExpansionLayout.Listener() {
                    @Override
                    public void onExpansionChanged(ExpansionLayout expansionLayout, boolean expanded) {
                        if(expressions_list.isEmpty() && expanded){
                            ExpressionService.Get_instance(callbacksForNonRelationalWords, parent_activity).GetAllWords();
                        }
                    }
                });
                break;
            case "irregulars":
                Log.d(TAG, "Irregulars layer creation started");

                expansionLayout = LayoutCreator.GetInstance(parent_activity).Build_layout_irregulars(expl_irregulars_view, context, label, irregulars_adapter);
                expansionLayout.addListener(new ExpansionLayout.Listener() {
                    @Override
                    public void onExpansionChanged(ExpansionLayout expansionLayout, boolean expanded) {
                        if(irregulars_list.isEmpty() && expanded){
                            IrregularService.Get_instance(callbacksForIrregularWords, parent_activity).GetAllWords();
                        }
                    }
                });
                break;
            default:
                Log.e(TAG, "Wrong string for layer creation received");
                return null;
        }

        expansionLayout.setBackgroundColor(Color.parseColor("#EEEEEE"));

        try{
            dynamicLayoutContainer.addView(expansionLayout, ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
            Log.d(TAG, "Dynamic layout adding successful");
        } catch (Exception ex){
            Log.e(TAG, "Dynamic layout adding error");
            ex.getMessage();
        }

        expansionHeader.setExpansionLayout(expansionLayout);

        return expansionLayout;
    }


    @Override
    public void Return_irregulars_array(ArrayList<IrregularObject> irregulars) {
        for(IrregularObject irregular : irregulars){
            irregulars_list.add(irregular.Get_all_forms());
        }
        irregulars_adapter.notifyDataSetChanged();
    }


    private void Add_to_list_nonrelational(ArrayList<String> array, ArrayList<String> list) {
        for(String element : array){
            list.add(element);
        }
    }

    @Override
    public void Return_array(ArrayList<String> array, int option) {
        switch (option){
            case 1:
                Add_to_list_nonrelational(array, slangs_list);
                slangs_adapter.notifyDataSetChanged();
                Log.d(TAG, "List of slangs received from server");
                return;
            case 2:
                Add_to_list_nonrelational(array, archaisms_list);
                archaisms_adapter.notifyDataSetChanged();
                Log.d(TAG, "List of archaisms received from server");
                return;
            case 3:
                Add_to_list_nonrelational(array, expressions_list);
                expressions_adapter.notifyDataSetChanged();
                Log.d(TAG, "List of expressions received from server");
                return;
            default:
                Log.e(TAG, "List of received words from server has wrong 'id' number");
                return;
        }
    }


    @Override
    public void Return_word(String word) {

    }

    @Override
    public void Return_normal_object(TemporalObject temporalObject) {
        Log.d(TAG, "Temporal object returned from server");

        String input = temporalObject.getInputedWord();
        String type = temporalObject.getType();
        String connection = temporalObject.getConnectionWord();


        String message = "The word " + input + " was proposed as ";
        if(connection == null || connection.length() == 0 || connection.trim().equals("")){
            message += "a new " + type;
        } else{
            message += "an update for " + connection;
        }

        Toast.makeText(parent_activity, message, Toast.LENGTH_LONG).show();
    }

    @Override
    public void Return_irregular_object(TemporalObjectForIrregular temporalObjectForIrregular) {
        Log.d(TAG, "Irregular object returned from server");

        String input = temporalObjectForIrregular.getInputedWord().Get_all_forms();
        String connection = temporalObjectForIrregular.getConnectionWord();

        String message = "The verb " + input + " was proposed as ";
        if(connection == null || connection.length() == 0 || connection.trim().equals("")){
            message += "a new irregular verb";
        } else{
            message += "an update for " + connection;
        }

        Toast.makeText(parent_activity, message, Toast.LENGTH_LONG).show();
    }


    @Override
    public void Return_all_words(ArrayList<ArrayList<String>> all_words) {
        Log.d(TAG, "Relational words list received from server");

        for(ArrayList<String> list : all_words){
            relational_list.add(list.get(0));
        }
        synonims_adapter.notifyDataSetChanged();
        antonims_adapter.notifyDataSetChanged();
    }

}