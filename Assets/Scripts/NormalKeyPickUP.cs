using UnityEngine;

public class NormalKeyPickUp : MonoBehaviour
{
    [Header("Key Settings")]
    public GameObject keyOB;
    public AudioSource keySound;
    public float interactionDistance = 3f;

    [Header("UI Elements")]
    public GameObject pickUpText;

    [Header("Player Settings")]
    public GameObject player;
    public Camera playerCamera;

    private bool isPicked = false;
    private bool wasHit = false;

    private InventoryItem inventoryItem;
    private Inventory playerInventory;

    void Start()
    {
        pickUpText.SetActive(false);

        inventoryItem = GetComponent<InventoryItem>();
        playerInventory = player.GetComponent<Inventory>();
    }

    void OnEnable()
    {
        isPicked = false;
    }

    void Update()
    {
        if (isPicked) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance) && hit.collider.gameObject == gameObject)
        {
            wasHit = true;
            pickUpText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpKey();
            }
        }
        else if (wasHit)
        {
            pickUpText.SetActive(false);
            wasHit = false;
        }
    }

    private void PickUpKey()
    {
        keySound.Play();
        keyOB.SetActive(false);
        pickUpText.SetActive(false);
        isPicked = true;

        playerInventory.AddItem(new Inventory.InventoryItem(
            inventoryItem.itemName,
            inventoryItem.itemIcon,
            inventoryItem.stackSize
        ));
    }
}
