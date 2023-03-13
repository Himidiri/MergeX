using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIntractable : MonoBehaviour
{
    private UI_InputWindow inputWindow_;

    public GameObject inputWindow;

    public void Interact()
        {
            ChatBubble.Create(transform.transform, new Vector3(-0.87f, 1.056f, 0f), ChatBubble.IconType.Happy, "Hello World !");

            Debug.Log("Interact!");
            
            inputWindow_.Show();
        }
    public void Start()
    {
        inputWindow_ = inputWindow.GetComponent<UI_InputWindow>();
    }
    }

    
    

    
