import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders  } from '@angular/common/http';

import { environment } from 'src/environments/environment.prod';
import { Department } from '../interfaces/hgdepartment.interface';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { GlobalvarService } from './globalvar.service';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private apiUrl = environment.api_url;
  constructor(private http: HttpClient,private globalvarService: GlobalvarService) { }

  getHGDepartment(): Observable<Department> {
    return this.http.get<Department>(`${this.apiUrl}/HGShopAPI/GetDapartment`);
  }


  login(username: string, password: string): Observable<any> {
 
   const gensessionid =this.globalvarService.getSecretKey();

   const credentials = {
    username: username,
    password: password,
    sessionid: gensessionid
  };
  
  // Convert the JSON object to a string
  const jsonCredentials = JSON.stringify(credentials);
  
  // Encode the string to Base64
  const base64Credentials = btoa(jsonCredentials);

     // Define the data to be sent in the request body
     const body = {
      credential: base64Credentials
    };

    // Define the headers for the request (if needed)
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    // Define the HTTP options for the request
    const options = {
      headers: headers
    };

    // Make the POST request to the login endpoint
    return this.http.post<any>(`${this.apiUrl}/Login/login`, body, options).pipe(
      catchError(error => {
        console.error('Error during login:', error);
        // Handle error (e.g., return a custom error message)
        return throwError('Login failed: Unexpected error occurred.');
      })
    );

  }

  
}
