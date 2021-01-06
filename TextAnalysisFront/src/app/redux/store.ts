import { FullText } from '../models/FullText';
import { AnalysedText } from '../models/AnalysedText';
import { Login } from '../models/Login';
import { User } from '../models/User';
import { IrregularObject } from '../models/IrregularObject';
import { TemporalObject } from '../models/TemporalObject';
import { TemporalObjectForIrregular } from '../models/TemporalObjectForIrregular';
import { MatTableDataSource } from '@angular/material/table';

export class Store{
    public analysedText:AnalysedText;
    public fullTextModel:FullText;
    
    public user:User;
    
    public isLoggedIn=false;
    public needSignIn=false;

    public archaism:string;
    public slang:string;
    public expression:string;
    public irregular:IrregularObject;

    public allUserAnalitics:User[]=[];
    public dataUserAnaliticsSource = new MatTableDataSource(this.allUserAnalitics);
    public allUserAnaliticsError:string;




    public antonims:string[]=[];
    public synonims:string[]=[];
    public archaisms:string[]=[];
    public slangs:string[]=[];
    public expressions:string[]=[];
    public irregulars:IrregularObject[]=[];
    
    public allAntonims:Array<Array<string>>;
    public allSynonims:Array<Array<string>>;
    
    public tempAntonims:TemporalObject[]=[];
    public tempSynonims:TemporalObject[]=[];
    public tempArchaisms:TemporalObject[]=[];
    public tempIrregulars:TemporalObjectForIrregular[]=[];
    public tempSlangs:TemporalObject[]=[];
    public tempExpressions:TemporalObject[]=[];

    public tempAntonim:TemporalObject;
    public tempSynonim:TemporalObject;
    public tempArchaism:TemporalObject;
    public tempIrregular:TemporalObjectForIrregular;
    public tempSlang:TemporalObject;
    public tempExpression:TemporalObject;


    public CheckByWordsError:string;
    public CheckBySentenceError:string;
    public CheckByFullTextError:string;

    public loginUser:Login = new Login();
    public loginError:string;
    public signUpError:string;
    
    public getAllArchaismsError:string;
    public postArchaismError:string;
    public putArchaismError:string;
    public insertArchaismError:string;
    public deleteArchaismError:string;
    public deleteAllArchaismsError:string;

    public getAllIrregularsError:string;
    public postIrregularError:string;
    public putIrregularError:string;
    public insertIrregularError:string;
    public deleteIrregularError:string;
    public deleteAllIrregularsError:string;

    public getAllSlangsError:string;
    public postSlangError:string;
    public putSlangError:string;
    public insertSlangError:string;
    public deleteSlangError:string;
    public deleteAllSlangsError:string;

    public getAllExpressionsError:string;
    public postExpressionError:string;
    public putExpressionError:string;
    public insertExpressionError:string;
    public deleteExpressionError:string;
    public deleteAllExpressionsError:string;

    public getAllAntonimsError:string;
    public getAntonimError:string;
    public postAntonimError:string;
    public putAntonimError:string;
    public insertAntonimError:string;
    public deleteAntonimError:string;
    public deleteAllAntonimsError:string;

    public getAllSynonimsError:string;
    public getSynonimError:string;
    public postSynonimError:string;
    public putSynonimError:string;
    public insertSynonimError:string;
    public deleteSynonimError:string;
    public deleteAllSynonimsError:string;



    public getAllTempArchaismsError:string;
    public postTempArchaismError:string;
    public putTempArchaismError:string;
    public deleteTempArchaismError:string;
    public deleteAllTempArchaismsError:string;

    public getAllTempIrregularsError:string;
    public postTempIrregularError:string;
    public putTempIrregularError:string;
    public deleteTempIrregularError:string;
    public deleteAllTempIrregularsError:string;

    public getAllTempSlangsError:string;
    public postTempSlangError:string;
    public putTempSlangError:string;
    public deleteTempSlangError:string;
    public deleteAllTempSlangsError:string;

    public getAllTempExpressionsError:string;
    public postTempExpressionError:string;
    public putTempExpressionError:string;
    public deleteTempExpressionError:string;
    public deleteAllTempExpressionsError:string;

    public getAllTempSynonimsError:string;
    public postTempSynonimError:string;
    public putTempSynonimError:string;
    public insertTempSynonimError:string;
    public deleteTempSynonimError:string;
    public deleteAllTempSynonimsError:string;

    public getAllTempAntonimsError:string;
    public postTempAntonimError:string;
    public putTempAntonimError:string;
    public insertTempAntonimError:string;
    public deleteTempAntonimError:string;
    public deleteAllTempAntonimsError:string;

    public allTemporalWords=[];
    public allTemporalWordsError:string;
    public dataTemporalWordsSource = new MatTableDataSource(this.allTemporalWords);

    public text:string;


    public delayTimeForSignOut:number=1800000;
    //public delayTimeForSignOut:number=100000;
}