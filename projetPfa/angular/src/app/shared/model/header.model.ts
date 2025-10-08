export interface sideBar {
    tittle?: string;
    base?: string;
    showAsTab?: boolean;
    separateRoute?: boolean;
    menu?: MenuItem[];
    base2?: string;
    base3?: string;
    base4?: string;
    route?: string;
    class?: boolean;
  }
  
  export interface MenuItem {
    menuValue: string;
    route?: string;
    hasSubRoute: boolean;
    showSubRoute: boolean;
    base?: string;
    page?: string;
    page2?: string;
    page3?: string;
    page4?: string;
    last?: string;
    icon?: string;
    subMenus: SubMenuItem[];
  }
  
  export interface SubMenuItem {
    menuValue: string;
    hasSubRoute: boolean;
    showSubRoute: boolean;
    base?: string;
    page?: string;
    last?: string;
    route?: string;
    subMenus: never[];
  }