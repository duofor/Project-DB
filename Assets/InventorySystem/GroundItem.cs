using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;

    public bool isFocused = false; // is tooltip enabled?
    public bool canPickup = true; // some items we do not want to pickup by default. ex: shop items with a price


    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
#endif
    }

    void OnTriggerEnter2D(Collider2D other) {
        if ( other.GetComponent<Player>() == null ) return;
        ItemToolTip toolTip = ItemToolTip.instance;

        toolTip.setItemObject(item);
        toolTip.setPosition(transform.position);
        toolTip.enable();

        isFocused = true;
    }
    void OnTriggerExit2D(Collider2D other) {
        if ( other.GetComponent<Player>() == null ) return;
        ItemToolTip toolTip = ItemToolTip.instance;
        toolTip.disable();
        isFocused = false;
    }
}
