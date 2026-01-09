import { Component, signal } from '@angular/core';
import { MenuComponent } from './shared/menu/menu.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [MenuComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('psycare-romania');
}
