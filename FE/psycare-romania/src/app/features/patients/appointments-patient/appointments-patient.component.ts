import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { Appointment } from '../../../services/appointments.service';

@Component({
  selector: 'app-appointments-patient',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatToolbarModule,
    MatMenuModule,
    MatBadgeModule
  ],
  templateUrl: './appointments-patient.component.html',
  styleUrls: ['./appointments-patient.component.scss'],
})
export class AppointmentsPatientComponent implements OnInit {
  appointments: Appointment[] = [];
  loading = false;
  userName: string = '';
  unreadMessages: number = 0;
  notificationsCount: number = 0;

  constructor(private router: Router) {}

  ngOnInit() {
    this.loadUserName();
    this.appointments = this.getDummyAppointments();
  }

  private loadUserName(): void {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        this.userName = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || 
                        payload.name || 
                        payload.unique_name || 
                        'User';
      } catch (error) {
        console.error('Error parsing token:', error);
        this.userName = 'User';
      }
    }
  }

  formatDate(dateStr: string): string {
    const date = new Date(dateStr);
    return date.toLocaleString('en-US', {
      weekday: 'short',
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  isUpcoming(dateStr: string): boolean {
    const appointmentDate = new Date(dateStr);
    const now = new Date();
    const hoursDiff = (appointmentDate.getTime() - now.getTime()) / (1000 * 60 * 60);
    return hoursDiff <= 24 && hoursDiff > 0;
  }

  getAppointmentColor(dateStr: string): string {
    const appointmentDate = new Date(dateStr);
    const now = new Date();
    const daysDiff = Math.ceil((appointmentDate.getTime() - now.getTime()) / (1000 * 60 * 60 * 24));
    
    if (daysDiff <= 1) return '#f44336'; // Red - today/tomorrow
    if (daysDiff <= 3) return '#ff9800'; // Orange - within 3 days
    if (daysDiff <= 7) return '#ffc107'; // Yellow - within a week
    return '#2196f3'; // Blue - more than a week
  }

  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  private getDummyAppointments(): Appointment[] {
    return [
      {
        id: 1,
        patientId: 123,
        psychologistId: 101,
        date: '2026-01-15T09:30:00',
        notes: 'Initial consultation - Anxiety assessment',
        fee: 150,
        patientName: 'Pop Ion',
        psychologistName: 'Dr. Maria Popescu'
      },
      {
        id: 2,
        patientId: 123,
        psychologistId: 102,
        date: '2026-01-18T14:00:00',
        notes: 'Follow-up session - CBT therapy',
        fee: 120,
        patientName: 'Pop Ion',
        psychologistName: 'Dr. Alexandru Ionescu'
      },
      {
        id: 3,
        patientId: 123,
        psychologistId: 101,
        date: '2026-01-22T11:15:00',
        notes: 'Progress evaluation and treatment plan review',
        fee: 150,
        patientName: 'Pop Ion',
        psychologistName: 'Dr. Maria Popescu'
      },
      {
        id: 4,
        patientId: 123,
        psychologistId: 103,
        date: '2026-01-25T16:00:00',
        notes: 'Group therapy session',
        fee: 80,
        patientName: 'Pop Ion',
        psychologistName: 'Dr. Elena Dumitrescu'
      },
      {
        id: 5,
        patientId: 123,
        psychologistId: 101,
        date: '2026-01-29T10:00:00',
        notes: 'Weekly check-in',
        fee: 120,
        patientName: 'Pop Ion',
        psychologistName: 'Dr. Maria Popescu'
      }
    ];
  }
}