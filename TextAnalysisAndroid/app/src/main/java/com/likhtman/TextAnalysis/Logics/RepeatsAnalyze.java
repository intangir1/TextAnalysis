package com.likhtman.TextAnalysis.Logics;


import android.text.TextUtils;
import android.util.Log;

import com.likhtman.TextAnalysis.Entity.TwoSenteces;
import com.likhtman.TextAnalysis.Model.AnalysedText;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Dictionary;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class RepeatsAnalyze {
    final String TAG = RepeatsAnalyze.this.getClass().getSimpleName();

    private HashSet<String> repeatedContains;
    private static RepeatsAnalyze instance = null;

    private RepeatsAnalyze(){}

    public static RepeatsAnalyze GetInstance(){
        if (instance == null){
            instance = new RepeatsAnalyze();
        }
        instance.repeatedContains=new HashSet<String>();
        return instance;
    }

    public String CompareAllSentencesWords(String[] text)
    {
        int size = 2;
        if (text.length > 1)
        {
            size = text.length;
        }

        for (int placeInTextArray = 0; placeInTextArray < size - 1; placeInTextArray++)
        {
            TwoSenteces twoSenteces;
            if (text.length == 1)
            {
                twoSenteces = new TwoSenteces(text[placeInTextArray], "");
            }
            else
            {
                twoSenteces = new TwoSenteces(text[placeInTextArray], text[placeInTextArray + 1]);
            }
            AnalysedText analysedText = AnalyseTwoSentences(twoSenteces);
            if (analysedText.getRepeated().size() > 0)
            {
                String[] repeated = analysedText.getRepeated().toArray(new String[0]);
                for (int placeInRepeatedArray = 0; placeInRepeatedArray < repeated.length; placeInRepeatedArray++)
                {
                    text[placeInTextArray] = ReplaceInsideStringAndroid(text[placeInTextArray], repeated[placeInRepeatedArray]);
                    if (text.length > 1)
                    {
                        text[placeInTextArray + 1] = ReplaceInsideStringAndroid(text[placeInTextArray + 1], repeated[placeInRepeatedArray]);
                    }
                }
            }
        }
        String result = TextUtils.join(" ", text);
        Log.d(TAG, "'By sentences' check complete");
        return result;
    }




    private AnalysedText AnalyseTwoSentences(TwoSenteces twoSenteces)
    {
        String pattern = "[:;,!?.]";
        String[] firstSentence = TextSplitter.Get_instance().splitSentence(twoSenteces.getFirstSentence(), pattern);
        String[] firstSentenceClear = Clean_words_array(firstSentence);
        String[] clearTwoArraysSplitted = firstSentenceClear;
        String[] secondSentence = { };
        String[] secondSentenceClear = { };

        if (twoSenteces.getSecondSentence()!=null && !twoSenteces.getSecondSentence().trim().equals(""))
        {
            secondSentence = TextSplitter.Get_instance().splitSentence(twoSenteces.getSecondSentence(), pattern);
            secondSentenceClear = Clean_words_array(secondSentence);
            List<String> tmpArray = new ArrayList<>();
            Collections.addAll(tmpArray,clearTwoArraysSplitted);
            Collections.addAll(tmpArray,secondSentenceClear);
            clearTwoArraysSplitted = tmpArray.toArray(new String[tmpArray.size()]);
        }
        repeatedContains.addAll(StringFullIntersectBoolean(clearTwoArraysSplitted));

        AnalysedText analysedText = new AnalysedText(repeatedContains);
        Log.d(TAG, "AnalyzedText object created");
        return analysedText;
    }





    private HashSet<String> StringFullIntersectBoolean(String[] words)
    {
        if (words != null && words.length > 0)
        {
            words = Arrays.stream(words).map(word -> word.toLowerCase()).toArray(String[]::new);
        }
        HashMap<String, Boolean> booleanDictionary = new HashMap<String, Boolean>();


        for (int placeInWordsArray = 0; placeInWordsArray < words.length; placeInWordsArray++)
        {
            if (WordCondition.Get_instance().WordAcccepted(words[placeInWordsArray]))
            {
                String[] finalWords = words;
                int finalPlaceInWordsArray = placeInWordsArray;

                String dictKey = booleanDictionary.entrySet().stream()
                        .filter(element -> finalWords[finalPlaceInWordsArray].toLowerCase().contains(element.getKey()))
                        .map(element -> element.getKey())
                        .collect(Collectors.joining());

                if (!booleanDictionary.containsKey(words[placeInWordsArray].toLowerCase()) && (dictKey == null || dictKey.equals("")))
                {
                    booleanDictionary.put(words[placeInWordsArray], false);
                }
                else if (booleanDictionary.containsKey(words[placeInWordsArray].toLowerCase()))
                {
                    booleanDictionary.put(words[placeInWordsArray].toLowerCase(), true);
                }
                else
                {
                    booleanDictionary.put(dictKey.toLowerCase(), true);
                }
            }
        }

        String[] repeats = booleanDictionary
                .entrySet()
                .stream()
                .filter(x -> x.getValue()==true)
                .map(element -> element.getKey())
                .toArray(String[]::new);

        return new HashSet<>(Arrays.asList(repeats));
    }

    private String ReplaceInsideStringAndroid(String stringToReplace, String replaceBy)
    {
        String result = "";
        Pattern mypattern = Pattern.compile(replaceBy, Pattern.CASE_INSENSITIVE);
        Matcher mymatcher= mypattern.matcher(stringToReplace);
        while (mymatcher.find()) {
            if (!stringToReplace.contains("#" + mymatcher.group() + "#")){
                result = stringToReplace.replaceAll(mymatcher.group(),"#" + mymatcher.group() + "#");
                stringToReplace = result;
            }
            else
                result = stringToReplace;
        }
        result = stringToReplace;
        return result;
    }



    private String[] Clean_words_array(String[] words) {
        String[] words_clean = Arrays.stream(words).map(word -> WordCondition.Get_instance().GetClearWord(word)).toArray(String[]::new);
        return words_clean;
    }


    public String CompareWordsAndroid(String text, int percent){
        String pattern = "[:;,!?.]";
        String[] words = TextSplitter.Get_instance().splitSentence(text, pattern);
        String[] wordsClear = Clean_words_array(words);
        repeatedContains.addAll(StringFullIntersectPercent(wordsClear, percent));

        AnalysedText analysedText = new AnalysedText(repeatedContains);
        if (analysedText.getRepeated().size() > 0)
        {
            String[] repeated = analysedText.getRepeated().toArray(new String[0]);
            for (int placeInRepeatedArray = 0; placeInRepeatedArray < repeated.length; placeInRepeatedArray++)
            {
                text = ReplaceInsideStringAndroid(text, repeated[placeInRepeatedArray]);
            }
        }
        Log.d(TAG, "'By words' check complete");
        return text;
    }




    private HashSet<String> StringFullIntersectPercent(String[] words, int percent)
    {
        if (words != null && words.length > 0)
        {
            words = Arrays.stream(words).map(word -> word.toLowerCase()).toArray(String[]::new);
        }
        HashMap<String, Integer> booleanDictionary = new HashMap<String, Integer>();


        for (int placeInWordsArray = 0; placeInWordsArray < words.length; placeInWordsArray++)
        {
            if (WordCondition.Get_instance().WordAcccepted(words[placeInWordsArray]))
            {
                String[] finalWords = words;
                int finalPlaceInWordsArray = placeInWordsArray;

                String dictKey = booleanDictionary.entrySet().stream()
                        .filter(element -> finalWords[finalPlaceInWordsArray].toLowerCase().contains(element.getKey()))
                        .map(element -> element.getKey())
                        .collect(Collectors.joining());

                if (!booleanDictionary.containsKey(words[placeInWordsArray].toLowerCase()) && (dictKey == null || dictKey.equals("")))
                {
                    booleanDictionary.put(words[placeInWordsArray], 1);
                }
                else if (booleanDictionary.containsKey(words[placeInWordsArray].toLowerCase()))
                {
                    booleanDictionary.put(words[placeInWordsArray].toLowerCase(), booleanDictionary.get(words[placeInWordsArray].toLowerCase())+1);
                }
                else
                {
                    booleanDictionary.put(dictKey.toLowerCase(), booleanDictionary.get(dictKey.toLowerCase().toLowerCase())+1);
                }
            }
        }
        final int length = words.length;
        String[] repeats = booleanDictionary
                .entrySet()
                .stream()
                .filter(x -> (double) (((int)x.getValue()* 100) / length) >= percent)
                .map(element -> element.getKey())
                .toArray(String[]::new);

        return new HashSet<>(Arrays.asList(repeats));
    }
}