import { Component, OnInit , ElementRef, ViewChild,AfterViewInit  } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { OrderService } from '../../services/order.service';
import { ActivatedRoute,Router } from '@angular/router';
import { SNOrder } from '../../interfaces/order.interface';
import { GlobalvarService } from '../../services/globalvar.service'; 

import { HogashopapiService } from '../../services/hogashopapi.service';
import { Department } from '../../interfaces/hgdepartment.interface';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HGOrder, Part, Supplier, Customer  } from '../../interfaces/hgdepartment.interface';
import { environment } from 'src/environments/environment.prod';

import { Location } from '@angular/common';

import { YesnomodalComponent } from 'src/modal/yesnomodal/yesnomodal.component';
import { MessagemodalComponent } from 'src/modal/messagemodal/messagemodal.component';
@Component({
  selector: 'app-orderview',
  templateUrl: './orderview.component.html',
  styleUrls: ['./orderview.component.scss'],
  styles: [`
  .mat-dialog-container {
    padding: 0px!important;
    background: red !important;
    height:450px!important;
    max-height:450px!important;
  }
`]
})
export class OrderviewComponent implements OnInit {
  
  private baseUrl = environment.baseUrl;
  private apiUrl = environment.api_url;

  isLoading: boolean = true; // Flag to track loading state
  order: SNOrder; // Changed variable name and added type
  totalOrderPrice: number = 0; // Variable to store total order price
  isError: boolean = false; // Flag to track error state
  isshowdepartment: boolean=false;
  department:Department;
  processingMessage: string;
  isProcessing:boolean=false;
  hgorder:HGOrder;

  isConnectedToHogashop:boolean=false;
  loadingHogashopInfo:boolean=true;
 
  constructor(private route: ActivatedRoute,
    private router:Router, 
    public dialog: MatDialog,
    public orderservice: OrderService ,
    private globalvarService: GlobalvarService,
    private hogaapi : HogashopapiService,
    private snackBar: MatSnackBar,
    private location: Location,
    
    ) {}



    

  ngOnInit(): void {
    this.isConnectedToHogashop=false;
    this.route.queryParams.subscribe(params => {
      const paramValue = params['id'];
      console.log(paramValue);

    

      if (paramValue) {
        this.GetOrder(paramValue);
      }
    });


  
    this.getDepartment();
       // Initialize supplier
       const supplier: Supplier = {
        id: 67,
        name: "Mundo AG",
        street: "Buzibachstrasse 15",
        zip: "6023",
        city: "Rothenburg",
        phone: "041 288 89 29",
        fax: "041 288 89 28",
        emailInfo: "verkauf@mundoag.ch",
        website: "http://www.mundoag.ch",
        contactingPerson: {
          firstName: "Katrin",
          lastName: "Böttcher"
        },
        flags: {
          canReadCustomerInput: false
        }
      };
  
      // Initialize part
      const part: Part = {
        supplier: supplier,
        partTotal: "562.56",
        deliveryDate: "2024-03-08",
        partDeliveryFee: "0.00",
        freeShippingFrom: "0.00",
        isCustomerInputAllowed: false
      };
  
      // Initialize customer
      const customer: Customer = {
        id: 20284,
        fax: "",
        zip: "2000",
        city: "Neuchâtel",
        name: "CalcMenu Test Account",
        brand: {
          name: "HOGASHOP",
          title: "HOGASHOP",
          hostname: ""
        },
        email: "marc.enggist@eg-software.com",
        phone: "032 753 02 70",
        canton: "NE",
        gender: "M",
        street: "Rte de Pierre-à-Bot 92, 2000 Neuchâtel",
        company: "",
        country: "CH",
        comments: "",
        language: "de",
        csvFormat: {
          encoding: "UTF-8",
          delimiter: "comma",
          lineBreaker: "crlf"
        },
        contactingPerson: {
          lastName: "Enggist",
          firstName: "Marc"
        },
        hogalogContactingPerson: {
          lastName: "Sauder",
          firstName: ""
        }
      };
  
      // Initialize HGOrder
      this.hgorder = {
        parts: [part],
        total: "562.56",
        customer: customer,
        grandTotal: "562.56",
        deliveryFee: "0.00",
        sumOfAmounts: 12,
        countOfProducts: 2
      };


       // Initialize HGOrder
    this.hgorder = {
      parts: [part],
      total: "562.56",
      customer: customer,
      grandTotal: "562.56",
      deliveryFee: "0.00",
      sumOfAmounts: 12,
      countOfProducts: 2
    };
  }


 

  
  GetOrder(id: number) {
    this.orderservice.getOrderById(id).subscribe(
      (response: SNOrder) => {
        this.order = response;
        console.log(this.order); // Log the retrieved order
        this.calculateTotalOrderPrice(); // Calculate total order price when data is received
        this.isLoading = false; // Set loading flag to false when data is received
  
      },
      error => {
        console.error('Error fetching order:', error);
        this.isError = true; // Set error flag to true
        this.isLoading = false; // Set loading flag to false when data is received
        if (error.status === 401) {
        
          this.router.navigateByUrl(window.location.href);
          
        }
  
      }
    );
  }
  selectedDate: Date;
  calculateTotalOrderPrice(): void {
    this.totalOrderPrice = this.order.orderDetails.reduce((total, detail) => total + (detail.quantity * detail.sellingPrice), 0);
  }

  incrementQuantity(detail: any): void {

    if(detail.quantity>=9999) return;
    detail.quantity++;
    this. calculateTotalOrderPrice();
  }



  getDepartment()
  {
    this.isshowdepartment=false;

    this.hogaapi.getHGDepartment().subscribe(
      (response: Department) => {
        this.department  = response;
        console.log(this.department); // Log the retrieved order
       
        this.isshowdepartment=true;
        this.loadingHogashopInfo=false;
        this.isConnectedToHogashop=true;

      },
      error => {
        console.error('Error fetching order:', error);
        this.isshowdepartment=false;

        this.loadingHogashopInfo=false;
        console.log("department error ",error);
        if(error.status==406)
        {
        this.isConnectedToHogashop=false;
        }

  
      }
    );

    this.isshowdepartment=false;
   
  }


  decrementQuantity(detail: any): void {
    if(detail.quantity==1) return;
    if (detail.quantity > 1) {
      detail.quantity--;
      this. calculateTotalOrderPrice();
    }
  }

 
  onKeyPress(event: KeyboardEvent): void {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      event.preventDefault();
    }
  }




saveOrder(): void {
  
this.isProcessing=true;


    this.orderservice.saveOrder(this.order)
      .subscribe(
        response => {
          console.log('Order saved successfully:', response);
          this.isProcessing=false;
          // Handle response as needed
        },
        error => {
          this.isProcessing=false;
          console.error('Error saving order:', error);
          // Handle error as needed
        }
      );
  }

 
  openDeleteConfirmation(): void {
    const dialogRef = this.dialog.open(YesnomodalComponent, {
      width:'350px',
      panelClass: 'my-custom-dialog-class'
    });

    // dialogRef.updateSize('400', '300'); // Update size of the dialog
   

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Delete action
        this.sendOrder();
        console.log('Deleted');
      } else {
        // Cancel deletion action
        console.log('Canceled');
      }
    });

  }

  testmessage()
  {
    const modalRef = this.dialog.open(MessagemodalComponent,{ width:'350px',
    panelClass: 'my-custom-dialog-class'});
    modalRef.componentInstance.title = "title";
    modalRef.componentInstance.message = "message";
  }

  openMessageDialog( title:string,message:string)
  {
    const modalRef = this.dialog.open(MessagemodalComponent,{ width:'350px',
    panelClass: 'my-custom-dialog-class'});
    modalRef.componentInstance.title = title;
    modalRef.componentInstance.message = message;
  }

  sendOrder(): void {
    

    this.isProcessing=true;
    this.processingMessage="Sending order to hogashop...";
    this.order.departmentId=this.department.items[0].id.toString();
        this.orderservice.sendOrder(this.order)
          .subscribe(
            response => {
              console.log('Order send successfully:', response);
              this.processingMessage="Order sent";
             this.isProcessing=false;

              this.openMessageDialog("Success","The order has been successfully sent to Hogashop.");
             //this.displayMessage('Order sent successfully','successsnackbar');
           
              // Handle response as needed
            },
            error => {
              this.processingMessage="order not sent";

              this.openMessageDialog("Failed",error.error.message);
                            
              //this.displayMessage(error.statusText + "("+error.status+")","errorsnackbar");
            this.isProcessing=false;

              console.error('Error send order:', error);
              // Handle error as needed
            }
          );
      }


      private displayMessage(message: string,statuspanel:string) {
        this.snackBar.open(message, 'Close', {
          duration: 999999, // Duration in milliseconds
          verticalPosition: 'top', // Position of the snackbar
          panelClass: [statuspanel] // CSS class for styling success messages
        });
      }


   
  GetAccessTOken() {
    this.orderservice.getAccessToken().subscribe(
      (response: any) => {
        console.log(this.order); // Log the retrieved order
      },
      error => {
        console.error('Error fetching order:', error);
       
      }
    );
  }

 
////////////////////////////////////////////////////////////////
///Hogashop Login///////////////////////////////////////////////
////////////////////////////////////////////////////////////////
  loginPopup: Window;



  openLoginPopup() {

    console.log('openLoginPopup');
  
    const mykey= this.globalvarService.getSession();



    const jsonData = {
      "sessionKey": mykey,
      "codeuser" : this.globalvarService.getuserinfojson("codeUser"),
      "baseurl" : this.baseUrl,
      "callbackurl" :this.apiUrl
    };
    
        // Convert JSON object to string
        const jsonString = JSON.stringify(jsonData);
    
        // Convert string to Base64
        const base64String = btoa(jsonString);

    // URL of your login page
    const loginPageUrl = 'https://www.proto-hogashop.ch/grant-access/calcmenu?state='+base64String ;
 
    //const loginPageUrl = this.baseUrl+'/loginsuccess';
    // Open the login page in a popup window
    // this.loginPopup = window.open(loginPageUrl, 'Login', 'width=600,height=400');

    const width = 400;
    const height = 510;

    // Calculate the position to center the popup
    const left = (window.innerWidth - width) / 2;
    const top = (window.innerHeight - height) / 2;

    // Open the popup window with calculated dimensions and position
    this.loginPopup = window.open(loginPageUrl, 'Login', `width=${width}, height=${height}, left=${left}, top=${top}`);


    // Add a listener for the message event
    window.addEventListener('message', this.handleMessage.bind(this));
  }

  openLoginPopupSaviva() {

    console.log('openLoginPopupSaviva');
  
    const mykey= this.globalvarService.getSession();



    const jsonData = {
      "sessionKey": mykey,
      "codeuser" : this.globalvarService.getuserinfojson("codeUser"),
      "baseurl" : this.baseUrl,
      "callbackurl" :this.apiUrl
    };
    
        // Convert JSON object to string
        const jsonString = JSON.stringify(jsonData);
    
        // Convert string to Base64
        const base64String = btoa(jsonString);

    // URL of your login page
    const loginPageUrl = 'https://auth.integrale.ch/login?client_id=1vh81mtjatrr20hi8hvjdcgvju&redirect_uri=http://localhost&scope=openid&response_type=code&response_mode=query&identity_provider%0A=COGNITO&state='+base64String ;
 
    //const loginPageUrl = this.baseUrl+'/loginsuccess';
    // Open the login page in a popup window
    // this.loginPopup = window.open(loginPageUrl, 'Login', 'width=600,height=400');

    const width = 400;
    const height = 510;

    // Calculate the position to center the popup
    const left = (window.innerWidth - width) / 2;
    const top = (window.innerHeight - height) / 2;

    // Open the popup window with calculated dimensions and position
    this.loginPopup = window.open(loginPageUrl, 'Login', `width=${width}, height=${height}, left=${left}, top=${top}`);


    // Add a listener for the message event
    window.addEventListener('message', this.handleMessageSaviva.bind(this));
  }


  handleMessage(event: MessageEvent) {

    console.log('1 Source : ',event.source );
    console.log('2 Source : ',this.loginPopup );
    if (event.source === this.loginPopup) {
      // Check the content of the message
      const responseData = event.data;
      console.log(event.data);
      if (responseData.success) {
        // Handle successful login
        console.log('Login successful:', responseData.message);
        this. getDepartment();
        //this.router.navigateByUrl('/login?state='+responseData.state);
        // You might want to update your application state or perform other actions here
      } else {
        // Handle login failure or other scenarios
        console.error('Login failed:', responseData.message);
        // You might want to display an error message to the user
      }

      // Close the popup window
      this.loginPopup.close();
    }
  }

  handleMessageSaviva(event: MessageEvent) {

    console.log('1 Source : ',event.source );
    console.log('2 Source : ',this.loginPopup );
    if (event.source === this.loginPopup) {
      // Check the content of the message
      const responseData = event.data;
      console.log(event.data);
      if (responseData.success) {
        // Handle successful login
        console.log('Login successful:', responseData.message);
        this. getDepartment();
        //this.router.navigateByUrl('/login?state='+responseData.state);
        // You might want to update your application state or perform other actions here
      } else {
        // Handle login failure or other scenarios
        console.error('Login failed:', responseData.message);
        // You might want to display an error message to the user
      }

      // Close the popup window
      this.loginPopup.close();
    }
  }
  


}
