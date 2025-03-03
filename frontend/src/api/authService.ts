import axiosInstance from './axiosConfig';
import { LoginCredentials, AuthResponse } from '../models/Auth';

export const login = async (credentials: LoginCredentials): Promise<AuthResponse> => {
  const response = await axiosInstance.post<AuthResponse>('/auth/login', credentials);
  return response.data;
};