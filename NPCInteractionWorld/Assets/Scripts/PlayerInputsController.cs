using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInputsController : MonoBehaviour
{
    private UI_InputWindow inputWindow_; // Reference to the UI_InputWindow script

    public GameObject inputWindow; // Reference to the input window object

    public TMP_InputField inputField; // Reference to the TMP_InputField component

    public void Start()
    {
        inputWindow_ = inputWindow.GetComponent<UI_InputWindow>(); // Get the UI_InputWindow component
    }

    public void userInputs()
    {
        ChatBubble.Create(transform.transform, new Vector3(-0.6f, 2.01f, 0.08f), Quaternion.Euler(0f, 180f, 0f), ChatBubble.IconType.Happy, inputField.text); // Create a chat bubble with the specified properties
    }

    public void cancelInputs()
    {
        inputWindow_.Hide(); // Hide the input window
    }
}
