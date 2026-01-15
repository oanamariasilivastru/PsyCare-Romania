import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Appointment } from '../../../services/appointments.service';
import { RescheduleDialogComponent } from './reschedule-dialog.component';

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
    MatBadgeModule,
    MatDialogModule,
    MatSnackBarModule
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

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private cdr: ChangeDetectorRef
  ) {}

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

  rescheduleAppointment(appointment: Appointment): void {
    const dialogRef = this.dialog.open(RescheduleDialogComponent, {
      width: '550px',
      data: { appointment }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('Dialog closed with result:', result);
      
      if (result) {
        // HARDCODED TEST: For appointment ID 2, always change to 25th
        let newDate = result.newDate;
        if (appointment.id === 2) {
          newDate = '2026-01-25T14:00:00';
          console.log('Hardcoded: Changing appointment 2 to Jan 25th');
        }
        
        console.log('Appointment to update:', appointment);
        console.log('New date:', newDate);
        console.log('Before update:', JSON.stringify(this.appointments));
        
        // Create completely new array with updated appointment
        const updatedAppointments: Appointment[] = [];
        for (const app of this.appointments) {
          if (app.id === appointment.id) {
            updatedAppointments.push({
              ...app,
              date: newDate,
              notes: result.notes || app.notes
            });
            console.log('Updated appointment:', updatedAppointments[updatedAppointments.length - 1]);
          } else {
            updatedAppointments.push(app);
          }
        }
        
        this.appointments = updatedAppointments;
        
        console.log('After update:', JSON.stringify(this.appointments));
        
        // Force change detection
        this.cdr.detectChanges();
        
        this.snackBar.open(
          appointment.id === 2 
            ? 'Appointment rescheduled to January 25th (hardcoded test)!' 
            : 'Appointment rescheduled successfully!', 
          'Close', 
          {
            duration: 5000,
            horizontalPosition: 'center',
            verticalPosition: 'top',
            panelClass: ['success-snackbar']
          }
        );
      } else {
        console.log('Dialog was cancelled');
      }
    });
  }

  cancelAppointment(appointment: Appointment): void {
    const confirmCancel = confirm(
      `Are you sure you want to cancel your appointment with ${appointment.psychologistName} on ${this.formatDate(appointment.date)}?`
    );

    if (confirmCancel) {
      // Remove the appointment from the list
      this.appointments = this.appointments.filter(a => a.id !== appointment.id);
      
      this.snackBar.open('Appointment cancelled successfully', 'Close', {
        duration: 3000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
        panelClass: ['success-snackbar']
      });

      // Here you would typically call a service to cancel the appointment on the backend
      // this.appointmentsService.cancelAppointment(appointment.id).subscribe(...)
    }
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
        psychologistId: 101,
        date: '2026-01-22T11:15:00',
        notes: 'Progress evaluation and treatment plan review',
        fee: 150,
        patientName: 'Pop Ion',
        psychologistName: 'Dr. Maria Popescu'
      },
      {
        id: 3,
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