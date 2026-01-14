import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  username = '';
  password = '';
  hidePassword = true;
  loading = false;
  errorMessage = '';

  constructor(private router: Router, private authService: AuthService) {}

  togglePassword() {
    this.hidePassword = !this.hidePassword;
  }

  login() {
    this.errorMessage = '';

    if (!this.username || !this.password) {
      this.errorMessage = 'Please enter both username and password';
      return;
    }

    this.loading = true;

    this.authService.login(this.username, this.password).subscribe({
      next: (role) => {
        this.loading = false;
        if (role === 'psychologist') {
          this.router.navigate(['/dashboard']);
        } else if (role === 'patient') {
          this.router.navigate(['/patient']);
        } else {
          this.errorMessage = 'Invalid credentials';
        }
      },
      error: (err) => {
        this.loading = false;
        if (err.status === 401) {
          this.errorMessage = 'Invalid credentials';
        } else {
          this.errorMessage = 'Login failed. Please try again later.';
        }
      },
    });
  }
}
