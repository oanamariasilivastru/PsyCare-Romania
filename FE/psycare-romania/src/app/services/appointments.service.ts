import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface Appointment {
  id: number;
  patientId: number;
  psychologistId: number;
  date: string;
  notes: string;
  fee: number;
  patientName?: string;
  psychologistName?: string;
}

@Injectable({
  providedIn: 'root',
})
export class AppointmentsService {
  private apiUrl = 'http://localhost:5286/api/PSYCare/Appointment';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': token ? `Bearer ${token}` : ''
    });
  }

  private getPatientIdFromToken(): number | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const patientId = payload.sub || payload.patientId;
      return patientId ? parseInt(patientId, 10) : null;
    } catch (error) {
      console.error('Error parsing token:', error);
      return null;
    }
  }

  getAppointmentsForPatient(): Observable<Appointment[]> {
    const patientId = this.getPatientIdFromToken();
    if (!patientId) {
      console.error('Cannot fetch appointments: patientId is null');
      return of([]);
    }

    return this.http.get<Appointment[]>(
      `${this.apiUrl}/Patient/${patientId}`,
      { headers: this.getHeaders() }
    ).pipe(
      catchError(err => {
        console.error('Error fetching appointments:', err);
        return of([]);
      })
    );
  }

  getAppointmentsForPsychologist(psychologistId: number): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(
      `${this.apiUrl}/Psychologist/${psychologistId}`,
      { headers: this.getHeaders() }
    ).pipe(
      catchError(err => {
        console.error('Error fetching appointments for psychologist:', err);
        return of([]);
      })
    );
  }
}
