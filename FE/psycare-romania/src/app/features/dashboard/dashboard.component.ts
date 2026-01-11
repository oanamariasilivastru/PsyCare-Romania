import { Component } from '@angular/core';
import { CalendarComponent } from '../../shared/calendar/calendar.component';
import { CalendarOptions } from '@fullcalendar/core/index.js';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  imports: [CalendarComponent],
})
export class DashboardComponent {
  calendarOptions: CalendarOptions = {
    height: 'calc(100% - 80px)',
    initialView: 'listWeek',
    buttonText: {
      today: 'Today',
    },
  };
}
