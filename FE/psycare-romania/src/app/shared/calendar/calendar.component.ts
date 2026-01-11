import { Component, Input } from '@angular/core';
import { FullCalendarModule } from '@fullcalendar/angular';
import { CalendarOptions } from '@fullcalendar/core/index.js';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import dayGridPlugin from '@fullcalendar/daygrid';
import listPlugin from '@fullcalendar/list';

@Component({
  selector: 'app-calendar',
  standalone: true,
  imports: [FullCalendarModule],
  templateUrl: './calendar.component.html',
  styleUrl: './calendar.component.scss',
})
export class CalendarComponent {
  @Input() config: CalendarOptions = {};

  mockEvents = [
    {
      id: '1',
      title: 'Therapy: John D.',
      start: '2026-01-03T09:00:00',
      end: '2026-01-03T10:00:00',
      color: '#2196f3',
    },
    {
      id: '2',
      title: 'Therapy: Mary S.',
      start: '2026-01-04T11:00:00',
      end: '2026-01-04T12:00:00',
      color: '#2196f3',
    },
    {
      id: '3',
      title: 'Group Session',
      start: '2026-01-05T14:00:00',
      end: '2026-01-05T15:30:00',
      color: '#4caf50',
    },
    {
      id: '4',
      title: 'Follow-up: Alex P.',
      start: '2026-01-06T10:30:00',
      end: '2026-01-06T11:00:00',
      color: '#ff9800',
    },
    {
      id: '5',
      title: 'Therapy: Sophia L.',
      start: '2026-01-08T13:00:00',
      end: '2026-01-08T14:00:00',
      color: '#2196f3',
    },
    {
      id: '6',
      title: 'Follow-up: Michael K.',
      start: '2026-01-09T09:30:00',
      end: '2026-01-09T10:00:00',
      color: '#ff9800',
    },
    {
      id: '7',
      title: 'Group Session',
      start: '2026-01-10T15:00:00',
      end: '2026-01-10T16:30:00',
      color: '#4caf50',
    },
    {
      id: '8',
      title: 'Therapy: John D.',
      start: '2026-01-12T08:00:00',
      end: '2026-01-12T09:00:00',
      color: '#2196f3',
    },
    {
      id: '9',
      title: 'Follow-up: Mary S.',
      start: '2026-01-13T11:30:00',
      end: '2026-01-13T12:00:00',
      color: '#ff9800',
    },
    {
      id: '10',
      title: 'Therapy: Alex P.',
      start: '2026-01-14T14:00:00',
      end: '2026-01-14T15:00:00',
      color: '#2196f3',
    },
    {
      id: '11',
      title: 'Group Session',
      start: '2026-01-15T10:00:00',
      end: '2026-01-15T11:30:00',
      color: '#4caf50',
    },
    {
      id: '12',
      title: 'Follow-up: Sophia L.',
      start: '2026-01-16T09:00:00',
      end: '2026-01-16T09:30:00',
      color: '#ff9800',
    },
    {
      id: '13',
      title: 'Therapy: Michael K.',
      start: '2026-01-18T13:00:00',
      end: '2026-01-18T14:00:00',
      color: '#2196f3',
    },
    {
      id: '14',
      title: 'Group Session',
      start: '2026-01-19T15:00:00',
      end: '2026-01-19T16:30:00',
      color: '#4caf50',
    },
    {
      id: '15',
      title: 'Follow-up: John D.',
      start: '2026-01-20T08:30:00',
      end: '2026-01-20T09:00:00',
      color: '#ff9800',
    },
  ];

  calendarOptions: CalendarOptions = {
    editable: true,
    selectable: true,
    plugins: [timeGridPlugin, interactionPlugin, dayGridPlugin, listPlugin],

    initialView: 'timeGridWeek',
    timeZone: 'UTC+2',
    slotMinTime: '00:00:00',
    slotMaxTime: '24:00:00',
    slotLabelFormat: { hour: '2-digit', minute: '2-digit', hour12: false },
    eventTimeFormat: { hour: '2-digit', minute: '2-digit', hour12: false },

    firstDay: 1,
    nowIndicator: true,
    events: this.mockEvents,

    height: 'calc(100% - 80px)',
    scrollTime: '06:00:00',

    dateClick: function (info) {
      alert('clicked ' + info.dateStr);
    },
    select: function (info) {
      alert('selected ' + info.startStr + ' to ' + info.endStr);
    },
  };

  ngOnInit() {
    this.calendarOptions = { ...this.calendarOptions, ...this.config };
  }
}
