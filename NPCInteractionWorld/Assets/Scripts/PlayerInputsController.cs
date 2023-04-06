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
    }

    public void cancelInputs()
    {
        chatWindow_.Hide(); // Hide the input window
    }

    private string GenerateNPCResponse()
    {
        // Return a random NPC response
        string[] responses = { "I'm not sure, could you be more specific?", "That's an interesting question, let me think...", "I'm sorry, I don't know the answer to that.", "I think I know what you're asking, but I'm not sure." };
        return responses[Random.Range(0, responses.Length)];
    }
}