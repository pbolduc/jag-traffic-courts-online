import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatStepper } from '@angular/material/stepper';
import { ActivatedRoute, Router } from '@angular/router';
import { FormUtilsService } from '@core/services/form-utils.service';
import { LoggerService } from '@core/services/logger.service';
import { UtilsService } from '@core/services/utils.service';
import { Address } from '@shared/models/address.model';
import { BaseDisputeFormPage } from 'app/components/classes/BaseDisputeFormPage';
import { DisputeFormStateService } from 'app/services/dispute-form-state.service';
import { DisputeResourceService } from 'app/services/dispute-resource.service';
import { DisputeService } from 'app/services/dispute.service';

@Component({
  selector: 'app-step-disputant',
  templateUrl: './step-disputant.component.html',
  styleUrls: ['./step-disputant.component.scss'],
})
export class StepDisputantComponent
  extends BaseDisputeFormPage
  implements OnInit {
  @Input() public stepper: MatStepper;
  @Output() public stepSave: EventEmitter<MatStepper> = new EventEmitter();
  @Output() public stepCancel: EventEmitter<MatStepper> = new EventEmitter();

  public isMobile: boolean;
  public previousButtonIcon = 'keyboard_arrow_left';
  public previousButtonKey = 'stepper.backReview';
  public saveButtonKey = 'stepper.next';
  public provinces: any = [
    {
      "name": "Alberta",
      "abbreviation": "AB"
    },
    {
      "name": "British Columbia",
      "abbreviation": "BC"
    },
    {
      "name": "Manitoba",
      "abbreviation": "MB"
    },
    {
      "name": "New Brunswick",
      "abbreviation": "NB"
    },
    {
      "name": "Newfoundland and Labrador",
      "abbreviation": "NL"
    },
    {
      "name": "Northwest Territories",
      "abbreviation": "NT"
    },
    {
      "name": "Nova Scotia",
      "abbreviation": "NS"
    },
    {
      "name": "Nunavut",
      "abbreviation": "NU"
    },
    {
      "name": "Ontario",
      "abbreviation": "ON"
    },
    {
      "name": "Prince Edward Island",
      "abbreviation": "PE"
    },
    {
      "name": "Quebec",
      "abbreviation": "QC"
    },
    {
      "name": "Saskatchewan",
      "abbreviation": "SK"
    },
    {
      "name": "Yukon Territory",
      "abbreviation": "YT"
    }
  ]

  public maxDateOfBirth: Date;

  private MINIMUM_AGE = 18;

  /**
   * @description
   * Whether to show the address line fields.
   * Commented out address lookup functionality until license issues resolved
   */
  //  public showManualButton: boolean;
  //  public showAddressFields: boolean;

  constructor(
    protected route: ActivatedRoute,
    protected router: Router,
    protected formBuilder: FormBuilder,
    protected disputeService: DisputeService,
    protected disputeResource: DisputeResourceService,
    protected disputeFormStateService: DisputeFormStateService,
    private formUtilsService: FormUtilsService,
    private utilsService: UtilsService,
    private logger: LoggerService
  ) {
    super(
      route,
      router,
      formBuilder,
      disputeService,
      disputeResource,
      disputeFormStateService
    );
    const today = new Date();
    this.maxDateOfBirth = new Date();
    this.maxDateOfBirth.setFullYear(today.getFullYear() - this.MINIMUM_AGE);
    this.isMobile = this.utilsService.isMobile();
    // this.showManualButton = true;
    // this.showAddressFields = false;
  }

  public ngOnInit() {
    this.form = this.disputeFormStateService.stepDisputantForm;
    this.patchForm().then(() => {
      // this.showManualButton = !this.mailingAddress.value;
      // this.showAddressFields = !!this.mailingAddress.value;
    });
  }

  public onBack() {
    this.stepCancel.emit();
  }

  public onSubmit(): void {
    if (this.formUtilsService.checkValidity(this.form)) {
      this.stepSave.emit(this.stepper);
    } else {
      this.utilsService.scrollToErrorSection();
    }
  }

  /**
   * Updates form fields with Canada Post Autocomplete Retrieve result
   */
  public onAutocomplete({ countryCode, provinceCode, postalCode, address, city }: Address): void {
    this.form.patchValue({country: countryCode});
    this.form.patchValue({province: provinceCode});
    this.form.patchValue({postalCode});
    this.form.patchValue({mailingAddress: address});
    this.form.patchValue({city});
    // this.showManualButton = !address;
    // this.showAddressFields = !!address;
  }

  // public showManualAddress(): void {
  //   this.showAddressFields = true;
  // }

  public get phoneNumber(): FormControl {
    return this.form.get('phoneNumber') as FormControl;
  }

  public get workPhone(): FormControl {
    return this.form.get('workPhone') as FormControl;
  }

  public get emailAddress(): FormControl {
    return this.form.get('emailAddress') as FormControl;
  }

  public get mailingAddress(): FormControl {
    return this.form.get('mailingAddress') as FormControl;
  }
}
