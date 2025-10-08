import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CapitaineComponent } from './capitaine.component';

describe('CapitaineComponent', () => {
  let component: CapitaineComponent;
  let fixture: ComponentFixture<CapitaineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CapitaineComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CapitaineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
