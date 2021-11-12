import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { concatMap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Product } from '../product/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private productsUrl:string;
  private productsImagesUrl:string;
  constructor(private http: HttpClient) {
      this.productsUrl=environment.productsUrl;
      this.productsImagesUrl=environment.productsImagesUrl;
   }
  getAllProducts(){
    return this.http.get<Product[]>(this.productsUrl);
  }
  getProductById(id:string){
    return this.http.get<Product>(this.productsUrl+"/"+id);
  }
  putProduct(product:Product){
    console.log(product);
    return this.http.put(this.productsUrl+`/${product.id}`,product);
  }
  deleteProduct(id:string){
    return this.http.delete(this.productsUrl+ `/${id}`)
    .pipe(concatMap(_=>this.deletePicturesForProduct(id)));
  }
  postProduct(product:Product){
    return this.http.post<Product>(this.productsUrl,product);
  }

  postProductPhoto(id:string,obj:any){
    return this.http.post(this.productsImagesUrl+"/eshop-products/"+id,obj);
  }
  deletePicturesForProduct(id:string){
    return this.http.delete(this.productsImagesUrl+"/eshop-products/"+id)
  }
  uploadMultiFiles(id: string,obj: any){
    return this.http.post(this.productsImagesUrl+"/eshop-products/multi/"+id,obj);
  }
  getImagesForProduct(id:string){
    return this.http.get<any[]>(this.productsImagesUrl+"/productPictures/"+id);
  }
}
