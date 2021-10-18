using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image image;

    void Start(){
        image = GetComponent<Image>();
    }

    public void HandleOnPlayerHealthChanged(float newValue) {
        image.fillAmount=newValue;
    }
}
