// team.component.ts
import { Component, OnInit } from '@angular/core';
import { routes } from 'src/app/core/core.index';
import { EquipeService } from 'src/app/core/service/equipe/equipe.service';
import { AuthService } from 'src/app/core/service/auth/authservice';

@Component({
  selector: 'app-team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})
export class TeamComponent implements OnInit {
  public routes = routes;
  public userStatus: 'capitaine' | 'member' | 'user' | 'loading' = 'loading';
  public teamData: any = null;

  constructor(
    private equipeService: EquipeService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.checkUserMembership();
  }

  private checkUserMembership(): void {
    const userId = this.authService.getUserId(); 
    this.equipeService.checkMembership(userId).subscribe({
      next: (response) => {
        this.teamData = response;
        if (response.isMember) {
          this.userStatus = response.isCapitaine ? 'capitaine' : 'member';         
        } else {
          this.userStatus = 'user';
        }
      },
      error: (err) => {
        console.error('Erreur lors de la vérification du statut', err);
        this.userStatus = 'user';
      }
    });
  }
}