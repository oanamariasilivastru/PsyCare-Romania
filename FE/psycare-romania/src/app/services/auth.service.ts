import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, map, catchError, of } from 'rxjs';

interface LoginResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5286/api/Auth';
  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string): Observable<'psychologist' | 'patient' | null> {
    const body = { name: username, password };
    return this.http.post<LoginResponse>(`${this.apiUrl}/login/psychologist`, body)
      .pipe(
        map(res => {
          localStorage.setItem('token', res.token);
          return 'psychologist' as const;
        }),
        catchError(_ =>
          this.http.post<LoginResponse>(`${this.apiUrl}/login/patient`, body).pipe(
            map(res => {
              localStorage.setItem('token', res.token);
              return 'patient' as const;
            }),
            catchError(__ => of(null)) 
          )
        )
      );
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
