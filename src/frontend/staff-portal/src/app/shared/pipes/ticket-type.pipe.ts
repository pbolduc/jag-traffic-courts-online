import { Pipe, PipeTransform } from '@angular/core';
import { ticketTypes } from '@shared/enums/ticket-type.enum';

@Pipe({
  name: 'ticketType',
})
export class TicketTypePipe implements PipeTransform {
  transform(value: any, explicit: string = null): string {
    // Null check to allow for default pipe chaining, but allow
    // for an explicit yes or no if required
    if (value === 'S') {
      return ticketTypes.CAMERA_TICKET;
    } else if (value === 'E') {
      return ticketTypes.ELECTRONIC_TICKET;
    } else if (value === 'A') {
      return ticketTypes.HANDWRITTEN_TICKET;
    }
    return null;
  }
}
