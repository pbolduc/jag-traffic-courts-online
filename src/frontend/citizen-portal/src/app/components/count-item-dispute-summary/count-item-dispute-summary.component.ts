import { Component, Input, OnInit } from '@angular/core';
// import { OffenceView } from '@shared/models/offenceView.model';

@Component({
  selector: 'app-count-item-dispute-summary',
  templateUrl: './count-item-dispute-summary.component.html',
  styleUrls: ['./count-item-dispute-summary.component.scss'],
})
export class CountItemDisputeSummaryComponent implements OnInit {
  @Input() public count;

  constructor() { // do nothing.
  }

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnInit(): void {
  }
}
