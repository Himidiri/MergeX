using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_InputWindow : MonoBehaviour
{
    private PlayerInput playerInput;

    public GameObject player;

    public void Start()
    {
        playerInput = player.GetComponent<PlayerInput>();
        Debug.Log(playerInput);
        Debug.Log(player);
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        playerInput.enabled=false;
        Cursor.visible=true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        playerInput.enabled=true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
