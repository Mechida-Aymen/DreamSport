import { Component } from '@angular/core';
import { routes } from 'src/app/core/helpers/routes';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { UsersService } from 'src/app/core/service/Backend/users/users.service';

@Component({
  selector: 'app-other-settings',
  templateUrl: './other-settings.component.html',
  styleUrls: ['./other-settings.component.scss']
})
export class OtherSettingsComponent {
  public routes = routes;
  public isDeactivating: boolean = false;

  constructor(
    private authService: AuthService,
    private userService: UsersService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  // Ouvre le modal de confirmation
  openDeactivateModal() {
    // Le modal s'ouvre via data-bs-toggle dans le template
  }

  // Méthode appelée quand on confirme la suppression
  confirmDeactivate() {
    this.isDeactivating = true;
    const userId = this.authService.getUserId();
    
    if (!userId) {
      this.toastr.error('User ID not found', 'Error');
      this.isDeactivating = false;
      return;
    }

    this.userService.deleteUser(userId).subscribe({
      next: () => {
        this.toastr.success('Account deactivated successfully', 'Success');
        this.authService.logout();
        this.router.navigate([routes.login]);
      },
      error: (err) => {
        this.toastr.error('Failed to deactivate account', 'Error');
        console.error('Error deactivating account:', err);
        this.isDeactivating = false;
      }
    });
  }
}