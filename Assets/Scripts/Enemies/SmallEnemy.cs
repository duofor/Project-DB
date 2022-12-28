using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmallEnemy : MonoBehaviour, IMonster  {
    Util util = new Util();

    int health = 5;

    private SpriteRenderer spriteRenderer;

    public static DamageManager instance;
    public TextMeshProUGUI damageText; // set in inspector for now


    void Awake() {
        damageText = GameObject.Find("DamageNumbersText").transform.GetComponent<TextMeshProUGUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if ( col.transform.tag == util.skillTag ) {
            StartCoroutine(showDamageNumbers());
            StartCoroutine(flash());
            
        }
    }

    IEnumerator showDamageNumbers() {
        Transform objTransform = transform;
        TextMeshProUGUI damageTextObj = Instantiate(damageText, objTransform.gameObject.transform.position, transform.rotation);
        
        damageTextObj.transform.SetParent(DamageManager.instance.transform);
        damageTextObj.transform.localScale = new Vector3 (1,1,0);
        damageTextObj.text = "1";

        Vector3 objSize = objTransform.GetComponent<SpriteRenderer>().bounds.size;

        float yDest = objTransform.gameObject.transform.position.y + objSize.y;
        Vector3 destPosition = new Vector3(damageTextObj.transform.position.x, yDest, 0);

        float timeElapsed = 0f;
        float duration = 1.5f;
        while ( timeElapsed < duration ) {
            damageTextObj.transform.position = Vector3.Lerp(
                damageTextObj.transform.position,
                destPosition,
                timeElapsed / duration
            );

            timeElapsed += Time.fixedDeltaTime; 
            if ( damageTextObj.transform.position == destPosition ) {
                Destroy(damageTextObj.gameObject);
                yield break;
            }
            yield return null;
        }
        Destroy(damageTextObj.gameObject);
        yield return null;

    }

    IEnumerator flash() {
        Color initialColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;

        float timeElapsed = 0f;
        float flashDuration = 0.3f;
        while ( timeElapsed < flashDuration ) {
            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }

        spriteRenderer.color = initialColor;
        yield return null;
    }


}
