using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    // Static method for creating a chat bubble with the specified properties
    public static void Create(Transform parent, Vector3 localPosition, Quaternion rotation, IconType iconType, string text)
    {
        // Instantiate the chat bubble prefab
        Transform chatBubbleTransform = Instantiate(NPCInteractionWorldAssets.i.ChatBubble, parent);

        // Set the local position of the chat bubble
        chatBubbleTransform.localPosition = localPosition;

        // Set the rotation of the chat bubble
        chatBubbleTransform.rotation = rotation;

        // Set the scale of the chat bubble
        chatBubbleTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        // Call the Setup method on the ChatBubble component to configure it
        chatBubbleTransform.GetComponent<ChatBubble>().Setup(iconType, text);

        // Optionally destroy the chat bubble after a set amount of time
        Destroy(chatBubbleTransform.gameObject, 6f);
    }

    // Enumeration of available icon types for the chat bubble
    public enum IconType
    {
        Happy,
        Neutral,
        Angry,
    }

    // Serialized fields for the sprite icons to use for each icon type
    [SerializeField] private Sprite happyIconSprite;
    [SerializeField] private Sprite neutralIconSprite;
    [SerializeField] private Sprite angryIconSprite;

    // References to the child components of the chat bubble object
    private SpriteRenderer backgroundSpriteRenderer;
    private SpriteRenderer iconSpriteRenderer;
    private TextMeshPro textMeshPro;

    // Method called when the chat bubble object is instantiated
    private void Awake()
    {
        // Find the background sprite renderer child component and store it as a reference
        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();

        // Find the icon sprite renderer child component and store it as a reference
        iconSpriteRenderer = transform.Find("Icon").GetComponent<SpriteRenderer>();

        // Find the TextMeshPro child component and store it as a reference
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    // Method called to set up the chat bubble with the specified icon and text
    private void Setup(IconType iconType, string text)
    {
        // Set the text of the TextMeshPro component to the specified text
        textMeshPro.SetText(text);

        // Force an update to the TextMeshPro mesh
        textMeshPro.ForceMeshUpdate();

        // Get the size of the rendered text with no extra padding
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        // Add padding to the text size to create the background sprite size
        Vector2 padding = new Vector2(9f, 3f);
        backgroundSpriteRenderer.size = textSize + padding;

        // Set the position of the background sprite to be centered horizontally and offset to the left
        Vector3 offset = new Vector3(-5f, 0f);
        backgroundSpriteRenderer.transform.localPosition = new Vector3(backgroundSpriteRenderer.size.x / 2f, 0f) + offset;

        // Get the appropriate icon sprite for the specified icon type and set it on the icon sprite renderer
        iconSpriteRenderer.sprite = GetIconSprite(iconType);
    }

    // Method to retrieve the appropriate icon sprite for the specified icon type
    private Sprite GetIconSprite(IconType iconType)
    {
        switch (iconType)
        {
            default:
            case IconType.Happy: return happyIconSprite;
            case IconType.Neutral: return neutralIconSprite;
            case IconType.Angry: return angryIconSprite;
        }
    }
}
