import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NavComponent } from './home/nav/nav.component';
import { FooterComponent } from './home/footer/footer.component';
import { PartnerLogoComponent } from './home/partner-logo/partner-logo.component';
import { IndexComponent } from './index/index.component';
import { LoginComponent } from './users/login/login.component';
import { SignupComponent } from './users/signup/signup.component';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,

    NavComponent,
    FooterComponent,
    PartnerLogoComponent,
    IndexComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
