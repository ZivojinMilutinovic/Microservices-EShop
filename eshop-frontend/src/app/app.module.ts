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
import { ProductCreateComponent } from './product/product-create/product-create.component';
import { ProductIndexComponent } from './product/product-index/product-index.component';
import { PaginationComponent } from './home/pagination/pagination.component';
import { ProductDetailComponent } from './product/product-detail/product-detail.component';
import { ProductUpdateComponent } from './product/product-update/product-update.component';
import { UploadComponent } from './home/upload/upload.component';
import { UsersIndexComponent } from './users/users-index/users-index.component';
import { UsersCreateComponent } from './users/users-create/users-create.component';
import { UsersUpdateComponent } from './users/users-update/users-update.component';
import { ShopComponent } from './shop/shop.component';
import { CartComponent } from './cart/cart.component';
import { CheckoutComponent } from './checkout/checkout.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,

    NavComponent,
    FooterComponent,
    PartnerLogoComponent,
    IndexComponent,
    ProductCreateComponent,
    ProductIndexComponent,
    PaginationComponent,
    ProductDetailComponent,
    ProductUpdateComponent,
    UploadComponent,
    UsersIndexComponent,
    UsersCreateComponent,
    UsersUpdateComponent,
    ShopComponent,
    CartComponent,
    CheckoutComponent
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
