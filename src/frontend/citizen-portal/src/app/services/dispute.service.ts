import { Injectable } from '@angular/core';
import { ShellTicketData } from '@shared/models/shellTicketData.model';
// import { TicketDisputeView } from '@shared/models/ticketDisputeView.model';
import { BehaviorSubject } from 'rxjs';

export interface IDisputeService {
  ticket$: BehaviorSubject<any>;
  ticket: any;
}

@Injectable({
  providedIn: 'root',
})
export class DisputeService implements IDisputeService {
  private _ticket: BehaviorSubject<any>;
  private _shellTicketData: BehaviorSubject<ShellTicketData>;

  constructor() {
    this._ticket = new BehaviorSubject<any>(null);
    this._shellTicketData = new BehaviorSubject<ShellTicketData>(null);
  }

  public get shellTicketData$(): BehaviorSubject<ShellTicketData> {
    return this._shellTicketData;
  }

  public get ticket$(): BehaviorSubject<any> {
    return this._ticket;
  }

  public get ticket(): any {
    return this._ticket.value;
  }

  public get shellTicketData(): ShellTicketData {
    return this._shellTicketData.value;
  }
  // public imageData={}

    public setTicketData(data){
     this._ticket=data

    }

    // public getImageData(){
    //     return this.imageData
   
    //    }
}
