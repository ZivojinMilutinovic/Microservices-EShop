import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserService } from 'src/app/service/user.service';
import { GetUser } from '../models/get-user.model';

import { User } from '../models/user.model';

@Component({
  selector: 'app-users-index',
  templateUrl: './users-index.component.html',
  styleUrls: ['./users-index.component.scss']
})
export class UsersIndexComponent implements OnInit,OnDestroy {

  public users:GetUser[];
  public usersForShowing:GetUser[];
  public userDelete:GetUser;
  private usersSub:Subscription;
  private deleteSub:Subscription;
  constructor(private userService:UserService) { }
 

  ngOnInit(): void {
    this.userDelete=new GetUser();
    this.userService.getAllUsers().subscribe(res=>this.users=res);
  }
  assignDeleteUser(user:GetUser){
    this.userDelete=user;
   }
   deleteUser(){
     console.log(this.userDelete);
    this.deleteSub=this.userService.deleteUser(this.userDelete.userID.toString()).subscribe(res=>window.location.reload());
  }
  setPageItems(users:any[]){
    this.usersForShowing=users;
}

  ngOnDestroy(): void {
    if(this.usersSub!=null){
        this.usersSub.unsubscribe();
    }
    if(this.deleteSub!=null){
      this.deleteSub.unsubscribe();
  }
  }

}
