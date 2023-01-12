using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collectable : MonoBehaviour {

    public int amount = 1;
    public TextMeshProUGUI collectText;
    Camera mainCamera;

    void Start() {
        mainCamera = Camera.main;
    }

    public virtual void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.transform.name);
        Player player = other.GetComponent<Player>();
        if ( player ) {
            player.coins.addToValue(amount);
            StartCoroutine(onCollect());
        }
    }

    public virtual IEnumerator onCollect() {
        if (collectText == null) {
            Destroy(gameObject);
            yield break;  
        }  

        collectText.text = amount.ToString();
        Vector3 size = GetComponent<SpriteRenderer>().sprite.bounds.size;
        
        Vector3 initialP = transform.position;
        initialP.z = 0;

        TextMeshProUGUI textObj = Instantiate(
            collectText,
            initialP,
            transform.rotation,
            collectText.transform
        );
        
        textObj.enabled = true;
        textObj.text += "G";
        // textObj.transform.position = new Vector3(0,0,0) + size;

        Vector3 destination = textObj.transform.position + size * 3;

        float timeElapsed = 0f;
        float duration = 0.3f;
        GetComponent<SpriteRenderer>().enabled = false;
        while ( timeElapsed < duration ) {
            // for lerp, we gotta make an animation. Otherwise we only end up messing up with camera position math. fuck that
            textObj.transform.position = initialP;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        DestroyImmediate(textObj.gameObject);
        Destroy(gameObject);
    }
}
