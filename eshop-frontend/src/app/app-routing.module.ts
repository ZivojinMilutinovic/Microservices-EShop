import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexComponent } from './index/index.component';
import { LoginComponent } from './users/login/login.component';
import { SignupComponent } from './users/signup/signup.component';


const routes: Routes = [
  {path:'home',component:IndexComponent},
  {path:'login',component:LoginComponent},
  {path:'signup',component:SignupComponent},
  {path: '',   redirectTo: '/home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
