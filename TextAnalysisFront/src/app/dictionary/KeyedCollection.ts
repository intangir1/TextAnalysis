import { IKeyedCollection } from './IKeyedCollection';
import { Injectable } from '@angular/core';


@Injectable({
    providedIn: 'root'
  })
export class KeyedCollection<T> implements IKeyedCollection<T> {
    private items: { [index: string]: T } = {};
 
    private count: number = 0;
 
    public ContainsKey(key: string): boolean {
        return this.items.hasOwnProperty(key);
    }
 
    public Count(): number {
        return this.count;
    }
 
    public Add(key: string, value: T) {
        if(!this.items.hasOwnProperty(key))
             this.count++;
 
        this.items[key] = value;
    }
 
    public Remove(key: string): T {
        var val = this.items[key];
        delete this.items[key];
        this.count--;
        return val;
    }
 
    public Item(key: string): T {
        return this.items[key];
    }
 
    public Keys(): string[] {
        var keySet: string[] = [];
 
        for (var prop in this.items) {
            if (this.items.hasOwnProperty(prop)) {
                keySet.push(prop);
            }
        }
 
        return keySet;
    }
 
    public Values(): T[] {
        var values: T[] = [];
 
        for (var prop in this.items) {
            if (this.items.hasOwnProperty(prop)) {
                values.push(this.items[prop]);
            }
        }
 
        return values;
    }

    public WordIncludesKey(bigWord: string): string {
        let tmpKey =  this.Keys().filter(element => bigWord.includes(element))[0];
        if (tmpKey!==null && tmpKey!==undefined && tmpKey!==""){
            return tmpKey;
        } else{
            return null;
        }
    }

    public KeyIncludesWord(smallWord: string): string {
        let tmpKey = this.Keys().filter(element => element.includes(smallWord))[0];
        if (tmpKey!==null && tmpKey!==undefined && tmpKey!==""){
            return smallWord;
        } else {
            return null;
        }
    }

    public GetCommonPart(word: string): string {
        // if(word.length<3){
        //     return null
        // }
        let ifContains = this.ContainsKey(word);
        if (ifContains===true){
            return word;
        } else {
            let tmpKey = this.WordIncludesKey(word);
            if (tmpKey!==null && tmpKey!==undefined && tmpKey!==""){
                return tmpKey;
            } else {
                tmpKey = this.KeyIncludesWord(word);
                if (tmpKey!==null && tmpKey!==undefined && tmpKey!==""){
                    return tmpKey;
                } else {
                    return null;
                }
            }
        }
    }
}