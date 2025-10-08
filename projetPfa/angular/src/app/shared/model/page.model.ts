export interface coachBooking {
  id: number;
  courtName: string;
  courtNumber: string;
  bookingTime: string;
  playerName: string;
  date: string;
  time: string;
  additionalGuests: string;
  payment: string;
  details: string;
  chat: string;
  img1: string;
  img2: string;
}
export interface coachEarning {
  id: number;
  courtName: string;
  courtNumber: string;
  payment: string;
  bookingTime: string;
  playerName: string;
  date: string;
  time: string;
  additionalGuests: string;
  download: string;
  chat: string;
  img1: string;
  img2: string;
  paidDate: string;
}
export interface venueCarousel {
  id?: number;
  reviews?: string;
  rating?: string;
  badge?: string;
  rate?: string;
  name?: string;
  title?: string;
  content?: string;
  address?: string;
  date?: string;
  img?: string;
  img2?: string;
  status?: string;
  favourite?: boolean;
  distance?: string;
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
  fav?: boolean;
}
export interface allcourts {
  id: number;
  courtname: string;
  noofcourt?: number | string;
  location: string;
  amount: string;
  maxguest: string;
  additionalguest: string;
  addedon: string;
  img: string;
  status: boolean;
}
export interface coachRequest {
  id: number;
  courtName: string;
  img1: string;
  img2: string;
  courtNumber: string;
  playerName: string;
  date: string;
  paidDate: string;
  time: string;
  additionalGuests: string;
  payment: string;
  status1: string;
  status2: string;
  status3: string;
}
export interface coachTimeDate {
  day: string;
  date: string;
}
export interface coachWallet {
  id: number;
  refID: string;
  transactionFor: string;
  date: string;
  time: string;
  payment: string;
  status: string;
  img: string;
}
export interface earningCoaching {
  id: number;
  playerName: string;
  img: string;
  lessonType: string;
  date: string;
  time: string;
  payment: string;
  additionalGuests: string;
  paidDate: string;
  download: string;
}
export interface featuredVenuesSlider {
  img: string;
  content: string;
  price: string;
  span: string;
  name: string;
  place: string;
  para: string;
  profile: string;
  booking: string;
  availability: string;
  date: string;
  reviews: string;
  like ?: boolean;
}
export interface coachDetail {
  img : string;
}