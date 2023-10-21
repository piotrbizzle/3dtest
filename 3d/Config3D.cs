using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Config3D : MonoBehaviour
{    
    public Color lColor;
    public Color mColor;
    public Color rColor;

    [SerializeField]
    private GameObject LeftOptions;

    [SerializeField]
    private GameObject MiddleOptions;

    [SerializeField]
    private GameObject RightOptions;
    
    
    public void Start() {
	// initialize menu options from defaults

	// left
	foreach (Transform child in this.LeftOptions.transform) {
	    if (child.name == "SliderR") {
		child.GetComponent<Slider>().value = this.lColor.r;
	    } else if (child.name == "SliderG") {
		child.GetComponent<Slider>().value = this.lColor.g;
	    } else if (child.name == "SliderB") {
		child.GetComponent<Slider>().value = this.lColor.b;
	    }		
	}

	// middle
	foreach (Transform child in this.MiddleOptions.transform) {
	    if (child.name == "SliderR") {
		child.GetComponent<Slider>().value = this.mColor.r;
	    } else if (child.name == "SliderG") {
		child.GetComponent<Slider>().value = this.mColor.g;
	    } else if (child.name == "SliderB") {
		child.GetComponent<Slider>().value = this.mColor.b;
	    }		
	}
	
	// right
	foreach (Transform child in this.RightOptions.transform) {
	    if (child.name == "SliderR") {
		child.GetComponent<Slider>().value = this.rColor.r;
	    } else if (child.name == "SliderG") {
		child.GetComponent<Slider>().value = this.rColor.g;
	    } else if (child.name == "SliderB") {
		child.GetComponent<Slider>().value = this.rColor.b;
	    }		
	}
    }

    public void Update() {
	// assumes that all or none of options will be active
	if (!this.LeftOptions.activeInHierarchy) {
	    return;
	}

	// left
	foreach (Transform child in this.LeftOptions.transform) {
	    if (child.name == "SliderR") {
		this.lColor.r = child.GetComponent<Slider>().value;
	    } else if (child.name == "SliderG") {
		this.lColor.g = child.GetComponent<Slider>().value;
	    } else if (child.name == "SliderB") {
		this.lColor.b = child.GetComponent<Slider>().value;
	    }		
	}

	// middle
	foreach (Transform child in this.MiddleOptions.transform) {
	    if (child.name == "SliderR") {
		this.mColor.r = child.GetComponent<Slider>().value;
	    } else if (child.name == "SliderG") {
		this.mColor.g = child.GetComponent<Slider>().value;
	    } else if (child.name == "SliderB") {
		this.mColor.b = child.GetComponent<Slider>().value;
	    }		
	}
	
	// right
	foreach (Transform child in this.RightOptions.transform) {
	    if (child.name == "SliderR") {
		this.rColor.r = child.GetComponent<Slider>().value;
	    } else if (child.name == "SliderG") {
		this.rColor.g = child.GetComponent<Slider>().value;
	    } else if (child.name == "SliderB") {
		this.rColor.b = child.GetComponent<Slider>().value;
	    }		
	}

    }
}
