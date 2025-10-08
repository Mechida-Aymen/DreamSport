import { Terrain } from "../../service/terrain/terrain.service";
import { userRes } from "../Users/userRes";

export interface getReservation {
    id: number;
    dateRes: string;
    idUtilisateur: number;
    idTerrain: number; 
    idEmploye: number;
    idStatus: number;
    status: resStatus;
    terrain: Terrain;
    user: userRes;
  }

  export interface resStatus {
    id: number;
    libelle: string;
  }