export class AnalysedText {
    private _repeated: string[];
    private _archaisms: string[];
    private _slangs: string[];
    private _irregulars: string[];
    private _expressions: string[];

    public constructor(
        repeated?: string[],
        archaisms?: string[],
        slangs?: string[],
        irregulars?: string[],
        expressions?: string[]
    ) {
        this.repeated = repeated;
        this.archaisms = archaisms;
        this.slangs = slangs;
        this.irregulars = irregulars;
        this.expressions = expressions;
     }

    get repeated():string[]{
        console.debug("get repeated: " + JSON.stringify(this._repeated));
        return this._repeated;
    }

    set repeated(val){
        this._repeated=val;
        console.debug("set repeated: " + JSON.stringify(this._repeated));
    }

    get archaisms():string[]{
        console.debug("get archaisms: " + JSON.stringify(this._archaisms));
        return this._archaisms;
    }

    set archaisms(val){
        this._archaisms=val;
        console.debug("set archaisms: " + JSON.stringify(this._archaisms));
    }

    get slangs():string[]{
        console.debug("get slangs: " + JSON.stringify(this._slangs));
        return this._slangs;
    }

    set slangs(val){
        this._slangs=val;
        console.debug("set slangs: " + JSON.stringify(this._slangs));
    }

    get irregulars():string[]{
        console.debug("get irregulars: " + JSON.stringify(this._irregulars));
        return this._irregulars;
    }

    set irregulars(val){
        this._irregulars=val;
        console.debug("set irregulars: " + JSON.stringify(this._irregulars));
    }

    get expressions():string[]{
        console.debug("get expressions: " + JSON.stringify(this._expressions));
        return this._expressions;
    }

    set expressions(val){
        this._expressions=val;
        console.debug("set expressions: " + JSON.stringify(this._expressions));
    }
}