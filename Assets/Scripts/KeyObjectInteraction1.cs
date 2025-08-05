using UnityEngine;
using TMPro; // Make sure to add this for TextMeshPro

public class KeyObjectInteraction : MonoBehaviour
{
    // A reference to the player's Inventory script
    public Inventory playerInventory;

    // The GameObject for "key object 3"
    public GameObject keyObject3;

    // The TextMeshPro element to display the message
    public TMP_Text uiMessageText;

    // The names of the items required
    public string requiredItemName1 = "Key Object 1";
    public string requiredItemName2 = "Key Object 2"; // --- NEW REQUIRED ITEM ---

    // The message to display when a required item is missing
    public string requirementMessage = "You need Key Object 1 and Key Object 2 to proceed.";

    // The message to display when the player is near the object
    public string interactionPrompt = "Press 'E' to use the key objects";

    private bool isPlayerNearby = false;
    private bool isUnlocked = false;

    void Start()
    {
        // Ensure "key object 3" is invisible at the start of the game.
        if (keyObject3 != null)
        {
            keyObject3.SetActive(false);
        }

        // Find the player's Inventory if it's not assigned in the Inspector.
        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<Inventory>();
        }

        if (playerInventory == null)
        {
            Debug.LogError("Player Inventory script not found in the scene.");
        }

        // Hide the UI message text at the start
        if (uiMessageText != null)
        {
            uiMessageText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Only run the interaction logic if the player is nearby and the object is not unlocked
        if (isPlayerNearby && !isUnlocked)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
    }

    // This method is called when another collider enters this object's trigger collider.
    void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (!isUnlocked && uiMessageText != null)
            {
                uiMessageText.text = interactionPrompt;
                uiMessageText.gameObject.SetActive(true);
            }
        }
    }

    // This method is called when another collider exits this object's trigger collider.
    void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (uiMessageText != null)
            {
                uiMessageText.gameObject.SetActive(false);
            }
        }
    }

    void Interact()
    {
        // Check the inventory for BOTH required items.
        if (playerInventory != null && playerInventory.HasItem(requiredItemName1) && playerInventory.HasItem(requiredItemName2))
        {
            // If both items are found, make "key object 3" visible.
            if (keyObject3 != null)
            {
                keyObject3.SetActive(true);
                isUnlocked = true; // Mark as unlocked to prevent future triggers.

                // Remove both required items from the inventory.
                playerInventory.RemoveItem(requiredItemName1);
                playerInventory.RemoveItem(requiredItemName2); // --- REMOVE SECOND ITEM ---

                Debug.Log("Key Object 3 is now visible. Key Object 1 and Key Object 2 have been removed from inventory!");

                // Hide the UI message after a successful interaction
                if (uiMessageText != null)
                {
                    uiMessageText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // If the items are not found, display a message to the player.
            if (uiMessageText != null)
            {
                uiMessageText.text = requirementMessage;
            }
            Debug.Log(requirementMessage);
        }
    }
}