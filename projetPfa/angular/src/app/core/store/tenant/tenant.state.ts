export interface TenantState {
    tenantId:any | null;
    siteInfo: any | null;
    faq: any | null;
    annonces: any | null;
    error: string | null;
  }
  
  export const initialTenantState: TenantState = {
    siteInfo: null,
    faq: null,
    annonces: null,
    tenantId:null,
    error: null
  };
  