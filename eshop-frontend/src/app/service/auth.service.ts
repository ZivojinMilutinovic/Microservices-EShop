import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoggedUser } from '../users/logged-user.module';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private currentUserSubject!:BehaviorSubject<LoggedUser> | any;
  public tokenString:string | null;
  public currentUser:Observable<LoggedUser>;
  private usersUrl:string;
  constructor(private http: HttpClient) {
        this.currentUserSubject=new BehaviorSubject<LoggedUser>(JSON.parse(<any>localStorage.getItem('currentUser')));
        this.tokenString=localStorage.getItem('token');
        this.currentUser=this.currentUserSubject.asObservable();
        this.usersUrl=environment.usersUrl;
   }
   public get currentUserValue():LoggedUser{
     return this.currentUserSubject.value;
   }
  login(user : any){
      return this.http.post<any>(this.usersUrl+"/authenticate",user).pipe(map(
        user=>{
          localStorage.setItem('currentUser',JSON.stringify(user));
          localStorage.setItem('token',user.token);
          this.currentUserSubject.next((<LoggedUser>user));
          return user;
        }
      ));
  }
  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
      }
}
