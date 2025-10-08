export class routes {
  private static base = '';
  static navigate: string;

  public static get baseUrl(): string {
    const urlParts = window.location.pathname.split('/').filter(part => part !== '');
    const tenantIdOrSlug = urlParts[0];
    
    const tenantId = parseInt(tenantIdOrSlug, 10);
    if (!isNaN(tenantId)) {
      this.base = `/${tenantId}`;
    } else {
      this.base = `/${tenantIdOrSlug}`;
    }
    
    return this.base;
  }
  // auth routes
  public static get auth(): string {
    return this.baseUrl + '/auth';
  }
  public static get login(): string {
    return this.baseUrl + '/auth/login';
  }
  public static get register(): string {
    return this.baseUrl + '/auth/register';
  }
  public static get forgotPassword(): string {
    return this.baseUrl + '/auth/forgot-password';
  }

  public static get user(): string {
    return this.baseUrl + '/user';
  }
  // auth routes *ends*

  // error pages routes

  public static get errorPages(): string {
    return this.baseUrl + '/errorpages';
  }
  public static get error404(): string {
    return this.baseUrl + 'error/error404';
  }

  // error pages routes *ends*

  // core pages routes

  public static get core(): string {
    return this.baseUrl;
  }
  public static get home(): string {
    return this.core + '/home';
  }

  // core pages routes *ends*

  // core pages child routes

  //blog pages route //

  public static get blog(): string {
    return this.baseUrl + '/blog';
  }
  
  public static get bloglist(): string {
    return this.blog + '/blog-list';
  }

  //pages routes//
  public static get pages(): string {
    return this.baseUrl + '/pages';
  }
  public static get aboutus(): string {
    return this.baseUrl + '/pages/about-us';
  }
  public static get ourteams(): string {
    return this.baseUrl + '/pages/our-teams';
  }
  public static get faq(): string {
    return this.baseUrl + '/pages/faq';
  }
  public static get maintenance(): string {
    return this.baseUrl + '/pages/maintenance';
  }
  public static get privacyPolicy(): string {
    return this.baseUrl + '/pages/privacy-policy';
  }
  public static get termsCondition(): string {
    return this.baseUrl + '/pages/terms';
  }

  public static get contactUs(): string {
    return this.baseUrl + '/pages/contact-us';
  }
  //pages routes *end*//


  //User routes *start*//

  public static get userTeam(): string {
    return this.user + '/team';
  }

  public static get userTeamCapitain(): string {
    return this.userTeam + '/capitaine';
  }
  public static get userTeamInvitation(): string {
    return this.userTeam + '/send-invitation';
  }
  public static get userTeamSetting(): string {
    return this.userTeam + '/setting';
  }
  
  
  public static get userTeamMember(): string {
    return this.userTeam + '/member';
  }
  public static get userTeamUser(): string {
    return this.userTeam + '/user';
  }
  public static get userBookings(): string {
    return this.user + '/user-bookings/details';
  }
  public static get usertimedate(): string {
    return this.user + '/user-bookings/timedate';
  }
  public static get userChat(): string {
    return this.user + '/chat';
  }
  public static get userInvitation(): string {
    return this.user + '/invitation';
  }
  public static get sendInvitation(): string {
    return this.user + '/send-invitation';
  }
  public static get teamInvitation(): string {
    return this.user + '/team-invitation';
  }
  public static get userWallet(): string {
    return this.user + '/wallet';
  }
  public static get bookingDetails(): string {
    return this.user + '/user-booking/details';
  }
  public static get userProfile(): string {
    return this.user + '/user-profile';
  }
  public static get userChangepassword(): string {
    return this.user + '/change-password';
  }
  public static get userOthersettings(): string {
    return this.user + '/user-profile-setting';
  }

  // admin routes
  public static get admin(): string {
    return this.baseUrl + '/admin/';
  }
  public static get adminDashboard(): string {
    return this.admin + '/dashboard';
  }
  public static get admin_annonces(): string {
    return this.admin + '/annonces';
  }
  public static get admin_site(): string {
    return this.admin + '/site';
  }
  public static get admin_faqs(): string {
    return this.admin + '/faqs';
  }
  public static get admin_employees(): string {
    return this.admin + '/employees';
  }
  public static get admin_profile_settings(): string {
    return this.admin + '/profile-settings';
  }
  public static get admin_profile_edit(): string {
    return this.admin + '/profile-settings/admin-profile';
  }
  public static get admin_profile(): string {
    return this.admin + '/profile-settings/myprofile';
  }
  public static get admin_setting_password(): string {
    return this.admin + '/profile-settings/setting-password';
  }
  public static get admin_courts(): string {
    return this.admin + '/courts';
  }
  // coaches routes start
  public static get coach(): string {
    return this.baseUrl + '/coaches/';
  }
  public static get coachPages(): string {
    return this.baseUrl + '/coaches/pages';
  }
  public static get coach_profile_edit(): string {
    return this.coach + 'pages/profile-settings/coach-profile';
  }
  public static get coach_profile(): string {
    return this.coach + 'pages/profile-settings/myprofile';
  }
  public static get coach_other_setting(): string {
    return this.coach + 'pages/profile-settings/othersetting';
  }
  public static get coach_setting_availability(): string {
    return this.coach + 'pages/profile-settings/availability';
  }
  public static get coach_setting_lesson(): string {
    return this.coach + 'pages/profile-settings/setting-lesson';
  }
  public static get coach_setting_password(): string {
    return this.coach + 'pages/profile-settings/setting-password';
  }
  public static get coachUsers(): string {
    return this.coach + 'pages/users';
  }
  public static get coachProfile(): string {
    return this.coach + 'pages/profile-settings/coach-profile';
  }
  public static get coachRequest(): string {
    return this.coach + 'pages/requests';
  }
  public static get coachTimeDate(): string {
    return this.coach + 'booking-steps/coach-timedate';
  }
  public static get coachCourts(): string {
    return this.coach + 'pages/courts/all-courts';
  }
  public static get coachactivecourts(): string {
    return this.coach + 'pages/courts/court-active';
  }
  public static get coachinactivecourts(): string {
    return this.coach + 'pages/courts/court-inactive';
  }

  
  // coaches routes end
}
