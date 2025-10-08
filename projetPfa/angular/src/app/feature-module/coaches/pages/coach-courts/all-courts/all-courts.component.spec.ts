import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllCourtsComponent } from './all-courts.component';

describe('AllCourtsComponent', () => {
  let component: AllCourtsComponent;
  let fixture: ComponentFixture<AllCourtsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllCourtsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllCourtsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
