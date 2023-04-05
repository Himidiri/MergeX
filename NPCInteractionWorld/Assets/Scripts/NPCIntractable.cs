using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIntractable : MonoBehaviour
{
    private ChatWindow chatWindow_; // Reference to the ChatWindow script

    public GameObject chatWindow; // Reference to the chat window object

    public void Interact()
    {
        Debug.Log("Interact!"); // Log a message to the console

        chatWindow_.Show(); // Show the chat window
    }

    public void Start()
    {
        chatWindow_ = chatWindow.GetComponent<ChatWindow>(); // Get the ChatWindow component
    }
}
