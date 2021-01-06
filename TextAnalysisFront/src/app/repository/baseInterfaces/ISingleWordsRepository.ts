export interface ISingleWordsRepository {
    GetAllWords(): void;
    PostWord(word:string, mongoId:string): void;
    PutWord(word_to_replace:string, word:string, mongoId:string): void;
    DeleteWord(wordToRemove:string): void;
    DeleteCollection(): void;
}