using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera _mainCam; // Reference to the main camera

    private void Start()
    {
        _mainCam = Camera.main; // Get the main camera component
    }

    private void LateUpdate()
    {
        var rotation = _mainCam.transform.rotation; // Get the camera rotation
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up); // Rotate the object to face the camera
    }
}
