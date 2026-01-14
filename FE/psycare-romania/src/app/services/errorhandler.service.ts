import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

export interface ErrorDetails {
  message: string;
  code?: string;
  statusCode?: number;
  timestamp: Date;
}

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  constructor(private snackBar: MatSnackBar) {}

  handleError(error: any, context?: string): ErrorDetails {
    const errorDetails = this.parseError(error, context);
    this.logError(errorDetails);
    this.showUserFriendlyMessage(errorDetails);
    return errorDetails;
  }

  private parseError(error: any, context?: string): ErrorDetails {
    const timestamp = new Date();

    // HTTP Error
    if (error instanceof HttpErrorResponse) {
      return {
        message: this.getHttpErrorMessage(error),
        code: `HTTP_${error.status}`,
        statusCode: error.status,
        timestamp
      };
    }

    // Network Error
    if (error instanceof TypeError && error.message.includes('fetch')) {
      return {
        message: 'Network connection failed. Please check your internet connection.',
        code: 'NETWORK_ERROR',
        timestamp
      };
    }

    // Generic Error
    return {
      message: error?.message || 'An unexpected error occurred',
      code: context || 'UNKNOWN_ERROR',
      timestamp
    };
  }

  private getHttpErrorMessage(error: HttpErrorResponse): string {
    switch (error.status) {
      case 0:
        return 'Cannot connect to server. Please check your internet connection.';
      case 400:
        return error.error?.message || 'Invalid request. Please check your input.';
      case 401:
        return 'Session expired. Please log in again.';
      case 403:
        return 'You do not have permission to perform this action.';
      case 404:
        return 'The requested resource was not found.';
      case 500:
        return 'Server error. Please try again later.';
      case 503:
        return 'Service temporarily unavailable. Please try again later.';
      default:
        return error.error?.message || `An error occurred (${error.status})`;
    }
  }

  private showUserFriendlyMessage(errorDetails: ErrorDetails): void {
    this.snackBar.open(errorDetails.message, 'Close', {
      duration: 5000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['error-snackbar']
    });
  }

  private logError(errorDetails: ErrorDetails): void {
    console.error('[Error Handler]', {
      timestamp: errorDetails.timestamp.toISOString(),
      code: errorDetails.code,
      message: errorDetails.message,
      statusCode: errorDetails.statusCode
    });
  }

  showSuccess(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['success-snackbar']
    });
  }

  showWarning(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 4000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['warning-snackbar']
    });
  }

  handleHttpError(error: HttpErrorResponse): Observable<never> {
    this.handleError(error, 'HTTP_REQUEST');
    return throwError(() => error);
  }
}