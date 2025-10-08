export interface TeamInvitationDTO {
  id: number;
  invitation: {
    id: number;
    name: string;
    description?: string;
    avatar: string;
  };
  Recerpteur?: number; 
  recepteur?: number; 
  adminId?: number;
}

export interface SendTeamInvitationDTO {
 
  Emetteur:number;
  Recepteur:number;
  

}