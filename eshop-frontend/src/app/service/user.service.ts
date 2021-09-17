import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../users/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usersUrl:string;
  constructor(private http: HttpClient) {
    this.usersUrl=environment.usersUrl;
   }
   getUserById(id:string){
    return this.http.get<User>(this.usersUrl+"/"+id);
  }
  putUser(user: User) {
    return this.http.put(this.usersUrl + `/${user.userID}`, user);
  }

  deleteUser(_id: string) {
    return this.http.delete(this.usersUrl + `/${_id}`);
  }
  postUser(user: User) {
    return this.http.post(this.usersUrl, user);
  }
}
