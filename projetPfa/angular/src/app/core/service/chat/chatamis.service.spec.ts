import { TestBed } from '@angular/core/testing';

import { ChatAmisService } from './chatamis.service';

describe('ChatAmisService', () => {
  let service: ChatAmisService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatAmisService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
