using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractionWorldAssets : MonoBehaviour
{
    private static NPCInteractionWorldAssets _i; // Singleton instance
    public static NPCInteractionWorldAssets i // Public getter for the singleton instance
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("NPCInteractionWorldAssets")) as GameObject).GetComponent<NPCInteractionWorldAssets>(); // Instantiate the NPCInteractionWorldAssets prefab if it hasn't been instantiated yet
            return _i;
        }
    }

    public Transform ChatBubble; // Reference to the ChatBubble prefab
}
