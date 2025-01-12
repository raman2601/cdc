import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
interface User {
  username: string,
  passwordHash: string
}
@Injectable({
  providedIn: 'root',
})

export class LoginService {
  private apiUrl = 'https://localhost:44322/api'; // Replace with your API URL
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;

  constructor(private http: HttpClient) { 
    this.currentUserSubject = new BehaviorSubject<any>(
      JSON.parse(localStorage.getItem('currentUser') || '{}')
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  login(username: string, password: string): Observable<any>
  {
    const userobj: User = {
      username: username,
      passwordHash: password
    }
    return this.http
      .post(`${this.apiUrl}/Users/login`, userobj)
      .pipe(
        map((response:any) => {
          if (response && response.token) {
            // Store the user details and JWT in local storage
            localStorage.setItem('currentUser', response.token);
            this.currentUserSubject.next(response.token);
          }
          console.log(response);
          return response;
        })
      );
  }

  // Get the JWT token from localStorage
  getToken(): string | null {
    return localStorage.getItem('currentUser');
  }

  // Clear the JWT token from localStorage (on logout)
  logout(): void {
    localStorage.removeItem('currentUser');
  }
  //logout() {
  //  localStorage.removeItem('currentUser');
  //  this.currentUserSubject.next(null);
  //}

  public get currentUserValue(): any {
    return this.currentUserSubject.value;
  }

  dashboardDetails() {
    debugger;
    const userobj: User = {
      username: '9743432877',
      passwordHash: 'p9743432877'
    }
   const mobile:string = '9729916367';
    return this.http
      .post(`${this.apiUrl}/Users/paitentlogin`, JSON.stringify(mobile), {
        headers: { 'Content-Type': 'application/json' }
      })
      .pipe(
        map((response) => {
          debugger;
          return response;
        })
      );
  }

}
