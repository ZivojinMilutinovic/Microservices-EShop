import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';
import { LoggedUser } from 'src/app/users/logged-user.module';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  currentUser:LoggedUser;
  constructor(private router: Router,
    private authService: AuthService) {
      this.authService.currentUser.subscribe(_user=>this.currentUser=_user);
     }
     public userIsAdmin():boolean{
      return this.currentUser!=null && this.currentUser.role=="Admin";
    }
  ngOnInit(): void {
  }
  logout(){
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}
