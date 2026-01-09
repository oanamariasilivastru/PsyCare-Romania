import { Component } from '@angular/core';

@Component({
  selector: 'app-settings',
  standalone: true,
  template: `
    <div class="settings-container">
      <h1>Settings</h1>
      <p>Configure your application settings</p>
    </div>
  `,
  styles: [
    `
      .settings-container {
        padding: 20px;
      }
    `,
  ],
})
export class SettingsComponent {}
