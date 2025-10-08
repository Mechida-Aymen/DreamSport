export interface AmisChatReturnedDTO {
    id: number;
    amiName: string;
    lastMessage: string;
    date: Date;
    unreadCount?: number;
    statut?: string;
    avatar: string;
    idMember:string;
    type: 'amis'; 
  }

 export interface PaginatedResponse<T> {
    page: number;
    pageSize: number;
    totalCount: number;
    items: T[];
  }