import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild, ElementRef, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatSliderModule } from '@angular/material/slider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { Chart, ChartConfiguration, registerables } from 'chart.js';
import { MoodService, Mood } from '../../../services/mood.service';

Chart.register(...registerables);

interface MoodDisplay {
  score: number;
  date: string;
}

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatSliderModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatToolbarModule,
    MatIconModule,
    MatMenuModule,
    MatBadgeModule,
  ],
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.scss'],
})
export class PatientDashboardComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('moodChartCanvas') moodChartCanvas!: ElementRef<HTMLCanvasElement>;
  
  selectedMood: number = 5;
  moodHistory: MoodDisplay[] = [];
  chart: Chart | null = null;
  loading: boolean = true;
  chartReady: boolean = false;
  userName: string = '';
  unreadMessages: number = 0;
  notificationsCount: number = 0;

  moodLegend = [
    { range: '1–2', label: 'Very sad 😞', color: '#f44336' },
    { range: '3–4', label: 'Sad 😕', color: '#ff9800' },
    { range: '5', label: 'Okay 🙂', color: '#ffc107' },
    { range: '6–7', label: 'Good 😊', color: '#8bc34a' },
    { range: '8–9', label: 'Happy 😄', color: '#4caf50' },
    { range: '10', label: 'Excellent 🥳', color: '#2196f3' },
  ];

  constructor(
    private moodService: MoodService,
    private snackBar: MatSnackBar,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private ngZone: NgZone
  ) {}

  ngOnInit(): void {
    this.loadUserName();
    this.loadNotifications();
    this.loadMoodHistory();
  }

  ngAfterViewInit(): void {
    this.chartReady = true;
    this.cdr.detectChanges();
    
    // Try to render chart if data is already loaded
    if (this.moodHistory.length > 0 && !this.loading) {
      setTimeout(() => this.renderChart(), 100);
    }
  }

  ngOnDestroy(): void {
    if (this.chart) {
      this.chart.destroy();
      this.chart = null;
    }
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

  private loadNotifications(): void {
    this.unreadMessages = 0;
    this.notificationsCount = 0;
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('moodHistory');
    this.router.navigate(['/login']);
  }

  getEmotion(score: number): string {
    if (score <= 2) return 'Very sad 😞';
    if (score <= 4) return 'Sad 😕';
    if (score === 5) return 'Okay 🙂';
    if (score <= 7) return 'Good 😊';
    if (score <= 9) return 'Happy 😄';
    return 'Excellent 🥳';
  }

  getMoodColor(score: number): string {
    if (score <= 2) return '#f44336';
    if (score <= 4) return '#ff9800';
    if (score === 5) return '#ffc107';
    if (score <= 7) return '#8bc34a';
    if (score <= 9) return '#4caf50';
    return '#2196f3';
  }

  loadMoodHistory(): void {
    const localMoods = this.loadFromLocalStorage();
    
    // If we have local data, show it immediately
    if (localMoods.length > 0) {
      this.moodHistory = localMoods.sort((a, b) => 
        new Date(a.date).getTime() - new Date(b.date).getTime()
      );
      
      this.ngZone.run(() => {
        this.loading = false;
        this.cdr.detectChanges();
      });
      
      if (this.chartReady) {
        setTimeout(() => this.renderChart(), 100);
      }
    }
    
    // Load from API
    console.log('Calling getMoodHistory API...');
    this.moodService.getMoodHistory().subscribe({
      next: (moods: Mood[]) => {
        console.log('API Response:', moods);
        
        const apiMoods = moods.map(m => ({
          score: m.score,
          date: m.completionDate.split('T')[0]
        }));
        
        const combined = this.mergeMoods(apiMoods, localMoods);
        
        this.moodHistory = combined.sort((a, b) => 
          new Date(a.date).getTime() - new Date(b.date).getTime()
        );
        
        // Save merged data to localStorage
        this.saveToLocalStorage();
        
        this.ngZone.run(() => {
          this.loading = false;
          this.cdr.detectChanges();
        });
        
        if (this.chartReady) {
          setTimeout(() => this.renderChart(), 100);
        }
      },
      error: (error) => {
        console.error('Error loading moods from API:', error);
        
        // Use local data if API fails
        if (localMoods.length === 0) {
          // Create sample data if nothing exists
          this.moodHistory = this.createSampleData();
          this.saveToLocalStorage();
        }
        
        this.ngZone.run(() => {
          this.loading = false;
          this.cdr.detectChanges();
        });
        
        if (this.chartReady) {
          setTimeout(() => this.renderChart(), 100);
        }
        
        const message = localMoods.length > 0 ? 'Using local data' : 'Could not load mood history';
        this.snackBar.open(message, 'OK', { duration: 3000 });
      }
    });
  }

  private loadFromLocalStorage(): MoodDisplay[] {
    try {
      const stored = localStorage.getItem('moodHistory');
      if (stored) {
        const parsed = JSON.parse(stored);
        if (Array.isArray(parsed)) {
          const validMoods = parsed.filter(
            (item): item is MoodDisplay => 
              typeof item === 'object' &&
              typeof item.score === 'number' &&
              typeof item.date === 'string'
          );
          if (validMoods.length > 0) {
            console.log('Loaded from localStorage:', validMoods.length, 'moods');
            return validMoods;
          }
        }
      }
    } catch (error) {
      console.error('Error loading from localStorage:', error);
    }
    return [];
  }

  private createSampleData(): MoodDisplay[] {
    const today = new Date();
    const sampleData: MoodDisplay[] = [];
    
    // Create 7 days of sample data
    for (let i = 6; i >= 0; i--) {
      const date = new Date(today);
      date.setDate(date.getDate() - i);
      const dateStr = date.toISOString().split('T')[0];
      
      // Generate a varied mood pattern
      let score: number;
      if (i === 6) score = 4;
      else if (i === 5) score = 5;
      else if (i === 4) score = 7;
      else if (i === 3) score = 6;
      else if (i === 2) score = 8;
      else if (i === 1) score = 7;
      else score = 5;
      
      sampleData.push({ score, date: dateStr });
    }
    
    console.log('Created sample mood data:', sampleData);
    return sampleData;
  }

  private saveToLocalStorage(): void {
    try {
      localStorage.setItem('moodHistory', JSON.stringify(this.moodHistory));
      console.log('Saved to localStorage:', this.moodHistory.length, 'moods');
    } catch (error) {
      console.error('Error saving to localStorage:', error);
    }
  }

  private mergeMoods(apiMoods: MoodDisplay[], localMoods: MoodDisplay[]): MoodDisplay[] {
    const moodMap = new Map<string, MoodDisplay>();
    
    // Add local moods first
    localMoods.forEach(mood => {
      moodMap.set(mood.date, mood);
    });
    
    // API moods override local moods for same date
    apiMoods.forEach(mood => {
      moodMap.set(mood.date, mood);
    });
    
    console.log('Merged moods - API:', apiMoods.length, 'Local:', localMoods.length, 'Total:', moodMap.size);
    return Array.from(moodMap.values());
  }

  submitMood(): void {
    const now = new Date();
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const day = String(now.getDate()).padStart(2, '0');
    const today = `${year}-${month}-${day}`;

    const newMood: MoodDisplay = { score: this.selectedMood, date: today };
    const isUpdate = this.moodHistory.some(m => m.date === today);

    // Update immediately for better UX (optimistic update)
    this.updateMoodHistory(newMood);
    this.saveToLocalStorage();
    
    // Update chart smoothly without full re-render
    this.updateChartData(newMood, isUpdate);
    
    // Send to API
    console.log('Submitting mood to API:', newMood);
    this.moodService.addMood(this.selectedMood).subscribe({
      next: (response) => {
        console.log('Mood saved to API successfully:', response);
        this.snackBar.open('Mood saved successfully!', '✓', { duration: 2000 });
      },
      error: (error) => {
        console.error('Error submitting mood to API:', error);
        this.snackBar.open('Saved locally (API unavailable)', '✓', { duration: 2000 });
      }
    });
  }

  private updateChartData(newMood: MoodDisplay, isUpdate: boolean): void {
    if (!this.chart) {
      this.renderChart();
      return;
    }

    const date = new Date(newMood.date + 'T00:00:00');
    const label = date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });

    if (isUpdate) {
      // Find and update existing point
      const index = this.chart.data.labels?.findIndex(l => l === label);
      if (index !== undefined && index !== -1 && this.chart.data.datasets[0].data[index] !== undefined) {
        this.chart.data.datasets[0].data[index] = newMood.score;
        const pointColors = this.chart.data.datasets[0].backgroundColor as string[];
        if (pointColors && pointColors[index]) {
          pointColors[index] = this.getMoodColor(newMood.score);
        }
      }
    } else {
      // Add new point
      this.chart.data.labels?.push(label);
      this.chart.data.datasets[0].data.push(newMood.score);
      
      const pointColors = this.chart.data.datasets[0].backgroundColor as string[];
      if (pointColors) {
        pointColors.push(this.getMoodColor(newMood.score));
      }
    }

    // Smooth update animation
    this.chart.update('active');
  }

  private updateMoodHistory(newMood: MoodDisplay): void {
    const existingIndex = this.moodHistory.findIndex(m => m.date === newMood.date);
    
    if (existingIndex > -1) {
      this.moodHistory[existingIndex] = newMood;
      console.log('Updated existing mood for', newMood.date);
    } else {
      this.moodHistory.push(newMood);
      console.log('Added new mood for', newMood.date);
    }

    this.moodHistory.sort((a, b) => 
      new Date(a.date).getTime() - new Date(b.date).getTime()
    );
  }

  renderChart(): void {
    if (!this.chartReady || !this.moodChartCanvas?.nativeElement) {
      console.log('Chart not ready yet');
      return;
    }
    
    const canvas = this.moodChartCanvas.nativeElement;
    const ctx = canvas.getContext('2d');
    
    if (!ctx) {
      console.error('Could not get canvas context');
      return;
    }

    // Destroy existing chart
    if (this.chart) {
      this.chart.destroy();
      this.chart = null;
    }

    if (this.moodHistory.length === 0) {
      console.log('No mood history to display');
      return;
    }

    console.log('Rendering chart with', this.moodHistory.length, 'data points');

    const gradient = ctx.createLinearGradient(0, 0, 0, 400);
    gradient.addColorStop(0, 'rgba(33, 150, 243, 0.3)');
    gradient.addColorStop(0.5, 'rgba(76, 175, 80, 0.2)');
    gradient.addColorStop(1, 'rgba(255, 152, 0, 0.1)');

    const labels = this.moodHistory.map(m => {
      const date = new Date(m.date + 'T00:00:00');
      const isMobile = window.innerWidth < 768;
      if (isMobile && this.moodHistory.length > 7) {
        // Show only month and day on mobile if many points
        return date.toLocaleDateString('en-US', { month: 'numeric', day: 'numeric' });
      }
      return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
    });

    const dataPoints = this.moodHistory.map(m => m.score);
    
    // Responsive font sizes
    const isMobile = window.innerWidth < 768;
    const isSmallMobile = window.innerWidth < 480;
    const fontSize = isSmallMobile ? 10 : isMobile ? 11 : 12;
    const pointRadius = isSmallMobile ? 5 : isMobile ? 6 : 7;

    const config: ChartConfiguration = {
      type: 'line',
      data: {
        labels: labels,
        datasets: [
          {
            label: 'Mood Score',
            data: dataPoints,
            fill: true,
            backgroundColor: gradient,
            borderColor: '#2196f3',
            borderWidth: isMobile ? 2 : 3,
            tension: 0.4,
            pointBackgroundColor: dataPoints.map(score => this.getMoodColor(score)),
            pointBorderColor: '#fff',
            pointBorderWidth: isMobile ? 2 : 3,
            pointRadius: pointRadius,
            pointHoverRadius: pointRadius + 3,
            pointHoverBorderWidth: isMobile ? 2 : 3,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        interaction: {
          intersect: false,
          mode: 'index',
        },
        scales: {
          y: { 
            min: 0, 
            max: 11,
            ticks: { 
              stepSize: 1,
              font: { size: fontSize },
              callback: (value) => {
                if (isSmallMobile) {
                  if (value === 1) return '😞';
                  if (value === 5) return '😐';
                  if (value === 10) return '🥳';
                  return value;
                }
                if (value === 1) return '😞 1';
                if (value === 5) return '😐 5';
                if (value === 10) return '🥳 10';
                return value;
              }
            },
            grid: {
              color: 'rgba(0, 0, 0, 0.05)',
              drawOnChartArea: true,
              drawTicks: false
            }
          },
          x: {
            grid: {
              display: false,
              drawOnChartArea: false
            },
            ticks: {
              font: { size: fontSize },
              maxRotation: isMobile ? 45 : 0,
              minRotation: isMobile ? 45 : 0,
              autoSkip: true,
              maxTicksLimit: isSmallMobile ? 5 : isMobile ? 7 : 10
            }
          }
        },
        plugins: { 
          legend: { 
            display: false
          },
          tooltip: {
            backgroundColor: 'rgba(0, 0, 0, 0.85)',
            padding: isMobile ? 12 : 16,
            titleFont: {
              size: isMobile ? 13 : 15,
              weight: 'bold'
            },
            bodyFont: {
              size: isMobile ? 12 : 14
            },
            borderColor: '#2196f3',
            borderWidth: 1,
            displayColors: false,
            callbacks: {
              title: (context) => {
                const index = context[0].dataIndex;
                return this.moodHistory[index].date;
              },
              label: (context) => {
                const score = context.parsed.y;
                if (score === null) return '';
                return `${this.getEmotion(score)} (${score}/10)`;
              }
            }
          }
        },
        animation: {
          duration: 1000,
          easing: 'easeInOutQuart'
        }
      },
    };

    try {
      this.chart = new Chart(ctx, config);
      console.log('Chart rendered successfully');
    } catch (error) {
      console.error('Error creating chart:', error);
    }
  }
}