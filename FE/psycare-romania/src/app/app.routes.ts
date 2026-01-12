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
