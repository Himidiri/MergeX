# Mergex: Interactive NPC Conversations in Open World Video Games

[![Unity Build](https://github.com/Himidiri/MergeX/actions/workflows/build-game.yaml/badge.svg)](https://github.com/Himidiri/MergeX/actions/workflows/build-game.yaml) [![Backend Build](https://github.com/Himidiri/MergeX/actions/workflows/build-backend.yaml/badge.svg)](https://github.com/Himidiri/MergeX/actions/workflows/build-backend.yaml) [![Backend Deploy](https://github.com/Himidiri/MergeX/actions/workflows/deploy-backend.yaml/badge.svg)](https://github.com/Himidiri/MergeX/actions/workflows/deploy-backend.yaml) [![Model Build](https://github.com/Himidiri/MergeX/actions/workflows/build-ai.yaml/badge.svg)](https://github.com/Himidiri/MergeX/actions/workflows/build-ai.yaml)

The project aims to enhance the player experience in modern open-world video games by enabling open and unbound conversations with non-playable characters (NPCs) using natural language processing (NLP). Developers can program each NPC with a biography, which enables the conversation to follow the personality traits of the NPC. This project is developed using Python and Unity.

## Requirements
To run the project, you will need to have the following tools installed:
- Docker
- Git
- Git LFS
- Unity 2021.3 or later

## Installation

To install and run the project locally, follow these steps:

1. Clone the repository using Git and Git LFS by running the following command in a terminal or command prompt:
    ```sh
    git clone https://github.com/Himidiri/MergeX.git
    ```
2. Open a terminal or command prompt and run the following command to download and run the backend container:
    ```sh
    docker run -p 5000:5000 ghcr.io/himidiri/mergex/backend:latest
    ```
3. Open another terminal or command prompt and run the following command to download and run the AI model container:
    ```sh
    docker run -p 5001:5000 ghcr.io/himidiri/mergex/mergex-ai:latest
    ```
4. Open the Unity project in Unity 2021.3 or later.
5. Navigate to the `ChatBubble.cs` script in the project hierarchy and update the `apiEndpoint` variable to the IP address and port of the backend API endpoint.
6. Run the Unity project in the Unity Editor or build and run the game.
7. Interact with the NPCs in the game to engage in open and unbound conversations using the NLP-powered AI.

## Credits
This project was developed by CodeRaiders as part of Software Development Group Project at University of Westminster.

## License
This project is licensed under the Common Development and Distribution License (CDDL-1.0).