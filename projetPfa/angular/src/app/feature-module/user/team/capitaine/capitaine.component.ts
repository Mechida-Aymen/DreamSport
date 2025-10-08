// capitaine.component.ts
import { Component, Input, OnInit } from '@angular/core';
import { routes } from 'src/app/core/core.index';
import { UserService } from 'src/app/core/service/user/user.service';
import { EquipeService  } from 'src/app/core/service/equipe/equipe.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { TerrainService } from 'src/app/core/service/terrain/terrain.service';

// Interface for Team Member relation
interface TeamMemberRelation {
  userId: number;
  equipeId: number;
}

interface ChangerCapitaineEquipeDTO {
  idEquipe: number;
  idCapitain: number;
}

interface TeamDetails {
  id: number;
  adminId: number | null;
  sportId: number;
  name: string;
  description: string;
  avatar: string;
  captainId: number;
  membres: TeamMemberRelation[];
}



// Interface for User data
interface User {
  id: number;
  nom: string;
  prenom: string;
  username: string | null;
  email: string;
  imageUrl: string | null;
  bio?: string;
}

// Interface for Input Team Data
interface InputTeamData {
  isMember: boolean;
  equipeId: number;
  equipeNom: string;
  isCapitaine: boolean;
  userId: number;
}

@Component({
  selector: 'app-capitaine',
  templateUrl: './capitaine.component.html',
  styleUrls: ['./capitaine.component.scss']
})
export class CapitaineComponent implements OnInit {
  @Input() teamData!: InputTeamData; 
  public routes = routes;
  
  teamDetails: TeamDetails | null = null;
  members: User[] = [];
  isLoading = true;
  errorMessage: string | null = null;
  memberToRemove: number | null = null;
  isCurrentUserCaptain: boolean = false;
  memberForCaptainTransfer: any ;
  currentSportName: string = '';
  sportCategories: SportCategory[] = [];
  defaultAvatarUrl= 'https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg';

  constructor(
    private userService: UserService,
    private teamService: EquipeService,
    private toastr: ToastrService,
    private router: Router,
    private terrainService:TerrainService


  ) {
    
  }

  ngOnInit(): void {
    console.log('Team Data Input:', this.teamData);
    this.loadTeamDetails();
    this.loadSportCategories();
  }

  private loadTeamDetails(): void {
    if (this.teamData?.equipeId) {
      this.teamService.getTeamDetails(this.teamData.equipeId).subscribe({
        next: (response: TeamDetails) => {
          this.teamDetails = response;
          this.getSportName();
          this.isCurrentUserCaptain = response.captainId === this.teamData.userId;
          this.loadMembers();
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error loading team details:', err);
          this.errorMessage = 'Error loading team details';
          this.isLoading = false;
        }
      });
    } else {
      console.error('No equipeId found in teamData');
      this.errorMessage = 'No team ID provided';
      this.isLoading = false;
    }
  }

  private loadMembers(): void {
    if (!this.teamDetails) return;
    
    if (this.teamDetails.membres && this.teamDetails.membres.length > 0) {
      this.members = [];
      
      // D'abord charger le capitaine
      this.userService.getUser(this.teamDetails.captainId).subscribe({
        next: (captain: User) => {
          this.members.push(captain);
          
          // Ensuite charger les autres membres
          this.teamDetails!.membres
            .filter(m => m.userId !== this.teamDetails!.captainId)
            .forEach((memberRelation: TeamMemberRelation) => {
              this.userService.getUser(memberRelation.userId).subscribe({
                next: (user: User) => {
                  this.members.push(user);
                },
                error: (err) => {
                  console.error('Error loading user:', memberRelation.userId, err);
                }
              });
            });
        },
        error: (err) => {
          console.error('Error loading captain:', this.teamDetails!.captainId, err);
        }
      });
    }
  }

  setMemberToRemove(memberId: number): void {
    this.memberToRemove = memberId;
  }

  confirmRemoveMember(): void {
    if (!this.teamDetails || this.memberToRemove === null) return;
  
    this.teamService.removeMember(this.teamData.equipeId, this.memberToRemove).subscribe({
      next: () => {
        this.members = this.members.filter(m => m.id !== this.memberToRemove);
        this.teamDetails!.membres = this.teamDetails!.membres.filter(
          m => m.userId !== this.memberToRemove
        );
        this.toastr.success('Member removed successfully');
      },
      error: (err) => {
        console.error('Error removing member:', err);
        this.toastr.error('Error removing member');
      }
    });
  }

  leaveTeamAsCaptain(): void {
    if (!this.teamDetails) return;

    this.teamService.capitaineQuitteEquipe(
      this.teamData.equipeId,
      this.teamData.userId
    ).subscribe({
      next: () => {
        this.toastr.success('You have left the team successfully');
        // Recharger la page complète
        this.router.navigateByUrl(this.routes.userTeam).then(() => {
          window.location.reload();
        });
      },
      error: (err) => {
        console.error('Error leaving team:', err);
        this.toastr.error('Error leaving team');
      }
    });
  }

  setMemberForCaptainTransfer(memberId: number): void {
    this.memberForCaptainTransfer = memberId;
  }

  confirmTransferCaptain(): void {
    if (!this.teamDetails || this.memberForCaptainTransfer === null) return;

    const transferDto: ChangerCapitaineEquipeDTO = {
      idEquipe: this.teamDetails.id,
      idCapitain: this.memberForCaptainTransfer
    };

    this.teamService.transferCaptaincy(transferDto).subscribe({
      next: () => {
        this.toastr.success('Captain role transferred successfully');
        // Update the team details
        if (this.teamDetails) {
          this.teamDetails.captainId = this.memberForCaptainTransfer;
          this.isCurrentUserCaptain = false;
        }
        this.memberForCaptainTransfer = null;
      },
      error: (err) => {
        console.error('Error transferring captaincy:', err);
        this.toastr.error('Error transferring captaincy');
      }
    });
  }

  loadSportCategories(): void {
    this.terrainService.getSportCategories().subscribe({
      next: (categories) => {
        this.sportCategories = categories;
        this.getSportName(); 
      },
      error: (err) => console.error('Error loading sports:', err)
    });
  }
  
  getSportName(): void {
    try {
      if (!this.teamDetails?.sportId || !this.sportCategories?.length) {
        this.currentSportName = 'No sport selected';
        return;
      }
  
      const foundSport = this.sportCategories.find(c => c.id === this.teamDetails!.sportId);
      this.currentSportName = foundSport?.name.replace(/\r\n/g, '').trim() ?? 'Sport not found';
      
    } catch (error) {
      console.error('Error getting sport name:', error);
      this.currentSportName = 'Error loading sport';
    }
  }

  confirmDeleteTeam(): void {
    if (!this.teamDetails) return;
  
    this.teamService.deleteTeam(this.teamDetails.id).subscribe({
      next: () => {
        this.toastr.success('Team deleted successfully');
        this.router.navigateByUrl(this.routes.userTeam).then(() => {
          window.location.reload();
        });
      },
      error: (err) => {
        console.error('Error deleting team:', err);
        this.toastr.error('Error deleting team');
      }
    });
  }
  
}


interface SportCategory {
  id: number;
  name: string;
  nombreMax: number;
}