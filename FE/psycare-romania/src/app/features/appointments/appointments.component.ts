import { Component } from '@angular/core';
import { CalendarComponent } from '../../shared/calendar/calendar.component';
import { CalendarOptions } from '@fullcalendar/core/index.js';

@Component({
  selector: 'app-appointments',
  standalone: true,
  templateUrl: './appointments.component.html',
  styleUrl: './appointments.component.scss',
  imports: [CalendarComponent],
})
export class AppointmentsComponent {
  calendarOptions: CalendarOptions = {
    height: 'calc(100% - 80px)',

    headerToolbar: {
      left: 'dayGridMonth,timeGridWeek,timeGridDay today',
      center: 'title',
      right: 'prevYear,prev,next,nextYear',
    },
    buttonText: {
      dayGridMonth: 'Month',
      timeGridWeek: 'Week',
      timeGridDay: 'Day',
      today: 'Today',
    },
  };
}
