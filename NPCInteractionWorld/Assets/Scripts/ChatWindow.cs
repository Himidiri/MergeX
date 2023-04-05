using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChatWindow : MonoBehaviour
{
    private PlayerInput playerInput;

    public GameObject player;

    public void Start()
    {
        // Get the PlayerInput component of the player game object
        playerInput = player.GetComponent<PlayerInput>();

        // Hide the input window when the script starts
        Hide();
    }

    // Show the input window and disable the player input
    public void Show()
    {
        gameObject.SetActive(true);
        playerInput.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Hide the input window and enable the player input
    public void Hide()
    {
        gameObject.SetActive(false);
        playerInput.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
