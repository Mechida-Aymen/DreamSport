import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { routes } from 'src/app/core/core.index';
import { selectTenantData } from 'src/app/core/store/tenant/tenant.selectors';
interface data {
  value: string;
}
@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  public routes = routes;
  public selectedValue1 = '';
  public selectedValue2 = '';
  isLoggedIn = false;
  tenantData$: Observable<any>;
  
   constructor(private store: Store) {
        this.tenantData$ = this.store.select(selectTenantData);
         
   }
   ngOnInit(): void {
    this.checkAuthStatus();
  }


   private checkAuthStatus(): void {
    const userString = localStorage.getItem('user_data');
    if (userString) {
      this.isLoggedIn = true;    
    } 
  }
}
