import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login.component/login.component').then(
        (m) => m.LoginComponent
      ),
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/dashboard.component').then(
        (m) => m.DashboardComponent
      ),
  },
  {
    path: 'patient',
    loadComponent: () =>
      import('./features/patients/patient-dashboard/patient-dashboard').then(
        (m) => m.PatientDashboardComponent
      ),
  },
  {
    path: 'appointments/patient',
    loadComponent: () =>
      import('./features/patients/appointments-patient/appointments-patient.component').then(
        (m) => m.AppointmentsPatientComponent,
      ),
  },
    {
    path: 'tests/patient',
    loadComponent: () =>
      import('./features/patients/tests/tests.component').then(
        (m) => m.TestsComponent
      ),
  },
  {
    path: 'messages/patient',
    loadComponent: () =>
      import('./features/patients/messages-patient/messages-patient.component').then(
        (m) => m.MessagesPatientComponent
      ),
  },
  
  {
    path: 'patients',
    loadComponent: () =>
      import('./features/patients/patients.component').then(
        (m) => m.PatientsComponent
      ),
  },
  {
    path: 'appointments',
    loadComponent: () =>
      import('./features/appointments/appointments.component').then(
        (m) => m.AppointmentsComponent
      ),
  },
  {
    path: 'billing',
    loadComponent: () =>
      import('./features/billing/billing.component').then(
        (m) => m.BillingComponent
      ),
  },
  {
    path: 'messages',
    loadComponent: () =>
      import('./features/messages/messages.component').then(
        (m) => m.MessagesComponent
      ),
  },
  {
    path: 'settings',
    loadComponent: () =>
      import('./features/settings/settings.component').then(
        (m) => m.SettingsComponent
      ),
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
