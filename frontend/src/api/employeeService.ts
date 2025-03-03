import axiosInstance from './axiosConfig';
import { Employee } from '../models/Employee';
import { PaginatedResponse } from 'models/Pagination';

export const getAllEmployees = async (
  page: number = 1, 
  pageSize: number = 10
): Promise<PaginatedResponse<Employee>> => {
  try {
    const response = await axiosInstance.get<PaginatedResponse<Employee>>(
      `/employees?page=${page}&pageSize=${pageSize}`
    );
    return response.data;
  } catch (error) {
    console.error('Erro ao buscar funcionários:', error);
    throw error;
  }
};

export const getAllEmployeesList = async (): Promise<Employee[]> => {
  try {
    const response = await getAllEmployees();
    return response.items || [];
  } catch (error) {
    console.error('Erro ao buscar lista de funcionários:', error);
    throw error;
  }
};

export const getEmployeeById = async (id: string): Promise<Employee> => {
  const response = await axiosInstance.get<Employee>(`/employees/${id}`);
  return response.data;
};

export const createEmployee = async (employee: Employee): Promise<Employee> => {
  const response = await axiosInstance.post<Employee>('/employees', employee);
  return response.data;
};

export const updateEmployee = async (id: string, employee: Employee): Promise<Employee> => {
  const response = await axiosInstance.put<Employee>(`/employees/${id}`, employee);
  return response.data;
};

export const deleteEmployee = async (id: string): Promise<void> => {
  await axiosInstance.delete(`/employees/${id}`);
};