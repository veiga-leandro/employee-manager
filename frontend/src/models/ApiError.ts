export interface ApiValidationError {
  title: string;
  status: number;
  message: string;
  errors: {
    [key: string]: string[];
  };
}