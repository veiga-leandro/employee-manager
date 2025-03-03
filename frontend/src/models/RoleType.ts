export enum RoleType {
  Admin = 1,
  HR = 2,
  Manager = 3,
  Employee = 4
}

export const getRoleTypeLabel = (role: number): string => {
  switch (role) {
    case RoleType.Admin:
      return 'Administrador';
    case RoleType.HR:
      return 'RH';
    case RoleType.Manager:
      return 'Gestor';
    case RoleType.Employee:
      return 'Funcionário';
    default:
      return 'Desconhecido';
  }
};