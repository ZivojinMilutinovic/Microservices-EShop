import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import OriginalShoppingCartProduct from '../product/original-shopping-card-product.module';
import ShoppingCartProduct from '../product/shopping-cart-product.model';
import { AuthService } from '../service/auth.service';
import { ProductService } from '../service/product.service';
import { ShoppingCartService } from '../service/shopping-cart.service';
import { LoggedUser } from '../users/models/logged-user.module';
import ShoppingCart from './shopping-cart';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit,OnDestroy {

  public shoppingCart:ShoppingCart;
  private userSub:Subscription|null;
  public taxes:number=10;
  public shoppingCardEmpty:boolean=false;
  public totalProductsSum:number=0;
  public totalShipping:number=5;
  public totalSum:number;
  private shoppingCartSub:Subscription|null;
  public imageUrls:Map<number,string>;
  public currentUser:LoggedUser;
  private subArr:Subscription[]=[];
  public pictureUrl:string="https://eshop-products.s3.eu-central-1.amazonaws.com/";
  constructor(private shoppingCartService:ShoppingCartService,private authService:AuthService,
    private productService:ProductService,private router:Router) {
    this.imageUrls=new Map();
    this.shoppingCart=new ShoppingCart();
   }
 

  ngOnInit(): void {
    this.userSub=this.authService.currentUser.subscribe(_user=>{
        this.currentUser=_user;
        this.shoppingCartService.getActiveShoppingCart(this.currentUser.id)
        .subscribe(res=>{
          this.shoppingCart=res;
          let products=this.shoppingCart.products;
          
          products.forEach(product=>{
            let productId=product.originalProductId.toString();
            let helpSub=this.productService.getImagesForProduct(productId)
            .subscribe(res1=>{
               let newPictureUrl=this.pictureUrl+res1[0].pictureUrl;
               this.imageUrls.set(product.originalProductId,newPictureUrl);    
            });
            this.subArr.push(helpSub);
          });
          
          this.productsSum();
        }
        ,err=>{
          this.shoppingCardEmpty=true;
        });
    });
  }
  removeProduct(product:OriginalShoppingCartProduct){
      this.shoppingCart.products=this.shoppingCart.products.filter(pr=>pr!=product);
      console.log(this.shoppingCart);
  }
  fullPrice(price:number,count:HTMLSpanElement){
    return price*Number.parseInt(count.innerHTML);
  }
  minusSpanCount(product:OriginalShoppingCartProduct){
    if(product.numberOfItems>1)
     product.numberOfItems--;
     this.productsSum();
  }
  plusSpanCount(product:OriginalShoppingCartProduct){
    product.numberOfItems++;
    this.productsSum();
  }
  productsSum(){
    let sum=0;
    this.shoppingCart.products.forEach(pr=>sum+=(pr.price*pr.numberOfItems));
    this.totalProductsSum=sum;
  }
  goToCheckout(){
      console.log(this.shoppingCart)
      this.shoppingCartService.updateShoppingCart(this.shoppingCart.id,this.shoppingCart)
      .subscribe(res=>{
        console.log(res);
        this.router.navigateByUrl("/checkout");
      })
      
  }
  ngOnDestroy(): void {
    if(this.userSub!=null){
      this.userSub.unsubscribe();
    }
    if(this.shoppingCartSub!=null){
      this.shoppingCartSub.unsubscribe();
    }
    
    this.subArr.forEach(sub=>sub.unsubscribe());
  }

}
