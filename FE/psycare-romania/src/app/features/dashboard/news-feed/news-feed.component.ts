import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { TimeAgoPipe } from '../../../pipes/time-ago.pipe';

interface NewsItem {
  snippet: any;
  title: string;
  content: string;
  imageUrl: string | null;
  date: string;
}

@Component({
  selector: 'app-news-feed',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, TimeAgoPipe],
  templateUrl: './news-feed.component.html',
  styleUrl: './news-feed.component.scss',
})
export class NewsFeedComponent implements OnInit {
  psychologyNews: NewsItem[] = [
    {
      title: 'Study Shows Mindfulness Reduces Anxiety',
      snippet: 'New research finds mindfulness meditation significantly lowers anxiety levels.',
      content:
        'A recent study conducted by the University of California found that participants practicing mindfulness meditation for 8 weeks experienced a measurable reduction in anxiety and stress markers. Researchers believe mindfulness helps rewire emotional regulation pathways in the brain.',
      imageUrl:
        'https://images.unsplash.com/photo-1506126613408-eca07ce68773?q=80&w=799&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
      date: '2026-01-12',
    },
    {
      title: 'Social Media Usage Linked to Sleep Problems',
      snippet: 'Excessive social media can disrupt sleep patterns, according to psychologists.',
      content:
        'Psychologists warn that heavy use of social media late at night is associated with insomnia and fragmented sleep. The study suggests limiting screen time before bed to improve mental health and cognitive function.',
      imageUrl:
        'https://images.unsplash.com/photo-1721815731590-d4aae3a7fc9f?q=80&w=2012&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
      date: '2026-01-14',
    },
    {
      title: 'Cognitive Behavioral Therapy Helps Combat Depression',
      snippet: 'CBT continues to be an effective tool for managing depressive symptoms.',
      content:
        'Therapists report that Cognitive Behavioral Therapy (CBT) provides patients with practical techniques to challenge negative thoughts and develop healthier coping strategies, showing significant improvements in mood and resilience.',
      imageUrl:
        'https://images.unsplash.com/photo-1620147461831-a97b99ade1d3?q=80&w=1527&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
      date: '2026-01-10',
    },
    {
      title: 'New App Tracks Mental Health Through Daily Journals',
      snippet: 'Technology merges with psychology to help monitor mood and mental wellbeing.',
      content:
        'A new mobile app allows users to track daily emotions, stress levels, and thought patterns. The data can be shared with mental health professionals, providing better insights for therapy and early intervention.',
      imageUrl:
        'https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?q=80&w=2080&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
      date: '2026-01-08',
    },
    {
      title: 'Exercise Found to Improve Cognitive Function in Older Adults',
      snippet: 'Physical activity boosts brain health and memory retention.',
      content:
        'A longitudinal study found that adults over 60 who engage in regular aerobic exercise show improved memory, attention, and executive function. Researchers suggest combining physical and mental workouts for optimal brain health.',
      imageUrl:
        'https://images.unsplash.com/photo-1707985287164-c84627ad6eba?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
      date: '2026-01-05',
    },
    {
      title: 'Virtual Reality Therapy Shows Promise for PTSD Patients',
      snippet: 'VR exposure therapy could transform treatment for trauma survivors.',
      content:
        'Psychologists are using immersive VR experiences to help PTSD patients confront and process traumatic memories safely. Early trials indicate reduced symptoms and improved coping strategies.',
      imageUrl:
        'https://images.unsplash.com/photo-1670804317345-f8602cf7dab2?q=80&w=910&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
      date: '2026-01-02',
    },
    {
      title: 'Sleep Deprivation Impacts Emotional Regulation',
      snippet: 'Lack of sleep can heighten negative emotional responses.',
      content:
        'New research highlights that even partial sleep deprivation can impair the brain’s ability to regulate emotions, making individuals more reactive and less capable of problem-solving.',
      imageUrl:
        'https://images.unsplash.com/photo-1660912354460-6e6c83e6d59f?q=80&w=1964&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
      date: '2025-12-30',
    },
    {
      title: 'Music Therapy Shows Benefits for Anxiety',
      snippet: 'Listening to music can reduce stress and improve mood.',
      content:
        'Therapists are increasingly using music therapy to help clients with anxiety disorders. Structured listening and active participation in music sessions can decrease cortisol levels and improve overall emotional wellbeing.',
      imageUrl:
        'https://images.unsplash.com/photo-1488376739361-ed24c9beb6d0?q=80&w=2076&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
      date: '2025-12-28',
    },
    {
      title: 'Positive Psychology Techniques Improve Workplace Satisfaction',
      snippet: 'Simple interventions increase employee happiness and productivity.',
      content:
        'Companies adopting positive psychology exercises like gratitude journaling and strength-based feedback report higher morale and engagement among employees. Experts emphasize consistency and integration into company culture.',
      imageUrl:
        'https://images.unsplash.com/photo-1510074377623-8cf13fb86c08?q=80&w=1172&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
      date: '2025-12-25',
    },
    {
      title: 'Childhood Trauma Linked to Adult Health Risks',
      snippet: 'Early adverse experiences have long-term psychological effects.',
      content:
        'A meta-analysis shows that individuals exposed to trauma in childhood are at higher risk for depression, anxiety, and chronic health conditions later in life. Interventions in early years are critical for resilience.',
      imageUrl:
        'https://images.unsplash.com/photo-1723823932575-c36ea477f48e?q=80&w=693&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
      date: '2025-12-22',
    },
  ];

  ngOnInit(): void {
    this.psychologyNews.sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
  }
}
