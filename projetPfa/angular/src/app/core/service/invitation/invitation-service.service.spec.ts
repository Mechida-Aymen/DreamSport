import { TestBed } from '@angular/core/testing';

import { InvitationServiceService } from './invitation-service.service';

describe('InvitationServiceService', () => {
  let service: InvitationServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InvitationServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
