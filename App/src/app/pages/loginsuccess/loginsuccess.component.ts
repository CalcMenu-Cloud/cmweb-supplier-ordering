import { Component, OnInit } from '@angular/core';
import { ActivatedRoute,Router  } from '@angular/router';

import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-loginsuccess',
  templateUrl: './loginsuccess.component.html',
  styleUrls: ['./loginsuccess.component.scss']
})
export class LoginsuccessComponent implements OnInit {

  private baseUrl = environment.baseUrl;

  constructor(private route: ActivatedRoute) { }
  state: string;
  ngOnInit(): void {

    this.route.queryParams.subscribe(params => {
      const paramValue = params['code'];
      this.state= params['state'];
      console.log(paramValue);
      console.log('state value :' + this.state);
      this.loginsuccess();
    })  
  }


  loginsuccess()
  {
    try {
      // Send response back to the parent window
      const response = { success: true, message: 'Login successful',state:this.state };
      console.log(response);
    //  window.opener.postMessage(response, this.baseUrl+"/logindialog"); // Use the same origin dynamically
    window.opener.postMessage(response, `${this.baseUrl}/loginsuccess?state=`+this.state); // Use the same origin dynamically
    } catch (error) {
      console.error('Error sending message to parent window:', error);
      // Handle communication errors gracefully, if needed
    }
  }

}
