export interface PhoneNumber {
  id?: string;
  number: string;
  type: number;
}

export interface Employee {
  id?: string;
  firstName: string;
  lastName: string;
  fullName?: string;
  email: string;
  cpf: string;
  managerId?: string;
  birthDate: string;
  role: number;
  phoneNumbers: PhoneNumber[];
  password?: string;
}