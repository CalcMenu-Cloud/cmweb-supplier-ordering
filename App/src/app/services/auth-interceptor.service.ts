

import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders,HttpResponse ,HttpEvent,HttpErrorResponse} from '@angular/common/http';
import { GlobalvarService } from './globalvar.service';
import { CanActivate,ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, NavigationExtras } from '@angular/router';
import { environment } from 'src/environments/environment.prod';

import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { ActivatedRoute,Router  } from '@angular/router';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {
  id: string = '';
  constructor(private globalvarService: GlobalvarService,private route: ActivatedRoute,private router: Router) {}
  returnUrl="";
    
  private baseUrl = environment.baseUrl;

  intercept(req: HttpRequest<any>, next: HttpHandler) {
   
    const sessionid=this.globalvarService.getSession();

if(  this.globalvarService.checkSession()==1)
{
   this.id=this.globalvarService.getuserinfojson('id');
}


    console.log(sessionid);
      // Set sessionid to empty string if it's null
   
    // Add headers to the request
    const headers = new HttpHeaders({
      'sessionid': sessionid, // Example header
      'sysid': this.id// Example header
    });

    

    // Clone the request and add the headers
    const authReq = req.clone({ headers });

    // Pass the cloned request to the next handler
   // return next.handle(authReq);

   return next.handle(authReq).pipe(
    tap(event => {
      if (event instanceof HttpResponse) {
        // Handle successful responses here
        console.log('HTTP Response:', event);
      }
    }),
    catchError((error: HttpErrorResponse) => {
   
      if (error.status === 401) {
        console.log("error status : ",error.status); 
        

      
        this.globalvarService.clearSession();
        console.error('HTTP Error 401:', error);
              // Redirect to login page
              return throwError(this.router.createUrlTree(['/login']));
        // You can perform actions like redirecting the user to the login page or showing an error message
      }
      // Pass the error to the caller
      return throwError(error);
    })
  );
 
  }




  
}