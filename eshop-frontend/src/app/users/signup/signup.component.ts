import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserService } from '../../service/user.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit,OnDestroy {

  private registrationSub:Subscription|null=null;
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
    
    this.registrationSub=this.userService.postUser(this.userForm.value).subscribe(
      _=>this.router.navigateByUrl("/login"))
    }

    ngOnDestroy(): void {
      if(this.registrationSub!=null){
        this.registrationSub.unsubscribe();
      }
    }
}
