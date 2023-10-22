using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite3D : MonoBehaviour
{

    public bool isStatic;
    public int depth;
    
    private int nextDepth;

    private Config3D config;
    
    private GameObject lGo;
    private GameObject rGo;

    private Sprite[] lSprites = new Sprite[5];
    private Sprite[] mSprites = new Sprite[5];
    private Sprite[] rSprites = new Sprite[5];
    
    void Start() {
	// get color config
	this.config = GameObject.Find("/Config3D").GetComponent<Config3D>();
	
	// load all required sprites
	if (this.isStatic) {
	    this.lSprites[this.depth] = Resources.Load<Sprite>(this.name + "_" + this.depth.ToString() + "l");
	    this.mSprites[this.depth] = Resources.Load<Sprite>(this.name + "_" + this.depth.ToString() + "m");
	    this.rSprites[this.depth] = Resources.Load<Sprite>(this.name + "_" + this.depth.ToString() + "r");
	} else {
	    for (int i = 0; i < 5; i++) {
		this.lSprites[i] = Resources.Load<Sprite>(this.name + "_" + i.ToString() + "l");
		this.mSprites[i] = Resources.Load<Sprite>(this.name + "_" + i.ToString() + "m");
		this.rSprites[i] = Resources.Load<Sprite>(this.name + "_" + i.ToString() + "r");		
	    }
	}
	
	// create left and right view and colorize
	this.lGo = new GameObject();
	this.lGo.transform.position = this.transform.position;
	this.lGo.transform.SetParent(this.transform);
	this.lGo.AddComponent<SpriteRenderer>().color = this.config.lColor;

	this.GetComponent<SpriteRenderer>().color = this.config.mColor;
	
	this.rGo = new GameObject();
	this.rGo.transform.position = this.transform.position;
	this.rGo.transform.SetParent(this.transform);
	this.rGo.AddComponent<SpriteRenderer>().color = this.config.rColor;

	// init nextDepth
	this.nextDepth = this.depth;
	this.depth = -1;
    }

    void Update() {
	// adjust colors
	if (this.lGo.GetComponent<SpriteRenderer>().color != this.config.lColor) {
	    this.lGo.GetComponent<SpriteRenderer>().color = this.config.lColor;
	}
	if (this.GetComponent<SpriteRenderer>().color != this.config.mColor) {
	    this.GetComponent<SpriteRenderer>().color = this.config.mColor;
	}
	if (this.rGo.GetComponent<SpriteRenderer>().color != this.config.rColor) {
	    this.rGo.GetComponent<SpriteRenderer>().color = this.config.rColor;
	}
	
	// return early if no change to depth
	if (this.nextDepth == this.depth) {
	    return;
	}

	// update sprites for new depth
	this.depth = this.nextDepth;
        this.lGo.GetComponent<SpriteRenderer>().sprite = this.lSprites[this.depth];
	this.lGo.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("D" + this.depth.ToString());
	
	this.GetComponent<SpriteRenderer>().sprite = this.mSprites[this.depth];
	this.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("D" + this.depth.ToString());
	
	this.rGo.GetComponent<SpriteRenderer>().sprite = this.rSprites[this.depth];
	this.rGo.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("D" + this.depth.ToString());

    }

    public void SetDepth(int depth) {
	if (this.isStatic) {
	    return;
	}
	this.nextDepth = depth;
    }

    public int GetDepth() {
	return this.depth;
    }
}