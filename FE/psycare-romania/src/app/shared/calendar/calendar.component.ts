import { AfterViewInit, Component, Input, OnInit, ViewChild } from '@angular/core';
import { FullCalendarComponent, FullCalendarModule } from '@fullcalendar/angular';
import { CalendarOptions } from '@fullcalendar/core/index.js';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import dayGridPlugin from '@fullcalendar/daygrid';
import listPlugin from '@fullcalendar/list';
import { EventDialogComponent } from './event-dialog/event-dialog.component';
import { MatDialog } from '@angular/material/dialog';

type AppointmentType = 'Therapy' | 'Follow-up' | 'Group' | 'Other';

interface Appointment {
  id: string;
  title: string;
  start: string;
  end?: string;
  type: AppointmentType;
}

@Component({
  selector: 'app-calendar',
  standalone: true,
  imports: [FullCalendarModule, EventDialogComponent],
  templateUrl: './calendar.component.html',
  styleUrl: './calendar.component.scss',
})
export class CalendarComponent implements OnInit, AfterViewInit {
  @Input() config: CalendarOptions = {};

  @ViewChild(FullCalendarComponent) calendarComponent!: FullCalendarComponent;

  private readonly STORAGE_KEY = 'calendar_appointments';

  private mockEvents: Appointment[] = [
    {
      id: '1',
      title: 'Therapy: John D.',
      start: '2026-01-03T09:00:00',
      end: '2026-01-03T10:00:00',
      type: 'Therapy',
    },
    {
      id: '2',
      title: 'Therapy: Mary S.',
      start: '2026-01-04T11:00:00',
      end: '2026-01-04T12:00:00',
      type: 'Therapy',
    },
    {
      id: '3',
      title: 'Group Session',
      start: '2026-01-05T14:00:00',
      end: '2026-01-05T15:30:00',
      type: 'Group',
    },
    {
      id: '4',
      title: 'Follow-up: Alex P.',
      start: '2026-01-06T10:30:00',
      end: '2026-01-06T11:00:00',
      type: 'Follow-up',
    },
    {
      id: '5',
      title: 'Therapy: Sophia L.',
      start: '2026-01-08T13:00:00',
      end: '2026-01-08T14:00:00',
      type: 'Therapy',
    },
    {
      id: '6',
      title: 'Follow-up: Michael K.',
      start: '2026-01-09T09:30:00',
      end: '2026-01-09T10:00:00',
      type: 'Follow-up',
    },
    {
      id: '7',
      title: 'Group Session',
      start: '2026-01-10T15:00:00',
      end: '2026-01-10T16:30:00',
      type: 'Group',
    },
    {
      id: '8',
      title: 'Therapy: John D.',
      start: '2026-01-12T08:00:00',
      end: '2026-01-12T09:00:00',
      type: 'Therapy',
    },
    {
      id: '9',
      title: 'Follow-up: Mary S.',
      start: '2026-01-13T11:30:00',
      end: '2026-01-13T12:00:00',
      type: 'Follow-up',
    },
    {
      id: '10',
      title: 'Therapy: Alex P.',
      start: '2026-01-14T14:00:00',
      end: '2026-01-14T15:00:00',
      type: 'Therapy',
    },
    {
      id: '11',
      title: 'Group Session',
      start: '2026-01-15T10:00:00',
      end: '2026-01-15T11:30:00',
      type: 'Group',
    },
    {
      id: '12',
      title: 'Follow-up: Sophia L.',
      start: '2026-01-16T09:00:00',
      end: '2026-01-16T09:30:00',
      type: 'Follow-up',
    },
    {
      id: '13',
      title: 'Therapy: Michael K.',
      start: '2026-01-18T13:00:00',
      end: '2026-01-18T14:00:00',
      type: 'Therapy',
    },
    {
      id: '14',
      title: 'Group Session',
      start: '2026-01-19T15:00:00',
      end: '2026-01-19T16:30:00',
      type: 'Group',
    },
    {
      id: '15',
      title: 'Follow-up: John D.',
      start: '2026-01-20T08:30:00',
      end: '2026-01-20T09:00:00',
      type: 'Follow-up',
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
    events: [],

    height: 'calc(100% - 80px)',
    scrollTime: '06:00:00',

    select: (info) => {
      this.openDialog(info.startStr, info.endStr);
    },

    eventDrop: (info) => {
      this.updateEvent(info.event);
    },

    eventResize: (info) => {
      this.updateEvent(info.event);
    },
  };

  constructor(private dialog: MatDialog) {}

  public ngOnInit() {
    this.calendarOptions = { ...this.calendarOptions, ...this.config };
  }

  public ngAfterViewInit() {
    const storedEvents = localStorage.getItem(this.STORAGE_KEY);

    if (storedEvents) {
      this.mockEvents = JSON.parse(storedEvents);
    } else {
      this.saveToStorage();
    }

    const calendarApi = this.calendarComponent.getApi();
    this.enrichEvents(this.mockEvents).forEach((event) => {
      calendarApi.addEvent(event);
    });
  }

  private getColorForType(type: AppointmentType): string {
    switch (type) {
      case 'Therapy':
        return '#2196f3';
      case 'Follow-up':
        return '#ff9800';
      case 'Group':
        return '#4caf50';
      case 'Other':
        return '#9c27b0';
      default:
        return '#9e9e9e';
    }
  }

  private enrichEvents(events: any[]) {
    return events.map((event) => {
      return {
        ...event,
        color: this.getColorForType(event.type),
      };
    });
  }

  private openDialog(start: string, end?: string) {
    const dialogRef = this.dialog.open(EventDialogComponent, {
      width: '400px',
      data: { start, end },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) return;

      this.addEvent(result, start, end);
    });
  }

  private addEvent(result: any, start: string, end?: string) {
    const newEvent: Appointment = {
      id: crypto.randomUUID(),
      title: result.title,
      start,
      end,
      type: result.type,
    };

    this.mockEvents.push(newEvent);
    this.saveToStorage();

    this.calendarComponent.getApi().addEvent({
      ...newEvent,
      color: this.getColorForType(newEvent.type),
    });
  }

  private updateEvent(event: any) {
    const index = this.mockEvents.findIndex((e) => e.id === event.id);
    if (index !== -1) {
      this.mockEvents[index] = {
        ...this.mockEvents[index],
        start: event.startStr,
        end: event.endStr || undefined,
      };
      this.saveToStorage();
    }
  }

  private saveToStorage() {
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(this.mockEvents));
  }
}
