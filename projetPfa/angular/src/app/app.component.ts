import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { selectTenantData } from './core/store/tenant/tenant.selectors';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'dreams-sports';
  tenantData$: Observable<any>;
  
  constructor(private titleService: Title ,private store: Store) {
    this.titleService.setTitle('DreamsSports - Plateforme Sportive');

    this.tenantData$ = this.store.select(selectTenantData);
          this.tenantData$.subscribe(data => {
            if (data && data.siteInfo && data.siteInfo.length > 0) {
              this.titleService.setTitle(data.siteInfo[0].name);

            }
          });     
  }
}
