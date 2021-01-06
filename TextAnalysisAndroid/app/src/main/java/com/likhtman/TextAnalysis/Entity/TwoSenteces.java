package com.likhtman.TextAnalysis.Entity;

public class TwoSenteces {
    private String _firstSentence;
    private String _secondSentence;

    public TwoSenteces() {

    }

    public TwoSenteces(String tmpFirstSentence, String tmpSecondSentence) {
        setFirstSentence(tmpFirstSentence);
        setSecondSentence(tmpSecondSentence);
    }

    public String getFirstSentence() {
        return _firstSentence;
    }

    public void setFirstSentence(String _firstSentence) {
        this._firstSentence = _firstSentence;
    }

    public String getSecondSentence() {
        return _secondSentence;
    }

    public void setSecondSentence(String _secondSentence) {
        this._secondSentence = _secondSentence;
    }


}
