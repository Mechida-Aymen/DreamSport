import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { OwlOptions } from 'ngx-owl-carousel-o';
import { Observable } from 'rxjs';
import { routes } from 'src/app/core/helpers/routes';
import { bloggrid, ourteams } from 'src/app/core/models/models';
import { DataService } from 'src/app/core/service/data/data.service';
import { selectTenantData } from 'src/app/core/store/tenant/tenant.selectors';

@Component({
  selector: 'app-about-us',
  templateUrl: './about-us.component.html',
  styleUrls: ['./about-us.component.scss']
})
export class AboutUsComponent {
  public routes=routes
tenantData$: Observable<any>;

    constructor(private store: Store) {
      this.tenantData$ = this.store.select(selectTenantData);

    }

    public topVenueOwlOptions: OwlOptions = {
      margin: 24,
      nav: true,
      loop: true,
      dots: false,
      smartSpeed: 2000,
      navText: [
        '<i class="fas fa-chevron-left"></i>',
        '<i class="fas fa-chevron-right"></i>',
      ],
      responsive: {
        0: {
          items: 1,
        },
        768: {
          items: 3,
        },
        1170: {
          items: 3,
        },
      },
    };

    formatDate(date: string): string {
      const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
      return new Date(date).toLocaleDateString('en-US', options);
    }
 
}
