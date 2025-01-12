import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginService } from './login.service';

@Injectable({
  providedIn: 'root'
})
export class JwtInterceptorService implements HttpInterceptor {

  constructor(private loginService: LoginService) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    debugger;
    const token = this.loginService.getToken();
    if (token) {
      const clonedRequest = req.clone({
        setHeaders: {
          'Authorization': `Bearer ${token}`
        }
      });
      // Send the cloned request with the Authorization header
      return next.handle(clonedRequest);
    }

    // If there's no token, simply pass the request along as is
    return next.handle(req);
  }
}
