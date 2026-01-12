import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

export interface MoodDto {
  score: number;       
  date?: string;      
}

export interface Mood {
  patient: number;
  completion_date: string;
  score: number;
}

@Injectable({
  providedIn: 'root',
})
export class MoodService {
  private apiUrl = 'http://localhost:5286/api/PSYCare';

  constructor(private http: HttpClient) {}

  private getPatientIdFromToken(): number | null {
    const token = localStorage.getItem('token');
    if (!token) return null;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.sub; // sub = patientId
    } catch {
      return null;
    }
  }

  getMoodHistory(): Observable<Mood[]> {
    const patientId = this.getPatientIdFromToken();
    if (!patientId) return of([]);
    return this.http.get<Mood[]>(`${this.apiUrl}/Mood/${patientId}`);
  }

  addMood(score: number): Observable<any> {
    const patientId = this.getPatientIdFromToken();
    if (!patientId) return of({ error: 'Patient not found in token' });

    const body: MoodDto = { score };
    body.date = new Date().toISOString();

    return this.http.post(`${this.apiUrl}/Mood`, {
      ...body,
      patientId,
    });
  }
}
