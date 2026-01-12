import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatSliderModule } from '@angular/material/slider';
import { Chart, ChartConfiguration, ChartType } from 'chart.js';
import { MoodService, Mood } from '../../../services/mood.service';

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatSliderModule,
  ],
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.scss'],
})
export class PatientDashboardComponent implements OnInit {
  selectedMood = 5;
  moodHistory: Mood[] = [];
  chart: Chart | null = null;

  constructor(private moodService: MoodService) {}

  ngOnInit(): void {
    this.loadMoodHistory();
  }

  loadMoodHistory() {
    this.moodService.getMoodHistory().subscribe((moods) => {
      this.moodHistory = moods;
      this.renderChart();
    });
  }

  submitMood() {
    this.moodService.addMood(this.selectedMood).subscribe({
      next: () => {
        this.loadMoodHistory();
      },
      error: (err) => {
        console.error('Failed to add mood', err);
      },
    });
  }

  renderChart() {
    const ctx = document.getElementById('moodChart') as HTMLCanvasElement;
    if (!ctx) return;

    const labels = this.moodHistory.map((m) =>
      new Date(m.completion_date).toLocaleDateString()
    );
    const data = this.moodHistory.map((m) => m.score);

    if (this.chart) {
      this.chart.destroy();
    }

    const config: ChartConfiguration = {
      type: 'line' as ChartType,
      data: {
        labels,
        datasets: [
          {
            label: 'Mood score',
            data,
            backgroundColor: 'rgba(33, 150, 243, 0.2)',
            borderColor: 'rgba(33, 150, 243, 1)',
            borderWidth: 2,
            fill: true,
            tension: 0.3,
          },
        ],
      },
      options: {
        responsive: true,
        scales: {
          y: { min: 1, max: 10, ticks: { stepSize: 1 } },
        },
      },
    };

    this.chart = new Chart(ctx, config);
  }
}
