import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import {
  adminPages,
  apiResultFormat,
  coachPages,
  coaches,
  usermenu,
} from '../../models/models';
import { map } from 'rxjs';
import { routes } from '../../core.index';
import { sideBar } from 'src/app/shared/model/header.model';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  constructor(private http: HttpClient) {}
  public getCoachBookingCanceled() {
    return this.http
      .get<apiResultFormat>('assets/json/booking-canceled.json')
      .pipe(
        map((res: apiResultFormat) => {
          return res;
        })
      );
  }
  public getCoachBooking() {
    return this.http
      .get<apiResultFormat>('assets/json/coach-booking.json')
      .pipe(
        map((res: apiResultFormat) => {
          return res;
        })
      );
  }
  public getCoachEarning() {
    return this.http
      .get<apiResultFormat>('assets/json/coach-earnings.json')
      .pipe(
        map((res: apiResultFormat) => {
          return res;
        })
      );
  }

  public getEarningCoaching() {
    return this.http
      .get<apiResultFormat>('assets/json/earning-coaching.json')
      .pipe(
        map((res: apiResultFormat) => {
          return res;
        })
      );
  }
  public getuserDashboard() {
    return this.http
      .get<apiResultFormat>('assets/json/user-dashboard.json')
      .pipe(
        map((res: apiResultFormat) => {
          return res;
        })
      );
  }
  public getuserCancelled() {
    return this.http
      .get<apiResultFormat>('assets/json/user-cancelled.json')
      .pipe(
        map((res: apiResultFormat) => {
          return res;
        })
      );
  }
  public getuserInvitation() {
    return this.http.get<apiResultFormat>('assets/json/user-Invitation.json').pipe(
      map((res: apiResultFormat) => {
        return res;
      })
    );
  }
  public getuserbookings() {
    return this.http
      .get<apiResultFormat>('assets/json/user-coaching.json')
      .pipe(
        map((res: apiResultFormat) => {
          return res;
        })
      );
  }
  public getuserWallet() {
    return this.http.get<apiResultFormat>('assets/json/user-wallet.json').pipe(
      map((res: apiResultFormat) => {
        return res;
      })
    );
  }
  public getAllCourts() {
    return this.http.get<apiResultFormat>('assets/json/all-courts.json').pipe(
      map((res: apiResultFormat) => {
        return res;
      })
    );
  }
  public getActiveCourts() {
    return this.http
      .get<apiResultFormat>('assets/json/active-courts.json')
      .pipe(
        map((res: apiResultFormat) => {
          return res;
        })
      );
  }
  public getInactiveCourts() {
    return this.http
      .get<apiResultFormat>('assets/json/inactive-courts.json')
      .pipe(
        map((res: apiResultFormat) => {
          return res;
        })
      );
  }
  public getuserCompleted() {
    return this.http
      .get<apiResultFormat>('assets/json/user-completed.json')
      .pipe(
        map((res: apiResultFormat) => {
          return res;
        })
      );
  }
  public getuserOngoing() {
    return this.http.get<apiResultFormat>('assets/json/user-ongoing.json').pipe(
      map((res: apiResultFormat) => {
        return res;
      })
    );
  }
  public getCoachRequest() {
    return this.http
      .get<apiResultFormat>('assets/json/coach-request.json')
      .pipe(
        map((res: apiResultFormat) => {
          return res;
        })
      );
  }
  public getCoachWallet() {
    return this.http.get<apiResultFormat>('assets/json/coach-wallet.json').pipe(
      map((res: apiResultFormat) => {
        return res;
      })
    );
  }

  public sideBar: sideBar[] = [
    {
      tittle: 'Home',
      base: 'home',
      showAsTab: false,
      separateRoute: true,
      route: routes.home,
    },
    {
      tittle: 'About Us',
      base: 'pages',
      showAsTab: false,
      separateRoute: true,
      route: routes.aboutus,
    },
    {
      tittle: 'FAQ',
      base: 'pages',
      showAsTab: false,
      separateRoute: true,
      route: routes.faq,
    },

    {
      tittle: 'Blogs',
      base: 'blog',
      showAsTab: false,
      separateRoute: true,
      route: routes.bloglist,
    },
  
    {
      tittle: 'Contact Us',
      base: 'contact-us',
      showAsTab: false,
      separateRoute: true,
      route: routes.contactUs,
    },
  ];
  public badmintonList = [
    {
      title: 'Badminton',
      img1: 'assets/img/venues/venues-07.jpg',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'OrlandoWaters',
      year: '15 May 2023',
      para: 'Mastering the Badminton Basics: A Guide for Beginners ',
      feedback: '45',
      command: '40',
      img3: 'assets/img/icons/clock.svg',
      time: '10 Min To Read',
    },
    {
      title: 'Sports Activites',
      img1: 'assets/img/venues/venues-08.jpg',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'ClaireNichols',
      year: '16 Jun 2023',
      para: 'Sports Make Us A Lot Stronger And Healthier Than We Think',
      feedback: '32',
      command: '21',
      img3: 'assets/img/icons/clock.svg',
      time: '5 Min To Read',
    },
    {
      title: 'Rules of Game',
      img1: 'assets/img/venues/venues-09.jpg',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'JoannaLe',
      year: '17 May 2023',
      para: 'We Organize Events & Parties in our Club',
      feedback: '55',
      command: '32',
      img3: 'assets/img/icons/clock.svg',
      time: '4 Min To Read',
    },
    {
      title: 'Badminton',
      img1: 'assets/img/venues/venues-15.jpg',
      img2: 'assets/img/profiles/avatar-02.jpg',
      name: 'Andrew',
      year: '17 May 2023',
      para: 'Mastering the Badminton Basics: A Guide for Beginners ',
      feedback: '55',
      command: '32',
      img3: 'assets/img/icons/clock.svg',
      time: '4 Min To Read',
    },
    {
      title: 'Rules of Game',
      img1: 'assets/img/venues/venues-16.jpg',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'MartSublin',
      year: '16 Jun 2023',
      para: 'The Art of Footwork: Enhancing Agility in Badminton',
      feedback: '32',
      command: '21',
      img3: 'assets/img/icons/clock.svg',
      time: '5 Min To Read',
    },
    {
      title: 'Sports Activites',
      img1: 'assets/img/venues/venues-17.jpg',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'Rebecca',
      year: '15 May 2023',
      para: 'Unleashing Your Badminton Potential: Tips for Skill Development',
      feedback: '45',
      command: '40',
      img3: 'assets/img/icons/clock.svg',
      time: '10 Min To Read',
    },
  ];
  public venuesList = [
    {
      title: 'Badminton',
      img1: 'assets/img/venues/venues-07.jpg',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'OrlandoWaters',
      year: '15 May 2023',
      para: 'A Great And Fun Activity For You And Your Entire Family',
      feedback: '45',
      command: '45',
      img3: 'assets/img/icons/clock.svg',
      time: '10 Min To Read',
    },
    {
      title: 'Sports Activites',
      img1: 'assets/img/venues/venues-08.jpg',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'ClaireNichols',
      year: '16 Jun 2023',
      para: 'Sports Make Us A Lot Stronger And Healthier Than We Think',
      feedback: '35',
      command: '35',
      img3: 'assets/img/icons/clock.svg',
      time: '12 Min To Read',
    },
    {
      title: 'Rules of Game',
      img1: 'assets/img/venues/venues-09.jpg',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'JoannaLe',
      year: '11 May 2023',
      para: 'A Great And Fun Activity For You And Your Entire Family',
      feedback: '25',
      command: '25',
      img3: 'assets/img/icons/clock.svg',
      time: '14 Min To Read',
    },
    {
      title: 'Sports Activites',
      img1: 'assets/img/venues/venues-08.jpg',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'MartSublin',
      year: '12 May 2023',
      para: 'Sports Make Us A Lot Stronger And Healthier Than We Think',
      feedback: '35',
      command: '35',
      img3: 'assets/img/icons/clock.svg',
      time: '12 Min To Read',
    },
  ];
  public commandList = [
    {
      name: 'Antony Hilfn',
      img: 'assets/img/profiles/avatar-04.jpg',
      date: '15 Mar 2023',
      para: "Lorem Ipsum available but the or majority have that suffered alteration words which don't look even slightly believable. There are many available but the majority the have suffered alteration. There are many variations of dumm passages Lorem  majority the have the suffered alteration.",
      response: 'Reply',
    },
    {
      name: 'Besant',
      img: 'assets/img/profiles/avatar-05.jpg',
      date: '15 Mar 2023',
      para: "Lorem Ipsum available but the or majority have that suffered alteration words which don't look even slightly believable. There are many available but the majority the have suffered alteration. There are many variations of dumm passages Lorem  majority the have the suffered alteration.",
      response: 'Reply',
    },
    {
      name: 'Maria Fin',
      img: 'assets/img/profiles/avatar-06.jpg',
      date: '15 Mar 2023',
      para: "Lorem Ipsum available but the or majority have that suffered alteration words which don't look even slightly believable. There are many available but the majority the have suffered alteration. There are many variations of dumm passages Lorem  majority the have the suffered alteration.",
      response: 'Reply',
    },
  ];
  public latestpost = [
    {
      img: 'assets/img/blog/latestpost-01.jpg',
      name: 'Sarah',
      content: 'These 15 Cabin Wedding Venues Have All the Rustic  Vibes',
    },
    {
      img: 'assets/img/blog/latestpost-02.jpg',
      name: 'Kim Forrest',
      content: '23 Super-Chic Blazer Dresses for Your Home Game',
    },
    {
      img: 'assets/img/blog/latestpost-03.jpg',
      name: 'Jessica Estrada',
      content: 'The Prettiest Floral Bats for a Rage Look',
    },
    {
      img: 'assets/img/blog/latestpost-04.jpg',
      name: 'Naoimh',
      content: '31 Beautiful Courts Around the World',
    },
  ];

  public ourteams = [
    {
      img: 'assets/img/aboutus/team-01.jpg',
      name: 'Caterine',
      role: 'Chief Executive Officer',
      para: 'Results-driven CEO, Ms. Caterine, fosters innovation and drives growth with proven success. Her strategic vision, collaborative leadership, and financial acumen consistently deliver excellence, value, and strong stakeholder relationships.',
    },
    {
      img: 'assets/img/aboutus/team-02.jpg',
      name: 'Anto',
      role: 'Marketing Manager',
      para: 'Mr. Anto, our Marketing Manager, passionately drives impactful marketing campaigns, expanding the reach of badminton. With expertise in brand management and digital marketing, he elevates our presence and engages our target audience.',
    },
    {
      img: 'assets/img/aboutus/team-03.jpg',
      name: 'Lucas Finn',
      role: 'Team Leader',
      para: 'Introducing Mr. Lucas Finn, our inspiring Team Leader in badminton. Lucass commitment to excellence ensures seamless project execution and success in the sport.',
    },
    {
      img: 'assets/img/aboutus/team-04.jpg',
      name: 'Andrew',
      role: 'Designer',
      para: 'Mr. Andrew is a skilled badminton designer known for transforming courts, creating striking packaging, designing stylish apparel, and developing captivating tournament branding.',
    },
    {
      img: 'assets/img/aboutus/team-05.jpg',
      name: 'Lucas Finn',
      role: 'Team Leader',
      para: 'Introducing Mr. Lucas Finn, our inspiring Team Leader in badminton. Lucass commitment to excellence ensures seamless project execution and success in the sport.',
    },
    {
      img: 'assets/img/aboutus/team-06.jpg',
      name: 'Anto',
      role: 'Marketing Manager',
      para: 'it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum',
    },
    {
      img: 'assets/img/aboutus/team-07.jpg',
      name: 'Andrew',
      role: 'Designer',
      para: 'it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum',
    },
    {
      img: 'assets/img/aboutus/team-08.jpg',
      name: 'Hendrita',
      role: 'Developer',
      para: 'it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum',
    },
  ];
  public events = [
    {
      img: 'assets/img/events/event-01.jpg',
      date: '20',
      month: 'Sep',
      year: '2023',
      time: '06:20 AM',
      address: '152, 1st Street New York',
      event: 'Smash Masters',
    },
    {
      img: 'assets/img/events/event-02.jpg',
      date: '19',
      month: 'Sep',
      year: '2023',
      time: '06:20 AM',
      address: '152, 1st Street New York',
      event: 'Rise to Victory',
    },
    {
      img: 'assets/img/events/event-03.jpg',
      date: '18',
      month: 'Sep',
      year: '2023',
      time: '06:20 AM',
      address: '152, 1st Street New York',
      event: 'Shuttle Storm',
    },
    {
      img: 'assets/img/events/event-04.jpg',
      date: '17',
      month: 'Sep',
      year: '2023',
      time: '06:20 AM',
      address: '152, 1st Street New York',
      event: 'Flight of the Feathers',
    },
    {
      img: 'assets/img/events/event-05.jpg',
      date: '16',
      month: 'Sep',
      year: '2023',
      time: '06:20 AM',
      address: '152, 1st Street New York',
      event: 'Battle at the Net',
    },
    {
      img: 'assets/img/events/event-06.jpg',
      date: '15',
      month: 'Sep',
      year: '2023',
      time: '06:20 AM',
      address: '152, 1st Street New York',
      event: 'Badminton Fusion',
    },
  ];
  public allservices = [
    {
      img: 'assets/img/services/service-05.jpg',
      service: 'Court Rent',
      para: 'Rent a premium court for your sports activities. Check availability, reserve easily, and enjoy state-of-the-art facilities at competitive rates.',
    },
    {
      img: 'assets/img/services/service-06.jpg',
      service: 'Group Lesson',
      para: 'Discover the thrill of group lessons in badminton, where you can enhance your skills, connect with others, and enjoy the sport to the fullest.',
    },
    {
      img: 'assets/img/services/service-07.jpg',
      service: 'Training Program',
      para: 'Our badminton training program provides a holistic approach to promote your performance and maximize your potential on the court.',
    },
    {
      img: 'assets/img/services/service-08.jpg',
      service: 'Private Lessons',
      para: 'Discover the thrill of group lessons in badminton, where you can enhance your skills, connect with others, and enjoy the sport to the fullest.',
    },
    {
      img: 'assets/img/services/service-09.jpg',
      service: 'Court Rent',
      para: 'Rent a premium court for your sports activities. Check availability, reserve easily, and enjoy state-of-the-art facilities at competitive rates.',
    },
    {
      img: 'assets/img/services/service-10.jpg',
      service: 'Group Lesson',
      para: 'Discover the thrill of group lessons in badminton, where you can enhance your skills, connect with others, and enjoy the sport to the fullest.',
    },
  ];
  public coaching = [
    {
      img: 'assets/img/services/service-05.jpg',
      service: 'Sports Performance Training',
      para: 'DreamSports offers tailored sports performance training programs designed to enhance your athletic performance in badminton.',
    },
    {
      img: 'assets/img/services/service-06.jpg',
      service: 'Sports Science and Technology',
      para: 'Utilize advanced technology, such as video analysis and motion sensors, to evaluate and improve your technique.',
    },
    {
      img: 'assets/img/services/service-07.jpg',
      service: 'Sports Performance Training',
      para: 'Discover the thrill of group lessons in badminton, where you can enhance your skills, connect with others, and enjoy the sport to the fullest.',
    },
  ];
  public lessons = [
    {
      img: 'assets/img/services/service-08.jpg',
      service: 'Service 4',
      para: 'Lorem ipsum is simply free text dolor sit am adipi we help you ensure everyone.',
    },
    {
      img: 'assets/img/services/service-09.jpg',
      service: 'Service 5',
      para: 'Lorem ipsum is simply free text dolor sit am adipi we help you ensure everyone.',
    },
    {
      img: 'assets/img/services/service-10.jpg',
      service: 'Service 6',
      para: 'Lorem ipsum is simply free text dolor sit am adipi we help you ensure everyone.',
    },
  ];
  public Coaches = [
    {
      img: 'assets/img/services/service-05.jpg',
      service: 'Elite Badminton',
      para: 'Our team of experienced and qualified coaches is dedicated to helping you achieve your goals.',
    },
    {
      img: 'assets/img/services/service-06.jpg',
      service: 'Service 2',
      para: 'Our team of experienced and qualified coaches is dedicated to helping you achieve your goals.',
    },
    {
      img: 'assets/img/services/service-07.jpg',
      service: 'Service 3',
      para: 'Our team of experienced and qualified coaches is dedicated to helping you achieve your goals.',
    },
  ];
  public brandlogos = [
    {
      img: 'assets/img/testimonial-icon-01.svg',
    },
    {
      img: 'assets/img/testimonial-icon-04.svg',
    },
    {
      img: 'assets/img/testimonial-icon-03.svg',
    },
    {
      img: 'assets/img/testimonial-icon-04.svg',
    },
    {
      img: 'assets/img/testimonial-icon-05.svg',
    },
    {
      img: 'assets/img/testimonial-icon-03.svg',
    },
    {
      img: 'assets/img/testimonial-icon-04.svg',
    },
  ];
  public testimonials = [
    {
      rating: '5.0',
      heading: 'Finibus mauris. Aliquam consectetur',
      content:
        'Finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passagesnunc. In sem leo, fermentum at lorem in, porta finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passages',
      img: 'assets/img/profiles/avatar-01.jpg',
      name: 'Ariyan Rusov',
      field: 'Badminton',
    },
    {
      rating: '5.0',
      heading: 'Finibus mauris. Aliquam consectetur',
      content:
        'Finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passagesnunc. In sem leo, fermentum at lorem in, porta finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passages',
      img: 'assets/img/profiles/avatar-05.jpg',
      name: 'Ariyan Rusov',
      field: 'Badminton',
    },
    {
      rating: '5.0',
      heading: 'Finibus mauris. Aliquam consectetur',
      content:
        'Finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passagesnunc. In sem leo, fermentum at lorem in, porta finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passages',
      img: 'assets/img/profiles/avatar-06.jpg',
      name: 'Ariyan Rusov',
      field: 'Badminton',
    },
    {
      rating: '5.0',
      heading: 'Finibus mauris. Aliquam consectetur',
      content:
        'Finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passagesnunc. In sem leo, fermentum at lorem in, porta finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passages',
      img: 'assets/img/profiles/avatar-01.jpg',
      name: 'Ariyan Rusov',
      field: 'Badminton',
    },
    {
      rating: '5.0',
      heading: 'Finibus mauris. Aliquam consectetur',
      content:
        'Finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passagesnunc. In sem leo, fermentum at lorem in, porta finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passages',
      img: 'assets/img/profiles/avatar-05.jpg',
      name: 'Ariyan Rusov',
      field: 'Badminton',
    },
    {
      rating: '5.0',
      heading: 'Finibus mauris. Aliquam consectetur',
      content:
        'Finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passagesnunc. In sem leo, fermentum at lorem in, porta finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passages',
      img: 'assets/img/profiles/avatar-06.jpg',
      name: 'Ariyan Rusov',
      field: 'Badminton',
    },
    {
      rating: '5.0',
      heading: 'Finibus mauris. Aliquam consectetur',
      content:
        'Finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passagesnunc. In sem leo, fermentum at lorem in, porta finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passages',
      img: 'assets/img/profiles/avatar-01.jpg',
      name: 'Ariyan Rusov',
      field: 'Badminton',
    },
    {
      rating: '5.0',
      heading: 'Finibus mauris. Aliquam consectetur',
      content:
        'Finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passagesnunc. In sem leo, fermentum at lorem in, porta finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passages',
      img: 'assets/img/profiles/avatar-05.jpg',
      name: 'Ariyan Rusov',
      field: 'Badminton',
    },
    {
      rating: '5.0',
      heading: 'Finibus mauris. Aliquam consectetur',
      content:
        'Finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passagesnunc. In sem leo, fermentum at lorem in, porta finibus mauris. Aliquam consectetur, ex in gravida porttitor. There are many variations of passages',
      img: 'assets/img/profiles/avatar-06.jpg',
      name: 'Ariyan Rusov',
      field: 'Badminton',
    },
  ];

  public userMenus: Array<usermenu> = [
    {
      title: 'Book Now',
      img: 'assets/img/icons/booking-icon.svg',
      page: 'bookings',
      page2: 'user bookings',
      route: routes.userBookings,

    },
    {
      title: 'Chat',
      img: 'assets/img/icons/chat-icon.svg',
      page: 'chat',
      route: routes.userChat,

    },
    {
      title: 'Invitation',
      img: 'assets/img/icons/Invitation-icon.svg',
      page: 'Invitation',
      route: routes.userInvitation,

    },
    {
      title: 'Team',
      img: 'assets/img/icons/team.svg',
      page: 'wallet',
      route: routes.userTeam,

    },
    {
      title: 'Profile Setting',
      img: 'assets/img/icons/profile-icon.svg',
      page: 'profile',
      page2: 'change password',
      page3: 'user profile',
      route: routes.userProfile,

    },
  ];
  public getuserMenus: BehaviorSubject<Array<usermenu>> = new BehaviorSubject<
    Array<usermenu>
  >(this.userMenus);
  public coachPages: Array<coachPages> = [
    /*{
      title: 'Dashboard',
      img: 'assets/img/icons/dashboard-icon.svg',
      route: routes.coachDashboard,
      page: 'dashboard',
      span: false,
    },*/
    {
      title: 'Courts',
      img: 'assets/img/icons/court-icon.svg',
      route: routes.coachCourts,
      page: 'courts',
      span: false,
    },
    {
      title: 'Requests',
      img: 'assets/img/icons/request-icon.svg',
      route: routes.coachRequest,
      page: 'requests',
      span: true,
    },
    /*{
      title: 'Bookings',
      img: 'assets/img/icons/booking-icon.svg',
      route: routes.coachBooking,
      page: 'booking',
      span: false,
    },
    {
      title: 'Earnings',
      img: 'assets/img/icons/Invitation-icon.svg',
      route: routes.coachEarning,
      page: 'earnings',
      span: false,
    },*/
    {
      title: 'Users',
      img: 'assets/img/icons/profile-icon.svg',
      page: 'users',
      route: routes.coachUsers,
      span: false,
    },
    {
      title: 'Profile Setting',
      img: 'assets/img/icons/profile-icon.svg',
      page: 'profile settings',
      route: routes.coachProfile,
      span: false,
    },
    
  ];
  public getCoachPages: BehaviorSubject<Array<coachPages>> =
    new BehaviorSubject<Array<coachPages>>(this.coachPages);
    
    
    public adminPages: Array<adminPages> =  [
     /* {
        title: 'Dashboard',
        img: 'assets/img/icons/dashboard-icon.svg',
        route: routes.adminDashboard,
        page: 'dashboard',
        span: false,
      },*/
      {
        title: 'Employees',
        img: 'assets/img/icons/employee.svg',
        route: routes.admin_employees,
        page: 'employees',
        span: false,
      },
      {
        title: 'Site',
        img: 'assets/img/icons/Sitee.svg',
        route: routes.admin_site,
        page: 'site',
        span: false,
      },
      {
        title: 'Annonces',
        img: 'assets/img/icons/annonce.svg',
        route: routes.admin_annonces,
        page: 'annonces',
        span: false,
      },
      {
        title: 'FAQs',
        img: 'assets/img/icons/faqq.svg',
        route: routes.admin_faqs,
        page: 'faqs',
        span: false,
      },
      {
        title: 'Courts',
        img: 'assets/img/icons/stadium.svg',
        route: routes.admin_courts,
        page: 'courts',
        span: false,
      },
      {
        title: 'Profile Setting',
        img: 'assets/img/icons/profile-icon.svg',
        page: 'profile settings',
        route: routes.admin_profile_edit,
        span: false,
      },
      
    ];
    public getAdminPages: BehaviorSubject<Array<adminPages>> =
    new BehaviorSubject<Array<adminPages>>(this.adminPages);


  userDashboard: Array<coaches> = [
    {
      id: 1,
      img: 'assets/img/booking/booking-02.jpg',
      image2: 'assets/img/featured/featured-05.jpg',
      name: 'Kevin Anderson',
      category: 'Onetime',
      court: 'Leap Sports Academy',
      courtcount: '1',
      guests: '4',
      hours: '2hrs',
      date: 'Mon, Jul 11',
      time: '06:00 PM - 08:00 PM',
      amount: '400',
    },
    {
      id: 2,
      img: 'assets/img/booking/booking-03.jpg',
      image2: 'assets/img/featured/featured-06.jpg',
      name: 'Angela Roudrigez',
      category: 'Single Lesson',
      court: 'Wing Sports Academy',
      courtcount: '3',
      guests: '3',
      hours: '3hrs',
      date: 'Mon, Jul 12',
      time: '09:00 PM - 12:00 PM',
      amount: '600',
    },
    {
      id: 3,
      img: 'assets/img/booking/booking-03.jpg',
      image2: 'assets/img/featured/featured-07.jpg',
      name: 'Evon Raddick',
      category: 'Onetime',
      court: 'Feather Badminton',
      courtcount: '1',
      guests: '5',
      hours: '6hrs',
      date: 'Mon, Jul 11',
      time: '06:00 PM - 12:00 PM',
      amount: '1200',
    },
    {
      id: 4,
      img: 'assets/img/booking/booking-04.jpg',
      image2: 'assets/img/featured/featured-08.jpg',
      name: 'Harry Richardson',
      category: 'Onetime',
      court: 'Bwing Sports Academy',
      courtcount: '3',
      guests: '6',
      hours: '1hr',
      date: 'Mon, Jul 13',
      time: '04:00 PM - 08:00 PM',
      amount: '1400',
    },
    {
      id: 5,
      img: 'assets/img/booking/booking-05.jpg',
      image2: 'assets/img/featured/featured-09.jpg',
      name: 'Pete Hill',
      category: 'Onetime',
      court: 'Feather Badminton',
      courtcount: '1',
      guests: '5',
      hours: '6hrs',
      date: 'Mon, Jul 11',
      time: '06:00 PM - 12:00 PM',
      amount: '1200',
    },
  ];
  public coachDetail1 = [
    {
      img: 'assets/img/gallery/gallery3/gallery-01.jpg',
    },
    {
      img: 'assets/img/gallery/gallery3/gallery-02.jpg',
    },
    {
      img: 'assets/img/gallery/gallery3/gallery-03.jpg',
    },
    {
      img: 'assets/img/gallery/gallery3/gallery-01.jpg',
    },
    {
      img: 'assets/img/gallery/gallery3/gallery-02.jpg',
    },
    {
      img: 'assets/img/gallery/gallery3/gallery-03.jpg',
    },
  ];
  public coachDetail2 = [
    {
      img: 'assets/img/gallery/gallery-01.jpg',
    },
    {
      img: 'assets/img/gallery/gallery-02.jpg',
    },
    {
      img: 'assets/img/gallery/gallery-03.jpg',
    },
    {
      img: 'assets/img/gallery/gallery-04.jpg',
    },
    {
      img: 'assets/img/gallery/gallery-05.jpg',
    },
  ];
  public bloglist = [
    {
      title: 'Badminton',
      img1: 'assets/img/blog/blog-01.jpg',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'OrlandoWaters',
      year: '15 May 2023',
      para: 'Mastering the Badminton Basics: A Guide for Beginners',
      head: "Master the fundamentals of badminton with our beginner's guide, unlocking the path to skillful play and enjoyment on the court.",
      feedback: '45',
      command: '40',
      img3: 'assets/img/icons/clock.svg',
      time: '10 Min To Read',
    },
    {
      title: 'Sports Activites',
      img1: 'assets/img/blog/blog-02.jpg',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'ClaireNichols',
      year: '16 Jun 2023',
      para: 'Unleashing Your Badminton Potential: Tips for Skill Development',
      head: "Unlock your badminton potential and elevate your skills with our expert tips for effective skill development on the court",
      feedback: '32',
      command: '21',
      img3: 'assets/img/icons/clock.svg',
      time: '5 Min To Read',
    },
    {
      title: 'Rules of Game',
      img1: 'assets/img/blog/blog-03.jpg',
      img2: 'assets/img/profiles/user-04.jpg',
      name: 'JoannaLe',
      year: '17 May 2023',
      para: 'The Art of Footwork: Enhancing Agility in Badminton ',
      head: "Master the art of footwork and elevate your agility in badminton with our proven techniques and training methods.",
      feedback: '55',
      command: '32',
      img3: 'assets/img/icons/clock.svg',
      time: '4 Min To Read',
    },
  ];
  public blogleft = [
    {
      title: 'Badminton',
      img1: 'assets/img/blog/blog-01.jpg',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'OrlandoWaters',
      year: '15 May 2023',
      para: 'Mastering the Badminton Basics: A Guide for Beginners',
      head: "Master the fundamentals of badminton with our beginner's guide, unlocking the path to skillful play and enjoyment on the court.",
      feedback: '45',
      command: '40',
      img3: 'assets/img/icons/clock.svg',
      time: '10 Min To Read',
    },
    {
      title: 'Sports Activites',
      img1: 'assets/img/blog/blog-02.jpg',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'ClaireNichols',
      year: '16 Jun 2023',
      para: 'Unleashing Your Badminton Potential: Tips for Skill Development',
      head: "Unlock your badminton potential and elevate your skills with our expert tips for effective skill development on the court.",
      feedback: '32',
      command: '21',
      img3: 'assets/img/icons/clock.svg',
      time: '5 Min To Read',
    },
    {
      title: 'Rules of Game',
      img1: 'assets/img/blog/blog-03.jpg',
      img2: 'assets/img/profiles/user-04.jpg',
      name: 'JoannaLe',
      year: '17 May 2023',
      para: 'The Art of Footwork: Enhancing Agility in Badminton',
      head: "Master the art of footwork and elevate your agility in badminton with our proven techniques and training methods.",
      feedback: '55',
      command: '32',
      img3: 'assets/img/icons/clock.svg',
      time: '4 Min To Read',
    },
  ];
  public bloggrid = [
    {
      title: 'Badminton',
      img1: 'assets/img/venues/venues-07.jpg',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'OrlandoWaters',
      year: '15 May 2023',
      para: 'Mastering the Badminton Basics: A Guide for Beginners',
      feedback: '45',
      command: '40',
      img3: 'assets/img/icons/clock.svg',
      time: '10 Min To Read',
    },
    {
      title: 'Sports Activites',
      img1: 'assets/img/venues/venues-08.jpg',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'ClaireNichols',
      year: '16 Jun 2023',
      para: 'Unleashing Your Badminton Potential: Tips for Skill Development',
      feedback: '32',
      command: '21',
      img3: 'assets/img/icons/clock.svg',
      time: '5 Min To Read',
    },
    {
      title: 'Rules of Game',
      img1: 'assets/img/venues/venues-09.jpg',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'JoannaLe',
      year: '17 May 2023',
      para: 'Unleashing Your Badminton Potential: Tips for Skill Development',
      feedback: '55',
      command: '32',
      img3: 'assets/img/icons/clock.svg',
      time: '4 Min To Read',
    },
    {
      title: 'Badminton',
      img1: 'assets/img/venues/venues-15.jpg',
      img2: 'assets/img/profiles/avatar-02.jpg',
      name: 'Andrew',
      year: '17 May 2023',
      para: 'Unleashing Your Badminton Potential: Tips for Skill Development',
      feedback: '55',
      command: '32',
      img3: 'assets/img/icons/clock.svg',
      time: '4 Min To Read',
    },
  ];
  public listinggrid = [
    {
      img1: 'assets/img/venues/venues-03.jpg',
      fees: '$350',
      time: '/hr',
      points: '4.7',
      reviews: '120 Reviews',
      title: 'Manchester Academy',
      para: "Manchester Academy: Where dreams meet excellence in sports education and training",
      address: 'Guysville, OH',
      open: 'Next availablity : ',
      date: '16 May 2023',
      img2: 'assets/img/profiles/avatar-03.jpg',
      name: 'Andrew',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-04.jpg',
      fees: '$100',
      time: '/hr',
      points: '5.0',
      reviews: '100 Reviews',
      title: 'NG Sports Academy',
      para: "Achieve your badminton goals at NG Sports. Book your training session now in our futuristic facility",
      address: 'Roanoke, VA',
      open: 'Next availablity : ',
      date: '20 May 2023',
      img2: 'assets/img/profiles/avatar-05.jpg',
      name: 'Jerry',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-05.jpg',
      fees: '$540',
      time: '/hr',
      points: '4.5',
      reviews: '80 Reviews',
      title: 'Rivan Badminton Academy',
      para: "Rivan Badminton Academy: Where dreams soar through excellence in badminton training",
      address: 'Huntsville, AL',
      open: 'Next availablity : ',
      date: '19 May 2023',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'Jerry',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-06.jpg',
      fees: '$780',
      time: '/hr',
      points: '4.2',
      reviews: '140 Reviews',
      title: 'Russ Badminton',
      para: "Russ Badminton : Nurturing excellence in badminton education & training.",
      address: 'Sacramento, CA',
      open: 'Next availablity : ',
      date: '18 May 2023',
      img2: 'assets/img/profiles/avatar-04.jpg',
      name: 'Melvin',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-07.jpg',
      fees: '$550',
      time: '/hr',
      points: '5.0',
      reviews: '120 Reviews',
      title: 'Andy’s Sports Academy',
      para: "Unlock your athletic potential at Andy's Sport Academy. Book now at our state-of-the-art facility",
      address: 'Washington, MD',
      open: 'Next availablity : ',
      date: '19 May 2023',
      img2: 'assets/img/profiles/avatar-07.jpg',
      name: 'Richard',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-08.jpg',
      fees: '$740',
      time: '/hr',
      points: '4.5',
      reviews: '470 Reviews',
      title: 'Herique Badminton',
      para: "Herique Badminton: Elevate your game. Train with passion at our exclusive facility.",
      address: 'Indianapolis, IN',
      open: 'Next availablity : ',
      date: '16 May 2023',
      img2: 'assets/img/profiles/avatar-08.jpg',
      name: 'Wayne',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-09.jpg',
      fees: '$470',
      time: '/hr',
      points: '4.2',
      reviews: '70 Reviews',
      title: 'AG Sports',
      para: "AG Sports: Ignite your sporting journey. Train with dedication at our premier facility",
      address: 'Roseau, MN',
      open: 'Next availablity : ',
      date: '17 May 2023',
      img2: 'assets/img/profiles/avatar-09.jpg',
      name: 'Wayne',
      select: 'Book Now',
    },
  ];
  public listinggridsidebar = [
    {
      fees: '$450',
      img1: 'assets/img/venues/venues-01.jpg',
      time: '/hr',
      points: '4.2',
      reviews: '300 Reviews',
      title: 'Sarah Sports Academy',
      para: "Elevate your athletic journey at Sarah Sports Academy, where excellence meets opportunity.",
      address: 'Port Alsworth, AK',
      open: 'Next availablity : ',
      date: '21 May 2023',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'Mart Sublin',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-02.jpg',
      fees: '$200',
      time: '/hr',
      points: '5.0',
      reviews: '150 Reviews',
      title: 'Badminton Academy',
      para: "Unleash your badminton potential at our premier Badminton Academy, where champions are made.",
      address: 'Sacramento, CA',
      open: 'Next availablity : ',
      date: '15 May 2023',
      img2: 'assets/img/profiles/avatar-02.jpg',
      name: 'Mart Sublin',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-03.jpg',
      fees: '$350',
      time: '/hr',
      points: '4.7',
      reviews: '120 Reviews',
      title: 'Manchester Academy',
      para: "Manchester Academy: Where dreams meet excellence in sports education and training",
      address: 'Guysville, OH',
      open: 'Next availablity : ',
      date: '16 May 2023',
      img2: 'assets/img/profiles/avatar-03.jpg',
      name: 'Andrew',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-04.jpg',
      fees: '$100',
      time: '/hr',
      points: '5.0',
      reviews: '100 Reviews',
      title: 'NG Sports Academy',
      para: "Achieve your badminton goals at NG Sports. Book your training session now in our futuristic facility",
      address: 'Roanoke, VA',
      open: 'Next availablity : ',
      date: '20 May 2023',
      img2: 'assets/img/profiles/avatar-05.jpg',
      name: 'Jerry',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-05.jpg',
      fees: '$540',
      time: '/hr',
      points: '4.5',
      reviews: '80 Reviews',
      title: 'Rivan Badminton Academy',
      para: "Rivan Badminton Academy: Where dreams soar through excellence in badminton training",
      address: 'Huntsville, AL',
      open: 'Next availablity : ',
      date: '19 May 2023',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'Jerry',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-06.jpg',
      fees: '$780',
      time: '/hr',
      points: '4.2',
      reviews: '140 Reviews',
      title: 'Russ Badminton',
      para: "Russ Badminton : Nurturing excellence in badminton education & training",
      address: 'Sacramento, CA',
      open: 'Next availablity : ',
      date: '18 May 2023',
      img2: 'assets/img/profiles/avatar-04.jpg',
      name: 'Melvin',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-07.jpg',
      fees: '$550',
      time: '/hr',
      points: '5.0',
      reviews: '120 Reviews',
      title: 'Andy’s Sports Academy',
      para: "Unlock your athletic potential at Andy's Sport Academy. Book now at our state-of-the-art facility",
      address: 'Washington, MD',
      open: 'Next availablity : ',
      date: '19 May 2023',
      img2: 'assets/img/profiles/avatar-07.jpg',
      name: 'Richard',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-08.jpg',
      fees: '$740',
      time: '/hr',
      points: '4.5',
      reviews: '470 Reviews',
      title: 'Herique Badminton',
      para: "Herique Badminton: Elevate your game. Train with passion at our exclusive facility",
      address: 'Indianapolis, IN',
      open: 'Next availablity : ',
      date: '16 May 2023',
      img2: 'assets/img/profiles/avatar-08.jpg',
      name: 'Wayne',
      select: 'Book Now',
    },
  ];
  public sports = [
    {
      img1: 'assets/img/venues/venues-03.jpg',
      fees: '$350',
      time: '/hr',
      points: '4.7',
      reviews: '120 Reviews',
      title: 'Manchester Academy',
      para: "Manchester Academy: Where dreams meet excellence in sports education and training",
      address: 'Little Rock, AR',
      open: 'Next availablity : ',
      date: '16 May 2023',
      img2: 'assets/img/profiles/avatar-03.jpg',
      name: 'Andrew',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-10.jpg',
      fees: '$100',
      time: '/hr',
      points: '5.0',
      reviews: '100 Reviews',
      title: 'NG Sports Academy',
      para: "Achieve your badminton goals at NG Sports. Book your training session now in our futuristic facility.",
      address: 'Roanoke, VA',
      open: 'Next availablity : ',
      date: '20 May 2023',
      img2: 'assets/img/profiles/avatar-05.jpg',
      name: 'Jerry',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-11.jpg',
      fees: '$540',
      time: '/hr',
      points: '4.5',
      reviews: '80 Reviews',
      title: 'Rivan Badminton Academy',
      para: "Rivan Badminton Academy: Where dreams soar through excellence in badminton training.",
      address: 'Huntsville, AL',
      open: 'Next availablity : ',
      date: '19 May 2023',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'Jerry',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-12.jpg',
      fees: '$780',
      time: '/hr',
      points: '4.5',
      reviews: '140 Reviews',
      title: 'Russ Badminton',
      para: "Russ Badminton : Nurturing excellence in badminton education & training",
      address: 'Sacramento, CA',
      open: 'Next availablity : ',
      date: '18 May 2023',
      img2: 'assets/img/profiles/avatar-04.jpg',
      name: 'Melvin',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-13.jpg',
      fees: '$550',
      time: '/hr',
      points: '5',
      reviews: '140 Reviews',
      title: 'Andy’s Sports Academy',
      para: "Unlock your athletic potential at Andy's Sport Academy. Book now at our state-of-the-art facility",
      address: 'Washington, MD',
      open: 'Next availablity : ',
      date: '18 May 2023',
      img2: 'assets/img/profiles/avatar-04.jpg',
      name: 'Richard',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-13.jpg',
      fees: '$740',
      time: '/hr',
      points: '4.5',
      reviews: '140 Reviews',
      title: 'Herique Badminton',
      para: "Herique Badminton: Elevate your game. Train with passion at our exclusive facility",
      address: 'Indianapolis, IN',
      open: 'Next availablity : ',
      date: '16 May 2023',
      img2: 'assets/img/profiles/avatar-08.jpg',
      name: 'Wayne',
      select: 'Book Now',
    },
    {
      img1: 'assets/img/venues/venues-14.jpg',
      fees: '$470',
      time: '/hr',
      points: '4.2',
      reviews: '70 Reviews',
      title: 'AG Sports',
      para: "AG Sports: Ignite your sporting journey. Train with dedication at our premier facility.",
      address: 'Roseau, MN',
      open: 'Next availablity : ',
      date: '17 May 2023',
      img2: 'assets/img/profiles/avatar-09.jpg',
      name: 'Philip',
      select: 'Book Now',
    },
  ];

  public coachTimeDate = [
    {
      day: 'Monday',
      date: 'Apr 24',
    },
    {
      day: 'Tuesday',
      date: 'Apr 25',
    },
    {
      day: 'Wednesday',
      date: 'Apr 26',
    },
    {
      day: 'Thursday',
      date: 'Apr 27',
    },
    {
      day: 'Friday',
      date: 'Apr 28',
    },
    {
      day: 'Saturday',
      date: 'Apr 29',
    },
  ];
  featuredVenuesSlider = [
    {
      img: 'assets/img/featured/featured-05.jpg',
      content: 'Professional',
      price: 'From $350',
      span: '/hr',
      name: 'Kevin Anderson',
      place: 'Port Alsworth, AK',
      para: "Certified Badminton Coach with a deep understanding of the sport's and strategies game.",
      profile: 'View Profile',
      booking: 'Book Now',
      availability: 'Next Availabilty',
      date: '20 May 2023',
      rating: '4.5',
      reviews: '80 Reviews',
      like: false,
    },
    {
      img: 'assets/img/featured/featured-06.jpg',
      content: 'Rookie',
      price: 'From $120',
      span: '/hr',
      name: 'Angela Roudrigez',
      place: 'Guysville, OH',
      para: "Experienced coach dedicated to enhancing your badminton skills and unlocking your full potential",
      profile: 'View Profile',
      booking: 'Book Now',
      availability: 'Next Availabilty',
      date: '21 May 2023',
      rating: '4.5',
      reviews: '80 Reviews',
      like: false,
    },
    {
      img: 'assets/img/featured/featured-07.jpg',
      content: 'Professional',
      price: 'From $750',
      span: '/hr',
      name: 'Evon Raddick',
      place: 'Little Rock, AR',
      para: "Passionate Badminton Coach unlocking players' potential through strategic gameplay",
      profile: 'View Profile',
      booking: 'Book Now',
      availability: 'Next Availabilty',
      date: '22 May 2023',
      rating: '4.5',
      reviews: '470 Reviews',
      like: false,
    },
    {
      img: 'assets/img/featured/featured-08.jpg',
      content: 'Intermediate',
      price: 'From $550',
      span: '/hr',
      name: 'Harry Richardson',
      place: 'Roanoke, VA',
      para: "Experienced coach dedicated to enhancing your badminton skills and unlocking your full potential",
      profile: 'View Profile',
      booking: 'Book Now',
      availability: 'Next Availabilty',
      date: '27 May 2023',
      rating: '4.5',
      reviews: '180 Reviews',
      like: false,
    },
  ];
}
