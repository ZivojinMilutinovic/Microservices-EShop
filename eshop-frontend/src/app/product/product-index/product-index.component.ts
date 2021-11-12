import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ProductService } from 'src/app/service/product.service';
import { Product } from '../product.model';

@Component({
  selector: 'app-product-index',
  templateUrl: './product-index.component.html',
  styleUrls: ['./product-index.component.scss']
})
export class ProductIndexComponent implements OnInit,OnDestroy {

  public products:Product[];
  public productsForShowing:Product[];
  public productForDelete:Product;
  private allProductsSub:Subscription=null;
  private deleteProductSub:Subscription=null;
  constructor(private productService:ProductService,private route:ActivatedRoute,private router:Router) 
  {
    this.productForDelete=new Product();
   }
 

  ngOnInit(): void {
    this.allProductsSub=this.productService.getAllProducts()
    .subscribe(res=>{
      this.products=res;
    });
  }
  deleteProduct(){
    this.deleteProductSub=this.productService.deleteProduct(this.productForDelete.id.toString())
    .subscribe(_=>window.location.reload());
  }
  goToProductDetail($event:Event,id:any){
    let nodeName=(<HTMLElement>($event.target)).nodeName;
    if(nodeName=="BUTTON" || nodeName=="A") return;
    this.router.navigateByUrl("/product-detail/"+id);
  }
  assignDeleteProduct(product:Product){
    this.productForDelete=product;
  }
  setPageItems(products:any[]){
      this.productsForShowing=products;
  }
  ngOnDestroy(): void {
    if(this.allProductsSub!=null)
    this.allProductsSub.unsubscribe();
    if(this.deleteProductSub!=null)
    this.deleteProductSub.unsubscribe();
  }

}
