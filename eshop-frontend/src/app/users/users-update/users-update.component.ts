import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserService } from 'src/app/service/user.service';
import { GetUser } from '../models/get-user.model';
import { User } from '../models/user.model';

@Component({
  selector: 'app-users-update',
  templateUrl: './users-update.component.html',
  styleUrls: ['./users-update.component.scss']
})
export class UsersUpdateComponent implements OnInit,OnDestroy {
  private userSub:Subscription=null;
  private putUserSub:Subscription=null;
  public errorMessage:string="";
  public user:GetUser;
  userForm=this.formBuilder.group({
    firstName:["",Validators.required],
    lastName:["",Validators.required],
    address:["",Validators.required],
    username:["",Validators.required],
    password:["",Validators.required],
    email:["",Validators.required],
    role:["User",Validators.required],
});
  constructor(private route:ActivatedRoute,
    private formBuilder:FormBuilder,
    private userService:UserService,
    private router:Router) {
      this.user=new GetUser();
     }


  ngOnInit(): void {
    const id=this.route.snapshot.paramMap.get("id");
    this.userSub=this.userService.getUserById(id)
    .subscribe(res=>{
      this.user=res;
      this.userForm.patchValue({
        firstName:this.user.firstName,
        lastName:this.user.lastName,
        address:this.user.address,
        username:this.user.username,
        password:"",
        email:this.user.email,
        role:"User"
      });
    },
    err=>this.errorMessage=err.message
    )

  }
  onSubmit(){
    const userID=this.user.userID;
    const obj=Object.assign({},this.userForm.value,{userID});
    this.putUserSub=this.userService.putUser(obj)
    .subscribe(_=>{
      this.router.navigateByUrl("/users")
    });
  }
  ngOnDestroy(): void {
    if(this.userSub!=null){
      this.userSub.unsubscribe();
    }
    if(this.putUserSub!=null){
      this.putUserSub.unsubscribe();
    }
  }
}
