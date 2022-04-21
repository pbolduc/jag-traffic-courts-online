import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators
} from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Config } from '@config/config.model';
// import { ConfigService } from '@config/config.service';
import { FormUtilsService } from '@core/services/form-utils.service';
import { LoggerService } from '@core/services/logger.service';
import { UtilsService } from '@core/services/utils.service';
import { ConfirmDialogComponent } from '@shared/dialogs/confirm-dialog/confirm-dialog.component';
import { DialogOptions } from '@shared/dialogs/dialog-options.model';
import { ShellTicketData } from '@shared/models/shellTicketData.model';
// import { ShellTicketView } from '@shared/models/shellTicketView.model';
// import { TicketDisputeView } from '@shared/models/ticketDisputeView.model';
import { Configuration, TicketsService } from 'app/api';
import { AppRoutes } from 'app/app.routes';
import { AppConfigService } from 'app/services/app-config.service';
import { DisputeResourceService } from 'app/services/dispute-resource.service';
import { DisputeService } from 'app/services/dispute.service';
import { NgProgress, NgProgressRef } from 'ngx-progressbar';
import { Subscription } from 'rxjs';
import *  as moment from 'moment';
import { TicketStorageService } from 'app/services/ticket-storage.service';


@Component({
  selector: 'app-shell-ticket',
  templateUrl: './shell-ticket.component.html',
  styleUrls: ['./shell-ticket.component.scss'],
})
export class ShellTicketComponent implements OnInit {
  public busy: Subscription | Promise<any>;
  public ticketImageSrc: string;
  public ticketFilename: string;
  public form: FormGroup;
  public todayDate: Date = new Date();
  public maxDateOfBirth: Date;
  public isMobile: boolean;
  protected basePath = '';
  public configuration = new Configuration();
  isHidden = true;
  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;

  private progressRef: NgProgressRef;
  private MINIMUM_AGE = 18;
  public fieldsData;
  constructor(
    private formBuilder: FormBuilder,
    private formUtilsService: FormUtilsService,
    private disputeService: DisputeService,
    // private configService: ConfigService,
    private utilsService: UtilsService,
    private dialog: MatDialog,
    private router: Router,
    private ngProgress: NgProgress,
    private logger: LoggerService,
    public ticketService: TicketsService,
    private ticketStorageService: TicketStorageService,
    private http: HttpClient,
  ) {
    if (typeof this.configuration.basePath !== 'string') {
      this.configuration.basePath = this.basePath;
    }
    this.progressRef = this.ngProgress.ref();

    this.maxDateOfBirth = new Date();
    this.maxDateOfBirth.setFullYear(
      this.todayDate.getFullYear() - this.MINIMUM_AGE
    );
    this.isMobile = this.utilsService.isMobile();
    this.fieldsData = this.ticketStorageService.getImageData()
    this.form = this.formBuilder.group({
      violationTicketNumber: [null, [Validators.required]],
      violationDate: [null, [Validators.required]],
      violationTime: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      givenNames: [null, [Validators.required]],
      gender: [null],
      address: [null],
      city: [null],
      province: [null],
      postalCode: [null],
      driverLicenseNumber: [null],
      driverLicenseProvince: [null],
      count1Charge: [
        null
      ],
      count1FineAmount: [
        null
      ],
      count2Charge: [
        null
      ],
      count2FineAmount: [
        null
      ],
      count3Charge: [
        null,
      ],
      count3FineAmount: [
        null,
      ],
      courtHearingLocation: [null],
      detachmentLocation: [null],
      _chargeCount: [1],
      _amountOwing: [null],
      _count1ChargeDesc: [null],
      _count2ChargeDesc: [null],
      _count3ChargeDesc: [null]
    });
    this.onFulfilled()
    this.disputeService.shellTicketData$.subscribe((shellTicketData) => {
      if (!shellTicketData) {
        this.router.navigate([AppRoutes.disputePath(AppRoutes.FIND)]);
        return;
      }

      this.ticketImageSrc = shellTicketData.ticketImage;
      this.ticketFilename = shellTicketData.filename;
    });
  }

  public ngOnInit(): void {
    this.form.disable();
  }

  public toggle() {
    this.isHidden = !this.isHidden;
  }

  public onSubmit(): void {
    const validity = this.formUtilsService.checkValidity(this.form);
    const errors = this.formUtilsService.getFormErrors(this.form);

    if (!validity) {
      this.utilsService.scrollToErrorSection();
      return;
    }

    const data: DialogOptions = {
      titleKey: 'Are you sure all ticket information is correct?',
      messageKey: `Please ensure that all entered fields match the paper ticket copy exactly.
        If you are not sure, please go back and update any fields as needed before submitting ticket information.`,
      actionTextKey: 'Yes I am sure, create online ticket',
      cancelTextKey: 'Go back and edit',
    };
    const payload = this.form.getRawValue();

    const data2
      =
    {
      violationTicketNumber: payload.violationTicketNumber,
      violationDate: payload.violationDate,
      violationTime: payload.violationTime ? payload.violationTime.replace(' ', ':') : '',
      offences: [
        {
          offenceNumber: 1,
          ticketedAmount: payload.count1FineAmount,
          amountDue: payload.count1FineAmount,
          violationDateTime: '2020-09-18T00:00:00',
          offenceDescription: payload._count1ChargeDesc,
          invoiceType: 'Traffic Violation Ticket', // should be unused
          vehicleDescription: null,
          discountAmount: 25,
          status: 'New',
          offenceAgreementStatus: null,
          requestReduction: false,
          requestMoreTime: false,
          reductionAppearInCourt: false,
          _applyToAllCounts: false,
          _allowApplyToAllCounts: false,
          _firstOffence: false,
          _within30days: false,
          _amountDue: payload.count1FineAmount,
          _offenceStatusDesc: 'Active',
          _offenceStatus: 'New'
        },
        {
          offenceNumber: 2,
          ticketedAmount: payload.count2FineAmount,
          amountDue: payload.count2FineAmount,
          violationDateTime: '2020-09-18T00:00:00',
          offenceDescription: payload._count2ChargeDesc,
          invoiceType: 'Traffic Violation Ticket',
          vehicleDescription: null,
          discountAmount: null,
          status: 'New',
          offenceAgreementStatus: null,
          requestReduction: false,
          requestMoreTime: false,
          reductionAppearInCourt: false,
          _applyToAllCounts: false,
          _allowApplyToAllCounts: false,
          _firstOffence: false,
          _within30days: false,
          _amountDue: payload.count2FineAmount,
          _offenceStatusDesc: 'Active',
          _offenceStatus: 'New'
        },
        {
          offenceNumber: 3,
          ticketedAmount: payload.count3FineAmount,
          amountDue: payload.count3FineAmount,
          violationDateTime: '2020-09-18T00:00:00',
          offenceDescription:
            payload._count3ChargeDesc,
          invoiceType: 'Traffic Violation Ticket',
          vehicleDescription: null,
          discountAmount: 25,
          status: 'New',
          offenceAgreementStatus: null,
          requestReduction: false,
          requestMoreTime: false,
          reductionAppearInCourt: false,
          _applyToAllCounts: false,
          _allowApplyToAllCounts: false,
          _firstOffence: false,
          _within30days: false,
          _amountDue: payload.count3FineAmount,
          _offenceStatusDesc: 'Active',
          _offenceStatus: 'New'
        }
      ]
    }

    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .subscribe((response: boolean) => {
        if (response) {
          const payload = this.form.getRawValue();

          this.disputeService.ticket$.next(data2);

          this.router.navigate([AppRoutes.disputePath(AppRoutes.SUMMARY)], {
            queryParams: {
              ticketNumber: payload.violationTicketNumber,
              time: payload.violationTime ? payload.violationTime.replace(' ', ':') : '',
            },
          });
        }
      });
  }

  public onFileChange(event: any) {
    let filename: string;
    let ticketImage: string;

    this.ticketImageSrc = null;
    this.ticketFilename = null;
    this.form.reset();

    if (!event.target.files[0] || event.target.files[0].length === 0) {
      this.logger.info('You must select an image');
      return;
    }

    const mimeType = event.target.files[0].type;

    if (mimeType.match(/image\/*/) == null) {
      this.logger.info('Only images are supported');
      return;
    }

    this.progressRef.start();

    const reader = new FileReader();
    const ticketFile: File = event.target.files[0];
    this.logger.info('file target', event.target.files[0]);

    filename = ticketFile.name;
    reader.readAsDataURL(ticketFile);
    this.logger.info('file', ticketFile.name, ticketFile.lastModified);

    reader.onload = () => {
      ticketImage = reader.result as string;

      const shellTicketData: ShellTicketData = {
        filename,
        ticketImage,
        ticketFile,
      };
      const fd = new FormData();
      fd.append('file', ticketFile);

      this.http.post(`${this.configuration.basePath}/api/tickets/analyse`, fd)
        .subscribe(res => {
          console.log('image data 2', res);
          this.fieldsData = res;
          this.ticketStorageService.setImageData(res);
          this.onFulfilled()
          this.disputeService.shellTicketData$.next(shellTicketData);
          this.router.navigate([AppRoutes.disputePath(AppRoutes.SHELL)]);
        })
    };
  }

  private onFulfilled() {
    const invoices = this.fieldsData

    if (!invoices || invoices.length <= 0) {
      throw new Error('Expecting at least one invoice in analysis result');
    }

    const invoice = invoices;
    this.logger.info('First invoice:', invoice);
    if (!invoice.fields) {
      return true
    }

    const invoiceIdFieldIndex = 'violationTicketNumber';
    const invoiceIdField = invoice.fields[invoiceIdFieldIndex];
    if (invoiceIdField.type === 'String') {
      this.logger.info(
        `  violationTicketNumber: '${invoiceIdField.value || '<missing>'
        }', with confidence of ${invoiceIdField.confidence}`
      );
    }
    const invoiceDateFieldIndex = 'violationDate';
    const invoiceDateField = invoice.fields[invoiceDateFieldIndex];
    if (invoiceIdField.type === 'Date') {
      this.logger.info(
        `  violationTicketNumber: '${invoiceDateField.value || '<missing>'
        }', with confidence of ${invoiceDateField.confidence}`
      );
    }
    const invoiceTimeFieldIndex = 'violationTime';
    const invoiceTimeField = invoice.fields[invoiceTimeFieldIndex];
    if (invoiceTimeField.type === 'Time') {
      this.logger.info(
        `  violationTicketNumber: '${invoiceTimeField.value || '<missing>'
        }', with confidence of ${invoiceTimeField.confidence}`
      );
    }
    const surnameFieldIndex = 'surname';
    const surnameField = invoice.fields[surnameFieldIndex];
    if (surnameField.type === 'String') {
      this.logger.info(
        `  surname: '${surnameField.value || '<missing>'
        }', with confidence of ${surnameField.fieldConfidence}`
      );
    }
    const givenNameFieldIndex = 'givenName';
    const givenNameField = invoice.fields[givenNameFieldIndex];
    if (givenNameField.type === 'String') {
      this.logger.info(
        `  given name: '${givenNameField.value || '<missing>'
        }', with confidence of ${givenNameField.confidence}`
      );
    }
    const driverLicenceFieldIndex = 'driverLicenceNumber';
    const driverLicenceField = invoice.fields[driverLicenceFieldIndex];
    if (driverLicenceField.type === 'String') {
      this.logger.info(
        `  given name: '${driverLicenceField.value || '<missing>'
        }', with confidence of ${driverLicenceField.confidence}`
      );
    }
    const detachmentLocationFieldIndex = 'detachmentLocation';
    const detachmentLocationField = invoice.fields[detachmentLocationFieldIndex];
    if (detachmentLocationField.type === 'String') {
      this.logger.info(
        `  given name: '${detachmentLocationField.value || '<missing>'
        }', with confidence of ${detachmentLocationField.confidence}`
      );
    }
    const count1DescFieldIndex = 'count1Description';
    const count1DescField = invoice.fields[count1DescFieldIndex];
    if (count1DescField.type === 'String') {
      this.logger.info(
        `  count 1 description: '${count1DescField.value || '<missing>'
        }', with confidence of ${count1DescField.confidence}`
      );
    }
    const count1SectionFieldIndex = 'count1Section';
    const count1SectionField = invoice.fields[count1SectionFieldIndex];
    if (count1SectionField.valueType === 'String') {
      this.logger.info(
        `  count 1 section: '${count1SectionField.value || '<missing>'
        }', with confidence of ${count1SectionField.confidence}`
      );
    }
    const count1TicketAmountFieldIndex = 'count1TicketAmount';
    const count1TicketAmountField =
      invoice.fields[count1TicketAmountFieldIndex];
    if (count1TicketAmountField.type === 'Double') {
      this.logger.info(
        `  count 1 ticket amount: '${count1TicketAmountField.value || '<missing>'
        }', with confidence of ${count1TicketAmountField.confidence}`
      );
    }
    const driverLicenseProvinceIndex = 'driverLicenceProvince';
    const driverLicenceProvinceField = invoice.fields[driverLicenseProvinceIndex];

    const count1ActRegsFieldIndex = 'count1ActRegs';
    const count1ActRegsField = invoice.fields[count1ActRegsFieldIndex];
    if (count1ActRegsField.type === 'String') {
      this.logger.info(
        `  count 1 ticket amount: '${count1ActRegsField.value || '<missing>'
        }', with confidence of ${count1ActRegsField.confidence}`
      );
    }
    const count2DescFieldIndex = 'count2Description';
    const count2DescField = invoice.fields[count2DescFieldIndex];
    if (count2DescField.type === 'String') {
      this.logger.info(
        `  count 2 description: '${count2DescField.value || '<missing>'
        }', with confidence of ${count2DescField.confidence}`
      );
    }
    const count2SectionFieldIndex = 'count2Section';
    const count2SectionField = invoice.fields[count2SectionFieldIndex];
    if (count2SectionField.type === 'String') {
      this.logger.info(
        `  count 2 section: '${count2SectionField.value || '<missing>'
        }', with confidence of ${count2SectionField.confidence}`
      );
    }
    const count2TicketAmountFieldIndex = 'count2TicketAmount';
    const count2TicketAmountField =
      invoice.fields[count2TicketAmountFieldIndex];
    if (count2TicketAmountField.type === 'Double') {
      this.logger.info(
        `  count 2 ticket amount: '${count2TicketAmountField.value || '<missing>'
        }', with confidence of ${count2TicketAmountField.confidence}`
      );
    }
    const count2ActRegsFieldIndex = 'count2ActRegs';
    const count2ActRegsField = invoice.fields[count2ActRegsFieldIndex];
    if (count2ActRegsField.type === 'String') {
      this.logger.info(
        `  count 2 ticket amount: '${count2ActRegsField.value || '<missing>'
        }', with confidence of ${count2ActRegsField.confidence}`
      );
    }
    const count3DescFieldIndex = 'count3Description';
    const count3DescField = invoice.fields[count3DescFieldIndex];
    if (count3DescField.type === 'String') {
      this.logger.info(
        `  count 3 description: '${count3DescField.value || '<missing>'
        }', with confidence of ${count3DescField.confidence}`
      );
    }
    const count3SectionFieldIndex = 'count3Section';
    const count3SectionField = invoice.fields[count3SectionFieldIndex];
    if (count3SectionField.type === 'String') {
      this.logger.info(
        `  count 3 section: '${count3SectionField.value || '<missing>'
        }', with confidence of ${count3SectionField.confidence}`
      );
    }
    const count3TicketAmountFieldIndex = 'count3TicketAmount';
    const count3TicketAmountField =
      invoice.fields[count3TicketAmountFieldIndex];
    if (count3TicketAmountField.type === 'Double') {
      this.logger.info(
        `  count 3 ticket amount: '${count3TicketAmountField.value || '<missing>'
        }', with confidence of ${count3TicketAmountField.confidence}`
      );
    }
    const count3ActRegsFieldIndex = 'count3ActRegs';
    const count3ActRegsField = invoice.fields[count3ActRegsFieldIndex];
    if (count3ActRegsField.type === 'String') {
      this.logger.info(
        `  count 3 ticket amount: '${count3ActRegsField.value || '<missing>'
        }', with confidence of ${count3ActRegsField.confidence}`
      );
    }

    let chargeCount = 0;
    if (
      count1DescField.value ||
      count1SectionField.value ||
      count1TicketAmountField.value
    ) {
      chargeCount++;
    }
    if (
      count2DescField.value ||
      count2SectionField.value ||
      count2TicketAmountField.value
    ) {
      chargeCount++;
    }
    if (
      count3DescField.value ||
      count3SectionField.valuet ||
      count3TicketAmountField.value
    ) {
      chargeCount++;
    }
    this.logger.info('chargeCount', chargeCount);

    const shellTicket = {
      violationTicketNumber: invoiceIdField.value
        ? String(invoiceIdField.value)
        : '',
      violationTime: invoiceTimeField.value
        ? invoiceTimeField.value.replace(' ', ':')
        : '',
      _violationDate: new Date(moment(invoiceDateField.value).utc().format('L')),

      lastName: surnameField.value
        ? surnameField.value
        : '',
      givenNames: givenNameField.value
        ? givenNameField.value
        : '',
      birthdate: '',
      gender: '',
      address: '',
      city: '',
      province: '',
      postalCode: '',
      driverLicenseNumber: driverLicenceField.value
        ? driverLicenceField.value
        : '',
      driverLicenseProvince: driverLicenceProvinceField.value
        ? driverLicenceProvinceField.value
        : '',
      courtHearingLocation: '',
      detachmentLocation: detachmentLocationField.value
        ? detachmentLocationField.value
        : '',

      count1Charge: count1ActRegsField.value
        ? count1ActRegsField.value
        : '',
      _count1ChargeDesc: count1DescField.value
        ? count1DescField.value
        : '',
      _count1ChargeSection: count1SectionField.value
        ? count1SectionField.value.replace(/\s/g, '')
        : '',
      count1FineAmount: count1TicketAmountField.value
        ? parseFloat(count1TicketAmountField.value.replace(/[^.0-9]/g, ''))
        : 0,
      count2Charge: count2ActRegsField.value
        ? count2ActRegsField.value
        : '',
      _count2ChargeDesc: count2DescField.value
        ? count2DescField.value
        : '',
      _count2ChargeSection: count2SectionField.value
        ? count2SectionField.value.replace(/\s/g, '')
        : '',
      count2FineAmount: count2TicketAmountField.value
        ? parseFloat(count2TicketAmountField.value.replace(/[^.0-9]/g, ''))
        : 0,
      count3Charge: count3ActRegsField.value
        ? count3ActRegsField.value
        : '',
      _count3ChargeDesc: count3DescField.value
        ? count3DescField.value
        : '',
      _count3ChargeSection: count3SectionField.value
        ? count3SectionField.value.replace(/\s/g, '')
        : '',
      count3FineAmount: count3TicketAmountField.value
        ? parseFloat(count3TicketAmountField.value.replace(/[^.0-9]/g, ''))
        : 0,
      _chargeCount: chargeCount,
      _amountOwing: 0,
    };

    this.logger.info('before', { ...shellTicket });
    delete shellTicket._count1ChargeSection;
    delete shellTicket._count2ChargeSection;
    delete shellTicket._count3ChargeSection;

    this.form.setValue(shellTicket);
    this.progressRef.complete();
  }

  public onStatuteSelected(event$: MatAutocompleteSelectedEvent): void {
    this.logger.log('onStatuteSelected', event$.option.value);
  }

  public get violationTicketNumber(): FormControl {
    return this.form.get('violationTicketNumber') as FormControl;
  }

  public get violationDate(): FormControl {
    return this.form.get('violationDate') as FormControl;
  }
}
