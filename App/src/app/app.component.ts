import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})



export class AppComponent implements OnInit {
 // envName=environment.name;
  constructor(
    private router: Router,
    //envName = environment.name;
  //  @Inject(PLATFORM_ID) private platformId: any,
  //   private windowRef: WindowRef
  ) {
  }
  ngOnInit(): void {
   // throw new Error('Method not implemented.');
   console.log('[INFO] Re-routing....');

  }

  title = 'egs_pimupdate-app';
}
