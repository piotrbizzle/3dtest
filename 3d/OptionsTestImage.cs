using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsTestImage : MonoBehaviour
{
    [SerializeField]
    private Slider RedSlider;

    [SerializeField]
    private Slider GreenSlider;

    [SerializeField]
    private Slider BlueSlider;

    
    public void Update() {
	this.GetComponent<Image>().color = new Vector4(this.RedSlider.value, this.GreenSlider.value, this.BlueSlider.value, 1f);
    }
}
