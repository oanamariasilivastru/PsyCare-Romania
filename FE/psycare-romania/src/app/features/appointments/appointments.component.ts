import { Component } from '@angular/core';

@Component({
  selector: 'app-appointments',
  standalone: true,
  template: `
    <div class="appointments-container">
      <h1>Appointments</h1>
      <p>Manage your appointments</p>
    </div>
  `,
  styles: [
    `
      .appointments-container {
        padding: 20px;
      }
    `,
  ],
})
export class AppointmentsComponent {}
