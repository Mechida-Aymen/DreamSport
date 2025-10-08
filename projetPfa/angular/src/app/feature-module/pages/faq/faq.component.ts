import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { routes } from 'src/app/core/helpers/routes';
import { selectTenantData } from 'src/app/core/store/tenant/tenant.selectors';

@Component({
  selector: 'app-faq',
  templateUrl: './faq.component.html',
  styleUrls: ['./faq.component.scss']
})
export class FaqComponent {
public routes=routes

 tenantData$: Observable<any>;

    constructor(private store: Store) {
      this.tenantData$ = this.store.select(selectTenantData);

    }
}
