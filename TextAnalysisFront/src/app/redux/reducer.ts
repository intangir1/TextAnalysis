import { Store } from "./store";
import { Action } from "./action";
import { ActionType } from "./action-type";
import { AnalysedText } from '../models/AnalysedText';
import { TemporalObject } from '../models/TemporalObject';
import { TemporalObjectForIrregular } from '../models/TemporalObjectForIrregular';
import { MatTableDataSource } from '@angular/material/table';
import { User } from '../models/User';
import { Login } from '../models/Login';

export class Reducer{
    public static reduce(oldStore: Store, action:Action):Store{
        let newStore:Store = {...oldStore};
        switch(action.type){
            case ActionType.CheckByTwoSentencesByRepeated:
                newStore.analysedText=new AnalysedText();
                newStore.analysedText.repeated=action.payload;
                break;
                
            case ActionType.CheckByTwoSentences:
                newStore.analysedText=action.payload;
                break;
            case ActionType.CheckByTwoSentencesError:
                newStore.CheckBySentenceError=action.payload;
                break;

            case ActionType.CheckByFullText:
                newStore.analysedText=action.payload;
                break;
            case ActionType.CheckByFullTextError:
                newStore.CheckByFullTextError=action.payload;
                break;
                
            case ActionType.GetAllUserAnalitics:
                newStore.allUserAnalitics=action.payload.map(x => new User(x.userID, x.userFirstName, x.userLastName, x.userNickName, "", x.userEmail, x.userGender, x.userBirthDate, x.userPicture, 0, x.userRole, "", x.userRegistrationDate, x.userLoginDate));
                newStore.dataUserAnaliticsSource = new MatTableDataSource(newStore.allUserAnalitics);
                break;
            case ActionType.GetAllUserAnaliticsError:
                newStore.allUserAnaliticsError=action.payload;
                break;
                
            case ActionType.AddUser:
                newStore.loginUser=action.payload;
                newStore.isLoggedIn=true;
                break;
            case ActionType.UpdateUser:
                newStore.isLoggedIn=true;
                break;
            case ActionType.UserLogin:
                newStore.loginUser=action.payload;
                newStore.isLoggedIn=true;
                break;
            case ActionType.LoginError:
                newStore.loginError=action.payload;
                break;
            case ActionType.SignUpError:
                newStore.signUpError=action.payload;
                break;
            case ActionType.SetIsLoggedIn:
                newStore.isLoggedIn=action.payload;
                break;
            case ActionType.UserLogOut:
                newStore.loginUser=new Login();
                newStore.isLoggedIn=false;
                break;
            
            case ActionType.NeedSignIn:
                newStore.needSignIn=action.payload;
                break;
                
            case ActionType.GetAllArchaisms:
                newStore.archaisms=action.payload;
                break;
            case ActionType.GetAllArchaismsError:
                newStore.getAllArchaismsError=action.payload;
                break;
            case ActionType.PostArchaism:
                newStore.archaism=action.payload;
                newStore.archaisms.push(action.payload);
                break;
            case ActionType.PostArchaismError:
                newStore.postArchaismError=action.payload;
                break;
            case ActionType.PutArchaism:
                let index = newStore.archaisms.findIndex(item => item === action.payload);
                newStore.archaisms[index] = action.payload;
                newStore.archaism=action.payload;
                break;
            case ActionType.PutArchaismError:
                newStore.putArchaismError=action.payload;
                break;
            case ActionType.DeleteArchaism:
                newStore.archaisms.forEach( (item, index) => {
                    if(item === action.payload)
                        newStore.archaisms.splice(index,1);
                    });
                break;
            case ActionType.DeleteArchaismError:
                newStore.deleteArchaismError=action.payload;
                break;
            case ActionType.DeleteAllArchaisms:
                newStore.archaisms=[];
                break;
            case ActionType.DeleteAllArchaismsError:
                newStore.deleteAllArchaismsError=action.payload;
                break;

            case ActionType.GetAllIrregulars:
                newStore.irregulars=action.payload;
                break;
            case ActionType.GetAllIrregularsError:
                newStore.getAllIrregularsError=action.payload;
                break;
            case ActionType.PostIrregular:
                newStore.irregular=action.payload;
                break;
            case ActionType.PostIrregularError:
                newStore.postIrregularError=action.payload;
                break;
            case ActionType.PutIrregular:
                newStore.irregular=action.payload;
                break;
            case ActionType.PutIrregularError:
                newStore.putIrregularError=action.payload;
                break;
            case ActionType.DeleteIrregular:
                newStore.irregulars.forEach( (item, index) => {
                    if(item.Equals(action.payload))
                        newStore.irregulars.splice(index,1);
                    });
                break;
            case ActionType.DeleteIrregularError:
                newStore.deleteIrregularError=action.payload;
                break;
            case ActionType.DeleteAllIrregulars:
                newStore.irregulars=[];
                break;
            case ActionType.DeleteAllIrregularsError:
                newStore.deleteAllIrregularsError=action.payload;
                break;

            case ActionType.GetAllSlangs:
                newStore.slangs=action.payload;
                break;
            case ActionType.GetAllSlangsError:
                newStore.getAllSlangsError=action.payload;
                break;
            case ActionType.PostSlang:
                newStore.slang=action.payload;
                newStore.slangs.push(action.payload);
                break;
            case ActionType.PostSlangError:
                newStore.postSlangError=action.payload;
                break;
            case ActionType.PutSlang:
                let index2 = newStore.slangs.findIndex(item => item === action.payload);
                newStore.slangs[index2] = action.payload;
                newStore.slang=action.payload;
                break;
            case ActionType.PutSlangError:
                newStore.putSlangError=action.payload;
                break;
            case ActionType.DeleteSlang:
                newStore.slangs.forEach( (item, index) => {
                    if(item === action.payload)
                        newStore.slangs.splice(index,1);
                    });
                break;
            case ActionType.DeleteSlangError:
                newStore.deleteSlangError=action.payload;
                break;
            case ActionType.DeleteAllSlangs:
                newStore.slangs=[];
                break;
            case ActionType.DeleteAllSlangsError:
                newStore.deleteAllSlangsError=action.payload;
                break;

            case ActionType.GetAllExpressions:
                newStore.expressions=action.payload;
                break;
            case ActionType.GetAllExpressionsError:
                newStore.getAllExpressionsError=action.payload;
                break;
            case ActionType.PostExpression:
                newStore.expression=action.payload;
                newStore.expressions.push(action.payload);
                break;
            case ActionType.PostExpressionError:
                newStore.postExpressionError=action.payload;
                break;
            case ActionType.PutExpression:
                let index3 = newStore.expressions.findIndex(item => item === action.payload);
                newStore.expressions[index3] = action.payload;
                newStore.expression=action.payload;
                break;
            case ActionType.PutExpressionError:
                newStore.putExpressionError=action.payload;
                break;
            case ActionType.DeleteExpression:
                newStore.expressions.forEach( (item, index) => {
                    if(item === action.payload)
                        newStore.expressions.splice(index,1);
                    });
                break;
            case ActionType.DeleteExpressionError:
                newStore.deleteExpressionError=action.payload;
                break;
            case ActionType.DeleteAllExpressions:
                newStore.expressions=[];
                break;
            case ActionType.DeleteAllExpressionsError:
                newStore.deleteAllExpressionsError=action.payload;
                break;

            case ActionType.GetAllSynonims:
                newStore.allSynonims=action.payload;
                break;
            case ActionType.GetAllSynonimsError:
                newStore.getAllSynonimsError=action.payload;
                break;
            case ActionType.GetSynonim:
                newStore.synonims=action.payload;
                break;
            case ActionType.GetSynonimError:
                newStore.synonims=[];
                newStore.getSynonimError=action.payload;
                break;
            case ActionType.PostSynonim:
                newStore.synonims=action.payload;
                break;
            case ActionType.PostSynonimError:
                newStore.postSynonimError=action.payload;
                break;
            case ActionType.PutSynonim:
                newStore.synonims=action.payload;
                break;
            case ActionType.PutSynonimError:
                newStore.putSynonimError=action.payload;
                break;
            case ActionType.InsertSynonim:
                newStore.synonims=action.payload;
                break;
            case ActionType.InsertSynonimError:
                newStore.insertSynonimError=action.payload;
                break;
            case ActionType.DeleteSynonim:
                newStore.synonims=action.payload;
                break;
            case ActionType.DeleteSynonimError:
                newStore.deleteSynonimError=action.payload;
                break;
            case ActionType.DeleteAllSynonims:
                newStore.synonims=[];
                break;
            case ActionType.DeleteAllSynonimsError:
                newStore.deleteAllSynonimsError=action.payload;
                break;

            case ActionType.GetAllAntonims:
                newStore.allAntonims=action.payload;
                break;
            case ActionType.GetAllAntonimsError:
                newStore.getAllAntonimsError=action.payload;
                break;
            case ActionType.GetAntonim:
                newStore.antonims=action.payload;
                break;
            case ActionType.GetAntonimError:
                newStore.antonims=[];
                newStore.getAntonimError=action.payload;
                break;
            case ActionType.PostAntonim:
                newStore.antonims=action.payload;
                break;
            case ActionType.PostAntonimError:
                newStore.postAntonimError=action.payload;
                break;
            case ActionType.PutAntonim:
                newStore.antonims=action.payload;
                break;
            case ActionType.PutAntonimError:
                newStore.putAntonimError=action.payload;
                break;
            case ActionType.InsertAntonim:
                newStore.antonims=action.payload;
                break;
            case ActionType.InsertAntonimError:
                newStore.insertAntonimError=action.payload;
                break;
            case ActionType.DeleteAntonim:
                newStore.antonims=action.payload;
                break;
            case ActionType.DeleteAntonimError:
                newStore.deleteAntonimError=action.payload;
                break;
            case ActionType.DeleteAllAntonims:
                newStore.antonims=[];
                break;
            case ActionType.DeleteAllAntonimsError:
                newStore.deleteAllAntonimsError=action.payload;
                break;
                
            case ActionType.GetAllTempArchaisms:
                newStore.tempArchaisms=action.payload.map(x => new TemporalObject(x.mongoId, x.action, x.connectionWord, x.inputedWord, x.type));
                break;
            case ActionType.GetAllTempArchaismsError:
                newStore.getAllTempArchaismsError=action.payload;
                break;
            case ActionType.PostTempArchaism:
                newStore.tempArchaism=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PostTempArchaismError:
                newStore.postTempArchaismError=action.payload;
                break;
            case ActionType.PutTempArchaism:
                newStore.tempArchaism=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PutTempArchaismError:
                newStore.putTempArchaismError=action.payload;
                break;
            case ActionType.DeleteTempArchaism:
                newStore.tempArchaisms.forEach( (item, index) => {
                    if(item.mongoId===action.payload)
                        newStore.tempArchaisms.splice(index,1);
                    });
                break;
            case ActionType.DeleteTempArchaismError:
                newStore.deleteTempArchaismError=action.payload;
                break;
            case ActionType.DeleteAllTempArchaisms:
                newStore.tempArchaisms=[];
                break;
            case ActionType.DeleteAllTempArchaismsError:
                newStore.deleteAllTempArchaismsError=action.payload;
                break;
                
            case ActionType.GetAllTempSlangs:
                newStore.tempSlangs=action.payload.map(x => new TemporalObject(x.mongoId, x.action, x.connectionWord, x.inputedWord, x.type));
                break;
            case ActionType.GetAllTempSlangsError:
                newStore.getAllTempSlangsError=action.payload;
                break;
            case ActionType.PostTempSlang:
                newStore.tempSlang=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PostTempSlangError:
                newStore.postTempSlangError=action.payload;
                break;
            case ActionType.PutTempSlang:
                newStore.tempSlang=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PutTempSlangError:
                newStore.putTempSlangError=action.payload;
                break;
            case ActionType.DeleteTempSlang:
                newStore.tempSlangs.forEach( (item, index) => {
                    if(item.mongoId===action.payload)
                        newStore.tempSlangs.splice(index,1);
                    });
                break;
            case ActionType.DeleteTempSlangError:
                newStore.deleteTempSlangError=action.payload;
                break;
            case ActionType.DeleteAllTempSlangs:
                newStore.tempSlangs=[];
                break;
            case ActionType.DeleteAllTempSlangsError:
                newStore.deleteAllTempSlangsError=action.payload;
                break;

            case ActionType.GetAllTempExpressions:
                newStore.tempExpressions=action.payload.map(x => new TemporalObject(x.mongoId, x.action, x.connectionWord, x.inputedWord, x.type));
                break;
            case ActionType.GetAllTempExpressionsError:
                newStore.getAllTempExpressionsError=action.payload;
                break;
            case ActionType.PostTempExpression:
                newStore.tempExpression=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PostTempExpressionError:
                newStore.postTempExpressionError=action.payload;
                break;
            case ActionType.PutTempExpression:
                newStore.tempExpression=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PutTempExpressionError:
                newStore.putTempExpressionError=action.payload;
                break;
            case ActionType.DeleteTempExpression:
                newStore.tempExpressions.forEach( (item, index) => {
                    if(item.mongoId===action.payload)
                        newStore.tempExpressions.splice(index,1);
                    });
                break;
            case ActionType.DeleteTempExpressionError:
                newStore.deleteTempExpressionError=action.payload;
                break;
            case ActionType.DeleteAllTempExpressions:
                newStore.tempExpressions=[];
                break;
            case ActionType.DeleteAllTempExpressionsError:
                newStore.deleteAllTempExpressionsError=action.payload;
                break;
                
            case ActionType.GetAllTempIrregulars:
                newStore.tempIrregulars=action.payload.map(x => new TemporalObjectForIrregular(x.mongoId, x.action, x.connectionWord, x.inputedWord, x.type));
                break;
            case ActionType.GetAllTempIrregularsError:
                newStore.getAllTempIrregularsError=action.payload;
                break;
            case ActionType.PostTempIrregular:
                newStore.tempIrregular=new TemporalObjectForIrregular(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PostTempIrregularError:
                newStore.postTempIrregularError=action.payload;
                break;
            case ActionType.PutTempIrregular:
                newStore.tempIrregular=new TemporalObjectForIrregular(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PutTempIrregularError:
                newStore.putTempIrregularError=action.payload;
                break;
            case ActionType.DeleteTempIrregular:
                newStore.tempIrregulars.forEach( (item, index) => {
                    if(item.mongoId===action.payload)
                        newStore.tempIrregulars.splice(index,1);
                    });
                break;
            case ActionType.DeleteTempIrregularError:
                newStore.deleteTempIrregularError=action.payload;
                break;
            case ActionType.DeleteAllTempIrregulars:
                newStore.tempIrregulars=[];
                break;
            case ActionType.DeleteAllTempIrregularsError:
                newStore.deleteAllTempIrregularsError=action.payload;
                break;
                
            case ActionType.GetAllTempSynonims:
                newStore.tempSynonims=action.payload.map(x => new TemporalObject(x.mongoId, x.action, x.connectionWord, x.inputedWord, x.type));
                break;
            case ActionType.GetAllTempSynonimsError:
                newStore.getAllTempSynonimsError=action.payload;
                break;
            case ActionType.PostTempSynonim:
                newStore.tempSynonim=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PostTempSynonimError:
                newStore.postTempSynonimError=action.payload;
                break;
            case ActionType.PutTempSynonim:
                newStore.tempSynonim=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PutTempSynonimError:
                newStore.putTempSynonimError=action.payload;
                break;
            case ActionType.InsertTempSynonim:
                newStore.tempSynonim=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.InsertTempSynonimError:
                newStore.insertTempSynonimError=action.payload;
                break;
            case ActionType.DeleteTempSynonim:
                newStore.tempSynonims.forEach( (item, index) => {
                    if(item.mongoId===action.payload)
                        newStore.tempSynonims.splice(index,1);
                    });
                break;
            case ActionType.DeleteTempSynonimError:
                newStore.deleteTempSynonimError=action.payload;
                break;
            case ActionType.DeleteAllTempSynonims:
                newStore.tempSynonims=[];
                break;
            case ActionType.DeleteAllTempSynonimsError:
                newStore.deleteAllTempSynonimsError=action.payload;
                break;

            case ActionType.GetAllTempAntonims:
                newStore.tempAntonims=action.payload.map(x => new TemporalObject(x.mongoId, x.action, x.connectionWord, x.inputedWord, x.type));
                break;
            case ActionType.GetAllTempAntonimsError:
                newStore.getAllTempAntonimsError=action.payload;
                break;
            case ActionType.PostTempAntonim:
                newStore.tempAntonim=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PostTempAntonimError:
                newStore.postTempAntonimError=action.payload;
                break;
            case ActionType.PutTempAntonim:
                newStore.tempAntonim=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.PutTempAntonimError:
                newStore.putTempAntonimError=action.payload;
                break;
            case ActionType.InsertTempAntonim:
                newStore.tempAntonim=new TemporalObject(action.payload.mongoId, action.payload.action, action.payload.connectionWord, action.payload.inputedWord, action.payload.type);
                break;
            case ActionType.InsertTempAntonimError:
                newStore.insertTempAntonimError=action.payload;
                break;
            case ActionType.DeleteTempAntonim:
                newStore.tempAntonims.forEach( (item, index) => {
                    if(item.mongoId===action.payload)
                        newStore.tempAntonims.splice(index,1);
                    });
                break;
            case ActionType.DeleteTempAntonimError:
                newStore.deleteTempAntonimError=action.payload;
                break;
            case ActionType.DeleteAllTempAntonims:
                newStore.tempAntonims=[];
                break;
            case ActionType.DeleteAllTempAntonimsError:
                newStore.deleteAllTempAntonimsError=action.payload;
                break;
                
            case ActionType.GetAllTemporalWords:
                newStore.allTemporalWords = action.payload.map(function (x) {
                    if (x.inputedWord.hasOwnProperty('mongoId'))
                        return new TemporalObject(x.mongoId, x.action, x.connectionWord, x.inputedWord.first + " - " + x.inputedWord.second + " - " + x.inputedWord.third, x.type);
                    else
                        return new TemporalObject(x.mongoId, x.action, x.connectionWord, x.inputedWord, x.type);
                });
                newStore.dataTemporalWordsSource = new MatTableDataSource(newStore.allTemporalWords);
                break;
            case ActionType.GetAllTemporalWordsError:
                newStore.allTemporalWordsError=action.payload;
                break;
            case ActionType.DeleteTemporalWord:
                newStore.allTemporalWords.forEach( (item, index) => {
                    if(item.mongoId===action.payload)
                        newStore.allTemporalWords.splice(index,1);
                });
                newStore.dataTemporalWordsSource = new MatTableDataSource(newStore.allTemporalWords);
                break;
                
            case ActionType.CompareAllSentencesWords:
                newStore.text = action.payload;
                break;
            case ActionType.GetAllTemporalWordsError:
                newStore.allTemporalWordsError=action.payload;
                break;
        }
        return newStore;
    }
}