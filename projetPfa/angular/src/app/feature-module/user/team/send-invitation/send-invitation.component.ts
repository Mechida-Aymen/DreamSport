import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { debounceTime, distinctUntilChanged, forkJoin, map, of, Subject, switchMap } from 'rxjs';
import { routes } from 'src/app/core/core.index';
import { UserService, User } from 'src/app/core/service/user/user.service';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { InvitationService } from 'src/app/core/service/invitation/invitation-service.service';
import { ToastrService } from 'ngx-toastr';
import { EquipeService } from 'src/app/core/service/equipe/equipe.service';

@Component({
  selector: 'app-send-invitation',
  templateUrl: './send-invitation.component.html',
  styleUrls: ['./send-invitation.component.scss']
})
export class SendInvitationComponent {
  public routes = routes;
  public searchDataValue = '';
  public filteredUsers: User[] = [];
  public isLoading = false;
  private searchSubject = new Subject<string>();
  private equipeID:any;

  constructor(
    private router: Router,
    private userService: UserService,
    private authservice: AuthService,
    private invitationService: InvitationService,
    private toastr: ToastrService,
    private equipeService:EquipeService,
    private authService:AuthService
  ) {
    this.setupSearchDebounce();
    this.equipeId();
  }

  private setupSearchDebounce(): void {
    this.searchSubject.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe(searchValue => {
      this.searchUsers(searchValue);
    });
  }

  onSearchChange(value: string): void {
    this.searchSubject.next(value);
  }

  searchUsers(searchTerm: string): void {
    const trimmedTerm = searchTerm.trim();
    
    if (trimmedTerm === '') {
      this.filteredUsers = [];
      return;
    }
  
    this.isLoading = true;
    const encodedSearchTerm = encodeURIComponent(trimmedTerm);
    
    this.userService.searchUsers(encodedSearchTerm, this.authservice.getUserId()).subscribe({
      next: (users) => {
        if (trimmedTerm.includes(' ')) {
          const terms = trimmedTerm.toLowerCase().split(' ');
          this.filteredUsers = users.filter(user => 
            this.matchesAllTerms(user, terms)
          );
        } else {
          this.filteredUsers = users;
        }
  
        // Vérifier le statut de chaque utilisateur
        this.filteredUsers.forEach(user => {
          this.equipeService.checkMembership(user.id).subscribe({
            next: (response) => {
              user.isMemberOfTeam = response.isMember;
            },
            error: () => {
              user.isMemberOfTeam = false;
            }
          });
        });
  
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.filteredUsers = [];
      }
    });
  }

  private matchesAllTerms(user: User, terms: string[]): boolean {
    const userFullName = `${user.prenom} ${user.nom}`.toLowerCase();
    const userReverseName = `${user.nom} ${user.prenom}`.toLowerCase();
    const searchPattern = terms.join(' ').toLowerCase();

    return userFullName.includes(searchPattern) || 
           userReverseName.includes(searchPattern) ||
           terms.every(term =>
             user.nom.toLowerCase().includes(term) ||
             user.prenom.toLowerCase().includes(term) ||
             (user.username && user.username.toLowerCase().includes(term)) ||
             (user.bio && user.bio.toLowerCase().includes(term))
           );
  }

  private equipeId(): void {
    const userId = this.authService.getUserId(); // Adaptez selon votre implémentation
    this.equipeService.checkMembership(userId).subscribe({
      next: (response) => {
        this.equipeID=response.equipeId;
        console.warn("voici le : ",response);
        console.warn("voici le : ",response.equipeId);
        console.warn("voici le : ",this.equipeID);

      }
    });

  }

  sendInvitation(user: User): void {
    user.sending = true;
    const invitationData = {
      Emetteur: this.equipeID,
      Recepteur: user.id,
    };
    console.warn("voici les donnee : ",invitationData);
    this.invitationService.sendTeamInvitation(invitationData).subscribe({
      next: () => {
        this.toastr.success('Invitation sent successfully!', 'Success');
        user.sending = false;
        this.filteredUsers = this.filteredUsers.filter(u => u.id !== user.id);
      },
      error: (err) => {
        console.error('Error sending invitation', err);
        this.toastr.error('Failed to send invitation', 'Error');
        user.sending = false;
      }
    });
  }
}