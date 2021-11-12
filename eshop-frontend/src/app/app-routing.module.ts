import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CartComponent } from './cart/cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { IndexComponent } from './index/index.component';
import { ProductCreateComponent } from './product/product-create/product-create.component';
import { ProductDetailComponent } from './product/product-detail/product-detail.component';
import { ProductIndexComponent } from './product/product-index/product-index.component';
import { ProductUpdateComponent } from './product/product-update/product-update.component';
import { ShopComponent } from './shop/shop.component';
import { LoginComponent } from './users/login/login.component';
import { SignupComponent } from './users/signup/signup.component';
import { UsersCreateComponent } from './users/users-create/users-create.component';
import { UsersIndexComponent } from './users/users-index/users-index.component';
import { UsersUpdateComponent } from './users/users-update/users-update.component';


const routes: Routes = [
  {path:'product-update/:id',component:ProductUpdateComponent},
  {path:'product-detail/:id',component:ProductDetailComponent},
  {path:'products',component:ProductIndexComponent},
  {path:'product-create',component:ProductCreateComponent},
  {path:'users',component:UsersIndexComponent},
  {path:'user-create',component:UsersCreateComponent},
  {path:'user-update/:id',component:UsersUpdateComponent},
  {path:'home',component:IndexComponent},
  {path:'login',component:LoginComponent},
  {path:'signup',component:SignupComponent},
  {path:"shop",component:ShopComponent},
  {path:"shopping-cart",component:CartComponent},
  {path:"checkout",component:CheckoutComponent},
  {path: '',   redirectTo: '/home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
