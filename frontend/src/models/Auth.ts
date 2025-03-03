export interface LoginCredentials {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  expiration: string;
}

export interface DecodedToken {
  nameid: string;
  email: string;
  unique_name: string;
  exp: number;
  [key: string]: any;
}