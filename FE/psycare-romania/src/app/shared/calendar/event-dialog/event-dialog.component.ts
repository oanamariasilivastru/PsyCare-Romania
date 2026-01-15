import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AngularMaterialModule } from '../../../material/angular-material.module';
import { MatSelectModule } from '@angular/material/select';

type AppointmentType = 'Therapy' | 'Follow-up' | 'Group' | 'Other';

@Component({
  selector: 'app-event-dialog',
  standalone: true,
  imports: [AngularMaterialModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './event-dialog.component.html',
  styleUrl: './event-dialog.component.scss',
})
export class EventDialogComponent {
  types: AppointmentType[] = ['Therapy', 'Follow-up', 'Group', 'Other'];

  fb: FormBuilder = new FormBuilder();

  form = this.fb.group({
    title: ['', Validators.required],
    type: ['Therapy', Validators.required],
  });

  constructor(
    private dialogRef: MatDialogRef<EventDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { start: string; end?: string }
  ) {
    this.form = this.fb.group({
      title: ['', Validators.required],
      type: ['Therapy', Validators.required],
    });
  }

  save() {
    if (this.form.valid) {
      this.dialogRef.close({
        ...this.form.value,
        start: this.data.start,
        end: this.data.end ?? this.data.start,
      });
    }
  }

  cancel() {
    this.dialogRef.close();
  }
}
