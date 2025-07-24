using UnityEngine;

public class KeyPickUpRaycast : MonoBehaviour
{
    public GameObject keyOB;
    public GameObject pickUpText;
    public GameObject player;
    public AudioSource keySound;

    public float interactionDistance = 3f;
    public Camera playerCamera;

    public OpenBoxRaycast boxScript; // ← Reference to box script

    private InventoryItem inventoryItem;

    private bool isPicked = false;
    private bool wasHit = false;

    void Start()
    {
        pickUpText.SetActive(false);
        inventoryItem = GetComponent<InventoryItem>();
    }

    void OnEnable()
    {
        isPicked = false;
    }

    void Update()
    {
        if (isPicked || (boxScript != null && !boxScript.isBoxOpen)) return; // ← Block pickup until box is open

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                wasHit = true;
                pickUpText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    keySound.Play();
                    keyOB.SetActive(false);
                    pickUpText.SetActive(false);
                    isPicked = true;
                    player.GetComponent<Inventory>().AddItem(new Inventory.InventoryItem(inventoryItem.itemName, inventoryItem.itemIcon, inventoryItem.stackSize));
                }
            }
        }
        else
        {
            if (wasHit)
            {
                pickUpText.SetActive(false);
                wasHit = false;
            }
        }
    }
}
