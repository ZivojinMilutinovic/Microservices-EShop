import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import ShoppingCart from '../cart/shopping-cart';
import ShoppingCartProduct from '../product/shopping-cart-product.model';

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {

  private shoppingCartUrl:string;
  public currentShoppingCart:ShoppingCart;
  constructor(private http:HttpClient) {
    this.shoppingCartUrl=environment.shoppingCartUrl;
   }
  getAllShoppingCart(){
      return this.http.get<ShoppingCart[]>(this.shoppingCartUrl);
  }
  getShoppingCartById(id:number){
    return this.http.get<ShoppingCart[]>(this.shoppingCartUrl+"/"+id);
  }
  addProductToShoppingCart(postProductShoppingCart:any){
      return this.http.post<ShoppingCart>(this.shoppingCartUrl,postProductShoppingCart);
  }
  removeProductFromShoppingCart(shoppingCartId:number,productId:number){
          return this.http.delete(this.shoppingCartUrl+`/${shoppingCartId}/${productId}`);
  }
  updateShoppingCart(shoppingCartId:number,shopingCart:ShoppingCart){
          return this.http.put(this.shoppingCartUrl+"/shoppingCart/"+shoppingCartId,shopingCart);
  }
  processShoppingCart(shoppingCartId:number,shopingCart:ShoppingCart){
    return this.http.put(this.shoppingCartUrl+"/"+shoppingCartId,shopingCart);
}
  getActiveShoppingCart(userId:number)
  {
      return this.http.get<ShoppingCart>(this.shoppingCartUrl+`/active/${userId}`);
  }
}
