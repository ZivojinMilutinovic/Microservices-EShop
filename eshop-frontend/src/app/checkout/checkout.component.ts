import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import ShoppingCart from '../cart/shopping-cart';
import { AuthService } from '../service/auth.service';
import { ProductService } from '../service/product.service';
import { ShippingService } from '../service/shipping.service';
import { ShoppingCartService } from '../service/shopping-cart.service';
import { LoggedUser } from '../users/models/logged-user.module';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit,OnDestroy {

  public shoppingCart:ShoppingCart;
  public shoppingCartIsEmpty:boolean=false;
  public successfullShopping:boolean=false;
  private userSub:Subscription|null;
  private shippingSub:Subscription|null;
  private shoppingCartSub:Subscription|null;
  private innerCartSub:Subscription|null;
  public taxes:number=10;
  public totalShipping:number=5;
  public currentUser:LoggedUser;
  public formError:string="";
  checkoutForm=this.formBuilder.group({
    cardNumber1:["",Validators.required],
    cardNumber2:["",Validators.required],
    cardNumber3:["",Validators.required],
    cardNumber4:["",Validators.required],
    cardHolder:["",Validators.required],
    expirationDateMonth:["",Validators.required],
    expirationDateYear:["",Validators.required],
    ccv:["",Validators.required],
});
  constructor(private shoppingCartService:ShoppingCartService,private authService:AuthService,
    private shippingService:ShippingService,private router:Router,private formBuilder:FormBuilder) { }


  ngOnInit(): void {
    this.userSub=this.authService.currentUser.subscribe(_user=>{
      this.currentUser=_user;
      this.shoppingCartService.getActiveShoppingCart(this.currentUser.id)
      .subscribe(res=>{
        this.shoppingCart=res;
      },err=>{
        this.shoppingCartIsEmpty=true;
      })
    });
  }
  calculateTotalSum(){
    let sum=0;
    this.shoppingCart.products.forEach(pr=>sum+=(pr.price*pr.numberOfItems));
    return (sum+(sum*this.taxes)/100+this.totalShipping);
  }
  onSubmit(){
    if(this.successfullShopping){
      return;
    }
    if(this.shoppingCartIsEmpty){
      this.formError="Shopping cart can not be empty!";
      return;
    }
    if(this.checkoutForm.invalid){
      this.formError="There is an error with a card!";
      return;
    }
    
    
    let cardNumber=this.checkoutForm.value.cardNumber1+this.checkoutForm.value.cardNumber2
    +this.checkoutForm.value.cardNumber3+this.checkoutForm.value.cardNumber4;
    let cardHolder=this.checkoutForm.value.cardHolder;
    let expirationDateMonth=Number.parseInt(this.checkoutForm.value.expirationDateMonth);
    let expirationDateYear=Number.parseInt(this.checkoutForm.value.expirationDateYear);
    let ccv=Number.parseInt(this.checkoutForm.value.ccv);
    let userId=this.currentUser.id;
    let firstName=this.currentUser.firstName;
    let lastName=this.currentUser.lastName;
    let address=this.currentUser.address;
    let totalSum=this.calculateTotalSum();
    let obj={cardNumber,cardHolder,expirationDateMonth,expirationDateYear,ccv,userId,
      address,firstName,lastName,totalSum
    };
    console.log(obj)
    this.shippingSub=this.shippingService.postShippingDetails(obj)
      .subscribe(res=>{
       this.innerCartSub= this.shoppingCartService.processShoppingCart(this.shoppingCart.id,this.shoppingCart)
        .subscribe(_res=>{
          this.successfullShopping=true;
          setTimeout(()=>{
          },3000);
            this.router.navigateByUrl("/");
        })
      
    });
    
    

  }

  ngOnDestroy(): void {
    if(this.userSub!=null){
      this.userSub.unsubscribe();
    }
    if(this.shoppingCartSub!=null){
      this.shoppingCartSub.unsubscribe();
    }
    if(this.shippingSub!=null){
      this.shippingSub.unsubscribe();
    }
    if(this.innerCartSub!=null){
      this.innerCartSub.unsubscribe();
    }
  }

}
