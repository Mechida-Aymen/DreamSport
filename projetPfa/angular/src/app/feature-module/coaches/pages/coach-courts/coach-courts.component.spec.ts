import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoachCourtsComponent } from './coach-courts.component';

describe('CoachCourtsComponent', () => {
  let component: CoachCourtsComponent;
  let fixture: ComponentFixture<CoachCourtsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoachCourtsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CoachCourtsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
