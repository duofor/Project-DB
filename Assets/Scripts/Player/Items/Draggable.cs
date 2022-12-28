using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour {
    Util util = new Util();

    Vector3 initialPosition;
    Vector3 defaultPosition = new Vector3(0,0,0);
    Vector3 mousePosition = new Vector3(0,0,0);

    BoxCollider2D box;

    Vector3 initialBoxColliderSize;
    public ItemSlot currentItemSlot = null;


    void OnMouseDown() {
        Debug.Log("hey");
        initialPosition = transform.position;

        box = transform.GetComponent<BoxCollider2D>();
        initialBoxColliderSize = box.size;
        box.size = new Vector3(0,0,0);
    }

    void OnMouseUp() {
        /*  raycast to place where dropped.
        if inventory assign it
        if not inventory, put it back where it came from */

        int found = 0;
        RaycastHit2D hit = getTargetAtMouse();
        if ( hit && hit.transform.gameObject.layer != 5 ) {
            Debug.Log("tried to put " + transform.name + " at " + hit.transform.name);
        } else if (hit && hit.transform.tag == util.inventoryWeaponSlotTag ) { //set in weapon slot
            ItemSlot inventorySlot = hit.transform.GetComponent<WeaponSlot>();
            if ( inventorySlot.isEmpty() ) {
                inventorySlot.setItem(this);

                if ( currentItemSlot != null ) {
                    currentItemSlot.clearItem();
                } 

                currentItemSlot = inventorySlot;
                found = 1; 
            }
        } else if ( hit && hit.transform.tag == util.inventoryItemSlotTag ) { //set in inventory slot
            ItemSlot inventorySlot = hit.transform.GetComponent<InventorySlot>();
            if ( inventorySlot.isEmpty() ) {
                inventorySlot.setItem(this);

                if ( currentItemSlot != null ) {
                    currentItemSlot.clearItem();
                } 

                currentItemSlot = inventorySlot;
                found = 1; 
            }
        } else {
            if ( hit ) {
                Debug.Log("tried to put " + transform.name + " in " + hit.transform.name);
            }
        }

        if ( found == 0 ) {
            transform.position = initialPosition;
        }

        initialPosition = defaultPosition;
        box.size = initialBoxColliderSize;
    }

    void FixedUpdate() {
        if ( initialPosition != defaultPosition ) {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //get start pos when click
            transform.position = new Vector3( mousePosition.x, mousePosition.y, 0 );
        }
    }

    private RaycastHit2D getTargetAtMouse() {
        Vector2 origin = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f);
        return hit;
    }

    public virtual List<Skill> getWeaponSkills() {
        return null;
    }
}