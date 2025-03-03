import React from 'react';
import { Box, CircularProgress, Typography, LinearProgress } from '@mui/material';

type LoadingType = 'circular' | 'linear' | 'overlay';

interface LoadingIndicatorProps {
  message?: string;
  type?: LoadingType;
  fullHeight?: boolean;
}

const LoadingIndicator: React.FC<LoadingIndicatorProps> = ({ 
  message = 'Carregando dados...', 
  type = 'circular',
  fullHeight = false 
}) => {
  
  if (type === 'overlay') {
    return (
      <Box sx={{ 
        position: 'fixed',
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        backgroundColor: 'rgba(255, 255, 255, 0.8)',
        zIndex: 9999,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center'
      }}>
        <CircularProgress size={60} />
        <Typography variant="h6" sx={{ mt: 2 }}>
          {message}
        </Typography>
      </Box>
    );
  }
  
  return (
    <Box sx={{ 
      display: 'flex', 
      flexDirection: 'column',
      justifyContent: 'center', 
      alignItems: 'center', 
      height: fullHeight ? '50vh' : 'auto',
      width: '100%',
      my: 4
    }}>
      {type === 'circular' ? (
        <CircularProgress size={50} />
      ) : (
        <Box sx={{ width: '50%', maxWidth: 400 }}>
          <LinearProgress />
        </Box>
      )}
      <Typography variant="body1" sx={{ mt: 2 }}>
        {message}
      </Typography>
    </Box>
  );
};

export default LoadingIndicator;
