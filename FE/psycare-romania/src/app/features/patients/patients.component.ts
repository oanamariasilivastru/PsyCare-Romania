import { Component } from '@angular/core';

@Component({
  selector: 'app-patients',
  standalone: true,
  template: `
    <div class="patients-container">
      <h1>Patients</h1>
      <p>Manage patient information</p>
    </div>
  `,
  styles: [
    `
      .patients-container {
        padding: 20px;
      }
    `,
  ],
})
export class PatientsComponent {}
