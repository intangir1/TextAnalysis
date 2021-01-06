package com.likhtman.TextAnalysis.Logics;

import android.util.Log;

import java.util.Arrays;
import java.util.LinkedList;

public class TextSplitter {
    final String TAG = TextSplitter.this.getClass().getSimpleName();

    private static TextSplitter instance = null;

    private TextSplitter(){

    }

    public static TextSplitter Get_instance(){
        if(instance == null){
            instance = new TextSplitter();
        }
        return instance;
    }

    public String[] Split_text(String text){
        String[] splitted = text.split("(?<=([!?.]))");
        Arrays.parallelSetAll(splitted, (i) -> splitted[i].replaceAll("( +)"," ").trim());
        if (splitted[splitted.length - 1].equals("")) {
            Arrays.copyOf(splitted, splitted.length - 1);
        }
        if(Arrays.stream(splitted).allMatch(sentence -> sentence.length() > 1)){
            return splitted;
        }
        return Clean_splitted(splitted);
    }


    private String[] Clean_splitted(String[] splitted) {
        LinkedList<String> splitted_real = new LinkedList<String>();
        for(int i = 0; i < splitted.length; i++){

            if(i > 0 && splitted[i].length() == 1){
                String value = splitted_real.pollLast() + splitted[i];
                splitted_real.add(value);
            } else{
                splitted_real.add(splitted[i]);
            }
        }

        String[] result = splitted_real.toArray(new String[0]);
        Log.d(TAG, "Text splitted and cleaned");
        return result;
    }

    public String[] splitSentence(String sentence, String pattern)
    {
        sentence = sentence.replaceAll(pattern, "");
        String[] words = sentence.split(" ");
        Log.d(TAG, "Sentence splitted");
        return words;
    }

}
