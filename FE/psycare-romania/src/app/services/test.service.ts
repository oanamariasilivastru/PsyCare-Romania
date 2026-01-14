import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface TestCompletionDto {
  patientId: number;
  testCode: string;
  score: number;
  completedAt: string;
  answers?: any;
}

export interface TestCompletionResponseDto {
  id: number;
  patientId: number;
  testCode: string;
  score: number;
  completedAt: string;
  answers?: any;
}

export interface TestHistoryDto {
  testCode: string;
  completions: TestCompletionResponseDto[];
}

@Injectable({
  providedIn: 'root'
})
export class TestCompletionService {
  private apiUrl = 'http://localhost:5286/api/PSYCare/TestCompletion';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': token ? `Bearer ${token}` : ''
    });
  }

  createTestCompletion(testCompletion: TestCompletionDto): Observable<boolean> {
    return this.http.post<any>(this.apiUrl, testCompletion, { headers: this.getHeaders() })
      .pipe(
        catchError(err => {
          console.error('Failed to create test completion', err);
          return of(false);
        }),
        // Return true if successful
        // backend doesn't return any value except message, so map to true
        (source$) => new Observable<boolean>(observer => {
          source$.subscribe({
            next: _ => observer.next(true),
            error: _ => observer.next(false),
            complete: () => observer.complete()
          });
        })
      );
  }

  getTestCompletionById(id: number): Observable<TestCompletionResponseDto | null> {
    return this.http.get<TestCompletionResponseDto>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() })
      .pipe(catchError(err => {
        console.error('Failed to get test completion by id', err);
        return of(null);
      }));
  }

  getPatientTestCompletions(patientId: number): Observable<TestCompletionResponseDto[]> {
    return this.http.get<TestCompletionResponseDto[]>(`${this.apiUrl}/patient/${patientId}`, { headers: this.getHeaders() })
      .pipe(catchError(err => {
        console.error('Failed to get patient test completions', err);
        return of([]);
      }));
  }

  getPatientTestsByCode(patientId: number, testCode: string): Observable<TestCompletionResponseDto[]> {
    return this.http.get<TestCompletionResponseDto[]>(`${this.apiUrl}/patient/${patientId}/test/${testCode}`, { headers: this.getHeaders() })
      .pipe(catchError(err => {
        console.error('Failed to get patient tests by code', err);
        return of([]);
      }));
  }

  getTestHistory(patientId: number): Observable<TestHistoryDto[]> {
    return this.http.get<TestHistoryDto[]>(`${this.apiUrl}/patient/${patientId}/history`, { headers: this.getHeaders() })
      .pipe(catchError(err => {
        console.error('Failed to get test history', err);
        return of([]);
      }));
  }

  deleteTestCompletion(id: number): Observable<boolean> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() })
      .pipe(
        catchError(err => {
          console.error('Failed to delete test completion', err);
          return of(false);
        }),
        (source$) => new Observable<boolean>(observer => {
          source$.subscribe({
            next: _ => observer.next(true),
            error: _ => observer.next(false),
            complete: () => observer.complete()
          });
        })
      );
  }
}
