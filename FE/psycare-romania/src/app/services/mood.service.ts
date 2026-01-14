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
  private apiUrl = 'http://localhost:5286/api/PSYCare/Mood';

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

  getMoodHistory(): Observable<Mood[]> {
    const patientId = this.getPatientIdFromToken();
    if (!patientId) return of([]);

    return this.http.get<Mood[]>(`${this.apiUrl}/${patientId}`, { headers: this.getHeaders() })
      .pipe(
        catchError(err => {
          console.error('Failed to fetch mood history', err);
          return of([]);
        })
      );
  }

  addMood(score: number): Observable<any> {
    const patientId = this.getPatientIdFromToken();
    if (!patientId) return throwError(() => new Error('Patient not found in token'));

    const body: MoodDto = {
      patientId,
      score,
      date: new Date().toISOString()
    };

    return this.http.post(`${this.apiUrl}`, body, { headers: this.getHeaders() })
      .pipe(
        catchError(err => {
          console.error('Failed to add mood', err);
          return throwError(() => err);
        })
      );
  }
}
