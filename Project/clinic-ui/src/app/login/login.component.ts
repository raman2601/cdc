import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private loginService: LoginService, private router: Router) { }

  login() {
    this.loginService.login(this.username, this.password).subscribe(
      (data) => {
        this.router.navigate(['/dashboard']); // Redirect to a secured route
      },
      (error) => {
        this.errorMessage = 'Invalid credentials';
      },

    );
  }
}
