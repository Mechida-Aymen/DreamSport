import { Component } from '@angular/core';
import { routes } from 'src/app/core/helpers/routes';
import { ourteams } from 'src/app/core/models/models';
import { DataService } from 'src/app/core/service/data/data.service';

@Component({
  selector: 'app-our-teams',
  templateUrl: './our-teams.component.html',
  styleUrls: ['./our-teams.component.scss']
})
export class OurTeamsComponent {
  public routes = routes
  public ourteams: ourteams[] = [];
  constructor (private dataservice:DataService){
    this.ourteams = this.dataservice.ourteams
  }
}
