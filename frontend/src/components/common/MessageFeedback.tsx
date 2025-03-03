import React from 'react';
import { Alert, AlertTitle } from '@mui/material';

interface MessageFeedbackProps {
  message: string;
  severity?: 'error' | 'warning' | 'info' | 'success';
  onClose?: () => void;
}

const MessageFeedback: React.FC<MessageFeedbackProps> = ({ 
  message, 
  severity = 'error',
  onClose
}) => {
  if (!message) return null;
  
  const titleMap = {
    error: 'Erro',
    warning: 'Aviso',
    info: 'Informação',
    success: 'Sucesso'
  };
  
  return (
    <Alert 
      severity={severity} 
      onClose={onClose}
      sx={{ mb: 2, width: '100%' }}
    >
      <AlertTitle>{titleMap[severity]}</AlertTitle>
      {message}
    </Alert>
  );
};

export default MessageFeedback;
