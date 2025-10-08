import { TestBed } from '@angular/core/testing';

import { ChatTeamService } from './chat-team.service';

describe('ChatTeamService', () => {
  let service: ChatTeamService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatTeamService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
