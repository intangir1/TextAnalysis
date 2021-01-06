import { Component, OnInit, ElementRef, ViewChild, Renderer2, HostListener } from '@angular/core';
import { FullText } from 'src/app/models/FullText';
import { Store } from 'src/app/redux/store';
import { NgRedux } from 'ng2-redux';
import { Unsubscribe } from 'redux';
import { LogService } from 'src/app/services/log.service';
import { LocalAnalyzerService } from 'src/app/services/local-analyzer.service';
import { TextService } from 'src/app/services/ApiConnections/text.service';
import { KeyedCollection } from 'src/app/dictionary/KeyedCollection';
import { SynonimService } from 'src/app/services/ApiConnections/MainDb/synonim.service';
import { AntonimService } from 'src/app/services/ApiConnections/MainDb/antonim.service';
import { WordConditionService } from 'src/app/services/word-condition.service';
import { MatSlider } from '@angular/material/slider';
import { ActionType } from 'src/app/redux/action-type';
import { Action } from 'src/app/redux/action';

@Component({
  selector: 'app-check-text',
  templateUrl: './check-text.component.html',
  styleUrls: ['./check-text.component.css']
})
export class CheckTextComponent implements OnInit {
  @ViewChild('template') myTemplate: ElementRef;

  private tempStr:string="";
  public value:number=1;
  
  private colorsArray:number[]=[-40,-40,-40];
  
  public option: string='By Sentence';
  public options: string[] = ['By Words', 'By Sentence'];

  public textToCheck=new FullText();

  public synonims: string[]=[];
  public antonims: string[]=[];

  public repeated: string[];
  public text:string;
  
  public analizedText: string="";
  public textToReturn:string;
  private innerTexts = new KeyedCollection<string>();

  private lastTime:string = ""
  private isLoggedIn:boolean;

  private unsubscribe:Unsubscribe;
  
  constructor(
    private synonimService:SynonimService,
    private antonimService:AntonimService,
    private elementRef:ElementRef,
    private renderer:Renderer2,
    private localAnalyzerService:LocalAnalyzerService,
    private textService:TextService,
    private wordConditionService:WordConditionService,
    private redux:NgRedux<Store>,
    private logger: LogService) { }
    
  public ngOnInit(): void {
    this.unsubscribe = this.redux.subscribe(()=>{
      let logged = this.redux.getState().isLoggedIn;
      if (this.isLoggedIn!==logged){
        if (this.isLoggedIn!==undefined && this.isLoggedIn!==null){
          this.clearText();
        }
        this.isLoggedIn = logged;
      }
      if (this.redux.getState().text!=null && this.redux.getState().text.length>0){
        this.text = this.redux.getState().text;
        this.logger.debug("Returned Text: ", this.text);
        this.highlight();
      }
      if (this.redux.getState().synonims!=null){
        this.synonims = this.redux.getState().synonims;
        this.logger.debug("Returned synonims: ", this.synonims.join(", "));
      }
      if (this.redux.getState().antonims!=null){
        this.antonims = this.redux.getState().antonims;
        this.logger.debug("Returned antonims: ", this.antonims.join(", "));
      }
    });
  }
  
  public ByChange(){
    if(this.textToCheck.textToCheck===""){
      this.myTemplate.nativeElement.innerHTML = "";
      const action: Action={type:ActionType.CompareAllSentencesWords, payload:""};
      this.redux.dispatch(action);
      this.logger.debug("Compare All Sentences Words: ", "");
    } else {
      this.analizedText = this.textToCheck.textToCheck;
      let last = this.textToCheck.textToCheck.slice(-1);
      if(last==="!" || last==="?" || last==="."){
        if(this.analizedText.replace(/[^a-z0-9-]/g, '') === this.lastTime.replace(/[^a-z0-9-]/g, '')){
          return;
        }
        this.lastTime = this.analizedText;
        if (this.option === 'By Words'){
          this.ByWords();
        } else{
          this.BySentence();
        }
      }
    }
  }
  
  public BySentence(){
    this.innerTexts = new KeyedCollection<string>();
    let regexp = new RegExp(/([!?.]+)/);
    let splitted = this.textToCheck.textToCheck.split(regexp);
    splitted = splitted.map(string => string.trim())
    if(splitted[splitted.length-1]===''){
      splitted.pop();
    }
    for (let i=0;i<splitted.length-1;i++){
      if(regexp.test(splitted[i+1])){
        splitted[i]=splitted[i]+splitted[i+1];
        splitted.splice(i+1,1);
      } 
    }
    if (!this.isLoggedIn){
      this.localAnalyzerService.CompareAllSentencesWords(splitted);
    } else{
      this.textService.CompareAllSentencesWords(splitted);
    }
  }

  public ByWords(){
    this.innerTexts = new KeyedCollection<string>();
    if(this.textToCheck!==null && this.textToCheck.textToCheck!==null&& this.textToCheck.textToCheck!==undefined && this.textToCheck.textToCheck.trim()!==""){
      if (!this.isLoggedIn){
        this.localAnalyzerService.AnalyseFullText(this.textToCheck, this.value)
      } else{
        this.textService.AnalyseFullText(this.textToCheck, this.value);
      }
    }
  }

  public highlight() {
    this.textToReturn=this.text;
    this.myTemplate.nativeElement.innerHTML = this.textToReturn;
    this.CreateWorkingStylesAndFunctions();
    this.colorsArray=[-40,-40,-40];
  }

  
  public CreateColor(placeInColorsArray:number):number {
    this.colorsArray[placeInColorsArray]=this.colorsArray[placeInColorsArray]+40;
    if (this.colorsArray[placeInColorsArray]>255){
      this.colorsArray[placeInColorsArray]=this.colorsArray[placeInColorsArray]-255;
    }
    return this.colorsArray[placeInColorsArray];
  }


  private CustomColorNonRepeated(color_red:number, color_green:number, color_blue:number, word_for_check:string) : string{
    let myColor:string;
    if (this.innerTexts.ContainsKey(word_for_check.toLowerCase())){
      myColor = this.innerTexts.Item(word_for_check.toLowerCase());
    } else {
      myColor = "rgb(" + color_red + "," + color_green + "," + color_blue + ")";
      this.innerTexts.Add(word_for_check.toLowerCase(), myColor);
    }
    return myColor;
  }

  private CustomColorRepeated(color_red:number, color_green:number, color_blue:number, word_for_check:string) : string{
    let myColor:string;
    if (this.wordConditionService.WordAcccepted(word_for_check.toLowerCase())){
      let commonWord = this.GetCommonWord(word_for_check.toLowerCase());
      if (commonWord===false){
        myColor = "rgb(" + color_red + "," + color_green + "," + color_blue + ")";
        this.innerTexts.Add(word_for_check.toLowerCase(), myColor);
      } else {
        myColor = this.innerTexts.Item(commonWord);
      }
    }
    return myColor;
  }
  
  private GetColor(withNoDigits:string, res:any):string {
    let tmpColor:string;
    if (withNoDigits!==""){
      switch(withNoDigits){
        case "expressions":
          tmpColor = "rgb(" + 255 + "," + 153 + "," + 153 + ")";
          break;
        case "slangs":
          tmpColor = this.CustomColorNonRepeated(this.CreateColor(0), 0, 255, res.innerText);
          break;
        case "archaisms":
          tmpColor = this.CustomColorNonRepeated(0, 255, this.CreateColor(2), res.innerText);
          break;
        case "repeated":
          tmpColor = this.CustomColorRepeated(255, this.CreateColor(1), 0, res.innerText);
          break;
        default:
          tmpColor = this.CustomColorRepeated(255, this.CreateColor(1), 0, withNoDigits);
          break;
      }
    }
    return tmpColor;
  }
  
  public CreateWorkingStylesAndFunctions(){
    var elem = this.elementRef.nativeElement.querySelectorAll('span');
    if (elem) {
      elem.forEach(res => {
        if (res.id!==null && res.id!==undefined && res.id!==""){
          res.id = res.id.replace("+1","");
          var withNoDigits = res.id.replace(/[0-9]/g, '');
          if (withNoDigits==="expressions"){
            res.innerHTML=res.innerText;
          }
          let color:string = this.GetColor(withNoDigits, res);
          if (color!==null && color!==undefined && color!==""){
            this.renderer.setStyle(res, 'background-color', color);
          }
          if (this.isLoggedIn){
            this.renderer.setStyle(res, 'cursor','pointer');
            res.addEventListener("mouseenter", function(){
              if(withNoDigits!=="" && withNoDigits!=="expressions" && withNoDigits!=="slangs" && withNoDigits!=="archaisms" && withNoDigits!=="repeated"){
                this.FindSynonims(withNoDigits);
              } else{
                this.FindSynonims(res.innerText);
              }
            }.bind(this), false)
          }
        }
      })
    }
  }
  
  private GetCommonWord(word:string){
    let smallWord:string = this.innerTexts.GetCommonPart(word);
    if (smallWord===null){
      return false;
    } else {
      return smallWord;
    }
  }
  
  
  FindSynonims(str){
    if(str!==this.tempStr){
      this.logger.debug("FindSynonims: ", str);
      this.synonimService.GetWordsBy(str);
      this.antonimService.GetWordsBy(str);
      this.tempStr=str;
    }
  }
  
  pitch(event: any) {
    this.value=event.value;
  }

  formatLabel(val: number) {
    return val;
  }

  sliderValue=this.value;
  @ViewChild(MatSlider) slider: MatSlider;
  @HostListener('window:mouseup', ['$event'])
    mouseUp(event) {
      if(this.sliderValue!=this.value){
        this.ByWords();
      }
    }
  @HostListener('window:mousedown', ['$event'])
    mouseDown(event) {
      this.sliderValue=this.value
    }


  public radioChange(event){
    if (event.value === 'By Words'){
      this.ByWords();
    } else{
      this.BySentence();
    }
  }

  public clearText(){
    this.textToCheck.textToCheck="";
    this.myTemplate.nativeElement.innerHTML = "";
    if (this.redux.getState().text!==null && this.redux.getState().text!==undefined && this.redux.getState().text!==""){
     const action: Action={type:ActionType.CompareAllSentencesWords, payload:""};
     this.redux.dispatch(action);
     this.logger.debug("Compare All Sentences Words: ", "");
    }
  }

  public ngOnDestroy(): void {
    this.unsubscribe();
  }
}