export interface TestQuestion {
  id: number;
  text: string;
  options: {
    text: string;
    score: number;
  }[];
}

export interface PsychologicalTest {
  id: string;
  title: string;
  shortName: string;
  description: string;
  duration: string;
  category: string;
  icon: string;
  color: string;
  recommendedBy: string;
  instructions: string;
  questions: TestQuestion[];
  scoring: {
    ranges: {
      min: number;
      max: number;
      label: string;
      description: string;
      severity: 'none' | 'minimal' | 'mild' | 'moderate' | 'severe';
    }[];
  };
}

export const PSYCHOLOGICAL_TESTS: PsychologicalTest[] = [
  {
    id: 'phq9',
    title: 'Patient Health Questionnaire-9',
    shortName: 'PHQ-9',
    description: 'Screening tool for depression severity',
    duration: '5-10 minutes',
    category: 'Depression',
    icon: 'psychology',
    color: '#1976d2',
    recommendedBy: 'Dr. Maria Ionescu',
    instructions: 'Over the last 2 weeks, how often have you been bothered by any of the following problems?',
    questions: [
      {
        id: 1,
        text: 'Little interest or pleasure in doing things',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 2,
        text: 'Feeling down, depressed, or hopeless',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 3,
        text: 'Trouble falling or staying asleep, or sleeping too much',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 4,
        text: 'Feeling tired or having little energy',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 5,
        text: 'Poor appetite or overeating',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 6,
        text: 'Feeling bad about yourself or that you are a failure or have let yourself or your family down',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 7,
        text: 'Trouble concentrating on things, such as reading the newspaper or watching television',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 8,
        text: 'Moving or speaking so slowly that other people could have noticed. Or the opposite - being so fidgety or restless that you have been moving around a lot more than usual',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 9,
        text: 'Thoughts that you would be better off dead, or of hurting yourself in some way',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      }
    ],
    scoring: {
      ranges: [
        { min: 0, max: 4, label: 'Minimal Depression', description: 'Your symptoms suggest minimal or no depression.', severity: 'minimal' },
        { min: 5, max: 9, label: 'Mild Depression', description: 'Your symptoms suggest mild depression.', severity: 'mild' },
        { min: 10, max: 14, label: 'Moderate Depression', description: 'Your symptoms suggest moderate depression.', severity: 'moderate' },
        { min: 15, max: 19, label: 'Moderately Severe Depression', description: 'Your symptoms suggest moderately severe depression.', severity: 'moderate' },
        { min: 20, max: 27, label: 'Severe Depression', description: 'Your symptoms suggest severe depression.', severity: 'severe' }
      ]
    }
  },
  {
    id: 'gad7',
    title: 'Generalized Anxiety Disorder-7',
    shortName: 'GAD-7',
    description: 'Assessment tool for anxiety symptoms',
    duration: '3-5 minutes',
    category: 'Anxiety',
    icon: 'error_outline',
    color: '#f57c00',
    recommendedBy: 'Dr. Maria Ionescu',
    instructions: 'Over the last 2 weeks, how often have you been bothered by the following problems?',
    questions: [
      {
        id: 1,
        text: 'Feeling nervous, anxious, or on edge',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 2,
        text: 'Not being able to stop or control worrying',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 3,
        text: 'Worrying too much about different things',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 4,
        text: 'Trouble relaxing',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 5,
        text: 'Being so restless that it is hard to sit still',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 6,
        text: 'Becoming easily annoyed or irritable',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      },
      {
        id: 7,
        text: 'Feeling afraid, as if something awful might happen',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'Several days', score: 1 },
          { text: 'More than half the days', score: 2 },
          { text: 'Nearly every day', score: 3 }
        ]
      }
    ],
    scoring: {
      ranges: [
        { min: 0, max: 4, label: 'Minimal Anxiety', description: 'Your symptoms suggest minimal anxiety.', severity: 'minimal' },
        { min: 5, max: 9, label: 'Mild Anxiety', description: 'Your symptoms suggest mild anxiety.', severity: 'mild' },
        { min: 10, max: 14, label: 'Moderate Anxiety', description: 'Your symptoms suggest moderate anxiety.', severity: 'moderate' },
        { min: 15, max: 21, label: 'Severe Anxiety', description: 'Your symptoms suggest severe anxiety.', severity: 'severe' }
      ]
    }
  },
  {
    id: 'pcl5',
    title: 'PTSD Checklist for DSM-5',
    shortName: 'PCL-5',
    description: 'Screening for Post-Traumatic Stress Disorder',
    duration: '5-10 minutes',
    category: 'PTSD',
    icon: 'emergency',
    color: '#d32f2f',
    recommendedBy: 'Dr. Maria Ionescu',
    instructions: 'In the past month, how much were you bothered by:',
    questions: [
      {
        id: 1,
        text: 'Repeated, disturbing, and unwanted memories of the stressful experience',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'A little bit', score: 1 },
          { text: 'Moderately', score: 2 },
          { text: 'Quite a bit', score: 3 },
          { text: 'Extremely', score: 4 }
        ]
      },
      {
        id: 2,
        text: 'Repeated, disturbing dreams of the stressful experience',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'A little bit', score: 1 },
          { text: 'Moderately', score: 2 },
          { text: 'Quite a bit', score: 3 },
          { text: 'Extremely', score: 4 }
        ]
      },
      {
        id: 3,
        text: 'Suddenly feeling or acting as if the stressful experience were actually happening again',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'A little bit', score: 1 },
          { text: 'Moderately', score: 2 },
          { text: 'Quite a bit', score: 3 },
          { text: 'Extremely', score: 4 }
        ]
      },
      {
        id: 4,
        text: 'Feeling very upset when something reminded you of the stressful experience',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'A little bit', score: 1 },
          { text: 'Moderately', score: 2 },
          { text: 'Quite a bit', score: 3 },
          { text: 'Extremely', score: 4 }
        ]
      },
      {
        id: 5,
        text: 'Having strong physical reactions when something reminded you of the stressful experience',
        options: [
          { text: 'Not at all', score: 0 },
          { text: 'A little bit', score: 1 },
          { text: 'Moderately', score: 2 },
          { text: 'Quite a bit', score: 3 },
          { text: 'Extremely', score: 4 }
        ]
      }
    ],
    scoring: {
      ranges: [
        { min: 0, max: 10, label: 'Minimal PTSD', description: 'Your symptoms suggest minimal PTSD symptoms.', severity: 'minimal' },
        { min: 11, max: 20, label: 'Mild PTSD', description: 'Your symptoms suggest mild PTSD.', severity: 'mild' },
        { min: 21, max: 30, label: 'Moderate PTSD', description: 'Your symptoms suggest moderate PTSD.', severity: 'moderate' },
        { min: 31, max: 50, label: 'Severe PTSD', description: 'Your symptoms suggest severe PTSD.', severity: 'severe' }
      ]
    }
  },
  {
    id: 'rosenberg',
    title: 'Rosenberg Self-Esteem Scale',
    shortName: 'RSES',
    description: 'Measure of self-esteem and self-worth',
    duration: '3-5 minutes',
    category: 'Self-Esteem',
    icon: 'favorite',
    color: '#7b1fa2',
    recommendedBy: 'Dr. Maria Ionescu',
    instructions: 'Please indicate how much you agree or disagree with each statement:',
    questions: [
      {
        id: 1,
        text: 'I feel that I am a person of worth, at least on an equal plane with others',
        options: [
          { text: 'Strongly Disagree', score: 0 },
          { text: 'Disagree', score: 1 },
          { text: 'Agree', score: 2 },
          { text: 'Strongly Agree', score: 3 }
        ]
      },
      {
        id: 2,
        text: 'I feel that I have a number of good qualities',
        options: [
          { text: 'Strongly Disagree', score: 0 },
          { text: 'Disagree', score: 1 },
          { text: 'Agree', score: 2 },
          { text: 'Strongly Agree', score: 3 }
        ]
      },
      {
        id: 3,
        text: 'All in all, I am inclined to feel that I am a failure',
        options: [
          { text: 'Strongly Disagree', score: 3 },
          { text: 'Disagree', score: 2 },
          { text: 'Agree', score: 1 },
          { text: 'Strongly Agree', score: 0 }
        ]
      },
      {
        id: 4,
        text: 'I am able to do things as well as most other people',
        options: [
          { text: 'Strongly Disagree', score: 0 },
          { text: 'Disagree', score: 1 },
          { text: 'Agree', score: 2 },
          { text: 'Strongly Agree', score: 3 }
        ]
      },
      {
        id: 5,
        text: 'I feel I do not have much to be proud of',
        options: [
          { text: 'Strongly Disagree', score: 3 },
          { text: 'Disagree', score: 2 },
          { text: 'Agree', score: 1 },
          { text: 'Strongly Agree', score: 0 }
        ]
      },
      {
        id: 6,
        text: 'I take a positive attitude toward myself',
        options: [
          { text: 'Strongly Disagree', score: 0 },
          { text: 'Disagree', score: 1 },
          { text: 'Agree', score: 2 },
          { text: 'Strongly Agree', score: 3 }
        ]
      },
      {
        id: 7,
        text: 'On the whole, I am satisfied with myself',
        options: [
          { text: 'Strongly Disagree', score: 0 },
          { text: 'Disagree', score: 1 },
          { text: 'Agree', score: 2 },
          { text: 'Strongly Agree', score: 3 }
        ]
      },
      {
        id: 8,
        text: 'I wish I could have more respect for myself',
        options: [
          { text: 'Strongly Disagree', score: 3 },
          { text: 'Disagree', score: 2 },
          { text: 'Agree', score: 1 },
          { text: 'Strongly Agree', score: 0 }
        ]
      },
      {
        id: 9,
        text: 'I certainly feel useless at times',
        options: [
          { text: 'Strongly Disagree', score: 3 },
          { text: 'Disagree', score: 2 },
          { text: 'Agree', score: 1 },
          { text: 'Strongly Agree', score: 0 }
        ]
      },
      {
        id: 10,
        text: 'At times I think I am no good at all',
        options: [
          { text: 'Strongly Disagree', score: 3 },
          { text: 'Disagree', score: 2 },
          { text: 'Agree', score: 1 },
          { text: 'Strongly Agree', score: 0 }
        ]
      }
    ],
    scoring: {
      ranges: [
        { min: 0, max: 14, label: 'Low Self-Esteem', description: 'Your score suggests low self-esteem. Consider discussing this with your therapist.', severity: 'severe' },
        { min: 15, max: 25, label: 'Moderate Self-Esteem', description: 'Your score suggests moderate self-esteem with room for improvement.', severity: 'moderate' },
        { min: 26, max: 30, label: 'High Self-Esteem', description: 'Your score suggests healthy self-esteem.', severity: 'minimal' }
      ]
    }
  },
  {
    id: 'pss',
    title: 'Perceived Stress Scale',
    shortName: 'PSS-10',
    description: 'Assessment of stress levels',
    duration: '5 minutes',
    category: 'Stress',
    icon: 'speed',
    color: '#388e3c',
    recommendedBy: 'Dr. Maria Ionescu',
    instructions: 'In the last month, how often have you:',
    questions: [
      {
        id: 1,
        text: 'Been upset because of something that happened unexpectedly?',
        options: [
          { text: 'Never', score: 0 },
          { text: 'Almost Never', score: 1 },
          { text: 'Sometimes', score: 2 },
          { text: 'Fairly Often', score: 3 },
          { text: 'Very Often', score: 4 }
        ]
      },
      {
        id: 2,
        text: 'Felt that you were unable to control the important things in your life?',
        options: [
          { text: 'Never', score: 0 },
          { text: 'Almost Never', score: 1 },
          { text: 'Sometimes', score: 2 },
          { text: 'Fairly Often', score: 3 },
          { text: 'Very Often', score: 4 }
        ]
      },
      {
        id: 3,
        text: 'Felt nervous and stressed?',
        options: [
          { text: 'Never', score: 0 },
          { text: 'Almost Never', score: 1 },
          { text: 'Sometimes', score: 2 },
          { text: 'Fairly Often', score: 3 },
          { text: 'Very Often', score: 4 }
        ]
      },
      {
        id: 4,
        text: 'Felt confident about your ability to handle your personal problems?',
        options: [
          { text: 'Never', score: 4 },
          { text: 'Almost Never', score: 3 },
          { text: 'Sometimes', score: 2 },
          { text: 'Fairly Often', score: 1 },
          { text: 'Very Often', score: 0 }
        ]
      },
      {
        id: 5,
        text: 'Felt that things were going your way?',
        options: [
          { text: 'Never', score: 4 },
          { text: 'Almost Never', score: 3 },
          { text: 'Sometimes', score: 2 },
          { text: 'Fairly Often', score: 1 },
          { text: 'Very Often', score: 0 }
        ]
      },
      {
        id: 6,
        text: 'Found that you could not cope with all the things that you had to do?',
        options: [
          { text: 'Never', score: 0 },
          { text: 'Almost Never', score: 1 },
          { text: 'Sometimes', score: 2 },
          { text: 'Fairly Often', score: 3 },
          { text: 'Very Often', score: 4 }
        ]
      },
      {
        id: 7,
        text: 'Been able to control irritations in your life?',
        options: [
          { text: 'Never', score: 4 },
          { text: 'Almost Never', score: 3 },
          { text: 'Sometimes', score: 2 },
          { text: 'Fairly Often', score: 1 },
          { text: 'Very Often', score: 0 }
        ]
      },
      {
        id: 8,
        text: 'Felt that you were on top of things?',
        options: [
          { text: 'Never', score: 4 },
          { text: 'Almost Never', score: 3 },
          { text: 'Sometimes', score: 2 },
          { text: 'Fairly Often', score: 1 },
          { text: 'Very Often', score: 0 }
        ]
      },
      {
        id: 9,
        text: 'Been angered because of things that were outside of your control?',
        options: [
          { text: 'Never', score: 0 },
          { text: 'Almost Never', score: 1 },
          { text: 'Sometimes', score: 2 },
          { text: 'Fairly Often', score: 3 },
          { text: 'Very Often', score: 4 }
        ]
      },
      {
        id: 10,
        text: 'Felt difficulties were piling up so high that you could not overcome them?',
        options: [
          { text: 'Never', score: 0 },
          { text: 'Almost Never', score: 1 },
          { text: 'Sometimes', score: 2 },
          { text: 'Fairly Often', score: 3 },
          { text: 'Very Often', score: 4 }
        ]
      }
    ],
    scoring: {
      ranges: [
        { min: 0, max: 13, label: 'Low Stress', description: 'Your stress levels are low.', severity: 'minimal' },
        { min: 14, max: 26, label: 'Moderate Stress', description: 'Your stress levels are moderate.', severity: 'moderate' },
        { min: 27, max: 40, label: 'High Stress', description: 'Your stress levels are high. Consider stress management techniques.', severity: 'severe' }
      ]
    }
  }
];