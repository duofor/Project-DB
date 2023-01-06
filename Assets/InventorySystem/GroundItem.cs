using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;

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
    }
    void OnTriggerExit2D(Collider2D other) {
        if ( other.GetComponent<Player>() == null ) return;
        ItemToolTip toolTip = ItemToolTip.instance;
        toolTip.disable();
    }
}
