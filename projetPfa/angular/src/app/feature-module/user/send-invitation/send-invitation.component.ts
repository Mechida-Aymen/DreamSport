import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { routes } from 'src/app/core/core.index';
import { UserService, User } from 'src/app/core/service/user/user.service';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { InvitationService } from 'src/app/core/service/invitation/invitation-service.service';
import { ToastrService } from 'ngx-toastr';

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

  constructor(
    private router: Router,
    private userService: UserService,
    private authservice: AuthService,
    private invitationService: InvitationService,
    private toastr: ToastrService
  ) {
    this.setupSearchDebounce();
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
    const currentUserId = this.authservice.getUserId();
    
    console.log('Starting search for:', trimmedTerm);
    console.log('Current user ID:', currentUserId);
  
    this.userService.searchUsers(encodedSearchTerm, currentUserId).subscribe({
      next: (users) => {
        console.log('Initial users found:', users);
  
        if (trimmedTerm.includes(' ')) {
          const terms = trimmedTerm.toLowerCase().split(' ');
          this.filteredUsers = users.filter(user => 
            this.matchesAllTerms(user, terms)
          );
        } else {
          this.filteredUsers = users;
        }
  
        console.log('Filtered users:', this.filteredUsers);
  
        // Vérification pour chaque utilisateur
        this.filteredUsers.forEach(user => {
          console.log(`Checking user ${user.id} (${user.nom} ${user.prenom})`);
  
          // 1. Vérifier l'amitié
          this.invitationService.checkFriendship(currentUserId, user.id).subscribe({
            next: (areFriends) => {
              console.log(`Friendship status with ${user.id}:`, areFriends);
              user.areFriends = areFriends;
              
              if (!areFriends) {
                // 2. Vérifier les invitations dans les deux sens
                this.invitationService.getUserInvitations(currentUserId).subscribe({
                  next: (invitationsResponse) => {
                    console.log('All invitations:', invitationsResponse.invitations);
                    
                    // Vérifier dans les deux sens
                    const hasPendingInvitation = invitationsResponse.invitations.some(inv => {
                      const condition1 = (inv.emetteur.id === currentUserId && inv.recepteur === user.id); // J'ai envoyé
                      const condition2 = (inv.emetteur.id === user.id && inv.recepteur === currentUserId); // J'ai reçu
                      
                      console.log(`Invitation check for ${user.id}:`, {
                        condition1,
                        condition2,
                        emetteur: inv.emetteur.id,
                        recepteur: inv.recepteur
                      });
                      
                      return condition1 || condition2;
                    });
                    
                    user.hasPendingInvitation = hasPendingInvitation;
                    console.log(`Final status for ${user.id}:`, {
                      areFriends: user.areFriends,
                      hasPendingInvitation: user.hasPendingInvitation
                    });
                  },
                  error: (err) => {
                    console.error('Error fetching invitations:', err);
                    user.hasPendingInvitation = false;
                  }
                });
              } else {
                user.hasPendingInvitation = false;
              }
            },
            error: (err) => {
              console.error('Error checking friendship:', err);
              user.areFriends = false;
            }
          });
        });
  
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Search error:', err);
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

  sendInvitation(user: User): void {
    user.sending = true;
    const invitationData = {
      emetteur: this.authservice.getUserId(),
      recerpteur: user.id,
      adminId: this.authservice.getUserId()
    };

    this.invitationService.sendInvitation(invitationData).subscribe({
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