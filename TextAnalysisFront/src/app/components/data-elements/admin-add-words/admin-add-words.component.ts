import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Unsubscribe } from 'redux';
import { NgRedux } from 'ng2-redux';
import { Store } from 'src/app/redux/store';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TempAntonimService } from 'src/app/services/ApiConnections/TemporalDb/tempAntonim.service';
import { TempSynonimService } from 'src/app/services/ApiConnections/TemporalDb/tempSynonim.service';
import { TempArchaismService } from 'src/app/services/ApiConnections/TemporalDb/tempArchaism.service';
import { TempExpressionService } from 'src/app/services/ApiConnections/TemporalDb/tempExpression.service';
import { TempSlangService } from 'src/app/services/ApiConnections/TemporalDb/tempSlang.service';
import { TempIrregularService } from 'src/app/services/ApiConnections/TemporalDb/tempIrregular.service';
import { AntonimService } from 'src/app/services/ApiConnections/MainDb/antonim.service';
import { SynonimService } from 'src/app/services/ApiConnections/MainDb/synonim.service';
import { ArchaismService } from 'src/app/services/ApiConnections/MainDb/archaism.service';
import { ExpressionService } from 'src/app/services/ApiConnections/MainDb/expression.service';
import { SlangService } from 'src/app/services/ApiConnections/MainDb/slang.service';
import { IrregularService } from 'src/app/services/ApiConnections/MainDb/irregular.service';
import { IrregularObject } from 'src/app/models/IrregularObject';
import { GetAllRealDataService } from 'src/app/services/ApiConnections/get-all-real-data.service';

@Component({
  selector: 'app-admin-add-words',
  templateUrl: './admin-add-words.component.html',
  styleUrls: ['./admin-add-words.component.css']
})
export class AdminAddWordsComponent implements OnInit, OnDestroy {

  @ViewChild(MatSort) sort: MatSort;
  public displayedColumns: string[] = ['Verdict', 'InputedWord', 'ConnectionWord', 'Type', 'Action'];
  public dataSource = new MatTableDataSource();
  private unsubscribe:Unsubscribe;
  
  constructor(private redux:NgRedux<Store>,
    private tempAntonimService: TempAntonimService,
    private tempSynonimService: TempSynonimService,
    private tempArchaismService: TempArchaismService,
    private tempExpressionService: TempExpressionService,
    private tempSlangService: TempSlangService,
    private tempIrregularService: TempIrregularService,
    
    private antonimService: AntonimService,
    private synonimService: SynonimService,
    private archaismService: ArchaismService,
    private expressionService: ExpressionService,
    private slangService: SlangService,
    private irregularService: IrregularService,
    
    private getAllRealDataService:GetAllRealDataService) {
      this.dataSource = this.redux.getState().dataTemporalWordsSource;
      this.dataSource.sort = this.sort;
  }

  public ngOnInit(): void {
    this.getAllRealDataService.GetAllTemporalWords();
    this.dataSource = this.redux.getState().dataTemporalWordsSource;
    this.dataSource.sort = this.sort;
    this.unsubscribe = this.redux.subscribe(()=>{
      this.dataSource = this.redux.getState().dataTemporalWordsSource;
      this.dataSource.sort = this.sort;
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  public Accept(element): void {
    if(element.action==='post'){
      this.Post(element);
    } else if(element.action==='put'){
      this.Put(element);
    } else if(element.action==='insert'){
      this.Insert(element);
    }
  }

  public Post(element): void {
    if(element.type==='synonim'){
      this.synonimService.PostWord(element.inputedWord, element.mongoId);
    } else if(element.type==='antonim'){
      this.antonimService.PostWord("", element.inputedWord, element.mongoId);
    } else if(element.type==='archaism'){
      this.archaismService.PostWord(element.inputedWord, element.mongoId);
    }else if(element.type==='expression'){
      this.expressionService.PostWord(element.inputedWord, element.mongoId);
    }else if(element.type==='slang'){
      this.slangService.PostWord(element.inputedWord, element.mongoId);
    }else if(element.type==='irregular'){
      let splitted = element.inputedWord.split(" - ");
      let irregularObject = new IrregularObject("", splitted[0], splitted[1], splitted[2]);
      this.irregularService.PostWord(irregularObject, element.mongoId);
    }
  }

  public Put(element): void {
    if(element.type==='synonim'){
      this.synonimService.PutWord(element.connectionWord, element.inputedWord, element.mongoId);
    } else if(element.type==='antonim'){
      this.antonimService.PutWord(element.connectionWord, element.inputedWord, element.mongoId);
    } else if(element.type==='archaism'){
      this.archaismService.PutWord(element.connectionWord, element.inputedWord, element.mongoId);
    }else if(element.type==='expression'){
      this.expressionService.PutWord(element.connectionWord, element.inputedWord, element.mongoId);
    }else if(element.type==='slang'){
      this.slangService.PutWord(element.connectionWord, element.inputedWord, element.mongoId);
    }else if(element.type==='irregular'){
      let splitted = element.inputedWord.split(" - ");
      let irregularObject = new IrregularObject("", splitted[0], splitted[1], splitted[2]);
      this.irregularService.PutWord(element.connectionWord, irregularObject, element.mongoId);
    }
  }

  public Insert(element): void {
    if(element.type==='synonim'){
      this.synonimService.InsertWord(element.connectionWord, element.inputedWord, element.mongoId);
    } else if(element.type==='antonim'){
      this.antonimService.InsertWord(element.connectionWord, element.inputedWord, element.mongoId);
    }
  }

  public Reject(element): void {
    if(element.type==='synonim'){
      this.tempSynonimService.DeleteWord(element.mongoId);
    } else if(element.type==='antonim'){
      this.tempAntonimService.DeleteWord(element.mongoId);
    } else if(element.type==='archaism'){
      this.tempArchaismService.DeleteWord(element.mongoId);
    }else if(element.type==='expression'){
      this.tempExpressionService.DeleteWord(element.mongoId);
    }else if(element.type==='slang'){
      this.tempSlangService.DeleteWord(element.mongoId);
    }else if(element.type==='irregular'){
      this.tempIrregularService.DeleteWord(element.mongoId);
    }
  }

  public ngOnDestroy(): void {
    this.unsubscribe();
  }
}