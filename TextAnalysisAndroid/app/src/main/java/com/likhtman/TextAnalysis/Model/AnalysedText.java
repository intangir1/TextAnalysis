package com.likhtman.TextAnalysis.Model;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashSet;

public class AnalysedText {
    private HashSet<String> repeated = new HashSet<String>();
    private HashSet<String> archaisms = new HashSet<String>();
    private HashSet<String> slangs = new HashSet<String>();
    private HashSet<String> irregulars = new HashSet<String>();
    private HashSet<String> expressions = new HashSet<String>();

    public AnalysedText() { }

    public AnalysedText(
            HashSet<String> tmpRepeated,
            HashSet<String> tmpArchaisms,
            HashSet<String> tmpSlangs,
            HashSet<String> tmpIrregulars,
            HashSet<String> tmpExpressions)
    {
        repeated.addAll(tmpRepeated);
        archaisms.addAll(tmpArchaisms);
        slangs.addAll(tmpSlangs);
        irregulars.addAll(tmpIrregulars);
        expressions.addAll(tmpExpressions);
    }


    public AnalysedText(String downloadedText){
        try {
            JSONObject obj = new JSONObject(downloadedText);

            JSONArray j_repeated = obj.getJSONArray("repeated");
            for(int i = 0; i < j_repeated.length(); i++){
                repeated.add(j_repeated.getString(i));
            }

            JSONArray j_archaisms = obj.getJSONArray("archaisms");
            for(int i = 0; i < j_archaisms.length(); i++){
                archaisms.add(j_archaisms.getString(i));
            }


            JSONArray j_slangs = obj.getJSONArray("slangs");
            for(int i = 0; i < j_slangs.length(); i++){
                slangs.add(j_slangs.getString(i));
            }


            JSONArray j_irregulars = obj.getJSONArray("irregulars");
            for(int i = 0; i < j_irregulars.length(); i++){
                irregulars.add(j_irregulars.getString(i));
            }


            JSONArray j_expressions = obj.getJSONArray("expressions");
            for(int i = 0; i < j_expressions.length(); i++){
                expressions.add(j_expressions.getString(i));
            }

        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public AnalysedText(HashSet<String> tmpRepeated)
    {
        repeated.addAll(tmpRepeated);
    }


    public HashSet<String> getRepeated() {
        return repeated;
    }

    public void setRepeated(HashSet<String> _repeated) {
        this.repeated = _repeated;
    }

    public HashSet<String> getArchaisms() {
        return archaisms;
    }

    public void setArchaisms(HashSet<String> _archaisms) {
        this.archaisms = _archaisms;
    }

    public HashSet<String> getSlangs() {
        return slangs;
    }

    public void setSlangs(HashSet<String> _slangs) {
        this.slangs = _slangs;
    }

    public HashSet<String> getIrregulars() {
        return irregulars;
    }

    public void setIrregulars(HashSet<String> _irregulars) {
        this.irregulars = _irregulars;
    }

    public HashSet<String> getExpressions() {
        return expressions;
    }

    public void setExpressions(HashSet<String> _expressions) {
        this.expressions = _expressions;
    }

    @Override
    public String toString() {
        return "AnalysedText{" +
                "repeated=" + repeated +
                ", archaisms=" + archaisms +
                ", slangs=" + slangs +
                ", irregulars=" + irregulars +
                ", expressions=" + expressions +
                '}';
    }
}
