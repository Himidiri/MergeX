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
        ChatBubble.Create(transform.transform, new Vector3(6f, 1f, 0f), Quaternion.Euler(0f, -57.659f, 0f) , ChatBubble.IconType.Happy, inputField.text);
    }

    public void cancelInputs()
    {
        inputWindow_.Hide();
    }
}
