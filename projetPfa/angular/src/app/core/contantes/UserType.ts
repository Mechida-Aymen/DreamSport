export enum UserType {
    ADMIN = 'Admin',
    EMPLOYEE = 'Employee',
    CLIENT = 'User',
  }

  export const LOGIN_ENDPOINTS: Record<UserType, string> = {
    [UserType.ADMIN]: '/login/admin',
    [UserType.EMPLOYEE]: '/login/employer',
    [UserType.CLIENT]: '/login/user',
  };