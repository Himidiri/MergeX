using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    private void Update()
    {
        // Check if the E key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Define the range of interaction and get all the colliders within that range
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);

            // Loop through all the colliders and check if they have the NPCIntractable component
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out NPCIntractable npcInteractable))
                {
                    // Call the Interact() function of the NPCIntractable component
                    npcInteractable.Interact();
                }
            }
        }
    }
}
