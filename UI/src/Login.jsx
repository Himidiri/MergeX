import React, { useState } from 'react';

import {
  Box,
  Button,
  Checkbox,
  Container,
  Divider,
  FormControl,
  FormLabel,
  Heading,
  HStack,
  Input,
  Stack,
  Text,
} from '@chakra-ui/react';
import { PasswordField } from './PasswordField';
import { Logo } from './Logo';
import { useToast } from '@chakra-ui/react';

export const Login = ({ setLoginView, setLoggedIn }) => {
  const toast = useToast();
  const [loading, setLoading] = useState(false);

  const [formValue, setFormValue] = useState({
    email: '',
    password: '',
  });
  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormValue((prevState) => {
      return {
        ...prevState,
        [name]: value,
      };
    });
  };

  const handleLogin = async () => {
    setLoading(true);
    try {
      const response = await fetch(
        'https://backend-ghumo23ctq-as.a.run.app/login/',
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            email: formValue.email,
            password: formValue.password,
          }),
        }
      );

      const data = await response.json();
      toast({
        description: data.message,
        status: response.ok ? 'success' : 'error',
        duration: 4000,
        isClosable: true,
      });

      if (response.ok) {
        await new Promise((resolve) => setTimeout(resolve, 1000));
        setLoggedIn(true);
      }
    } catch (err) {
      toast({
        description: err.message,
        status: 'error',
        duration: 4000,
        isClosable: true,
      });
    }
    setLoading(false);
  };

  return (
    <>
      <Stack spacing="6">
        <Logo />
        <Stack spacing={{ base: '2', md: '3' }} textAlign="center">
          <Heading size={{ base: 'xs', md: 'sm' }}>
            Log in to your account
          </Heading>
          <HStack spacing="1" justify="center">
            <Text color="muted">Don't have an account?</Text>
            <Button
              variant="link"
              colorScheme="blue"
              on
              onClick={() => setLoginView(false)}
            >
              Sign up
            </Button>
          </HStack>
        </Stack>
      </Stack>
      <Box
        py={{ base: '0', sm: '8' }}
        px={{ base: '4', sm: '10' }}
        bg={{ base: 'transparent', sm: 'bg-surface' }}
        boxShadow={{ base: 'none', sm: 'md' }}
        borderRadius={{ base: 'none', sm: 'xl' }}
      >
        <Stack spacing="6">
          <Stack spacing="5">
            <FormControl>
              <FormLabel htmlFor="email">Email</FormLabel>
              <Input name="email" type="email" onChange={handleChange} />
            </FormControl>
            <PasswordField
              name="password"
              text="Password"
              onChange={handleChange}
            />
          </Stack>
          <Button
            colorScheme="blue"
            onClick={() => handleLogin()}
            isLoading={loading}
            loadingText="Authenticating"
          >
            Login
          </Button>
        </Stack>
      </Box>
    </>
  );
};
