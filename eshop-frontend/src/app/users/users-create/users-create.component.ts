import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-users-create',
  templateUrl: './users-create.component.html',
  styleUrls: ['./users-create.component.scss']
})
export class UsersCreateComponent implements OnInit,OnDestroy {
  private registrationSub:Subscription|null=null;
  public errorMessage:string="";
  userForm=this.formBuilder.group({
    firstName:["",Validators.required],
    lastName:["",Validators.required],
    address:["",Validators.required],
    username:["",Validators.required],
    password:["",Validators.required],
    email:["",Validators.required],
    role:["User",Validators.required],
});
  constructor(private formBuilder:FormBuilder,
    private userService:UserService,
    private router:Router) { }

  ngOnInit(): void {
  }
  onSubmit(){
    console.log(this.userForm.value);
    this.registrationSub=this.userService.postUser(this.userForm.value).subscribe(
      _=>this.router.navigateByUrl("/users"),
      err=>this.errorMessage=err.message
      )
    }
       ngOnDestroy(): void {
      if(this.registrationSub!=null){
        this.registrationSub.unsubscribe();
      }
    }

}
