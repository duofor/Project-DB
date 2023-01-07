using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour {

    public static ItemToolTip instance;

    ItemObject item;

    public Image image;

    public Image _itemSprite;
    public TextMeshProUGUI _itemName;
    public TextMeshProUGUI _itemType;

    List<TextMeshProUGUI> attrList = new List<TextMeshProUGUI>();
    public TextMeshProUGUI _attribute_1;
    public TextMeshProUGUI _attribute_2;
    public TextMeshProUGUI _attribute_3;

    List<TextMeshProUGUI> attrValueList = new List<TextMeshProUGUI>();
    public TextMeshProUGUI _attribute_value_1;
    public TextMeshProUGUI _attribute_value_2;
    public TextMeshProUGUI _attribute_value_3;

    public TextMeshProUGUI _description;

    void Awake() {
        if ( instance == null ) {
            instance = this;
        }
    }
    void Start() {

        //add to list so we can dinamically set them
        attrList.Add(_attribute_1); attrList.Add(_attribute_2); attrList.Add(_attribute_3);
        attrValueList.Add(_attribute_value_1); 
        attrValueList.Add(_attribute_value_2); 
        attrValueList.Add(_attribute_value_3);
    }

    public void setPosition(Vector3 position) {
        transform.position = position;
    }
    public void enable() {
        image.transform.position = transform.position;

        _itemName.text = item.data.Name;
        _itemType.text = item.type.ToString();

        _attribute_1.text = "STR";
        _attribute_2.text = "DEX";
        _attribute_3.text = "INT";

        _attribute_value_1.text = item.data._STR.ToString();
        _attribute_value_2.text = item.data._DEX.ToString();
        _attribute_value_3.text = item.data._INT.ToString();
        
        _description.text = item.description;
        _itemSprite.sprite = item.uiDisplay;

    }
    public void disable() {
        image.transform.position = new Vector3(5555,5555,0);
    }

    public void setItemObject(ItemObject io) {
        item = io;
    }

}
