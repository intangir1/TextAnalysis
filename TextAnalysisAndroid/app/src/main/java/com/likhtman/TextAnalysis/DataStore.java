package com.likhtman.TextAnalysis;

public final class DataStore {
    public static final String baseUrl="http://10.0.2.2:54534/";
    public static final String mainUrl=baseUrl+"api/";

    public static final String loginUrl=baseUrl+"token";

    public static final String usersUrl = mainUrl+"users";

    public static final String byWords=mainUrl+"AnalyseFullText/";
    public static final String byAllSentencesWords=mainUrl+"allSentencesWords/";

    public static final String byArchaism=mainUrl+"archaisms/";
    public static final String bySlangs=mainUrl+"slangs/";
    public static final String byIrregulars=mainUrl+"irregulars/";
    public static final String byExpressions=mainUrl+"expressions/";
    public static final String byAntonims=mainUrl+"antonims/";
    public static final String bySynonim=mainUrl+"synonims/";

    public static final String byTempArchaism=mainUrl+"temp_archaisms/";
    public static final String byTempSlangs=mainUrl+"temp_slangs/";
    public static final String byTempIrregulars=mainUrl+"temp_irregulars/";
    public static final String byTempExpressions=mainUrl+"temp_expressions/";
    public static final String byTempAntonims=mainUrl+"temp_antonims/";
    public static final String byTempSynonim=mainUrl+"temp_synonims/";
    public static final String byAllTemporalWords=mainUrl+"getAllTemporalWords/";
}
