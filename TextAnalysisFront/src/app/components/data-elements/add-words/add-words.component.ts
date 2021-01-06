import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

import { TempAntonimService } from 'src/app/services/ApiConnections/TemporalDb/tempAntonim.service';
import { TempSynonimService } from 'src/app/services/ApiConnections/TemporalDb/tempSynonim.service';

import { AntonimService } from 'src/app/services/ApiConnections/MainDb/antonim.service';
import { SynonimService } from 'src/app/services/ApiConnections/MainDb/synonim.service';
import { NgRedux } from 'ng2-redux';
import { Store } from 'src/app/redux/store';
import { Unsubscribe } from 'redux';
import { IrregularObject } from 'src/app/models/IrregularObject';
import { ArchaismService } from 'src/app/services/ApiConnections/MainDb/archaism.service';
import { ExpressionService } from 'src/app/services/ApiConnections/MainDb/expression.service';
import { IrregularService } from 'src/app/services/ApiConnections/MainDb/irregular.service';
import { SlangService } from 'src/app/services/ApiConnections/MainDb/slang.service';
import { TempArchaismService } from 'src/app/services/ApiConnections/TemporalDb/tempArchaism.service';
import { TempExpressionService } from 'src/app/services/ApiConnections/TemporalDb/tempExpression.service';
import { TempSlangService } from 'src/app/services/ApiConnections/TemporalDb/tempSlang.service';
import { TempIrregularService } from 'src/app/services/ApiConnections/TemporalDb/tempIrregular.service';
import { ConfigurationService } from 'src/app/services/configuration.service';
import { IIrregularsRepository } from 'src/app/repository/extendedInterfaces/IIrregularsRepository';
import { IAntonimRepository } from 'src/app/repository/extendedInterfaces/IAntonimRepository';
import { ISynonimRepository } from 'src/app/repository/extendedInterfaces/ISynonimRepository';
import { IArchaismRepository } from 'src/app/repository/extendedInterfaces/IArchaismRepository';
import { IExpressionRepository } from 'src/app/repository/extendedInterfaces/IExpressionRepository';
import { ISlangRepository } from 'src/app/repository/extendedInterfaces/ISlangRepository';
import { GetAllRealDataService } from 'src/app/services/ApiConnections/get-all-real-data.service';
import { LogService } from 'src/app/services/log.service';
import { HttpClient } from '@angular/common/http';

export function AuthenticationAntonimFactory(http: HttpClient, redux: NgRedux<Store>, logger: LogService, configService: ConfigurationService) {
  if (configService.IsAdmin() === false) {
     return new TempAntonimService(http, redux, logger);
  }
  return new AntonimService(http, redux, logger);
}

export function AuthenticationSynonimFactory(http: HttpClient, redux: NgRedux<Store>, logger: LogService, configService: ConfigurationService) {
  if (configService.IsAdmin() === false) {
     return new TempSynonimService(http, redux, logger);
  }
  return new SynonimService(http, redux, logger);
}

export function AuthenticationArchaismFactory(http: HttpClient, redux: NgRedux<Store>, logger: LogService, configService: ConfigurationService) {
  if (configService.IsAdmin() === false) {
     return new TempArchaismService(http, redux, logger);
  }
  return new ArchaismService(http, redux, logger);
}

export function AuthenticationExpressionFactory(http: HttpClient, redux: NgRedux<Store>, logger: LogService, configService: ConfigurationService) {
  if (configService.IsAdmin() === false) {
     return new TempExpressionService(http, redux, logger);
  }
  return new ExpressionService(http, redux, logger);
}

export function AuthenticationSlangFactory(http: HttpClient, redux: NgRedux<Store>, logger: LogService, configService: ConfigurationService) {
  if (configService.IsAdmin() === false) {
     return new TempSlangService(http, redux, logger);
  }
  return new SlangService(http, redux, logger);
}

export function AuthenticationIrregularFactory(http: HttpClient, redux: NgRedux<Store>, logger: LogService, configService: ConfigurationService) {
  if (configService.IsAdmin() === false) {
     return new TempIrregularService(http, redux, logger);
  }
  return new IrregularService(http, redux, logger);
}

@Component({
  selector: 'app-add-words',
  templateUrl: './add-words.component.html',
  styleUrls: ['./add-words.component.css'],
  providers: [
    {provide: IAntonimRepository, useFactory: AuthenticationAntonimFactory, deps: [HttpClient, NgRedux, LogService, ConfigurationService]},
    {provide: ISynonimRepository, useFactory: AuthenticationSynonimFactory, deps: [HttpClient, NgRedux, LogService, ConfigurationService]},
    {provide: IArchaismRepository, useFactory: AuthenticationArchaismFactory, deps: [HttpClient, NgRedux, LogService, ConfigurationService]},
    {provide: IExpressionRepository, useFactory: AuthenticationExpressionFactory, deps: [HttpClient, NgRedux, LogService, ConfigurationService]},
    {provide: ISlangRepository, useFactory: AuthenticationSlangFactory, deps: [HttpClient, NgRedux, LogService, ConfigurationService]},
    {provide: IIrregularsRepository, useFactory: AuthenticationIrregularFactory, deps: [HttpClient, NgRedux, LogService, ConfigurationService]}
  ]
})
export class AddWordsComponent implements OnInit, OnDestroy {
  private unsubscribe:Unsubscribe;
  synonims_antonims: string[] = ['Synonim', 'Antonim'];
  otherTypes: string[] = ['Archaism', 'Expression', 'Slang'];
  irregularType: string = 'Irregular';

  selectedSynonimConnection: string;
  selectedAntonimConnection: string;


  selectedArchaismConnection: string;
  selectedExpressionConnection: string;
  selectedSlangConnection: string;
  selectedIrregularConnection: string;

  synonimToSend:string;
  antonimToSend:string;
  archaismToSend:string;
  expressionToSend:string;
  slangToSend:string;
  irregelarToSend:string[]=[];

  synonimConnections:string[] = [];
  antonimConnections:string[] = [];
  archaismConnections:string[] = [];
  expressionConnections:string[] = [];
  slangConnections:string[] = [];
  irregularConnections:IrregularObject[] = [];

  public allSynonims(synonims:Array<Array<string>>): void {
    if (synonims!=undefined && synonims!=null){
      for(let i=0;i<synonims.length;i++){
        this.synonimConnections.push(synonims[i][0]);
      }
    }
  }

  public allAntonims(antonims:Array<Array<string>>): void {
    if (antonims!=undefined && antonims!=null){
      for(let i=0;i<antonims.length;i++){
        this.antonimConnections.push(antonims[i][0]);
      }
    }
  }

  public allArchaisms(archaisms:string[]): void {
    if (archaisms!=undefined && archaisms!=null){
      for(let i=0;i<archaisms.length;i++){
        this.archaismConnections.push(archaisms[i]);
      }
    }
  }
  
  public allSlangs(slangs:string[]): void {
    if (slangs!=undefined && slangs!=null){
      for(let i=0;i<slangs.length;i++){
        this.slangConnections.push(slangs[i]);
      }
    }
  }

  public allExpressions(expressions:string[]): void {
    if (expressions!=undefined && expressions!=null){
      for(let i=0;i<expressions.length;i++){
        this.expressionConnections.push(expressions[i]);
      }
    }
  }
  
  public allIrregulars(irregulars:IrregularObject[]): void {
    if (irregulars!=undefined && irregulars!=null){
      for(let i=0;i<irregulars.length;i++){
        this.irregularConnections.push(irregulars[i]);
      }
    }
  }

  wordFormControl:FormControl[] = [new FormControl('', [Validators.required]), new FormControl('', [Validators.required])];
  wordFormControls:FormControl[] = [new FormControl('', [Validators.required]), new FormControl('', [Validators.required]), new FormControl('', [Validators.required])];
  irregularFormControls:FormControl[] = [new FormControl('', [Validators.required]), new FormControl('', [Validators.required]), new FormControl('', [Validators.required])];
  panelOpenState = false;
  
  constructor(
    private antonimService: IAntonimRepository,
    private synonimService: ISynonimRepository,
    private archaismService: IArchaismRepository,
    private expressionService: IExpressionRepository,
    private slangService: ISlangRepository,
    private irregularService: IIrregularsRepository,

    private getAllRealDataService:GetAllRealDataService,
    private redux:NgRedux<Store>
  ){ }

  public ngOnInit(): void {
    this.unsubscribe = this.redux.subscribe(()=>{
      this.allAntonims(this.redux.getState().allAntonims);
      this.allSynonims(this.redux.getState().allSynonims);
      this.allArchaisms(this.redux.getState().archaisms);
      this.allSlangs(this.redux.getState().slangs);
      this.allExpressions(this.redux.getState().expressions);
      this.allIrregulars(this.redux.getState().irregulars);
    });
  }

  DownloadSynonims(){
    //this.ClearSelected();
    if(this.synonimConnections===null || this.synonimConnections.length==0)
      this.getAllRealDataService.GetAllSynonims();
  }

  DownloadAntonims(){
    //this.ClearSelected();
    if(this.antonimConnections===null || this.antonimConnections.length==0)
      this.getAllRealDataService.GetAllAntonims();
  }

  DownloadArchaisms(){
    //this.ClearSelected();
    if(this.archaismConnections===null || this.archaismConnections.length==0)
      this.getAllRealDataService.GetAllArchaisms();
  }

  DownloadExpressions(){
    //this.ClearSelected();
    if(this.expressionConnections===null || this.expressionConnections.length==0)
      this.getAllRealDataService.GetAllExpressions();
  }

  DownloadSlangs(){
    //this.ClearSelected();
    if(this.slangConnections===null || this.slangConnections.length==0)
      this.getAllRealDataService.GetAllSlangs();
  }

  DownloadIrregulars(){
    //this.ClearSelected();
    if(this.irregularConnections===null || this.irregularConnections.length==0)
      this.getAllRealDataService.GetAllIrregulars();
  }

  DownloadDataArray(nameOfArray){
    //this.ClearSelected();
    if(nameOfArray === 'Synonim'){
      this.DownloadSynonims();
    } else if(nameOfArray === 'Antonim'){
      this.DownloadAntonims();
    } else if(nameOfArray === 'Archaism'){
      this.DownloadArchaisms();
    } else if(nameOfArray === 'Expression'){
      this.DownloadExpressions();
    } else if(nameOfArray === 'Slang'){
      this.DownloadSlangs();
    } else if(nameOfArray === 'Irregular'){
      this.DownloadIrregulars();
    }
  }

  

  public ClearSelected(event){
    this.selectedSynonimConnection=null;
    this.selectedAntonimConnection=null;
    this.selectedArchaismConnection=null;
    this.selectedExpressionConnection=null;
    this.selectedSlangConnection=null;
    this.selectedIrregularConnection=null;
    event.stopPropagation();
  }

  public NewTab(event){
    let nameOfArray = event.target.innerText;
    this.DownloadDataArray(nameOfArray);
    this.ClearSelected(event);
  }

  Post(type:string) {
    if(type==='Synonim' && this.synonimToSend!==null && this.synonimToSend!==undefined && this.synonimToSend!=""){
      this.synonimService.PostWord(this.synonimToSend)
    } else if(type==='Antonim' && this.selectedAntonimConnection!==null && this.selectedAntonimConnection!==undefined && this.selectedAntonimConnection!=""){
      this.antonimService.PostWord(this.selectedAntonimConnection, this.antonimToSend)
    } else if(type==='Archaism' && this.archaismToSend!==null && this.archaismToSend!==undefined && this.archaismToSend!=""){
      this.archaismService.PostWord(this.archaismToSend)
    }else if(type==='Expression' && this.expressionToSend!==null && this.expressionToSend!==undefined && this.expressionToSend!=""){
      this.expressionService.PostWord(this.expressionToSend)
    }else if(type==='Slang' && this.slangToSend!==null && this.slangToSend!==undefined && this.slangToSend!=""){
      this.slangService.PostWord(this.slangToSend)
    }else if(type==='Irregular' && this.irregelarToSend[0]!==null && this.irregelarToSend[0]!==undefined && this.irregelarToSend[0]!=""){
      let irregularObject = new IrregularObject("", this.irregelarToSend[0], this.irregelarToSend[1], this.irregelarToSend[2])
      this.irregularService.PostWord(irregularObject, "")
    }
  }

  Put(type:string) {
    if(type==='Synonim' && this.selectedSynonimConnection!==null && this.selectedSynonimConnection!==undefined && this.selectedSynonimConnection!=""){
      this.synonimService.PutWord(this.selectedSynonimConnection, this.synonimToSend)
    } else if(type==='Antonim' && this.selectedAntonimConnection!==null && this.selectedAntonimConnection!==undefined && this.selectedAntonimConnection!=""){
      this.antonimService.PutWord(this.selectedAntonimConnection, this.antonimToSend)
    } else if(type==='Archaism' && this.selectedArchaismConnection!==null && this.selectedArchaismConnection!==undefined && this.selectedArchaismConnection!=""){
      this.archaismService.PutWord(this.selectedArchaismConnection, this.archaismToSend)
    }else if(type==='Expression' && this.selectedExpressionConnection!==null && this.selectedExpressionConnection!==undefined && this.selectedExpressionConnection!=""){
      this.expressionService.PutWord(this.selectedExpressionConnection, this.expressionToSend)
    }else if(type==='Slang' && this.selectedSlangConnection!==null && this.selectedSlangConnection!==undefined && this.selectedSlangConnection!=""){
      this.slangService.PutWord(this.selectedSlangConnection, this.slangToSend)
    }else if(type==='Irregular' && this.selectedIrregularConnection!==null && this.selectedIrregularConnection!==undefined && this.selectedIrregularConnection!=""){
      let irregularObject = new IrregularObject("", this.irregelarToSend[0], this.irregelarToSend[1], this.irregelarToSend[2])
      this.irregularService.PutWord(this.selectedIrregularConnection, irregularObject, "")
    }
  }

  Insert(type:string) {
    if(type==='Synonim' && this.selectedSynonimConnection!==null && this.selectedSynonimConnection!==undefined && this.selectedSynonimConnection!=""){
      this.synonimService.InsertWord(this.selectedSynonimConnection, this.synonimToSend)
    } else if (type==='Antonim' && this.selectedAntonimConnection!==null && this.selectedAntonimConnection!==undefined && this.selectedAntonimConnection!=""){
      this.antonimService.InsertWord(this.selectedAntonimConnection, this.antonimToSend)
    }
  }
  
  public ngOnDestroy(): void {
    this.unsubscribe();
  }
}