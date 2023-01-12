using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sellable : MonoBehaviour {

    public int itemPrice = 153;
    private bool isNotPurchased = true;

    public TextMeshProUGUI coinPrice; // its the canvas coin text element.
    public GameObject coinPricePosition;
    private TextMeshProUGUI priceText_obj; // represents the spawned text obj in the scene

    public Sprite coinImage; // its the sprite of the coin
    public GameObject gameImagePosition;

    private GroundItem sellItem; // to be constructed dynamically

    Player player_ref; // needing this to check if can be purchased

    void Start() {
        player_ref = GameManager.instance.player;

        GameObject reward = RewardPicker.instance.getRandomRewardObj();
        sellItem = reward.GetComponent<GroundItem>();
        sellItem.canPickup = false; // Disabling auto-pickup. Will be available only once the price is paid. 
        sellItem.transform.name = "Shop item";
        sellItem.transform.SetParent(transform);
        sellItem.transform.localPosition = Vector3.zero;
        
        coinPrice.text = itemPrice.ToString();
        priceText_obj = Instantiate(
            coinPrice,
            coinPricePosition.transform.position,
            transform.rotation,
            coinPrice.transform //or we cant see it
        );
        priceText_obj.enabled = true; 

        SpriteRenderer coinRenderer = gameImagePosition.AddComponent<SpriteRenderer>();
        coinRenderer.sprite = coinImage;
        coinRenderer.sortingOrder = 1;
    }

    void Update() {
        if ( priceText_obj ) priceText_obj.transform.position = coinPricePosition.transform.position;

        if ( sellItem.isFocused && Input.GetKeyDown(KeyCode.E) && isNotPurchased) {
            int playerCoins = player_ref.coins.getValue();            
            if ( playerCoins >= itemPrice ) {
                //buying the item
                isNotPurchased = false;
                player_ref.coins.removeFromValue(itemPrice);

                sellItem.canPickup = true; // maybe do an autopickup function instead of this
                player_ref.currentTouchedItem = sellItem;


                Destroy(priceText_obj.gameObject);
                Destroy(gameImagePosition.gameObject);
            } else {
                Debug.Log("you dont have enough moeny bitch");
            }
        }
    }


}
