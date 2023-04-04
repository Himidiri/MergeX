import React, { useState } from 'react';

import {
  Button,
  CardFooter,
  Heading,
  Card,
  CardHeader,
  CardBody,
  Text,
} from '@chakra-ui/react';

export const Welcome = ({ setLoggedIn }) => {
  return (
    <>
      <Card align="center">
        <CardHeader>
          <Heading size="md">Welcome Gamer!</Heading>
        </CardHeader>
        <CardBody>
          <Text align="center">Your account has been authorized.</Text>
          <Text align="center">
            Feel free to open the game and continue to play
          </Text>
        </CardBody>
        <CardFooter>
          <Button
            onClick={() => {
              setLoggedIn(false);
            }}
            colorScheme="red"
          >
            Logout
          </Button>
        </CardFooter>
      </Card>
    </>
  );
};
