import { Component, OnInit } from '@angular/core';
import { GlobalvarService } from '../services/globalvar.service'; 
@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  
  ishShow:number ;


  constructor(private globalvarService: GlobalvarService) { }

  ngOnInit(): void {
    this.ishShow=this.globalvarService.checkSession();
    console.log('is show header');
    console.log(this.ishShow);
  }

}
