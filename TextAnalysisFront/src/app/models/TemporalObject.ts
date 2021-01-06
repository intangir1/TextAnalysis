export class TemporalObject {
    public constructor(
        public mongoId: string="",
        public action: string="",
        public connectionWord: string="",
        public inputedWord: string="",
        public type: string=""
    ) { }

    public Equals(str: string) : boolean { 
        return this.action === str || this.connectionWord === str || this.inputedWord === str;
    } 
}