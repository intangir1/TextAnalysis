package com.likhtman.TextAnalysis.Callbacks;

import com.likhtman.TextAnalysis.Model.TemporalObject;
import com.likhtman.TextAnalysis.Model.TemporalObjectForIrregular;

public interface CallbacksForTemporalObjects {
    void Return_normal_object(TemporalObject temporalObject);
    void Return_irregular_object(TemporalObjectForIrregular temporalObjectForIrregular);
}
