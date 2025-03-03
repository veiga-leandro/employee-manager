export enum PhoneType {
  Mobile = 1,
  Home = 2,
  Work = 3,
  Other = 4
}

export const getPhoneTypeLabel = (type: number): string => {
  switch (type) {
    case PhoneType.Mobile:
      return 'Celular';
    case PhoneType.Home:
      return 'Residencial';
    case PhoneType.Work:
      return 'Trabalho';
    case PhoneType.Other:
      return 'Outro';
    default:
      return 'Desconhecido';
  }
};