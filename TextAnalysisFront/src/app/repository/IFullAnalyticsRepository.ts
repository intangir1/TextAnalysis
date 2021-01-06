import { FullText } from '../models/FullText';

export interface IFullAnalyticsRepository{
    AnalyseFullText(fullText:FullText, limit:number): void;
    CompareAllSentencesWords(text:string[]):void;
}