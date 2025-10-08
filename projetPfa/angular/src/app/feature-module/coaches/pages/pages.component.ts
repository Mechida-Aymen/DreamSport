import { Component } from '@angular/core';
import { NavigationStart, Router, Event as RouterEvent } from '@angular/router';
import { CommonService, DataService, coachPages, routes, url } from 'src/app/core/core.index';

@Component({
  selector: 'app-pages',
  templateUrl: './pages.component.html',
  styleUrls: ['./pages.component.scss']
})
export class PagesComponent {
public routes = routes;
base = "";
page = "";
last = "";
end = "";
currentUrl = '';

coachPages: Array<coachPages> = [];

constructor(private Router: Router, private dataservice: DataService,private common: CommonService,) {
  this.common.base.subscribe((res: string) => {
    this.base = res?.replace('-', ' ');
  });
  this.common.page.subscribe((res: string) => {
    this.page = res?.replace('-', ' ');
  });
  this.common.last.subscribe((res: string) => {
    this.last = res?.replace('-', ' ');
  });
  this.common.end.subscribe((res: string) => {
    this.end = res?.replaceAll('-', ' ');
  });
  this.dataservice.getCoachPages.subscribe((res: Array<coachPages>) => {
    this.coachPages = res;
  });
  this.Router.events.subscribe((data: RouterEvent) => {
    if (data instanceof NavigationStart) {
      this.setRouting(data);
      console.log("currentUrl",this.currentUrl);
      console.log("end",this.end);
      console.log("last",this.last);
    }
    if (this.currentUrl == routes.coachCourts) {
      this.end = 'Courts';
    }
  });

}
private setRouting(data: url): void {
  this.currentUrl = data.url;
  this.common.base.next(data.url.split('/')[2]);
  this.common.page.next(data.url.split('/')[3]);
  this.common.last.next(data.url.split('/')[4]);
  this.common.end.next(data.url.split('/')[5]);

}
}
