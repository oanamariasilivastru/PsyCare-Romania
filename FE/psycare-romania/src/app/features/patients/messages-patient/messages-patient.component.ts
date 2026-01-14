import { Component, OnInit, ViewChild, ElementRef, AfterViewChecked, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ErrorHandlerService } from '../../../services/errorhandler.service'
import { catchError, retry, timeout } from 'rxjs/operators';
import { of } from 'rxjs';

interface Message {
  id: number;
  senderId: number;
  senderName: string;
  senderType: 'patient' | 'psychologist';
  content: string;
  timestamp: string;
  read: boolean;
}

interface Conversation {
  psychologistId: number;
  psychologistName: string;
  psychologistAvatar: string;
  lastMessage: string;
  lastMessageTime: string;
  unreadCount: number;
}

@Component({
  selector: 'app-messages-patient',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatToolbarModule,
    MatMenuModule,
    MatBadgeModule,
    MatListModule,
    MatDividerModule,
    MatSnackBarModule
  ],
  templateUrl: './messages-patient.component.html',
  styleUrls: ['./messages-patient.component.scss']
})
export class MessagesPatientComponent implements OnInit, AfterViewChecked, OnDestroy {
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;
  
  conversations: Conversation[] = [];
  selectedConversation: Conversation | null = null;
  messages: Message[] = [];
  newMessage: string = '';
  userName: string = '';
  currentUserId: number = 123;
  notificationsCount: number = 0;
  isLoading: boolean = false;
  isSending: boolean = false;
  private shouldScrollToBottom = false;
  private responseTimeout: any;

  constructor(
    private router: Router,
    private errorHandler: ErrorHandlerService
  ) {}

  ngOnInit(): void {
    try {
      this.loadUserName();
      this.loadConversations();
    } catch (error) {
      this.errorHandler.handleError(error, 'INIT_COMPONENT');
    }
  }

  ngAfterViewChecked(): void {
    if (this.shouldScrollToBottom) {
      this.scrollToBottom();
      this.shouldScrollToBottom = false;
    }
  }

  ngOnDestroy(): void {
    if (this.responseTimeout) {
      clearTimeout(this.responseTimeout);
    }
  }

  private loadUserName(): void {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        throw new Error('No authentication token found');
      }

      const payload = JSON.parse(atob(token.split('.')[1]));
      this.userName = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || 
                      payload.name || 
                      payload.unique_name || 
                      'User';
    } catch (error) {
      this.errorHandler.handleError(error, 'LOAD_USER');
      this.userName = 'User';
      // Redirect to login if token is invalid
      if (error instanceof Error && error.message.includes('token')) {
        this.router.navigate(['/login']);
      }
    }
  }

  private loadConversations(): void {
    try {
      this.isLoading = true;
      
      // Simulate API call - replace with actual HTTP request
      this.conversations = [
        {
          psychologistId: 101,
          psychologistName: 'Dr. Maria Popescu',
          psychologistAvatar: '👩‍⚕️',
          lastMessage: 'How are you feeling today?',
          lastMessageTime: '2026-01-14T10:30:00',
          unreadCount: 2
        },
        {
          psychologistId: 102,
          psychologistName: 'Dr. Alexandru Ionescu',
          psychologistAvatar: '👨‍⚕️',
          lastMessage: 'Great progress in our last session!',
          lastMessageTime: '2026-01-13T15:20:00',
          unreadCount: 0
        },
        {
          psychologistId: 103,
          psychologistName: 'Dr. Elena Dumitrescu',
          psychologistAvatar: '👩‍⚕️',
          lastMessage: 'Don\'t forget your homework exercises',
          lastMessageTime: '2026-01-12T09:15:00',
          unreadCount: 1
        }
      ];

      if (this.conversations.length > 0) {
        this.selectConversation(this.conversations[0]);
      }

      this.isLoading = false;
    } catch (error) {
      this.isLoading = false;
      this.errorHandler.handleError(error, 'LOAD_CONVERSATIONS');
    }
  }

  selectConversation(conversation: Conversation): void {
    try {
      if (!conversation) {
        throw new Error('Invalid conversation selected');
      }

      this.selectedConversation = conversation;
      this.loadMessages(conversation.psychologistId);
      conversation.unreadCount = 0;
    } catch (error) {
      this.errorHandler.handleError(error, 'SELECT_CONVERSATION');
    }
  }

  private loadMessages(psychologistId: number): void {
    try {
      this.isLoading = true;
      
      // Try to load from localStorage
      const key = `messages_${psychologistId}`;
      const stored = localStorage.getItem(key);
      
      if (stored) {
        this.messages = JSON.parse(stored);
      } else {
        this.messages = this.getDummyMessages(psychologistId);
      }

      this.shouldScrollToBottom = true;
      this.isLoading = false;
    } catch (error) {
      this.isLoading = false;
      this.errorHandler.handleError(error, 'LOAD_MESSAGES');
      this.messages = [];
    }
  }

  sendMessage(): void {
    if (!this.newMessage.trim()) {
      this.errorHandler.showWarning('Please enter a message');
      return;
    }

    if (!this.selectedConversation) {
      this.errorHandler.handleError(new Error('No conversation selected'), 'SEND_MESSAGE');
      return;
    }

    try {
      this.isSending = true;

      const message: Message = {
        id: this.messages.length + 1,
        senderId: this.currentUserId,
        senderName: this.userName,
        senderType: 'patient',
        content: this.newMessage.trim(),
        timestamp: new Date().toISOString(),
        read: false
      };

      this.messages.push(message);
      const sentMessage = this.newMessage;
      this.newMessage = '';
      this.shouldScrollToBottom = true;

      // Update conversation
      this.selectedConversation.lastMessage = message.content;
      this.selectedConversation.lastMessageTime = message.timestamp;

      // Save to localStorage
      this.saveMessages();

      this.isSending = false;
      this.errorHandler.showSuccess('Message sent');

      // Simulate psychologist response
      this.responseTimeout = setTimeout(() => {
        this.simulatePsychologistResponse();
      }, 2000);

    } catch (error) {
      this.isSending = false;
      this.errorHandler.handleError(error, 'SEND_MESSAGE');
      // Restore message on error
      this.newMessage = this.newMessage || '';
    }
  }

  private simulatePsychologistResponse(): void {
    if (!this.selectedConversation) return;

    try {
      const responses = [
        "Thank you for sharing that with me. How does that make you feel?",
        "I understand. Can you tell me more about that?",
        "That's a great insight. Let's explore this further in our next session.",
        "I'm here to support you. Remember to practice the coping techniques we discussed.",
        "It sounds like you're making good progress. Keep up the good work!"
      ];

      const response: Message = {
        id: this.messages.length + 1,
        senderId: this.selectedConversation.psychologistId,
        senderName: this.selectedConversation.psychologistName,
        senderType: 'psychologist',
        content: responses[Math.floor(Math.random() * responses.length)],
        timestamp: new Date().toISOString(),
        read: false
      };

      this.messages.push(response);
      this.shouldScrollToBottom = true;

      this.selectedConversation.lastMessage = response.content;
      this.selectedConversation.lastMessageTime = response.timestamp;

      this.saveMessages();
    } catch (error) {
      this.errorHandler.handleError(error, 'SIMULATE_RESPONSE');
    }
  }

  private saveMessages(): void {
    if (!this.selectedConversation) return;
    
    try {
      const key = `messages_${this.selectedConversation.psychologistId}`;
      localStorage.setItem(key, JSON.stringify(this.messages));
    } catch (error) {
      if (error instanceof Error && error.name === 'QuotaExceededError') {
        this.errorHandler.handleError(
          new Error('Storage limit exceeded. Some messages may not be saved.'),
          'SAVE_MESSAGES'
        );
      } else {
        this.errorHandler.handleError(error, 'SAVE_MESSAGES');
      }
    }
  }

  private scrollToBottom(): void {
    try {
      if (this.messagesContainer?.nativeElement) {
        const element = this.messagesContainer.nativeElement;
        element.scrollTop = element.scrollHeight;
      }
    } catch (error) {
      console.error('Scroll error:', error);
    }
  }

  formatMessageTime(timestamp: string): string {
    try {
      const date = new Date(timestamp);
      if (isNaN(date.getTime())) {
        throw new Error('Invalid date');
      }

      const now = new Date();
      const diff = now.getTime() - date.getTime();
      const minutes = Math.floor(diff / 60000);
      const hours = Math.floor(diff / 3600000);
      const days = Math.floor(diff / 86400000);

      if (minutes < 1) return 'Just now';
      if (minutes < 60) return `${minutes}m ago`;
      if (hours < 24) return `${hours}h ago`;
      if (days < 7) return `${days}d ago`;
      
      return date.toLocaleDateString('en-US', { 
        month: 'short', 
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      });
    } catch (error) {
      return 'Unknown';
    }
  }

  formatLastMessageTime(timestamp: string): string {
    try {
      const date = new Date(timestamp);
      if (isNaN(date.getTime())) {
        throw new Error('Invalid date');
      }

      return date.toLocaleTimeString('en-US', {
        hour: '2-digit',
        minute: '2-digit',
        hour12: false
      });
    } catch (error) {
      return '';
    }
  }

  get unreadMessages(): number {
    return this.conversations.reduce((sum, conv) => sum + conv.unreadCount, 0);
  }

  logout(): void {
    try {
      localStorage.removeItem('token');
      this.router.navigate(['/login']);
    } catch (error) {
      this.errorHandler.handleError(error, 'LOGOUT');
    }
  }

  private getDummyMessages(psychologistId: number): Message[] {
    const baseMessages: Message[] = [
      {
        id: 1,
        senderId: psychologistId,
        senderName: this.conversations.find(c => c.psychologistId === psychologistId)?.psychologistName || 'Psychologist',
        senderType: 'psychologist',
        content: 'Hello! How have you been feeling since our last session?',
        timestamp: '2026-01-14T09:00:00',
        read: true
      },
      {
        id: 2,
        senderId: this.currentUserId,
        senderName: this.userName,
        senderType: 'patient',
        content: 'Hi Dr. I\'ve been doing better. The breathing exercises really help.',
        timestamp: '2026-01-14T09:15:00',
        read: true
      },
      {
        id: 3,
        senderId: psychologistId,
        senderName: this.conversations.find(c => c.psychologistId === psychologistId)?.psychologistName || 'Psychologist',
        senderType: 'psychologist',
        content: 'That\'s wonderful to hear! Have you been practicing them daily?',
        timestamp: '2026-01-14T09:20:00',
        read: true
      },
      {
        id: 4,
        senderId: this.currentUserId,
        senderName: this.userName,
        senderType: 'patient',
        content: 'Yes, every morning and before bed. It helps me stay calm.',
        timestamp: '2026-01-14T09:25:00',
        read: true
      },
      {
        id: 5,
        senderId: psychologistId,
        senderName: this.conversations.find(c => c.psychologistId === psychologistId)?.psychologistName || 'Psychologist',
        senderType: 'psychologist',
        content: 'Excellent! Keep up the good work. How are you feeling today?',
        timestamp: '2026-01-14T10:30:00',
        read: false
      }
    ];

    return baseMessages;
  }
}