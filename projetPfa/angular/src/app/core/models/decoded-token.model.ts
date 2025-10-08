export interface DecodedToken {
    id: number;
    userId: number;
    adminId: number;
    Role: string;
    nom: string;
    prenom: string;
    imageUrl?: string; // Optional
    exp?: number; // Token expiration time (Unix timestamp)
  }