import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TenantGuard } from './tenant.guard';
import { provideMockStore } from '@ngrx/store/testing';

describe('TenantGuard', () => {
  let guard: TenantGuard;
  let router: Router;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        TenantGuard,
        provideMockStore(),
        { provide: Router, useValue: { navigate: jasmine.createSpy() } }
      ]
    });

    guard = TestBed.inject(TenantGuard);
    router = TestBed.inject(Router);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should navigate to /error if tenant ID is invalid', () => {
    spyOn(router, 'navigate'); // Espionner la méthode navigate du router

    // Simuler un tenantId invalide
    const invalidTenantId = null;
    guard.canActivate().subscribe((result) => {
      expect(result).toBeFalsy();
      expect(router.navigate).toHaveBeenCalledWith(['error/error404']);
    });
  });

  it('should allow activation if tenant ID is valid', () => {
    spyOn(router, 'navigate');

    // Simuler un tenantId valide
    const validTenantId = 1; // Mettez ici un tenantId valide
    guard.canActivate().subscribe((result) => {
      expect(result).toBeTrue();
      expect(router.navigate).not.toHaveBeenCalled();
    });
  });
});
