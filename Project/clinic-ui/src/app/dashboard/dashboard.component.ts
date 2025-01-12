import { Component, OnInit } from '@angular/core';
import { DashboardService } from './dashboard.service';
import { Subscription } from 'rxjs';  // Import Subscription
import { LoginService } from '../login/login.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  //private dashboardSubscription: Subscription = new Subscription;  // To hold the subscription

  constructor(private dashboardService: LoginService) { }

  ngOnInit(): void {
    this.dashboardService.dashboardDetails().subscribe({
      next: (response) => {
        debugger;
        // Handle the successful response
        console.log('Dashboard Details:', response);
        // Add your logic here to process the response data
      },
      error: (error) => {
        // Handle errors
        console.error('Error fetching dashboard details:', error);
        // Add your error handling logic here
      },
      complete: () => {
        // Optional: Handle the completion of the subscription
        console.log('Dashboard details subscription completed.');
      }
    });
  }

  //ngOnDestroy(): void {
  //  // Unsubscribe when the component is destroyed to avoid memory leaks
  //  if (this.dashboardSubscription) {
  //    this.dashboardSubscription.unsubscribe();
  //  }
  //}
}

