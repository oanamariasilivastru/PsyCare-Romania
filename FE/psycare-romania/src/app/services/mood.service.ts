import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface MoodDto {
  patientId: number;
  score: number;
  date: string;
}

export interface Mood {
  patientId: number;
  completionDate: string;
  score: number;
}

@Injectable({
  providedIn: 'root',
})
export class MoodService {
  private apiUrl = 'http://localhost:5286/api/PSYCare';

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
    if (!token) {
      console.error('No token found');
      return null;
    }
    
    try {
      const parts = token.split('.');
      if (parts.length !== 3) {
        console.error('Invalid token format');
        return null;
      }
      
      const payload = JSON.parse(atob(parts[1]));
      console.log('Token payload:', payload);
      
      const patientId = payload.sub || payload.patientId;
      
      if (!patientId) {
        console.error('PatientId not found in token. Keys:', Object.keys(payload));
        return null;
      }
      
      return parseInt(patientId, 10);
    } catch (error) {
      console.error('Error parsing token:', error);
      return null;
    }
  }

  getMoodHistory(): Observable<Mood[]> {
    const patientId = this.getPatientIdFromToken();
    
    if (!patientId) {
      console.error('Cannot get mood history: patientId is null');
      return of([]);
    }
    
    console.log('Fetching moods for patient:', patientId);
    
    return this.http.get<Mood[]>(
      `${this.apiUrl}/Mood/${patientId}`,
      { headers: this.getHeaders() }
    ).pipe(
      catchError((error) => {
        console.error('Error fetching mood history:', error);
        return of([]);
      })
    );
  }

  addMood(score: number): Observable<any> {
    const patientId = this.getPatientIdFromToken();
    
    if (!patientId) {
      console.error('Cannot add mood: patientId is null');
      return throwError(() => new Error('Patient not found in token'));
    }

    const body: MoodDto = {
      patientId: patientId,
      score: score,
      date: new Date().toISOString()
    };

    console.log('Adding mood:', body);

    return this.http.post(
      `${this.apiUrl}/Mood`,
      body,
      { headers: this.getHeaders() }
    ).pipe(
      catchError((error) => {
        console.error('Error adding mood:', error);
        return throwError(() => error);
      })
    );
  }
}