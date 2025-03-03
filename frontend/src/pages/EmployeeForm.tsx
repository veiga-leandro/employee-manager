import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Formik, Form, Field, FieldArray, FormikHelpers, FormikErrors } from 'formik';
import * as Yup from 'yup';
import axios from 'axios';
import {
  Container,
  Typography,
  Box,
  Button,
  TextField,
  Grid,
  Paper,
  IconButton,
  MenuItem,
  FormHelperText,
  FormControl,
  InputLabel,
  Select,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import {
  createEmployee,
  updateEmployee,
  getEmployeeById,
  getAllEmployeesList,
} from '../api/employeeService';
import { Employee, PhoneNumber } from '../models/Employee';
import { RoleType, getRoleTypeLabel } from '../models/RoleType';
import LoadingIndicator from '../components/common/LoadingIndicator';
import MessageFeedback from '../components/common/MessageFeedback';
import { PhoneType } from '../models/PhoneType';
import CPFMaskField from 'components/common/CPFMaskField';

// Interface para erros de validação da API
export interface ApiValidationError {
  title: string;
  status: number;
  message: string;
  errors: {
    [key: string]: string[];
  };
}

// Mapeamento de campos da API para campos do formulário
const apiToFormikFieldMap: Record<string, string> = {
  Cpf: 'cpf',
  FirstName: 'firstName',
  LastName: 'lastName',
  BirthDate: 'birthDate',
  PhoneNumbers: 'phoneNumbers',
  // adicione outros mapeamentos conforme necessário
};

// Validação para garantir que a pessoa não é menor de idade
const calculateAge = (birthday: Date): number => {
  const ageDifMs = Date.now() - new Date(birthday).getTime();
  const ageDate = new Date(ageDifMs);
  return Math.abs(ageDate.getUTCFullYear() - 1970);
};

const employeeSchema = Yup.object().shape({
  firstName: Yup.string().required('Nome é obrigatório'),
  lastName: Yup.string().required('Sobrenome é obrigatório'),
  email: Yup.string().email('Email inválido').required('Email é obrigatório'),
  cpf: Yup.string().required('CPF é obrigatório'),
  birthDate: Yup.date()
    .required('Data de nascimento é obrigatória')
    .test(
      'age',
      'Funcionário deve ter pelo menos 18 anos',
      (value) => !value || calculateAge(value) >= 18,
    ),
  phoneNumbers: Yup.array()
    .of(
      Yup.object().shape({
        number: Yup.string().required('Número de telefone é obrigatório'),
        type: Yup.number().required('Tipo de telefone é obrigatório'),
      }),
    )
    .min(1, 'Pelo menos um número de telefone é obrigatório'),
  role: Yup.number().required('Cargo é obrigatório'),
  managerId: Yup.string().nullable(),
  password: Yup.string().when('id', {
    is: (id: string | undefined) => !id,
    then: (schema) =>
      schema
        .required('Senha é obrigatória')
        .min(8, 'Senha deve ter pelo menos 8 caracteres')
        .matches(/[A-Z]/, 'Senha deve conter pelo menos uma letra maiúscula')
        .matches(/[a-z]/, 'Senha deve conter pelo menos uma letra minúscula')
        .matches(/[0-9]/, 'Senha deve conter pelo menos um número')
        .matches(/[^A-Za-z0-9]/, 'Senha deve conter pelo menos um caractere especial'),
    otherwise: (schema) => schema,
  }),
});

const EmployeeForm: React.FC = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [employee, setEmployee] = useState<Employee | null>(null);
  const [managers, setManagers] = useState<Employee[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>('');

  const isEditMode = !!id;

  useEffect(() => {
    const fetchData = async (): Promise<void> => {
      try {
        setLoading(true);
        // Buscar todos os funcionários para popular o dropdown de gestores
        const allEmployees = await getAllEmployeesList();
        setManagers(allEmployees); // Manter todos os funcionários como potenciais gestores

        // Se estiver editando, buscar os dados do funcionário
        if (isEditMode && id) {
          const data = await getEmployeeById(id);
          setEmployee(data);
        }
      } catch (err) {
        if (err instanceof Error) {
          setError(err.message);
        } else {
          setError('Falha ao carregar dados');
        }
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id, isEditMode]);

  const initialValues: Employee = employee
    ? {
        ...employee,
        // Garantir que temos pelo menos um telefone
        phoneNumbers:
          employee.phoneNumbers && employee.phoneNumbers.length > 0
            ? employee.phoneNumbers.map((phone) => ({
                ...phone,
                // Garantir que type seja um número válido
                type: phone.type >= 1 && phone.type <= 4 ? phone.type : PhoneType.Mobile,
              }))
            : [{ number: '', type: PhoneType.Mobile }],
      }
    : {
        firstName: '',
        lastName: '',
        email: '',
        cpf: '',
        birthDate: '',
        managerId: '',
        role: RoleType.Employee, // Padrão: funcionário comum
        phoneNumbers: [{ number: '', type: PhoneType.Mobile }], // Use o enum
        password: '',
      };

  const handleSubmit = async (
    values: Employee,
    { setSubmitting, setErrors, setStatus }: FormikHelpers<Employee>,
  ): Promise<void> => {
    // Limpar status anterior
    setStatus(undefined);

    try {
      if (isEditMode && id) {
        await updateEmployee(id, values);
      } else {
        await createEmployee(values);
      }
      // Definir status de sucesso (opcional)
      setStatus({ success: 'Funcionário salvo com sucesso!' });
      navigate('/');
    } catch (err) {
      console.error('Erro ao salvar:', err);

      if (axios.isAxiosError(err) && err.response?.data) {
        const apiError = err.response.data as ApiValidationError;

        // Verificar se temos erros de validação do FluentValidation
        if (apiError.errors) {
          // Converter os erros da API para o formato que o Formik espera
          const formikErrors: { [key: string]: string } = {};

          // Mapear os erros para os campos correspondentes no Formik
          Object.entries(apiError.errors).forEach(([field, messages]) => {
            // Usar o mapa para converter nomes de campos ou converter manualmente
            const formikField =
              apiToFormikFieldMap[field] || field.charAt(0).toLowerCase() + field.slice(1);
            formikErrors[formikField] = Array.isArray(messages) ? messages[0] : messages;
          });

          // Define os erros nos campos específicos
          setErrors(formikErrors);
        }

        // Define um status geral para o formulário com a mensagem principal
        setStatus({ error: apiError.message || 'Erro de validação' });
      } else {
        // Erro genérico
        setStatus({ error: 'Falha ao salvar funcionário' });
      }
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) return <LoadingIndicator message="Carregando dados do funcionário..." fullHeight />;

  return (
    <Container maxWidth="md">
      <Paper sx={{ p: 3, my: 4 }}>
        <Typography variant="h4" gutterBottom>
          {isEditMode ? 'Editar Funcionário' : 'Adicionar Novo Funcionário'}
        </Typography>

        {error && <MessageFeedback message={error} severity="error" onClose={() => setError('')} />}

        <Formik
          initialValues={initialValues}
          validationSchema={employeeSchema}
          onSubmit={handleSubmit}
          enableReinitialize
        >
          {({ values, errors, touched, handleChange, setFieldValue, isSubmitting, status }) => (
            <Form>
              {/* Exibir mensagem de erro do status */}
              {status?.error && (
                <MessageFeedback
                  message={status.error}
                  severity="error"
                  onClose={() => setFieldValue('status', undefined)}
                />
              )}

              {/* Exibir mensagem de sucesso do status */}
              {status?.success && (
                <MessageFeedback
                  message={status.success}
                  severity="success"
                  onClose={() => setFieldValue('status', undefined)}
                />
              )}

              <Grid container spacing={2}>
                <Grid item xs={12} sm={6}>
                  <Field
                    as={TextField}
                    name="firstName"
                    label="Nome *"
                    fullWidth
                    error={touched.firstName && Boolean(errors.firstName)}
                    helperText={touched.firstName && errors.firstName}
                  />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <Field
                    as={TextField}
                    name="lastName"
                    label="Sobrenome *"
                    fullWidth
                    error={touched.lastName && Boolean(errors.lastName)}
                    helperText={touched.lastName && errors.lastName}
                  />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <Field
                    as={TextField}
                    name="email"
                    label="Email *"
                    fullWidth
                    error={touched.email && Boolean(errors.email)}
                    helperText={touched.email && errors.email}
                  />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <Field
                    name="cpf"
                    label="CPF *"
                    component={CPFMaskField}
                    fullWidth
                    required
                    error={touched.cpf && Boolean(errors.cpf)}
                    helperText={touched.cpf && errors.cpf}
                  />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <Field
                    as={TextField}
                    name="birthDate"
                    label="Data de Nascimento *"
                    type="date"
                    fullWidth
                    InputLabelProps={{ shrink: true }}
                    error={touched.birthDate && Boolean(errors.birthDate)}
                    helperText={touched.birthDate && errors.birthDate}
                  />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <FormControl fullWidth error={touched.role && Boolean(errors.role)}>
                    <InputLabel id="role-label">Cargo *</InputLabel>
                    <Field as={Select} labelId="role-label" name="role" label="Cargo *">
                      <MenuItem value={RoleType.Admin}>Administrador</MenuItem>
                      <MenuItem value={RoleType.HR}>RH</MenuItem>
                      <MenuItem value={RoleType.Manager}>Gestor</MenuItem>
                      <MenuItem value={RoleType.Employee}>Funcionário</MenuItem>
                    </Field>
                    {touched.role && errors.role && (
                      <FormHelperText>{errors.role as string}</FormHelperText>
                    )}
                  </FormControl>
                </Grid>
                <Grid item xs={12} sm={6}>
                  <FormControl fullWidth error={touched.managerId && Boolean(errors.managerId)}>
                    <InputLabel id="manager-label">Gestor</InputLabel>
                    <Field as={Select} labelId="manager-label" name="managerId" label="Gestor">
                      <MenuItem value="">Nenhum</MenuItem>
                      {managers.map((manager) => (
                        <MenuItem key={manager.id} value={manager.id}>
                          {`${manager.firstName} ${manager.lastName}`}
                        </MenuItem>
                      ))}
                    </Field>
                    {touched.managerId && errors.managerId && (
                      <FormHelperText>{errors.managerId as string}</FormHelperText>
                    )}
                  </FormControl>
                </Grid>
                {!isEditMode && (
                  <Grid item xs={12}>
                    <Field
                      as={TextField}
                      name="password"
                      label="Senha *"
                      type="password"
                      fullWidth
                      error={touched.password && Boolean(errors.password)}
                      helperText={touched.password && errors.password}
                    />
                  </Grid>
                )}
                <Grid item xs={12}>
                  <Typography variant="h6" gutterBottom>
                    Números de Telefone
                  </Typography>
                  <FieldArray name="phoneNumbers">
                    {({ remove, push }) => (
                      <Box>
                        {values.phoneNumbers.map((phone: PhoneNumber, index: number) => (
                          <Grid container spacing={2} key={index} sx={{ mb: 2 }}>
                            <Grid item xs={5}>
                              <Field
                                as={TextField}
                                name={`phoneNumbers.${index}.number`}
                                label="Número de Telefone *"
                                fullWidth
                                error={
                                  touched.phoneNumbers?.[index]?.number &&
                                  Boolean(
                                    (errors.phoneNumbers?.[index] as FormikErrors<PhoneNumber>)
                                      ?.number,
                                  )
                                }
                                helperText={
                                  touched.phoneNumbers?.[index]?.number &&
                                  (errors.phoneNumbers?.[index] as FormikErrors<PhoneNumber>)
                                    ?.number
                                }
                              />
                            </Grid>
                            <Grid item xs={5}>
                              <FormControl fullWidth>
                                <InputLabel>Tipo *</InputLabel>
                                <Field
                                  as={Select}
                                  name={`phoneNumbers.${index}.type`}
                                  label="Tipo *"
                                >
                                  <MenuItem value={PhoneType.Mobile}>Celular</MenuItem>
                                  <MenuItem value={PhoneType.Home}>Residencial</MenuItem>
                                  <MenuItem value={PhoneType.Work}>Trabalho</MenuItem>
                                  <MenuItem value={PhoneType.Other}>Outro</MenuItem>
                                </Field>
                              </FormControl>
                            </Grid>
                            <Grid item xs={2}>
                              <IconButton
                                color="error"
                                onClick={() => remove(index)}
                                disabled={values.phoneNumbers.length === 1}
                                title="Remover telefone"
                              >
                                <DeleteIcon />
                              </IconButton>
                            </Grid>
                          </Grid>
                        ))}
                        <Button
                          startIcon={<AddIcon />}
                          onClick={() => push({ number: '', type: PhoneType.Mobile })}
                          variant="outlined"
                        >
                          Adicionar Telefone
                        </Button>
                      </Box>
                    )}
                  </FieldArray>
                </Grid>
                <Grid item xs={12} sx={{ mt: 3 }}>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                    <Button variant="outlined" onClick={() => navigate('/')}>
                      Cancelar
                    </Button>
                    <Button
                      variant="contained"
                      color="primary"
                      type="submit"
                      disabled={isSubmitting}
                    >
                      {isEditMode ? 'Atualizar' : 'Criar'} Funcionário
                    </Button>
                  </Box>
                </Grid>
              </Grid>
            </Form>
          )}
        </Formik>
      </Paper>
    </Container>
  );
};

export default EmployeeForm;
