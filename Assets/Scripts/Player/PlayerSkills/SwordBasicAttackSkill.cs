using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBasicAttackSkill : Skill  {
    Util util = new Util();

    [SerializeField] private Sprite testAttackSprite;

    void Start() {
        skillDamage = 1;
        skillManaCost = 1;
    }
    
    public override IEnumerator startAttackAnimation() {
        GameObject go = Instantiate(new GameObject(), transform.position, transform.rotation);

        SpriteRenderer spr = go.AddComponent<SpriteRenderer>();
        spr.sprite = testAttackSprite; 
        spr.sortingOrder = 4;

        go.transform.position = transform.position / 4; //dunno why this works, might be the hierarchy depth 4

        float lerpDuration = 3.0f;
        float timeElapsed = 0f;

        Vector3 targetPosition = Input.mousePosition;

        while ( timeElapsed < lerpDuration ) {
            go.transform.position = Vector3.Lerp(go.transform.position, targetPosition, timeElapsed / lerpDuration );
            timeElapsed += Time.deltaTime;
            
            if ( go.transform.position == targetPosition ) {
                Destroy(go.gameObject);
                // fireAtTarget( target );
                yield break;
            }
            
            yield return null;
        }
    }


    
}