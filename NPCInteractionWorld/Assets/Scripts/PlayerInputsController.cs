using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInputsController : MonoBehaviour
{
    private ChatWindow chatWindow_; // Reference to the ChatWindow script

    public GameObject chatWindow; // Reference to the chat window object

    public TMP_InputField inputField; // Reference to the TMP_InputField component
    public TMP_Text npcTextPrefab; // Reference to the NPC's text prefab
    public TMP_Text playerTextPrefab; // Reference to the player's text prefab
    public TMP_Text defaultNpcResponseText; // Reference to the default NPC response text
    public Transform content; // Reference to the content transform of the scroll view
    private TMP_Text lastPlayerMessage;
    private TMP_Text lastNpcMessage;


    private string npcName = "Merlin";
    private string npcResponse;

    public void Start()
    {
        chatWindow_ = chatWindow.GetComponent<ChatWindow>();

        // Set up the NPC's default response
        npcResponse = string.Format("Hello, I'm {0}. {1}", npcName, "Ask me anything about magic and I will do my best to help.");
        defaultNpcResponseText.text = npcResponse;
    }

    public void userInputs()
    {
        // Instantiate the player's message prefab
        GameObject playerMessageObject = Instantiate(playerTextPrefab.gameObject, content);
        TMP_Text playerMessage = playerMessageObject.GetComponent<TMP_Text>();

        // Set the player's message text
        string playerMessageText = string.Format("<color=green>You: </color>{0}", inputField.text);
        playerMessage.text = playerMessageText;

        // Set the player message's position to be below the last player message
        if (lastPlayerMessage != null)
        {
            playerMessageObject.transform.SetSiblingIndex(lastPlayerMessage.transform.GetSiblingIndex() + 1);
            lastPlayerMessage.text = "";
        }

        // Store the last player message
        lastPlayerMessage = playerMessage;

        // Instantiate the NPC's message prefab
        GameObject npcMessageObject = Instantiate(npcTextPrefab.gameObject, content);
        TMP_Text npcMessage = npcMessageObject.GetComponent<TMP_Text>();

        // Set the NPC's message text
        string npcMessageText = string.Format("<color=red>{0}: </color>{1}", npcName, GenerateNPCResponse());
        npcMessage.text = npcMessageText;

        // Set the NPC message's position to be below the last NPC message
        if (lastNpcMessage != null)
        {
            npcMessageObject.transform.SetSiblingIndex(lastNpcMessage.transform.GetSiblingIndex() + 1);
            lastNpcMessage.text = "";

        }

        // Store the last NPC message
        lastNpcMessage = npcMessage;

        // Clear the input field
        inputField.text = "";
    }

    public void cancelInputs()
    {
        chatWindow_.Hide(); // Hide the input window

        // Clear the Player and NPC text Messages
        lastPlayerMessage.text = "";
        lastNpcMessage.text = "";
    }

    private string GenerateNPCResponse()
    {
        // Return a random NPC response
        string[] responses = { "I'm not sure, could you be more specific?", "That's an interesting question, let me think...", "I'm sorry, I don't know the answer to that.", "I think I know what you're asking, but I'm not sure." };
        return responses[Random.Range(0, responses.Length)];
    }
}