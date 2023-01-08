using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpFillSetter : MonoBehaviour {

    public float health;
    public float maxHealth;

    public Image image;

    private void Update() {
        if ( maxHealth > 0 ) {
            image.enabled = true;
        } else {
            image.enabled = false;
        }

        image.fillAmount = Mathf.Clamp01( health / maxHealth );
    }
}
