using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LoginAccount : MonoBehaviour
{
    public GameObject Login;
    
     public TMP_InputField username; // Reference to the TMP_InputField component
     public TMP_InputField password;

    public void Start()
    {
        // Hide the Login window when the script starts
        HideLoginWindow();
    }

    // Show the Login window 
    public void ShowLoginWindow()
    {
        gameObject.SetActive(true);
    }

    // Hide the Login window 
    public void HideLoginWindow()
    {
        gameObject.SetActive(false);
    }
}
