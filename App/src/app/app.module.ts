import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {MatDialogModule} from '@angular/material/dialog';

import { RouterModule } from '@angular/router';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { ModalpopupComponent } from './pages/modalpopup/modalpopup.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';

import { LoginComponent } from './pages/login/login.component';
import { OrderlistComponent } from './pages/orderlist/orderlist.component';
import { OrderviewComponent } from './pages/orderview/orderview.component';

import { LoginsuccessComponent } from './pages/loginsuccess/loginsuccess.component';
import { AuthInterceptorService } from './services/auth-interceptor.service';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { YesnomodalComponent } from '../modal/yesnomodal/yesnomodal.component';
import { MessagemodalComponent } from '../modal/messagemodal/messagemodal.component';



@NgModule({
  declarations: [
     AppComponent,
     HeaderComponent,
     FooterComponent,
     ModalpopupComponent,
     OrderlistComponent,
     OrderviewComponent,
     LoginsuccessComponent,
     LoginComponent,
     YesnomodalComponent,
     MessagemodalComponent,

  ],
  imports: [
    FormsModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    RouterModule,
    MatDialogModule,
    BrowserAnimationsModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatNativeDateModule,
    MatSnackBarModule
  ],

  exports: [
    HeaderComponent,
    FooterComponent,
  ],
  providers: [
    { //EGS NOTE: This is needed by API calls since we are not specifying the content-type in the headers
      provide: HTTP_INTERCEPTORS,
      //useClass: HTTPINCEPTORService,
      useClass: AuthInterceptorService,
      multi: true
    }

  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
