using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Monster : MonoBehaviour {
    Util util = new Util();

    public int health = 0;

    [SerializeField] private Projectile projectile;
    [SerializeField] private GameObject shootingPoint;
    private SpriteRenderer spriteRenderer;
    Color initialColor;

    private TextMeshProUGUI damageText;
    List<TextMeshProUGUI> damageTexts = new List<TextMeshProUGUI>();

    void Awake() {
        damageText = GameObject.Find("DamageNumbersText").transform.GetComponent<TextMeshProUGUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;

        //start invoking projectiles.
        InvokeRepeating("fireAt", 0f, 0.5f);
    }
    
    void Update() {
        if ( health <= 0 ) {
            foreach( TextMeshProUGUI text in damageTexts ) {
                if (text != null ) {
                    Destroy(text);
                }
            }
            Destroy(gameObject);
        }
    }

    public virtual IEnumerator showDamageNumbers() {
        Transform objTransform = transform;
        TextMeshProUGUI damageTextObj = Instantiate(damageText, objTransform.gameObject.transform.position, transform.rotation);
        damageTexts.Add(damageTextObj); // so we can destroy them once this gets destroyed

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
                break;
            }
            yield return null;
        }
        Destroy(damageTextObj.gameObject);
    }

    public virtual IEnumerator flash() {
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

    public virtual void fireAt() {
        if ( projectile == null ) return;
        
        Vector3 targetPos = GameObject.Find( "Player" ).transform.position; // trash code to be changed later
        Vector3 direction = targetPos - shootingPoint.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        Quaternion rota = Quaternion.AngleAxis(angle, Vector3.forward); 

        Projectile go = Instantiate( projectile, shootingPoint.transform.position, rota );
        Destroy(go.gameObject, 1);

    }

}
