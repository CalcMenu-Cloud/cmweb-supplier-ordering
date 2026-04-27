import { Component, OnInit } from '@angular/core';
import { ActivatedRoute,Router  } from '@angular/router';
import { GlobalvarService } from '../../services/globalvar.service'; 

import { MatDialog } from '@angular/material/dialog';

import { LoginService } from '../../services/login.service';
import { MatSnackBar } from '@angular/material/snack-bar';

import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
 

  loginInfo = { username: 'Sales CH', password: 'Foodservice_CH' }; // Define login information object

  state: string;
  clientcode: string;

  logintype:string;

  constructor(private route: ActivatedRoute,private globalvarService: GlobalvarService, private router: Router,public dialog: MatDialog,private login:LoginService,
    private snackBar: MatSnackBar) { }
 
  ngOnInit() {

    
    this.returnUrl;
    this.route.queryParams.subscribe(params => {
      const paramValue = params['code'];
      this.state= params['state'];
      this.clientcode= params['clientcode'];
      console.log(paramValue);
      console.log('clientcode ' + this.clientcode);
      console.log('state value :' + this.state);
      console.log('save state value :' + this.globalvarService.getSecretKey());


      if(this.globalvarService.checkSession())
      {
        

            if (this.state === null || this.state === undefined || this.state === '') {
                
            console.log("NO session");
            this.router.navigateByUrl("/");
            return;
            // variable is either null, undefined, or empty
            } 

            const jsonString = atob(this.state);

            // Parse the JSON string back to an object
            const jsonData = JSON.parse(jsonString);

            const seskey=jsonData.sessionKey;
            const returl=jsonData.returnUrl;
            console.log("session key "+seskey);
            console.log("return url "+returl);

            const decodedUrl = decodeURIComponent(returl);
            console.log('decoded url :' + decodedUrl);
            this.router.navigateByUrl(decodedUrl);
            return;

      }
    
      if (this.state === null || this.state === undefined || this.state === '') {
       
        console.log("NO session");
        return;
        // variable is either null, undefined, or empty
     } 
     console.log("NO2 session");
   const  _secreykey=this.globalvarService.getSecretKey();

     if (_secreykey === null || _secreykey === undefined || _secreykey === '') {
    
     
      return;
      // variable is either null, undefined, or empty
        } 

      const jsonString = atob(this.state);

      // Parse the JSON string back to an object
      const jsonData = JSON.parse(jsonString);

      const seskey=jsonData.sessionKey;
      const returl=jsonData.returnUrl;
      console.log("session key "+seskey);
      console.log("return url "+returl);


      if(seskey==this.globalvarService.getSecretKey())
      {
        console.log('session write :' + seskey);
        this.globalvarService.writeSession(seskey);
      //  window.location.href="/orderlist";
      const decodedUrl = decodeURIComponent(returl);
      console.log('decoded url :' + decodedUrl);
      this.router.navigateByUrl(decodedUrl);

      //  this.route.queryParams.subscribe(params => {
      //   // const returnUrl = params['returnUrl'] || '/';
      //   const returnUrl = returl;
      //   // Redirect to the attempted URL or to a default route
      //   this.router.navigateByUrl(returnUrl);
      // });

      }


    });
   
  }

  Login()
  {
    this.logintype="1";
    this.route.queryParams.subscribe(params => {
      this.returnUrl  = params['returnUrl'] || '/';
      // Redirect to the attempted URL or to a default route
      console.log(this.returnUrl);
      // this.router.navigateByUrl(returnUrl);
    });

    const mykey= this.globalvarService.setSecretKey();

    this.login.login(this.loginInfo.username,this.loginInfo.password).subscribe(
      response => {

     this.globalvarService.writeSession(response.sessionId);
     this.globalvarService.writeuserinfojson("",response);
        console.log('Login successful:',response);


        const jsonData = {
          "returnUrl": this.returnUrl,
          "sessionKey": response.sessionId
        };
        
            // Convert JSON object to string
            const jsonString = JSON.stringify(jsonData);
        
            // Convert string to Base64
            const base64String = btoa(jsonString);


        this.router.navigateByUrl('/login?state='+base64String);
        
        // Handle successful login response here (e.g., store token, redirect)
      },
      error => {
        console.error('Login failed:',error);
        this.showInvalidLoginMessage();
        return;
        // Handle error response here (e.g., display error message)
      }
    );

  }


  private showInvalidLoginMessage() {
    this.snackBar.open('Invalid username or password', 'Close', {
      duration: 3000, // Duration in milliseconds
      verticalPosition: 'top' // Position of the snackbar
    });
  }


  returnUrl: any;
  HogashopLogin()
  {
  this.logintype="2";
    this.route.queryParams.subscribe(params => {
      this.returnUrl  = params['returnUrl'] || '/';
      // Redirect to the attempted URL or to a default route
      console.log(this.returnUrl);
      // this.router.navigateByUrl(returnUrl);
    });



    const mykey= this.globalvarService.setSecretKey();

    const jsonData = {
      "returnUrl": this.returnUrl,
      "sessionKey": mykey
    };
    
        // Convert JSON object to string
        const jsonString = JSON.stringify(jsonData);
    
        // Convert string to Base64
        const base64String = btoa(jsonString);
        window.location.href = 'https://www.hogashop.ch/grant-access/calcmenu?state=' + base64String; // Replace '/target-route' with the URL you want to navigate to

  }

  loginPopup: Window;

  openLoginPopup() {

    console.log('openLoginPopup');
    this.route.queryParams.subscribe(params => {
      this.returnUrl  = params['returnUrl'] || '/';
      // Redirect to the attempted URL or to a default route
      console.log(this.returnUrl);
      // this.router.navigateByUrl(returnUrl);
    });



    const mykey= this.globalvarService.setSecretKey();

    const jsonData = {
      "returnUrl": this.returnUrl,
      "sessionKey": mykey
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

  
  handleMessage(event: MessageEvent) {

    // console.log(event);
 

    if (event.source === this.loginPopup) {
      // Check the content of the message
      const responseData = event.data;
      if (responseData.success) {
        // Handle successful login
        console.log('Login successful:', responseData.message);
        this.router.navigateByUrl('/login?state='+responseData.state);
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
