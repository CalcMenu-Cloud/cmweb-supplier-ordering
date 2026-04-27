import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './pages/login/login.component';
import { OrderlistComponent } from './pages/orderlist/orderlist.component';
import { OrderviewComponent } from './pages/orderview/orderview.component';
import { LoginsuccessComponent } from './pages/loginsuccess/loginsuccess.component';
import { SessionGuard } from './session.guard';
//@idName
const routes: Routes = [

  { path: '', component: OrderlistComponent , canActivate: [SessionGuard]},
  { path: 'login', component: LoginComponent},
  { path: 'orderlist', component: OrderlistComponent, canActivate: [SessionGuard]},
  { path: 'orderview', component: OrderviewComponent, canActivate: [SessionGuard]},
  { path: 'loginsuccess', component: LoginsuccessComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
