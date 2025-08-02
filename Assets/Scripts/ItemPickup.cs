using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    // This allows you to set the item's properties in the Inspector
    [SerializeField]
    public Inventory.InventoryItem itemDetails;

    private bool eUp = false;
    private bool ePressed = false;
    private bool eDown = false;

    public string InteractionPrompt => itemDetails.itemName;
    public bool buttonDown => eDown;
    public bool buttonPressed => ePressed;
    public bool buttonUp => eUp;

    void Awake()
    {
        if (itemDetails == null)
        {
            Debug.LogError("Item details not set for " + gameObject.name, this);
        }
    }

    void Update()
    {
        eDown = Input.GetKeyDown(KeyCode.E);
        ePressed = Input.GetKey(KeyCode.E);
        eUp = Input.GetKeyUp(KeyCode.E);
    }

    public void Interact(GameObject interactor)
    {
        if (interactor.GetComponent<Inventory>().AddItem(itemDetails))
        {
            Destroy(gameObject);
        }
    }
}