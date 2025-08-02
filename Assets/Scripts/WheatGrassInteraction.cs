using UnityEngine;

// This script should be attached to the wheat grass GameObject in the world.
// It acts as an IInteractable, but with a specific condition.
public class WheatGrassInteraction : MonoBehaviour, IInteractable
{
    // The details of the item that will be added to the inventory when the grass is cut.
    [SerializeField]
    public Inventory.InventoryItem wheatGrassItemDetails;

    [Tooltip("The exact item name of the tool required to cut the grass (e.g., 'Sickle').")]
    public string requiredToolName = "Sickle";

    [Tooltip("The sound to play when the grass is successfully cut.")]
    public AudioSource cuttingSound;

    // References to the player's components
    private Inventory playerInventory;

    // Internal state to track input and conditions
    private bool eDown = false;
    private bool ePressed = false;
    private bool eUp = false;
    private bool hasRequiredTool = false;

    // IInteractable properties for the interaction system
    public string InteractionPrompt
    {
        get
        {
            if (hasRequiredTool)
            {
                // The prompt when the player has the sickle.
                return "Cut Grass [E]";
            }
            else
            {
                // The prompt when the player does NOT have the sickle.
                return "You need a " + requiredToolName + " to cut this.";
            }
        }
    }
    
    public bool buttonDown => eDown;
    public bool buttonPressed => ePressed;
    public bool buttonUp => eUp;


    void Start()
    {
        // Find the player's inventory at the start of the game.
        playerInventory = FindObjectOfType<Inventory>();
        
        if (playerInventory == null)
        {
            Debug.LogError("Inventory script not found on the player! Please ensure the player has the 'Inventory' component.");
        }
        
        if (wheatGrassItemDetails == null)
        {
            Debug.LogError("Wheat Grass Item Details not set on the " + gameObject.name + "!");
        }
    }

    void Update()
    {
        // Poll for input state every frame
        eDown = Input.GetKeyDown(KeyCode.E);
        ePressed = Input.GetKey(KeyCode.E);
        eUp = Input.GetKeyUp(KeyCode.E);

        // Check if the player has the required tool in their inventory.
        // This check is performed every frame to keep the prompt up to date.
        if (playerInventory != null)
        {
            hasRequiredTool = playerInventory.HasItem(requiredToolName);
        }
    }

    public void Interact(GameObject interactor)
    {
        // Check if the player has the required tool before proceeding.
        if (hasRequiredTool)
        {
            // Try to add the wheat grass item to the inventory.
            if (playerInventory.AddItem(wheatGrassItemDetails))
            {
                // Play the cutting sound if an AudioSource is assigned.
                if (cuttingSound != null)
                {
                    cuttingSound.Play();
                }

                // The item was successfully added, so we can destroy the grass object.
                Destroy(gameObject);
            }
        }
        else
        {
            // The player does not have the tool. You could play a "thud" sound or
            // show a different visual/audio cue here.
            Debug.Log("Cannot cut grass. You need a " + requiredToolName + "!");
        }
    }
}