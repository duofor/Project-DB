using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    public Attribute[] attributes;
    
    //my shit
    public PlayerSkillInterface skillInterface;
    private Stats stats;
    public GameObject weaponPoint;
    private GameObject myCurrentWeapon;


    void Awake() {
        stats = GetComponent<Stats>();
    }

    private void Start() {
        for (int i = 0; i < attributes.Length; i++) {
            attributes[i].SetParent(this);
        }
        for (int i = 0; i < equipment.GetSlots.Length; i++) {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                //remove the buffs
                for (int i = 0; i < _slot.item.buffs.Length; i++) {
                    for (int j = 0; j < attributes.Length; j++) {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }

                //remove the skills
                for (int i = 0; i < _slot.AllowedItems.Length; i++) {
                    ItemType itemType = _slot.AllowedItems[i];
                    if ( itemType == ItemType.Weapon && skillInterface) {
                        skillInterface.clearPlayerSkills();
                        break;
                    }
                }

                //remove the weapon if it exists
                for (int i = 0; i < _slot.AllowedItems.Length; i++) {
                    ItemType itemType = _slot.AllowedItems[i];
                    if ( (itemType == ItemType.Weapon) && (weaponPoint != null) ) {
                        if ( myCurrentWeapon != null ) {
                            Destroy(myCurrentWeapon.gameObject);
                        }
                        break;
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));
                
                //add the buffs
                for (int i = 0; i < _slot.item.buffs.Length; i++) {
                    for (int j = 0; j < attributes.Length; j++) {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }

                //add the skills
                for (int i = 0; i < _slot.AllowedItems.Length; i++) {
                    ItemType itemType = _slot.AllowedItems[i];
                    if ( itemType == ItemType.Weapon && skillInterface ) {
                        skillInterface.addSkills(_slot.item.skills);
                        break;
                    }
                }

                //set the weapon
                // for (int i = 0; i < _slot.AllowedItems.Length; i++) {
                //     ItemType itemType = _slot.AllowedItems[i];
                //     if ( (itemType == ItemType.Weapon) && weaponPoint && _slot.item.prefab != null) {
                //         to be done later perhaps #TODO
                //         GameObject playerWeapon = Instantiate( _slot.item.prefab );
                //         playerWeapon.transform.position = weaponPoint.transform.position;
                //         playerWeapon.transform.SetParent(transform);
                //         myCurrentWeapon = playerWeapon; //storing the wep ref so we can destroy once we unnequip it
                //         break;
                //     }
                // }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem)
        {
            Item _item = new Item(groundItem.item);
            if (inventory.AddItem(_item, 1)) 
            {
                Destroy(other.gameObject);
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            inventory.Save();
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
            equipment.Load();
        }
    }
    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }
    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public Player parent;
    public Attributes type;
    public ModifiableInt value;
    
    public void SetParent(Player _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}