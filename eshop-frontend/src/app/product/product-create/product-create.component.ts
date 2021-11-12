import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { flatMap, mergeMap } from 'rxjs/operators';
import { ProductService } from 'src/app/service/product.service';
import { Util } from 'src/app/util/util';

@Component({
  selector: 'app-product-create',
  templateUrl: './product-create.component.html',
  styleUrls: ['./product-create.component.scss']
})
export class ProductCreateComponent implements OnInit,OnDestroy {

  productForm=this.formBuilder.group({
    productNumber:["",Validators.required],
    price:["",Validators.required],
    description:["",Validators.required],
    collection:["Women",Validators.required],
    clothingType:["Sweater",Validators.required],
  });
  private productFile:File=null;
  public collection:string[];
  public clothingTypes:string[];
  public errorMessage:string="";
  private productSub:Subscription;
  constructor(private formBuilder:FormBuilder,private productService:ProductService,
    private router:Router) {
      this.collection=Util.getCollection();
      this.clothingTypes=Util.getClothingTypes();
     }

  ngOnInit(): void {
  }
  
  public uploadProductFinishedPhoto=(file:any)=>{
      this.productFile=file;
  }
  onSubmit(){
    if(!this.productForm.valid){
      this.errorMessage="Form is not valid!";
      return;
    }
    if(this.productFile==null ){
      this.errorMessage="Enter a photo for the product";
      return;
    }
    const product=this.productForm.value;
    let formProductPhoto=new FormData();
    formProductPhoto.append("File",this.productFile,this.productFile.name);
    this.productSub=this.productService.postProduct(product)
    .pipe(
      mergeMap(
        res=>this.productService.postProductPhoto(res.id.toString(),formProductPhoto)
      ))
    .subscribe(_=>this.router.navigateByUrl("products"));
  }
  ngOnDestroy(): void {
    if(this.productSub!=null){
      this.productSub.unsubscribe();
    }
  }

}
