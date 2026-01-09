import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  template: `
    <div class="dashboard-container">
      <h1>Dashboard</h1>
      <p>Welcome to the PsyCare Romania dashboard</p>
    </div>
  `,
  styles: [
    `
      .dashboard-container {
        padding: 20px;
      }
    `,
  ],
})
export class DashboardComponent {}
