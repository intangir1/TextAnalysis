package com.likhtman.TextAnalysis.Logics;

import android.content.Context;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;

import com.github.florent37.expansionpanel.ExpansionLayout;
import com.likhtman.TextAnalysis.R;

public class DrawableResize {
    private static DrawableResize instance = null;
    private static Context context;

    private DrawableResize(){

    }

    public static DrawableResize GetInstance(Context _context){
        if(instance == null){
            instance = new DrawableResize();
        }
        context = _context;
        return instance;
    }

    public Drawable Get_resize(Drawable drawable, int width, int height){
        Bitmap bitmap = ((BitmapDrawable) drawable).getBitmap();
        Drawable drawable_final = new BitmapDrawable(context.getResources(), Bitmap.createScaledBitmap(bitmap, width, height, true));
        return drawable_final;
    }
}
