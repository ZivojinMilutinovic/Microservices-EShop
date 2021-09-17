import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public invalidLogin:boolean=false;
  userForm=this.formBuilder.group({
    username:["",Validators.required],
    password:["",Validators.required],
});
  constructor(private formBuilder:FormBuilder,
    private authService:AuthService,
    private router:Router) { }

  ngOnInit(): void {
  }
  onSubmit(){
      this.authService.login(this.userForm.value)
      .pipe(first())
      .subscribe(res=>{
        this.router.navigate(["/"]);
      },err=>this.invalidLogin=true);
  }
}
