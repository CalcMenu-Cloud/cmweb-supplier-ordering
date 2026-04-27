import { Injectable } from '@angular/core';
import { HttpClient ,HttpHeaders} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.prod';
import { SNOrder } from '../interfaces/order.interface';
import { SNOrderList } from '../interfaces/order.interface';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private apiUrl = environment.api_url;

  constructor(private http: HttpClient) { }

// Method to fetch an order by its ID
getOrderById(id: number): Observable<SNOrder> {
  return this.http.get<SNOrder>(`${this.apiUrl}/Order/GetOrder?id=${id}`);
}

// Method to fetch an order by its ID
getOrderlistByClientId(clientid: number): Observable<SNOrder[]> {
  return this.http.get<SNOrder[]>(`${this.apiUrl}/Order/GetOrderlist?ClientId=${clientid}`);
}

saveOrder(orderData: any): Observable<any> {
  return this.http.post<any>(`${this.apiUrl}/order/SaveOrder`, orderData);
}

sendOrder(orderData: any): Observable<any> {
  return this.http.post<any>(`${this.apiUrl}/HGShopAPI/SendOrder`, orderData);
}

getAccessToken(): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}/HGShopAPI/GetAccessToken`);
}

}
