import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoachRequestComponent } from './coach-request.component';

describe('CoachRequestComponent', () => {
  let component: CoachRequestComponent;
  let fixture: ComponentFixture<CoachRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoachRequestComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CoachRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
