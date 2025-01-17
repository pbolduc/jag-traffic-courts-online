import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DisputeTicketStepperComponent } from '@components/dispute-ticket-stepper/dispute-ticket-stepper.component';
import { ScanTicketComponent } from '@components/scan-ticket/scan-ticket.component';
import { TicketPageComponent } from '@components/ticket-page/ticket-page.component';
import { FeatureFlagGuard } from '@core/guards/feature-flag.guard';
import { AppRoutes } from './app.routes';
import { DisputeSubmitSuccessComponent } from './components/dispute-submit-success/dispute-submit-success.component';
import { InitiateResolutionComponent } from './components/initiate-resolution/initiate-resolution.component';
import { FindTicketComponent } from './components/find-ticket/find-ticket.component';
import { LandingComponent } from './components/landing/landing.component';

const routes: Routes = [
  {
    path: AppRoutes.TICKET,
    component: TicketPageComponent,
    canActivate: [FeatureFlagGuard],
    data: { featureFlag: 'dispute' },
    children: [
      {
        path: AppRoutes.FIND,
        component: FindTicketComponent,
      },
      {
        path: AppRoutes.SCAN,
        component: ScanTicketComponent,
      },
      {
        path: AppRoutes.SUBMIT_SUCCESS,
        component: DisputeSubmitSuccessComponent,
      },
      {
        path: AppRoutes.SUMMARY,
        component: InitiateResolutionComponent,
      },
      {
        path: AppRoutes.STEPPER,
        component: DisputeTicketStepperComponent,
      },
      {
        path: '',
        redirectTo: AppRoutes.FIND,
        pathMatch: 'full',
      },
    ],
  },
  {
    path: '',
    component: LandingComponent,
  },
  {
    path: '**',
    redirectTo: '/',
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })],
  exports: [RouterModule],
})
export class AppRoutingModule { }
