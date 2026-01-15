import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule } from '@angular/forms';
import { Appointment } from '../../../services/appointments.service';

@Component({
  selector: 'app-reschedule-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    FormsModule
  ],
  template: `
    <h2 mat-dialog-title>Reschedule Appointment</h2>
    <mat-dialog-content>
      <p class="appointment-info">
        <strong>Current appointment:</strong><br>
        {{ data.appointment.psychologistName }}<br>
        {{ formatDate(data.appointment.date) }}
      </p>

      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Select new date</mat-label>
        <input 
          matInput 
          [matDatepicker]="picker" 
          [(ngModel)]="selectedDate"
          [min]="minDate"
          placeholder="Choose a date"
        >
        <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
        <mat-datepicker #picker></mat-datepicker>
      </mat-form-field>

      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Time</mat-label>
        <input 
          matInput 
          type="time" 
          [(ngModel)]="selectedTime"
          placeholder="HH:MM"
        >
      </mat-form-field>

      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Notes (optional)</mat-label>
        <textarea 
          matInput 
          [(ngModel)]="notes"
          rows="3"
          placeholder="Add any notes about the rescheduling"
        ></textarea>
      </mat-form-field>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">Cancel</button>
      <button 
        mat-raised-button 
        color="primary" 
        (click)="onConfirm()"
        [disabled]="!selectedDate || !selectedTime"
      >
        Confirm Reschedule
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 400px;
      padding: 20px 24px;
    }

    .appointment-info {
      background: #f5f5f5;
      padding: 16px;
      border-radius: 8px;
      margin-bottom: 24px;
      line-height: 1.6;
    }

    .full-width {
      width: 100%;
      margin-bottom: 16px;
    }

    mat-dialog-actions {
      padding: 16px 24px;
      gap: 8px;
    }
  `]
})
export class RescheduleDialogComponent {
  selectedDate: Date | null = null;
  selectedTime: string = '';
  notes: string = '';
  minDate: Date = new Date();

  constructor(
    public dialogRef: MatDialogRef<RescheduleDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { appointment: Appointment }
  ) {
    // Set default time to current appointment time
    const currentDate = new Date(data.appointment.date);
    this.selectedTime = currentDate.toTimeString().slice(0, 5); // HH:MM format
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

  onCancel(): void {
    this.dialogRef.close();
  }

  onConfirm(): void {
    if (this.selectedDate && this.selectedTime) {
      const [hours, minutes] = this.selectedTime.split(':');
      const newDate = new Date(this.selectedDate);
      newDate.setHours(parseInt(hours, 10), parseInt(minutes, 10), 0, 0);

      this.dialogRef.close({
        newDate: newDate.toISOString(),
        notes: this.notes
      });
    }
  }
}