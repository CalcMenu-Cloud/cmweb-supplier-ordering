import { Component, OnInit } from '@angular/core';
import { SNOrder } from '../../interfaces/order.interface';
import { OrderService } from '../../services/order.service';
import { ActivatedRoute } from '@angular/router';

import { Router } from '@angular/router';

@Component({
  selector: 'app-orderlist',
  templateUrl: './orderlist.component.html',
  styleUrls: ['./orderlist.component.scss']
})
export class OrderlistComponent implements OnInit {

  isLoading: boolean = true; // Flag to track loading state
 
  isError: boolean = false; // Flag to track error state
  orderlist: SNOrder[];

  constructor(public orderservice: OrderService ,private router: Router) { }

  ngOnInit(): void {

    this.GetOrder(1);
  }


  GetOrder(clientId: number) {
    this.orderservice.getOrderlistByClientId(clientId).subscribe(
      (response: SNOrder[]) => {
        // When the response is received successfully
        this.orderlist = response; // Assign the response to orderlist
        console.log("My Orderlist"); // Log the retrieved order
        console.log(this.orderlist); // Log the retrieved order
        this.isLoading = false; // Set loading flag to false when data is received
      },
      error => {
        // When there is an error in fetching the order
        console.error('Error fetching order:', error); // Log the error
        this.isError = true; // Set error flag to true
        this.isLoading = false; // Set loading flag to false when data is received
        if (error.status === 401) {
       
        }
  
      }
    );
  }

  ViewOrder(orderid:number): void
  {

    this.router.navigate(['/orderview'], { queryParams: { id: orderid } });

  }

}
