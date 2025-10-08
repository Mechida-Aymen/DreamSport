import { Component } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { CommonService, routes, url } from 'src/app/core/core.index';


@Component({
  selector: 'app-coach-courts',
  templateUrl: './coach-courts.component.html',
  styleUrls: ['./coach-courts.component.scss']
})
export class CoachCourtsComponent {
 public routes =routes;

 base = '';
 page = '';
 last = '';
 end = '';

 currentUrl = '';
 constructor(
   private Router: Router,
   private common: CommonService
 ) {
   this.common.base.subscribe((res: string) => {
     this.base = res?.replaceAll('-', ' ');
   });
   this.common.page.subscribe((res: string) => {
     this.page = res?.replaceAll('-', ' ');
   });
   this.common.last.subscribe((res: string) => {
     this.last = res?.replaceAll('-', ' ');
   });
   this.common.end.subscribe((res: string) => {
    this.end = res?.replaceAll('-', ' ');
  });
   Router.events.subscribe((event: object) => {
     if (event instanceof NavigationStart) {
       this.getRoutes(event);
     }
   });
   this.getRoutes(this.Router);
 }
 getRoutes(event: url) {
   const splitVal = event.url.split('/');
   this.currentUrl = event.url;
   this.common.base.next(splitVal[1]);
   this.common.page.next(splitVal[2]);
   this.common.last.next(splitVal[3]);

   if (this.currentUrl == routes.coachCourts) {
     this.end = 'Courts';
   }
 }
}
