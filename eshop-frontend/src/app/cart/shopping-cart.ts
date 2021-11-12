import OriginalShoppingCartProduct from "../product/original-shopping-card-product.module";
import { Product } from "../product/product.model";

export default class ShoppingCart{
    id:number;
    userId:number;
    processed:boolean;
    products:OriginalShoppingCartProduct[];
}