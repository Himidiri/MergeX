using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInputsController : MonoBehaviour
{
    private UI_InputWindow inputWindow_;

    public GameObject inputWindow;

    public TMP_InputField inputField;

    public void Start()
    {
        inputWindow_ = inputWindow.GetComponent<UI_InputWindow>();
    }

    public void userInputs()
    {
        ChatBubble.Create(transform.transform, new Vector3(-0.6f, 2.01f, 0.08f), Quaternion.Euler(0f, 180f, 0f) , ChatBubble.IconType.Happy, inputField.text);
    }

    public void cancelInputs()
    {
        inputWindow_.Hide();
    }
}
