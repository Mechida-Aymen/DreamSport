import { UserInfoDTO } from "./user-info.dto";

export interface TeamMessageDTO {
    id: number;
    contenu: string;
    dateEnvoi: Date;
    emetteur: UserInfoDTO;
    statut: string;
    teamId?:number;
    chatTeamId:number
  }