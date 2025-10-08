export interface pageSelection {
  skip: number;
  limit: number;
}
export interface apiResultFormat {
  data: [];
  totalData: number;
}
export interface routerlink {
  url: string;
  id: number;
  type: number;
}
export interface badmintonList {
  title: string;
  img1: string;
  img2: string;
  name: string;
  year: string | number;
  para: string;
  feedback: string;
  command: string;
  img3: string;
  time: string | number;
  favourite?: boolean;
}
export interface venuesList {
  title: string;
  img1: string;
  img2: string;
  name: string;
  year: string | number;
  para: string;
  feedback: string;
  command: string;
  img3: string;
  time: string | number;
  favourite?: boolean;
}
export interface commandList {
  name: string;
  img: string;
  date: string | number;
  para: string;
  response: string;
}
export interface latestpost {
  img: string;
  name: string;
  content: string;
}
export interface usermenu {
  title?: string,
  img?: string,
  page?: string,
  page1?: string
  route?: string,
  page2?: string,
  page3?: string,

}
export interface coachPages {
  title?: string,
  img?: string,
  page?: string,
  page1?: string
  route?: string,
  span : boolean
}
export interface adminPages {
  title?: string,
  img?: string,
  page?: string,
  page1?: string
  route?: string,
  span : boolean
}
export interface blogmenu {
  title?: string;
  img?: string;
  page?: string;
  route?: string;
}
export interface userDashboard {
  id: number;
  image: string;
  image2?: string;
  name?: string;
  booked?: string;
  category?: string;
  content: string;
  court: string;
  date: string;
  time: string;
  payment: string;
  paiddate: string;
  status: string;
}
export interface bookings {
  id: number;
  image: string;
  image2?: string;
  name?: string;
  booked?: string;
  category?: string;
  content: string;
  court: string;
  date: string;
  time: string;
  payment: string;
  paiddate: string;
  status: string;
  availability?: string;
  star: boolean;
}
export interface coaches {
  id: number;
  img: string;
  image2?: string;
  name?: string;
  category?: string;
  court: string;
  courtcount: string;
  guests: string;
  hours: string;
  date: string;
  time: string;
  amount: string;
}
export interface bloglist {
  title: string;
  img1: string;
  img2: string;
  name: string;
  year: string | number;
  para: string;
  head: string;
  feedback: string | number;
  command: string | number;
  img3: string;
  time: string | number;
  favourite?: boolean;
}
export interface blogleft {
  title: string;
  img1: string;
  img2: string;
  name: string;
  year: string | number;
  para: string;
  head: string;
  feedback: string;
  command: string;
  img3: string;
  time: string | number;
  favourite?: boolean;
}
export interface url {
  url: string;
}
export interface coachDetail {
  img: string;
  gallery: string;
}
export interface ourteams {
  img: string;
  name: string;
  role: string;
  para: string;
}
export interface events {
  img: string;
  date: string;
  month: string;
  year: string;
  time: string;
  address: string;
  event: string;
}
export interface brandlogos {
  img: string;
}
export interface allservices {
  img: string;
  service: string;
  para: string;
}
export interface coaching {
  img: string;
  service: string;
  para: string;
}
export interface lessons {
  img: string;
  service: string;
  para: string;
}
export interface Coaches {
  img: string;
  service: string;
  para: string;
}
export interface testimonials {
  rating: string;
  heading: string;
  content: string;
  img: string;
  name: string;
  field: string;
}
export interface listinggrid {
 
  fees: string;
  img1: string;
  time: string;
  points: string;
  reviews: string | number;
  title: string;
  para: string;
  address: string;
  open: string;
  date: string | number;
  img2: string;
  name: string;
  select: string;
  favourite?: boolean;
}
export interface listinggridsidebar {
  
  fees: string;
  img1: string;
  time: string;
  points: string;
  reviews: string | number;
  title: string;
  para: string;
  address: string;
  open: string;
  date: string | number;
  img2: string;
  name: string;
  select: string;
  favourite?: boolean;
}
export interface sports {
 
  fees: string;
  img1: string;
  time: string;
  points: string;
  reviews: string | number;
  title: string;
  para: string;
  address: string;
  open: string;
  date: string | number;
  img2: string;
  name: string;
  select: string;
  favourite?: boolean;
}
export interface Invitation {
  id: number;
  image: string;
  image2?: string;
  name?: string;
  booked?: string;
  category?: string;
  content: string;
  court: string;
  date: string;
  time: string;
  payment: string;
  paiddate: string;
  status: string;
  availability?: string;
  paidOn: string;
  coachId: string;
  refId: string
}
export interface bloggrid {
  title: string;
  img1: string;
  img2: string;
  name: string;
  year: string;
  para: string;
  feedback: string;
  command: string;
  img3: string;
  time: string;
  favourite?: boolean;
}
