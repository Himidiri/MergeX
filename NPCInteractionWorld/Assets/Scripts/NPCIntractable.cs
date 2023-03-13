using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIntractable : MonoBehaviour
{
    private UI_InputWindow inputWindow_; // Reference to the UI_InputWindow script

    public GameObject inputWindow; // Reference to the input window object

    public void Interact()
    {
        ChatBubble.Create(transform.transform, new Vector3(-0.87f, 1.056f, 0f), Quaternion.Euler(0f, 180f, 0f), ChatBubble.IconType.Happy, "Hello"); // Create a chat bubble with the specified properties

        Debug.Log("Interact!"); // Log a message to the console

        inputWindow_.Show(); // Show the input window
    }

    public void Start()
    {
        inputWindow_ = inputWindow.GetComponent<UI_InputWindow>(); // Get the UI_InputWindow component
    }
}
