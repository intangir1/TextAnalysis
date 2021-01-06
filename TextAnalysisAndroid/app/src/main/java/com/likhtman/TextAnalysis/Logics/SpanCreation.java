package com.likhtman.TextAnalysis.Logics;

import android.graphics.Color;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextPaint;
import android.text.style.BackgroundColorSpan;
import android.text.style.ClickableSpan;
import android.text.style.ForegroundColorSpan;
import android.util.Log;
import android.view.View;

import com.likhtman.TextAnalysis.DialogMessage;
import com.likhtman.TextAnalysis.Fragments.CheckTextFragment;
import com.likhtman.TextAnalysis.Services.MainDB.AntonimService;
import com.likhtman.TextAnalysis.Services.MainDB.SynonimService;
import com.likhtman.TextAnalysis.StorageDataFromServer;

import java.util.HashMap;

public class SpanCreation {
    final String TAG = SpanCreation.this.getClass().getSimpleName();

    private static SpanCreation instance = null;
    CheckTextFragment checkTextFragment;

    HashMap<String, int[]> colors_repeated;
    private int rgb_colors [] = {-40, -40, -40};



    private SpanCreation(){
        colors_repeated = new HashMap<String, int[]>();
    }

    public static SpanCreation GetInstance(CheckTextFragment _checkTextFragment){
        if (instance == null){
            instance = new SpanCreation();
        }
        instance.checkTextFragment = _checkTextFragment;
        return instance;
    }


    public void Clear_repeated(){
        instance.colors_repeated.clear();
        rgb_colors = new int[]{-40, -40, -40};
        Log.d(TAG, "Repeated and colors cleared");
    }


    public SpannableStringBuilder Get_span(String word){
        SpannableStringBuilder spanWithTag = null;

        int count_sign_repeated = (int)word.chars().filter(ch -> ch == '#').count();
        int count_sign_expressions = (int)word.chars().filter(ch -> ch == '@').count();
        int count_sign_archaisms = (int)word.chars().filter(ch -> ch == '_').count();
        int count_sign_slangs = (int)word.chars().filter(ch -> ch == '^').count();
        int count_sign_total = count_sign_repeated + count_sign_expressions + count_sign_archaisms + count_sign_slangs;

        if(count_sign_repeated > 2){            // ostalnie ravni nulu
            spanWithTag = Get_span_multiple(word);
        } else if (count_sign_total == 2 || (count_sign_repeated == 2 && count_sign_expressions == 2)){
            spanWithTag = Get_span_single(word);
        } else{
                word.replaceAll("[#@_^]", "");
                spanWithTag = new SpannableStringBuilder(word);
        }

        Log.d(TAG, "Span for single words created");
        return spanWithTag;
    }


    public SpannableStringBuilder Get_span_expression(String expression) {
        while(expression.chars().filter(ch -> ch == '@' ).count() > 2){
            int start = expression.indexOf("@", 1);
            int end = expression.indexOf("@", start + 1);
            String to_remove = expression.substring(start, end+1);
            expression = expression.replaceAll(to_remove, "");
        }
        expression = expression.replaceAll("[#^_]", "");
        int index_start = expression.indexOf("@");
        int index_end = expression.lastIndexOf("@");
        expression = expression.replaceAll("@", "");

        SpannableStringBuilder span = new SpannableStringBuilder(expression);
        int used_color_rgb[] = Get_new_color("@", "");
        final BackgroundColorSpan bcs = new BackgroundColorSpan(Color.rgb(used_color_rgb[0], used_color_rgb[1], used_color_rgb[2]));
        span.setSpan(bcs, index_start, index_end - 1, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE);
        Log.d(TAG, "Span for expressions created");
        return span;
    }


    private SpannableStringBuilder Get_span_string_regular(String word, String tag, String tag_mark, int index_start, int index_end) {
        SpannableStringBuilder span = new SpannableStringBuilder(word);
        int used_color_rgb[];

        if(colors_repeated.containsKey(tag.toLowerCase())){
            used_color_rgb = colors_repeated.get(tag.toLowerCase());
        } else{
            used_color_rgb = Get_new_color(tag_mark, tag);
        }
        String finalTag = tag;

        ClickableSpan clickableSpan = new ClickableSpan() {
            @Override
            public void onClick(View textView) {
                awesomeButtonClicked(finalTag);
            }
            @Override
            public void updateDrawState(TextPaint ds) {
                ds.bgColor = Color.rgb(used_color_rgb[0], used_color_rgb[1], used_color_rgb[2]);
                ds.setUnderlineText(false);
            }
        };
        span.setSpan(clickableSpan, index_start-1, index_end-1, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE);
        return span;
    }


    private SpannableStringBuilder Get_span_single(String word){
        String[] tag_marks = {"#", "_", "@", "^"};
        String tag_mark = "";
        int index_start = -1;
        for(String temp_tag_mark : tag_marks){
            index_start = word.indexOf(temp_tag_mark) + 1;
            if(index_start > 0){
                tag_mark = temp_tag_mark;
                break;
            }
        }
        int index_end = word.lastIndexOf(tag_mark);

        String tag;
        word = word.replace(tag_mark, "");

        if(tag_mark.equals("#") && word.contains("@")){
            int rift_index_start = word.indexOf("@") + 1;
            int rift_index_end = word.lastIndexOf("@");
            tag = word.substring(rift_index_start, rift_index_end);
            word = word.substring(rift_index_end+1);
            index_end -= tag.length() + 2;
        } else{
            tag = word.substring(index_start-1,index_end-1);
        }

        SpannableStringBuilder span = Get_span_string_regular(word, tag, tag_mark, index_start, index_end);

        return span;
    }


    private SpannableStringBuilder Get_span_string_multiple(SpannableStringBuilder span, String tag, int index_start, int index_end) {
        int used_color_rgb[];

        if(colors_repeated.containsKey(tag)){
            used_color_rgb = colors_repeated.get(tag);
        } else{
            used_color_rgb = Get_new_color("#", tag);
        }
        String finalTag = tag;

        ClickableSpan clickableSpan = new ClickableSpan() {
            @Override
            public void onClick(View textView) {
                awesomeButtonClicked(finalTag);
            }
            @Override
            public void updateDrawState(TextPaint ds) {
                ds.setColor(Color.rgb(used_color_rgb[0], used_color_rgb[1], used_color_rgb[2]));
                ds.setUnderlineText(false);
            }
        };
        span.setSpan(clickableSpan, index_start, index_end - 1, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE);
        return span;
    }


    private SpannableStringBuilder Get_span_multiple(String word){
        String word_current;
        SpannableStringBuilder span = new SpannableStringBuilder(word);
        int index_start = word.indexOf("#");

        do{
            word_current = span.toString();
            int index_end = word_current.indexOf("#", index_start + 1);

            StringBuilder sb = new StringBuilder();
            sb.append(word_current);
            sb.deleteCharAt(index_end);
            sb.deleteCharAt(index_start);
            word_current = sb.toString();

            String tag = word_current.substring(index_start, index_end - 1);


            span.replace(index_start, index_end + 1, tag);

            span = Get_span_string_multiple(span, tag, index_start, index_end);

            index_start = word_current.indexOf("#");

        } while (index_start > -1);

        return span;
    }


    private void awesomeButtonClicked(String text) {
        if(StorageDataFromServer.Get_instance().getLogin().getUserLevel() > 0){
            SynonimService.Get_instance(checkTextFragment.getActivity()).GetWordsBy(text, instance.checkTextFragment);
            Log.d(TAG, "Synonim service invoked");
            AntonimService.Get_instance(checkTextFragment.getActivity()).GetWordsBy(text, instance.checkTextFragment);
            Log.d(TAG, "Antonim service invoked");
        } else{
            DialogMessage.Get_instance(3, checkTextFragment.getContext()).Show_dialog();
            Log.d(TAG, "'Not registered' message shown");
        }
    }


    private int Update_color(int index_rgb) {
        rgb_colors[index_rgb] += 40;
        if(rgb_colors[index_rgb] > 255){
            rgb_colors[index_rgb] -= 255;
        }
        return rgb_colors[index_rgb];
    }


    private int[] Get_new_color(String tag_mark, String tag) {
        int color[];
        switch (tag_mark){
            case "#":
                color = new int[] {255, Update_color(1), 0};
                break;
            case "@":
                color = new int[] {255, 153, 153};
                return color;
            case "^":
                color = new int[] {Update_color(0), 0, 255};
                break;
            case "_":
                color = new int[] {0, 255, Update_color(2)};
                break;
            default:
                color = new int[] {0, 0, 0};
                Log.e(TAG, "New color creation error");
                break;
        }
        colors_repeated.put(tag.toLowerCase(), color);
        return color;
    }

}
