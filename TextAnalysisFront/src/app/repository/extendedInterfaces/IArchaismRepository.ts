import { Injectable } from '@angular/core';
import { ISingleWordsRepository } from '../baseInterfaces/ISingleWordsRepository';
@Injectable()
export abstract class IArchaismRepository implements ISingleWordsRepository{
    abstract GetAllWords(): void;
    abstract PostWord(word:string): void;
    abstract PutWord(word_to_replace:string, word:string): void;
    abstract DeleteWord(wordToRemove:string): void;
    abstract DeleteCollection(): void;
}