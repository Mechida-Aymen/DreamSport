import { Component, OnInit } from '@angular/core';
import * as AOS from 'aos';
import { OwlOptions } from 'ngx-owl-carousel-o';
import { routes } from 'src/app/core/helpers/routes';
import { Store} from '@ngrx/store';
import { Observable } from 'rxjs';
import { selectTenantData } from '../../core/store/tenant/tenant.selectors';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})


export class HomeComponent implements OnInit {

  public routes = routes; 
  tenantData$: Observable<any>;
  imageUrl='https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/banner.jpg';



    constructor(private store: Store) {
      this.tenantData$ = this.store.select(selectTenantData);
      this.tenantData$.subscribe(data => {
        if (data && data.siteInfo && data.siteInfo.length > 0) {
          this.imageUrl = data.siteInfo[0].background;
        }
      });     
    }

  
    ngOnInit() {
      AOS.init({ duration: 1200, once: true });
      
    }

    formatDate(date: string): string {
      const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
      return new Date(date).toLocaleDateString('en-US', options);
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

}
