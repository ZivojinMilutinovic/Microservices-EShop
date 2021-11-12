import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Product } from '../product/product.model';
import { ProductService } from '../service/product.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit,OnDestroy {

  public maleProducts:Product[];
  public femaleProducts:Product[];
  public imageUrls:Map<number,string>;
  private subArr:Subscription[]=[];
  public pictureUrl:string="https://eshop-products.s3.eu-central-1.amazonaws.com/";
  public productsSub:Subscription=null;
  constructor(private productService:ProductService) {
    this.imageUrls=new Map();
   }
 

  ngOnInit(): void {
    this.productsSub=this.productService.getAllProducts()
    .subscribe(res=>{
        this.maleProducts=res.filter(p=>p.collection=="Men");
        this.femaleProducts=res.filter(p=>p.collection=="Women");
        res.forEach(product=>{
          let productId=product.id.toString();
            let helpSub=this.productService.getImagesForProduct(productId)
            .subscribe(res1=>{
              let newPictureUrl=this.pictureUrl+res1[0].pictureUrl;
              this.imageUrls.set(product.id,newPictureUrl);    
              
            });
            this.subArr.push(helpSub);
        });
        console.log(this.maleProducts);
        console.log(this.femaleProducts)
    });
  }
  ngOnDestroy(): void {
    if(this.productsSub!=null){
      this.productsSub.unsubscribe();
    }
    this.subArr.forEach(sub=>sub.unsubscribe());
  }

}
