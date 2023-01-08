using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

public class Player : MonoBehaviour {

    //scriptable objects
    public InventoryObject inventory;
    public InventoryObject equipment;
    
    //my shit
    public Stats stats = new Stats();
    public PlayerSkillInterface skillInterface;
    
    public StatUpdater statUpdater;

    public GameObject weaponPoint;
    private GameObject myCurrentWeapon;

    bool canPickup = false;
    GroundItem currentTouchedItem;

    void OnDestroy() {
        for (int i = 0; i < equipment.GetSlots.Length; i++) {
            equipment.GetSlots[i].OnBeforeUpdate -= OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate -= OnAfterSlotUpdate;
        }
    }

    private void Start() {
        for (int i = 0; i < equipment.GetSlots.Length; i++) {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }

        canPickup = false;
        statUpdater.stats_ref = stats;
    }

    void Update() {
        if ( canPickup ) {
            if ( Input.GetKeyDown(KeyCode.E) && currentTouchedItem != null ) {
                canPickup = false;
                Item _item = new Item(currentTouchedItem.item);
                if (inventory.AddItem(_item, 1)) {
                    Destroy(currentTouchedItem.gameObject);
                }
            }
        }
    }

    public void OnBeforeSlotUpdate(InventorySlot _slot) {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                //remove the buffs
                if ( _slot.item.stats != null ) {
                    Debug.Log("removing stats ");
                    stats.STR -= _slot.item.stats.STR;
                    stats.DEX -= _slot.item.stats.DEX;
                    stats.INT -= _slot.item.stats.INT;
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
                if ( _slot.item.stats != null ) {
                    stats.STR += _slot.item.stats.STR;
                    stats.DEX += _slot.item.stats.DEX;
                    stats.INT += _slot.item.stats.INT;
                    Debug.Log("adding " + _slot.item.stats.STR + " to stats STR: " + stats.STR);
                    Debug.Log("adding " + _slot.item.stats.DEX + " to stats STR: " + stats.DEX);
                }

                //add the skills
                for (int i = 0; i < _slot.AllowedItems.Length; i++) {
                    ItemType itemType = _slot.AllowedItems[i];
                    if ( itemType == ItemType.Weapon && skillInterface ) { // we can change later to more skills if we want to
                        string weaponName = _slot.item.Name;
                        Skill skill = Resources.Load<Skill>($"Prefabs/Weapons/{weaponName}/Skills/Skill_1");
                        List<Skill> sk = new List<Skill>();
                        sk.Add(skill);
                        skillInterface.addSkills(sk);
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
    
    void OnTriggerEnter2D(Collider2D other) {
        GroundItem groundItem = other.GetComponent<GroundItem>();
        if (groundItem) {
            canPickup = true;
            currentTouchedItem = groundItem;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        GroundItem groundItem = other.GetComponent<GroundItem>();
        if (groundItem) {
            canPickup = false;
            currentTouchedItem = null;
        }
    }


    public void AttributeModified(Attribute attribute) {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }
    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }

    public void savePlayerData() {
        inventory.Save();
        equipment.Save();
    }
    public void loadPlayerData() {
        inventory.Load();
        equipment.Load();
    }
}


[System.Serializable]
public class Attribute {
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