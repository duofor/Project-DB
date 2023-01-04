using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Monster : MonoBehaviour {
    Util util = new Util();

    public bool readyForDestroy = false; 

    public float health = 0;

    public Projectile projectile;

    [SerializeField] private GameObject shootingPoint;
    private SpriteRenderer spriteRenderer;
    Color initialColor;

    private TextMeshProUGUI damageText;
    List<TextMeshProUGUI> damageTexts = new List<TextMeshProUGUI>();
    
    private Material material;

    void Awake() {
        damageText = GameObject.Find("DamageNumbersText").transform.GetComponent<TextMeshProUGUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    
        //defaulting this
        material = spriteRenderer.material;
        material.SetFloat("_Fade", 1f);
        material.SetFloat("Scale", 200f);
    }

    void Start() {
        //start invoking projectiles.
        InvokeRepeating("fireAt", 0f, 0.5f);
    }
    
    void Update() {
        if (readyForDestroy) return;

        if ( health <= 0 ) {
            foreach( TextMeshProUGUI text in damageTexts ) {
                if (text != null ) {
                    Destroy(text);
                }
            }
            StartCoroutine(dissolve()); // this also destroyes the obj
        }
    }

    public virtual IEnumerator showDamageNumbers() {
        if (readyForDestroy) yield break;

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
        if (readyForDestroy) yield break;

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
        if ( projectile == null || readyForDestroy) return;
        
        GameObject p = GameObject.Find( "Player" );
        Vector3 targetPos = p.transform.position; // trash code to be changed later
        Vector3 direction = targetPos - shootingPoint.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        Quaternion rota = Quaternion.AngleAxis(angle, Vector3.forward); 

        if ( p != null ) {
            Projectile go = Instantiate( projectile, shootingPoint.transform.position, rota );
            Destroy(go.gameObject, 1);
        }
    }

    public IEnumerator doSomeSmallShake() {
        if (readyForDestroy) yield break;

        float timePassed = 0;
        bool flip = false;
        while (timePassed < 0.2f) {
            // Shake
            if (flip) {
                flip = !flip; 
                transform.position += new Vector3(0, transform.position.y / 80, 0);
                transform.position += new Vector3(transform.position.x / 80, 0, 0);
            } else {
                flip = !flip; 
                transform.position -= new Vector3(0, transform.position.y / 80, 0);
                transform.position -= new Vector3(transform.position.x / 80, 0, 0);
            }
            timePassed += Time.deltaTime;
            yield return null;
        }
    }

    public void takeDamage(float amount) {
        StartCoroutine(showDamageNumbers());
        StartCoroutine(doSomeSmallShake());
        StartCoroutine(flash());
        health -= amount;
    }

    IEnumerator dissolve() {
        float fade = 1f;
        readyForDestroy = true;

        while ( fade >= 0f ) {
            material.SetFloat("_Fade", fade);
            fade -= Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}
