using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractionWorldAssets : MonoBehaviour
{
    private static NPCInteractionWorldAssets _i;
    public static NPCInteractionWorldAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("NPCInteractionWorldAssets")) as GameObject).GetComponent<NPCInteractionWorldAssets>();
            return _i;
        }
    }

    public Transform ChatBubble;
}

