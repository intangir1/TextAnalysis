import { User } from 'src/app/models/User';
import { Login } from 'src/app/models/Login';
import { Observable } from 'rxjs';

export interface IUserRepository{
    login(loginUser:Login): any;
    loginCore(loginUser:Login): any;
    signUp(userModel:User): Observable<User>;
    updateUser(user:User): void;
    UploadFile(id, file): void;
}