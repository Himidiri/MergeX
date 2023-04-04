import React, { useState } from 'react';
import { Login } from './Login';
import { SignUp } from './SignUp';
import { Welcome } from './Welcome';

import { Container, Stack } from '@chakra-ui/react';
import { ChakraProvider } from '@chakra-ui/react';

function App() {
  const [loginView, setLoginView] = useState(true);
  const [loggedIn, setLoggedIn] = useState(false);

  let center = loginView ? (
    <Login setLoginView={setLoginView} setLoggedIn={setLoggedIn} />
  ) : (
    <SignUp setLoginView={setLoginView} setLoggedIn={setLoggedIn} />
  );

  if (loggedIn) {
    center = <Welcome setLoggedIn={setLoggedIn} />;
  }

  return (
    <ChakraProvider>
      <Container
        maxW="lg"
        py={{ base: '12', md: '24' }}
        px={{ base: '0', sm: '8' }}
      >
        <Stack spacing="8">{center}</Stack>
      </Container>
    </ChakraProvider>
  );
}

export default App;
