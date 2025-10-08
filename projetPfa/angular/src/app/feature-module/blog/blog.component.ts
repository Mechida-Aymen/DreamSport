import { Component } from '@angular/core';
import { CommonService } from 'src/app/core/core.index';
import { routes } from 'src/app/core/helpers/routes';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.scss']
})
export class BlogComponent {
  public routes = routes;
  breadcrum = true;
  base ="";
  page ="";
  last ="";
  end ="";

  
  constructor(private common: CommonService,) {



    this.common.base.subscribe((res: string) => {
      this.base = res?.replaceAll('-', ' ');
    });
    this.common.page.subscribe((res: string) => {
      this.page = res;
    });
    this.common.last.subscribe((res: string) => {
      this.last = res?.replaceAll('-', ' ');
    });
    this.common.end.subscribe((res: string) => {
      this.end = res?.replaceAll('-', ' ');
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
