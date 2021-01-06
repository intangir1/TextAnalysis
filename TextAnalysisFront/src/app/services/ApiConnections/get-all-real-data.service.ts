import { Injectable } from '@angular/core';
import { AntonimService } from './MainDb/antonim.service';
import { SynonimService } from './MainDb/synonim.service';
import { ArchaismService } from './MainDb/archaism.service';
import { ExpressionService } from './MainDb/expression.service';
import { SlangService } from './MainDb/slang.service';
import { IrregularService } from './MainDb/irregular.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgRedux } from 'ng2-redux';
import { Store } from 'src/app/redux/store';
import { LogService } from '../log.service';
import { Action } from 'src/app/redux/action';
import { ActionType } from 'src/app/redux/action-type';
import { byAllTemporalWords } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GetAllRealDataService {
  constructor(private antonimService: AntonimService,
    private synonimService: SynonimService,
    private archaismService: ArchaismService,
    private expressionService: ExpressionService,
    private slangService: SlangService,
    private irregularService: IrregularService,
    private http: HttpClient,
    private redux: NgRedux<Store>,
    private logger: LogService) { }

    public GetAllAntonims(): void {
      this.antonimService.GetAllWords();
    }

    public GetAllSynonims(): void {
      this.synonimService.GetAllWords();
    }

    public GetAllArchaisms(): void {
      this.archaismService.GetAllWords();
    }

    public GetAllExpressions(): void {
      this.expressionService.GetAllWords();
    }

    public GetAllSlangs(): void {
      this.slangService.GetAllWords();
    }

    public GetAllIrregulars(): void {
      this.irregularService.GetAllWords();
    }

    public GetAllTemporalWords(): void {
      let he = new HttpHeaders({'Content-Type':  'application/json','Authorization': 'Bearer ' + sessionStorage.getItem('access_token') });
      let observable = this.http.get<[]>(byAllTemporalWords, { headers: he });
      observable.subscribe(returnedAllTemporalWords=>{
        const action: Action={type:ActionType.GetAllTemporalWords, payload:returnedAllTemporalWords};
        this.redux.dispatch(action);
        this.logger.debug("Get All Temporal Words: ", returnedAllTemporalWords);
      }, error => {
        const action: Action={type:ActionType.GetAllTemporalWordsError, payload:error.message};
        this.redux.dispatch(action);
        this.logger.error("Get All TemporalWords Error: ", error.message);
      });
    }
}
