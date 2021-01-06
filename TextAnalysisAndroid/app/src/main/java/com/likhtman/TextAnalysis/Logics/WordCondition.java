package com.likhtman.TextAnalysis.Logics;

import android.util.Log;

import java.util.regex.Pattern;

public class WordCondition {
    final String TAG = WordCondition.this.getClass().getSimpleName();

    private static WordCondition instance = null;

    private WordCondition(){

    }


    public static WordCondition Get_instance(){
        if(instance == null){
            instance = new WordCondition();
        }
        return instance;
    }


    private static String prefixesPattern = "^(re|dis|over|un|mis|out|de|fore|inter|pre|sub|trans|under)";
    private static String suffixesPattern = "(ing|ed|ies)$";
    private static String rgx_acceptable_endings = "(see|add|ass|ess|all|ill|ell|oll|iss|ree|ess|uzz|oss|lee|iff)$";
    private static String exceptionsPattern = "(go|be|do)";
    private static String exceptionsEndingPattern = "(thing)$";


    public String GetClearWord(String word)
    {
        word = word.replaceAll("#", "");


        String temp_word;

        temp_word = word.replaceAll(prefixesPattern, "");

        if(!temp_word.equals(word) && WordAcccepted(temp_word)){
            word = temp_word;
        }

        temp_word = word.replaceAll(suffixesPattern, "");

        if(!temp_word.equals(word) && !Pattern.compile(exceptionsEndingPattern).matcher(word).find() && WordAcccepted(temp_word)){
            word = temp_word;
            if (word.length() > 2)
            {
                String ending = word.substring(word.length() - 2);
                if (ending.chars().distinct().count() == 1 && !Pattern.compile(rgx_acceptable_endings).matcher(word).find())
                {
                    word = word.substring(0, word.length() - 1);
                }
            }
        }
        return word;
    }

    public boolean WordAcccepted(String word){
        if(word!=null && !word.equals("") && (word.length() > 2 || Pattern.compile(exceptionsPattern).matcher(word).find())){
            Log.d(TAG, "The word '" + word + "' accepted");
            return true;
        }
        Log.d(TAG, "The word '" + word + "' not accepted");
        return false;
    }

    

}
