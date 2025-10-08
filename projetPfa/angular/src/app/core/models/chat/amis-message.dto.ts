import { UserInfoDTO } from "./user-info.dto";

export interface AmisMessageDTO {
    id: number;
    contenu: string;
    dateEnvoi: Date;
    emetteur: UserInfoDTO;
    statut: string;
    chatAmisId: number;  
    RecepteurId?:number;

  }
