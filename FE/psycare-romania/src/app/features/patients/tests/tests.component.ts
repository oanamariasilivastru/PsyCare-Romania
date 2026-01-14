import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatRadioModule } from '@angular/material/radio';
import { FormsModule } from '@angular/forms';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { PSYCHOLOGICAL_TESTS, PsychologicalTest, TestQuestion } from './tests-data';

interface TestAnswer {
  questionId: number;
  selectedScore: number;
}

interface TestProgress {
  testId: string;
  answers: TestAnswer[];
  totalScore?: number;
  completedAt?: string;
  result?: string;
}

@Component({
  selector: 'app-tests',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatProgressBarModule,
    MatRadioModule,
    MatToolbarModule,
    MatMenuModule,
    MatBadgeModule,
    MatSnackBarModule
  ],
  templateUrl: './tests.component.html',
  styleUrls: ['./tests.component.scss']
})
export class TestsComponent implements OnInit {
  tests = PSYCHOLOGICAL_TESTS;
  selectedTest: PsychologicalTest | null = null;
  currentQuestionIndex: number = 0;
  answers: TestAnswer[] = [];
  selectedAnswer: number | null = null;
  showResults: boolean = false;
  testResult: any = null;
  userName: string = '';
  unreadMessages: number = 0;
  notificationsCount: number = 0;

  constructor(
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadUserName();
    this.loadTestProgress();
  }

  private loadUserName(): void {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        this.userName = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || 
                        payload.name || 
                        payload.unique_name || 
                        'User';
      } catch (error) {
        console.error('Error parsing token:', error);
        this.userName = 'User';
      }
    }
  }

  private loadTestProgress(): void {
    // Load any saved progress from localStorage
    const savedProgress = localStorage.getItem('testProgress');
    if (savedProgress) {
      try {
        const progress: TestProgress = JSON.parse(savedProgress);
        // You can restore progress here if needed
      } catch (error) {
        console.error('Error loading test progress:', error);
      }
    }
  }

  startTest(test: PsychologicalTest): void {
    this.selectedTest = test;
    this.currentQuestionIndex = 0;
    this.answers = [];
    this.selectedAnswer = null;
    this.showResults = false;
    this.testResult = null;
  }

  selectAnswer(score: number): void {
    this.selectedAnswer = score;
  }

  nextQuestion(): void {
    if (this.selectedAnswer === null) {
      this.snackBar.open('Please select an answer', 'OK', { duration: 2000 });
      return;
    }

    // Save answer
    const existingIndex = this.answers.findIndex(
      a => a.questionId === this.currentQuestion.id
    );

    if (existingIndex > -1) {
      this.answers[existingIndex].selectedScore = this.selectedAnswer;
    } else {
      this.answers.push({
        questionId: this.currentQuestion.id,
        selectedScore: this.selectedAnswer
      });
    }

    // Move to next question or show results
    if (this.currentQuestionIndex < this.selectedTest!.questions.length - 1) {
      this.currentQuestionIndex++;
      this.selectedAnswer = this.getAnswerForCurrentQuestion();
    } else {
      this.calculateResults();
    }

    // Save progress
    this.saveProgress();
  }

  previousQuestion(): void {
    if (this.currentQuestionIndex > 0) {
      this.currentQuestionIndex--;
      this.selectedAnswer = this.getAnswerForCurrentQuestion();
    }
  }

  private getAnswerForCurrentQuestion(): number | null {
    const answer = this.answers.find(
      a => a.questionId === this.currentQuestion.id
    );
    return answer ? answer.selectedScore : null;
  }

  private calculateResults(): void {
    const totalScore = this.answers.reduce((sum, answer) => sum + answer.selectedScore, 0);
    
    const scoreRange = this.selectedTest!.scoring.ranges.find(
      range => totalScore >= range.min && totalScore <= range.max
    );

    this.testResult = {
      totalScore,
      scoreRange,
      completedAt: new Date().toISOString()
    };

    this.showResults = true;
    this.saveCompletedTest();
  }

  private saveProgress(): void {
    if (!this.selectedTest) return;

    const progress: TestProgress = {
      testId: this.selectedTest.id,
      answers: this.answers
    };

    localStorage.setItem('testProgress', JSON.stringify(progress));
  }

  private saveCompletedTest(): void {
    if (!this.selectedTest || !this.testResult) return;

    const completedTest: TestProgress = {
      testId: this.selectedTest.id,
      answers: this.answers,
      totalScore: this.testResult.totalScore,
      completedAt: this.testResult.completedAt,
      result: this.testResult.scoreRange.label
    };

    // Save to localStorage (you can also send to API here)
    const completedTests = this.getCompletedTests();
    completedTests.push(completedTest);
    localStorage.setItem('completedTests', JSON.stringify(completedTests));

    // Clear progress
    localStorage.removeItem('testProgress');

    this.snackBar.open('Test completed successfully!', '✓', { duration: 3000 });
  }

  private getCompletedTests(): TestProgress[] {
    const stored = localStorage.getItem('completedTests');
    if (stored) {
      try {
        return JSON.parse(stored);
      } catch {
        return [];
      }
    }
    return [];
  }

  backToTests(): void {
    this.selectedTest = null;
    this.currentQuestionIndex = 0;
    this.answers = [];
    this.selectedAnswer = null;
    this.showResults = false;
    this.testResult = null;
  }

  retakeTest(): void {
    if (this.selectedTest) {
      this.startTest(this.selectedTest);
    }
  }

  get currentQuestion(): TestQuestion {
    return this.selectedTest!.questions[this.currentQuestionIndex];
  }

  get progress(): number {
    if (!this.selectedTest) return 0;
    return ((this.currentQuestionIndex + 1) / this.selectedTest.questions.length) * 100;
  }

  get progressText(): string {
    if (!this.selectedTest) return '';
    return `${this.currentQuestionIndex + 1} / ${this.selectedTest.questions.length}`;
  }

  getSeverityColor(severity: string): string {
    switch (severity) {
      case 'minimal': return '#4caf50';
      case 'mild': return '#8bc34a';
      case 'moderate': return '#ff9800';
      case 'severe': return '#f44336';
      default: return '#9e9e9e';
    }
  }

  isTestCompleted(testId: string): boolean {
    const completedTests = this.getCompletedTests();
    return completedTests.some(t => t.testId === testId);
  }

  getLastCompletedDate(testId: string): string | null {
    const completedTests = this.getCompletedTests();
    const test = completedTests.find(t => t.testId === testId);
    if (test && test.completedAt) {
      const date = new Date(test.completedAt);
      return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
    }
    return null;
  }

  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}