import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ShippingService {

  private shippingUrl:string;
  constructor(private http: HttpClient) {
    this.shippingUrl=environment.shippingUrl;
   }
  postShippingDetails(obj:any){
    return this.http.post<any>(this.shippingUrl,obj);
  }
  getShippingDetailsForUser(userId:number){
      return this.http.get<any>(this.shippingUrl+"/"+userId);
  }
}
