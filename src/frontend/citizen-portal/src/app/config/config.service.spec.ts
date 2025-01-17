import { inject, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { ConfigService } from './config.service';
import { MockConfigService } from 'tests/mocks/mock-config.service';

describe('ConfigService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        {
          provide: ConfigService,
          useClass: MockConfigService,
        },
      ],
    })
  );

  it('should create', inject([ConfigService], (service: ConfigService) => {
    expect(service).toBeTruthy();
  }));

  it('should get provinces code', inject(
    [ConfigService],
    (service: ConfigService) => {
      const code = service.provinces[0];
      expect(code).toBeDefined();
    }
  ));

  it('should get courtLocations code', inject(
    [ConfigService],
    (service: ConfigService) => {
      const code = service.courtLocations[0].code;
      expect(code).toBeDefined();
    }
  ));

  it('should get countries code', inject(
    [ConfigService],
    (service: ConfigService) => {
      const code = service.countries[0].code;
      expect(code).toBeDefined();
    }
  ));

  it('should get statuses code', inject(
    [ConfigService],
    (service: ConfigService) => {
      const code = service.statuses[0].code;
      expect(code).toBeDefined();
    }
  ));
});
