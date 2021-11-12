import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, iif, of, Subscription } from 'rxjs';
import { flatMap, mergeMap } from 'rxjs/operators';
import { ProductService } from 'src/app/service/product.service';
import { Util } from 'src/app/util/util';
import { Product } from '../product.model';

@Component({
  selector: 'app-product-update',
  templateUrl: './product-update.component.html',
  styleUrls: ['./product-update.component.scss']
})
export class ProductUpdateComponent implements OnInit,OnDestroy {
  public collection:string[];
  public clothingTypes:string[];
  public product:Product;
  public errorMessage:string="";
  public pictureUrl:string="https://eshop-products.s3.eu-central-1.amazonaws.com/";
  public productPictureUrl:string="";
  private productFile:File=null;
  private productSub:Subscription=null;
  private productPicturesSub:Subscription=null;
  private putProductSub:Subscription;
  productForm=this.formBuilder.group({
    productNumber:["",Validators.required],
    price:["",Validators.required],
    description:["",Validators.required],
    collection:["Women",Validators.required],
    clothingType:["Sweater",Validators.required],
  });
  constructor(private route:ActivatedRoute,private formBuilder:FormBuilder,private productService:ProductService,
    private router:Router) {
      this.product=new Product();
      this.collection=Util.getCollection();
      this.clothingTypes=Util.getClothingTypes();
     }
 

  ngOnInit(): void {
    const id=this.route.snapshot.paramMap.get('id');
    this.productSub=this.productService.getProductById(id)
    .subscribe(res=>{
      this.product=res;
      this.productForm.patchValue({
        productNumber:this.product.productNumber,
        price:this.product.price,
        description:this.product.description,
        collection:this.product.collection,
        clothingType:this.product.clothingType,
      });
    });
    this.productPicturesSub=this.productService.getImagesForProduct(id).subscribe(
      res=>{
        this.productPictureUrl=this.pictureUrl+res[0].pictureUrl;
      }
    )
  }
  onSubmit(){
    let id=this.product.id;
      let productFormPhoto=new FormData();
      if(this.productFile!=null){
        productFormPhoto.append("File",this.productFile,this.productFile.name);
      }
      let obj=Object.assign({},this.productForm.value,{id});
      this.putProductSub=this.productService.putProduct(obj)
      .subscribe(res=>{
        if(this.productFile!=null){
          this.productService.deletePicturesForProduct(id.toString())
          .pipe(mergeMap(_=>forkJoin([
            iif(()=>this.productFile!=null,this.productService.postProductPhoto(id.toString(),productFormPhoto),of(null))
          ]))).subscribe(_=>this.router.navigateByUrl("/products"))
        }else{
          this.router.navigateByUrl("/products");
        }
      });
  }
  public uploadProductFinishedPhoto=(file:any)=>{
    this.productFile=file;
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
