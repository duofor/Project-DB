using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSlot : MonoBehaviour { 
    
    public Draggable item; //just so we can assign it in inspector 

    public virtual void setItem(Draggable i) {
        disableBoxCollider();
        i.transform.position = transform.position;
        i.transform.SetParent(transform, true);
        item = i;
    }

    public virtual void clearItem() {
        // item.transform.SetParent(null); // this fixes the scaling problem when setting player weapon.
        item = null;
        enableBoxCollider();
    }

    public void disableBoxCollider() {
        GetComponent<BoxCollider2D>().enabled = false;
    }
    public void enableBoxCollider() {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public bool isEmpty() {
        return item == null ? true : false; 
    }
    public Draggable getItemInSlot() {
        return item;
    }
}