import { TestBed } from '@angular/core/testing';

import { ChatSignalRService  } from './chatsignalr.service';

describe('ChatsignalrService', () => {
  let service: ChatSignalRService ;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatSignalRService );
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
