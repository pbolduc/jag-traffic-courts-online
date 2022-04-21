import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService } from '@config/config.service';
import { ApiHttpErrorResponse } from '@core/models/api-http-error-response.model';
import { ApiHttpResponse } from '@core/models/api-http-response.model';
import { LoggerService } from '@core/services/logger.service';
import { ToastService } from '@core/services/toast.service';
import { DisputesService, TicketsService, OcrViolationTicket } from 'app/api';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class DisputeResourceService {
  constructor(
    private toastService: ToastService,
    private logger: LoggerService,
    private configService: ConfigService,
    private ticketAPIService: TicketsService,
    private disputeAPIService: DisputesService
  ) { }

  /**
   * Get the ticket from RSI.
   *
   * @param params containing the ticketNumber and time
   */
  public getTicket(params: {
    ticketNumber: string;
    time: string;
  }): Observable<any> {

    return this.ticketAPIService.apiTicketsSearchGet(params.ticketNumber, params.time)
      .pipe(
        map((response: any) =>
          response ? response : null
        ),
        tap((updatedTicket) =>
          this.logger.info('DisputeResourceService::getTicket', updatedTicket)
        ),
        catchError((error: any) => {
          this.toastService.openErrorToast(this.configService.ticket_error);
          this.logger.error(
            'DisputeResourceService::getTicket error has occurred: ',
            error
          );
          throw error;
        })
      );
  }

  	public postTicket(
	  image: any
  	): Observable<OcrViolationTicket> {
      const formData = new FormData();
      formData.append('image',image)
	  return this.ticketAPIService.apiTicketsAnalysePost(image)
	    .pipe(
	      map((response: any) =>
	        response ? response : null
	      ),
	      map((ticket: any) => {
	        if (ticket) {
	        console.log('service',ticket)
	        }

	        return ticket;
	      }),
	      tap((updatedTicket) =>
	        this.logger.info('DisputeResourceService::getTicket', updatedTicket)
	      ),
	      catchError((error: any) => {
	        this.toastService.openErrorToast(this.configService.ticket_error);
	        this.logger.error(
	          'DisputeResourceService::getTicket error has occurred: ',
	          error
	        );
	        throw error;
	      })
	    );
	}

  /**
   * Create the ticket dispute
   *
   * @param ticketDispute The dispute to be created
   */
  public createTicketDispute(ticketDispute: any): Observable<any> {
    const ticketToCreate = this.cleanTicketDispute(ticketDispute);
    this.logger.info('DisputeResourceService::createTicketDispute', ticketToCreate);

    var result: any;
    return new Observable();
  }
  //   return this.disputeAPIService.apiDisputesCreatePost(ticketDispute).pipe(
  //     map((response: ApiHttpResponse<TicketDisputeView>) =>
  //       response ? response.result : null
  //     ),
  //     map((ticket: TicketDisputeView) => {
  //       if (ticket) {
  //         this.updateTicketViewModel(ticket);
  //       }
  //       return ticket;
  //     }),
  //     tap((updatedTicket) => {
  //       this.toastService.openSuccessToast(
  //         'The request has been successfully submitted'
  //       );

  //       this.logger.info(
  //         'DisputeResourceService::NEW_DISPUTE_TICKET',
  //         updatedTicket
  //       );
  //     }),
  //     catchError((error: any) => {
  //       this.toastService.openErrorToast('The request could not be created');
  //       this.logger.error(
  //         'DisputeResourceService::createTicketDispute error has occurred: ',
  //         error
  //       );
  //       throw error;
  //     })
  //   );
  // }

  /**
   * Create the shell ticket
   *
   * @param ticket The ticket to be created
   */
  // public createShellTicket(ticket: any): Observable<any> {
  public createShellTicket(ticket: any) {
    const ticketToCreate = this.cleanShellTicket(ticket);
    this.logger.info('DisputeResourceService::createShellTicket', ticketToCreate);

    // return this.ticketAPIService.apiTicketsShellticketPost(ticket)
    //   .pipe(
    //     map((response: ApiHttpResponse<any>) =>
    //       response ? response.result : null
    //     ),
    //     map((savedTicket: any) => {
    //       if (savedTicket) {
    //         this.updateTicketViewModel(savedTicket);
    //       }
    //       return savedTicket;
    //     }),
    //     tap((updatedTicket) => {
    //       this.toastService.openSuccessToast(
    //         'The ticket has been successfully created'
    //       );

    //       this.logger.info(
    //         'DisputeResourceService:: NEW_SHELL_TICKET',
    //         updatedTicket
    //       );
    //     }),
    //     catchError((error: ApiHttpErrorResponse) => {
    //       if (Array.isArray(error.errors) && error.errors.length > 0) {
    //         const msg = error.errors.join(' ');
    //         this.toastService.openErrorToast(msg);
    //       } else {
    //         this.toastService.openErrorToast('Ticket could not be created');
    //       }
    //       this.logger.error(
    //         'DisputeResourceService::createShellTicket error has occurred: ',
    //         error
    //       );
    //       throw error;
    //     })
    //   );
  }

  /**
   * @description
   * strip the calculated fields from the object
   */
  private cleanTicketDispute(ticket: any) {
    const ticketDispute = { ...ticket };
    for (const property in ticket) {
      if (property.charAt(0) === '_') {
        delete ticketDispute[property];
      }
    }
    return ticketDispute;
  }

  /**
   * @description
   * strip the calculated fields from the object
   */
  private cleanShellTicket(ticket: any) {
    const shellTicket = { ...ticket };

    // cleanup payload data
    if (shellTicket._chargeCount < 3) {
      shellTicket._count3ChargeDesc = null;
      shellTicket._count3ChargeSection = null;
      // shellTicket.count3Charge = null;
      // shellTicket.count3FineAmount = null;
    }

    // cleanup payload data
    if (shellTicket._chargeCount < 2) {
      shellTicket._count2ChargeDesc = null;
      shellTicket._count2ChargeSection = null;
      // shellTicket.count2Charge = null;
      // shellTicket.count2FineAmount = null;
    }

    for (const property in ticket) {
      if (property.charAt(0) === '_') {
        delete shellTicket[property];
      }
    }
    return shellTicket;
  }
}
