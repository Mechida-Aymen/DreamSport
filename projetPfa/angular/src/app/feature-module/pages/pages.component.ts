import { Component } from '@angular/core';
import { CommonService, routes } from 'src/app/core/core.index';

@Component({
  selector: 'app-pages',
  templateUrl: './pages.component.html',
  styleUrls: ['./pages.component.scss']
})
export class PagesComponent {
public routes = routes;
breadcrum = true;
base ="";
page ="";
last ="";
end ="";

constructor(private common: CommonService,) {
  this.common.base.subscribe((res: string) => {
    this.base = res?.replace('-', ' ');
  });
  this.common.page.subscribe((res: string) => {
    this.page = res;
  });
  this.common.last.subscribe((res: string) => {
    this.last = res?.replace('-', ' ');
  });
  this.common.end.subscribe((res: string) => {
    this.end = res?.replace('-', ' ');
  });
  if(this.page === 'coming-soon' ) {
    this.breadcrum = false;
  }else if( this.page === 'maintenance') {
    this.breadcrum = false;
  }else if(  this.page === 'error') {
    this.breadcrum = false;
  }else {
    this.breadcrum = true;
  }
}

}
