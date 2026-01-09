import { Component } from '@angular/core';

@Component({
  selector: 'app-billing',
  standalone: true,
  template: `
    <div class="billing-container">
      <h1>Billing</h1>
      <p>View and manage billing information</p>
    </div>
  `,
  styles: [
    `
      .billing-container {
        padding: 20px;
      }
    `,
  ],
})
export class BillingComponent {}
