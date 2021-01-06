export class IrregularObject {
    
    public constructor(
        public mongoId: string="",
        public first: string="",
        public second: string="",
        public third: string=""
    ) { }

    public Equals(str: string) : boolean { 
        return this.first === str || this.second === str || this.third === str;
    }
}