using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button3DOptions : MonoBehaviour, IPointerClickHandler {
    [SerializeField]
    private GameObject Options3D;

    [SerializeField]
    private GameObject MenuBackground;

    [SerializeField]
    private Sprite DismissSprite;
    private Sprite ActivateSprite;
    
    public void Start() {
	this.MenuBackground.SetActive(false); 
	this.Options3D.SetActive(false);
	this.ActivateSprite = this.GetComponent<Image>().sprite;
    }
    
    public void OnPointerClick(PointerEventData pointerEventData) {
	if (!this.Options3D.activeInHierarchy) {
	    this.MenuBackground.SetActive(true);
	    this.Options3D.SetActive(true);
	    this.GetComponent<Image>().sprite = this.DismissSprite;
	} else {
	    this.MenuBackground.SetActive(false);
	    this.Options3D.SetActive(false);
	    this.GetComponent<Image>().sprite = this.ActivateSprite;
	}
    }
}
