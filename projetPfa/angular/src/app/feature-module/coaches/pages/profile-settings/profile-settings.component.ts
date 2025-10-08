import { Component } from '@angular/core';
import { CommonService, routes } from 'src/app/core/core.index';

@Component({
  selector: 'app-profile-settings',
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.scss']
})
export class ProfileSettingsComponent {
public routes = routes;
base = '';
page = '';
last = '';
constructor(
  private common: CommonService,
){
  this.common.base.subscribe((res: string) => {
    this.base = res;
  });
  this.common.page.subscribe((res: string) => {
    this.page = res;
  });
  this.common.last.subscribe((res: string) => {
    this.last = res;
  });
}
}
