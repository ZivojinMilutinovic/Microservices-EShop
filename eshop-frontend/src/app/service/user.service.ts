import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { GetUser } from '../users/models/get-user.model';
import { LoggedUser } from '../users/models/logged-user.module';
import { User } from '../users/models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usersUrl:string;
  constructor(private http: HttpClient) {
    this.usersUrl=environment.usersUrl;
   }
   getAllUsers(){
     return this.http.get<GetUser[]>(this.usersUrl);
   }
   getUserById(id:string){
    return this.http.get<GetUser>(this.usersUrl+"/"+id);
  }
  putUser(user: any) {
    return this.http.put(this.usersUrl + `/${user.userID}`, user);
  }

  deleteUser(_id: string) {
    return this.http.delete(this.usersUrl + `/${_id}`);
  }
  postUser(user: any) {
    return this.http.post(this.usersUrl, user);
  }
}
