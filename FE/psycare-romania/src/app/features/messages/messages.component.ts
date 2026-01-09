import { Component } from '@angular/core';

@Component({
  selector: 'app-messages',
  standalone: true,
  template: `
    <div class="messages-container">
      <h1>Messages</h1>
      <p>View and send messages</p>
    </div>
  `,
  styles: [
    `
      .messages-container {
        padding: 20px;
      }
    `,
  ],
})
export class MessagesComponent {}
