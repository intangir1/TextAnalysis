export const environment = {
  production: false
};

export const baseUrl="http://127.0.0.1:54534/";
export const mainUrl=baseUrl+"api/";

export const loginUrl=baseUrl+"token";

export const usersUrl = mainUrl+"users/";
export const usersAnaliticsUrl = mainUrl+"users_analitics/";

export const byAnalyseFullText=mainUrl+"AnalyseFullText/";
export const byAllSentencesWords=mainUrl+"allSentencesWords/";

export const byArchaism=mainUrl+"archaisms/";
export const bySlangs=mainUrl+"slangs/";
export const byIrregulars=mainUrl+"irregulars/";
export const byExpressions=mainUrl+"expressions/";
export const byAntonims=mainUrl+"antonims/";
export const bySynonim=mainUrl+"synonims/";

export const byTempArchaism=mainUrl+"temp_archaisms/";
export const byTempSlangs=mainUrl+"temp_slangs/";
export const byTempIrregulars=mainUrl+"temp_irregulars/";
export const byTempExpressions=mainUrl+"temp_expressions/";
export const byTempAntonims=mainUrl+"temp_antonims/";
export const byTempSynonim=mainUrl+"temp_synonims/";
export const byAllTemporalWords=mainUrl+"getAllTemporalWords/";