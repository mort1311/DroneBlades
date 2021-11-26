using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveEnergyBar : MonoBehaviour
{
    Image image;

    void Start(){
        image = GetComponent<Image>();
    }

    public void HandleOnPlayerMoveEnergyChanged(float newValue) {
        image.fillAmount=newValue;
    }
}
