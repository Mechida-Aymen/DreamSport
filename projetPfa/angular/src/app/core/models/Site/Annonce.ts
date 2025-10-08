export interface Annonce {
    id: number;
    description: string | null;
    launchedAt: string;  // ISO format date string (e.g., "2023-11-15T14:30:45Z")
    lifeTimeBySeconds: number;
    adminId: number;
    endDate?: string; 
  }