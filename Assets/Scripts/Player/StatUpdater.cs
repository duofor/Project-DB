using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUpdater : MonoBehaviour{
    
    public Stats stats_ref;

    public TextMeshProUGUI STR_Value;
    public TextMeshProUGUI DEX_Value;
    public TextMeshProUGUI INT_Value;

    void Start() {
        updateUIStats();
    }

    void Update() {
        updateUIStats();
    }

    void updateUIStats() {
        if ( stats_ref == null) return;
        
        STR_Value.text = stats_ref.STR.ToString();
        DEX_Value.text = stats_ref.DEX.ToString();
        INT_Value.text = stats_ref.INT.ToString();
    }

}
