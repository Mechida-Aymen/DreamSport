export interface TeamChatReturnedDTO {
    id: number;
    equipeName: string;
    lasteMessage: string;
    date: Date;
    nbrMessage?: number;
    st?: string;
    avatar: string;
    type: 'team'; 

  }