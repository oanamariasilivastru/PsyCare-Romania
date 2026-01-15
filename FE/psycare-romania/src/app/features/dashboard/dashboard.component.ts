import { Component } from '@angular/core';
import { CalendarComponent } from '../../shared/calendar/calendar.component';
import { CalendarOptions } from '@fullcalendar/core/index.js';
import { NewsFeedComponent } from './news-feed/news-feed.component';
import  {MenuComponent} from "../../shared/menu/menu.component";

@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  imports: [CalendarComponent, NewsFeedComponent, MenuComponent],
})
export class DashboardComponent {
  calendarOptions: CalendarOptions = {
    height: 'calc(100% - 140px)',
    initialView: 'listWeek',
    buttonText: {
      today: 'Today',
    },
  };
}
