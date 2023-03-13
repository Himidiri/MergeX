using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIntractable : MonoBehaviour
{
    private UI_InputWindow inputWindow_;

    public GameObject inputWindow;

    public void Interact()
        {
            Debug.Log("Interact!");
            
            inputWindow_.Show();
        }
    public void Start()
    {
        inputWindow_ = inputWindow.GetComponent<UI_InputWindow>();
    }
    }

    
    

    
