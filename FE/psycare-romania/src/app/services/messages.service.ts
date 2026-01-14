import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface Message {
  id: number;
  senderId: number;
  senderName: string;
  senderType: string;
  receiverId: number;
  receiverName: string;
  receiverType: string;
  content: string;
  sentAt: string;
  readAt?: string;
  isRead: boolean;
}

export interface Conversation {
  userId: number;
  userType: string;
  otherUserId: number;
  otherUserType: string;
  messages: Message[];
}

export interface SendMessageDto {
  senderId: number;
  senderType: string;
  receiverId: number;
  receiverType: string;
  content: string;
}

export interface MarkAsReadDto {
  userId: number;
  userType: string;
  otherUserId: number;
  otherUserType: string;
}

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  private apiUrl = 'http://localhost:5286/api/Message';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': token ? `Bearer ${token}` : ''
    });
  }

  sendMessage(message: SendMessageDto): Observable<Message | null> {
    return this.http.post<Message>(this.apiUrl, message, { headers: this.getHeaders() })
      .pipe(catchError(err => {
        console.error('Failed to send message', err);
        return of(null);
      }));
  }

  getConversation(userId: number, userType: string, otherUserId: number, otherUserType: string): Observable<Message[]> {
    const params = new HttpParams()
      .set('userId', userId)
      .set('userType', userType)
      .set('otherUserId', otherUserId)
      .set('otherUserType', otherUserType);

    return this.http.get<Message[]>(`${this.apiUrl}/conversation`, { headers: this.getHeaders(), params })
      .pipe(catchError(err => {
        console.error('Failed to get conversation', err);
        return of([]);
      }));
  }

  getConversations(userId: number, userType: string): Observable<Conversation[]> {
    const params = new HttpParams()
      .set('userId', userId)
      .set('userType', userType);

    return this.http.get<Conversation[]>(`${this.apiUrl}/conversations`, { headers: this.getHeaders(), params })
      .pipe(catchError(err => {
        console.error('Failed to get conversations', err);
        return of([]);
      }));
  }

  markAsRead(dto: MarkAsReadDto): Observable<boolean> {
    return this.http.put<boolean>(`${this.apiUrl}/mark-as-read`, dto, { headers: this.getHeaders() })
      .pipe(catchError(err => {
        console.error('Failed to mark messages as read', err);
        return of(false);
      }));
  }

  getUnreadCount(userId: number, userType: string): Observable<number> {
    const params = new HttpParams()
      .set('userId', userId)
      .set('userType', userType);

    return this.http.get<number>(`${this.apiUrl}/unread-count`, { headers: this.getHeaders(), params })
      .pipe(catchError(err => {
        console.error('Failed to get unread count', err);
        return of(0);
      }));
  }
}
