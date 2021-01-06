import { Injectable } from '@angular/core';
import { IrregularObject } from 'src/app/models/IrregularObject';

@Injectable()
export abstract class IIrregularsRepository {
    abstract GetAllWords(): void;
    abstract PostWord(word:IrregularObject, mongoId:string): void;
    abstract PutWord(word_to_replace:string, word:IrregularObject, mongoId:string): void;
    abstract DeleteWord(wordToRemove:string): void;
    abstract DeleteCollection(): void;
}