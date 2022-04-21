import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, ErrorHandler, NgModule } from '@angular/core';
// import { ConfigService } from '@config/config.service';
import { AppConfigService } from 'app/services/app-config.service';
import { ErrorHandlerInterceptor } from './interceptors/error-handler.interceptor';
import { ErrorHandlerService } from './services/error-handler.service';

export function initConfig(config: AppConfigService) {
  return () => {
    return config.loadAppConfig();
  };
}

@NgModule({
  declarations: [],
  imports: [CommonModule], // , KeycloakModule],
  providers: [
    {
      provide: ErrorHandler,
      useClass: ErrorHandlerService,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorHandlerInterceptor,
      multi: true,
    },
    {
      provide: APP_INITIALIZER,
      useFactory: initConfig,
      deps: [AppConfigService],
      multi: true,
    },
  ],
})
export class CoreModule {}
