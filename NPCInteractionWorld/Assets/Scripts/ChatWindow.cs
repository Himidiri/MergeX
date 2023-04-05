using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ChatWindow : MonoBehaviour
{
    private PlayerInput playerInput;
    // public ScrollRect scrollRect;
    public bool isOpened = false;

    public GameObject player;
    private NPCResponse npcResponse;

    // public GameObject chatObjectPrefab;
    // public TMP_Text playerTextPrefab;
    // public TMP_Text npcTextPrefab;
    // public Transform content;
    // public GameObject lastChatObject;

    
    public TMP_InputField inputField; // Reference to the TMP_InputField component
    public TMP_Text chatLog;

    public void Start()
    {
        // Get the PlayerInput component of the player game object
        playerInput = player.GetComponent<PlayerInput>();

        // Hide the input window when the script starts
        Hide();
    }

    // Show the input window and disable the player input
    public void Show(NPCResponse npcResponse)
    {
        isOpened = true;
        gameObject.SetActive(true);
        playerInput.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        this.npcResponse = npcResponse;
    }

    // Hide the input window and enable the player input
    public void Hide()
    {
        gameObject.SetActive(false);
        playerInput.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inputField.text = "";
        chatLog.text = "";
        isOpened = false;
    }

    public void Answer(){
        StartCoroutine(npcResponse.GenerateResponse(inputField.text));
        inputField.text = "";
    }

    void Update()
    {
        if(npcResponse != null){
            string welcome = npcResponse.welcomeMessage;

            if(npcResponse.lastQuery == ""){
                chatLog.text = welcome;
                return;
            }

            string query = string.Format("<color=green>You: </color>{0}", npcResponse.lastQuery);
            string response;

            if(npcResponse.loading){
                response = string.Format("<color=red>{0}: </color> Thinking... ", npcResponse.name);
            }else{
                response = string.Format("<color=red>{0}: </color>{1}", npcResponse.name, npcResponse.lastResponse);
            }

            chatLog.text = string.Format("{0}\n\n{1}\n\n{2}", welcome, query, response);
        }
    }
}