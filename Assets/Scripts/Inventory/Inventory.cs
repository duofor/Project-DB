using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    
    public static Inventory instance;

    private List<InventorySlot> inventorySlots; 

    void Awake() {
        instance = this;

        inventorySlots = getInventorySlots();
        
        foreach (var slot in inventorySlots) {
            GameObject itemInsideSlot = new GameObject();
            itemInsideSlot.transform.parent = slot.transform;
            //<<<------- needs logic 
        }
    }

    /** inventory slots need to be named Inventory<lalalal>
    this returns a empty inventory list now......**/
    private List<InventorySlot> getInventorySlots() {
        int invSlotsCount = transform.childCount;
        var inventorySlotObjects = new List<InventorySlot>();

        for (int childNumber = 0; childNumber < invSlotsCount; childNumber++) {
            InventorySlot slot = transform.GetChild(childNumber).GetComponent<InventorySlot>();
            if ( slot.name.StartsWith("Inventory") ) {
                inventorySlotObjects.Add( slot );
            }
        }

        return inventorySlotObjects;
    }

    public List<InventorySlot> getInventory() {
        return inventorySlots;
    }

    public bool addItemToInventory(Draggable item) {
        Debug.Log("cur");
        foreach ( InventorySlot slot in inventorySlots ) {
            if ( slot.getItemInSlot() != null )
                continue;

            Debug.Log("adding " + item.transform.name + " to " + slot.transform.name);
            slot.setItem( item );
            item.currentItemSlot = slot;
            return true;
        }
        return false;
    }

    public InventorySlot getNextFreeSlot() {
        foreach ( InventorySlot slot in inventorySlots ) {
            if ( slot.getItemInSlot() == null )
                return slot;
        }
        return null;
    }

    public bool isInInventory(Draggable item) {
        foreach ( InventorySlot slot in inventorySlots ) {
            if ( slot.getItemInSlot() == item ) {
                return true;
            } 
        }

        return false;
    }
}