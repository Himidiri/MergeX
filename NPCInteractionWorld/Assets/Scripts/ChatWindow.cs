using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ChatWindow : MonoBehaviour
{

    private PlayerInput playerInput; // Reference to the player input component

    public bool isOpened = false; // Flag indicating whether the chat window is currently open or not

    public GameObject player; // Reference to the player object

    private NPCResponse npcResponse; // Reference to the NPCResponse script attached to the NPC

    public TMP_InputField inputField; // Reference to the input field for entering text

    public TMP_Text chatLog; // Reference to the text component for displaying the chat log

    public TMP_Dropdown dropdown; // Reference to the dropdown for selecting predefined questions


    // Start is called before the first frame update
    public void Start()
    {
        playerInput = player.GetComponent<PlayerInput>(); // Get a reference to the PlayerInput component attached to the player

        Hide();  // Hide the chat window initially
    }

    // Show the chat window with the NPC's welcome message and predefined questions
    public void Show(NPCResponse npcResponse)
    {

        isOpened = true;  // Set the flag to indicate that the chat window is open

        gameObject.SetActive(true); // Make the chat window visible

        playerInput.enabled = false;  // Disable player input while the chat window is open

        // Make the cursor visible and unlock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


        dropdown.options.Clear();   // Clear any existing options in the dropdown


        dropdown.options.Add(new TMP_Dropdown.OptionData() { text = "Select a question" });  // Add a default option to the dropdown

        this.npcResponse = npcResponse; // Save a reference to the NPCResponse script attached to the NPC

        // Add all the predefined questions to the dropdown
        for (int i = 0; i < npcResponse.predefineQuestion.Count; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = npcResponse.predefineQuestion[i].question });
        }

        chatLog.text = npcResponse.welcomeMessage;  // Display the NPC's welcome message in the chat log
    }

    // Hide the chat window and reset its state
    public void Hide()
    {

        gameObject.SetActive(false); // Make the chat window invisible

        playerInput.enabled = true; // Enable player input

        // Hide the cursor and lock it to the center of the screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Clear the input field and chat log
        inputField.text = "";
        chatLog.text = "";

        // Set the flag to indicate that the chat window is closed
        isOpened = false;
    }

    // Send the player's input to the NPC to generate a response
    public void Answer()
    {
        // Start the coroutine to generate the NPC's response
        StartCoroutine(npcResponse.GenerateResponse(inputField.text));

        // Clear the input field
        inputField.text = "";
    }

    public void SelectedText()
    {
        // When a dropdown option is selected, get the index of the option and display the corresponding question and answer in the chat log

        Debug.Log(dropdown.value - 1); // Debugging statement to print the index of the selected option in the console

        if (npcResponse != null)
        {
            npcResponse.lastQuery = ""; // Clear the NPC's last query
            string welcome = npcResponse.welcomeMessage; // Get the NPC's welcome message
            string query = string.Format("<color=green>You: </color>{0}", npcResponse.predefineQuestion[dropdown.value - 1].question); // Get the selected question and format it as a query from the player
            string response = string.Format("<color=red>{0}: </color>{1}", npcResponse.name, npcResponse.predefineQuestion[dropdown.value - 1].answer); // Get the answer to the selected question and format it as a response from the NPC
            chatLog.text = string.Format("{0}\n\n{1}\n\n{2}", welcome, query, response); // Update the chat log with the welcome message, query, and response
        }
    }

    void Update()
    {
        // Update the chat log with the player's query and the NPC's response

        if (npcResponse != null)
        {
            string welcome = npcResponse.welcomeMessage; // Get the NPC's welcome message

            if (npcResponse.lastQuery == "")
            {
                return; // If there is no query from the player, do nothing
            }

            string query = string.Format("<color=green>You: </color>{0}", npcResponse.lastQuery); // Get the player's query and format it as a query from the player

            string response;

            if (npcResponse.loading)
            {
                response = string.Format("<color=red>{0}: </color> Thinking... ", npcResponse.name); // If the NPC is still generating a response, display a "thinking" message
            }
            else
            {
                response = string.Format("<color=red>{0}: </color>{1}", npcResponse.name, npcResponse.lastResponse); // If the NPC has generated a response, display the response
            }

            chatLog.text = string.Format("{0}\n\n{1}\n\n{2}", welcome, query, response); // Update the chat log with the welcome message, query, and response
        }
    }
}
