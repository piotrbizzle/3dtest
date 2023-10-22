using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite3D : MonoBehaviour
{

    public bool isStatic;
    public bool isScaling;
    public int depth;
    
    public bool isDepthDirty; // does depth need an update

    private Config3D config;
    
    private GameObject lGo;
    private GameObject rGo;

    private Sprite[] lSprites = new Sprite[5];
    private Sprite[] mSprites = new Sprite[5];
    private Sprite[] rSprites = new Sprite[5];
    public Sprite dialogueSprite;
    public Sprite spriteReal;
    
    void Start() {
	// get color config
	this.config = GameObject.Find("/Config3D").GetComponent<Config3D>();

	// set dialogue sprite from default sprite
	this.dialogueSprite = this.GetComponent<SpriteRenderer>().sprite;
	this.spriteReal = Resources.Load<Sprite>(this.name + "_real");
	
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
	this.isDepthDirty = true;
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
	if (!this.isDepthDirty) {
	    return;
	}
	this.isDepthDirty = false;

	// update sprites for new depth
        this.lGo.GetComponent<SpriteRenderer>().sprite = this.lSprites[this.depth];
	this.lGo.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("D" + this.depth.ToString());
	
	this.GetComponent<SpriteRenderer>().sprite = this.mSprites[this.depth];
	this.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("D" + this.depth.ToString());
	
	this.rGo.GetComponent<SpriteRenderer>().sprite = this.rSprites[this.depth];
	this.rGo.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("D" + this.depth.ToString());

	// scale if needed
	if (this.isScaling) {
	    this.ScaleSprite();
	}	
    }

    public void ResetScaleSprite() {
	this.transform.localScale = new Vector3(1f, 1f, 1f);
	this.rGo.transform.localScale = new Vector3(1f, 1f, 1f);
	this.lGo.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    
    private void ScaleSprite() {
	// skip scaling for held items
	if (this.transform.GetComponent<PickUpable>() != null && this.transform.parent.GetComponent<Player>() != null) {
	    return;
	}
	float scaleFactor = this.GetScaleFactor();
	this.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
	this.rGo.transform.localScale = new Vector3(1f, 1f, 1f);
	this.lGo.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public float GetScaleFactor() {
	return 1f - 0.08f * this.depth;
    }

    public void SetDepth(int depth) {
	if (this.isStatic) {
	    return;
	}
	this.depth = depth;
	this.isDepthDirty = true;
    }

    public int GetDepth() {
	return this.depth;
    }
}
