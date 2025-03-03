import React, { useState, useEffect } from 'react';
import {
  Container,
  Typography,
  Box,
  Button,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  TablePagination,
} from '@mui/material';
import { Link } from 'react-router-dom';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { getAllEmployees, deleteEmployee } from '../api/employeeService';
import { Employee } from '../models/Employee';
import axios from 'axios';
import { PaginatedResponse } from 'models/Pagination';
import MessageFeedback from 'components/common/MessageFeedback';
import LoadingIndicator from 'components/common/LoadingIndicator';
import { getRoleTypeLabel } from 'models/RoleType';

const EmployeeList: React.FC = () => {
  const [employeesData, setEmployeesData] = useState<PaginatedResponse<Employee>>({
    items: [],
    currentPage: 1,
    pageSize: 10,
    totalItems: 0,
    totalPages: 0,
  });
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState<number>(10);
  const [loading, setLoading] = useState<boolean>(true);
  const [isDeleting, setIsDeleting] = useState<boolean>(false);
  const [error, setError] = useState<string>('');
  const [successMessage, setSuccessMessage] = useState<string>('');

  const fetchEmployees = async (
    currentPage: number = 1,
    currentPageSize: number = 10,
  ): Promise<void> => {
    try {
      setLoading(true);
      const data = await getAllEmployees(currentPage, currentPageSize);
      setEmployeesData(data);
      setError('');
    } catch (err) {
      if (err instanceof Error) {
        setError(err.message);
      } else {
        setError('Falha ao carregar lista de funcionários');
      }
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchEmployees(page, pageSize);
  }, [page, pageSize]);

  const handlePageChange = (
    _: React.MouseEvent<HTMLButtonElement> | null,
    newPage: number,
  ): void => {
    setPage(newPage + 1);
  };

  const handleRowsPerPageChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const newPageSize = parseInt(event.target.value, 10);
    setPageSize(newPageSize);
    setPage(1);
  };

  const handleDelete = async (id: string): Promise<void> => {
    if (window.confirm('Tem certeza que deseja excluir este funcionário?')) {
      try {
        setIsDeleting(true);
        await deleteEmployee(id);

        setEmployeesData((prevData) => ({
          ...prevData,
          items: prevData.items.filter((emp) => emp.id !== id),
          totalItems: prevData.totalItems - 1,
        }));

        setSuccessMessage('Funcionário excluído com sucesso');
        setTimeout(() => setSuccessMessage(''), 3000); // Limpa após 3 segundos
      } catch (err) {
        console.error('Erro ao excluir funcionário:', err);

        if (axios.isAxiosError(err)) {
          if (err.response?.status === 403) {
            setError('Você não tem permissão para excluir este funcionário');
          } else if (err.response?.status === 404) {
            setError('Funcionário não encontrado - pode já ter sido excluído');
            fetchEmployees(employeesData.currentPage);
          } else if (err.response?.data?.message) {
            setError(err.response.data.message);
          } else {
            setError('Falha ao excluir o funcionário');
          }
        } else if (err instanceof Error) {
          setError(err.message);
        } else {
          setError('Ocorreu um erro inesperado');
        }
      } finally {
        setIsDeleting(false);
      }
    }
  };

  if (loading) {
    return <LoadingIndicator message="Carregando lista de funcionários..." fullHeight />;
  }

  return (
    <Container maxWidth="lg">
      <Box sx={{ my: 4 }}>
        {isDeleting && <LoadingIndicator type="overlay" message="Excluindo funcionário..." />}

        <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
          <Typography variant="h4">Funcionários</Typography>
          <Button variant="contained" color="primary" component={Link} to="/employees/new">
            Adicionar Funcionário
          </Button>
        </Box>

        {error && <MessageFeedback message={error} severity="error" onClose={() => setError('')} />}
        {successMessage && (
          <MessageFeedback
            message={successMessage}
            severity="success"
            onClose={() => setSuccessMessage('')}
          />
        )}

        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Nome Completo</TableCell>
                <TableCell>E-mail</TableCell>
                <TableCell>CPF</TableCell>
                <TableCell>Data de Nascimento</TableCell>
                <TableCell>Cargo</TableCell>
                <TableCell>Ações</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {employeesData.items.length > 0 ? (
                employeesData.items.map((employee) => (
                  <TableRow key={employee.id}>
                    <TableCell>{employee.fullName}</TableCell>
                    <TableCell>{employee.email}</TableCell>
                    <TableCell>{employee.cpf}</TableCell>
                    <TableCell>
                      {new Date(employee.birthDate).toLocaleDateString('pt-BR')}
                    </TableCell>
                    <TableCell>
                      <Box
                        component="span"
                        sx={{
                          display: 'inline-block',
                          px: 1,
                          py: 0.5,
                          borderRadius: 1,
                          fontSize: '0.875rem',
                          fontWeight: 'medium',
                          backgroundColor: (() => {
                            switch (employee.role) {
                              case 1:
                                return 'rgba(255, 86, 48, 0.1)'; // Vermelho claro para Admin
                              case 2:
                                return 'rgba(24, 144, 255, 0.1)'; // Azul claro para RH
                              case 3:
                                return 'rgba(82, 196, 26, 0.1)'; // Verde claro para Gestor
                              default:
                                return 'rgba(140, 140, 140, 0.1)'; // Cinza claro para Funcionário
                            }
                          })(),
                          color: (() => {
                            switch (employee.role) {
                              case 1:
                                return 'rgb(201, 52, 0)'; // Vermelho para Admin
                              case 2:
                                return 'rgb(11, 93, 172)'; // Azul para RH
                              case 3:
                                return 'rgb(39, 120, 0)'; // Verde para Gestor
                              default:
                                return 'rgb(66, 66, 66)'; // Cinza para Funcionário
                            }
                          })(),
                        }}
                      >
                        {getRoleTypeLabel(employee.role)}
                      </Box>
                    </TableCell>
                    <TableCell>
                      <IconButton
                        component={Link}
                        to={`/employees/edit/${employee.id}`}
                        color="primary"
                        title="Editar"
                      >
                        <EditIcon />
                      </IconButton>
                      <IconButton
                        onClick={() => employee.id && handleDelete(employee.id)}
                        color="error"
                        title="Excluir"
                      >
                        <DeleteIcon />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={5} align="center">
                    Não há funcionários cadastrados
                  </TableCell>
                </TableRow>
              )}
            </TableBody>

            {employeesData.totalPages > 1 && (
              <TablePagination
                component="div"
                count={employeesData.totalItems}
                page={employeesData.currentPage - 1}
                onPageChange={handlePageChange}
                rowsPerPage={employeesData.pageSize}
                rowsPerPageOptions={[5, 10, 25, 50]}
                onRowsPerPageChange={handleRowsPerPageChange}
                labelRowsPerPage="Itens por página:"
                labelDisplayedRows={({ from, to, count }) =>
                  `${from}-${to} de ${count !== -1 ? count : `mais de ${to}`}`
                }
              />
            )}
          </Table>
        </TableContainer>
      </Box>
    </Container>
  );
};

export default EmployeeList;
