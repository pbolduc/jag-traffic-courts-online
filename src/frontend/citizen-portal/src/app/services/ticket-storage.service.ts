import { Injectable } from "@angular/core"

@Injectable({
  providedIn: 'root'
})
export class TicketStorageService {
  public imageData = {}

  public setImageData(data) {
    this.imageData = data
  }

  public getImageData() {
    return this.imageData
  }
}