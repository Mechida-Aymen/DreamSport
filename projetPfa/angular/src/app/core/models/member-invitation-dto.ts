export interface MemberInvitationDTOO {
    id: number;
    emetteur: {
      id: number;
      firstName: string;
      lastName: string;
      username: string;
      imageUrl: string;
      bio:string;
    };
    Recerpteur?: number; // Ajoutez cette propriété
    recepteur?: number; // Gardez les deux pour compatibilité
    adminId?: number;
    dateCreated?: Date;
  }