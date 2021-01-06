import { Injectable } from '@angular/core';
import { IRelationalWordsRepository } from '../baseInterfaces/IRelationalWordsRepository';
@Injectable()
export abstract class IAntonimRepository implements IRelationalWordsRepository {
    abstract GetAllWords(): void;
    abstract GetWordsBy(word:string): void;
    abstract PostWord(synonimWord:string, antonimWord:string): void;
    abstract PutWord(word_to_replace:string, word:string): void;
    abstract InsertWord(word_to_find:string, word_to_add:string): void
    abstract DeleteWord(wordToRemove:string): void;
    abstract DeleteCollection(word:string): void;
}