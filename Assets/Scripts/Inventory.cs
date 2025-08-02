using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 5;
    [HideInInspector]
    public List<InventorySlot> inventorySlots;
    public Sprite defaultItemIcon;

    private DisplayInventory displayInventory;

    [System.Serializable]
    public class InventorySlot
    {
        public InventoryItem item;
        public int currentStack = 0;

        public InventorySlot(InventoryItem item, int stack)
        {
            this.item = item;
            this.currentStack = stack;
        }
    }

    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;
        public Sprite itemIcon;
        public int stackSize = 1;

        public InventoryItem(string itemName, Sprite itemIcon, int stackSize)
        {
            this.itemName = itemName;
            this.itemIcon = itemIcon;
            this.stackSize = stackSize;
        }

        public virtual void Use()
        {
        }
    }

    void Awake()
    {
        InitializeInventorySlots();
    }

    private void Start()
    {
        displayInventory = GetComponent<DisplayInventory>();
    }

    private void InitializeInventorySlots()
    {
        inventorySlots = new List<InventorySlot>(new InventorySlot[inventorySize]);
    }

    public bool AddItem(InventoryItem itemToAdd)
    {
        bool wasInventoryEmpty = IsInventoryEmpty();

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i] != null && inventorySlots[i].item != null && inventorySlots[i].item.itemName == itemToAdd.itemName && inventorySlots[i].currentStack < inventorySlots[i].item.stackSize)
            {
                inventorySlots[i].currentStack++;
                if (displayInventory != null)
                {
                    displayInventory.UpdateInventoryDisplay();
                }
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i] == null || inventorySlots[i].item == null)
            {
                if (itemToAdd.itemIcon == null)
                {
                    itemToAdd.itemIcon = defaultItemIcon;
                }
                inventorySlots[i] = new InventorySlot(itemToAdd, 1);
                if (displayInventory != null)
                {
                    displayInventory.UpdateInventoryDisplay();
                    if(wasInventoryEmpty)
                    {
                        displayInventory.SelectedIndex = 0;
                        displayInventory.Select();
                    }
                }
                return true;
            }
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    public bool RemoveItem(string itemName)
    {
        if (displayInventory != null)
        {
            displayInventory.Unselect();
        }
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i] != null && inventorySlots[i].item != null && inventorySlots[i].item.itemName == itemName && inventorySlots[i].currentStack > 0)
            {
                inventorySlots[i].currentStack--;
                if (inventorySlots[i].currentStack <= 0)
                {
                    inventorySlots[i] = null;
                    if (displayInventory != null)
                    {
                        displayInventory.SelectedIndex = i;
                    }
                }
                if (displayInventory != null)
                {
                    displayInventory.UpdateInventoryDisplay();
                    if(IsInventoryEmpty())
                        displayInventory.SelectedIndex = -1;
                }
                return true;
            }
        }
        Debug.Log(itemName + " not found in inventory.");
        return false;
    }

    public bool HasItem(string itemName)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot != null && slot.item != null && slot.item.itemName == itemName && slot.currentStack > 0)
            {
                return true;
            }
        }
        return false;
    }

    public int GetItemCount(string itemName)
    {
        int count = 0;
        foreach (var slot in inventorySlots)
        {
            if (slot != null && slot.item != null && slot.item.itemName == itemName)
            {
                count += slot.currentStack;
            }
        }
        return count;
    }

    public void UseItem(string itemName)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i] != null && inventorySlots[i].item != null && inventorySlots[i].item.itemName == itemName && inventorySlots[i].currentStack > 0)
            {
                inventorySlots[i].item.Use();
                inventorySlots[i].currentStack--;
                if (inventorySlots[i].currentStack <= 0)
                {
                    inventorySlots[i] = null;
                }
                return;
            }
        }
        Debug.Log(itemName + " not found in inventory to use.");
    }

    public bool IsInventoryEmpty()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot != null && slot.item != null && slot.currentStack > 0)
            {
                return false;
            }
        }
        return true;
    }

    public List<InventorySlot> GetInventory()
    {
        return inventorySlots;
    }
}