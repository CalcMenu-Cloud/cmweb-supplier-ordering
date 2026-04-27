import { Component, OnInit } from '@angular/core';
import { GlobalvarService } from '../services/globalvar.service'; 
import { retry } from 'rxjs';

import { HogashopapiService } from '../services/hogashopapi.service';
import { Department } from '../interfaces/hgdepartment.interface';
import { environment } from 'src/environments/environment.prod';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  private baseUrl = environment.baseUrl;

  department:Department;
  ishShow:number ;
fullname:string;
  constructor(private globalvarService: GlobalvarService,private hogaapi : HogashopapiService) { }

  ngOnInit(): void {

  
  // this.islogin();
  
 
    console.log('is show header');
    //this.getDepartment();
    console.log(this.ishShow);
    
  }

  islogin()
  {
    if( this.globalvarService.checkSession()==1)
    {
      this.fullname=this.globalvarService.getuserinfojson("fullname");
      this.ishShow=1;//this.globalvarService.checkSession();
      return 1;
    }


    return 0;
  }

  logout()
  {
    this.globalvarService.clearSession();
    window.location.href=this.baseUrl+"/login";
  }

  getDepartment()
  {
    this.hogaapi.getHGDepartment().subscribe(
      (response: Department) => {
        this.department  = response;
        console.log(this.department); // Log the retrieved order
       
  
      },
      error => {
        console.error('Error fetching order:', error);
       
  
      }
    );
   
  }

}
