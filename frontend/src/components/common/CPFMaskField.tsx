import React from 'react';
import { PatternFormat } from 'react-number-format';
import { Field, FieldProps } from 'formik';
import { TextField } from '@mui/material';

interface CPFMaskFieldProps extends FieldProps {
  label: string;
  fullWidth?: boolean;
  error?: boolean;
  helperText?: React.ReactNode;
}

const CPFMaskField: React.FC<CPFMaskFieldProps> = (props) => {
  const { field, form, label, fullWidth, error, helperText, ...rest } = props;

  return (
    <Field name={field.name}>
      {({ field, form }: FieldProps) => {
        const value = typeof field.value === 'string' ? field.value : '';

        return (
          <PatternFormat
            format="###.###.###-##"
            mask="_"
            customInput={TextField}
            label={label}
            fullWidth={fullWidth}
            error={error}
            helperText={helperText}
            value={value}
            onValueChange={(values) => {
              form.setFieldValue(field.name, values.value);
            }}
            onBlur={field.onBlur}
          />
        );
      }}
    </Field>
  );
};

export default CPFMaskField;
