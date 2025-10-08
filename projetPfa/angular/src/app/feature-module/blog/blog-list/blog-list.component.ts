import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { routes } from 'src/app/core/helpers/routes';
import { bloglist } from 'src/app/core/models/models';
import { DataService } from 'src/app/core/service/data/data.service';
import { selectTenantData } from 'src/app/core/store/tenant/tenant.selectors';

@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
  styleUrls: ['./blog-list.component.scss'],
})
export class BlogListComponent {
  public routes = routes;
  public bloglist: bloglist[] = [];
  tenantData$: Observable<any>;

  
  constructor(private dataservice: DataService ,private store: Store) {
    this.tenantData$ = this.store.select(selectTenantData);
    this.bloglist = this.dataservice.bloglist;
  }
  fav(data: bloglist) {
    data.favourite = !data.favourite;
  }

  topVenueOwlOptions = {
    loop: true,
    margin: 15,
    nav: true, // Active les boutons Prev/Next
    dots: true, // Affiche les points de navigation
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    navText: ['<i class="fa fa-chevron-left"></i>', '<i class="fa fa-chevron-right"></i>'], // Icônes FontAwesome
    responsive: {
      0: { items: 1 },
      600: { items: 2 },
      1000: { items: 3 }
    }
  };
  
  
  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });

  }
}
