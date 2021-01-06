import { IrregularObject } from './IrregularObject';

export class TemporalObjectForIrregular {
    
    public constructor(
        public mongoId: string="",
        public action: string="",
        public connectionWord: string="",
        public inputedWord: IrregularObject=null,
        public type: string=""
    ) { }

    public Equals(str: string) : boolean { 
        return this.action === str || this.connectionWord === str || this.inputedWord.Equals(str);
    } 
}