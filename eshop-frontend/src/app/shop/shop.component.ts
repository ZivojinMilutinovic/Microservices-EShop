import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Product } from '../product/product.model';
import { ProductService } from '../service/product.service';
import { Util } from 'src/app/util/util';
import { ShoppingCartService } from '../service/shopping-cart.service';
import { AuthService } from '../service/auth.service';
import { LoggedUser } from '../users/models/logged-user.module';
import { Router } from '@angular/router';
@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit,OnDestroy {

  public products:Product[];
  public productsForShowing:Product[];
  public imageUrls:Map<number,string>;
  private productSub:Subscription|null=null;
  private subArr:Subscription[]=[];
  public collection:string[];
  public filteredProducts:Product[];
  public clothingTypes:string[];
  public pictureUrl:string="https://eshop-products.s3.eu-central-1.amazonaws.com/";
  public displayModal:boolean=false;
  currentUserSub:Subscription|null=null;
  currentUser:LoggedUser;
  constructor(private productService:ProductService,private shoppingCartService:ShoppingCartService,
    private authService:AuthService,private router:Router) {
    this.collection=Util.getCollection();
    this.clothingTypes=Util.getClothingTypes();
    this.products=[];
    this.imageUrls=new Map();
   }


  ngOnInit(): void {
    this.currentUserSub=this.authService.currentUser.subscribe(_user=>this.currentUser=_user);
      this.productSub=this.productService.getAllProducts().subscribe(
        res=>{
            this.products=res;
            this.filteredProducts=this.products;
            this.products.forEach(product=>{
              let productId=product.id.toString();
                let helpSub=this.productService.getImagesForProduct(productId)
                .subscribe(res1=>{
                  let newPictureUrl=this.pictureUrl+res1[0].pictureUrl;
                  this.imageUrls.set(product.id,newPictureUrl);    
                  
                });
                this.subArr.push(helpSub);
            });
          }
      );
  }
  setPageItems(products:any[]){
    this.productsForShowing=products;
}
addProductToCart(product:Product){
  console.log(this.currentUser.id)
        let obj=Object.assign({},product,{userId:this.currentUser.id,originalProductId:product.id});
        this.shoppingCartService.addProductToShoppingCart(obj).subscribe(res=>{
           this.shoppingCartService.currentShoppingCart=res;
           console.log(this.shoppingCartService.currentShoppingCart);
        });
}
goToShoppingCart(){
    
}
  ngOnDestroy(): void {
    this.subArr.forEach(sub=>sub.unsubscribe());
    if(this.productSub!=null){
      this.productSub.unsubscribe();
    }
    if(this.currentUserSub!=null){
      this.currentUserSub.unsubscribe();
    }
  }
  sortFromLowerToHigher(a:number,b:number){
      return a-b;
  }
  sortFromHigherToLower(a:number,b:number){
    return b-a;
}
  applyFilters(selectClothingType:string,selectCollection:string,selectSorting:string){
   
    this.filteredProducts=[...this.products];
    let filterClothingType=(product:Product)=>product.clothingType==selectClothingType;
    let filterSelectCollection=(product:Product)=>product.collection==selectCollection
    let applyFilterClothingType=selectClothingType=="All";
    let applyFilterCollection=selectCollection=="All";
    if(!applyFilterClothingType && ! applyFilterCollection){
      this.filteredProducts=this.products.filter(filterClothingType).filter(filterSelectCollection);
    }else if(!applyFilterCollection){
        this.filteredProducts=this.products.filter(filterSelectCollection);

    }else if(!applyFilterClothingType){
            this.filteredProducts=this.products.filter(filterClothingType);
    }
      if(selectSorting=="Low Price"){
            this.filteredProducts.sort((a,b)=>a.price-b.price);
        }else if (selectSorting=="High Price"){
          this.filteredProducts.sort((a,b)=>b.price-a.price);
        }
        
        
        
  }

}
