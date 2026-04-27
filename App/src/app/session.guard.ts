import { Injectable } from '@angular/core';
import { CanActivate,ActivatedRoute, Router, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, NavigationExtras } from '@angular/router';
import { GlobalvarService } from './services/globalvar.service';

@Injectable({
  providedIn: 'root'
})
export class SessionGuard implements CanActivate {

  constructor(private route: ActivatedRoute,private globalvarService: GlobalvarService, private router: Router) {}


  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree {
    const hasSession = this.globalvarService.checkSession();
    if (!hasSession) {

      // Store the attempted URL in the query parameters
      const redirectUrl = state.url ? encodeURIComponent(state.url) :  "/";
      const queryParams: NavigationExtras = { queryParams: { returnUrl: redirectUrl } };
      // Redirect to login page with stored attempted URL
      return this.router.createUrlTree(['/login'], queryParams);
    }
    return true;
  }
}
