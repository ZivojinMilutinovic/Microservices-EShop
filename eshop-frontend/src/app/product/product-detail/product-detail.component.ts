import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { ProductService } from 'src/app/service/product.service';
import { Product } from '../product.model';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit,OnDestroy {

  public product:Product;
  public pictureUrl:string="https://eshop-products.s3.eu-central-1.amazonaws.com/";
  public productPictureUrl:string="";
  private productSub:Subscription=null;
  private productPicturesSub:Subscription=null;
  constructor(private route:ActivatedRoute,private productService:ProductService) {
    this.product=new Product();
    
   }

  ngOnInit(): void {
    const id=this.route.snapshot.paramMap.get('id');
    this.productSub=this.productService.getProductById(id)
    .subscribe(res=>{
        this.product=res;
    });

    this.productPicturesSub=this.productService.getImagesForProduct(id).subscribe(
      res=>{
        this.productPictureUrl=this.pictureUrl+res[0].pictureUrl;
      }
    )
  }

  ngOnDestroy(): void {
    if(this.productSub!=null){
      this.productSub.unsubscribe();
    }
    if(this.productPicturesSub!=null){
      this.productPicturesSub.unsubscribe();
    }
  }

}
